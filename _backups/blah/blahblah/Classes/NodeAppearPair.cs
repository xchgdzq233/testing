using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blahblah
{
    class NodeAppearPair
    {
        public NodeClass nodeClass { get; set; }
        public int totalAppearCount { get; set; }
        public Boolean isRequired { get; set; }
        public int maxAppearCount { get; set; }

        public NodeAppearPair()
        {
            nodeClass = null;
            totalAppearCount = 1;
            isRequired = true;
            maxAppearCount = 0;
        }

        public NodeAppearPair(String nodeClassName)
        {
            nodeClass = new NodeClass(nodeClassName);
            totalAppearCount = 1;
            isRequired = true;
            maxAppearCount = 0;
        }

        public NodeAppearPair(ref NodeClass nodeClass)
        {
            this.nodeClass = nodeClass;
            totalAppearCount = 1;
            isRequired = true;
            maxAppearCount = 0;
        }
    }
}
