using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundamentals.TestOnlineJudges
{
    [TestFixture]
    public class TestCodility
    {
        #region "demo test"
        private int solution(int[] A)
        {
            // write your code in C# 6.0 with .NET 4.5 (Mono)
            bool[] map = new bool[100000];
            foreach (int i in A)
            {
                if (i > 0)
                {
                    map[i - 1] = true;
                }
            }

            for (int i = 0; i < 100000; ++i)
            {
                if (!map[i])
                {
                    return i + 1;
                }
            }

            return 100001;
        }
        #endregion

        #region "socure 1"
        private int _1Socure(int N)
        {
            // write your code in C# 6.0 with .NET 4.5 (Mono)
            int[] map = new int[10];

            int input = N;
            while (input > 0)
            {
                map[input % 10]++;
                input /= 10;
            }

            int result = 0;
            for (int i = 9; i >= 0; --i)
            {
                int num = map[i];
                if (num > 0)
                {
                    for (int j = 0; j < num; j++)
                    {
                        result = result * 10 + i;
                    }
                }
            }
            return result;
        }
        #endregion

        #region "socure 3"
        private int ToBinary(int n)
        {
            int[] d = new int[30];
            int l = 0;
            while (n > 0)
            {
                d[29-l] = n % 2;
                n /= 2;
                l++;
            }
            int p;
            for (p = 1; p < 1 + l; ++p)
            {
                int i;
                bool ok = true;
                for (i = 30 - l; i + p < 30; ++i)
                {
                    if (d[i] != d[i + p])
                    {
                        ok = false;
                        break;
                    }
                }
                if (ok)
                {
                    return p;
                }
            }
            return -1;
        }

        private int CheckRecursive(char[] d)
        {
            int p, l = d.Length;
            for (p = 1; p < 1 + l; ++p)
            {
                int i;
                bool ok = true;
                for (i = l - 1; i > l / 2; --i)
                {
                    if (d[i] != d[i - p])
                    {
                        ok = false;
                        break;
                    }
                }
                if (ok)
                {
                    return p;
                }
            }
            return -1;
        }
        #endregion

        [Test]
        public void TestSocure()
        {
            #region "socure 3"
            //Assert.That(this.ToBinary(955), Is.EqualTo(1110111011));
            ////Assert.That(this.ToBinary(1), Is.EqualTo(-1));
            //Assert.That(this.ToBinary(1000000000), Is.EqualTo(-1));
            //Assert.That(this.ToBinary(955), Is.EqualTo(4));

            ////Assert.That(this.CheckRecursive("1".ToCharArray().Reverse().ToArray()), Is.EqualTo(-1));
            //Assert.That(this.CheckRecursive("10".ToCharArray().Reverse().ToArray()), Is.EqualTo(-1));
            ////Assert.That(this.CheckRecursive("111011100110101100101000000000".ToCharArray().Reverse().ToArray()), Is.EqualTo(4));
            //Assert.That(this.CheckRecursive("abcdabcd11".ToCharArray().Reverse().ToArray()), Is.EqualTo(4));
            //Assert.That(this.CheckRecursive("abcdabcd11".ToCharArray().Reverse().ToArray()), Is.EqualTo(4));
            ////Assert.That(this.CheckRecursive("abcdabcd11".ToCharArray()), Is.EqualTo(4));

            ////Assert.That(this.CheckRecursive("abracadabracadabra".ToCharArray()), Is.EqualTo(7));
            //Assert.That(this.CheckRecursive("abracadabracadabra".ToCharArray().Reverse().ToArray()), Is.EqualTo(7));
            ////Assert.That(this.CheckRecursive("codilitycodilityco".ToCharArray()), Is.EqualTo(8));
            //Assert.That(this.CheckRecursive("codilitycodilityco".ToCharArray().Reverse().ToArray()), Is.EqualTo(8));
            //Assert.That(this.CheckRecursive("1110111011".ToCharArray().Reverse().ToArray()), Is.EqualTo(4));
            #endregion

            #region "socure 1"
            //Assert.That(this._1Socure(0), Is.EqualTo(0));
            //Assert.That(this._1Socure(10000), Is.EqualTo(10000));
            //Assert.That(this._1Socure(111229), Is.EqualTo(922111));
            //Assert.That(this._1Socure(213), Is.EqualTo(321));
            //Assert.That(this._1Socure(553), Is.EqualTo(553));
            #endregion
        }
    }
}
