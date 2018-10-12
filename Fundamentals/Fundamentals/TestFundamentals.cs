using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using NUnit.Framework;
using System.Linq;

namespace Fundamentals
{
    public class RomeChar
    {
        public char theChar { get; set; }
        public int value { get; set; }
        public char preChar { get; set; }
        public char nextChar { get; set; }
        public char next5 { get; set; }

        private RomeChar() { }

        public RomeChar(char theChar, int value, char preChar, char nextChar, char next5)
        {
            this.theChar = theChar;
            this.value = value;
            this.preChar = preChar;
            this.nextChar = nextChar;
            this.next5 = next5;
        }
    }

    public class ListNode
    {
        public int val { get; set; }
        public ListNode next { get; set; }

        private ListNode() { }

        public ListNode(int value)
        {
            this.val = value;
            this.next = null;
        }

        public ListNode(List<int> values)
        {
            ListNode tempNode = this;
            for (int i = 0; i <= values.Count - 1; i++)
            {
                tempNode.val = values[i];
                if (i != values.Count - 1)
                {
                    tempNode.next = new ListNode();
                    tempNode = tempNode.next;
                }
            }
        }

        public List<int> GetThisAndAfterInList()
        {
            List<int> result = new List<int>();
            ListNode current = this;
            while (current != null)
            {
                result.Add(current.val);
                current = current.next;
            }
            return result;
        }

        public String GetThisAndAfterInString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (int i in this.GetThisAndAfterInList())
                sb.Append(i + " ");
            return sb.ToString();
        }
    }

    [TestFixture]
    public class UnitTest1
    {
        private static ILog logger = LogManager.GetLogger(typeof(UnitTest1));

        #region "functions"

        private int FindSub(List<int> A, int B, int C, int i, int digit)
        {
            if (i >= B) return 0;

            digit = digit / (int)Math.Pow(10, B - 1 - i);

            int zero = 0, less = 0, j = 0;

            for (; j <= A.Count - 1 && A[j] < digit; j++)
            {
                if (A[j] == 0) zero++;
                less++;
            }

            if (i == 0 && B > 1) less -= zero;

            int result = (int)Math.Pow(A.Count, B - 1 - i) * less;

            if (j <= A.Count - 1)
                if (A[j] == digit)
                    result += FindSub(A, B, C, i + 1, C % (int)Math.Pow(10, B - 1 - i));

            return result;
        }

        private int FindGcd(int A, int B)
        {
            if (A == 0 || A == B) return B;
            if (B == 0) return A;
            int min = Math.Min(Math.Min(A, B), Math.Abs(A - B));
            for (int i = min; i >= 2; i--)
                if (A % i == 0 && B % i == 0) return i;
            return 1;
        }

        private String SayIt(String input)
        {
            char previous = 'a';
            int count = 0;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < input.Length - 1; i++)
            {
                if (!input[i].Equals(input[i + 1]))
                {
                    count++;
                    sb.Append(count.ToString());
                    sb.Append(input[i]);
                    previous = 'a';
                    count = 0;
                }
                else
                {
                    previous = input[i];
                    count++;
                }
            }

            count++;
            sb.Append(count.ToString());
            sb.Append(input[input.Length - 1]);

            return sb.ToString();
        }

        private String IterateSayIt(int num)
        {
            if (num == 1) return "1";

            String last = "1";
            for (int i = 2; i <= num; i++)
                last = this.SayIt(last);

            return last;
        }

        private int titleToNumber(string A)
        {
            int result = 0;

            for (int i = 0; i <= A.Length - 1; i++)
            {
                char c = A[A.Length - 1 - i];
                int digit = (int)c - (int)'A' + 1;
                result += digit * (int)Math.Pow(26, i);
            }

            return result;
        }

        private List<int> spiralOrder(List<List<int>> input)
        {
            List<int> result = new List<int>();

            for (int i = 0; i <= (input.Count - 1) / 2 && i <= (input[0].Count - 1) / 2; i++)
            {
                result.Add(input[i][i]);

                for (int j = i + 1; j < input[0].Count - i; j++)
                    result.Add(input[i][j]);
                for (int j = i + 1; j < input.Count - i; j++)
                    result.Add(input[j][input[0].Count - 1 - i]);

                if (((i == (input.Count - 1) / 2) && (input.Count % 2 == 1)) || ((i == (input[0].Count - 1) / 2) && (input[0].Count % 2 == 1)))
                    break;

                for (int j = input[0].Count - i - 2; j >= 0 + i; j--)
                    result.Add(input[input.Count - 1 - i][j]);
                for (int j = input.Count - i - 2; j > 0 + i; j--)
                    result.Add(input[j][i]);
            }

            return result;
        }

        private String GetBinary(int A)
        {
            String result = "";
            while (A > 0)
            {
                result = (A % 2).ToString() + result;
                A = A / 2;
            }

            return result;
        }

        private long SumOfPairwiseHammingDistance(List<int> A)
        {
            List<long> ones = new List<long>();

            for (int i = 0; i <= A.Count - 1; i++)
            {
                String binary = this.GetBinary(A[i]);

                for (int j = 1; j <= binary.Length; j++)
                {
                    if (j > ones.Count)
                        ones.Add(0);

                    if (binary[binary.Length - j] == '1')
                        ones[j - 1]++;
                }
            }

            long sum = 0;
            foreach (int n in ones)
                sum += (A.Count - n) * n * 2 % 1000000007;

            return sum;
        }

        #endregion

        //[Test]
        public void TestMethod1()
        {
            #region "1"
            //List<List<List<int>>> inputs = new List<List<List<int>>>()
            //{
            //    new List<List<int>>() { new List<int>() { 11 } },

            //    new List<List<int>>() { new List<int>() { 11, 12 } },
            //    new List<List<int>>() { new List<int>() { 11 }, new List<int>() { 21 } },

            //    new List<List<int>>() { new List<int>() { 11, 12 }, new List<int>() { 21, 22 } },

            //    new List<List<int>>() { new List<int>() { 11, 12, 13 } },
            //    new List<List<int>>() { new List<int>() { 11 }, new List<int>() { 21 }, new List<int>() { 31 } },

            //    new List<List<int>>() { new List<int>() { 11, 12, 13 }, new List<int>() { 21, 22, 23 } },
            //    new List<List<int>>() { new List<int>() { 11, 12 }, new List<int>() { 21, 22 }, new List<int>() { 31, 32 } },

            //    new List<List<int>>() { new List<int>() { 11, 12, 13 }, new List<int>() { 21, 22, 23 }, new List<int>() { 31, 32, 33 } },

            //    new List<List<int>>()
            //    {
            //        new List<int>() { 335, 401, 128, 384, 345, 275, 324, 139, 127, 343, 197, 177, 127, 72, 13, 59 },
            //        new List<int>() { 102, 75, 151, 22, 291, 249, 380, 151, 85, 217, 246, 241, 204, 197, 227, 96 },
            //        new List<int>() { 261, 163, 109, 372, 238, 98, 273, 20, 233, 138, 40, 246, 163, 191, 109, 237 },
            //        new List<int>() { 179, 213, 214, 9, 309, 210, 319, 68, 400, 198, 323, 135, 14, 141, 15, 168 },
            //    }
            //};

            //List<List<int>> outputs = new List<List<int>>()
            //{
            //    new List<int>() { 11 },

            //    new List<int>() { 11, 12},
            //    new List<int>() { 11, 21},

            //    new List<int>() { 11, 12, 22, 21},

            //    new List<int>() { 11, 12, 13 },
            //    new List<int>() { 11, 21, 31 },

            //    new List<int>() { 11, 12, 13, 23, 22, 21 },
            //    new List<int>() { 11, 12, 22, 32, 31, 21 },

            //    new List<int>() { 11, 12, 13, 23, 33, 32, 31, 21, 22 },

            //    new List<int>() { 335, 401, 128, 384, 345, 275, 324, 139, 127, 343, 197, 177, 127, 72, 13, 59, 96, 237, 168, 15, 141, 14, 135, 323, 198, 400, 68, 319, 210, 309, 9, 214, 213, 179, 261, 102, 75, 151, 22, 291, 249, 380, 151, 85, 217, 246, 241, 204, 197, 227, 109, 191, 163, 246, 40, 138, 233, 20, 273, 98, 238, 372, 109, 163 }
            //};

            //for (int i = 0; i < inputs.Count; i++)
            //    Assert.That(this.spiralOrder(inputs[i]), Is.EquivalentTo(outputs[i]));
            #endregion

            #region "2"
            //int A = 5;
            //List<List<int>> result = new List<List<int>>();

            //for (int i = 0; i <= A - 1; i++)
            //{
            //    result.Add(new List<int>());
            //    result[i].Add(1);
            //    for (int j = 1; j < i; j++)
            //        result[i].Add(result[i - 1][j - 1] + result[i - 1][j]);
            //    if (i > 0)
            //        result[i].Add(1);
            //}
            #endregion

            #region "3"
            //int max = 0;
            //for (int i = 0; i <= A.Count - 1; i++)
            //{
            //    if (A[i] == max + 1)
            //    {
            //        A[i] = 0;
            //        max++;
            //    }
            //    else if (A[i] <= 0 || A[i] > A.Count || A[i] == i + 1)
            //        continue;
            //    else if (A[A[i] - 1] == A[i])
            //        A[i] = 0;
            //    else
            //    {
            //        int hold = A[A[i] - 1];
            //        A[A[i] - 1] = A[i];
            //        A[i] = hold;
            //        i--;
            //    }
            //}

            //for (int i = Math.Max(max, 0); i <= A.Count - 1; i++)
            //{
            //    if (A[i] <= 0 || A[i] > A.Count)
            //        break;
            //    max++;
            //}

            //return max + 1;
            #endregion

            #region "4"
            //List<int> a1 = new List<int>() { 1, 2, 0 };
            //List<int> a2 = new List<int>() { 3, 4, -1, 1 };
            //List<int> a3 = new List<int>() { -8, -7, -6 };
            //List<int> a4 = new List<int>() { 417, 929, 845, 462, 675, 175, 73, 867, 14, 201, 777, 407, 80, 882, 785, 563, 209, 261, 776, 362, 730, 74, 649, 465, 353, 801, 503, 154, 998, 286, 520, 692, 68, 805, 835, 210, 819, 341, 564, 215, 984, 643, 381, 793, 726, 213, 866, 706, 97, 538, 308, 797, 883, 59, 328, 743, 694, 607, 729, 821, 32, 672, 130, 13, 76, 724, 384, 444, 884, 192, 917, 75, 551, 96, 418, 840, 235, 433, 290, 954, 549, 950, 21, 711, 781, 132, 296, 44, 439, 164, 401, 505, 923, 136, 317, 548, 787, 224, 23, 185, 6, 350, 822, 457, 489, 133, 31, 830, 386, 671, 999, 255, 222, 944, 952, 637, 523, 494, 916, 95, 734, 908, 90, 541, 470, 941, 876, 264, 880, 761, 535, 738, 128, 772, 39, 553, 656, 603, 868, 292, 117, 966, 259, 619, 836, 818, 493, 592, 380, 500, 599, 839, 268, 67, 591, 126, 773, 635, 800, 842, 536, 668, 896, 260, 664, 506, 280, 435, 618, 398, 533, 647, 373, 713, 745, 478, 129, 844, 640, 886, 972, 62, 636, 79, 600, 263, 52, 719, 665, 376, 351, 623, 276, 66, 316, 813, 663, 831, 160, 237, 567, 928, 543, 508, 638, 487, 234, 997, 307, 480, 620, 890, 216, 147, 271, 989, 872, 994, 488, 291, 331, 8, 769, 481, 924, 166, 89, 824, -4, 590, 416, 17, 814, 728, 18, 673, 662, 410, 727, 667, 631, 660, 625, 683, 33, 436, 930, 91, 141, 948, 138, 113, 253, 56, 432, 744, 302, 211, 262, 968, 945, 396, 240, 594, 684, 958, 343, 879, 155, 395, 288, 550, 482, 557, 826, 598, 795, 914, 892, 690, 964, 981, 150, 179, 515, 205, 265, 823, 799, 190, 236, 24, 498, 229, 420, 753, 936, 191, 366, 935, 434, 311, 920, 167, 817, 220, 219, 741, -2, 674, 330, 909, 162, 443, 412, 974, 294, 864, 971, 760, 225, 681, 689, 608, 931, 427, 687, 466, 894, 303, 390, 242, 339, 252, 20, 218, 499, 232, 184, 490, 4, 957, 597, 477, 354, 677, 691, 25, 580, 897, 542, 186, 359, 346, 409, 655, 979, 853, 411, 344, 358, 559, 765, 383, 484, 181, 82, 514, 582, 593, 77, 228, 921, 348, 453, 274, 449, 106, 657, 783, 782, 811, 333, 305, 784, 581, 746, 858, 249, 479, 652, 270, 429, 614, 903, 102, 378, 575, 119, 196, 12, 990, 356, 277, 169, 70, 518, 282, 676, 137, 622, 616, 357, 913, 161, 3, 589, 327 };
            //List<int> a5 = new List<int>() { 1, 1, 1 };

            //Assert.That(this.FindFirstMissingPositive(a1), Is.EqualTo(3));
            //Assert.That(this.FindFirstMissingPositive(a2), Is.EqualTo(2));
            //Assert.That(this.FindFirstMissingPositive(a3), Is.EqualTo(1));
            //Assert.That(this.FindFirstMissingPositive(a4), Is.EqualTo(1));
            //Assert.That(this.FindFirstMissingPositive(a5), Is.EqualTo(2));
            #endregion

            #region "5"
            //int A = 36;

            //Queue<int> left = new Queue<int>();
            //Stack<int> right = new Stack<int>();

            //for (int i = 1; Math.Pow(i, 2) <= A; i++)
            //    if (A % i == 0)
            //    {
            //        left.Enqueue(i);
            //        if (Math.Pow(i, 2) != A)
            //            right.Push(A / i);
            //    }

            //List<int> result = new List<int>();

            //while (left.Count != 0)
            //    result.Add(left.Dequeue());
            //while (right.Count != 0)
            //    result.Add(right.Pop());
            #endregion

            #region "6"
            //List<List<int>> As = new List<List<int>>()
            //{
            //    new List<int>() { 2, 4, 6 },
            //    new List<int>() { 3, 3, 3 },
            //    new List<int>() { 96, 96, 7, 81, 2, 13 }
            //};

            //Assert.That(this.SumOfPairwiseHammingDistance(As[0]), Is.EqualTo(8));
            //Assert.That(this.SumOfPairwiseHammingDistance(As[1]), Is.EqualTo(0));
            //Assert.That(this.SumOfPairwiseHammingDistance(As[2]), Is.EqualTo(104));
            #endregion

            #region "7"
            //List<List<int>> As = new List<List<int>>()
            //{
            //    new List<int>() { 0, 1, 2, 5 }
            //};

            //Assert.That(this.FindSub(As[0], 3, 511, 0, 511), Is.EqualTo(37));
            #endregion

            #region "8"
            //Assert.That(this.titleToNumber("A"), Is.EqualTo(1));
            //Assert.That(this.titleToNumber("C"), Is.EqualTo(3));
            //Assert.That(this.titleToNumber("AA"), Is.EqualTo(27));
            //Assert.That(this.titleToNumber("AB"), Is.EqualTo(28));
            //Assert.That(this.titleToNumber("BBB"), Is.EqualTo(1406));
            #endregion

            #region "9"
            //Assert.That(FindGcd(4, 6), Is.EqualTo(2));
            //Assert.That(FindGcd(9, 6), Is.EqualTo(3));
            //Assert.That(FindGcd(16, 12), Is.EqualTo(4));
            //Assert.That(FindGcd(16, 15), Is.EqualTo(1));
            //Assert.That(FindGcd(286, 247), Is.EqualTo(13));
            #endregion

            #region "10"
            //Assert.That(this.IterateSayIt(1), Is.EqualTo("1"));
            //Assert.That(this.IterateSayIt(2), Is.EqualTo("11"));
            //Assert.That(this.IterateSayIt(3), Is.EqualTo("21"));
            //Assert.That(this.IterateSayIt(4), Is.EqualTo("1211"));
            //Assert.That(this.IterateSayIt(5), Is.EqualTo("111221"));
            //Assert.That(this.IterateSayIt(6), Is.EqualTo("312211"));
            //Assert.That(this.IterateSayIt(7), Is.EqualTo("13112221"));
            //Assert.That(this.IterateSayIt(8), Is.EqualTo("1113213211"));
            //Assert.That(this.IterateSayIt(9), Is.EqualTo("31131211131221"));
            //Assert.That(this.IterateSayIt(10), Is.EqualTo("13211311123113112211"));
            #endregion
        }

        #region "read rome"
        private int FindNextDiffRomeChar(String a, int start)
        {
            for (int i = start + 1; i <= a.Length - 1; i++)
                if (a[i] != a[i - 1])
                    return i;

            return a.Length;
        }

        private int ReadRome(String a)
        {
            int result = 0;
            RomeChar m = new RomeChar('M', 1000, 'C', char.MinValue, char.MinValue);
            RomeChar c = new RomeChar('C', 100, 'X', 'M', 'D');
            RomeChar x = new RomeChar('X', 10, 'I', 'C', 'L');
            RomeChar i = new RomeChar('I', 1, char.MinValue, 'X', 'V');
            List<RomeChar> romeChars = new List<RomeChar>() { m, c, x, i };

            for (int j = 0; j <= a.Length - 1; j++)
            {
                foreach (RomeChar aChar in romeChars)
                    if (a[j] == aChar.theChar)
                    {
                        int k = FindNextDiffRomeChar(a, j);
                        result += aChar.value * (k - j);
                        if (j > 0)
                            if (a[j - 1] == aChar.preChar)
                                result -= 2 * aChar.value / 10;
                        j = k - 1;
                    }
                    else if (a[j] == aChar.next5)
                    {
                        result += 5 * aChar.value;
                        if (j > 0)
                            if (a[j - 1] == aChar.theChar)
                                result -= 2 * aChar.value;
                    }
            }

            return result;
        }
        #endregion

        #region "write rome"
        private String WriteRome(int A)
        {
            int remain = A;
            int digit = 0;
            List<char> result = new List<char>();
            Dictionary<int, char> map = new Dictionary<int, char>() { { 1, 'I' }, { 10, 'X' }, { 100, 'C' }, { 1000, 'M' } };
            Dictionary<int, char> map5 = new Dictionary<int, char>() { { 1, 'V' }, { 10, 'L' }, { 100, 'D' } };

            while (remain > 0)
            {
                int num = remain % 10;
                if ((num >= 6 && num <= 8) || (num >= 1 && num <= 3))
                {
                    for (int i = 1; i <= num % 5; i++)
                        result.Insert(0, map[(int)Math.Pow(10, digit)]);
                    if (num >= 6) result.Insert(0, map5[(int)Math.Pow(10, digit)]);
                }
                else if (num != 0)
                {
                    if (num == 9)
                        result.Insert(0, map[(int)Math.Pow(10, digit + 1)]);
                    if (num == 4 || num == 5)
                        result.Insert(0, map5[(int)Math.Pow(10, digit)]);
                    if (num == 4 || num == 9)
                        result.Insert(0, map[(int)Math.Pow(10, digit)]);
                }
                remain /= 10;
                digit++;
            }

            StringBuilder sb = new StringBuilder();
            foreach (char c in result)
                sb.Append(c);

            return sb.ToString();
        }
        #endregion

        #region "binary count"
        private int BinaryFind(List<int> A, int B, int start, int end)
        {
            int mid = start + (end - start) / 2;
            if (B < A[start] || B > A[end] || start >= mid) return -1;
            if (B > A[mid])
                return BinaryFind(A, B, mid, end);
            else if (B < A[mid])
                return BinaryFind(A, B, start, mid);
            else
                return mid;
        }

        private int BinaryCount(List<int> A, int B)
        {
            if (B < A[0] || B > A[A.Count - 1]) return 0;
            int i = BinaryFind(A, B, 0, A.Count - 1);
            int count = 1;
            if (i == -1) return 0;
            for (int j = i + 1; j <= A.Count - 1; j++)
                if (A[i] == A[j])
                    count++;
            for (int j = i - 1; j >= 0; j--)
                if (A[i] == A[j])
                    count++;

            return count;
        }
        #endregion

        #region "square root"
        private int sqrt(int a)
        {
            if (a < 0) return 0;
            if (a <= 1) return a;
            long start = 1;
            long end = a;
            long mid = 1 + (a - 1) / 2;
            while (start < mid)
            {
                long square = mid * mid;
                if (square > a || square > Int32.MaxValue)
                    end = mid;
                else if (square < a)
                    start = mid;
                else
                    break;

                mid = start + (end - start) / 2;
            }
            return (int)mid;
        }
        #endregion

        #region "diff possible"
        private int DiffPossible(List<int> A, int k)
        {
            if (A.Count < 2) return 0;
            if ((A[A.Count - 1] - A[0]) < k) return 0;

            int i = 0, j = A.Count - 1;
            while (j > 0)
            {
                if ((A[j] - A[i]) < k || i == j)
                {
                    j--;
                    i = 0;
                }
                else if ((A[j] - A[i]) == k)
                    return 1;
                else
                    i++;
            }

            return 0;
        }
        #endregion

        #region "3 sum closest"
        private int threeSumClosest(List<int> A, int B)
        {
            int a = 0, b = 1, c = 2;
            if (A.Count < 3) return 0;
            int minDiff = Math.Abs(A[a] + A[b] + A[c] - B);

            for (int i = 3; i <= A.Count - 1; i++)
            {
                if (Math.Abs(A[i] + A[b] + A[c] - B) < minDiff)
                    a = i;
                else if (Math.Abs(A[a] + A[i] + A[c] - B) < minDiff)
                    b = i;
                else if (Math.Abs(A[a] + A[b] + A[i] - B) < minDiff)
                    c = i;
                else
                    continue;

                minDiff = Math.Abs(A[a] + A[b] + A[c] - B);
            }

            return A[a] + A[b] + A[c];
        }
        #endregion

        #region "3 sum 0"
        private List<List<int>> ThreeSum0(List<int> A)
        {
            A.Sort();
            List<List<int>> result = new List<List<int>>();
            for (int i = 0; i <= A.Count - 3; i++)
            {
                if (i > 0)
                    if (A[i] == A[i - 1])
                        continue;
                int j = i + 1, k = A.Count - 1;
                while (j < k)
                {
                    int sum = A[i] + A[j] + A[k];
                    if (sum == 0)
                    {
                        if (j > i + 1 && A[j] == A[j - 1])
                        {
                            j++;
                            continue;
                        }
                        result.Add(new List<int>() { A[i], A[j], A[k] });
                        j++;
                    }
                    else if (sum > 0)
                        k--;
                    else
                        j++;
                }
            }
            return result;
        }
        #endregion

        #region "min xor"
        private int MinXor(List<int> a)
        {
            a.Sort();
            int min = Int32.MaxValue;

            for (int i = 0; i <= a.Count - 2; i++)
            {
                int currentMin = a[i] ^ a[i + 1];
                int minPair = a[i + 1];
                for (int j = i + 2; j <= a.Count - 1 && (a[j] - minPair) < 2; j++)
                {
                    int xor = a[i] ^ a[j];
                    if (xor < currentMin)
                    {
                        currentMin = xor;
                        minPair = a[j];
                    }
                }
                min = Math.Min(min, currentMin);
            }
            return min;
        }
        #endregion

        #region "find single from triples"
        private int FindSingleFromTriples(List<int> A)
        {
            int result = 0;
            int digit = 1;
            bool hasRemain = true;

            while (hasRemain)
            {
                int digitSum = 0;
                hasRemain = false;

                for (int i = 0; i <= A.Count - 1; i++)
                {
                    digitSum += A[i] % 2;
                    A[i] /= 2;
                    if (!hasRemain)
                        if (A[i] > 0)
                            hasRemain = true;
                }
                result += digitSum % 3 * digit;
                digit *= 2;
            }

            return result;

            //long biSum = 0;
            //for (int i = 0; i <= A.Count - 1; i++)
            //    biSum += Int32.Parse(Convert.ToString(A[i], 2));

            //int result = 0, digit = 0;
            //while (biSum > 0)
            //{
            //    result += (int)(biSum % 10 % 3 * (int)Math.Pow(2, digit));
            //    biSum /= 10;
            //    digit++;
            //}
            //return result;
        }
        #endregion

        #region "total diff bits"
        private int TotalDiffBits(List<int> A)
        {
            bool hasRemain = true;
            int result = 0;
            while (hasRemain)
            {
                hasRemain = false;
                int ones = 0;
                for (int i = 0; i <= A.Count - 1; i++)
                {
                    if (A[i] % 2 == 1) ones++;
                    A[i] /= 2;
                    if (!hasRemain)
                        if (A[i] > 0)
                            hasRemain = true;
                }
                result += ones * (A.Count - ones) * 2;
            }
            return (int)(result % 1000000007);
        }
        #endregion

        #region "str str"
        private int StrStr(String A, String B)
        {
            if (A == null || A == "" || B == null || B == "")
                return -1;

            for (int i = 0; i <= A.Length - B.Length; i++)
            {
                int matches = 0;
                for (int j = 0; j <= B.Length - 1; j++)
                    if (A[i + j] != B[j])
                        break;
                    else
                        matches++;
                if (matches == B.Length)
                    return i;
            }

            return -1;
        }
        #endregion

        #region "pretty JSON"
        private StringBuilder PrettyJson(String A)
        {
            List<String> result = new List<string>();
            PrettyJsonSub(A, 0, 0, result);

            StringBuilder sb = new StringBuilder();
            foreach (String s in result)
                sb.AppendLine(s);
            return sb;
        }

        private int PrettyJsonSub(String A, int start, int level, List<String> result)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= level - 1; i++)
                sb.Append("\t");
            String indent = sb.ToString();
            sb = new StringBuilder();

            for (int i = start; i <= A.Length - 1; i++)
            {
                if (A[i] == ' ')
                    continue;

                if (A[i] == '{' || A[i] == '[')
                {
                    if (sb.Length > 0)
                        result.Add(indent + sb.ToString());
                    result.Add(indent + A[i].ToString());
                    i = PrettyJsonSub(A, i + 1, level + 1, result);
                    sb = new StringBuilder();
                    sb.Append(A[i]);
                }
                else if (A[i] == '}' || A[i] == ']')
                {
                    if (sb.Length > 0)
                        result.Add(indent + sb.ToString());
                    return i;
                }
                else
                {
                    sb.Append(A[i]);
                    if (A[i] == ',')
                    {
                        result.Add(indent + sb.ToString());
                        sb = new StringBuilder();
                    }
                }
            }
            if (sb.Length > 0)
                result.Add(indent + sb.ToString());

            return A.Length;
        }
        #endregion

        #region "atoi"
        private int Atoi(String A)
        {
            long result = 0;
            int i = 0;
            int maxDigit = 10;
            if (A[0] == '-' || A[0] == '+')
            {
                i++;
                maxDigit++;
            }

            bool numberDigit = true;
            for (; i <= maxDigit && numberDigit && i <= A.Length - 1; i++)
            {
                switch (A[i])
                {
                    case '0':
                        result = 10 * result;
                        break;
                    case '1':
                        result = 10 * result + 1;
                        break;
                    case '2':
                        result = 10 * result + 2;
                        break;
                    case '3':
                        result = 10 * result + 3;
                        break;
                    case '4':
                        result = 10 * result + 4;
                        break;
                    case '5':
                        result = 10 * result + 5;
                        break;
                    case '6':
                        result = 10 * result + 6;
                        break;
                    case '7':
                        result = 10 * result + 7;
                        break;
                    case '8':
                        result = 10 * result + 8;
                        break;
                    case '9':
                        result = 10 * result + 9;
                        break;
                    default:
                        numberDigit = false;
                        break;
                }
            }

            if (result > Int32.MaxValue)
            {
                if (A[0] == '-') return Int32.MinValue;
                else return Int32.MaxValue;
            }
            else
            {
                if (A[0] == '-') return -(int)result;
                else return (int)result;
            }
        }
        #endregion

        #region "longest palindrome"
        private String LongestPalindrome(String A)
        {
            String result = "";

            if (A.Length <= 1) return A[0].ToString();

            int max = 1, start = 0;

            for (int i = 0; i <= A.Length - 2; i++)
            {
                int leftLength = this.LeftPalindromeWithCenterLength(A, i);
                int length = 2 * leftLength + 1;
                if (length > max)
                {
                    max = length;
                    start = i - leftLength;
                }
                leftLength = this.LeftPalindromeLength(A, i);
                length = 2 * leftLength;
                if (length > max)
                {
                    max = length;
                    start = i - leftLength + 1;
                }
            }

            for (int i = start; i <= start + max - 1; i++)
                result += A[i];

            return result;
        }

        private int LeftPalindromeWithCenterLength(String A, int mid)
        {
            int result = 0;

            for (int i = 1; mid - i >= 0 && mid + i <= A.Length - 1; i++)
            {
                if (A[mid - i] != A[mid + i])
                    break;

                result++;
            }

            return result;
        }

        private int LeftPalindromeLength(String A, int mid)
        {
            int result = 0;
            if (A[mid] == A[mid + 1])
            {
                result++;
                for (int i = 1; mid - i >= 0 && mid + 1 + i <= A.Length - 1; i++)
                {
                    if (A[mid - i] != A[mid + 1 + i])
                        break;

                    result++;
                }
            }

            return result;
        }
        #endregion

        #region "reverse linked list"
        private ListNode ReversePartialLinkedList(ListNode A, int B, int C)
        {
            int pointer = 1;
            ListNode current = A.next;
            ListNode preReverse = null;
            if (B != 1) preReverse = A;
            ListNode firstReverse = A;

            while (current != null && pointer < C)
            {
                pointer++;

                if (pointer <= B)
                {
                    if (pointer != B)
                        preReverse = current;
                    else
                        firstReverse = current;
                    current = current.next;
                }
                else
                {
                    ListNode temp = null;
                    if (preReverse != null)
                    {
                        temp = preReverse.next;
                        preReverse.next = current;
                    }
                    else
                    {
                        temp = A;
                        A = current;
                    }
                    firstReverse.next = current.next;
                    current.next = temp;
                    current = firstReverse.next;
                }

                Console.WriteLine(A.val + "\n" + A.GetThisAndAfterInString());
            }

            return A;
        }
        #endregion

        #region "add 2 linked list"
        private ListNode Add2LinkedList(ListNode A, ListNode B)
        {
            if (A == null) return B;
            if (B == null) return A;

            ListNode dummy = new ListNode(0);
            dummy.next = A;
            ListNode previous = dummy;
            int carry = 0, digit = 0;

            while (A != null || B != null)
            {
                digit = carry;
                if (A != null)
                    digit += A.val;
                else
                {
                    A = new ListNode(0);
                    previous.next = A;
                }
                if (B != null)
                    digit += B.val;
                else
                    B = new ListNode(0);
                carry = digit / 10;
                digit %= 10;

                A.val = digit;
                previous = A;
                A = A.next;
                B = B.next;
            }

            if (carry != 0)
            {
                A = new ListNode(carry);
                previous.next = A;
            }
            if (A != null)
                if (A.val == 0)
                    previous.next = null;

            return dummy.next;
        }
        #endregion

        #region "insert sort linked list"
        public ListNode InsertionSortList(ListNode A)
        {
            if (A == null || A.next == null) return A;

            ListNode dummy = new ListNode(0);
            dummy.next = A;
            ListNode current = A.next;
            A.next = null;
            InsertionSortListSub(A, current, dummy);

            return A;
        }

        private void InsertionSortListSub(ListNode sorted, ListNode current, ListNode preSorted)
        {
            if (sorted == null)
            {
                sorted = new ListNode(current.val);
                preSorted.next = sorted;
            }
            else if (current.val > preSorted.val && current.val <= sorted.val)
            {
                ListNode newNode = new ListNode(current.val);
                newNode.next = sorted;
                preSorted.next = newNode;
            }
            else
            {
                sorted.next = new ListNode(current.val);
                sorted = sorted.next;
            }

            sorted.next = null;

            if (current.next != null)
                InsertionSortListSub(sorted, current.next, preSorted.next);

            return;
        }
        #endregion

        #region "detect cycle"
        private ListNode DetectCycle(ListNode a)
        {
            if (a == null || a.next == null || a.next.next == null) return null;

            ListNode p1 = a.next, p2 = a.next.next;

            while (p1.val != p2.val)
            {
                if (p1.next == null)
                    return null;
                else
                    p1 = p1.next;

                if (p2.next == null)
                    return null;
                else if (p2.next.next == null)
                    return null;
                else
                    p2 = p2.next.next;

                logger.InfoFormat("p1 - [{0}], p2 - [{1}]", p1.val, p2.val);
            }

            p2 = a;

            while (p1.val != p2.val)
            {
                p1 = p1.next;
                p2 = p2.next;
                logger.InfoFormat("p1 - [{0}], p2 - [{1}]", p1.val, p2.val);
            }

            logger.InfoFormat("p1 - [{0}], p2 - [{1}]", p1.val, p2.val);
            return p1;
        }
        #endregion

        #region "reverse polish notation"
        private int ReversePolishNotation(List<String> A)
        {
            if (A.Count == 1) return Int32.Parse(A[0]);

            Stack<int> s = new Stack<int>();
            int right = 0;
            for (int i = 0; i <= A.Count - 1; i++)
            {
                if (A[i] == "+")
                    s.Push(s.Pop() + s.Pop());
                else if (A[i] == "-")
                    s.Push((-s.Pop()) + s.Pop());
                else if (A[i] == "*")
                    s.Push(s.Pop() * s.Pop());
                else if (A[i] == "/")
                {
                    right = s.Pop();
                    int left = s.Pop();
                    s.Push(left / right);
                }
                else if (Int32.TryParse(A[i], out right))
                    s.Push(right);
            }

            return s.Pop();
        }

        #endregion

        #region "max ract"
        public class MaxRact
        {
            public int height;
            public int length;

            private MaxRact() { }

            public MaxRact(int height, int length)
            {
                this.height = height;
                this.length = length;
            }
        }

        private int GetMaxRact(List<int> A)
        {
            int max = A[0];
            Stack<MaxRact> s = new Stack<MaxRact>();
            s.Push(new MaxRact(A[0], 1));

            for (int i = 1; i <= A.Count - 1; i++)
            {
                if (A[i] <= A[i - 1])
                    s.Push(new MaxRact(A[i], s.Peek().length + 1));
                else
                {
                    if (A[i] >= s.Peek().height * (s.Peek().length + 1))
                        s.Push(new MaxRact(A[i], 1));
                    else
                        s.Push(new MaxRact(s.Peek().height, s.Peek().length + 1));
                }
                max = Math.Max(max, s.Peek().height * s.Peek().length);
            }

            return max;
        }
        #endregion

        #region "simplify path"
        private String SimplifyPath(String A)
        {
            Stack<String> s = new Stack<String>();
            String currentDir = "";

            for (int i = 0; i <= A.Length - 1; i++)
            {
                if (A[i] == '/')
                {
                    if (!currentDir.Equals(".") && !String.IsNullOrEmpty(currentDir))
                        s.Push(currentDir);

                    currentDir = "";
                }
                else if (A[i] == '.' && currentDir.Equals("."))
                {
                    if (s.Count != 0)
                        s.Pop();
                    currentDir = "";
                }
                else
                    currentDir += A[i].ToString();
            }

            if (!String.IsNullOrEmpty(currentDir) && !currentDir.Equals("."))
                s.Push(currentDir);
            if (s.Count == 0) return "/";

            String result = s.Pop();
            while (s.Count != 0)
                result = s.Pop().ToString() + "/" + result;

            return "/" + result;
        }
        #endregion

        #region "modular"
        private int Mod(int A, int B, int C)
        {
            if (A == 0) return 0;
            if (B == 0) return 1;

            if (B == 1)
            {
                if (A < 0 && C > 0)
                    return C - (-A % C);
                if (A > 0 && C < 0)
                    return (-A % C) - C;
                return A % C;
            }
            else
            {
                long temp = 0;
                if (B % 2 == 0)
                {
                    temp = Mod(A, B / 2, C);
                    return (int)(temp * temp % C);
                }
                temp = Mod(A, B - 1, C) % C;
                return (int)(A % C * temp);
            }
        }
        #endregion

        #region "reverse linked list recursivly"
        private ListNode ReverseList(ListNode A)
        {
            ListNode dummy = new ListNode(0);
            ReverseListSub(A, dummy).next = null;
            return dummy.next;
        }

        private ListNode ReverseListSub(ListNode A, ListNode dummy)
        {
            ListNode current = A;
            ListNode previous = null;

            if (current.next != null)
                previous = ReverseListSub(A.next, dummy);
            else
            {
                dummy.next = current;
                return current;
            }

            previous.next = current;
            return current;
        }
        #endregion

        #region "combination sum"
        private List<List<int>> CombinationSum(List<int> A, int B)
        {
            A.Sort();
            List<List<int>> result = new List<List<int>>();
            CombinationSumSub(A, B, new List<int>(), 0, 0, result);
            return result;
        }

        private void CombinationSumSub(List<int> A, int B, List<int> current, int index, int total, List<List<int>> result)
        {
            if (total == B)
            {
                result.Add(new List<int>(current));
                return;
            }

            for (int i = index; i <= A.Count - 1; i++)
            {
                if (i != index && A[i] == A[i - 1]) continue;
                if (total + A[i] > B) break;

                current.Add(A[i]);
                CombinationSumSub(A, B, current, i, total + A[i], result);
                current.RemoveAt(current.Count - 1);
            }
        }
        #endregion

        #region "letter combo"
        private List<String> LetterCombo(String A)
        {
            List<String> result = new List<string>();
            GetCombo(A, 0, "", result);
            return result;
        }

        private Dictionary<char, List<String>> map = new Dictionary<char, List<String>>()
        {
            {'0', new List<String>(){"0"} },
            {'1', new List<String>(){"1"} },
            {'2', new List<String>(){"a", "b", "c"} },
            {'3', new List<String>(){"d", "e", "f"} },
            {'4', new List<String>(){"g", "h", "i"} },
            {'5', new List<String>(){"j", "k", "l"} },
            {'6', new List<String>(){"m", "n", "o"} },
            {'7', new List<String>(){"p", "q", "r", "s"} },
            {'8', new List<String>(){"t", "u", "v"} },
            {'9', new List<String>(){"w", "x", "y", "z"} }
        };

        private void GetCombo(String A, int i, String current, List<String> result)
        {
            if (i > A.Length - 1)
            {
                if (!String.IsNullOrEmpty(current))
                    result.Add(current);
                return;
            }

            foreach (String s in map[A[i]])
            {
                String temp = current;
                temp += s;
                GetCombo(A, i + 1, temp, result);
            }
        }
        #endregion

        #region "get all int combo"
        private List<List<int>> GetAllIntCombo(List<int> A)
        {
            List<List<int>> result = new List<List<int>>();
            GetAllIntComboSub(A, new List<int>(), result);
            return result;
        }

        private bool[] intMap = new bool[10];

        private void GetAllIntComboSub(List<int> A, List<int> current, List<List<int>> result)
        {
            if (current.Count == A.Count)
            {
                result.Add(new List<int>(current));
                return;
            }

            for (int i = 0; i <= A.Count - 1; i++)
            {
                if (intMap[A[i]]) continue;

                current.Add(A[i]);
                intMap[A[i]] = true;
                GetAllIntComboSub(A, current, result);
                current.RemoveAt(current.Count - 1);
                intMap[A[i]] = false;
            }
        }
        #endregion

        #region "2 sum with hash"
        private List<int> TwoSumWithHash(List<int> A, int B)
        {
            List<int> result = new List<int>();
            Dictionary<int, List<int>> map = new Dictionary<int, List<int>>();
            for (int i = 0; i <= A.Count - 1; i++)
            {
                if (!map.ContainsKey(A[i]))
                    map[A[i]] = new List<int>();
                map[A[i]].Add(i);
            }

            for (int index2 = 1; index2 <= A.Count - 1; index2++)
                if (map.ContainsKey(B - A[index2]))
                {
                    List<int> pos = map[B - A[index2]];
                    if (pos[0] < index2)
                    {
                        result.Add(pos[0] + 1);
                        result.Add(index2 + 1);
                        return result;
                    }
                }

            return result;
        }
        #endregion

        #region "n queen"
        private List<List<String>> NQueens(int a)
        {
            List<List<String>> result = new List<List<string>>();

            for (int i = 0; i <= a - 1; i++)
            {
                int next = i + 1;
                next = next >= a ? next - a - 1 : next;
                result.Add(new List<string>(this.NQueensSub(a, next)));
            }

            return result;
        }

        private List<String> NQueensSub(int a, int start)
        {
            bool[] mapX = new bool[a];
            bool[] mapY = new bool[a];
            List<String> result = new List<string>();

            for (int i = 0; i <= a - 1; i++)
            {
                String line = "";

                for (int j = 0; j <= a - 1; j++)
                {
                    if (j == start && !mapX[j] && !mapY[i])
                    {
                        line += "Q";
                        mapX[j] = true;
                        mapY[i] = true;

                        int next = start + 2;
                        start = next >= a ? 0 : next;
                    }
                    else
                        line += ",";
                }

                result.Add(line);
            }

            return result;
        }
        #endregion

        #region "diff possible"
        private int DiffPossibleWithHash(List<int> A, int B)
        {
            if (B == 0) return 0;

            HashSet<int> map = new HashSet<int>(A);

            foreach (int i in A)
                if (map.Contains(B + A[i]) || map.Contains(A[i] - B))
                    return 1;

            return 0;
        }
        #endregion

        #region "sum equal"
        public class MyTuple<T1, T2>
        {
            public T1 Item1 { get; private set; }
            public T2 Item2 { get; private set; }
            internal MyTuple(T1 first, T2 second)
            {
                Item1 = first;
                Item2 = second;
            }
        }

        private List<int> SumEqual(List<int> A)
        {
            Dictionary<int, List<MyTuple<int, int>>> map = new Dictionary<int, List<MyTuple<int, int>>>();
            List<int> result = new List<int>();

            for (int i = 0; i < A.Count - 1; i++)
                for (int j = i + 1; j <= A.Count - 1; j++)
                {
                    int sum = A[i] + A[j];
                    if (!map.ContainsKey(sum))
                        map.Add(sum, new List<MyTuple<int, int>>());

                    map[sum].Add(new MyTuple<int, int>(i, j));
                }

            foreach (KeyValuePair<int, List<MyTuple<int, int>>> sums in map)
                if (sums.Value.Count > 1)
                {
                    for (int i = 0; i < sums.Value.Count - 1; i++)
                        for (int j = i + 1; j <= sums.Value.Count - 1; j++)
                            if (sums.Value[j].Item1 != sums.Value[i].Item1 && sums.Value[j].Item1 != sums.Value[i].Item2 && sums.Value[i].Item2 != sums.Value[j].Item2)
                            {
                                result.Add(sums.Value[i].Item1);
                                result.Add(sums.Value[i].Item2);
                                result.Add(sums.Value[j].Item1);
                                result.Add(sums.Value[j].Item2);
                                return result;
                            }
                }

            return result;
        }
        #endregion

        #region "sub string indexes"
        private List<int> SubStringIndexes(String A, List<String> B)
        {
            Dictionary<String, int> map = new Dictionary<String, int>();

            for (int i = 0; i <= B.Count - 1; i++)
            {
                if (!map.ContainsKey(B[i]))
                    map.Add(B[i], 1);
                else
                    map[B[i]]++;
            }

            foreach (KeyValuePair<String, int> pair in map)
                Console.WriteLine("{0} appeared {1} time(s)", pair.Key, pair.Value);

            List<int> result = new List<int>();

            for (int i = 0; i < A.Length + 1 - B.Count * B[0].Length; i++)
            {
                Dictionary<String, int> subMap = new Dictionary<String, int>();

                for (int j = 0; j <= B.Count * B[0].Length - 1;)
                {
                    String current = "";
                    while (current.Length != B[0].Length)
                    {
                        current += A[i + j];
                        j++;
                    }

                    Console.WriteLine("Checking {0}", current);

                    if (!subMap.ContainsKey(current))
                        subMap.Add(current, 1);
                    else
                        subMap[current]++;
                }

                bool allMatch = true;
                foreach (KeyValuePair<String, int> pair in map)
                    if (!subMap.ContainsKey(pair.Key) || pair.Value != subMap[pair.Key])
                    {
                        allMatch = false;
                        break;
                    }
                if (allMatch)
                {
                    Console.WriteLine("Found it at {0}", i);
                    result.Add(i);
                }
            }

            return result;
        }
        #endregion

        [Test]
        public void TestMethod()
        {
            //Assert.That(new List<string>() { "ABSG", "ABGS", "ASBG", "ASGB", "AGBS", "AGSB", "BASG", "BAGS", "BSAG", "BSGA", "BGAS", "BGSA", "SABG", "SAGB", "SBAG", "SBGA", "SGAB", "SGBA", "GABS", "GASB", "GBAS", "GBSA", "GSAB", "GSBA" }, Is.EquivalentTo(new List<string>() { "ABGS", "ABSG", "AGBS", "AGSB", "ASBG", "ASGB", "BAGS", "BASG", "BGAS", "BGSA", "BSAG", "BSGA", "GABS", "GASB", "GBAS", "GBSA", "GSAB", "GSBA", "SABG", "SAGB", "SBAG", "SBGA", "SGAB", "SGBA" }));
            //Console.WriteLine("123".ToCharArray().Reverse());
            //Console.WriteLine("123".ToCharArray().Reverse().ToString());
            //Console.WriteLine(new string("123".ToCharArray().Reverse().ToArray()));
            //Console.WriteLine(string.Concat(new string[] { "12", "3" }));

            //ListNode start = new ListNode(1);
            //ListNode tempNode = start;
            //for (int i = 3; i <= 15; i += 2)
            //{
            //    ListNode newNode = new ListNode(i);
            //    tempNode.next = newNode;
            //    tempNode = newNode;
            //}

            #region "sub string indexes"
            //Assert.That(this.SubStringIndexes("c", new List<string>() { "c" }), Is.EqualTo(new List<int>() { 0 }));
            //Assert.That(this.SubStringIndexes("barfoothefoobarman", new List<string>() { "foo", "bar" }), Is.EqualTo(new List<int>() { 0, 9 }));
            #endregion

            #region "sum equal"
            //Assert.That(this.SumEqual(new List<int>() { 0, 0, 1, 0, 2, 1 }), Is.EqualTo(new List<int>() { 0, 2, 1, 5 }));
            //Assert.That(this.SumEqual(new List<int>() { 1, 1, 1, 1, 1 }), Is.EqualTo(new List<int>() { 0, 1, 2, 3 }));
            //Assert.That(this.SumEqual(new List<int>() { 3, 4, 7, 1, 2, 9, 8 }), Is.EqualTo(new List<int>() { 0, 2, 3, 5 }));
            #endregion

            #region "diff possible"
            //Assert.That(this.DiffPossibleWithHash(new List<int>() { 2, 3, 5, 1 }, 2), Is.EqualTo(1));
            //Assert.That(this.DiffPossibleWithHash(new List<int>() { 2, 3, 5, 1 }, 0), Is.EqualTo(0));
            //Assert.That(this.DiffPossibleWithHash(new List<int>() { 2, 3, 5, 1 }, 100), Is.EqualTo(0));
            #endregion

            #region "2 sum with hash"
            //Assert.That(this.TwoSumWithHash(new List<int>() { 10, -3, 5, -7, -4, 5, 6, -7, 8, -5, 8, 0, 8, -5, -10, -1, 1, -6, 4, -1, -2, -2, 10, -2, -4, -7, 5, 1, 7, -10, 0, 5, 8, 6, -8, 8, -8, -8, 3, -9, -10, -5, -5, -10, 10, -4, 8, 0, -6, -2, 3, 7, -5, 5, 1, -7, 0, -5, 1, -3, 10, -4, -3, 3, 3, 5, 1, -2, -6, 3, -4, 10, -10, -3, -8, 2, -2, -3, 0, 10, -6, -8, -10, 6, 7, 0, 3, 9, -10, -7, 8, -7, -7 }, -2), Is.EqualTo(new List<int>() { 3, 4 }));
            #endregion

            #region "n queen"
            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine("Printing NQueen results...");
            //foreach(List<String> lines in this.NQueens(4))
            //{
            //    foreach (String line in lines)
            //        sb.AppendLine(line);
            //    sb.AppendLine();
            //}
            //logger.InfoFormat(sb.ToString());
            #endregion

            #region "get all int combo"
            //this.GetAllIntCombo(new List<int>() { 1, 2, 3 });
            #endregion

            #region "letter combo"
            //StringBuilder sb = new StringBuilder();
            //foreach (String s in this.LetterCombo("34"))
            //    sb.Append(s + " ");
            //Assert.That(sb.ToString(), Is.EqualTo("dg dh di eg eh ei fg fh fi "));
            #endregion

            #region "combination sum"
            //List<List<int>> result = this.CombinationSum(new List<int>() { 2, 3, 6, 7 }, 7);
            //Console.WriteLine();
            #endregion

            #region "modular"
            //Console.WriteLine("{0} = {1} = {2}", 3 % -5, -3 % 5, -3 % -5);
            //Console.WriteLine("{0} = {1} = {2}", -12 % 5, 12 % -5, -12 % -5);
            //Assert.That(this.Mod(71045970, 41535484, 64735492), Is.EqualTo(20805472));
            //Assert.That(this.Mod(-1, 1, 20), Is.EqualTo(19));
            //Assert.That(this.Mod(123, 0, 321), Is.EqualTo(1));
            //Assert.That(this.Mod(2, 3, 3), Is.EqualTo(2));
            #endregion

            #region "simplify path"
            //Assert.That(this.SimplifyPath("/lbk"), Is.EqualTo("/lbk"));
            //Assert.That(this.SimplifyPath("/bku/zjy/mia/jss/lyf/jzt/bxk/qfd/gpz/dbb/ayw/jhv/evi/."), Is.EqualTo("/bku/zjy/mia/jss/lyf/jzt/bxk/qfd/gpz/dbb/ayw/jhv/evi"));
            //Assert.That(this.SimplifyPath("/cbj/vfb/dyj/../../hjq/./unc/./cmv/./axj/../pzg/svs/oja/./rlc/vyr/cqq/../omk/viy/kyb/../ygr/mbx/nom/yvh/./../././lyg/qjv/./lwm/.././.././xga/krs/../xkl/wtj/nml/dal/hat/alw/jvo/./../xts/nul/yfe/upg/zhy/nzo/dtp/nqt/hqk/./../ref/gms/zhp/./bpp/jis/ccc/bmn/iip/nfv/../vbk/ugr/gpd/uez/./bqt/zqy/../vuf/ltg/mxm/../lvr/vef/../wpp/./rbc/xii/pkf/jsx/././xwo"), Is.EqualTo("/cbj/hjq/unc/cmv/pzg/svs/oja/rlc/vyr/omk/viy/ygr/mbx/nom/lyg/xga/xkl/wtj/nml/dal/hat/alw/xts/nul/yfe/upg/zhy/nzo/dtp/nqt/ref/gms/zhp/bpp/jis/ccc/bmn/iip/vbk/ugr/gpd/uez/bqt/vuf/ltg/lvr/wpp/rbc/xii/pkf/jsx/xwo"));
            //Assert.That(this.SimplifyPath("/./.././ykt/xhp/nka/eyo/blr/emm/xxm/fuv/bjg/./qbd/./../pir/dhu/./../../wrm/grm/ach/jsy/dic/ggz/smq/mhl/./../yte/hou/ucd/vnn/fpf/cnb/ouf/hqq/upz/akr/./pzo/../llb/./tud/olc/zns/fiv/./eeu/fex/rhi/pnm/../../kke/./eng/bow/uvz/jmz/hwb/./././ids/dwj/aqu/erf/./koz/.."), Is.EqualTo("/ykt/xhp/nka/eyo/blr/emm/xxm/fuv/bjg/wrm/grm/ach/jsy/dic/ggz/smq/yte/hou/ucd/vnn/fpf/cnb/ouf/hqq/upz/akr/llb/tud/olc/zns/fiv/eeu/fex/kke/eng/bow/uvz/jmz/hwb/ids/dwj/aqu/erf"));
            //Assert.That(this.SimplifyPath("/home/"), Is.EqualTo("/home"));
            //Assert.That(this.SimplifyPath("/a/./b/../../c/"), Is.EqualTo("/c"));
            //Assert.That(this.SimplifyPath("home/../user1/./Documents/"), Is.EqualTo("/user1/Documents"));
            //Assert.That(this.SimplifyPath("/.."), Is.EqualTo("/"));
            //Assert.That(this.SimplifyPath("/../"), Is.EqualTo("/"));
            #endregion

            #region "max ract"
            //Assert.That(this.GetMaxRact(new List<int>() { 2, 1, 5, 6, 2, 3 }), Is.EqualTo(10));
            //Assert.That(this.GetMaxRact(new List<int>() { 90, 58, 69, 70, 82, 100, 13, 57, 47, 18 }), Is.EqualTo(348));
            #endregion

            #region "reverse polish notation"
            //Assert.That(this.ReversePolishNotation(new List<String>() { "1" }), Is.EqualTo(1));
            //Assert.That(this.ReversePolishNotation(new List<String>() { "2", "2", "/" }), Is.EqualTo(1));
            //Assert.That(this.ReversePolishNotation(new List<String>() { "2", "3", "+", "4", "5", "+", "+", "6", "7", "+", "*", "2", "*" }), Is.EqualTo(364));
            #endregion

            #region "detect cycle"
            //ListNode n1 = new ListNode(1);
            //ListNode n2 = new ListNode(2);
            //n1.next = n2;
            //ListNode n3 = new ListNode(3);
            //n2.next = n3;
            //ListNode n4 = new ListNode(4);
            //n3.next = n4;
            //ListNode n5 = new ListNode(5);
            //n4.next = n5;
            //n5.next = n3;

            //Assert.That(this.DetectCycle(n1).val, Is.EqualTo(3));
            #endregion

            #region "insert sort linked list"
            //Assert.That(this.InsertionSortList(new ListNode(new List<int>() { 1, 7, 2, 5, 4, 6 })).GetThisAndAfterInString(), Is.EqualTo("1 2 4 5 6 7 "));
            #endregion

            #region "add 2 linked list"
            //Assert.That(this.Add2LinkedList(new ListNode(new List<int>() { 2, 4, 3 }), new ListNode(new List<int>() { 5, 6, 4 })).GetThisAndAfterInString(), Is.EqualTo("7 0 8 "));
            //Assert.That(this.Add2LinkedList(new ListNode(new List<int>() { 9, 9, 1 }), new ListNode(1)).GetThisAndAfterInString(), Is.EqualTo("0 0 2 "));
            //Assert.That(this.Add2LinkedList(new ListNode(1), new ListNode(new List<int>() { 9, 9, 1 })).GetThisAndAfterInString(), Is.EqualTo("0 0 2 "));
            //Assert.That(this.Add2LinkedList(new ListNode(1), new ListNode(new List<int>() { 9, 9, 9 })).GetThisAndAfterInString(), Is.EqualTo("0 0 0 1 "));
            #endregion

            #region "reverse linked list"
            //Assert.That(start.GetThisAndAfterInList, Is.EquivalentTo(new List<int>() { 1, 3, 5, 7, 9, 11, 13, 15 }));
            //Console.WriteLine("reversing: {0}", start.GetThisAndAfterInString());
            //Assert.That(this.ReversePartialLinkedList(start, 3, 6).GetThisAndAfterInList, Is.EquivalentTo(new List<int>() { 1, 3, 11, 9, 7, 5, 13, 15 }));
            //Console.WriteLine("reversing: {0}", start.GetThisAndAfterInString());
            //Assert.That(this.ReversePartialLinkedList(start, 1, 2).GetThisAndAfterInList, Is.EquivalentTo(new List<int>() { 3, 1, 5, 7, 9, 11, 13, 15 }));
            //Console.WriteLine("reversing: {0}", start.GetThisAndAfterInString());
            //Assert.That(this.ReversePartialLinkedList(start, 2, 3).GetThisAndAfterInList, Is.EquivalentTo(new List<int>() {1, 5, 3 }));
            #endregion

            #region "longest palindrome"
            //Assert.That(this.LongestPalindrome("aaaabaaa"), Is.EqualTo("aaabaaa"));
            //Assert.That(this.LongestPalindrome("abb"), Is.EqualTo("bb"));
            #endregion

            #region "atoi"
            //Assert.That(this.Atoi("9 2704"), Is.EqualTo(9));
            //Assert.That(this.Atoi("123e"), Is.EqualTo(123));
            //Assert.That(this.Atoi("-123e"), Is.EqualTo(-123));
            //Assert.That(this.Atoi("1234567890"), Is.EqualTo(1234567890));
            //Assert.That(this.Atoi("-1234567890"), Is.EqualTo(-1234567890));
            //Assert.That(this.Atoi("12345678901"), Is.EqualTo(2147483647));
            //Assert.That(this.Atoi("-12345678901"), Is.EqualTo(-2147483648));
            //Assert.That(this.Atoi("+7"), Is.EqualTo(7));
            #endregion

            #region "pretty JSON"
            //logger.InfoFormat("Printing results for [Pretty JSON]\n{0}", this.PrettyJson(@"{A:""B"",C:{D:""E"",F:{G:""H"",I:""J""}}}").ToString());
            //logger.InfoFormat("Printing results for [Pretty JSON]\n{0}", this.PrettyJson(@"[""foo"", {""bar"":[""baz"",null,1.0,2]}]").ToString());
            //logger.InfoFormat("Printing results for [Pretty JSON]\n{0}", this.PrettyJson(@"{""attributes"":[{""nm"":""ACCOUNT"",""lv"":[{""v"":{""Id"":null,""State"":null},""vt"":""java.util.Map"",""cn"":1}],""vt"":""java.util.Map"",""status"":""SUCCESS"",""lmd"":13585},{""nm"":""PROFILE"",""lv"":[{""v"":{""Party"":null,""Ads"":null},""vt"":""java.util.Map"",""cn"":2}],""vt"":""java.util.Map"",""status"":""SUCCESS"",""lmd"":41962}]}").ToString());
            #endregion

            #region "str str"
            //Assert.That(this.StrStr("abcabcd", "abcd"), Is.EqualTo(3));
            #endregion

            #region "total diff bits"
            //Assert.That(this.TotalDiffBits(new List<int>() { 1, 3, 5 }), Is.EqualTo(8));
            #endregion

            #region "find single from triples"
            //Assert.That(this.FindSingleFromTriples(new List<int>() { 2, 2, 2, 3, 3, 3, 5, 5, 5, 7 }), Is.EqualTo(7));
            //Assert.That(this.FindSingleFromTriples(new List<int>() { 890, 992, 172, 479, 973, 901, 417, 215, 901, 283, 788, 102, 726, 609, 379, 587, 630, 283, 10, 707, 203, 417, 382, 601, 713, 290, 489, 374, 203, 680, 108, 463, 290, 290, 382, 886, 584, 406, 809, 601, 176, 11, 554, 801, 166, 303, 308, 319, 172, 619, 400, 885, 203, 463, 303, 303, 885, 308, 460, 283, 406, 64, 584, 973, 572, 194, 383, 630, 395, 901, 992, 973, 938, 609, 938, 382, 169, 707, 680, 965, 726, 726, 890, 383, 172, 102, 10, 308, 10, 102, 587, 809, 460, 379, 713, 890, 463, 108, 108, 811, 176, 169, 313, 886, 400, 319, 22, 885, 572, 64, 120, 619, 313, 3, 460, 713, 811, 965, 479, 3, 247, 886, 120, 707, 120, 176, 374, 609, 395, 811, 406, 809, 801, 554, 3, 194, 11, 587, 169, 215, 313, 319, 554, 379, 788, 194, 630, 601, 965, 417, 788, 479, 64, 22, 22, 489, 166, 938, 66, 801, 374, 66, 619, 489, 215, 584, 383, 66, 680, 395, 400, 166, 572, 11, 992 }), Is.EqualTo(247));
            #endregion

            #region "min xor"
            //Assert.That(this.MinXor(new List<int>() { 0, 2, 5, 7 }), Is.EqualTo(2));
            //Assert.That(this.MinXor(new List<int>() { 2, 4, 7, 9 }), Is.EqualTo(3));
            #endregion

            #region "3 sum 0"
            //List<List<int>> result = ThreeSum0(new List<int> { 1, -4, 0, 0, 5, -5, 1, 0, -2, 4, -4, 1, -1, -4, 3, 4, -1, -1, -3 });
            #endregion

            #region "3 sum closest"
            //Assert.That(this.threeSumClosest(new List<int>() { 5, -2, -1, -10, 10 }, 5), Is.EqualTo(5));
            #endregion

            #region "diff possible"
            //Assert.That(this.DiffPossible(new List<int>() { 1, 3, 5 }, 2), Is.EqualTo(1));
            //Assert.That(this.DiffPossible(new List<int>() { 1, 2, 2, 3, 4 }, 0), Is.EqualTo(1));
            //Assert.That(this.DiffPossible(new List<int>() { 1, 2, 3 }, 0), Is.EqualTo(0));
            #endregion

            #region "square root"
            //Assert.That(this.sqrt(2), Is.EqualTo(1));
            //Assert.That(this.sqrt(11), Is.EqualTo(3));
            //Assert.That(this.sqrt(930675566), Is.EqualTo(30506));
            #endregion

            #region "binary count"
            //Assert.That(this.BinaryCount(new List<int>() { 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 5, 5, 6, 6, 6, 6, 6, 6, 7, 7, 8, 8, 8, 9, 9, 9, 10, 10, 10 }, 4), Is.EqualTo(0));
            #endregion

            #region "write rome"
            //Assert.That(this.WriteRome(5), Is.EqualTo("V"));
            //Assert.That(this.WriteRome(14), Is.EqualTo("XIV"));
            //Assert.That(this.WriteRome(9), Is.EqualTo("IX"));
            #endregion

            #region "read rome"
            //Assert.That(this.ReadRome("I"), Is.EqualTo(1));
            //Assert.That(this.ReadRome("III"), Is.EqualTo(3));
            //Assert.That(this.ReadRome("IV"), Is.EqualTo(4));
            //Assert.That(this.ReadRome("VI"), Is.EqualTo(6));
            //Assert.That(this.ReadRome("XIV"), Is.EqualTo(14));
            //Assert.That(this.ReadRome("XX"), Is.EqualTo(20));
            //Assert.That(this.ReadRome("CMVI"), Is.EqualTo(906));
            #endregion
        }
    }
}
