using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace blah
{
    public class AlphaNumericComparor : IComparer<String>
    {

        public int Compare(string x, string y)
        {
            string s1 = x;
            if (s1 == null)
            {
                return 0;
            }
            string s2 = y;
            if (s2 == null)
            {
                return 0;
            }

            int len1 = s1.Length;
            int len2 = s2.Length;
            int marker1 = 0;
            int marker2 = 0;

            // Walk through two the strings with two markers.
            while (marker1 < len1 && marker2 < len2)
            {
                char ch1 = s1[marker1];
                char ch2 = s2[marker2];

                // Some buffers we can build up characters in for each chunk.
                char[] space1 = new char[len1];
                int loc1 = 0;
                char[] space2 = new char[len2];
                int loc2 = 0;

                // Walk through all following characters that are digits or
                // characters in BOTH strings starting at the appropriate marker.
                // Collect char arrays.
                do
                {
                    space1[loc1++] = ch1;
                    marker1++;

                    if (marker1 < len1)
                    {
                        ch1 = s1[marker1];
                    }
                    else
                    {
                        break;
                    }
                } while (char.IsDigit(ch1) == char.IsDigit(space1[0]));

                do
                {
                    space2[loc2++] = ch2;
                    marker2++;

                    if (marker2 < len2)
                    {
                        ch2 = s2[marker2];
                    }
                    else
                    {
                        break;
                    }
                } while (char.IsDigit(ch2) == char.IsDigit(space2[0]));

                // If we have collected numbers, compare them numerically.
                // Otherwise, if we have strings, compare them alphabetically.
                string str1 = new string(space1);
                string str2 = new string(space2);

                int result;

                if (char.IsDigit(space1[0]) && char.IsDigit(space2[0]))
                {
                    int thisNumericChunk = int.Parse(str1);
                    int thatNumericChunk = int.Parse(str2);
                    result = thisNumericChunk.CompareTo(thatNumericChunk);
                }
                else
                {
                    result = str1.CompareTo(str2);
                }

                if (result != 0)
                {
                    return result;
                }
            }
            return len1 - len2;
        }
    }



    class Program
    {

        static void PrintPathsForSingleRepeals()
        {
            String paRepealsDir = @"\\10.101.17.16\c$\Users\fizerc\Desktop\oldstuff\ConvertedRegs\DeltaRegsConversion\Filings\69_PARepeals_doneindev__6_11_2014__donedone\DitaFiles";
            String[] dirs = Directory.GetDirectories(paRepealsDir, "*single_done*", SearchOption.TopDirectoryOnly);
            List<String> paths = new List<string>();
            List<String> subjects = new List<string>();
            foreach (String dir in dirs)
            {
                if (!dir.EndsWith("__inprod"))
                {
                    Directory.Move(dir, dir + "__inprod");
                }
            }
            subjects = subjects.OrderBy(x => x, new AlphaNumericComparor()).ToList();
            foreach (string s in subjects)
            {
                Console.WriteLine(s);
            }


        }

        static void scanXMLs(XmlNode inputNode, List<Dictionary<String, int>> results, int inputNodeDepth = 0)
        {
            Dictionary<String, int> currentDict = new Dictionary<String, int>(); ;
            if (inputNodeDepth + 1 >= results.Count)
            {
                results.Add(currentDict);
            }
            currentDict = results[inputNodeDepth + 1];

            foreach (XmlNode childNode in inputNode.ChildNodes)
            {
                if (childNode is XmlElement)
                {

                    if (currentDict.ContainsKey(childNode.Name))
                        currentDict[childNode.Name]++;
                    else
                        currentDict.Add(childNode.Name, 1);

                    //if (childNode.HasChildNodes)
                    //    scanXMLs(childNode, results, inputNodeDepth + 1);
                }
            }
        }
        static String logPath;

        static void logIt(String input, StreamWriter writer, String newLogPath)
        {
            if (!File.Exists(newLogPath))
                File.Create(newLogPath);
            using (writer)
                writer.WriteLine(input);
        }

        static void CreateNodeTree(XmlNode currentNode, NodeClass parentNodeClass)
        {
            NodeClass currentNodeClass;
            if (!parentNodeClass.HasChild(currentNode.Name))
            {
                currentNodeClass = new NodeClass(currentNode.Name);
                parentNodeClass.childNodeClasses.Add(currentNodeClass);
            }
            else
                currentNodeClass = parentNodeClass.GetChild(currentNode.Name);

            if (currentNode.HasChildNodes)
                foreach (XmlNode childNode in currentNode)
                    CreateNodeTree(childNode, currentNodeClass);
        }

        static void CreateRules(XmlNode currentNode, ElementClass parentElementClass)
        {
            ElementClass currentElementClass = new ElementClass(currentNode.Name);
            Boolean addElement = true;
            foreach (ElementClass el in resultRule)
                if (el.elementName.Equals(currentNode.Name))
                {
                    currentElementClass = el;
                    addElement = false;
                    break;
                }
            if (addElement)
                resultRule.Add(currentElementClass);

            if (!Object.ReferenceEquals(parentElementClass, null))
                if (!parentElementClass.HasChild(currentNode.Name))
                    parentElementClass.childElementClasses.Add(currentElementClass.elementName);

            if (currentNode.HasChildNodes)
                foreach (XmlNode childNode in currentNode)
                    if (childNode is XmlElement)
                        CreateRules(childNode, currentElementClass);
        }

        static void printTree(NodeClass result, int depth = 0)
        {
            String newLogPath = logPath + "Tree.txt";
            StreamWriter writer = new StreamWriter(newLogPath, true);
            String logLine = "";
            for (int i = 0; i < depth; i++)
                logLine += "-";
            logLine += result.nodeName;
            logIt(logLine, writer, newLogPath);

            if (result.childNodeClasses.Count > 0)
                foreach (NodeClass childNodeClass in result.childNodeClasses)
                    printTree(childNodeClass, depth + 1);
        }

        static void printRules()
        {
            String newLogPath = logPath + "Rule.txt";
            StreamWriter writer = new StreamWriter(newLogPath, true);
            StringBuilder logLine = new StringBuilder();

            foreach (ElementClass el in resultRule)
            {
                if (el.childElementClasses.Count > 0)
                {
                    logLine.AppendLine(String.Format("<{0}> can have children: ", el.elementName));
                    foreach (String childEl in el.childElementClasses)
                        logLine.AppendLine(String.Format("\t<{0}>", childEl));
                    logLine.AppendLine();
                }
            }
            logIt(logLine.ToString(), writer, newLogPath);
        }

        static List<ElementClass> resultRule;

        static void Main(string[] args)
        {
            #region "check missing"
            //logPath = Path.Combine(@"C:\Fairfax\Janet\tests\XML Author", String.Format("{0}T{1}.txt", DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("HHmmss")));
            //String[] files = Directory.GetFiles(@"\\10.101.17.16\c$\Users\fizerc\Desktop\oldstuff\ConvertedRegs\Titles", "*.dita", SearchOption.AllDirectories);
            //Console.WriteLine("Total Files: " + files.Length);

            //int finished = 0;
            //foreach (String file in files)
            //{
            //    bool title = true, shortdesc = true, prolog = true;
            //    if (finished % 100 == 0)
            //        Console.Title = String.Format("Processed {0:F2}% of files...", ((decimal)finished * 100 / (decimal)files.Length));
            //    using (Stream inputStream = File.OpenRead(file))
            //    {
            //        XmlDocument xml = new XmlDocument();
            //        xml.Load(inputStream);
            //        XmlNode root = xml.DocumentElement;
            //        foreach (XmlNode childNode in root)
            //        {
            //            if (childNode.Name.Equals("title"))
            //                title = false;
            //            if (childNode.Name.Equals("shortdesc"))
            //                shortdesc = false;
            //            if (childNode.Name.Equals("prolog"))
            //                prolog = false;
            //        }
            //    }
            //    if (title)
            //        Console.WriteLine(Path.GetFileName(file) + ": missing title");
            //    if (shortdesc)
            //        Console.WriteLine(Path.GetFileName(file) + ": missing shortdesc");
            //    if (prolog)
            //        Console.WriteLine(Path.GetFileName(file) + ": missing prolog");
            //}
            //Environment.Exit(0);
            #endregion

            logPath = Path.Combine(@"C:\Fairfax\Janet\tests\XML Author", String.Format("{0}T{1}-", DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("hhmmss")));

            //String[] files = Directory.GetFiles(@"\\10.101.17.16\c$\Users\fizerc\Desktop\oldstuff\ConvertedRegs\Titles\Title_1\1-1h\convertedfiles\eReg\1", "*.dita", SearchOption.AllDirectories);
            String[] files = Directory.GetFiles(@"\\10.101.17.16\c$\Users\fizerc\Desktop\oldstuff\ConvertedRegs\Titles", "*.dita", SearchOption.AllDirectories);
            Console.WriteLine("Total Files: " + files.Length);

            int finished = 0;
            NodeClass resultTree = new NodeClass();
            resultRule = new List<ElementClass>();

            foreach (String file in files)
            {
                if (finished % 100 == 0)
                    Console.Title = String.Format("Processed {0:F2}% of files...", ((decimal)finished * 100 / (decimal)files.Length));

                using (Stream inputStream = File.OpenRead(file))
                {
                    if (inputStream.Length == 0)
                    {
                        Console.WriteLine("***ERROR: Cannot read file: " + Path.GetFileName(file));
                        continue;
                    }

                    XmlDocument xml = new XmlDocument();
                    xml.Load(inputStream);
                    XmlElement root = xml.DocumentElement;

                    resultTree.nodeName = root.Name;
                    ElementClass currentElement = new ElementClass(root.Name);

                    foreach (XmlNode childNode in root)
                        CreateNodeTree(childNode, resultTree);
                    CreateRules(root, null);
                }

                finished++;
            }
            Console.Title = String.Format("Processed 100% of files.");
            Console.WriteLine();
            Console.WriteLine("Writing to log...");
            printTree(resultTree);
            printRules();
            Console.WriteLine(@"Finished at:\nC:\Fairfax\Janet\tests\XML Author");

            Environment.Exit(0);

            //foreach (String file in files)
            //{
            //    XmlDocument section = new XmlDocument();
            //    Stream input = null;
            //    try
            //    {
            //        input = File.OpenRead(file);
            //        section.Load(input);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("ooooh oh");
            //    }
            //    finally
            //    {
            //        if (input != null && input.CanRead)
            //        {
            //            input.Close();
            //        }
            //    }

            //}

            #region "old code"
            //  //    Dictionary<String, String> d2 = new Dictionary<string, string>();

            //  String docPath = @"\\10.101.17.16\c$\Users\fizerc\Desktop\oldstuff\ConvertedRegs\Titles\Title_1\1-1h\convertedfiles\eReg\1\1-1h-5.dita";
            //  Stream input = File.OpenRead(docPath);
            //  JanetsXMLGenius.Instance.DisplayXmlDocumentStructure(input);
            ////  JanetsXMLGenius.Instance.CreateSampleTopic();
            //  Environment.Exit(0);
            //  String filingDir = @"\\10.101.17.16\c$\Users\fizerc\Desktop\oldstuff\ConvertedRegs\DeltaRegsConversion\Filings";
            //  String paRepealDir = @"\\10.101.17.16\c$\Users\fizerc\Desktop\oldstuff\ConvertedRegs\DeltaRegsConversion\Filings\69_PARepeals_doneindev__6_11_2014__donedone__inprod\DitaFiles";
            //  String[] total = Directory.GetDirectories(filingDir, "*", SearchOption.TopDirectoryOnly);
            //  Console.WriteLine("Total = " + total.Length);
            //  String[] singles = Directory.GetDirectories(filingDir, "*single*", SearchOption.TopDirectoryOnly);
            //  Console.WriteLine("Total single  = " + singles.Length);
            //  String[] doneindev = Directory.GetDirectories(filingDir, "*doneindev*", SearchOption.TopDirectoryOnly);
            //  Console.WriteLine("Done in dev = " + doneindev.Length);
            //  String[] donedone = Directory.GetDirectories(filingDir, "*donedone*", SearchOption.TopDirectoryOnly);
            //  Console.WriteLine("Done Done  = " + donedone.Length);
            //  String[] inprod = Directory.GetDirectories(filingDir, "*inprod*", SearchOption.TopDirectoryOnly);
            //  Console.WriteLine("In Production  = " + inprod.Length);
            //  String[] inprodNoIssue = Directory.GetDirectories(filingDir, "*inprod", SearchOption.TopDirectoryOnly);
            //  Console.WriteLine("In Production No Issues  = " + inprodNoIssue.Length);
            //  String[] totalPARepeals = Directory.GetDirectories(paRepealDir);
            //  String[] inProdPARepeals = Directory.GetDirectories(paRepealDir, "*_inprod");
            //  Console.WriteLine("UnDone PARepeals = " + (totalPARepeals.Length - inProdPARepeals.Length));
            //  Environment.Exit(0);
            //  //    String[] dirs = Directory.GetDirectories(filingDir, "*", SearchOption.TopDirectoryOnly);
            //  //    List<String> missingReviews = new List<string>();
            //  //    List<String> missingReviewDir = new List<string>();
            //  //    foreach (String dir in dirs)
            //  //    {
            //  //        String reviewDir = Path.Combine(dir, "ToReview");
            //  //        if (!Directory.Exists(reviewDir))
            //  //        {
            //  //            missingReviewDir.Add(new DirectoryInfo(dir).Name);
            //  //        }
            //  //        else
            //  //        {
            //  //            String[] files = Directory.GetFiles(reviewDir);
            //  //            if (files.Length == 0)
            //  //            {
            //  //                missingReviews.Add(new DirectoryInfo(dir).Name);
            //  //            }
            //  //        }
            //  //    }

            //  //    Console.WriteLine("Missing Review files: " + missingReviews.Count);
            //  //    foreach(String d in missingReviews)
            //  //    {
            //  //        Console.WriteLine(d);
            //  //    }

            //  //    Console.WriteLine("Missing Review Folder: " + missingReviewDir.Count);
            //  //    foreach (String d in missingReviewDir)
            //  //    {
            //  //        Console.WriteLine(d);
            //  //    }
            //  //    Console.WriteLine();
            //  //    foreach (String file in Directory.GetDirectories(filingDir))
            //  //    {
            //  //        if (!file.Contains("donedone"))
            //  //        {
            //  //            Console.WriteLine(new DirectoryInfo(file).Name);
            //  //        }
            //  //    }
            //  //}

            #endregion
        }
    }
}
