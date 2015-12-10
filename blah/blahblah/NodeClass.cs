using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blahblah
{
    class NodeClass
    {
        public String name { get; set; }
        public List<NodeClass> childNodes { get; set; }

        public NodeClass()
        {
            name = "";
            childNodes = new List<NodeClass>();
        }

        public NodeClass(String name)
        {
            this.name = name;
            childNodes = new List<NodeClass>();
        }

        public Boolean HasChild(String childName)
        {
            if (childNodes.Count == 0)
                return false;
            foreach (NodeClass childClass in childNodes)
                if (childClass.name.Equals(childName))
                    return true;
            return false;
        }

        public NodeClass GetChild(String childName)
        {
            foreach (NodeClass childClass in childNodes)
                if (childClass.name.Equals(childName))
                    return childClass;
            return null;
        }
    }
}
