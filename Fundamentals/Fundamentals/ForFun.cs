using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundamentals
{
    [TestFixture]
    public class ForFun
    {
        public string CheerForBDay(string name)
        {
            string s = "aefhilmnorsxAEFHILMNORSX";
            StringBuilder sb = new StringBuilder();
            name = name.ToUpper();

            foreach (char c in name)
                if (c != ' ')
                    sb.AppendFormat(String.Format("Give me {0} {1}! {1}!\n", s.Contains(c) ? "an" : "a", c));
            sb.AppendLine("Who's the brithday gril?");
            for (int i = 0; i < 3; i++)
                sb.AppendFormat("{0}!\n", name);

            return sb.ToString();
        }

        [Test]
        public void TestMethod()
        {
            Console.WriteLine(CheerForBDay("wang lin"));
        }
    }
}
