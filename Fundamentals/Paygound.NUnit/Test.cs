using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Paygound.NUnit
{
    [TestFixture()]
    public class Test
    {
        [Test()]
        public void TestCase()
        {
            List<int> A = new List<int>() { 0, 1, 2, 5 };
            int B = 2;
            int C = 511;

            int result = 0;
            if (A[0] == 0 && C > 0 && C < 10)
                result += 1;

            result += FindSub(A, C.ToString(), 0);

            Assert.That(result, Is.EqualTo(37));
        }

        public int FindSub(List<int> A, String C, int i)
        {
            if (i >= C.Length) return 0;

            int zero = 0, less = 0, j = 0;

            for (; j <= A.Count - 1 && A[j] < C[i]; j++)
            {
                if (A[j] == 0) zero++;
                less++;
            }

            if (i == 0 && C.Length > 1) less -= zero;

            int result = (int)Math.Pow(A.Count, C.Length - 1 - i) * less;

            if (j <= A.Count - 1)
                if (A[j] == C[i])
                    result += FindSub(A, C, i + 1);

            return result;
        }
    }
}
