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
            {
                if (c != ' ')
                {
                    sb.AppendFormat(String.Format("Give me {0} {1}! {1}!\n", s.Contains(c) ? "an" : "a", c));
                }
            }
            sb.AppendLine("Who's the brithday gril?");
            for (int i = 0; i < 3; i++)
            {
                sb.AppendFormat("{0}!\n", name);
            }

            return sb.ToString();
        }

        private void ReplaceExtraParentheses(char[] input)
        {
            Stack<int> s = new Stack<int>();

            for(int i = 0; i < input.Length; i++)
            {
                if(input[i] == '(')
                {
                    s.Push(i);
                }
                else if(input[i] == ')')
                {
                    if(s.Count == 0)
                    {
                        input[i] = '*';
                    }
                    else
                    {
                        s.Pop();
                    }
                }
            }

            while(s.Count != 0)
            {
                input[s.Pop()] = '*';
            }
        }

        [Test]
        public void TestMethod()
        {


            //char[] input1 = ")(asdf)))".ToArray();
            //ReplaceExtraParentheses(input1);
            //char[] input2 = "((((asdf)))".ToArray();
            //ReplaceExtraParentheses(input2);
            //char[] input3 = "((((asdf))".ToArray();
            //ReplaceExtraParentheses(input3);
            //char[] input4 = "(ab)((cd)(asdf)))".ToArray();
            //ReplaceExtraParentheses(input4);
            //char[] input5 = "(ab)((cd)(asdf)())".ToArray();
            //ReplaceExtraParentheses(input5);
            //char[] input6 = "(ab)(((cd)(asdf)".ToArray();
            //ReplaceExtraParentheses(input6);
            //char[] input7 = "(ab)(((cd)(asdf".ToArray();
            //ReplaceExtraParentheses(input7);
            //char[] input8 = "(ab)(((cd)asdf)))))".ToArray();
            //ReplaceExtraParentheses(input8);


            //Assert.Multiple(() =>
            //{
            //    Assert.That(input1, Is.EqualTo("*(asdf)**".ToCharArray()));
            //    Assert.That(input2, Is.EqualTo("*(((asdf)))".ToCharArray()));
            //    Assert.That(input3, Is.EqualTo("**((asdf))".ToCharArray()));
            //    Assert.That(input4, Is.EqualTo("(ab)((cd)(asdf))*".ToCharArray()));
            //    Assert.That(input5, Is.EqualTo("(ab)((cd)(asdf)())".ToCharArray()));
            //    Assert.That(input6, Is.EqualTo("(ab)**(cd)(asdf)".ToCharArray()));
            //    Assert.That(input7, Is.EqualTo("(ab)**(cd)*asdf".ToCharArray()));
            //    Assert.That(input8, Is.EqualTo("(ab)(((cd)asdf))***".ToCharArray()));
            //});

            //Console.WriteLine(CheerForBDay("wang lin"));
        }
    }
}
