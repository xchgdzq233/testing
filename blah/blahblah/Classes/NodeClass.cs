using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace blahblah
{
    class NodeClass
    {
        public String name { get; set; }
        public List<NodeAppearPair> childNodes { get; set; }
        public Boolean containText { get; set; }
        public List<AttributeClass> attributes { get; set; }
        public XmlSchemaComplexType schemaType { get; set; }
        public int appearCount { get; set; }
        public int minOccurs { get; set; }
        public int maxOccurs { get; set; }

        public NodeClass()
        {
            name = "";
            childNodes = new List<NodeAppearPair>();
            containText = false;
            attributes = new List<AttributeClass>();
            schemaType = null;
            appearCount = 1;
            minOccurs = -1;
            maxOccurs = -1;
        }

        public NodeClass(String name)
        {
            this.name = name;
            childNodes = new List<NodeAppearPair>();
            containText = false;
            attributes = new List<AttributeClass>();
            schemaType = null;
            appearCount = 1;
            minOccurs = -1;
            maxOccurs = -1;
        }

        public Boolean HasChild(String childName)
        {
            if (childNodes.Count == 0)
                return false;
            foreach (NodeAppearPair childClass in childNodes)
                if (childClass.nodeClass.name.Equals(childName))
                    return true;
            return false;
        }

        public NodeAppearPair GetChild(String childName)
        {
            foreach (NodeAppearPair childClass in childNodes)
                if (childClass.nodeClass.name.Equals(childName))
                    return childClass;
            return null;
        }
    }
}
