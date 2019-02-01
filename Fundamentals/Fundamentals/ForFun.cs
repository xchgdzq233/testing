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

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '(')
                {
                    s.Push(i);
                }
                else if (input[i] == ')')
                {
                    if (s.Count == 0)
                    {
                        input[i] = '*';
                    }
                    else
                    {
                        s.Pop();
                    }
                }
            }

            while (s.Count != 0)
            {
                input[s.Pop()] = '*';
            }
        }

        private bool IsS1RotationOfS2(string s1, string s2)
        {
            // check edge cases
            if (s1 == null || s2 == null)
            {
                return false;
            }
            if (s1.Equals(s2))
            {
                return true;
            }

            int len = s1.Length;
            if (len != s2.Length)
            {
                return false;
            }

            for (int i = 0; i < len - 1; i++)
            {
                // found potential rotation point
                if (s2[i] == s1[len - 1] && s2[i + 1] == s1[0])
                {
                    string newS2 = new StringBuilder().Append(s2.Substring(i + 1, len - 1 - i)).Append(s2.Substring(0, i + 1)).ToString();
                    if (newS2.Equals(s1))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private decimal AddWithoutFloat(decimal a, decimal b)
        {
            string[] s1 = a.ToString().Split(new char[] { '.' });
            string[] s2 = b.ToString().Split(new char[] { '.' });

            string a1 = s1[0],
                a2 = s1.Length == 2 ? s1[1] : "",
                b1 = s2[0],
                b2 = s2.Length == 2 ? s2[1] : "";
            int len = (int)Math.Max(a2.Length, b2.Length);

            int right = Int32.Parse(a2.PadRight(len, '0')) + Int32.Parse(b2.PadRight(len, '0'));
            int divider = (int)Math.Pow(10, len);
            int left = Int32.Parse(a1) + Int32.Parse(b1) + right / divider;
            right %= divider;

            return Decimal.Parse(String.Format("{0}.{1}", left, right));
        }

        private List<string> Splits(int input)
        {
            List<string> result = new List<string>();
            string s = input.ToString();
            int len = s.Length;
            Queue<Tuple<string, StringBuilder>> q = new Queue<Tuple<string, StringBuilder>>();
            q.Enqueue(new Tuple<string, StringBuilder>(s, new StringBuilder()));

            while (q.Count != 0)
            {
                Tuple<string, StringBuilder> t = q.Dequeue();
                if (t.Item1.Length == 0)
                {
                    result.Add(t.Item2.ToString());
                }
                else
                {
                    int index = 0;
                    for (int i = 0; i < len; i++)
                    {
                        if (index > 26)
                        {
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private List<Dictionary<string, int>> BuyFruits(int money, Dictionary<string, int> prices)
        {
            this.money = money;
            this.prices = prices;
            fruits = prices.Keys.ToArray();
            result = new List<Dictionary<string, int>>();
            BuyMyFruits(new List<string>(), 0, 0);
            return result;
        }

        private int money;
        private Dictionary<string, int> prices;
        private string[] fruits;
        private List<Dictionary<string, int>> result;

        private void BuyMyFruits(List<string> cur, int sum, int index)
        {
            if (sum > money)
            {
                return;
            }
            if (sum == money)
            {
                Dictionary<string, int> map = new Dictionary<string, int>();
                foreach (string fruit in cur)
                {
                    if (!map.ContainsKey(fruit))
                    {
                        map.Add(fruit, 1);
                    }
                    else
                    {
                        map[fruit]++;
                    }
                }
                result.Add(map);
                return;
            }

            for(int i = index; i < fruits.Length; i++)
            {
                cur.Add(fruits[i]);
                BuyMyFruits(cur, sum + prices[fruits[i]], i);
                cur.RemoveAt(cur.Count - 1);
            }
        }

        [Test]
        public void TestMethod()
        {
            //Assert.That(BuyFruits(500, new Dictionary<string, int> { { "banana", 32 }, { "kiwi", 41 }, { "mango", 97 }, { "papaya", 254 }, { "pineapple", 399 } }), Is.EquivalentTo(new List<Dictionary<string, int>>() {  }));
            List<Dictionary<string, int>> result = BuyFruits(500, 
                new Dictionary<string, int> { { "banana", 32 }, { "kiwi", 41 }, { "mango", 97 }, { "papaya", 254 }, { "pineapple", 399 } });
            Console.WriteLine();

            //Assert.That(this.AddWithoutFloat(1.3m, 2.85m), Is.EqualTo(4.15m));

            //Assert.True(this.IsS1RotationOfS2("developers", "evelopersd"));
            //Assert.True(this.IsS1RotationOfS2("developers", "velopersde"));
            //Assert.True(this.IsS1RotationOfS2("developers", "lopersdeve"));
            //Assert.False(this.IsS1RotationOfS2("developers", "develoserp"));

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
