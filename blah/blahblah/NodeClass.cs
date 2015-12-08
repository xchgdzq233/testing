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
        public List<NodeClass> childClasses { get; set; }
        public List<String> possibleChildNames { get; set; }

        public NodeClass()
        {
            name = "";
            childClasses = new List<NodeClass>();
            possibleChildNames = new List<String>();
        }

        public NodeClass(String name)
        {
            this.name = name;
            childClasses = new List<NodeClass>();
            possibleChildNames = new List<String>();
        }

        public Boolean HasChild(String childName)
        {
            if (childClasses.Count == 0)
                return false;
            foreach (NodeClass childClass in childClasses)
                if (childClass.name.Equals(childName))
                    return true;
            return false;
        }

        public NodeClass GetChild(String childName)
        {
            foreach (NodeClass childClass in childClasses)
                if (childClass.name.Equals(childName))
                    return childClass;
            return null;
        }
    }
}
