using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace TestingXml
{
    class Program
    {
        private static ILog logger = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            String input = @"styling.xml";
            String output = @"styling-output.xml";

            RemoveNodes(new List<String>() { "font", "div", "strong", "em", "span" }, input, output);

            ProcessXml(output);

            //CheckXml(output);
        }

        private static void RemoveNodes(List<String> nodeNames, String inputFile, String outputFile)
        {
            String tempFile = @"styling-temp.xml";
            File.Copy(inputFile, tempFile, true);

            foreach (String nodeName in nodeNames)
            {
                String node = nodeName.Trim().ToLower();
                RemoveNode(node, tempFile, outputFile);
                File.Copy(outputFile, tempFile, true);
                RemoveNode(String.Concat("/", node), tempFile, outputFile);
                File.Copy(outputFile, tempFile, true);
            }
        }

        private static void RemoveNode(String nodeName, String inputFile, String outputFile)
        {
            using (StreamReader sr = File.OpenText(inputFile))
            using (MemoryStream ms = new MemoryStream())
            using (StreamWriter sw = new StreamWriter(ms))
            {
                while (sr.Peek() >= 0)
                    FindNode(sr, nodeName, sw);

                sw.Flush();
                using (FileStream fs = new FileStream(outputFile, FileMode.Create))
                {
                    ms.Position = 0;
                    ms.CopyTo(fs);
                }
            }
        }

        private static bool FindNode(StreamReader sr, String nodeName, StreamWriter sw)
        {
            char[] buffer = new char[1];
            sr.ReadBlock(buffer, 0, 1);

            List<char> listToIgnore = new List<char>() { '\t', '\n', '\r' };
            if (listToIgnore.Contains(buffer[0]))
                return false;

            if (!buffer[0].Equals('<'))
            {
                sw.Write(buffer[0]);
                return false;
            }

            List<char> listBuffer = new List<char>();
            while (sr.Peek() >= 0)
            {
                sr.ReadBlock(buffer, 0, 1);
                if (buffer[0].Equals('>'))
                    break;
                listBuffer.Add(buffer[0]);
            }

            bool _nodeFound = true;
            if (listBuffer.Count < nodeName.Length)
                _nodeFound = false;
            else
                for (int i = 0; i < nodeName.Length; i++)
                    if (!Char.ToLower(listBuffer[i]).Equals(nodeName[i]))
                    {
                        _nodeFound = false;
                        break;
                    }

            if (!_nodeFound)
            {
                sw.Write('<');
                foreach (char c in listBuffer)
                    sw.Write(c);
                sw.Write('>');
            }

            return _nodeFound;
        }

        private static void CheckXml(String file)
        {
            Dictionary<String, int> result = new Dictionary<string, int>();
            int totalA = 0, httpA = 0;

            XDocument doc = XDocument.Load(file);
            doc.Descendants("a").Where(e => String.IsNullOrEmpty(e.Value.Trim())).Remove();
            doc.Descendants("a").Attributes("href").All(e =>
            {
                String href = e.Value.Trim().ToLower();

                totalA++;

                if (!href.StartsWith("http"))
                {
                    if (!result.ContainsKey(href))
                        result.Add(href, 1);
                    else
                        result[href]++;
                }
                else
                    httpA++;

                return true;
            });

            result.OrderBy(pair => pair.Key);

            using (StreamWriter sw = new StreamWriter(@"checking-result.txt"))
            {
                foreach (KeyValuePair<String, int> pair in result)
                    sw.WriteLine("href=\"{0}\" - [{1}]", pair.Key, pair.Value);
            }

            Console.WriteLine("Total <a>: {0}, Http <a>: {1}", totalA.ToString(), httpA.ToString());
            Console.ReadKey();
        }

        private static void ProcessXml(String file)
        {
            XDocument doc = XDocument.Load(file);
            doc.Descendants("a").Where(e => String.IsNullOrEmpty(e.Value.Trim())).Remove();
            doc.Descendants("a").All(e => { e.SetAttributeValue("target", "_blank"); return true; });
            doc.Descendants("a").Attributes("href").All(e =>
            {
                String href = e.Value.Trim().ToLower();

                if (href.StartsWith("/sots/lib/sots"))
                    e.Value = href.Replace("/sots/lib/sots", "http://www.sots.ct.gov/sots/lib/sots");
                else if (href.StartsWith("../lib/sots"))
                    e.Value = href.Replace("../lib/sots", "http://www.sots.ct.gov/sots/lib/sots");
                else if (!e.Value.StartsWith("http"))
                    Console.WriteLine("{0} - {1}", e.Value, href);

                return true;
            });
            doc.Descendants("p").Attributes("align").Remove();
            doc.Descendants("p").Attributes("style").Remove();
            doc.Save(file);
        }
    }
}