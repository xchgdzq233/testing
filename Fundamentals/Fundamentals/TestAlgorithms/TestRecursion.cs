using NUnit.Framework;
using System;

namespace Fundamentals.TestAlgorithms
{
    [TestFixture]
    public class TestRecursion
    {
        #region "Tower Of Hanoi"
        public void TowerOfHanoi(int n, char from_rod, char to_rod, char aux_rod)
        {
            if (n == 1)
            {
                Console.WriteLine("Move disk 1 from rod " + from_rod + " to rod " + to_rod);
                return;
            }
            TowerOfHanoi(n - 1, from_rod, aux_rod, to_rod);
            Console.WriteLine("Move disk " + n + " from rod " + from_rod + " to rod " + to_rod);
            TowerOfHanoi(n - 1, aux_rod, to_rod, from_rod);
        }
        #endregion

        [Test]
        public void TestRecursionMethod()
        {
            #region "Tower Of Hanoi"
            this.TowerOfHanoi(3, 'a', 'c', 'b');
            #endregion
        }
    }
}
