using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace blahblah
{
    public class Program
    {
        static String logFolder;
        static String logPrefix;
        static NodeClass resultTree;
        static Dictionary<String, List<String>> resultRule;

        static void Main(string[] args)
        {
            logFolder = @"C:\Fairfax\Janet\tests\XML Author";
            logPrefix = String.Format("{0}T{1}-", DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("hhmmss"));

            //String[] files = Directory.GetFiles(@"\\10.101.17.16\c$\Users\fizerc\Desktop\oldstuff\ConvertedRegs\Titles\Title_1\1-1h\convertedfiles\eReg\1", "*.dita", SearchOption.AllDirectories);
            String[] files = Directory.GetFiles(@"\\10.101.17.16\c$\Users\fizerc\Desktop\oldstuff\ConvertedRegs\Titles", "*.dita", SearchOption.AllDirectories);

            if (files.Length == 0)
            {
                Console.WriteLine("Cannot find .dita files in the directory.");
                Environment.Exit(0);
            }

            Console.WriteLine(String.Format("Found {0} files.", files.Length));

            int finished = 0;
            resultTree = new NodeClass();
            resultRule = new Dictionary<string, List<String>>();

            foreach (String file in files)
            {
                if (finished % 100 == 0)
                    Console.Title = String.Format("Processed {0:F2}% of files...", ((decimal)finished * 100 / (decimal)files.Length));

                using (Stream xmlStream = File.OpenRead(file))
                {
                    if (xmlStream.Length == 0)
                    {
                        Console.WriteLine("***ERROR: Cannot read file: " + Path.GetFileName(file));
                        continue;
                    }

                    XmlDocument xml = new XmlDocument();
                    xml.Load(xmlStream);

                    LoadEls(xml.DocumentElement);
                }
            }
        }

        static void LoadEls(XmlElement currentEl, NodeClass parentNodeClass = null)
        {
            NodeClass currentNodeClass = new NodeClass(currentEl.Name);

            //creating the tree
            if (Object.ReferenceEquals(parentNodeClass, null))
            {
                //root El
                resultTree = currentNodeClass;

                List<String> possibleChild = new List<String>();
                resultRule.Add(currentEl.Name, possibleChild);
            }
            else
            {
                currentNodeClass = parentNodeClass.GetChild(currentEl.Name);
                //Tree
                if (Object.ReferenceEquals(currentNodeClass, null))
                {
                    //new treeNode
                    currentNodeClass = new NodeClass(currentEl.Name);
                    parentNodeClass.childClasses.Add(currentNodeClass);
                }

                //Rule
                if (!resultRule.ContainsKey(currentEl.Name))
                {
                    //new rule
                    List<String> possibleChild = new List<String>();
                    resultRule.Add(currentEl.Name, possibleChild);
                }
                else
                {

                }
            }


            //check child
            if (currentEl.HasChildNodes)
                foreach (XmlElement childEl in currentEl)
                    LoadEls(childEl, currentNodeClass);
        }
    }
}
