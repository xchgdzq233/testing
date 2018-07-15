using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CalculonBack.NodeClasses
{
    public class Document
    {
        private static Document _instance;
        private XmlDocument _xmlDoc;
        private List<Element> _nodes;

        private Document()
        {
            using (Stream inputStream = File.OpenRead(ConfigurationManager.AppSettings["InputPath"]))
            {
                //load xml
                _xmlDoc = new XmlDocument();
                _xmlDoc.Load(inputStream);

                //load paragraphs
                _nodes = new List<INode>();
                XmlNode body = _xmlDoc.SelectSingleNode("/body");
                LoadBody(body);
            }
        }

        private void LoadBody(XmlNode currentNode)
        {
            foreach (XmlNode childNode in currentNode.ChildNodes)
            {
                if (!(childNode is XmlElement))
                    continue;

                //if childnode has text
                XmlNodeList textChildren = childNode.SelectNodes("child::text()");
                if (textChildren.Count > 0)
                    foreach (XmlNode textChild in textChildren)
                        _nodes.Add(textChild.Value);

                if (childNode.HasChildNodes)
                    LoadBody(childNode);
            }
        }

        public static Document GetInstance()
        {
            if (Object.ReferenceEquals(null, _instance))
                _instance = new Document();

            return _instance;
        }

        public String GetTopic()
        {
            return "Title: " + _xmlDoc.SelectSingleNode("//title").InnerText;
        }

        public List<String> GetParagraphs()
        {
            return _nodes;
        }
    }
}
