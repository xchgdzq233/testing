using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blah
{
    public class ElementClass
    {
        public String elementName { get; set; }
        public List<ElementClass> childElementClasses { get; set; }

        public ElementClass()
        {
            elementName = "";
            childElementClasses = new List<ElementClass>();
        }

        public ElementClass(String elementName)
        {
            this.elementName = elementName;
            childElementClasses = new List<ElementClass>();
        }

        public Boolean HasChild (String childElementName)
        {
            if (childElementClasses.Count == 0)
                return false;
            foreach (ElementClass childElement in childElementClasses)
                if (childElement.elementName.Equals(childElementName))
                    return true;
            return false;
        }
    }
}
