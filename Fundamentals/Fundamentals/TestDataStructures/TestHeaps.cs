using NUnit.Framework;
using System.Collections.Generic;

namespace Fundamentals.TestDataStructures
{
    [TestFixture]
    public class TestHeaps
    {
        #region "get distinct number in window"
        private List<int> GetDistinctNumberInWindow(List<int> A, int B)
        {
            List<int> result = new List<int>();
            if (B > A.Count)
                return result;

            Queue<int> q = new Queue<int>();
            Dictionary<int, int> d = new Dictionary<int, int>();
            int i = 0;
            for (; i <= B - 1; i++)
            {
                q.Enqueue(A[i]);
                if (!d.ContainsKey(A[i]))
                    d.Add(A[i], 1);
                else
                    d[A[i]]++;
            }

            result.Add(d.Count);
            for (; i <= A.Count - 1; i++)
            {
                int digit = q.Dequeue();
                d[digit]--;
                if (d[digit] == 0) d.Remove(digit);

                q.Enqueue(A[i]);
                if (!d.ContainsKey(A[i]))
                    d.Add(A[i], 1);
                else
                    d[A[i]]++;

                result.Add(d.Count);
            }

            return result;
        }
        #endregion

        [Test]
        public void TestMethod()
        {
            #region "get distinct number in window"
            Assert.That(this.GetDistinctNumberInWindow(new List<int>() { 1, 2, 1, 3, 4, 3 }, 3), Is.EqualTo(new List<int>() { 2, 3, 3, 2 }));
            #endregion
        }
    }
}
