using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace blah
{
    public class NodeClass
    {
        public String nodeName { get; set; }
        public List<NodeClass> childNodeClasses { get; set; }

        public NodeClass()
        {
            nodeName = "";
            childNodeClasses = new List<NodeClass>();
        }

        public NodeClass(String nodeName)
        {
            this.nodeName = nodeName;
            childNodeClasses = new List<NodeClass>();
        }

        public Boolean HasChild(String childNodeName)
        {
            if (childNodeClasses.Count == 0)
                return false;
            foreach (NodeClass childNodeClass in childNodeClasses)
                if (childNodeClass.nodeName.Equals(childNodeName))
                    return true;
            return false;
        }

        public NodeClass GetChild(String childNodeName)
        {
            if (HasChild(childNodeName))
                foreach (NodeClass childNodeClass in childNodeClasses)
                    if (childNodeClass.nodeName.Equals(childNodeName))
                        return childNodeClass;
            return null;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(this.nodeName);
            foreach (NodeClass child in this.childNodeClasses)
            {
                sb.AppendLine(child.ToString());
            }
            return base.ToString();
        }
    }
}
