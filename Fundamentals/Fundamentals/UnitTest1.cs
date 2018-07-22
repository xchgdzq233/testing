using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Fundamentals
{
    [TestFixture]
    public class UnitTest1
    {
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

        [Test]
        public void TestMethod1()
        {
            List<List<List<int>>> inputs = new List<List<List<int>>>()
            {
                new List<List<int>>() { new List<int>() { 11 } },

                new List<List<int>>() { new List<int>() { 11, 12 } },
                new List<List<int>>() { new List<int>() { 11 }, new List<int>() { 21 } },

                new List<List<int>>() { new List<int>() { 11, 12 }, new List<int>() { 21, 22 } },

                new List<List<int>>() { new List<int>() { 11, 12, 13 } },
                new List<List<int>>() { new List<int>() { 11 }, new List<int>() { 21 }, new List<int>() { 31 } },

                new List<List<int>>() { new List<int>() { 11, 12, 13 }, new List<int>() { 21, 22, 23 } },
                new List<List<int>>() { new List<int>() { 11, 12 }, new List<int>() { 21, 22 }, new List<int>() { 31, 32 } },

                new List<List<int>>() { new List<int>() { 11, 12, 13 }, new List<int>() { 21, 22, 23 }, new List<int>() { 31, 32, 33 } },

                new List<List<int>>()
                {
                    new List<int>() { 335, 401, 128, 384, 345, 275, 324, 139, 127, 343, 197, 177, 127, 72, 13, 59 },
                    new List<int>() { 102, 75, 151, 22, 291, 249, 380, 151, 85, 217, 246, 241, 204, 197, 227, 96 },
                    new List<int>() { 261, 163, 109, 372, 238, 98, 273, 20, 233, 138, 40, 246, 163, 191, 109, 237 },
                    new List<int>() { 179, 213, 214, 9, 309, 210, 319, 68, 400, 198, 323, 135, 14, 141, 15, 168 },
                }
            };

            List<List<int>> outputs = new List<List<int>>()
            {
                new List<int>() { 11 },

                new List<int>() { 11, 12},
                new List<int>() { 11, 21},

                new List<int>() { 11, 12, 22, 21},

                new List<int>() { 11, 12, 13 },
                new List<int>() { 11, 21, 31 },

                new List<int>() { 11, 12, 13, 23, 22, 21 },
                new List<int>() { 11, 12, 22, 32, 31, 21 },

                new List<int>() { 11, 12, 13, 23, 33, 32, 31, 21, 22 },

                new List<int>() { 335, 401, 128, 384, 345, 275, 324, 139, 127, 343, 197, 177, 127, 72, 13, 59, 96, 237, 168, 15, 141, 14, 135, 323, 198, 400, 68, 319, 210, 309, 9, 214, 213, 179, 261, 102, 75, 151, 22, 291, 249, 380, 151, 85, 217, 246, 241, 204, 197, 227, 109, 191, 163, 246, 40, 138, 233, 20, 273, 98, 238, 372, 109, 163 }
            };

            for (int i = 0; i < inputs.Count; i++)
                Assert.That(this.spiralOrder(inputs[i]), Is.EquivalentTo(outputs[i]));
        }

        [Test]
        public void TestMethod2()
        {
            int A = 5;
            List<List<int>> result = new List<List<int>>();

            for (int i = 0; i <= A - 1; i++)
            {
                result.Add(new List<int>());
                result[i].Add(1);
                for (int j = 1; j < i; j++)
                    result[i].Add(result[i - 1][j - 1] + result[i - 1][j]);
                if (i > 0)
                    result[i].Add(1);
            }
        }

        private int FindFirstMissingPositive(List<int> A)
        {
            int max = 0;
            for (int i = 0; i <= A.Count - 1; i++)
            {
                if (A[i] == max + 1)
                {
                    A[i] = 0;
                    max++;
                }
                else if (A[i] <= 0 || A[i] > A.Count || A[i] == i + 1)
                    continue;
                else if (A[A[i] - 1] == A[i])
                    A[i] = 0;
                else
                {
                    int hold = A[A[i] - 1];
                    A[A[i] - 1] = A[i];
                    A[i] = hold;
                    i--;
                }
            }

            for (int i = Math.Max(max, 0); i <= A.Count - 1; i++)
            {
                if (A[i] <= 0 || A[i] > A.Count)
                    break;
                max++;
            }

            return max + 1;
        }

        [Test]
        public void TestMethod3()
        {
            List<int> a1 = new List<int>() { 1, 2, 0 };
            List<int> a2 = new List<int>() { 3, 4, -1, 1 };
            List<int> a3 = new List<int>() { -8, -7, -6 };
            List<int> a4 = new List<int>() { 417, 929, 845, 462, 675, 175, 73, 867, 14, 201, 777, 407, 80, 882, 785, 563, 209, 261, 776, 362, 730, 74, 649, 465, 353, 801, 503, 154, 998, 286, 520, 692, 68, 805, 835, 210, 819, 341, 564, 215, 984, 643, 381, 793, 726, 213, 866, 706, 97, 538, 308, 797, 883, 59, 328, 743, 694, 607, 729, 821, 32, 672, 130, 13, 76, 724, 384, 444, 884, 192, 917, 75, 551, 96, 418, 840, 235, 433, 290, 954, 549, 950, 21, 711, 781, 132, 296, 44, 439, 164, 401, 505, 923, 136, 317, 548, 787, 224, 23, 185, 6, 350, 822, 457, 489, 133, 31, 830, 386, 671, 999, 255, 222, 944, 952, 637, 523, 494, 916, 95, 734, 908, 90, 541, 470, 941, 876, 264, 880, 761, 535, 738, 128, 772, 39, 553, 656, 603, 868, 292, 117, 966, 259, 619, 836, 818, 493, 592, 380, 500, 599, 839, 268, 67, 591, 126, 773, 635, 800, 842, 536, 668, 896, 260, 664, 506, 280, 435, 618, 398, 533, 647, 373, 713, 745, 478, 129, 844, 640, 886, 972, 62, 636, 79, 600, 263, 52, 719, 665, 376, 351, 623, 276, 66, 316, 813, 663, 831, 160, 237, 567, 928, 543, 508, 638, 487, 234, 997, 307, 480, 620, 890, 216, 147, 271, 989, 872, 994, 488, 291, 331, 8, 769, 481, 924, 166, 89, 824, -4, 590, 416, 17, 814, 728, 18, 673, 662, 410, 727, 667, 631, 660, 625, 683, 33, 436, 930, 91, 141, 948, 138, 113, 253, 56, 432, 744, 302, 211, 262, 968, 945, 396, 240, 594, 684, 958, 343, 879, 155, 395, 288, 550, 482, 557, 826, 598, 795, 914, 892, 690, 964, 981, 150, 179, 515, 205, 265, 823, 799, 190, 236, 24, 498, 229, 420, 753, 936, 191, 366, 935, 434, 311, 920, 167, 817, 220, 219, 741, -2, 674, 330, 909, 162, 443, 412, 974, 294, 864, 971, 760, 225, 681, 689, 608, 931, 427, 687, 466, 894, 303, 390, 242, 339, 252, 20, 218, 499, 232, 184, 490, 4, 957, 597, 477, 354, 677, 691, 25, 580, 897, 542, 186, 359, 346, 409, 655, 979, 853, 411, 344, 358, 559, 765, 383, 484, 181, 82, 514, 582, 593, 77, 228, 921, 348, 453, 274, 449, 106, 657, 783, 782, 811, 333, 305, 784, 581, 746, 858, 249, 479, 652, 270, 429, 614, 903, 102, 378, 575, 119, 196, 12, 990, 356, 277, 169, 70, 518, 282, 676, 137, 622, 616, 357, 913, 161, 3, 589, 327 };
            List<int> a5 = new List<int>() { 1, 1, 1 };

            Assert.That(this.FindFirstMissingPositive(a1), Is.EqualTo(3));
            Assert.That(this.FindFirstMissingPositive(a2), Is.EqualTo(2));
            Assert.That(this.FindFirstMissingPositive(a3), Is.EqualTo(1));
            Assert.That(this.FindFirstMissingPositive(a4), Is.EqualTo(1));
            Assert.That(this.FindFirstMissingPositive(a5), Is.EqualTo(2));
        }

        [Test]
        public void TestMethod4()
        {
            int A = 36;

            Queue<int> left = new Queue<int>();
            Stack<int> right = new Stack<int>();

            for (int i = 1; Math.Pow(i, 2) <= A; i++)
                if (A % i == 0)
                {
                    left.Enqueue(i);
                    if (Math.Pow(i, 2) != A)
                        right.Push(A / i);
                }

            List<int> result = new List<int>();

            while (left.Count != 0)
                result.Add(left.Dequeue());
            while (right.Count != 0)
                result.Add(right.Pop());
        }
    }
}
