using Fundamentals.TestDataStructures;
using log4net;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundamentals.TestOnlineJudges
{
    public class UndirectedGraphNode
    {
        public int label;
        public List<UndirectedGraphNode> neighbors;

        public UndirectedGraphNode()
        {
            this.neighbors = new List<UndirectedGraphNode>();
        }

        public UndirectedGraphNode(int val)
        {
            this.label = val;
            this.neighbors = new List<UndirectedGraphNode>();
        }

        public static UndirectedGraphNode CloneNodeDfs(UndirectedGraphNode node, Dictionary<int, UndirectedGraphNode> cloned = null)
        {
            if (node == null) return null;
            if (cloned == null) cloned = new Dictionary<int, UndirectedGraphNode>();
            if (cloned.ContainsKey(node.label)) return cloned[node.label];

            UndirectedGraphNode dup = new UndirectedGraphNode(node.label);
            cloned.Add(node.label, dup);

            foreach (UndirectedGraphNode neighbour in node.neighbors)
            {
                UndirectedGraphNode neighbourDup = CloneNodeDfs(neighbour, cloned);
                dup.neighbors.Add(neighbourDup);
            }

            return dup;
        }

        public static UndirectedGraphNode CloneNodeBfs(UndirectedGraphNode node)
        {
            if (node == null) return null;

            Dictionary<int, UndirectedGraphNode> cloned = new Dictionary<int, UndirectedGraphNode>();
            Queue<UndirectedGraphNode> nodesToClone = new Queue<UndirectedGraphNode>();
            nodesToClone.Enqueue(node);

            UndirectedGraphNode result = null;
            while (nodesToClone.Count != 0)
            {
                UndirectedGraphNode nodeToClone = nodesToClone.Dequeue();
                UndirectedGraphNode dup;

                if (cloned.ContainsKey(nodeToClone.label))
                    dup = cloned[nodeToClone.label];
                else
                {
                    dup = new UndirectedGraphNode(nodeToClone.label);
                    if (cloned.Count == 0) result = dup;
                    cloned.Add(nodeToClone.label, dup);
                }

                foreach (UndirectedGraphNode neighbourToClone in nodeToClone.neighbors)
                {
                    UndirectedGraphNode neighbourDup;
                    if (cloned.ContainsKey(neighbourToClone.label))
                        neighbourDup = cloned[neighbourToClone.label];
                    else
                    {
                        neighbourDup = new UndirectedGraphNode(neighbourToClone.label);
                        cloned.Add(neighbourToClone.label, neighbourDup);
                        nodesToClone.Enqueue(neighbourToClone);
                    }

                    dup.neighbors.Add(neighbourDup);
                }
            }

            return result;
        }

        /// <summary>
        /// template: {0,1,2#1,2#2,2}
        /// explanation: node 0 connect to 1 and 2, node 1 connect to 2, node 2 connect to itself
        /// </summary>
        /// <param name="input"></param>
        public void ConstructGraphByLeetCodeTemplate(string input)
        {
            List<string> nodesInput = input.Split('#').ToList();
            Dictionary<string, UndirectedGraphNode> map = new Dictionary<string, UndirectedGraphNode>();

            foreach (string nodeInput in nodesInput)
            {
                List<string> nodes = nodeInput.Split(',').ToList();
                foreach (string node in nodes)
                {
                    int iNode;
                    if (!Int32.TryParse(node, out iNode))
                        throw new ArgumentException(String.Format("[{0}] is not an integer", node));
                    if (!map.ContainsKey(node))
                    {
                        if (map.Values.Count == 0)
                        {
                            this.label = iNode;
                            map.Add(node, this);
                        }
                        else
                            map.Add(node, new UndirectedGraphNode(iNode));
                    }
                }
                for (int i = 1; i <= nodes.Count - 1; i++)
                {
                    map[nodes[0]].neighbors.Add(map[nodes[i]]);
                    if (!nodes[0].Equals(nodes[i]))
                        map[nodes[i]].neighbors.Add(map[nodes[0]]);
                }
            }
        }

        private int CountDiff(UndirectedGraphNode node)
        {
            int counter = 0;
            string sthis = this.label.ToString();
            string sNode = node.label.ToString();
            for (int i = 0; i <= sthis.Length - 1; i++)
                if (!sthis[i].Equals(sNode[i]))
                    counter++;
            return counter;
        }

        public void AddNeighbourByDiff(UndirectedGraphNode neighbour, int diff = 1)
        {
            if (this.CountDiff(neighbour) != diff)
                return;

            neighbors.Add(neighbour);
            neighbour.neighbors.Add(this);
        }

        public int FindShortestPathToNodeWrong(UndirectedGraphNode node, List<UndirectedGraphNode> nodeToVisit, Dictionary<int, int> visited)
        {
            foreach (UndirectedGraphNode nextNode in nodeToVisit)
            {
                if (!visited.ContainsKey(nextNode.label))
                    visited.Add(nextNode.label, nextNode.label == this.label ? 1 : Int32.MaxValue);

                foreach (UndirectedGraphNode neighbour in nextNode.neighbors)
                {
                    if (!visited.ContainsKey(neighbour.label))
                        visited.Add(neighbour.label, visited[nextNode.label] == Int32.MaxValue ? Int32.MaxValue : visited[nextNode.label] + 1);
                    else
                        visited[neighbour.label] = (int)Math.Min(visited[neighbour.label], visited[nextNode.label] == Int32.MaxValue ? Int32.MaxValue : visited[nextNode.label] + 1);
                }
            }

            return visited[node.label] == Int32.MaxValue ? 0 : visited[node.label];
        }
    }

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

        #region "236 Lowest Common Ancestor of a Binary Tree"
        private TreeNode _236LowestCommonAncestorOfABinaryTree(TreeNode root, TreeNode p, TreeNode q)
        {
            return LowestCommonAncestor(root, p, q);
        }

        private TreeNode LowestCommonAncestor(TreeNode root, TreeNode p, TreeNode q)
        {
            if (root == null) return null;

            if (root.val == p.val || root.val == q.val)
                return root;

            TreeNode left = LowestCommonAncestor(root.left, p, q);
            TreeNode right = LowestCommonAncestor(root.right, p, q);

            if (left != null && right != null)
                return root;
            if (left != null)
                return left;

            return right;
        }
        #endregion

        #region "127 Word Ladder"
        private int _127WordLadder(string beginWord, string endWord, IList<string> wordList)
        {
            HashSet<string> map = new HashSet<string>(wordList);
            if (map.Contains(beginWord)) map.Remove(beginWord);

            Queue<string> q = new Queue<string>();
            q.Enqueue(beginWord);
            int level = 0;

            while (q.Count != 0)
            {
                // check all current level
                level++;
                List<string> levelList = new List<string>();
                while (q.Count != 0)
                {
                    string word = q.Dequeue();
                    for (int i = 0; i <= word.Length - 1; i++)
                    {
                        char[] chars = word.ToCharArray();
                        for (int j = 0; j <= 25; j++)
                        {
                            char newChar = (char)('a' + j);
                            if (newChar == chars[i])
                                continue;
                            chars[i] = newChar;
                            string newWord = new String(chars);
                            if (map.Contains(newWord))
                            {
                                if (endWord == newWord)
                                    return level + 1;
                                else
                                {
                                    levelList.Add(newWord);
                                    map.Remove(newWord);
                                }
                            }
                        }
                    }
                }
                q = new Queue<string>(levelList);
            }

            return 0;
        }

        /*
        private int _127WordLadderWrong(string beginWord, string endWord, IList<string> wordList)
        {
            List<GraphNode> graph = new List<GraphNode>() { new GraphNode(beginWord) };
            GraphNode endGraphNode = null;
            foreach (string word in wordList)
                if (!word.Equals(beginWord))
                {
                    GraphNode newNode = new GraphNode(word);
                    graph.Add(newNode);
                    if (word.Equals(endWord))
                        endGraphNode = newNode;
                }

            if (endGraphNode == null) return 0;

            for (int i = 0; i <= graph.Count - 2; i++)
                for (int j = i + 1; j <= graph.Count - 1; j++)
                    graph[i].AddNeighbourByDiff(graph[j]);

            return graph[0].FindShortestPathToNode(endGraphNode, graph, new Dictionary<string, int>());
        }
        */
        #endregion

        #region "102 Binary Tree Level Order Traversal"
        private IList<IList<int>> _102BinaryTreeLevelOrderTraversal(TreeNode root)
        {
            IList<IList<int>> result = new List<IList<int>>();
            if (root == null) return result;

            Queue<TreeNode> q = new Queue<TreeNode>();
            q.Enqueue(root);

            while (q.Count != 0)
            {
                result.Add(new List<int>());
                Queue<TreeNode> nextQ = new Queue<TreeNode>();

                while (q.Count != 0)
                {
                    TreeNode node = q.Dequeue();
                    result[result.Count - 1].Add(node.val);
                    if (node.left != null) nextQ.Enqueue(node.left);
                    if (node.right != null) nextQ.Enqueue(node.right);
                }

                q = nextQ;
            }

            return result;
        }
        #endregion

        #region "79 Word Search"
        private bool _79WordSearch(char[,] board, string word)
        {
            List<Tuple<int, int>> map = new List<Tuple<int, int>>();

            for (int row = 0; row <= board.GetUpperBound(0); row++)
                for (int col = 0; col <= board.GetUpperBound(1); col++)
                    if (board[row, col] == word[0])
                        if (this._79WordSearchFindWord(board, word, row, col))
                            return true;

            return false;
        }

        private bool _79WordSearchFindWord(char[,] board, string word, int row, int col, int index = 0)
        {
            if (index >= word.Length) return true;
            if (row < 0 || col < 0 || row > board.GetUpperBound(0) || col > board.GetUpperBound(1)) return false;
            if (word[index] != board[row, col]) return false;

            char c = board[row, col];
            board[row, col] = '*';

            bool res = (this._79WordSearchFindWord(board, word, row + 1, col, index + 1))
                || (this._79WordSearchFindWord(board, word, row - 1, col, index + 1))
                || (this._79WordSearchFindWord(board, word, row, col + 1, index + 1))
                || (this._79WordSearchFindWord(board, word, row, col - 1, index + 1));

            board[row, col] = c;
            return res;
        }
        #endregion

        #region "173 Binary Search Tree Iterator"
        /**
         * Definition for binary tree
         * public class TreeNode {
         *     public int val;
         *     public TreeNode left;
         *     public TreeNode right;
         *     public TreeNode(int x) { val = x; }
         * }
         */
        public class BSTIterator
        {
            private Stack<TreeNode> s;

            public BSTIterator(TreeNode root)
            {
                this.s = new Stack<TreeNode>();
                while (root != null)
                {
                    s.Push(root);
                    root = root.left;
                }
            }

            /** @return whether we have a next smallest number */
            public bool HasNext()
            {
                return s.Count != 0;
            }

            /** @return the next smallest number */
            public int Next()
            {
                TreeNode current = this.s.Pop();

                TreeNode right = current.right;
                while (right != null)
                {
                    s.Push(right);
                    right = right.left;
                }

                return current.val;
            }
        }
        /**
         * Your BSTIterator will be called like this:
         * BSTIterator i = new BSTIterator(root);
         * while (i.HasNext()) v[f()] = i.Next();
         */
        #endregion

        #region "208 Implement Trie - Prefix Tree"
        public class Trie
        {
            private Trie[] next;
            private bool end;

            /** Initialize your data structure here. */
            public Trie()
            {
                this.next = new Trie[26];
                this.end = false;
            }

            /** Inserts a word into the trie. */
            public void Insert(string word)
            {
                Trie nextLetter = this;
                for (int i = 0; i <= word.Length - 1; i++)
                {
                    int index = word[i] - 'a';
                    if (nextLetter.next[index] == null)
                        nextLetter.next[index] = new Trie();
                    nextLetter = nextLetter.next[index];
                }

                nextLetter.end = true;
            }

            /** Returns if the word is in the trie. */
            public bool Search(string word)
            {
                Trie nextLetter = this;
                for (int i = 0; i <= word.Length - 1; i++)
                {
                    int index = word[i] - 'a';
                    if (nextLetter.next[index] == null)
                        return false;
                    nextLetter = nextLetter.next[index];
                }
                return nextLetter.end;
            }

            /** Returns if there is any word in the trie that starts with the given prefix. */
            public bool StartsWith(string prefix)
            {
                Trie nextLetter = this;
                for (int i = 0; i <= prefix.Length - 1; i++)
                {
                    int index = prefix[i] - 'a';
                    if (nextLetter.next[index] == null)
                        return false;
                    nextLetter = nextLetter.next[index];
                }
                return true;
            }
        }

        /**
         * Your Trie object will be instantiated and called as such:
         * Trie obj = new Trie();
         * obj.Insert(word);
         * bool param_2 = obj.Search(word);
         * bool param_3 = obj.StartsWith(prefix);
         */
        #endregion

        #region "636 Exclusive Time of Functions"
        private int[] _636ExclusiveTimeOfFunctions(int n, IList<string> logs)
        {
            int[] result = new int[n];
            Stack<int> pausedFuncs = new Stack<int>();

            string[] log = logs[0].Split(':');
            pausedFuncs.Push(Int32.Parse(log[0]));
            int startedAt = Int32.Parse(log[2]);

            for (int i = 1; i <= logs.Count - 1; i++)
            {
                log = logs[i].Split(':');
                int func = Int32.Parse(log[0]);
                int time = Int32.Parse(log[2]);

                if (log[1][0] == 's')
                {
                    if (pausedFuncs.Count != 0)
                        result[pausedFuncs.Peek()] += time - startedAt;
                    pausedFuncs.Push(func);
                    startedAt = time;
                }
                else
                {
                    result[pausedFuncs.Pop()] += time - startedAt + 1;
                    startedAt = time + 1;
                }
            }

            return result;
        }

        private int[] _636MyExclusiveTimeOfFunctions(int n, IList<string> logs)
        {
            int[] result = new int[n];

            //int funcId = -1, startAt = 0;
            Stack<Tuple<int, int>> pausedFuncs = new Stack<Tuple<int, int>>();
            int lastFunc = -1, lastTime = 0;
            foreach (string log in logs)
            {
                string[] splited = log.Split(':');
                int index = Int32.Parse(splited[0]);
                bool start = splited[1].Equals("start");
                int timestamp = Int32.Parse(splited[2]);

                if (start)
                {
                    if (lastFunc != -1)
                        pausedFuncs.Push(new Tuple<int, int>(lastFunc, timestamp - lastTime));
                    else if (pausedFuncs.Count != 0)
                    {
                        Tuple<int, int> pausedFunc = pausedFuncs.Pop();
                        pausedFuncs.Push(new Tuple<int, int>(pausedFunc.Item1, pausedFunc.Item2 + timestamp - lastTime - 1));
                    }
                    lastFunc = index;
                }
                else
                {
                    if (lastFunc != -1)
                        result[index] += (timestamp - lastTime + 1);
                    else
                    {
                        Tuple<int, int> pausedFunc = pausedFuncs.Pop();
                        result[pausedFunc.Item1] += (timestamp - lastTime + pausedFunc.Item2);
                    }
                    lastFunc = -1;
                }

                lastTime = timestamp;
            }

            return result;
        }
        #endregion

        #region "341 Flatten Nested List Iterator"
        /**
         * // This is the interface that allows for creating nested lists.
         * // You should not implement it, or speculate about its implementation
         */
        public class NestedInteger
        {
            private bool isInt;
            private int val;
            private List<NestedInteger> vals;

            private NestedInteger() { }

            public NestedInteger(int input)
            {
                this.isInt = true;
                this.val = input;
                this.vals = new List<NestedInteger>();
            }

            public NestedInteger(List<NestedInteger> input)
            {
                this.isInt = false;
                this.vals = new List<NestedInteger>();
                foreach (NestedInteger i in input)
                {
                    if (i.isInt)
                        this.vals.Add(i);
                    else
                        this.vals.AddRange(i.GetList());
                }
            }

            // @return true if this NestedInteger holds a single integer, rather than a nested list.
            public bool IsInteger()
            {
                return this.isInt;
            }

            // @return the single integer that this NestedInteger holds, if it holds a single integer
            // Return null if this NestedInteger holds a nested list
            public int GetInteger()
            {
                return this.val;
            }

            // @return the nested list that this NestedInteger holds, if it holds a nested list
            // Return null if this NestedInteger holds a single integer
            public IList<NestedInteger> GetList()
            {
                return this.isInt ? null : this.vals;
            }
        }

        public class NestedIterator
        {
            Stack<NestedInteger> s;

            public NestedIterator(IList<NestedInteger> nestedList)
            {
                this.s = new Stack<NestedInteger>();
                for (int i = nestedList.Count - 1; i >= 0; i--)
                    this.s.Push(nestedList[i]);
            }

            public bool HasNext()
            {
                while (this.s.Count != 0)
                {
                    if (s.Peek().IsInteger())
                        return true;
                    IList<NestedInteger> nestedList = this.s.Pop().GetList();
                    for (int i = nestedList.Count - 1; i >= 0; i--)
                        s.Push(nestedList[i]);
                }

                return false;
            }

            public int Next()
            {
                return this.s.Pop().GetInteger();
            }
        }

        /*
         * Your NestedIterator will be called like this:
         * NestedIterator i = new NestedIterator(nestedList);
         * while (i.HasNext()) v[f()] = i.Next();
         */
        #endregion

        #region "78 Subsets"
        private IList<IList<int>> _78Subsets(int[] nums)
        {
            IList<IList<int>> result = new List<IList<int>>();
            result.Add(new List<int>());
            if (nums == null) return result;
            if (nums.Length == 0) return result;
            SubsetsSub(nums, result, new List<int>(), 0);
            return result;
        }

        private void SubsetsSub(int[] nums, IList<IList<int>> result, IList<int> list, int index)
        {
            for (int i = index; i <= nums.Length - 1; i++)
            {
                list.Add(nums[i]);
                result.Add(new List<int>(list));
                SubsetsSub(nums, result, list, i + 1);
                list.RemoveAt(list.Count - 1);
            }
        }
        #endregion

        #region "621 Task Scheduler"
        private int _621TaskScheduler(char[] tasks, int n)
        {
            int[] map = new int[26];

            int max = 0;
            foreach (char c in tasks)
            {
                int i = c - 'A';
                map[i]++;
                max = (int)Math.Max(max, map[i]);
            }

            int count = 0;
            foreach (int i in map)
                if (i == max)
                    count++;

            int atLeast = (max - 1) * (n + 1) + count;
            return (atLeast > tasks.Length) ? atLeast : tasks.Length;
        }
        #endregion

        #region "394 Decode String"
        private string _394DecodeString(string s)
        {
            Stack<string> str = new Stack<string>();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= s.Length - 1; i++)
            {
                if (s[i] != '[' && s[i] != ']')
                    sb.Append(s[i]);
                else if (s[i] == '[')
                {
                    if (sb.Length != 0)
                    {
                        str.Push(sb.ToString());
                        sb = new StringBuilder();
                    }
                }
                else
                {
                    string pre = str.Pop();

                    int total = 0, temp = 0, digit = 0;
                    while (pre.Length > 0)
                    {
                        if (!Int32.TryParse(pre[pre.Length - 1].ToString(), out temp))
                            break;
                        total += temp * (int)Math.Pow(10, digit);
                        pre = pre.Remove(pre.Length - 1);
                        digit++;
                    }

                    string current = sb.ToString();
                    sb = new StringBuilder();
                    sb.Append(pre);
                    for (int j = 1; j <= total; j++)
                        sb.Append(current);
                }
            }

            while (str.Count != 0)
            {
                string temp = sb.ToString();
                sb = new StringBuilder();
                sb.Append(str.Pop());
                sb.Append(temp);
            }
            return sb.ToString();
        }
        #endregion

        #region "277 Find the Celebrity"
        private int asked = 0;
        private bool Knows(int a, int b)
        {
            asked++;
            if (b == 5) return true;
            return false;
        }

        private int _277FindTheCelebrity(int n)
        {
            bool[] notCerl = new bool[n];
            int i = 0;
            for (int j = 1; j < n; j++)
            {
                if (Knows(i, j))
                {
                    notCerl[i] = true;
                    i = j;
                }
                else
                    notCerl[j] = true;
            }
            for (int j = 0; j < n; j++)
            {
                if (i == j)
                    continue;
                if (!Knows(j, i))
                    return -1;
            }

            return i;
        }
        #endregion

        #region "43 Multiply Strings"
        private string _43MultiplyStrings(string num1, string num2)
        {
            if (num1.Equals("0") || num2.Equals("0")) return "0";
            int[] digits = new int[num1.Length + num2.Length];
            for (int i = num1.Length - 1; i >= 0; i--)
                for (int j = num2.Length - 1; j >= 0; j--)
                {
                    int p1 = i + j, p2 = i + j + 1;
                    int sum = (num1[i] - '0') * (num2[j] - '0') + digits[p2];
                    digits[p1] += sum / 10;
                    digits[p2] = sum % 10;
                }

            StringBuilder sb = new StringBuilder();
            foreach (int i in digits)
                if (sb.Length != 0 || i != 0)
                    sb.Append(i);
            return sb.ToString();
        }

        private string _43MyMultiplyStrings(string num1, string num2)
        {
            if (num1.Equals("0") || num2.Equals("0")) return "0";

            int[] nums1 = new int[num1.Length + num2.Length], nums2 = new int[num1.Length + num2.Length];
            for (int i = 0; i <= num1.Length - 1; i++)
                nums1[nums1.Length - 1 - i] = Int32.Parse(num1[num1.Length - 1 - i].ToString());
            for (int i = 0; i <= num2.Length - 1; i++)
                nums2[nums2.Length - 1 - i] = Int32.Parse(num2[num2.Length - 1 - i].ToString());

            int carry = 0;
            StringBuilder result = new StringBuilder();

            for (int digit = 0; digit <= nums1.Length - 1; digit++)
            {
                int current = carry;
                for (int n1 = nums1.Length - 1, n2 = nums2.Length - 1 - digit; n2 <= nums2.Length - 1 && n2 >= 0; n1--, n2++)
                    current += nums1[n1] * nums2[n2];
                carry = current / 10;
                current %= 10;
                if (digit == nums1.Length - 1 && current == 0)
                    break;
                result.Insert(0, current.ToString());
            }
            return result.ToString();
        }
        #endregion

        #region "75 Sort Colors"
        private void _75SortColors(int[] nums)
        {
            int i = 0, left = 0, right = nums.Length - 1;
            while (i <= right)
            {
                if (nums[i] == 0)
                    Swap(nums, left++, i++);
                else if (nums[i] == 1)
                    i++;
                else
                    Swap(nums, right--, i);
            }
        }

        private void Swap(int[] nums, int i, int j)
        {
            int temp = nums[i];
            nums[i] = nums[j];
            nums[j] = temp;
        }

        private void _75MySortColors(int[] nums)
        {
            if (nums.Length == 1 || nums.Length == 0) return;

            int i = 0, count = 0, lastNot2 = nums.Length - 1;
            for (; i <= lastNot2; i++)
            {
                if (nums[i] == 1)
                {
                    count++;
                    nums[i] = 0;
                }
                else if (nums[i] == 2)
                {
                    nums[i] = nums[lastNot2];
                    nums[lastNot2] = 2;
                    lastNot2--;
                    i--;
                }
            }

            for (i = i - 1; i >= 0 && count > 0; i--, count--)
                nums[i] = 1;
        }
        #endregion

        #region "71 Simplify Path"
        private string _71SimplifyPath(string path)
        {
            Stack<string> s = new Stack<string>();
            StringBuilder sb = new StringBuilder();
            if (path[path.Length - 1] != '/') path += '/';
            for (int i = 0; i <= path.Length - 1; i++)
            {
                if (path[i] != '/')
                    sb.Append(path[i]);
                else if (sb.Length != 0)
                {
                    string current = sb.ToString();
                    sb = new StringBuilder();
                    if (current.Equals(".."))
                    {
                        if (s.Count != 0)
                            s.Pop();
                    }
                    else if (current.Equals("."))
                        continue;
                    else
                        s.Push(current);
                }
            }
            if (s.Count == 0) return "/";
            sb = new StringBuilder();
            while (s.Count != 0)
            {
                sb.Insert(0, s.Pop());
                sb.Insert(0, "/");
            }
            return sb.ToString();
        }
        #endregion

        #region "325 Maximum Size Subarray Sum Equals k"
        private int _325MaximumSizeSubarraySumEqualsK(int[] nums, int k)
        {
            if (nums.Length == 0) return 0;

            int sum = 0, max = 0;
            Dictionary<int, int> map = new Dictionary<int, int>();
            map.Add(0, -1);

            for (int i = 0; i <= nums.Length - 1; i++)
            {
                sum += nums[i];
                if (!map.ContainsKey(sum))
                    map.Add(sum, i);
                if (map.ContainsKey(sum - k))
                    max = (int)Math.Max(max, i - map[sum - k]);
            }

            return max;
        }

        private int _325MyMaximumSizeSubarraySumEqualsK(int[] nums, int k)
        {
            if (nums.Length == 0) return 0;
            Dictionary<int, int> map = new Dictionary<int, int>();
            int[] sums = new int[nums.Length];
            int sum = 0;
            for (int i = 0; i <= nums.Length - 1; i++)
            {
                sum += nums[i];
                sums[i] = sum;
                if (sum == 0) continue;
                if (!map.ContainsKey(sum))
                    map.Add(sum, i);
                else
                    map[sum] = (int)Math.Min(map[sum], i);
            }
            int max = 0;
            for (int i = 0; i <= sums.Length - 1; i++)
            {
                if (sums[i] == k)
                {
                    max = (int)Math.Max(max, i + 1);
                    continue;
                }
                int extra = sums[i] - k;
                if (!map.ContainsKey(extra))
                    continue;
                if (map[extra] > i)
                    continue;
                max = (int)Math.Max(max, i - map[extra]);
            }
            return max;
        }
        #endregion

        #region "785 Is Graph Bipartite"
        private bool _785IsGraphBipartite(int[][] graph)
        {
            int[] colors = new int[graph.Length];

            for (int i = 0; i <= graph.Length - 1; i++)
            {
                if (colors[i] != 0)
                    continue;
                if (!CanColor(graph, colors, i, 1))
                    return false;
            }

            return true;
        }

        public bool CanColor(int[][] graph, int[] colors, int current, int color)
        {
            if (colors[current] != 0)
                return colors[current] == -color;
            colors[current] = -color;

            foreach (int i in graph[current])
                if (!CanColor(graph, colors, i, -color))
                    return false;

            return true;
        }
        #endregion

        #region "210 Course Schedule 2"
        private int[] _210CourseSchedule2(int numCourses, int[,] prerequisites)
        {
            Dictionary<int, HashSet<int>> pres = new Dictionary<int, HashSet<int>>();

            for (int i = 0; i <= prerequisites.GetUpperBound(0); i++)
            {
                if (!pres.ContainsKey(prerequisites[i, 1]))
                    pres.Add(prerequisites[i, 1], new HashSet<int>());
                pres[prerequisites[i, 1]].Add(prerequisites[i, 0]);
            }

            int[] visits = new int[numCourses];
            Stack<int> schedule = new Stack<int>();
            for (int i = 0; i < numCourses; i++)
                if (!CanAddSchedules(pres, visits, schedule, i))
                    return new int[0];

            int[] result = new int[numCourses];
            for (int i = 0; i < numCourses; i++)
                result[i] = schedule.Pop();

            return result;
        }

        private bool CanAddSchedules(Dictionary<int, HashSet<int>> pres, int[] visits, Stack<int> schedule, int current)
        {
            if (visits[current] == 1) return false;
            if (visits[current] == 2) return true;

            visits[current] = 1;

            if (pres.ContainsKey(current))
                foreach (int i in pres[current])
                    if (!CanAddSchedules(pres, visits, schedule, i))
                        return false;

            visits[current] = 2;
            schedule.Push(current);

            return true;
        }
        #endregion

        #region "209 Minimum Size Subarray Sum"
        private int _209MinimumSizeSubarraySum(int s, int[] nums)
        {
            int min = nums.Length + 1, sum = 0, left = 0;
            for (int i = 0; i <= nums.Length - 1; i++)
            {
                sum += nums[i];
                while ((left <= i) && (sum >= s))
                {
                    min = (int)Math.Min(min, i - left + 1);
                    sum -= nums[left++];
                }
            }

            return min == nums.Length + 1 ? 0 : min;
        }
        #endregion

        #region "314 Binary Tree Vertical Order Traversal"
        private IList<IList<int>> _314BinaryTreeVerticalOrderTraversal(TreeNode root)
        {
            IList<IList<int>> result = new List<IList<int>>();
            if (root == null) return result;

            int col = 0;
            Dictionary<int, List<int>> map = new Dictionary<int, List<int>>();
            Queue<Tuple<int, TreeNode>> q = new Queue<Tuple<int, TreeNode>>();
            q.Enqueue(new Tuple<int, TreeNode>(col, root));

            while (q.Count != 0)
            {
                Queue<Tuple<int, TreeNode>> row = new Queue<Tuple<int, TreeNode>>();
                while (q.Count != 0)
                {
                    Tuple<int, TreeNode> node = q.Dequeue();
                    col = node.Item1;
                    if (node.Item2.left != null) row.Enqueue(new Tuple<int, TreeNode>(col - 1, node.Item2.left));
                    if (node.Item2.right != null) row.Enqueue(new Tuple<int, TreeNode>(col + 1, node.Item2.right));

                    if (!map.ContainsKey(node.Item1))
                        map.Add(node.Item1, new List<int>());
                    map[node.Item1].Add(node.Item2.val);
                }
                q = row;
            }

            List<int> cols = map.Keys.ToList();
            cols.Sort();
            for (int i = 0; i <= cols.Count - 1; i++)
                result.Add(new List<int>(map[cols[i]]));

            return result;
        }

        private IList<IList<int>> _314MyBinaryTreeVerticalOrderTraversal(TreeNode root)
        {
            IList<IList<int>> result = new List<IList<int>>();
            if (root == null) return result;

            Dictionary<int, Dictionary<int, List<int>>> map = new Dictionary<int, Dictionary<int, List<int>>>();
            TreeToDictByCol(root, map);
            List<int> cols = map.Keys.ToList();
            cols.Sort();
            for (int i = 0; i <= cols.Count - 1; i++)
            {
                Dictionary<int, List<int>> col = map[cols[i]];
                List<int> depths = col.Keys.ToList();
                depths.Sort();
                List<int> resultCol = new List<int>();
                for (int j = 0; j <= depths.Count - 1; j++)
                    resultCol.AddRange(col[depths[j]]);
                result.Add(resultCol);
            }

            return result;
        }

        private void TreeToDictByCol(TreeNode node, Dictionary<int, Dictionary<int, List<int>>> map, int col = 0, int depth = 0)
        {
            if (node == null)
                return;

            if (!map.ContainsKey(col))
                map.Add(col, new Dictionary<int, List<int>>());
            if (!map[col].ContainsKey(depth))
                map[col].Add(depth, new List<int>());
            map[col][depth].Add(node.val);

            TreeToDictByCol(node.left, map, col - 1, depth + 1);
            TreeToDictByCol(node.right, map, col + 1, depth + 1);
        }
        #endregion

        #region ""

        #endregion

        [Test]
        public void TestMedium()
        {
            #region "314 Binary Tree Vertical Order Traversal"
            //Assert.That(this._314BinaryTreeVerticalOrderTraversal(new TreeNode(new List<int>() { 3, 9, 8, 4, 0, 1, 7, -1, -1, -1, 2, 5, -1 })), Is.EqualTo(new List<IList<int>>()
            //{
            //    new List<int>() { 4 },
            //    new List<int>() { 9, 5 },
            //    new List<int>() { 3, 0, 1},
            //    new List<int>() { 8, 2 },
            //    new List<int>() { 7 },
            //}));
            //Assert.That(this._314BinaryTreeVerticalOrderTraversal(new TreeNode(new List<int>() { 3, 9, 8, 4, 0, 1, 7 })), Is.EqualTo(new List<IList<int>>()
            //{
            //    new List<int>() { 4 },
            //    new List<int>() { 9 },
            //    new List<int>() { 3, 0, 1 },
            //    new List<int>() { 8 },
            //    new List<int>() { 7 },
            //}));
            //Assert.That(this._314BinaryTreeVerticalOrderTraversal(new TreeNode(new List<int>() { 3, 9, 20, -1, -1, 15, 7 })), Is.EqualTo(new List<IList<int>>()
            //{
            //    new List<int>() { 9 },
            //    new List<int>() { 3, 15 },
            //    new List<int>() { 20 },
            //    new List<int>() { 7 },
            //}));
            #endregion

            #region "209 Minimum Size Subarray Sum"
            //Assert.That(this._209MinimumSizeSubarraySum(7, new int[] { 2, 3, 1, 2, 4, 3 }), Is.EqualTo(2));
            #endregion

            #region "210 Course Schedule 2"
            //Assert.That(this._210CourseSchedule2(4, new int[,]
            //{
            //    { 1, 0 }, { 2, 0 }, { 2, 1 }, { 3, 1 }, { 3, 2 },
            //}), Is.EqualTo(new int[] { 0, 1, 2, 3 }));
            #endregion

            #region "785 Is Graph Bipartite"
            //Assert.False(this._785IsGraphBipartite(new int[][]
            //{
            //    new int[]{ 4, 1 },
            //    new int[]{ 0, 2 },
            //    new int[]{ 1, 3 },
            //    new int[]{ 2, 4 },
            //    new int[]{ 3, 0 },
            //}));
            //Assert.False(this._785IsGraphBipartite(new int[][]
            //{
            //    new int[]{ 1, 2, 3 },
            //    new int[]{ 0, 2 },
            //    new int[]{ 0, 1, 3 },
            //    new int[]{ 0, 2 },
            //}));
            //Assert.True(this._785IsGraphBipartite(new int[][]
            //{
            //    new int[]{ 1, 3 },
            //    new int[]{ 0, 2 },
            //    new int[]{ 1, 3 },
            //    new int[]{ 0, 2 },
            //}));
            #endregion

            #region "325 Maximum Size Subarray Sum Equals k"
            //Assert.That(this._325MaximumSizeSubarraySumEqualsK(new int[] { 1, -1, 5, -2, 3 }, 3), Is.EqualTo(4));
            //Assert.That(this._325MaximumSizeSubarraySumEqualsK(new int[] { -2, -1, 2, 1 }, 1), Is.EqualTo(2));
            #endregion

            #region "71 Simplify Path"
            //Assert.That(this._71SimplifyPath("/..."), Is.EqualTo("/..."));
            //Assert.That(this._71SimplifyPath("/home//foo/"), Is.EqualTo("/home/foo"));
            //Assert.That(this._71SimplifyPath("/../"), Is.EqualTo("/"));
            //Assert.That(this._71SimplifyPath("/home/"), Is.EqualTo("/home"));
            //Assert.That(this._71SimplifyPath("/a/./b/../../c/"), Is.EqualTo("/c"));
            #endregion

            #region "75 Sort Colors"
            //int[] nums;

            //nums = new int[] { 1, 2, 0 };
            //this._75SortColors(nums);
            //Assert.That(nums, Is.EqualTo(new int[] { 0, 1, 2 }));

            //nums = new int[] { 1, 1 };
            //this._75SortColors(nums);
            //Assert.That(nums, Is.EqualTo(new int[] { 1, 1 }));

            //nums = new int[] { 2, 0, 1 };
            //this._75SortColors(nums);
            //Assert.That(nums, Is.EqualTo(new int[] { 0, 1, 2 }));

            //nums = new int[] { 2, 0, 2, 1, 1, 0 };
            //this._75SortColors(nums);
            //Assert.That(nums, Is.EqualTo(new int[] { 0, 0, 1, 1, 2, 2 }));
            #endregion

            #region "43 Multiply Strings"
            //Assert.That(this._43MultiplyStrings("6", "501"), Is.EqualTo("3006"));
            //Assert.That(this._43MultiplyStrings("99", "999"), Is.EqualTo("98901"));
            //Assert.That(this._43MultiplyStrings("1234", "567"), Is.EqualTo("699678"));
            //Assert.That(this._43MultiplyStrings("1234", "5678"), Is.EqualTo("7006652"));
            //Assert.That(this._43MultiplyStrings("123", "456"), Is.EqualTo("56088"));
            //Assert.That(this._43MultiplyStrings("2", "3"), Is.EqualTo("6"));
            #endregion

            #region "277 Find the Celebrity"
            //Assert.That(this._277FindTheCelebrity(2), Is.EqualTo(-1));
            //Assert.That(this._277FindTheCelebrity(6), Is.EqualTo(5));
            //Console.WriteLine(asked);
            //asked = 0;
            //Assert.That(this._277FindTheCelebrity(4), Is.EqualTo(-1));
            //Console.WriteLine(asked);
            //asked = 0;
            #endregion

            #region "394 Decode String"
            //Assert.That(this._394DecodeString("10[leet]"), Is.EqualTo("leetleetleetleetleetleetleetleetleetleet"));
            //Assert.That(this._394DecodeString("3[a]2[bc]"), Is.EqualTo("aaabcbc"));
            //Assert.That(this._394DecodeString("3[a2[c]]"), Is.EqualTo("accaccacc"));
            //Assert.That(this._394DecodeString("2[abc]3[cd]ef"), Is.EqualTo("abcabccdcdcdef"));
            #endregion

            #region "621 Task Scheduler"
            //Assert.That(this._621TaskScheduler(new char[] { 'A', 'A', 'A', 'A', 'B', 'B', 'B', 'C', 'C', 'D', 'D', 'F', 'F', 'G' }, 2), Is.EqualTo(14));
            //Assert.That(this._621TaskScheduler(new char[] { 'A', 'A', 'A', 'B', 'B', 'B' }, 2), Is.EqualTo(8));
            #endregion

            #region "78 Subsets"
            //Assert.That(this._78Subsets(new int[] { 1, 2 }), Is.EquivalentTo(new List<List<int>>()
            //{
            //    new List<int>() { },
            //    new List<int>() { 1 },
            //    new List<int>() { 2 },
            //    new List<int>() { 1, 2 },
            //}));
            //Assert.That(this._78Subsets(new int[] { 1, 2, 3 }), Is.EquivalentTo(new List<List<int>>()
            //{
            //    new List<int>() { },
            //    new List<int>() { 1 },
            //    new List<int>() { 2 },
            //    new List<int>() { 3 },
            //    new List<int>() { 1, 2 },
            //    new List<int>() { 1, 3 },
            //    new List<int>() { 2, 3 },
            //    new List<int>() { 1, 2, 3 },
            //}));
            #endregion

            #region "341 Flatten Nested List Iterator"
            //StringBuilder sb;
            //IList<NestedInteger> nested;
            //NestedIterator i;

            //sb = new StringBuilder();
            //nested = new List<NestedInteger>()
            //{
            //    new NestedInteger(new List<NestedInteger>())
            //};
            //i = new NestedIterator(nested);
            //while (i.HasNext()) sb.Append(i.Next());
            //Assert.That(sb.ToString(), Is.EqualTo(""));

            //sb = new StringBuilder();
            //nested = new List<NestedInteger>()
            //{
            //    new NestedInteger(1),
            //    new NestedInteger(new List<NestedInteger>()
            //    {
            //        new NestedInteger(4),
            //        new NestedInteger(new List<NestedInteger>()
            //        {
            //            new NestedInteger(6)
            //        })
            //    })
            //};
            //i = new NestedIterator(nested);
            //while (i.HasNext()) sb.Append(i.Next());
            //Assert.That(sb.ToString(), Is.EqualTo("146"));

            //sb = new StringBuilder();
            //nested = new List<NestedInteger>()
            //{
            //    new NestedInteger(new List<NestedInteger>(){ new NestedInteger(1), new NestedInteger(1) }),
            //    new NestedInteger(2),
            //    new NestedInteger(new List<NestedInteger>(){ new NestedInteger(1), new NestedInteger(1) })
            //};
            //i = new NestedIterator(nested);
            //while (i.HasNext()) sb.Append(i.Next());
            //Assert.That(sb.ToString(), Is.EqualTo("11211"));

            //sb = new StringBuilder();
            //nested = new List<NestedInteger>()
            //{
            //    new NestedInteger(new List<NestedInteger>() { new NestedInteger(1), new NestedInteger(2), new NestedInteger(3) }),
            //    new NestedInteger(4),
            //    new NestedInteger(new List<NestedInteger>() { new NestedInteger(5), new NestedInteger(6) }),
            //    new NestedInteger(7)
            //};
            //i = new NestedIterator(nested);
            //while (i.HasNext()) sb.Append(i.Next());
            //Assert.That(sb.ToString(), Is.EqualTo("1234567"));
            #endregion

            #region "636 Exclusive Time of Functions"
            //Assert.That(this._636ExclusiveTimeOfFunctions(2, new List<string>() { "0:start:0", "0:start:2", "0:end:5", "1:start:7", "1:end:7", "0:end:8" }), Is.EqualTo(new List<int>() { 8, 1 }));
            //Assert.That(this._636ExclusiveTimeOfFunctions(2, new List<string>() { "0:start:0", "1:start:2", "1:end:5", "0:start:6", "0:end:8", "0:end:10" }), Is.EqualTo(new List<int>() { 7, 4 }));
            //Assert.That(this._636ExclusiveTimeOfFunctions(2, new List<string>() { "0:start:0", "1:start:2", "1:end:5", "0:end:6" }), Is.EqualTo(new List<int>() { 3, 4 }));
            #endregion

            #region "208 Implement Trie - Prefix Tree"
            //Trie trie = new Trie();
            //trie.Insert("app");
            //trie.Insert("ape");
            //trie.Insert("at");
            //trie.Insert("atp");
            //trie.Insert("to");
            //Assert.True(trie.StartsWith("t"));
            //Assert.False(trie.Search("t"));
            //Assert.False(trie.StartsWith("b"));
            //Assert.True(trie.Search("app"));
            //Assert.True(trie.StartsWith("app"));
            //Assert.False(trie.Search("apt"));

            //trie = new Trie();
            //trie.Insert("apple");
            //Assert.True(trie.Search("apple"));   // returns true
            //Assert.False(trie.Search("app"));     // returns false
            //Assert.True(trie.StartsWith("app")); // returns true
            //trie.Insert("app");
            //Assert.True(trie.Search("app"));     // returns true
            #endregion

            #region "173 Binary Search Tree Iterator"
            //TreeNode root = new TreeNode(new List<int>() { 6, 3, 9, 2, 4, 7, 11, 1, -1, -1, 5, -1, 8, 10, -1 });
            //StringBuilder sb = new StringBuilder();
            //BSTIterator i = new BSTIterator(root);
            //while (i.HasNext()) sb.Append(i.Next());
            //Assert.That(sb.ToString(), Is.EqualTo("1234567891011"));
            #endregion

            #region "79 Word Search"
            //char[,] input;

            //input = new char[,]
            //{
            //    { 'F','Y','C','E','N','R','D' },
            //    { 'K','L','N','F','I','N','U' },
            //    { 'A','A','A','R','A','H','R' },
            //    { 'N','D','K','L','P','N','E' },
            //    { 'A','L','A','N','S','A','P' },
            //    { 'O','O','G','O','T','P','N' },
            //    { 'H','P','O','L','A','N','O' }
            //};
            //Assert.False(this._79WordSearch(input, "poland"));

            //input = new char[,]
            //{
            //    { 'A','B','C','E' },
            //    { 'S','F','E','S' },
            //    { 'A','D','E','E' }
            //};
            //Assert.True(this._79WordSearch(input, "ABCESEEEFS"));

            //input = new char[,]
            //{
            //    { 'a', 'b' },
            //    { 'c', 'd' }
            //};
            //Assert.False(this._79WordSearch(input, "abcd"));

            //input = new char[,]
            //{
            //    { 'a' }
            //};
            //Assert.True(this._79WordSearch(input, "a"));

            //input = new char[,]
            //{
            //    { 'A', 'B', 'C', 'E'},
            //    { 'S', 'F', 'C', 'S'},
            //    { 'A', 'D', 'E', 'E'}
            //};
            //Assert.True(this._79WordSearch(input, "ABCCED"));
            //Assert.True(this._79WordSearch(input, "SEE"));
            //Assert.False(this._79WordSearch(input, "ABCB"));
            #endregion

            #region "102 Binary Tree Level Order Traversal"
            //Assert.That(this._102BinaryTreeLevelOrderTraversal(new TreeNode(new List<int>() { 3, 9, 20, -1, -1, 15, 7 })), Is.EqualTo(new List<IList<int>>() {
            //    new List<int>(){3},
            //    new List<int>(){9,20 },
            //    new List<int>(){ 15, 7 }
            //}));
            #endregion

            #region "133 Clone Graph"
            //UndirectedGraphNode root = new UndirectedGraphNode();
            //root.ConstructGraphByLeetCodeTemplate("0,1,2#1,2#2,2");
            //UndirectedGraphNode newRootDfs = UndirectedGraphNode.CloneNodeDfs(root);
            //UndirectedGraphNode newRootBfs = UndirectedGraphNode.CloneNodeBfs(root);
            //Console.WriteLine();
            #endregion

            #region "127 Word Ladder"
            //Assert.That(this._127WordLadder("leet", "code", new List<string>() { "lest", "leet", "lose", "code", "lode", "robe", "lost" }), Is.EqualTo(6));
            //Assert.That(this._127WordLadder("hot", "dog", new List<string>() { "hot", "cog", "dog", "tot", "hog", "hop", "pot", "dot" }), Is.EqualTo(3));
            //Assert.That(this._127WordLadder("hot", "dog", new List<string>() { "hot", "dog" }), Is.EqualTo(0));
            //Assert.That(this._127WordLadder("hot", "dog", new List<string>() { "hot", "dog", "dot" }), Is.EqualTo(3));
            //Assert.That(this._127WordLadder("hit", "cog", new List<string>() { "hot", "dot", "dog", "lot", "log" }), Is.EqualTo(0));
            //Assert.That(this._127WordLadder("hit", "cog", new List<string>() { "hot", "dot", "dog", "lot", "log", "cog" }), Is.EqualTo(5));
            #endregion

            #region "236 Lowest Common Ancestor of a Binary Tree"
            //TreeNode root = new TreeNode(new List<int>() { 3, 5, 1, 6, 2, 0, 8, -1, -1, 7, 4 });
            //Assert.That(this._236LowestCommonAncestorOfABinaryTree(root, new TreeNode(5), new TreeNode(1)).val, Is.EqualTo(3));
            //Assert.That(this._236LowestCommonAncestorOfABinaryTree(root, new TreeNode(5), new TreeNode(4)).val, Is.EqualTo(5));
            #endregion

            #region "380 Insert Delete GetRandom O(1)"
            //RandomizedSet randomSet = new RandomizedSet();

            //randomSet = new RandomizedSet();
            //Assert.True(randomSet.Insert(3));
            //Assert.True(randomSet.Insert(-2));
            //Assert.False(randomSet.Remove(2));
            //Assert.True(randomSet.Insert(1));
            //Assert.True(randomSet.Insert(-3));
            //Assert.False(randomSet.Insert(-2));
            //Assert.True(randomSet.Remove(-2));
            //Assert.True(randomSet.Remove(3));
            //Assert.True(randomSet.Insert(-1));
            //Assert.True(randomSet.Remove(-3));
            //Assert.False(randomSet.Insert(1));
            //Assert.True(randomSet.Insert(-2));
            //Assert.False(randomSet.Insert(-2));
            //Assert.False(randomSet.Insert(-2));
            //Assert.False(randomSet.Insert(1));
            //Assert.That(randomSet.GetRandom(), Is.EqualTo(1).Or.EqualTo(-2));
            //Assert.False(randomSet.Insert(-2));
            //Assert.False(randomSet.Remove(0));
            //Assert.True(randomSet.Insert(-3));
            //Assert.False(randomSet.Insert(1));

            //randomSet = new RandomizedSet();
            //Assert.True(randomSet.Insert(1));
            //Assert.False(randomSet.Remove(2));
            //Assert.True(randomSet.Insert(2));
            //Assert.That(randomSet.GetRandom(), Is.EqualTo(1).Or.EqualTo(2));
            //Assert.True(randomSet.Remove(1));
            //Assert.False(randomSet.Insert(2));
            //Assert.That(randomSet.GetRandom(), Is.EqualTo(2));
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
