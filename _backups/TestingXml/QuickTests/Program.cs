using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QuickTests
{
    class Program
    {
        static void Main(string[] args)
        {
            String xml = @"<tr><td>State Board of Education</td><td>Administration of Medications by School Personnel and Administration of Medication During Before-and After-School Programs and School Readiness Programs</td><td>--</td><td>10/09/2014</td><td><div>See Notice of Intent Posted to this Page</div></td><td><p><a>Proposed Regulation</a></p><p><br/></p></td></tr>";
            XDocument doc = XDocument.Parse(xml);

            //String output = @"C:\Github\testing\TestingXml\TestingXml\bin\Debug\styling-output.xml";
            //XDocument doc = XDocument.Load(output);

            Console.WriteLine("before:\n{0}", doc.ToString());
            List<XElement> result = doc.Element("tr").Elements("td").Cast<XElement>().ToList();
            doc.Descendants().Where(e => String.IsNullOrEmpty(e.Value.Trim()) && !e.Name.LocalName.Equals("td")).Remove();

            Console.WriteLine("\nafter:\n{0}", doc.ToString());

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
