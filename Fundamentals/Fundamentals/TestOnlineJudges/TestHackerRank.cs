using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Fundamentals.TestOnlineJudges
{
    [TestFixture]
    public class TestHackerRank
    {
        #region "ABBCodeChallengesPractice1"
        private string[] ABBCodeChallengesPractice1_OneTime(string[] commands)
        {
            string[] result = new string[commands.Length];
            for (int i = 0; i < result.Length; i++)
                result[i] = IsCircle(commands[i]) ? "YES" : "NO";
            return result;
        }

        private bool IsCircle(string path)
        {
            int x = 0, y = 0;
            int dir = 0;

            // Traverse the path 
            // given for robot
            for (int i = 0; i < path.Length; i++)
            {
                // Find current move
                char move = path[i];

                // If move is left or
                // right, then change direction
                if (move == 'R')
                    dir = (dir + 1) % 4;
                else if (move == 'L')
                    dir = (4 + dir - 1) % 4;
                // If move is Go, then 
                // change x or y according to
                // current direction
                // if (move == 'G')
                else
                {
                    if (dir == 0)
                        y++;
                    else if (dir == 1)
                        x++;
                    else if (dir == 2)
                        y--;
                    else // dir == 3
                        x--;
                }
            }

            // If robot comes back to
            // (0, 0), then path is cyclic
            return (x == 0 && y == 0);
        }

        private string[] ABBCodeChallengesPractice1(string[] commands)
        {
            string[] result = new string[commands.Length];
            for (int i = 0; i < commands.Length; i++)
            {
                int x = 0, y = 0;
                int dX = 0, dY = 1;
                bool yes = false;

                for (int j = 0; j <= 4; j++)
                {
                    if (j == 1 || j == 2 || j == 4)
                    {
                        if (x == 0 && y == 0 && dX == 0 && dY == 1)
                        {
                            yes = true;
                            break;
                        }

                        if (j == 4)
                            break;
                    }

                    Tuple<int, int, int, int> ran = RunCommand(x, y, dX, dY, commands[i]);
                    x = ran.Item1;
                    y = ran.Item2;
                    dX = ran.Item3;
                    dY = ran.Item4;
                }

                result[i] = (yes) ? "YES" : "NO";
            }

            return result;
        }

        private static Tuple<int, int, int, int> RunCommand(int x, int y, int dX, int dY, string command)
        {
            foreach (char c in command.ToCharArray())
            {
                if (c == 'G')
                {
                    Tuple<int, int> move = Move(x, y, dX, dY, 1);
                    x = move.Item1;
                    y = move.Item2;
                }
                else
                {
                    Tuple<int, int> rotate = Rotate(dX, dY, c);
                    dX = rotate.Item1;
                    dY = rotate.Item2;
                }
            }
            return new Tuple<int, int, int, int>(x, y, dX, dY);
        }

        private static Tuple<int, int> Rotate(int dX, int dY, char d)
        {
            if (d == 'L')
            {
                if (dX == 0)
                    return new Tuple<int, int>(dY == 1 ? -1 : 1, 0);
                return new Tuple<int, int>(0, dX == 1 ? 1 : -1);
            }

            if (dX == 0)
                return new Tuple<int, int>(dY == 1 ? 1 : -1, 0);
            return new Tuple<int, int>(0, dX == 1 ? -1 : 1);
        }

        private static Tuple<int, int> Move(int x, int y, int dX, int dY, int step)
        {
            return new Tuple<int, int>(x + dX * step, y + dY * step);
        }
        #endregion

        #region "ABBCodeChallengesPractice12"
        private List<int> ABBCodeChallengesPractice12(List<float> prices, int target)
        {
            float fSum = 0;
            int iSum = 0;
            int[] floors = new int[prices.Count];
            for (int i = 0; i < prices.Count; i++)
            {
                floors[i] = (int)Math.Floor((decimal)prices[i]);
                iSum += floors[i];
                fSum += prices[i];
            }

            Tuple<float, List<int>> result = RoundPricesToMatchTargetSub(fSum, target, floors, new int[floors.Length], 0, 0, target - iSum, new Tuple<float, List<int>>(float.MaxValue, new List<int>()));
            foreach (int i in result.Item2)
                Console.WriteLine(i);

            return result.Item2;
        }

        private Tuple<float, List<int>> RoundPricesToMatchTargetSub(float fSum, int target, int[] floors, int[] current, int curSum, int index, int diff, Tuple<float, List<int>> min)
        {
            if (index == floors.Length)
            {
                float fDiff = Math.Abs(fSum - curSum);
                if (fDiff < min.Item1)
                    return new Tuple<float, List<int>>(fDiff, new List<int>(current));
                return min;
            }

            float fMin = min.Item1;
            List<int> listMin = min.Item2;

            if (floors.Length - index > diff)
            {
                current[index] = floors[index];
                Tuple<float, List<int>> result = RoundPricesToMatchTargetSub(fSum, target, floors, current, curSum + current[index], index + 1, diff, min);
                if (result.Item1 < fMin)
                {
                    fMin = result.Item1;
                    listMin = result.Item2;
                }
            }
            if (diff > 0)
            {
                current[index] = floors[index] + 1;
                Tuple<float, List<int>> result = RoundPricesToMatchTargetSub(fSum, target, floors, current, curSum + current[index], index + 1, diff - 1, min);
                if (result.Item1 < fMin)
                {
                    fMin = result.Item1;
                    listMin = result.Item2;
                }
            }
            return new Tuple<float, List<int>>(fMin, listMin);
        }

        // DFS
        private List<int> ABBCodeChallengesPractice12Dfs(List<float> prices, int target)
        {
            List<int> preRound = new List<int>();
            float fSum = 0;
            foreach (float f in prices)
            {
                preRound.Add((int)Math.Floor((decimal)f));
                fSum += f;
            }

            Tuple<float, List<int>> result = RoundPricesToMatchTargetDfs(fSum, target, preRound, new List<int>(), 0, new Tuple<float, List<int>>(float.MaxValue, new List<int>()));

            foreach (int i in result.Item2)
                Console.WriteLine(i);

            return result.Item2;
        }

        private Tuple<float, List<int>> RoundPricesToMatchTargetDfs(float fSum, int target, List<int> preRound, List<int> current, int curSum, Tuple<float, List<int>> min)
        {
            if (current.Count == preRound.Count)
            {
                if (curSum != target) return min;

                float diff = Math.Abs(fSum - curSum);
                if (diff < min.Item1)
                    return new Tuple<float, List<int>>(diff, new List<int>(current));
                return min;
            }

            int index = current.Count;
            current.Add(preRound[index]);
            Tuple<float, List<int>> floor = RoundPricesToMatchTargetDfs(fSum, target, preRound, current, curSum + current[index], min);
            current[index]++;
            Tuple<float, List<int>> ceiling = RoundPricesToMatchTargetDfs(fSum, target, preRound, current, curSum + current[index], floor);
            current.RemoveAt(index);
            return ceiling;
        }
        #endregion

        [Test]
        public void TestAirbnb()
        {
            #region "ABBCodeChallengesPractice12"
            //Assert.That(this.ABBCodeChallengesPractice12(new List<float>() { 0.7f, 2.8f, 4.9f }, 8), Is.EqualTo(new List<int>() { 0, 3, 5 }));
            //Assert.That(this.ABBCodeChallengesPractice12(new List<float>() { 1.0f, 2.0f, 4.0f }, 7), Is.EqualTo(new List<int>() { 1, 2, 4 }));
            //Assert.That(this.ABBCodeChallengesPractice12(new List<float>() { 0.1f, 2.1f, 4.1f }, 7), Is.EqualTo(new List<int>() { 0, 2, 5 }));
            #endregion

            #region "ABBCodeChallengesPractice1"
            //Assert.That(this.ABBCodeChallengesPractice1(new string[] { "GGGGR", "RGL" }), Is.EqualTo(new string[] { "YES", "NO" }));
            //Assert.That(this.ABBCodeChallengesPractice1(new string[] { "GRGRGRGRR" }), Is.EqualTo(new string[] { "YES" }));
            //Assert.That(this.ABBCodeChallengesPractice1(new string[] { "G", "L" }), Is.EqualTo(new string[] { "NO", "YES" }));
            //Assert.That(this.ABBCodeChallengesPractice1(new string[] { "GRGL" }), Is.EqualTo(new string[] { "NO" }));
            #endregion
        }

        #region "1"
        private int _1ServiceTitan(long num)
        {
            int count = 0;
            for (long div = 2; div <= num / 2; ++div)
            {
                long middle = num / div;
                if (div % 2 != 0)
                {
                    if (middle * div == num)
                    {
                        count++;
                    }
                }
                else
                {
                    if (middle < (div - 1) / 2 + 1)
                    {
                        break;
                    }
                    long sum = middle * ((div - 1) / 2 * 2 + 1);
                    if (sum + middle - (div - 1) / 2 - 1 == num)
                    {
                        count++;
                    }
                    if (sum + middle + (div - 1) / 2 + 1 == num)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int _1ServiceTitanTimeOut(long num)
        {
            long runningSum = 0, start = 1, end;
            for (end = 1; end < num; ++end)
            {
                runningSum += end;
                if (runningSum >= num)
                {
                    break;
                }
            }
            
            int count = 0;
            while (start < end)
            {
                if (runningSum == num)
                {
                    count++;
                    runningSum += ++end;
                }
                else if (runningSum > num)
                {
                    runningSum -= start++;
                }
                else
                {
                    runningSum += ++end;
                }
            }

            return count;
        }
        #endregion

        [Test]
        public void TestServiceTitan()
        {
            #region "1"
            //Assert.That(this._1ServiceTitan(10), Is.EqualTo(1));
            //Assert.That(this._1ServiceTitan(250), Is.EqualTo(3));
            //Assert.That(this._1ServiceTitan(5050), Is.EqualTo(5));
            #endregion
        }
    }
}
