using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundamentals.TestOnlineJudges
{
    [TestFixture]
    public class TestGeeksForGeeks
    {
        #region "Check for Amicable Pair"
        private bool CheckForAmicablePair(int m, int n)
        {
            return GetAmicableNum(m) == n && GetAmicableNum(n) == m;
        }

        private int GetAmicableNum(int num)
        {
            int sum = 1;
            for (int i = 2; i * i <= num; ++i)
            {
                if (num % i == 0)
                {
                    sum += i;
                    if (i * i != num)
                    {
                        sum += num / i;
                    }
                }
            }
            return sum;
        }
        #endregion

        #region "Pairs of Amicable Numbers"
        private List<int[]> PairsOfAmicableNumbers(int[] input)
        {
            List<int[]> result = new List<int[]>();

            Dictionary<int, int> map = new Dictionary<int, int>();
            for(int i = 0; i < input.Length; ++i)
            {
                int key = input[i];
                if (!map.ContainsKey(key))
                {
                    map.Add(key, i);
                }
            }

            int count = input.Length;
            for (int i = 0; i < count; ++i)
            {
                int num = input[i];
                int amicable = GetAmicableNum(num);
                if (map.ContainsKey(amicable))
                {
                    result.Add(new int[] { Math.Min(num, amicable), Math.Max(num, amicable) });
                    input[map[amicable]] = 0;
                    input[i] = 0;
                    map.Remove(num);
                    map.Remove(amicable);
                }
            }
            return result;
        }
        #endregion

        #region "Get Collatz Sequence"
        /// <summary>
        /// If n is even, then n = n / 2.
        /// If n is odd, then n = 3 * n + 1.
        /// Repeat above steps, until it becomes 1.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private List<int> GetCollatzSequence(int n)
        {
            List<int> res = new List<int>() { n };
            while(n > 1)
            {
                if (n % 2 == 0)
                {
                    n /= 2;
                }
                else
                {
                    n = 3 * n + 1;
                }
                res.Add(n);
            }
            return res;
        }
        #endregion

        private bool test (int x)
        {

            if (x < 0)
            {
                return false;
            }

            int size = x.ToString().Length;
            int left = x / (int)Math.Pow(10, (size + 1) / 2);
            string reversedLeft = new string(left.ToString().Reverse().ToArray());
            int left2 = Int32.Parse(reversedLeft);
            int right = x % (int)Math.Pow(10, size / 2);
            return left == right;
        }

        [Test]
        public void TestMethod()
        {
            #region "Get Collatz Sequence"
            //Assert.That(this.GetCollatzSequence(3), Is.EqualTo(new List<int>() { 3, 10, 5, 16, 8, 4, 2, 1 }));
            //Assert.That(this.GetCollatzSequence(6), Is.EqualTo(new List<int>() { 6, 3, 10, 5, 16, 8, 4, 2, 1 }));
            #endregion

            #region "Pairs of Amicable Numbers"
            //Assert.That(this.PairsOfAmicableNumbers(new int[] { 2620, 2924, 5020, 5564, 6232, 6368 }), Is.EquivalentTo(new List<int[]>() {
            //    new int[] { 2620, 2924 }, new int[] { 5020, 5564 }, new int[] { 6232, 6368 },
            //}));
            //Assert.That(this.PairsOfAmicableNumbers(new int[] { 220, 284, 1184, 1210, 2, 5 }), Is.EquivalentTo(new List<int[]>() {
            //    new int[] { 220, 284 }, new int[] { 1184, 1210 },  
            //}));
            #endregion

            #region "Check for Amicable Pair"
            //Assert.False(this.CheckForAmicablePair(4, 45612));
            //Assert.True(this.CheckForAmicablePair(220, 284));
            //Assert.True(this.CheckForAmicablePair(66928, 66992));
            //Assert.False(this.CheckForAmicablePair(66666, 45612));
            #endregion
        }
    }
}
