using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TestingXml
{
    class Program
    {
        static void Main(string[] args)
        {
            //Stack<Char> s = new Stack<Char>(3);
            //Queue<Char> q = new Queue<Char>(3);

            //String org = "12345";
            //foreach(Char c in org.ToCharArray())
            //{
            //    s.Push(c);
            //    q.Enqueue(c);
            //}
            //Console.WriteLine(new String(s.ToArray()));
            //Console.WriteLine(new String(q.ToArray()));
            //Console.ReadKey();


            String input = @"styling.xml";
            String output = @"styling-output.xml";
            RemoveNodes("<font", input, output);
        }

        private static void RemoveNodes(String theWord, String input, String output)
        {
            char[] buffer = new char[1];
            MyWord myWord = new MyWord(theWord);

            using (StreamReader sr = File.OpenText(input))
            using (MemoryStream ms = new MemoryStream())
            using (StreamWriter sw = new StreamWriter(ms))
            {
                while(!sr.EndOfStream)
                {
                    sr.ReadBlock(buffer, 0, 1);
                    sw.Write(buffer);
                    if (!myWord.QueueNewChar(buffer[0]))
                        continue;


                }

                using (FileStream fs = new FileStream(output, FileMode.Create))
                {
                    ms.Position = 0;
                    ms.CopyTo(fs);
                }
            }
        }

        private static void ReaderToWriter(XmlReader reader, XmlWriter writer)
        {
            writer.WriteStartElement(reader.Name);
        }
    }
}
