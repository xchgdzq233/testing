using log4net;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundamentals.TestDataStructures
{
    [TestFixture]
    public class TestStrings
    {
        private static ILog logger = LogManager.GetLogger(typeof(TestStrings));

        #region "add binary strings"
        private String AddBinaryStrings(String A, String B)
        {
            StringBuilder sb = new StringBuilder();
            int carry = 0;
            for (int i = A.Length - 1, j = B.Length - 1; i >= 0 || j >= 0; i--, j--)
            {
                int digit = carry;

                if (i >= 0)
                    if (A[i] == '1')
                        digit++;

                if (j >= 0)
                    if (B[j] == '1')
                        digit++;

                carry = digit / 2;
                digit = digit % 2;
                sb.Append(digit.ToString());
            }

            if (carry == 1)
                sb.Append(carry.ToString());

            return new string(sb.ToString().ToCharArray().Reverse().ToArray());
        }
        #endregion

        #region "get unique permutations"
        private List<List<int>> GetUniquePermutations(List<int> A)
        {
            Dictionary<int, int> d = new Dictionary<int, int>();
            foreach (int i in A)
                AddNewOrIncreaseOld(d, i);

            List<List<int>> result = new List<List<int>>();
            GetUniquePermutation(d, new List<int>() { }, result);

            StringBuilder sb = new StringBuilder();
            foreach (List<int> row in result)
            {
                foreach (int i in row)
                    sb.AppendFormat("{0} ", i);
                sb.AppendLine();
            }
            logger.InfoFormat("Printing results... \n{0}", sb.ToString());

            return result;
        }

        private void AddNewOrIncreaseOld(Dictionary<int, int> d, int i)
        {
            if (!d.ContainsKey(i))
                d.Add(i, 1);
            else
                d[i]++;
        }

        private void GetUniquePermutation(Dictionary<int, int> d, List<int> current, List<List<int>> result)
        {
            if (d.Count == 0)
            {
                result.Add(new List<int>(current));
                return;
            }

            List<int> keys = d.Keys.ToList();
            foreach (int key in keys)
            {
                d[key]--;
                if (d[key] == 0)
                    d.Remove(key);
                current.Add(key);
                GetUniquePermutation(d, current, result);
                current.RemoveAt(current.Count - 1);
                AddNewOrIncreaseOld(d, key);
            }

            return;
        }
        #endregion

        #region "get binary tree levels"
        private List<List<int>> GetBinaryTreeLevels(TreeNode A)
        {
            Dictionary<int, List<int>> d = new Dictionary<int, List<int>>();
            GetNodesInDictionary(A, 0, d);
            List<List<int>> result = new List<List<int>>();
            int height = 0;
            while(d.ContainsKey(height))
            {
                result.Add(d[height]);
                height++;
            }
            return result;
        }

        private void GetNodesInDictionary(TreeNode A, int height, Dictionary<int, List<int>> d)
        {
            if (A == null) return;

            CreateOrAddInt(A.val, height, d);
            GetNodesInDictionary(A.left, height + 1, d);
            GetNodesInDictionary(A.right, height + 1, d);

            return;
        }

        private void CreateOrAddInt(int i, int height, Dictionary<int, List<int>> d)
        {
            if (!d.ContainsKey(height))
                d.Add(height, new List<int>());
            d[height].Add(i);

            return;
        }
        #endregion

        [Test]
        public void TestMethod()
        {

            #region "get binary tree levels"
            //Assert.That(this.GetBinaryTreeLevels(new TreeNode(new List<int>() { 3, 9, 20, 15, 7 })), Is.EqualTo(new List<List<int>>()
            //{
            //    new List<int>(){3},
            //    new List<int>(){9, 20},
            //    new List<int>(){15, 7}
            //}));
            #endregion

            #region "get unque permutations"
            //Assert.That(this.GetUniquePermutations(new List<int>() { 1, 2, 3 }), Is.EqualTo(new List<List<int>>()
            //{
            //    new List<int>(){ 1, 2, 3 },
            //    new List<int>(){1, 3, 2},
            //    new List<int>(){2, 1, 3},
            //    new List<int>(){2, 3, 1},
            //    new List<int>(){3, 1, 2},
            //    new List<int>(){3, 2, 1}
            //}));
            //Assert.That(this.GetUniquePermutations(new List<int>() { 1, 1, 2 }), Is.EqualTo(new List<List<int>>()
            //{
            //    new List<int>(){1, 1, 2},
            //    new List<int>(){1, 2, 1},
            //    new List<int>(){2, 1, 1}
            //}));
            #endregion

            #region "add binary strings"
            //Assert.That(this.AddBinaryStrings("1110000000010110111010100100111", "101001"), Is.EqualTo("11100000000101101110101001010000"));
            //Assert.That(this.AddBinaryStrings("10000", "11"), Is.EqualTo("10011"));
            //Assert.That(this.AddBinaryStrings("100", "11"), Is.EqualTo("111"));
            #endregion
        }
    }
}
