﻿using Fundamentals.TestDataStructures;
using log4net;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundamentals.TestOnlineJudges
{
    [TestFixture]
    public class TestLeetCode
    {
        private ILog logger = LogManager.GetLogger(typeof(TestLeetCode));

        #region "1 two sum"
        private int[] _1TwoSum(int[] nums, int target)
        {
            int[] result = new int[2];

            Dictionary<int, List<int>> map = new Dictionary<int, List<int>>();
            for (int i = 0; i <= nums.Length - 1; i++)
            {
                if (!map.ContainsKey(nums[i]))
                    map.Add(nums[i], new List<int>());
                map[nums[i]].Add(i);
            }

            List<int> numAsKeys = map.Keys.ToList();
            for (int i = 0; i <= numAsKeys.Count - 1; i++)
            {
                if (map.ContainsKey(target - numAsKeys[i]))
                {
                    result[0] = map[numAsKeys[i]][0];
                    if (numAsKeys[i] * 2 == target)
                    {
                        if (map[numAsKeys[i]].Count < 2)
                            continue;
                        else
                            result[1] = map[target - numAsKeys[i]][1];
                    }
                    else
                        result[1] = map[target - numAsKeys[i]][0];
                    break;
                }
            }

            return result;
        }
        #endregion

        [Test]
        public void TestEasy()
        {
            #region ""

            #endregion

            #region "1 two sum"
            //Assert.That(this._1TwoSum(new int[] { 3, 2, 4 }, 6), Is.EquivalentTo(new int[] { 1, 2 }));
            //Assert.That(this._1TwoSum(new int[] { -1, -2, -3, -4, -5 }, -8), Is.EquivalentTo(new int[] { 2, 4 }));
            //Assert.That(this._1TwoSum(new int[] { 0, 4, 3, 0 }, 0), Is.EqualTo(new int[] { 0, 3 }));
            //Assert.That(this._1TwoSum(new int[] { 3, 3 }, 6), Is.EqualTo(new int[] { 0, 1 }));
            //Assert.That(this._1TwoSum(new int[] { 2, 7, 11, 15 }, 9), Is.EqualTo(new int[] { 0, 1 }));
            #endregion
        }

        #region "49 group anagrams"
        private IList<IList<String>> _49GroupAnagrams(String[] strs)
        {
            IList<IList<String>> result = new List<IList<String>>();

            Dictionary<String, int> map = new Dictionary<String, int>();
            int[] count = new int[26];

            foreach (String str in strs)
            {
                foreach (char c in str)
                    count[c - 'a']++;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i <= count.Length - 1; i++)
                    sb.AppendFormat("{0}{1}", count[i], 'a' + i);

                String key = sb.ToString();
                if (map.ContainsKey(key))
                    result[map[key]].Add(str);
                else
                {
                    result.Add(new List<String>() { str });
                    map.Add(key, result.Count - 1);
                }

                count = new int[26];
            }

            return result;
        }
        #endregion

        #region "200 number of islands"
        private int _200NumberOfIslands(char[,] grid)
        {
            int islands = 0;

            row = grid.GetUpperBound(0);
            col = grid.GetUpperBound(1);

            for (int currentRow = 0; currentRow <= row; currentRow++)
                for (int currentCol = 0; currentCol <= col; currentCol++)
                    if (grid[currentRow, currentCol] == '1')
                    {
                        WalkIsland(grid, currentRow, currentCol);
                        islands++;
                    }

            return islands;
        }

        private int row;
        private int col;

        private void WalkIsland(char[,] grid, int currentRow, int currentCol)
        {
            if (currentRow < 0 || currentCol < 0 || currentRow > row || currentCol > col || grid[currentRow, currentCol] == '0')
                return;
            grid[currentRow, currentCol] = '0';
            WalkIsland(grid, currentRow, currentCol + 1);
            WalkIsland(grid, currentRow, currentCol - 1);
            WalkIsland(grid, currentRow + 1, currentCol);
            WalkIsland(grid, currentRow - 1, currentCol);
        }
        #endregion

        #region "56 merge intervals"
        public class Interval
        {
            public int start;
            public int end;
            public Interval() { start = 0; end = 0; }
            public Interval(int s, int e) { start = s; end = e; }

            public override bool Equals(object obj)
            {
                return this.start == (obj as Interval).start && this.end == (obj as Interval).end;
            }
        }

        private List<Interval> _56MergeIntervals(List<Interval> intervals)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("merging interval: ");
            for (int i = 0; i <= intervals.Count - 1; i++)
                sb.AppendFormat("interval item {0}: {1}-{2}\n", i, intervals[i].start, intervals[i].end);

            if (intervals == null) return null;
            if (intervals.Count <= 1) return intervals;
            intervals = intervals.OrderBy(i => i.start).ToList();

            List<Interval> result = new List<Interval>() { intervals[0] };
            for (int i = 1; i <= intervals.Count - 1; i++)
            {
                if (intervals[i].start > result[result.Count - 1].end)
                    result.Add(intervals[i]);
                else
                    result[result.Count - 1].end = (int)Math.Max(result[result.Count - 1].end, intervals[i].end);
            }

            sb.AppendLine("interval merged: ");
            for (int i = 0; i <= result.Count - 1; i++)
                sb.AppendFormat("interval item {0}: {1}-{2}\n", i, intervals[i].start, intervals[i].end);
            logger.InfoFormat(sb.ToString());

            return result;
        }
        #endregion

        #region "3 sum"
        private IList<IList<int>> _3Sum(int[] nums)
        {
            if (nums == null) return null;

            IList<IList<int>> result = new List<IList<int>>();
            if (nums.Length < 3) return result;

            Array.Sort(nums);
            //nums = nums.OrderBy(n => n).ToArray();
            Dictionary<int, List<int>> map = new Dictionary<int, List<int>>();
            for (int i = 0; i <= nums.Length - 1; i++)
            {
                if (!map.ContainsKey(nums[i]))
                    map.Add(nums[i], new List<int>());
                map[nums[i]].Add(i);
            }

            for (int i = 0; i <= nums.Length - 3 && nums[i] <= 0; i++)
            {
                if (i > 0)
                    if (nums[i] == nums[i - 1])
                        continue;

                for (int j = i + 1; j <= nums.Length - 2 && nums[i] + nums[j] <= 0; j++)
                {
                    if (j > i + 1)
                        if (nums[j] == nums[j - 1])
                            continue;

                    int third = 0 - nums[i] - nums[j];
                    if (!map.ContainsKey(third))
                        continue;

                    int sameAsThrid = 1;
                    if (nums[i] == third) sameAsThrid++;
                    if (nums[j] == third) sameAsThrid++;

                    if (map[third].Count < sameAsThrid)
                        continue;

                    if (map[third][sameAsThrid - 1] < j)
                        continue;

                    result.Add(new List<int>() { nums[i], nums[j], third });
                }
            }

            return result;
        }

        private IList<IList<int>> _3SumLowHighPointer(int[] nums)
        {
            if (nums == null) return null;

            IList<IList<int>> result = new List<IList<int>>();
            if (nums.Length < 3) return result;

            Array.Sort(nums);
            for (int i = 0; i <= nums.Length - 3; i++)
            {
                if (i > 0)
                    if (nums[i] == nums[i - 1])
                        continue;

                int sum = 0 - nums[i];
                int low = i + 1, high = nums.Length - 1;

                while (low < high)
                {
                    if (nums[low] + nums[high] == sum)
                    {
                        result.Add(new List<int>() { nums[i], nums[low], nums[high] });
                        while (low < high && nums[low] == nums[low + 1]) low++;
                        while (low < high && nums[high] == nums[high - 1]) high--;
                        low++;
                        high--;
                    }
                    else if (nums[low] + nums[high] > sum)
                        high--;
                    else
                        low++;
                }
            }

            return result;
        }
        #endregion

        #region "238 product of array except self"
        private int[] _238ProductOfArrayExceptSelf(int[] nums)
        {
            if (nums == null && nums.Length == 1) return null;

            int[] result = new int[nums.Length];
            result[0] = 1;

            for (int i = 1; i <= nums.Length - 1; i++)
                result[i] = result[i - 1] * nums[i - 1];
            for (int i = nums.Length - 2; i >= 0; i--)
            {
                nums[i] *= nums[i + 1];
                result[i] *= nums[i + 1];
            }

            return result;

            /*
            long total = 1;
            int zeroPos = -1;
            for (int i = 0; i <= nums.Length - 1; i++)
            {
                if (nums[i] == 0)
                {
                    if (zeroPos == -1)
                        zeroPos = i;
                    else
                        return new int[nums.Length];
                }
                else
                    total *= nums[i];
            }

            if (zeroPos != -1)
            {
                nums = new int[nums.Length];
                nums[zeroPos] = (int)total;
                return nums;
            }

            for (int i = 0; i <= nums.Length - 1; i++)
                nums[i] = (int)(total / nums[i]);

            return nums;
            */


        }
        #endregion

        #region "17 letter combinations of a phone number"
        private IList<string> _17LetterCombinationsOfAPhoneNumber(string digits)
        {
            IList<string> result = new List<string>();
            if (String.IsNullOrEmpty(digits) || String.IsNullOrWhiteSpace(digits)) return result;

            map = new Dictionary<char, List<string>>()
            {
                {'2', new List<string>(){ "a", "b", "c" }},
                {'3', new List<string>(){ "d", "e", "f" }},
                {'4', new List<string>(){ "g", "h", "i" }},
                {'5', new List<string>(){ "j", "k", "l" }},
                {'6', new List<string>(){ "m", "n", "o" }},
                {'7', new List<string>(){ "p", "q", "r", "s" }},
                {'8', new List<string>(){ "t", "u", "v" }},
                {'9', new List<string>(){ "w", "x", "y", "z" }}
            };

            DigitToChar(result, digits, 0, "");
            return result;
        }

        Dictionary<char, List<string>> map;

        public void DigitToChar(IList<string> result, string digits, int i, string current)
        {
            if (current.Length == digits.Length)
            {
                result.Add(current);
                return;
            }

            foreach (string s in map[digits[i]])
                DigitToChar(result, digits, i + 1, current + s);
        }
        #endregion

        #region "33 Search in Rotated Sorted Array"
        private int _33SearchInRotatedSortedArray(int[] nums, int target)
        {
            if (nums == null) return -1;
            if (nums.Length == 0) return -1;

            int start = 0;
            int end = nums.Length - 1;

            while (start + 1 < end)
            {
                int mid = start + (end - start) / 2;
                if (nums[start] < nums[mid])
                {
                    if (nums[start] <= target && target <= nums[mid])
                        end = mid;
                    else
                        start = mid;
                }
                else
                {
                    if (nums[mid] <= target && target <= nums[end])
                        start = mid;
                    else end = mid;
                }
            }

            if (nums[start] == target) return start;
            else if (nums[end] == target) return end;
            else return -1;
        }
        #endregion

        #region "139 Word Break - didn't finish"
        private bool _139WordBreak(string s, IList<string> wordDict)
        {
            TrieNode root = new TrieNode();
            foreach (string word in wordDict)
                root.AddWord(word);

            bool[] result = new bool[s.Length];
            for (int i = 0; i <= s.Length - 1; i++)
            {
                int end = root.GetBreakPoint(s, i);
                if (end == i) continue;
                for (int j = i; j < end; j++)
                    result[j] = true;
            }
            foreach (bool b in result)
                if (!b) return false;

            return true;
        }

        public class TrieNode
        {
            public char val;
            public bool isEnd;
            public TrieNode[] next;

            public TrieNode()
            {
                this.val = '\0';
                isEnd = false;
                next = new TrieNode[26];
            }

            public TrieNode(char val)
            {
                this.val = val;
                isEnd = false;
                next = new TrieNode[26];
            }

            public void AddWord(string word, int start = 0)
            {
                if (start >= word.Length)
                    return;

                if (next[word[start] - 'a'] == null)
                    next[word[start] - 'a'] = new TrieNode(word[start]);
                next[word[start] - 'a'].AddWord(word, start + 1);

                if (start == word.Length - 1)
                    this.isEnd = true;
            }

            public bool ContainsWord(string word, int start = 0)
            {
                TrieNode node = next[word[start] - 'a'];

                if (node == null)
                    return false;
                else if (start + 1 >= word.Length)
                {
                    if (this.isEnd)
                        return true;
                    return false;
                }
                return node.ContainsWord(word, start + 1);
            }

            public int GetBreakPoint(string word, int start = 0)
            {
                TrieNode node = next[word[start] - 'a'];

                if (node == null)
                    return start;
                else if (start + 1 >= word.Length)
                {
                    if (this.isEnd)
                        return start + 1;
                    return start;
                }
                return node.GetBreakPoint(word, start + 1);
            }
        }
        #endregion

        #region "253 Meeting Rooms 2"
        private int _253MeetingRooms2(Interval[] intervals)
        {
            if (intervals == null) return 0;
            if (intervals.Length == 0 || intervals.Length == 1) return intervals.Length;

            intervals = intervals.OrderBy(i => i.start).ToArray();

            List<List<Interval>> rooms = new List<List<Interval>>() { new List<Interval>() { intervals[0] } };
            for (int i = 1; i <= intervals.Length - 1; i++)
            {
                bool foundRoom = false;
                foreach (List<Interval> room in rooms)
                    if (intervals[i].start >= room[room.Count - 1].end)
                    {
                        room.Add(intervals[i]);
                        foundRoom = true;
                        break;
                    }

                if (!foundRoom)
                    rooms.Add(new List<Interval>() { intervals[i] });
            }

            return rooms.Count;
        }
        #endregion

        #region "50 Pow x and n"
        private double _50PowXAndN(double x, int n)
        {
            double result = MyPowSub(x, (int)Math.Abs((long)n));

            if (n < 0)
                result = 1 / result;

            return (double)Decimal.Round((decimal)result, 5);
        }

        public double MyPowSub(double x, long n)
        {
            if (n == 0) return 1;

            double temp = MyPowSub(x, n / 2);

            if (n % 2 == 0) return temp * temp;
            return temp * temp * x;
        }

        public int GetPercision(double x)
        {
            string[] splited = x.ToString().Split(new char[] { '.' });
            if (splited.Length != 2) return 0;
            return splited[1].Length;
        }
        #endregion

        #region "98 Validate Binary Search Tree"
        private bool _98ValidateBinarySearchTree(TreeNode root)
        {
            if (root == null) return true;
            //return GetMyValidBSTResult(root).Item1;
            return GetValidBSTResult(root, null, null);
        }

        private bool GetValidBSTResult(TreeNode node, int? min, int? max)
        {
            if (node == null) return true;
            if (min.HasValue && node.val <= min) return false;
            if (max.HasValue && node.val >= max) return false;
            return GetValidBSTResult(node.left, min, node.val) && GetValidBSTResult(node.right, node.val, max);
        }

        private Tuple<bool, int, int> GetMyValidBSTResult(TreeNode node)
        {
            if (node == null) return new Tuple<bool, int, int>(true, 0, 0);

            int min, max;

            if (node.left == null)
                min = node.val;
            else
            {
                Tuple<bool, int, int> left = GetMyValidBSTResult(node.left);
                if (!left.Item1 || node.val <= left.Item3) return new Tuple<bool, int, int>(false, 0, 0);
                min = left.Item2;
            }

            if (node.right == null)
                max = node.val;
            else
            {
                Tuple<bool, int, int> right = GetMyValidBSTResult(node.right);
                if (!right.Item1 || node.val >= right.Item2) return new Tuple<bool, int, int>(false, 0, 0);
                max = right.Item3;
            }

            return new Tuple<bool, int, int>(true, min, max);
        }
        #endregion

        #region "535 Encode and Decode TinyURL"
        public class MyTinyUrlGenerator
        {
            private const string domain = @"http://tinyurl.com/";
            private static Dictionary<int, List<string>> map = new Dictionary<int, List<string>>();

            public string encode(string longUrl)
            {
                int hash = longUrl.GetHashCode();
                int index = -1;

                if (map.ContainsKey(hash))
                {
                    for (int i = 0; i <= map[hash].Count - 1; i++)
                        if (map[hash][i].Equals(longUrl))
                        {
                            index = i;
                            break;
                        }

                    if (index == -1)
                    {
                        map[hash].Add(longUrl);
                        index = map[hash].Count - 1;
                    }
                }
                else
                {
                    map.Add(hash, new List<string>() { longUrl });
                    index = 0;
                }

                return String.Format("{0}{1}I{2}", domain, hash, index);
            }

            public string decode(string tinyUrl)
            {
                List<string> tinys = tinyUrl.Replace(domain, "").Split(new char[] { 'I' }).ToList();

                if (tinys.Count != 2)
                    return String.Empty;

                int hash, index;
                if (!Int32.TryParse(tinys[0], out hash) || !Int32.TryParse(tinys[1], out index))
                    return String.Empty;

                if (!map.ContainsKey(hash))
                    return String.Empty;

                if (index >= map[hash].Count)
                    return String.Empty;

                return map[hash][index];
            }
        }

        public class TinyUrlGeneratorWithCustomHashing
        {
            private const string s = "ABCDEFGHIGKLMNOPQRSTUVWXYZabcdefghigklmnopqrstuvwxyz0123456789";
            private const string domain = @"http://tinyurl.com/";
            private static Random r = new Random();

            private Dictionary<string, string> longToShort = new Dictionary<string, string>();
            private Dictionary<string, string> shortToLong = new Dictionary<string, string>();

            public string encode(string longUrl)
            {
                if (longToShort.ContainsKey(longUrl))
                    return longToShort[longUrl];

                string shortUrl = "";
                StringBuilder sb = new StringBuilder();
                do
                {
                    for (int i = 0; i <= 5; i++)
                        sb.Append(s[r.Next() % 62]);
                    shortUrl = sb.ToString();
                }
                while (shortToLong.ContainsKey(shortUrl));

                longToShort.Add(longUrl, shortUrl);
                shortToLong.Add(shortUrl, longUrl);

                return shortUrl;
            }

            public string decode(string shortUrl)
            {
                shortUrl = shortUrl.Replace(domain, "");
                if (shortToLong.ContainsKey(shortUrl))
                    return shortToLong[shortUrl];
                return String.Empty;
            }
        }

        public class TinyUrlGeneratorWithEmbedHashing
        {
            private const string domain = @"http://tinyurl.com/";
            private static Dictionary<string, string> map = new Dictionary<string, string>();

            public string encode(string longUrl)
            {
                string shortUrl = Convert.ToInt32(longUrl.GetHashCode().ToString("X"), 16).ToString();
                map.Add(shortUrl, longUrl);
                return shortUrl;
            }

            public string decode(string shortUrl)
            {
                if (map.ContainsKey(shortUrl))
                    return map[shortUrl];
                return String.Empty;
            }
        }
        #endregion

        #region "215 Kth Largest Element in an Array"
        private int _215KthLargestElementInAnArray(int[] nums, int k)
        {
            Array.Sort(nums);
            return nums[nums.Length - k];
        }
        #endregion

        #region "380 Insert Delete GetRandom O(1)"
        public class MyRandomizedSet
        {
            private Dictionary<int, int> map;
            private List<int?> list;
            private Stack<int> stack;
            private Random r;

            public MyRandomizedSet()
            {
                this.map = new Dictionary<int, int>();
                this.list = new List<int?>();
                this.stack = new Stack<int>();
                this.r = new Random();
            }

            public bool insert(int val)
            {
                if (map.ContainsKey(val))
                    return false;

                int index = -1;
                if (stack.Count == 0)
                {
                    index = list.Count;
                    list.Add(val);
                }
                else
                {
                    index = stack.Pop();
                    list[index] = val;
                }
                map.Add(val, index);

                return true;
            }

            public bool remove(int val)
            {
                if (!map.ContainsKey(val))
                    return false;

                list[map[val]] = null;
                stack.Push(map[val]);
                map.Remove(val);

                return true;
            }

            public int GetRandom()
            {
                int? result = null;
                do
                {
                    result = list[r.Next() % list.Count];
                }
                while (!result.HasValue);
                return result.Value;
            }
        }

        public class RandomizedSet
        {
            private Dictionary<int, int> map;
            private List<int> list;
            private Random r;

            public RandomizedSet()
            {
                this.map = new Dictionary<int, int>();
                this.list = new List<int>();
                this.r = new Random();
            }

            public bool Insert(int val)
            {
                if (map.ContainsKey(val))
                    return false;

                map.Add(val, list.Count);
                list.Add(val);

                return true;
            }

            public bool Remove(int val)
            {
                if (!map.ContainsKey(val))
                    return false;

                int last = list[list.Count - 1];
                list[map[val]] = last;
                map[last] = map[val];
                map.Remove(val);
                list.RemoveAt(list.Count - 1);

                return true;
            }

            public int GetRandom()
            {
                return list[r.Next() % list.Count];
            }
        }
        #endregion

        [Test]
        public void TestMedium()
        {
            #region ""

            #endregion

            #region "380 Insert Delete GetRandom O(1)"
            RandomizedSet randomSet = new RandomizedSet();

            randomSet = new RandomizedSet();
            Assert.True(randomSet.Insert(3));
            Assert.True(randomSet.Insert(-2));
            Assert.False(randomSet.Remove(2));
            Assert.True(randomSet.Insert(1));
            Assert.True(randomSet.Insert(-3));
            Assert.False(randomSet.Insert(-2));
            Assert.True(randomSet.Remove(-2));
            Assert.True(randomSet.Remove(3));
            Assert.True(randomSet.Insert(-1));
            Assert.True(randomSet.Remove(-3));
            Assert.False(randomSet.Insert(1));
            Assert.True(randomSet.Insert(-2));
            Assert.False(randomSet.Insert(-2));
            Assert.False(randomSet.Insert(-2));
            Assert.False(randomSet.Insert(1));
            Assert.That(randomSet.GetRandom(), Is.EqualTo(1).Or.EqualTo(-2));
            Assert.False(randomSet.Insert(-2));
            Assert.False(randomSet.Remove(0));
            Assert.True(randomSet.Insert(-3));
            Assert.False(randomSet.Insert(1));

            randomSet = new RandomizedSet();
            Assert.True(randomSet.Insert(1));
            Assert.False(randomSet.Remove(2));
            Assert.True(randomSet.Insert(2));
            Assert.That(randomSet.GetRandom(), Is.EqualTo(1).Or.EqualTo(2));
            Assert.True(randomSet.Remove(1));
            Assert.False(randomSet.Insert(2));
            Assert.That(randomSet.GetRandom(), Is.EqualTo(2));
            #endregion

            #region "215 Kth Largest Element in an Array"
            //Assert.That(this._215KthLargestElementInAnArray(new int[] { 3, 2, 1, 5, 6, 4 }, 2), Is.EqualTo(5));
            //Assert.That(this._215KthLargestElementInAnArray(new int[] { 3, 2, 3, 1, 2, 4, 5, 5, 6 }, 4), Is.EqualTo(4));
            #endregion

            #region "535 Encode and Decode TinyURL"
            //MyTinyUrlGenerator myTiny = new MyTinyUrlGenerator();
            //TinyUrlGeneratorWithCustomHashing tinyCustom = new TinyUrlGeneratorWithCustomHashing();
            //TinyUrlGeneratorWithEmbedHashing tinyEmbed = new TinyUrlGeneratorWithEmbedHashing();

            //String url = @"https://leetcode.com/problems/design-tinyurl";
            //Assert.That(myTiny.decode(myTiny.encode(url)), Is.EqualTo(url));
            //Assert.That(tinyCustom.decode(tinyCustom.encode(url)), Is.EqualTo(url));
            //Assert.That(tinyEmbed.decode(tinyEmbed.encode(url)), Is.EqualTo(url));

            //url = @"https://leetcode.com/problems/what-the-heck";
            //Assert.That(myTiny.decode(myTiny.encode(url)), Is.EqualTo(url));
            //Assert.That(tinyCustom.decode(tinyCustom.encode(url)), Is.EqualTo(url));
            //Assert.That(tinyEmbed.decode(tinyEmbed.encode(url)), Is.EqualTo(url));

            //url = @"https://leetcode.com/problems/haha-no-way";
            //Assert.That(myTiny.decode(myTiny.encode(url)), Is.EqualTo(url));
            //Assert.That(tinyCustom.decode(tinyCustom.encode(url)), Is.EqualTo(url));
            //Assert.That(tinyEmbed.decode(tinyEmbed.encode(url)), Is.EqualTo(url));
            #endregion

            #region "98 Validate Binary Search Tree"
            //TreeNode root = new TreeNode();

            ////root = new TreeNode(new List<int>() { 2, 1, 3 });
            ////Assert.That(this._98ValidateBinarySearchTree(root), Is.EqualTo(true));

            //root = new TreeNode(new List<int>() { 10, 5, 15, -1, -1, 6, 20 });
            //Assert.That(this._98ValidateBinarySearchTree(root), Is.EqualTo(false));

            //root = new TreeNode(new List<int>() { 1, 1, -1 });
            //Assert.That(this._98ValidateBinarySearchTree(root), Is.EqualTo(false));

            //root = new TreeNode(new List<int>() { 5, 1, 4, -1, -1, 3, 6 });
            //Assert.That(this._98ValidateBinarySearchTree(root), Is.EqualTo(false));

            //root = new TreeNode(new List<int>() { 6, 4, 8, 2, 5, 7, 10, 1, 3, -1, -1, -1, -1, 11, 9 });
            //Assert.That(this._98ValidateBinarySearchTree(root), Is.EqualTo(false));

            //Assert.That(this._98ValidateBinarySearchTree(null), Is.EqualTo(true));
            #endregion

            #region "50 Pow x and n"
            //Assert.That(this._50PowXAndN(1.00000, -2147483648), Is.EqualTo(1));
            //Assert.That(this._50PowXAndN(2.10000, 1), Is.EqualTo(2.1));
            //Assert.That(this._50PowXAndN(2.10000, 0), Is.EqualTo(1));
            //Assert.That(this._50PowXAndN(2.00000, 10), Is.EqualTo(1024));
            //Assert.That(this._50PowXAndN(2.10000, 3), Is.EqualTo(9.261));
            //Assert.That(this._50PowXAndN(2.00000, -2), Is.EqualTo(0.25));
            #endregion

            #region "253 Meeting Rooms 2"
            //Assert.That(this._253MeetingRooms2(new Interval[] { new Interval(0, 30), new Interval(5, 10), new Interval(15, 20) }), Is.EqualTo(2));
            //Assert.That(this._253MeetingRooms2(new Interval[] { new Interval(7, 10), new Interval(2, 4) }), Is.EqualTo(1));
            #endregion

            #region "139 Word Break - didn't finish"
            //Assert.That(this._139WordBreak("abcd", new List<string>() { "a", "abc", "b", "cd" }), Is.EqualTo(true));
            //Assert.That(this._139WordBreak("leetcode", new List<string>() { "leet", "code" }), Is.EqualTo(true));
            //Assert.That(this._139WordBreak("applepenapple", new List<string>() { "apple", "pen" }), Is.EqualTo(true));
            //Assert.That(this._139WordBreak("catsandog", new List<string>() { "cats", "dog", "sand", "and", "cat" }), Is.EqualTo(false));
            #endregion

            #region "33 Search in Rotated Sorted Array"
            //Assert.That(this._33SearchInRotatedSortedArray(new int[] { 4, 5, 6, 7, 8, 1, 2, 3 }, 8), Is.EqualTo(4));
            //Assert.That(this._33SearchInRotatedSortedArray(new int[] { 1, 3 }, 3), Is.EqualTo(1));
            //Assert.That(this._33SearchInRotatedSortedArray(null, 6), Is.EqualTo(-1));
            //Assert.That(this._33SearchInRotatedSortedArray(new int[] { }, 6), Is.EqualTo(-1));
            //Assert.That(this._33SearchInRotatedSortedArray(new int[] { 4, 5, 6, 7, 0, 1, 2 }, 6), Is.EqualTo(2));
            //Assert.That(this._33SearchInRotatedSortedArray(new int[] { 4, 5, 6, 7, 0, 1, 2 }, 0), Is.EqualTo(4));
            //Assert.That(this._33SearchInRotatedSortedArray(new int[] { 4, 5, 6, 7, 0, 1, 2 }, 3), Is.EqualTo(-1));
            #endregion

            #region "17 letter combinations of a phone number"
            //Assert.That(this._17LetterCombinationsOfAPhoneNumber("23"), Is.EquivalentTo(new List<string>() { "ad", "ae", "af", "bd", "be", "bf", "cd", "ce", "cf" }));
            #endregion

            #region "238 product of array except self"
            //Assert.That(this._238ProductOfArrayExceptSelf(new int[] { 1, 0, 0, 4 }), Is.EquivalentTo(new int[] { 0, 0, 0, 0 }));
            //Assert.That(this._238ProductOfArrayExceptSelf(new int[] { 1, 2, 0, 4 }), Is.EquivalentTo(new int[] { 0, 0, 8, 0 }));
            //Assert.That(this._238ProductOfArrayExceptSelf(new int[] { 1, 2, 3, 4 }), Is.EquivalentTo(new int[] { 24, 12, 8, 6 }));
            #endregion

            #region "3 sum"
            //Assert.That(this._3SumLowHighPointer(new int[] { -2, 0, 0, 2, 2 }), Is.EquivalentTo(new List<IList<int>>() { new List<int>() { -2, 0, 2 } }));
            //Assert.That(this._3SumLowHighPointer(new int[] { 0, 0, 0, 0, 5 }), Is.EquivalentTo(new List<IList<int>>() { new List<int>() { 0, 0, 0 } }));
            //Assert.That(this._3SumLowHighPointer(new int[] { -1, 0, 0, 1, -4 }), Is.EquivalentTo(new List<IList<int>>() { new List<int>() { -1, 0, 1 } }));
            //Assert.That(this._3SumLowHighPointer(new int[] { -1, 0, 1, 2, -1, -4 }), Is.EquivalentTo(new List<IList<int>>() { new List<int>() { -1, 0, 1 }, new List<int>() { -1, -1, 2 } }));

            //Assert.That(this._3Sum(new int[] { 0, 0, 0, 0, 5 }), Is.EquivalentTo(new List<IList<int>>() { new List<int>() { 0, 0, 0 } }));
            //Assert.That(this._3Sum(new int[] { -1, 0, 0, 1, -4 }), Is.EquivalentTo(new List<IList<int>>() { new List<int>() { -1, 0, 1 } }));
            //Assert.That(this._3Sum(new int[] { -1, 0, 1, 2, -1, -4 }), Is.EquivalentTo(new List<IList<int>>() { new List<int>() { -1, 0, 1 }, new List<int>() { -1, -1, 2 } }));
            #endregion

            #region "56 merge intervals"
            //Assert.That(this._56MergeIntervals(new List<Interval>() { new Interval(1, 3), new Interval(1, 2) }), Is.EquivalentTo(new List<Interval>() { new Interval(1, 3) }));
            //Assert.That(this._56MergeIntervals(new List<Interval>() { new Interval(1, 3), new Interval(1, 3) }), Is.EquivalentTo(new List<Interval>() { new Interval(1, 3) }));
            //Assert.That(this._56MergeIntervals(new List<Interval>() { new Interval(1, 3), new Interval(1, 4) }), Is.EquivalentTo(new List<Interval>() { new Interval(1, 4) }));

            //Assert.That(this._56MergeIntervals(new List<Interval>() { new Interval(1, 3), new Interval(2, 2) }), Is.EquivalentTo(new List<Interval>() { new Interval(1, 3) }));
            //Assert.That(this._56MergeIntervals(new List<Interval>() { new Interval(1, 3), new Interval(2, 3) }), Is.EquivalentTo(new List<Interval>() { new Interval(1, 3) }));
            //Assert.That(this._56MergeIntervals(new List<Interval>() { new Interval(1, 3), new Interval(2, 4) }), Is.EquivalentTo(new List<Interval>() { new Interval(1, 4) }));

            //Assert.That(this._56MergeIntervals(new List<Interval>() { new Interval(1, 3), new Interval(4, 5) }), Is.EquivalentTo(new List<Interval>() { new Interval(1, 3), new Interval(4, 5) }));

            //Assert.That(this._56MergeIntervals(new List<Interval>() { new Interval(1, 3), new Interval(2, 6), new Interval(8, 10), new Interval(15, 18) }), Is.EquivalentTo(new List<Interval>() { new Interval(1, 6), new Interval(8, 10), new Interval(15, 18) }));
            #endregion

            #region "200 number of islands"
            //Assert.That(new char[,] {
            //    {'1', '2', '1', '1', '0' },
            //    {'3', '1', '0', '1', '0' },
            //    {'1', '1', '0', '0', '0' },
            //    {'0', '0', '0', '0', '0' }
            //}[0,1], Is.EqualTo('2'));
            //Assert.That(this._200NumberOfIslands(new char[,] {
            //    {'1', '1', '1', '1', '0' },
            //    {'1', '1', '0', '1', '0' },
            //    {'1', '1', '0', '0', '0' },
            //    {'0', '0', '0', '0', '0' }
            //}), Is.EqualTo(1));
            //Assert.That(this._200NumberOfIslands(new char[,] {
            //    {'1', '1', '0', '0', '0' },
            //    {'1', '1', '0', '0', '0' },
            //    {'0', '0', '1', '0', '0' },
            //    {'0', '0', '0', '1', '1' }
            //}), Is.EqualTo(3));
            #endregion

            #region "49 group anagrams"
            //Assert.That(this._49GroupAnagrams(new string[] { "eat", "tea", "tan", "ate", "nat", "bat" }), Is.EquivalentTo(new List<List<String>>()
            //{
            //    new List<string>() {"ate","eat","tea"},
            //    new List<string>() { "nat", "tan" },
            //    new List<string>() { "bat" }
            //}));
            #endregion
        }
    }
}