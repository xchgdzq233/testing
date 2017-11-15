using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

            //, "div"
            RemoveNodes(new List<String>() { "font", "strong", "em", "span" }, input, output);

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
            List<String> result = new List<String>();
            int totalA = 0, httpA = 0, mediaA = 0, hasChildA = 0;

            XDocument doc = XDocument.Load(file);
            doc.Descendants("a").Where(e => String.IsNullOrEmpty(e.Value.Trim())).Remove();
            doc.Descendants("a").Attributes("href").All(e =>
            {
                String href = e.Value.Trim().ToLower();

                totalA++;

                if (href.StartsWith("http"))
                    httpA++;
                else if (href.StartsWith("/-/media/sots/regulations/"))
                    mediaA++;
                else if (e.Parent.HasElements)
                    hasChildA++;
                else
                    result.Add(href);

                return true;
            });

            result.Sort();

            using (StreamWriter sw = new StreamWriter(@"checking-result.txt", true))
            {
                foreach (String href in result)
                    sw.WriteLine("href=\"{0}\"", href);

                sw.WriteLine();
            }

            Console.WriteLine("Total <a>: {0}, Http <a>: {1}, Starts with \"media\": {2}, A with Child: {3}", totalA.ToString(), httpA.ToString(), mediaA.ToString(), hasChildA.ToString());
            Console.ReadKey();
        }

        private static void ProcessXml(String file)
        {
            XDocument doc = XDocument.Load(file, LoadOptions.None);

            // 1. replace empty <a>
            doc.Descendants("a").Where(e => String.IsNullOrEmpty(e.Value.Trim())).Remove();

            // 2. add target="_blank" to all <a>
            doc.Descendants("a").All(e => { e.SetAttributeValue("target", "_blank"); return true; });

            // 3. correct all href for <a>
            doc.Descendants("a").Attributes("href").All(e =>
            {
                String href = e.Value.Trim().ToLower();

                if (href.StartsWith("/-/media/sots/regulations/"))
                    e.Value = href.Replace("/-/media/sots/regulations/", "http://portal.ct.gov/-/media/sots/regulations/");

                return true;
            });

            // 4. remove all stylging
            doc.Descendants().Attributes("align").Remove();
            doc.Descendants().Attributes("style").Remove();

            // 5: remove empty elements which is not a <td>
            doc.Descendants().Where(e => String.IsNullOrEmpty(e.Value.Trim()) && !e.Name.LocalName.Equals("td")).Remove();

            // 6: remove trailing spaces
            String s = Regex.Replace(doc.ToString(SaveOptions.DisableFormatting), @"\s+", " ");
            doc = XDocument.Parse(s, LoadOptions.None);

            // last: remove encoding
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = false;
            using (XmlWriter xw = XmlWriter.Create(file, settings))
                doc.Save(xw);
        }
    }
}