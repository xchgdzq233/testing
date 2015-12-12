using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blahblah
{
    class AttributeClass
    {
        public String name { get; set; }
        public int appearCount { get; set; }
        public String prefix { get; set; }

        public AttributeClass()
        {
            name = "";
            appearCount = 1;
            prefix = "";
        }

        public AttributeClass(String name)
        {
            this.name = name;
            appearCount = 1;
            prefix = "";
        }

        public AttributeClass(String name, String value)
        {
            this.name = name;
            appearCount = 1;
            this.prefix = "";
        }

        public AttributeClass(String name, String value, String prefix)
        {
            this.name = name;
            appearCount = 1;
            this.prefix = prefix;
        }
    }
}
