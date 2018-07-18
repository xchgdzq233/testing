using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fundamentals
{
    [TestClass]
    public class UnitTest1
    {
        private List<int> spiralOrder(List<List<int>> A)
        {
            List<int> result = new List<int>();

            for (int x = 0; x <= A.Count / 2; x++)
            {
                for (int y = x; y < A[0].Count - x && result.Count != A.Count * A[0].Count; y++)
                    result.Add(A[x][y]);
                for (int y = x + 1; y < A.Count - 1 - x && result.Count != A.Count * A[0].Count; y++)
                    result.Add(A[y][A[0].Count - 1 - x]);
                for (int y = A[0].Count - 1 - x; y >= x && result.Count != A.Count * A[0].Count; y--)
                    result.Add(A[A.Count - 1 - x][y]);
                for (int y = A.Count - 1 - x - 1; y > x && result.Count != A.Count * A[0].Count; y--)
                    result.Add(A[y][x]);
            }

            return result;
        }

        [TestMethod]
        public void TestMethod1()
        {
            List<int> result = this.spiralOrder(new List<List<int>> { new List<int>() { 11 }, new List<int>() { 21 }, new List<int>() { 31 }, new List<int>() { 41 } });
            Console.WriteLine();
        }
    }
}
