using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace blah
{
    public sealed class JanetsXMLGenius
    {
        private static JanetsXMLGenius _instance;

        public static JanetsXMLGenius Instance
        {
            get { return _instance == null ? _instance = new JanetsXMLGenius() : _instance; }
        }

        private JanetsXMLGenius() { }


        public void DisplayXmlDocumentStructure(Stream input)
        {
            XmlDocument document = new XmlDocument();
            document.Load(input);


            XmlElement docElement = document.DocumentElement;
            XmlNode root = docElement as XmlNode;
            DisplayStructureRcursively(root);

        }


        void DisplayStructureRcursively(XmlNode node,int depth = 0)
        {
            Console.Write("|");
            for(int i =0; i < depth;i++)
            {
                Console.Write("-");
            }
            Console.WriteLine(node.Name);
            foreach (XmlNode child in node.ChildNodes)
            {
                DisplayStructureRcursively(child, depth + 1);
            }
        }


       public void CreateSampleTopic()
        {
            topic t = new topic();
            t.title = "Test";
            t.shortdesc = "Description";
            t.prolog = new prolog();
            t.body = new body();
            String path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            Stream output = File.OpenWrite(Path.Combine(path, "topic.xml"));
            XmlSerializer ser = new XmlSerializer(typeof(topic));
            ser.Serialize(output, t);
            output.Flush();
        }

    }
}
