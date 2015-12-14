using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using log4net;
using blahblah.Utilities;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace blahblah
{
    public class Program
    {
        static ILog logger;

        //configuration variables
        static Boolean loadTree = false, loadRule = true, validateSchema = true;
        static String logFolder = @"C:\FFXLogs\SchemaGenerator\", logPrefix;
        static String xmlFilesFolder = @"\\10.101.17.16\c$\Users\fizerc\Desktop\oldstuff\ConvertedRegs\Titles\Title_1\1-1h\convertedfiles\eReg\1";
        //static String xmlFilesFolder = @"\\10.101.17.16\c$\Users\fizerc\Desktop\oldstuff\ConvertedRegs\Titles";

        //code used variables
        static NodeClass resultTree;
        static List<NodeClass> resultRule;
        static ILog log;
        static String currentFileName;

        #region "get unique elements"

        static List<String> GetAllUniqueElementsFromXmlDocument(XmlDocument document)
        {
            List<String> elements = new List<string>();
            XmlNode root = document.DocumentElement as XmlNode;
            Recurse(root, ref elements);
            return elements;
        }

        static void Recurse(XmlNode node, ref List<String> uniqueElements)
        {
            String currentNodeName = node.Name;
            if (!uniqueElements.Contains(currentNodeName))
            {
                uniqueElements.Add(currentNodeName);
            }
            foreach (XmlNode child in node.ChildNodes)
            {
                Recurse(child, ref uniqueElements);
            }
        }

        static List<String> GetAllUniqueElements(String path = @"\\10.101.17.16\c$\Users\fizerc\Desktop\oldstuff\ConvertedRegs\Titles")
        {
            String[] files = Directory.GetFiles(path, "*.dita", SearchOption.AllDirectories);
            List<String> uniqueElements = new List<string>();
            int finished = 0;
            foreach (String file in files)
            {
                if (finished % 100 == 0)
                    Console.Title = String.Format("Processed {0:F2}% of files...", ((decimal)finished * 100 / (decimal)files.Length));
                XmlDocument doc = new XmlDocument();
                doc.Load(File.OpenRead(file));
                List<String> docUniqueElements = GetAllUniqueElementsFromXmlDocument(doc);
                uniqueElements = uniqueElements.Union(docUniqueElements).ToList();
                finished++;
            }
            return uniqueElements;
        }

        #endregion

        static void Main(string[] args)
        {
            //DateTime start = DateTime.Now;

            //List<String> uniqueElements = GetAllUniqueElements();
            //Console.WriteLine("Total unique elmenets = " + uniqueElements.Count);
            //foreach (String el in uniqueElements)
            //{
            //    Console.WriteLine(el);
            //}

            //DateTime end = DateTime.Now;
            //Console.WriteLine("total time: " + (end - start).Seconds);

            //Environment.Exit(0);

            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);
            logPrefix = String.Format("{0}T{1}-", DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("hhmmss"));
            GlobalContext.Properties["LogName"] = logPrefix + "Log.txt";
            logger = LogManager.GetLogger(typeof(Program));

            Console.WriteLine("Extracting files from the directory...");
            String[] files = Directory.GetFiles(xmlFilesFolder, "*.dita", SearchOption.AllDirectories);
            if (files.Length == 0)
            {
                Console.WriteLine("Cannot find .dita files in the directory.");
                Environment.Exit(0);
            }
            Console.WriteLine(String.Format("Found {0} files.\n\nLoading data...", files.Length));

            int finished = 0;
            int failed = 0;
            resultTree = new NodeClass();
            resultRule = new List<NodeClass>();

            foreach (String file in files)
            {
                currentFileName = Path.GetFileName(file);
                if (finished % 100 == 0)
                    Console.Title = String.Format("Loaded {0:F2}% of files...", ((decimal)finished * 100 / (decimal)files.Length));

                //generate schema from xml files
                if (!validateSchema)
                {
                    using (Stream xmlStream = File.OpenRead(file))
                    {
                        if (xmlStream.Length == 0)
                        {
                            WriteToLog("Cannot read file: " + currentFileName, null, "ERROR");
                            failed++;
                            continue;
                        }

                        XmlDocument xml = new XmlDocument();
                        xml.Load(xmlStream);

                        LoadEls(xml.DocumentElement);
                    }
                }
                else
                //validate shcema against xml files
                {
                    XmlSerializer ser = new XmlSerializer(typeof(topic));
                    topic topic;
                    XmlReader reader = null;
                    try
                    {
                        reader = XmlReader.Create(file);
                        topic = (topic)ser.Deserialize(reader);
                    }
                    catch (InvalidOperationException ex)
                    {
                        //deserialization failed
                        failed++;
                        WriteToLog(file, null, "warn");
                    }
                    catch (Exception ex)
                    {
                        //other failure
                        failed++;
                        WriteToLog(file, null, "error");
                    }
                    finally
                    {
                        if (!Object.ReferenceEquals(reader, null))
                            reader.Close();
                    }
                }

                finished++;
            }
            Console.Title = String.Format("Loaded 100% of files.");
            Console.WriteLine(String.Format("Data loaded. {0} of files failed.\n", failed));
            if (failed > 0)
                WriteToLog(String.Format("Data loaded complete. {0} of files failed.\n", failed), null, "INFO");

            //check min/maxOccurs
            if (!validateSchema)
            {
                Console.WriteLine(String.Format("\nProcessing all {0} files.", files.Length));
                finished = 0;
                failed = 0;

                foreach (String file in files)
                {
                    currentFileName = Path.GetFileName(file);
                    if (finished % 100 == 0)
                        Console.Title = String.Format("Processed {0:F2}% of files...", ((decimal)finished * 100 / (decimal)files.Length));

                    using (Stream xmlStream = File.OpenRead(file))
                    {
                        if (xmlStream.Length == 0)
                        {
                            WriteToLog("Cannot read file: " + currentFileName, null, "ERROR");
                            failed++;
                            continue;
                        }

                        XmlDocument xml = new XmlDocument();
                        xml.Load(xmlStream);

                        ProcessLoadedData(xml.DocumentElement);
                    }
                    finished++;
                }
                Console.Title = "Processed 100% of files.";
                Console.WriteLine(String.Format("Data processed. {0} of files failed.\n", failed));
                if (failed > 0)
                    WriteToLog(String.Format("Data processed. {0} of files failed.\n", failed), null, "ERROR");
            }

            //print results
            Console.WriteLine("Printing Tree/Rule/Logs to files at:\n" + logFolder);
            if (!validateSchema)
                PrintResults();

            //clean empty log
            String logPath = Path.Combine(logFolder, logPrefix + "Log.txt");
            if (File.Exists(logPath))
            {
                Stream reader = File.OpenRead(logPath);
                if (reader.Length == 0)
                    File.Delete(logPath);
            }

            Console.WriteLine("Printed.\n");
        }

        static void LoadEls(XmlElement root)
        {
            //load tree
            if (loadTree)
            {
                resultTree.name = root.LocalName;
                LoadChildTree(root, resultTree);
            }
            //load rule
            if (loadRule)
            {
                LoadChildRule(root, null);
            }

            #region "using tasks"
            //Task treeTask = null, ruleTask = null;

            //if (loadTree)
            //{
            //    resultTree.name = root.Name;
            //    treeTask = new Task(() => LoadChildTree(root, resultTree));
            //}

            //if (loadRule)
            //{
            //    ruleTask = new Task(() => LoadChildRule(root, null));
            //}

            //if (!Object.ReferenceEquals(treeTask, null))
            //    treeTask.Start();
            //if (!Object.ReferenceEquals(ruleTask, null))
            //    ruleTask.Start();

            //if (!Object.ReferenceEquals(treeTask, null))
            //    treeTask.Wait();
            //if (!Object.ReferenceEquals(ruleTask, null))
            //    ruleTask.Wait();
            #endregion "using tasks"
        }

        static void LoadChildTree(XmlNode parentEl, NodeClass parentTree)
        {
            foreach (XmlNode currentEl in parentEl)
            {
                //jump <table>
                if (currentEl.LocalName.Equals("table"))
                    continue;

                NodeAppearPair currentNodeTreePair = parentTree.GetChild(currentEl.LocalName);
                if (Object.ReferenceEquals(currentNodeTreePair, null))
                {
                    //new tree
                    NodeClass currentNodeTree = new NodeClass(currentEl.LocalName);
                    parentTree.childNodes.Add(new NodeAppearPair(ref currentNodeTree));
                }
                else
                    //old tree
                    currentNodeTreePair.totalAppearCount++;

                //check child
                if (currentEl.HasChildNodes)
                    LoadChildTree(currentEl, currentNodeTreePair.nodeClass);
            }
        }

        static void LoadChildRule(XmlNode currentEl, NodeClass parentRule)
        {
            //jump <table>
            if (currentEl.LocalName.Equals("table"))
                return;

            //check new rule
            NodeClass currentRule = null;
            foreach (NodeClass rule in resultRule)
                if (rule.name.Equals(currentEl.LocalName))
                {
                    currentRule = rule;
                    currentRule.appearCount++;
                    break;
                }
            if (Object.ReferenceEquals(currentRule, null))
            {
                currentRule = new NodeClass(currentEl.LocalName);
                if (currentEl.LocalName.Equals("#text"))
                {
                    parentRule.containText = true;
                    return;
                }
                if (currentEl.LocalName.Equals("#comment"))
                    return;
                resultRule.Add(currentRule);
            }

            //add attributes
            if (!Object.ReferenceEquals(currentEl.Attributes, null) && currentEl.Attributes.Count > 0)
                foreach (XmlAttribute xmlAttr in currentEl.Attributes)
                {
                    AttributeClass currentAttr = null;

                    foreach (AttributeClass attr in currentRule.attributes)
                        if (attr.name.Equals(xmlAttr.LocalName))
                        {
                            //old attribute
                            currentAttr = attr;
                            currentAttr.appearCount++;
                            break;
                        }
                    //new attribute
                    if (Object.ReferenceEquals(currentAttr, null))
                    {
                        currentAttr = new AttributeClass(xmlAttr.LocalName, xmlAttr.Value);
                        currentRule.attributes.Add(currentAttr);
                    }

                    //check prefix
                    if (!String.IsNullOrEmpty(xmlAttr.Prefix))
                        currentAttr.prefix = xmlAttr.Prefix;
                }

            //check new child of its parent
            if (!Object.ReferenceEquals(parentRule, null))
                if (!parentRule.HasChild(currentEl.LocalName) && !currentRule.name.Equals("#text") && !currentRule.name.Equals("#comment"))
                    parentRule.childNodes.Add(new NodeAppearPair(ref currentRule));

            //check child
            if (currentEl.HasChildNodes)
                foreach (XmlNode childEl in currentEl)
                    LoadChildRule(childEl, currentRule);
        }

        static void PrintResults()
        {
            //print tree
            if (loadTree)
            {
                String treeLogPath = Path.Combine(logFolder, logPrefix + "Tree.txt");
                TextWriter writer = new StreamWriter(treeLogPath, true);
                PrintTree(resultTree, writer);
            }
            //print rule
            if (loadRule)
            {
                //String ruleLogPath = Path.Combine(logFolder, logPrefix + "Rule.txt");
                //TextWriter writer = new StreamWriter(ruleLogPath, true);
                //PrintRule(writer);
                PrintSchema();
            }

            #region "using tasks"
            //Task treePrint = null, rulePrint = null;

            //if (loadTree)
            //{
            //    String logPath = Path.Combine(logFolder, logPrefix + "Tree.txt");
            //    TextWriter writer = new StreamWriter(logPath, true);
            //    treePrint = new Task(() => PrintTree(resultTree, writer));
            //}

            //if (loadRule)
            //{
            //    String logPath = Path.Combine(logFolder, logPrefix + "Rule.txt");
            //    TextWriter writer = new StreamWriter(logPath, true);
            //    rulePrint = new Task(() => PrintRule(writer));
            //}

            //if (!Object.ReferenceEquals(treePrint, null))
            //    treePrint.Start();
            //if (!Object.ReferenceEquals(rulePrint, null))
            //    rulePrint.Start();

            //if (!Object.ReferenceEquals(treePrint, null))
            //    treePrint.Wait();
            //if (!Object.ReferenceEquals(rulePrint, null))
            //    rulePrint.Wait();
            #endregion "using tasks"
        }

        static void PrintTree(NodeClass currentNode, TextWriter writer, int depth = 0)
        {

            String logLine = "";
            for (int i = 0; i < depth; i++)
                logLine += "-";
            logLine += currentNode.name;
            LogUtility.LogIt(logLine, writer);

            if (currentNode.childNodes.Count > 0)
                foreach (NodeAppearPair childNodeAppearPair in currentNode.childNodes)
                    PrintTree(childNodeAppearPair.nodeClass, writer, depth + 1);
        }

        static void PrintRule(TextWriter writer)
        {
            //namespaces
            LogUtility.LogIt("<?xml version=\"1.0\" encoding=\"utf-8\"?>", writer);
            LogUtility.LogIt("<xs:schema elementFormDefault=\"qualified\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:ditaarch=\"http://dita.oasis-open.org/architecture/2005/\">", writer);

            //root
            LogUtility.LogIt(String.Format("<xs:element name=\"{0}\" type=\"{0}Type\"/>", resultRule[0].name), writer);

            foreach (NodeClass node in resultRule)
            {
                //simple type
                if (node.attributes.Count == 0 && node.childNodes.Count == 0)
                    continue;

                LogUtility.LogIt("", writer);
                LogUtility.LogIt(String.Format("<xs:complexType name=\"{0}\">", node.name + "Type"), writer);
                LogUtility.LogIt("<xs:sequence>", writer);
                LogUtility.LogIt("<xs:choice maxOccurs=\"unbounded\">", writer);

                foreach (NodeAppearPair childNodeAppearPair in node.childNodes)
                {
                    String nodeType = "";

                    //simple type
                    if (childNodeAppearPair.nodeClass.attributes.Count == 0 && childNodeAppearPair.nodeClass.childNodes.Count() == 0)
                        nodeType = "xs:string";
                    else
                        nodeType = childNodeAppearPair.nodeClass.name + "Type";
                    LogUtility.LogIt(String.Format("<xs:element name=\"{0}\" type=\"{1}\"/>", childNodeAppearPair.nodeClass.name, nodeType), writer);
                }

                LogUtility.LogIt("</xs:choice>", writer);
                LogUtility.LogIt("</xs:sequence>", writer);
                LogUtility.LogIt("</xs:complexType>", writer);
            }

            LogUtility.LogIt("</xs:schema>", writer);
            LogUtility.LogIt("", writer);
        }

        static void PrintSchema()
        {
            XmlSchema finalSchema = new XmlSchema();

            //add namespace
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());
            String defaultNamespace = @"http://www.w3.org/2001/XMLSchema";
            nsmgr.AddNamespace("xsi", @"http://www.w3.org/2001/XMLSchema-instance");
            nsmgr.AddNamespace("ditaarch", @"http://dita.oasis-open.org/architecture/2005/");

            //root element
            XmlSchemaElement rootSchemaEl = new XmlSchemaElement();
            rootSchemaEl.Name = resultRule[0].name;
            rootSchemaEl.SchemaTypeName = new XmlQualifiedName(rootSchemaEl.Name);
            finalSchema.Items.Add(rootSchemaEl);

            //all types
            foreach (NodeClass currentNode in resultRule)
            {
                //simple type
                if (currentNode.childNodes.Count == 0 && currentNode.attributes.Count() == 0)
                    continue;

                //complex Type
                XmlSchemaComplexType currentType = new XmlSchemaComplexType();
                currentType.Name = currentNode.name;

                //add attributes
                foreach (AttributeClass attr in currentNode.attributes)
                {
                    //check attribute name
                    if (!String.IsNullOrEmpty(attr.prefix))
                        continue;

                    XmlSchemaAttribute currentSchemaAttr = new XmlSchemaAttribute();
                    currentSchemaAttr.Name = attr.name;
                    currentSchemaAttr.SchemaTypeName = new XmlQualifiedName("string", defaultNamespace);

                    //required
                    if (attr.appearCount == currentNode.appearCount)
                        currentSchemaAttr.Use = XmlSchemaUse.Required;
                    else
                        currentSchemaAttr.Use = XmlSchemaUse.Optional;

                    //add attribute to schema
                    currentType.Attributes.Add(currentSchemaAttr);
                }

                //add child elements
                XmlSchemaSequence childElSequence = new XmlSchemaSequence();
                foreach (NodeAppearPair childNodeAppearPair in currentNode.childNodes)
                {
                    XmlSchemaElement currentSchemaEl = new XmlSchemaElement();
                    currentSchemaEl.Name = childNodeAppearPair.nodeClass.name;

                    //simple or complex type child
                    if (childNodeAppearPair.nodeClass.childNodes.Count == 0 && childNodeAppearPair.nodeClass.attributes.Count() == 0)
                        currentSchemaEl.SchemaTypeName = new XmlQualifiedName("string", defaultNamespace);
                    else
                        currentSchemaEl.SchemaTypeName = new XmlQualifiedName(childNodeAppearPair.nodeClass.name);

                    //min/maxOccurs
                    currentSchemaEl.MinOccurs = childNodeAppearPair.isRequired ? 1 : 0;

                    //maxOccurs
                    currentSchemaEl.MaxOccurs = childNodeAppearPair.maxAppearCount;

                    childElSequence.Items.Add(currentSchemaEl);
                }
                currentType.Particle = childElSequence;
                finalSchema.Items.Add(currentType);
            }

            finalSchema.Write(XmlWriter.Create(Path.Combine(logFolder, logPrefix + "Schema.xsd")), nsmgr);
        }

        static void WriteToLog(String message, Exception ex, String level)
        {
            GlobalContext.Properties["LogPath"] = Path.Combine(logFolder, logPrefix + "Log.txt");
            log4net.Config.XmlConfigurator.Configure();
            log = LogManager.GetLogger(typeof(Program));

            switch (level.ToUpper())
            {
                case "DEBUG": log.Debug(message, ex);
                    break;
                case "INFO": log.Info(message, ex);
                    break;
                case "WARN": log.Warn(message, ex);
                    break;
                case "ERROR": log.Error(message, ex);
                    break;
                case "FATAL": log.Fatal(message, ex);
                    break;
            }
        }

        static void ProcessLoadedData(XmlNode currentEl)
        {
            //jump certain nodes
            if (currentEl.Name.Equals("table") || !(currentEl is XmlElement))
                return;

            //find rule in result rule list
            NodeClass currentRule = null;
            foreach (NodeClass rule in resultRule)
                if (rule.name.Equals(currentEl.Name))
                {
                    currentRule = rule;
                    break;
                }
            if (Object.ReferenceEquals(currentRule, null))
            {
                WriteToLog(String.Format("Missing <{0}> in result rule!!", currentEl.Name), null, "ERROR");
                return;
            }

            foreach (NodeAppearPair childPair in currentRule.childNodes)
            {
                Boolean findEl = false;
                int maxAppear = 0;
                foreach (XmlNode childEl in currentEl.ChildNodes)
                    if (childEl.Name.Equals(childPair.nodeClass.name))
                    {
                        maxAppear++;
                        findEl = true;
                    }
                if (!findEl)
                    childPair.isRequired = false;
                if (maxAppear > childPair.maxAppearCount)
                    childPair.maxAppearCount = maxAppear;
            }

            if (currentEl.HasChildNodes)
                foreach (XmlNode childNode in currentEl)
                    ProcessLoadedData(childNode);
        }
    }
}
