using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

[assembly: log4net.Config.XmlConfigurator(Watch=true)]

namespace blahblah
{
    public class Program
    {
        //configuration variables
        static Boolean loadTree = false, loadRule = true;
        static String logFolder = @"C:\Fairfax\Janet\tests\XML Author", logPrefix;
        static String[] files = Directory.GetFiles(@"\\10.101.17.16\c$\Users\fizerc\Desktop\oldstuff\ConvertedRegs\Titles\Title_1\1-1h\convertedfiles\eReg\1", "*.dita", SearchOption.AllDirectories);
        //static String[] files = Directory.GetFiles(@"\\10.101.17.16\c$\Users\fizerc\Desktop\oldstuff\ConvertedRegs\Titles", "*.dita", SearchOption.AllDirectories);

        //code used variables
        static NodeClass resultTree;
        static List<NodeClass> resultRule;

        static void Main(string[] args)
        {
            logPrefix = String.Format("{0}T{1}-", DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("hhmmss"));

            Console.WriteLine("Extracting files from the directory...");
            if (files.Length == 0)
            {
                Console.WriteLine("Cannot find .dita files in the directory.");
                Environment.Exit(0);
            }
            Console.WriteLine(String.Format("Found {0} files. Start processing...\n", files.Length));

            int finished = 0;
            int failed = 0;
            resultTree = new NodeClass();
            resultRule = new List<NodeClass>();

            foreach (String file in files)
            {
                if (finished % 100 == 0)
                    Console.Title = String.Format("Processed {0:F2}% of files...", ((decimal)finished * 100 / (decimal)files.Length));

                using (Stream xmlStream = File.OpenRead(file))
                {
                    if (xmlStream.Length == 0)
                    {
                        Console.WriteLine("***ERROR: Cannot read file: " + Path.GetFileName(file));
                        failed++;
                        continue;
                    }

                    XmlDocument xml = new XmlDocument();
                    xml.Load(xmlStream);

                    LoadEls(xml.DocumentElement);
                }

                finished++;
            }
            Console.WriteLine(String.Format("Process complete. {0} of files failed.\n", failed));

            Console.WriteLine("Printing Tree/Rule to files at:\n" + logFolder);
            PrintResults();
            Console.WriteLine("Printing complete.");
        }

        static void LoadEls(XmlElement root)
        {
            Task treeTask = null, ruleTask = null;

            if (loadTree)
            {
                resultTree = new NodeClass(root.Name);
                treeTask = new Task(() => LoadChildTree(root, resultTree));
            }

            if (loadRule)
            {
                resultRule.Add(new NodeClass(root.Name));
                ruleTask = new Task(() => LoadChildRule(root, resultRule[0]));
            }

            if (!Object.ReferenceEquals(treeTask, null))
                treeTask.Start();
            if (!Object.ReferenceEquals(ruleTask, null))
                ruleTask.Start();

            if (!Object.ReferenceEquals(treeTask, null))
                treeTask.Wait();
            if (!Object.ReferenceEquals(ruleTask, null))
                ruleTask.Wait();
        }

        static void LoadChildTree(XmlElement parentEl, NodeClass parentTree)
        {
            foreach (XmlElement currentEl in parentEl)
            {
                NodeClass currentNodeTree = parentTree.GetChild(currentEl.Name);
                if (Object.ReferenceEquals(currentNodeTree, null))
                    //new tree
                    currentNodeTree = new NodeClass(currentEl.Name);
                parentTree.childNodes.Add(currentNodeTree);

                //check child
                if (currentEl.HasChildNodes)
                    foreach (XmlElement childEl in currentEl)
                        LoadChildTree(childEl, currentNodeTree);
            }
        }

        static void LoadChildRule(XmlElement parentEl, NodeClass parentRule)
        {
            foreach (XmlElement currentEl in parentEl)
            {
                NodeClass currentNodeRule = new NodeClass(parentEl.Name);
                Boolean newRule = true;
                foreach (NodeClass result in resultRule)
                    if (result.name.Equals(parentEl.Name))
                    {
                        //new rule
                        currentNodeRule = result;
                        newRule = false;
                        break;
                    }
                if (newRule)
                    resultRule.Add(currentNodeRule);
                if (!parentRule.HasChild(parentEl.Name))
                    //new child
                    parentRule.childNodes.Add(currentNodeRule);

                //check child
                if (currentEl.HasChildNodes)
                    foreach (XmlElement childEl in currentEl)
                        LoadChildTree(childEl, currentNodeRule);
            }
        }

        static void PrintResults()
        {
            Task treePrint = null, rulePrint = null;

            if (loadTree)
            {
                treePrint = new Task(() => PrintTree());
            }

            if (loadRule)
            {
                rulePrint = new Task(() => PrintRule());
            }

            if (!Object.ReferenceEquals(treePrint, null))
                treePrint.Start();
            if (!Object.ReferenceEquals(rulePrint, null))
                rulePrint.Start();

            if (!Object.ReferenceEquals(treePrint, null))
                treePrint.Wait();
            if (!Object.ReferenceEquals(rulePrint, null))
                rulePrint.Wait();
        }

        static void PrintTree()
        {
            log4net.ILog treeLog = LogHelper.GetLogger();

            String logPath = Path.Combine(logFolder, logPrefix + "Tree.txt");
        }

        static void PrintRule()
        {
            log4net.ILog ruleLog = LogHelper.GetLogger();
            String logPath = Path.Combine(logFolder, logPrefix + "Tree.txt");

        }
    }

    class LogHelper
    {
        public static log4net.ILog GetLogger([CallerFilePath]String filename = "")
        {
            return log4net.LogManager.GetLogger(filename);
        }
    }
}
