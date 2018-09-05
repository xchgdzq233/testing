using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundamentals
{
    [TestFixture]
    public class TestDynamicProgramming
    {
        #region "get largest plus sign"
        private int GetLargestPlusSign(int N, int[,] mines)
        {
            int maxArm = (N - 1) / 2;
            for (int arm = maxArm; arm >= 0; arm--)
            {
                List<Tuple<int, int>> xys = GetAllAxis(N, arm);
                if (!AxisOnMines(xys, mines, arm))
                    return arm + 1;
            }

            return 0;
        }

        private List<Tuple<int, int>> GetAllAxis(int N, int arm)
        {
            List<Tuple<int, int>> xys = new List<Tuple<int, int>>();
            for (int x = arm; x <= N - arm - 1; x++)
                for (int y = arm; y <= N - arm - 1; y++)
                    xys.Add(new Tuple<int, int>(x, y));
            return xys;
        }

        private Boolean AxisOnMines(List<Tuple<int, int>> xys, int[,] mines, int arm)
        {
            foreach (Tuple<int, int> xy in xys)
            {
                bool mineOnAxis = false;

                for (int i = 0; i < mines.GetLength(0); i++)
                    if ((mines[i, 0] == xy.Item1 && (mines[i, 1] >= xy.Item2 - arm && mines[i, 1] <= xy.Item2 + arm)) || (mines[i, 1] == xy.Item2 && (mines[i, 0] >= xy.Item1 - arm && mines[i, 0] <= xy.Item2 + arm)))
                    {
                        mineOnAxis = true;
                        break;
                    }

                if (!mineOnAxis) return false;
            }

            return true;
        }
        #endregion

        [Test]
        public void TestMethod()
        {
            #region "get largest plus sign"
            Assert.That(this.GetLargestPlusSign(5, new int[,] { { 0, 0 }, { 0, 3 }, { 1, 1 }, { 1, 4 }, { 2, 3 }, { 3, 0 }, { 4, 2 } }), Is.EqualTo(2));
            Assert.That(this.GetLargestPlusSign(2, new int[,] { { 0, 0 }, { 0, 1 }, { 1, 0 } }), Is.EqualTo(1));
            Assert.That(this.GetLargestPlusSign(2, new int[,] { }), Is.EqualTo(1));
            Assert.That(this.GetLargestPlusSign(1, new int[,] { { 0, 0 } }), Is.EqualTo(0));
            Assert.That(this.GetLargestPlusSign(2, new int[,] { }), Is.EqualTo(1));
            Assert.That(this.GetLargestPlusSign(5, new int[,] { { 4, 2 } }), Is.EqualTo(2));
            #endregion
        }
    }
}
