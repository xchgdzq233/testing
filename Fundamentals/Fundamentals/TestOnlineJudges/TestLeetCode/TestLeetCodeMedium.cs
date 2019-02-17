using Fundamentals.TestDataStructures;
using log4net;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fundamentals.TestOnlineJudges.TestLeetCode
{
    [TestFixture]
    public class TestLeetCodeMedium
    {
        private static ILog logger = LogManager.GetLogger(typeof(TestLeetCodeMedium));

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

            public override int GetHashCode()
            {
                return (start + end).GetHashCode();
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
        private int _253MeetingRooms2Old(Interval[] intervals)
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

        private int _253MeetingRooms2(Interval[] intervals)
        {
            intervals = intervals.OrderBy(i => i.start).ToArray();
            List<int> ends = new List<int>();
            foreach (Interval cur in intervals)
            {
                bool added = false;
                for (int i = 0; i < ends.Count; i++)
                {
                    if (cur.start > ends[i])
                    {
                        ends[i] = cur.end;
                        added = true;
                        break;
                    }
                }
                if (!added)
                {
                    ends.Add(cur.end);
                }
            }
            return ends.Count;
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

            public string Encode(string longUrl)
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

            public string Decode(string tinyUrl)
            {
                List<string> tinys = tinyUrl.Replace(domain, "").Split(new char[] { 'I' }).ToList();

                if (tinys.Count != 2)
                    return String.Empty;
                if (!Int32.TryParse(tinys[0], out int hash) || !Int32.TryParse(tinys[1], out int index))
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

            public string Encode(string longUrl)
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

            public string Decode(string shortUrl)
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

            public string Encode(string longUrl)
            {
                string shortUrl = Convert.ToInt32(longUrl.GetHashCode().ToString("X"), 16).ToString();
                map.Add(shortUrl, longUrl);
                return shortUrl;
            }

            public string Decode(string shortUrl)
            {
                if (map.ContainsKey(shortUrl))
                    return map[shortUrl];
                return String.Empty;
            }
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

            public bool Insert(int val)
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

            public bool Remove(int val)
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
            private readonly int val;
            private readonly List<NestedInteger> vals;

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
            IList<IList<int>> result = new List<IList<int>>
            {
                new List<int>()
            };
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
            Dictionary<int, int> map = new Dictionary<int, int>
            {
                { 0, -1 }
            };

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

        #region "285 Inorder Successor in BST"
        private TreeNode _285InorderSuccessorInBST(TreeNode root, TreeNode p)
        {
            if (root == null) return null;

            TreeNode[] result = new TreeNode[2];
            InorderSuccessorSub(root, p, result);
            return result[1] ?? null;
        }

        private void InorderSuccessorSub(TreeNode node, TreeNode p, TreeNode[] result)
        {
            if (node == null || result[1] != null) return;

            if (p.val == node.val)
            {
                result[0] = node;
                InorderSuccessorSub(node.right, p, result);
            }
            else
            {
                if (p.val < node.val)
                    InorderSuccessorSub(node.left, p, result);
                else
                    InorderSuccessorSub(node.right, p, result);

                if (result[0] != null && result[1] == null && node.val > p.val)
                {
                    result[1] = node;
                    return;
                }
            }
        }
        #endregion

        #region "426 Convert Binary Search Tree to Sorted Doubly Linked List"

        private TreeNode _426ConvertBinarySearchTreeToSortedDoublyLinkedList(TreeNode root)
        {
            if (root == null) return null;

            TreeNode dummy = new TreeNode(0);
            prev = dummy;
            ResetTree(root);

            prev.right = dummy.right;
            dummy.right.left = prev;
            return dummy.right;
        }

        TreeNode prev = null;

        private void ResetTree(TreeNode node)
        {
            if (node == null) return;

            ResetTree(node.left);

            prev.right = node;
            node.left = prev;
            prev = node;

            ResetTree(node.right);
        }
        #endregion

        #region "261 Graph Valid Tree"
        private bool _261GraphValidTree(int n, int[,] edges)
        {
            List<List<int>> map = new List<List<int>>();
            for (int i = 0; i < n; i++)
                map.Add(new List<int>());

            for (int i = 0; i <= edges.GetUpperBound(0); i++)
            {
                map[edges[i, 0]].Add(edges[i, 1]);
                map[edges[i, 1]].Add(edges[i, 0]);
            }

            HashSet<int> visited = new HashSet<int>();
            if (!Helper(map, visited, 0))
                return false;
            return visited.Count == n;
        }

        private bool Helper(List<List<int>> map, HashSet<int> visited, int current, int parent = -1)
        {
            if (visited.Contains(current)) return false;
            visited.Add(current);
            foreach (int i in map[current])
            {
                if (i == parent) continue;
                if (!Helper(map, visited, i, current))
                    return false;
            }
            return true;
        }

        private bool _261MyGraphValidTree(int n, int[,] edges)
        {
            if (edges == null) return true;
            if (edges.GetUpperBound(0) < 0)
            {
                if (n == 1) return true;
                return false;
            }

            Dictionary<int, List<int>> map = new Dictionary<int, List<int>>();

            for (int i = 0; i <= edges.GetUpperBound(0); i++)
            {
                if (!map.ContainsKey(edges[i, 0]))
                    map.Add(edges[i, 0], new List<int>());
                map[edges[i, 0]].Add(edges[i, 1]);

                if (!map.ContainsKey(edges[i, 1]))
                    map.Add(edges[i, 1], new List<int>());
                map[edges[i, 1]].Add(edges[i, 0]);
            }

            HashSet<int> visited;
            foreach (int i in map.Keys)
            {
                visited = new HashSet<int>();
                if (!WalkNodes(edges, map, visited, i))
                    return false;
                if (visited.Count == n)
                    return true;
            }

            return false;
        }

        private bool WalkNodes(int[,] edges, Dictionary<int, List<int>> map, HashSet<int> visited, int current, int prev = -1)
        {
            if (visited.Contains(current)) return false;

            visited.Add(current);

            if (map.ContainsKey(current))
                foreach (int i in map[current])
                {
                    if (i == prev) continue;
                    if (!WalkNodes(edges, map, visited, i, current))
                        return false;
                }

            return true;
        }
        #endregion

        #region "80 Remove Duplicates from Sorted Array 2"
        private int _80RemoveDuplicatesFromSortedArray2(int[] nums)
        {
            if (nums == null) return 0;
            if (nums.Length <= 2) return nums.Length;
            int count = 2;
            for (int i = 2; i <= nums.Length - 1; i++)
                if (nums[i] != nums[count - 2])
                    nums[count++] = nums[i];
            return count;
        }

        private int _80MyRemoveDuplicatesFromSortedArray2(int[] nums)
        {
            if (nums == null) return 0;
            if (nums.Length <= 2) return nums.Length;

            int count = 1, lastValid = 0;
            for (int i = 1; i <= nums.Length - 1; i++)
                if (nums[i] != nums[lastValid])
                {
                    count = 1;
                    if (lastValid != i - 1)
                        nums[lastValid + 1] = nums[i];
                    lastValid++;
                }
                else
                {
                    count++;
                    if (count <= 2)
                    {
                        if (lastValid != i - 1)
                            nums[lastValid + 1] = nums[i];
                        lastValid++;
                    }
                }
            return lastValid + 1;
        }
        #endregion

        #region "721 Accounts Merge"
        private IList<IList<string>> _721AccountsMerge(List<List<string>> accounts)
        {
            DSU dsu = new DSU();
            Dictionary<string, String> emailToName = new Dictionary<string, string>();
            Dictionary<string, int> emailToId = new Dictionary<string, int>();

            int id = 0;
            foreach (List<string> account in accounts)
            {
                string name = "";
                foreach (string email in account)
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        name = email;
                        continue;
                    }
                    if (!emailToName.ContainsKey(email))
                        emailToName.Add(email, name);
                    if (!emailToId.ContainsKey(email))
                        emailToId.Add(email, id++);
                    dsu.Union(emailToId[account[1]], emailToId[email]);
                }
            }

            Dictionary<int, List<string>> ans = new Dictionary<int, List<string>>();
            foreach (string email in emailToName.Keys)
            {
                int index = dsu.Find(emailToId[email]);
                if (!ans.ContainsKey(index))
                    ans.Add(index, new List<string>());
                ans[index].Add(email);
            }
            IList<IList<string>> result = new List<IList<string>>();
            foreach (List<string> component in ans.Values)
            {
                List<string> account = component.OrderBy(s => s, StringComparer.Ordinal).ToList();
                account.Insert(0, emailToName[account[0]]);
                result.Add(account);
            }

            return result;
        }

        public class DSU
        {
            readonly int[] parent;

            public DSU()
            {
                parent = new int[10001];
                for (int i = 0; i <= 10000; ++i)
                    parent[i] = i;
            }

            public int Find(int x)
            {
                if (parent[x] != x)
                    parent[x] = Find(parent[x]);
                return parent[x];
            }

            public void Union(int x, int y)
            {
                parent[Find(x)] = Find(y);
            }
        }

        // time limitation exceeded
        private IList<IList<string>> _721MyAccountsMerge(List<List<string>> accounts)
        {
            Dictionary<string, HashSet<int>> emails = new Dictionary<string, HashSet<int>>();
            for (int i = 0; i <= accounts.Count - 1; i++)
                for (int j = 1; j <= accounts[i].Count - 1; j++)
                {
                    if (!emails.ContainsKey(accounts[i][j]))
                        emails.Add(accounts[i][j], new HashSet<int>());
                    if (!emails[accounts[i][j]].Contains(i))
                        emails[accounts[i][j]].Add(i);
                }

            IList<IList<string>> result = new List<IList<string>>();
            List<string> keys = emails.Keys.ToList();
            foreach (string key in keys)
            {
                if (emails[key].Count == 0) continue;

                string name = accounts[emails[key].First()][0];
                List<string> account = new List<string>
                {
                    key
                };
                Queue<string> q = new Queue<string>();
                q.Enqueue(key);
                while (q.Count != 0)
                {
                    string e = q.Dequeue();
                    foreach (int index in emails[e])
                        for (int j = 1; j <= accounts[index].Count - 1; j++)
                            if (!account.Contains(accounts[index][j]))
                            {
                                q.Enqueue(accounts[index][j]);
                                account.Add(accounts[index][j]);
                            }
                    emails[e] = new HashSet<int>();
                }

                account = account.OrderBy(s => s, StringComparer.Ordinal).ToList();
                account.Insert(0, name);
                result.Add(account);
            }

            #region
            StringBuilder sb = new StringBuilder();
            foreach (List<string> s in result)
            {
                sb.AppendFormat("\n{0}: ", s[0]);
                for (int i = 1; i <= s.Count - 1; i++)
                    sb.AppendFormat("{0}, ", s[i]);
            }
            logger.InfoFormat("printing...{0}", sb.ToString());
            #endregion

            return result;
        }
        #endregion

        #region "8 String to Integer"
        private int _8StringtoInteger(string str)
        {
            if (str.Length == 0) return 0;

            int pointer = 0;
            while (str[pointer] == ' ')
            {
                pointer++;
                if (pointer >= str.Length)
                    return 0;
            }

            bool negative = false;
            if (str[pointer] == '-')
            {
                negative = true;
                pointer++;
            }
            else if (str[pointer] == '+')
                pointer++;

            if (pointer >= str.Length)
                return 0;

            if (!Int32.TryParse(str[pointer].ToString(), out int current))
                return 0;

            long result = 0;
            while (Int32.TryParse(str[pointer++].ToString(), out current))
            {
                result = result * 10 + current;

                if (result > Int32.MaxValue)
                    return negative ? Int32.MinValue : Int32.MaxValue;

                if (pointer >= str.Length)
                    break;
            }

            if (negative)
            {
                result = 0 - result;
                return result < Int32.MinValue ? Int32.MinValue : (int)result;
            }
            return result > Int32.MaxValue ? Int32.MaxValue : (int)result;
        }
        #endregion

        #region "12 Integer to Roman"
        private string _12IntegerToRoman(int num)
        {
            List<Tuple<int, char>> romans = new List<Tuple<int, char>>()
            {
                new Tuple<int, char>(1, 'I'),
                new Tuple<int, char>(5, 'V'),
                new Tuple<int, char>(10, 'X'),
                new Tuple<int, char>(50, 'L'),
                new Tuple<int, char>(100, 'C'),
                new Tuple<int, char>(500, 'D'),
                new Tuple<int, char>(1000, 'M')
            };
            StringBuilder result = new StringBuilder();

            int digit = num / 1000;
            for (int k = 0; k < digit; k++)
                result.Append('M');
            num %= 1000;

            for (int k = 6; k > 0;)
            {
                digit = num / romans[k - 2].Item1;
                result.Append(SubRoman(digit, romans[k--].Item2, romans[k--].Item2, romans[k].Item2));
                num %= romans[k].Item1;
            }

            return result.ToString();
        }

        private string SubRoman(int digit, char ten, char five, char one)
        {
            if (digit == 0) return "";

            StringBuilder result = new StringBuilder();

            if (digit == 9)
                result.Append(new char[] { one, ten });
            else if (digit == 4)
                result.Append(new char[] { one, five });
            else
            {
                if (digit >= 5)
                {
                    result.Append(five);
                    digit -= 5;
                }

                for (int k = 0; k < digit; k++)
                    result.Append(one);
            }

            return result.ToString();
        }
        #endregion

        #region "31 Next Permutation"
        private void _31NextPermutation(int[] nums)
        {
            if (nums == null) return;
            if (nums.Length == 0) return;

            int i = nums.Length - 1;
            while (i-- > 0)
                if (nums[i] < nums[i + 1])
                    break;

            if (i == -1)
            {
                this._31Reverse(nums, 0, nums.Length - 1);
                return;
            }

            int j = nums.Length;
            while (j-- > i)
                if (nums[j] > nums[i])
                    break;

            this._31Swap(nums, i, j);

            this._31Reverse(nums, i + 1, nums.Length - 1);
        }

        private void _31Reverse(int[] nums, int start, int end)
        {
            while (start < end)
                _31Swap(nums, start++, end--);
        }

        private void _31Swap(int[] nums, int i, int j)
        {
            int temp = nums[j];
            nums[j] = nums[i];
            nums[i] = temp;
        }

        // my anwser
        private void _31MyNextPermutation(int[] nums)
        {
            if (nums == null) return;
            if (nums.Length == 0) return;

            int i = nums.Length - 1;
            while (i-- > 0)
                if (nums[i] < nums[i + 1])
                    break;

            if (i == -1)
            {
                Array.Sort(nums);
                return;
            }

            int j = nums.Length;
            while (j-- > i)
                if (nums[j] > nums[i])
                    break;

            int temp = nums[j];
            nums[j] = nums[i];
            nums[i] = temp;

            Array.Sort(nums, i + 1, nums.Length - i - 1);
        }
        #endregion

        #region "347 Top K Frequent Elements"
        private IList<int> _347TopKFrequentElements(int[] nums, int k)
        {
            IList<int> result = new List<int>();
            if (nums == null) return result;
            if (nums.Length == 1)
            {
                result.Add(nums[0]);
                return result;
            }

            Dictionary<int, int> map = new Dictionary<int, int>();
            List<List<int>> freq = new List<List<int>>
            {
                new List<int>()
            };


            foreach (int i in nums)
            {
                if (!map.ContainsKey(i))
                {
                    map.Add(i, 1);
                    freq[0].Add(i);
                }
                else
                {
                    freq[map[i] - 1].Remove(i);
                    map[i]++;
                    if (freq.Count() < map[i])
                        freq.Add(new List<int>());
                    freq[map[i] - 1].Add(i);
                }
            }

            for (int i = freq.Count - 1; i >= 0; i--)
            {
                foreach (int j in freq[i])
                {
                    result.Add(j);
                    if (--k == 0)
                        return result;
                }
            }

            return result;
        }

        private IList<int> _347MyTopKFrequentElements(int[] nums, int k)
        {
            IList<int> result = new List<int>();
            if (nums == null) return result;
            if (nums.Length == 1)
            {
                result.Add(nums[0]);
                return result;
            }

            Dictionary<int, HeapNode> map = new Dictionary<int, HeapNode>();
            HeapNode head = new HeapNode(0, Int32.MaxValue);
            HeapNode tail = head;

            foreach (int i in nums)
            {
                if (!map.ContainsKey(i))
                {
                    HeapNode newNode = new HeapNode(i, 1);
                    map.Add(i, newNode);
                    tail.next = newNode;
                    newNode.pre = tail;
                    tail = tail.next;
                }
                else
                    map[i].IncreaseFreq(ref tail);
            }

            HeapNode node = head.next;
            for (int i = 0; i < k; i++)
            {
                result.Add(node.val);
                node = node.next;
            }

            return result;
        }

        public class HeapNode
        {
            public int val;
            public int freq;
            public HeapNode pre;
            public HeapNode next;
            public HeapNode(int val, int freq)
            {
                this.val = val;
                this.freq = freq;
                this.pre = null;
                this.next = null;
            }
            public void IncreaseFreq(ref HeapNode tail, int increase = 1)
            {
                this.freq++;
                if (this.freq > this.pre.freq)
                {
                    HeapNode moveAfterThis = this.pre;
                    while (this.freq > moveAfterThis.freq)
                        moveAfterThis = moveAfterThis.pre;

                    this.pre.next = this.next;
                    if (this.next != null)
                        this.next.pre = this.pre;
                    else
                        tail = this.pre;

                    moveAfterThis.next.pre = this;
                    this.next = moveAfterThis.next;
                    moveAfterThis.next = this;
                    this.pre = moveAfterThis;
                }
            }
        }
        #endregion

        #region "498 Diagonal Traverse"
        private int[] _498DiagonalTraverse(int[,] matrix)
        {
            if (matrix == null) return new int[] { };

            int row = matrix.GetUpperBound(0);
            int col = matrix.GetUpperBound(1);
            if (row == -1 || col == -1) return new int[] { };

            int[] result = new int[(row + 1) * (col + 1)];

            int count = 0;
            for (int sum = 0; sum <= row + col; sum++)
            {
                int low = Math.Max(0, sum - col);
                int high = Math.Min(sum, row);

                // going up
                if (sum % 2 == 0)
                {
                    for (int i = high, j = sum - i; i >= low; i--, j++)
                        result[count++] = matrix[i, j];
                }
                // going down
                else
                {
                    for (int i = low, j = sum - i; i <= high; i++, j--)
                        result[count++] = matrix[i, j];
                }
            }

            return result;
        }

        private int[] _498DiagonalTraverse_1st(int[,] matrix)
        {
            if (matrix == null) return new int[] { };

            int row = matrix.GetUpperBound(0);
            int col = matrix.GetUpperBound(1);
            if (row == -1 || col == -1) return new int[] { };

            List<int>[] map = new List<int>[row + col + 1];

            for (int i = 0; i <= row; i++)
                for (int j = 0; j <= col; j++)
                {
                    if (map[i + j] == null)
                        map[i + j] = new List<int>();
                    map[i + j].Add(matrix[i, j]);
                }

            int[] result = new int[(row + 1) * (col + 1)];
            int count = 0;
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = map[i].Count - 1; j >= 0; j--)
                    result[count++] = map[i][j];
                if (++i < map.Length)
                    for (int j = 0; j < map[i].Count; j++)
                        result[count++] = map[i][j];
            }
            return result;
        }
        #endregion

        #region "107 Binary Tree Level Order Traversal 2"
        private IList<IList<int>> _107BinaryTreeLevelOrderTraversal2(TreeNode root)
        {
            IList<IList<int>> result = new List<IList<int>>();
            if (root == null) return result;

            Queue<TreeNode> q = new Queue<TreeNode>();
            q.Enqueue(root);

            while (q.Count != 0)
            {
                IList<int> row = new List<int>();
                int size = q.Count;

                for (int i = 0; i < size; i++)
                {
                    TreeNode node = q.Dequeue();
                    if (node.left != null) q.Enqueue(node.left);
                    if (node.right != null) q.Enqueue(node.right);
                    row.Add(node.val);
                }

                result.Insert(0, row);
            }

            return result;
        }
        #endregion

        #region "105 Construct Binary Tree from Preorder and Inorder Traversal"
        public TreeNode _105ConstructBinaryTreeFromPreorderAndInorderTraversal(int[] preorder, int[] inorder)
        {
            if (preorder == null || inorder == null) return null;

            Dictionary<int, int> map = new Dictionary<int, int>();
            for (int i = 0; i < inorder.Length; i++)
                map.Add(inorder[i], i);

            return ConstructTree_PreIn(preorder, 0, preorder.Length - 1, map, 0, inorder.Length - 1);
        }

        public TreeNode ConstructTree_PreIn(int[] preorder, int pl, int pr, Dictionary<int, int> inorder, int il, int ir)
        {
            if (pl > pr) return null;

            TreeNode node = new TreeNode(preorder[pl]);
            node.left = ConstructTree_PreIn(preorder, pl + 1, pl + inorder[node.val] - il, inorder, il, inorder[node.val] - 1);
            node.right = ConstructTree_PreIn(preorder, pl + inorder[node.val] - il + 1, pr, inorder, inorder[node.val] + 1, ir);

            return node;
        }
        #endregion

        #region "106 Construct Binary Tree from Inorder and Postorder Traversal"
        private TreeNode _106ConstructBinaryTreeFromInorderAndPostorderTraversal(int[] inorder, int[] postorder)
        {
            return null;
        }
        #endregion

        #region "322 Coin Change"
        public int _322CoinChange(int[] coins, int amount)
        {
            if (amount < 1) return 0;
            int[] dp = new int[amount + 1];
            int sum = 0;

            while (++sum <= amount)
            {
                int min = -1;
                foreach (int coin in coins)
                {
                    if (sum >= coin && dp[sum - coin] != -1)
                    {
                        int temp = dp[sum - coin] + 1;
                        min = min < 0 ? temp : (temp < min ? temp : min);
                    }
                }
                dp[sum] = min;
            }
            return dp[amount];
        }

        public int _322CoinChangeOptimizedDP(int[] coins, int amount)
        {
            if (amount == 0)
            {
                return 0;
            }
            if (amount < 0)
            {
                return -1;
            }

            int min = Int32.MaxValue;
            foreach (int coin in coins)
            {
                int target = amount - coin;
                int cur;
                if (!calculated.ContainsKey(target))
                {
                    cur = _322CoinChange(coins, amount - coin);
                    calculated.Add(target, cur);
                }
                else
                {
                    cur = calculated[target];
                }

                if (cur != -1)
                {
                    min = Math.Min(min, cur);
                }
            }
            return min == Int32.MaxValue ? -1 : min + 1;
        }

        private Dictionary<int, int> calculated = new Dictionary<int, int>();

        public int _322CoinChangeDP(int[] coins, int amount)
        {
            return this._322DP(coins, amount, 0);
        }

        private int _322DP(int[] coins, int amount, int index)
        {
            if (index >= coins.Length)
            {
                return -1;
            }

            int count = 0, sum = 0, min = Int32.MaxValue;
            while (sum <= amount)
            {
                if (amount > sum)
                {
                    int res = _322DP(coins, amount - sum, index + 1);
                    if (res != -1)
                    {
                        min = Math.Min(min, count + res);
                    }
                }
                else if (amount == sum)
                {
                    min = Math.Min(min, count);
                }
                ++count;
                sum += coins[index];
            }

            return min == Int32.MaxValue ? -1 : min;
        }

        public int _322MyCoinChange(int[] coins, int amount)
        {
            if (amount == 0)
            {
                return 0;
            }

            Array.Sort(coins);
            this.coins = coins;
            this.amount = amount;
            this.res = Int32.MaxValue;

            for (int i = coins.Length - 1; i >= 0; --i)
            {
                this.cur = 0;
                int coin = coins[i];
                if (coin == amount)
                {
                    return 1;
                }
                else if (coin < amount)
                {
                    ++cur;
                    GetCoins(i, coin);
                }
            }

            return res == Int32.MaxValue ? -1 : res;
        }

        private int[] coins;
        private int amount;
        private int res;
        private int cur;

        public void GetCoins(int index, int sum)
        {
            if (sum == amount)
            {
                res = Math.Min(res, cur);
            }
            if (sum > amount)
            {
                return;
            }

            int coin = coins[index];
            if (cur + 1 > res)
            {
                return;
            }
            ++cur;
            sum += coin;
            GetCoins(index, sum);
            --cur;
            sum -= coin;

            for (int i = index - 1; i >= 0; --i)
            {
                coin = coins[i];
                if (cur + 1 > res)
                {
                    continue;
                }
                ++cur;
                sum += coin;
                GetCoins(i, sum);
                --cur;
                sum -= coin;
            }
        }
        #endregion

        #region "151 Reverse Words in a String"
        private string _151ReverseWordsInAString(string s)
        {
            string[] words = s.Trim().Split(new char[] { ' ' });
            StringBuilder sb = new StringBuilder();
            for (int i = words.Length - 1; i >= 0; i--)
            {
                string word = words[i].Trim();
                if (!String.IsNullOrEmpty(word))
                {
                    if (sb.Length != 0)
                    {
                        sb.Append(" ");
                    }
                    sb.Append(word);
                }
            }
            return sb.ToString();
        }
        #endregion

        #region "557. Reverse Words in a String III"
        private string _557ReverseWordsInAString3(string s)
        {
            char[] chars = s.Trim().ToArray();
            int left = 0, right = 0;
            for (int i = 0; i < chars.Length; i++)
            {
                while (i < chars.Length)
                {
                    if (chars[i] == ' ')
                    {
                        break;
                    }
                    i++;
                }
                right = i - 1;

                while (left < right)
                {
                    char temp = chars[left];
                    chars[left] = chars[right];
                    chars[right] = temp;
                    left++;
                    right--;
                }
                left = i + 1;
            }
            return new string(chars);
        }
        #endregion

        #region "421. Maximum XOR of Two Numbers in an Array"
        public class Trie421
        {
            public Trie421[] children;
            public Trie421()
            {
                children = new Trie421[2];
            }
        }

        //not my answer
        private int _421MaximumXOROfTwoNumbersInAnArray(int[] nums)
        {
            if (nums == null || nums.Length == 0)
            {
                return 0;
            }
            // Init Trie.
            Trie421 root = new Trie421();
            foreach (int num in nums)
            {
                Trie421 curNode = root;
                for (int i = 31; i >= 0; i--)
                {
                    int curBit = (num >> i) & 1;
                    if (curNode.children[curBit] == null)
                    {
                        curNode.children[curBit] = new Trie421();
                    }
                    curNode = curNode.children[curBit];
                }
            }
            int max = Int32.MinValue;
            foreach (int num in nums)
            {
                Trie421 curNode = root;
                int curSum = 0;
                for (int i = 31; i >= 0; i--)
                {
                    int curBit = (num >> i) & 1;
                    if (curNode.children[curBit ^ 1] != null)
                    {
                        curSum += (1 << i);
                        curNode = curNode.children[curBit ^ 1];
                    }
                    else
                    {
                        curNode = curNode.children[curBit];
                    }
                }
                max = Math.Max(curSum, max);
            }
            return max;
        }
        #endregion

        #region "18. 4Sum"
        private IList<IList<int>> _18FourSum2Pointer(int[] nums, int target)
        {
            IList<IList<int>> ans = new List<IList<int>>();
            if (nums.Length < 4) { return ans; }
            Array.Sort(nums);
            int len = nums.Length, lo, hi, sum;
            for (int i = 0; i < len - 3; i++)
            {
                if (nums[i] + nums[i + 1] + nums[i + 2] + nums[i + 3] > target) { break; }
                //if (nums[i] + nums[len - 1] + nums[len - 2] + nums[len - 3] < target) { continue; }
                if (i > 0 && nums[i] == nums[i - 1]) { continue; }

                for (int j = i + 1; j < len - 2; j++)
                {
                    if (nums[i] + nums[j] + nums[j + 1] + nums[j + 2] > target) { break; }
                    //if (nums[i] + nums[j] + nums[len - 1] + nums[len - 2] < target) { continue; }
                    if (j > i + 1 && nums[j] == nums[j - 1]) { continue; }

                    lo = j + 1;
                    hi = len - 1;
                    while (lo < hi)
                    {
                        sum = nums[i] + nums[j] + nums[lo] + nums[hi];
                        if (sum == target)
                        {
                            ans.Add(new List<int>() { nums[i], nums[j], nums[lo], nums[hi] });
                            while (lo < hi && nums[lo] == nums[lo + 1]) { lo++; }
                            while (lo < hi && nums[hi] == nums[hi - 1]) { hi--; }
                            lo++;
                            hi--;
                        }
                        else if (sum < target)
                        {
                            lo++;
                        }
                        else
                        {
                            hi--;
                        }
                    }
                }
            }
            return ans;
        }

        private IList<IList<int>> _18FourSumDP(int[] nums, int target)
        {
            Array.Sort(nums);
            this.nums = nums;
            map18 = new Dictionary<int, List<int>>();
            for (int i = 0; i < nums.Length; i++)
            {
                if (!map18.ContainsKey(nums[i]))
                {
                    map18.Add(nums[i], new List<int>());
                }
                map18[nums[i]].Add(i);
            }

            result = new List<IList<int>>();
            HashSet<int> tried = new HashSet<int>();
            for (int i = 0; i < nums.Length - 3; i++)
            {
                if (tried.Contains(nums[i]))
                {
                    continue;
                }

                cur18 = new List<int>
                {
                    nums[i]
                };
                FourSumSub(target - nums[i], i + 1);

                tried.Add(nums[i]);
            }
            return result;
        }

        private Dictionary<int, List<int>> map18;
        private IList<IList<int>> result;
        private int[] nums;
        private IList<int> cur18;

        private void FourSumSub(int target, int numsIndex)
        {
            if (cur18.Count == 3)
            {
                if (map18.ContainsKey(target))
                {
                    foreach (int n in map18[target])
                    {
                        if (n >= numsIndex)
                        {
                            cur18.Add(target);
                            result.Add(new List<int>(cur18));
                            cur18.RemoveAt(cur18.Count - 1);
                            break;
                        }
                    }
                }
                return;
            }

            HashSet<int> tried = new HashSet<int>();
            for (int i = numsIndex; i < nums.Length - 3 + cur18.Count; i++)
            {
                if (tried.Contains(nums[i]))
                {
                    continue;
                }

                cur18.Add(nums[i]);
                FourSumSub(target - nums[i], i + 1);
                cur18.RemoveAt(cur18.Count - 1);

                tried.Add(nums[i]);
            }
        }
        #endregion

        #region "752. Open the Lock"
        private int _752OpenTheLock(string[] deadends, string target)
        {
            tar = Int32.Parse(target);
            foreach (string deadend in deadends)
            {
                ends.Add(Int32.Parse(deadend));
            }

            WalkTree(0, 0);

            return min;
        }

        private int tar;
        private HashSet<int> ends = new HashSet<int>();
        private int min = Int32.MaxValue;

        private void WalkTree(int input, int count)
        {
            if (ends.Contains(input))
            {
                return;
            }

            if (input == tar)
            {
                min = (int)Math.Min(min, count);
            }

            ends.Add(input);
            for (int i = 3; i >= 0; i--)
            {
                WalkTree((input + (int)Math.Pow(10, i + 1) - (int)Math.Pow(10, i)) % (int)Math.Pow(10, i + 1), count + 1);
                WalkTree((input + (int)Math.Pow(10, i)) % (int)Math.Pow(10, i + 1), count + 1);
            }
        }
        #endregion

        #region "542. 01 Matrix"
        private int[,] _542_01Matrix(int[,] matrix)
        {
            int length0 = matrix.GetLength(0);
            int length1 = matrix.GetLength(1);
            int[,] result = new int[length0, length1];
            Queue<Tuple<int, int>> q = new Queue<Tuple<int, int>>();

            for (int row = 0; row < length0; row++)
            {
                for (int col = 0; col < length1; col++)
                {
                    if (matrix[row, col] == 0)
                    {
                        q.Enqueue(new Tuple<int, int>(row, col));
                    }
                    else
                    {
                        result[row, col] = Int32.MaxValue;
                    }
                }
            }

            int level = 1;
            while (q.Count != 0)
            {
                int size = q.Count;
                while (size-- > 0)
                {
                    int row = q.Peek().Item1;
                    int col = q.Dequeue().Item2;

                    // up
                    if (row > 0)
                    {
                        if (result[row - 1, col] > level)
                        {
                            result[row - 1, col] = level;
                            q.Enqueue(new Tuple<int, int>(row - 1, col));
                        }
                    }
                    // right
                    if (col < length1 - 1)
                    {
                        if (result[row, col + 1] > level)
                        {
                            result[row, col + 1] = level;
                            q.Enqueue(new Tuple<int, int>(row, col + 1));
                        }
                    }
                    // down
                    if (row < length0 - 1)
                    {
                        if (result[row + 1, col] > level)
                        {
                            result[row + 1, col] = level;
                            q.Enqueue(new Tuple<int, int>(row + 1, col));
                        }
                    }
                    // left
                    if (col > 0)
                    {
                        if (result[row, col - 1] > level)
                        {
                            result[row, col - 1] = level;
                            q.Enqueue(new Tuple<int, int>(row, col - 1));
                        }
                    }
                }
                level++;
            }

            return result;
        }
        #endregion

        #region "764. Largest Plus Sign"
        private int _764LargestPlusSign(int N, int[,] mines)
        {
            int minCount = mines.GetLength(0);
            if (minCount == 0)
            {
                return (N - 1) / 2 + 1;
            }

            n = N;
            map764 = new bool[n, n];
            for (int i = 0; i < minCount; i++)
            {
                map764[mines[i, 0], mines[i, 1]] = true;
            }

            count = new int[n, n][];
            for (int row = 0; row < n; row++)
            {
                for (int col = 0; col < n; col++)
                {
                    count[row, col] = new int[4];
                    FillCount(row, col, true);
                }
            }
            for (int row = 0; row < n; row++)
            {
                for (int col = 0; col < n; col++)
                {
                    FillCount(n - 1 - row, n - 1 - col, false);
                }
            }

            int max = 0;
            for (int row = 0; row < n; row++)
            {
                for (int col = 0; col < n; col++)
                {
                    int cur = n;
                    for (int i = 0; i < 4; i++)
                    {
                        cur = (int)Math.Min(cur, count[row, col][i]);
                    }
                    max = (int)Math.Max(max, cur);
                }
            }

            return max;
        }

        private int n;
        private bool[,] map764;
        private int[,][] count;

        private void FillCount(int row, int col, bool leftUp)
        {
            if (leftUp)
            {
                if (!map764[row, col])
                {
                    count[row, col][0] = (row == 0 ? 0 : count[row - 1, col][0]) + 1;
                    count[row, col][1] = (col == 0 ? 0 : count[row, col - 1][1]) + 1;
                }
            }
            else
            {
                if (!map764[row, col])
                {
                    count[row, col][2] = (col == n - 1 ? 0 : count[row, col + 1][2]) + 1;
                    count[row, col][3] = (row == n - 1 ? 0 : count[row + 1, col][3]) + 1;
                }
            }
        }
        #endregion

        #region "215 Kth Largest Element in an Array"
        private int _215KthLargestElementInAnArrayUsingSort(int[] nums, int k)
        {
            Array.Sort(nums);
            return nums[nums.Length - k];
        }
        #endregion

        #region "215. Kth Largest Element in an Array"
        private int _215KthLargestElementInAnArray(int[] nums215, int k)
        {
            this.nums215 = nums215;
            this.k = k;

            int result = GetKthLargest(0, nums215.Length - 1);

            /*foreach(int n in nums215) {
                Console.Write(n + " ");
            }
            Console.WriteLine();*/

            return result;
        }

        private int GetKthLargest(int left, int right)
        {
            if (left < right)
            {
                int pivot = Partition(left, right);

                if (pivot == nums215.Length - k)
                {
                    return nums215[pivot];
                }
                else if (pivot < nums215.Length - k)
                {
                    return GetKthLargest(pivot + 1, right);
                }
                else
                {
                    return GetKthLargest(left, pivot - 1);
                }
            }

            return nums215[nums215.Length - k];
        }

        private int[] nums215;
        private int k;

        private int Partition(int left, int right)
        {
            Random r = new Random();
            int pivot = r.Next(left, right);

            int temp = nums215[right];
            nums215[right] = nums215[pivot];
            nums215[pivot] = temp;

            for (int i = left; i < right; i++)
            {
                if (nums215[i] < nums215[right])
                {
                    temp = nums215[i];
                    nums215[i] = nums215[left];
                    nums215[left++] = temp;
                }
            }

            temp = nums215[right];
            nums215[right] = nums215[left];
            nums215[left] = temp;

            return left;
        }

        private int _215KthLargestElementInAnArrayBst(int[] nums, int k)
        {
            TreeNode215 root = new TreeNode215(nums[0]);

            for (int i = 1; i < nums.Length; i++)
            {
                // add node to tree
                TreeNode215 cur = root;
                while (cur != null)
                {
                    if (nums[i] <= cur.val)
                    {
                        if (cur.left == null)
                        {
                            cur.left = new TreeNode215(nums[i]);
                            break;
                        }
                        cur = cur.left;
                    }
                    else
                    {
                        if (cur.right == null)
                        {
                            cur.right = new TreeNode215(nums[i]);
                            break;
                        }
                        cur = cur.right;
                    }
                }

                // check whether needs to remove smallest
                if (i >= k)
                {
                    cur = root;
                    TreeNode215 prev = null;
                    while (cur.left != null)
                    {
                        prev = cur;
                        cur = cur.left;
                    }

                    if (prev == null)
                    {
                        root = cur.right;
                    }
                    else
                    {
                        if (cur.right == null)
                        {
                            prev.left = null;
                        }
                        else
                        {
                            prev.left = cur.right;
                        }
                    }
                }
            }

            // return last
            while (root.left != null)
            {
                root = root.left;
            }
            return root.val;
        }

        public class TreeNode215
        {
            public int val;
            public TreeNode215 left;
            public TreeNode215 right;

            public TreeNode215(int val)
            {
                this.val = val;
                this.left = null;
                this.right = null;
            }
        }
        #endregion

        #region "406. Queue Reconstruction by Height"
        private int[,] _406QueueReconstructionByHeight(int[,] people)
        {
            int count = people.GetLength(0);
            int[,] result = new int[count, 2];

            Dictionary<int, List<int>> map = new Dictionary<int, List<int>>();
            for (int i = 0; i < count; i++)
            {
                if (!map.ContainsKey(people[i, 0]))
                {
                    map.Add(people[i, 0], new List<int>());
                }
                map[people[i, 0]].Add(people[i, 1]);
                result[i, 0] = -1;
            }

            int[] keys = map.Keys.OrderBy(k => k).ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                List<int> prevs = map[keys[i]];
                prevs.Sort();

                for (int j = 0; j < prevs.Count; j++)
                {
                    InsertPerson(result, keys[i], prevs[j]);
                }
            }

            StringBuilder line1 = new StringBuilder().Append("Input:"),
                line2 = new StringBuilder().Append("Result:");
            for (int i = 0; i < result.GetLength(0); i++)
            {
                line1.Append($" ({people[i, 0]}, {people[i, 1]})");
                line2.Append($" ({result[i, 0]}, {result[i, 1]})");
            }
            Console.WriteLine(line1.ToString());
            Console.WriteLine(line2.ToString());

            return result;
        }

        private void InsertPerson(int[,] result, int height, int prevs)
        {
            int count = 0;
            for (int i = 0; i < result.GetLength(0); i++)
            {
                if (result[i, 0] >= height || result[i, 0] == -1)
                {
                    if (count++ == prevs)
                    {
                        result[i, 0] = height;
                        result[i, 1] = prevs;
                        return;
                    }
                }
            }
        }
        #endregion

        #region "204. Count Primes"

        private int _204CountPrimes(int n)
        {
            bool[] notPrime = new bool[n];
            int count = 0;

            for(int i = 2; i < n; i++)
            {
                if(!notPrime[i])
                {
                    count++;

                    for(int j = 2; i * j < n; j++)
                    {
                        notPrime[i * j] = true;
                    }
                }
            }

            return count;
        }

        private int _204CountPrimesMy(int n)
        {
            if (n <= 2)
            {
                return 0;
            }

            List<int> primes = new List<int> { 2 };
            for (int i = 3; i < n; i++)
            {
                bool isPrime = true;
                foreach (int prime in primes)
                {
                    if (i % prime == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }

                if (isPrime)
                {
                    primes.Add(i);
                }
            }

            return primes.Count;
        }

        #endregion

        [Test]
        public void TestMedium()
        {
            #region "204. Count Primes"
            
            Assert.That(_204CountPrimes(499979), Is.EqualTo(41537));

            #endregion

            #region "406. Queue Reconstruction by Height"

            //Assert.That(_406QueueReconstructionByHeight(new int[,] {
            //    { 7, 0 }, { 4, 4 }, { 7, 1 }, { 5, 0 }, { 6, 1 }, { 5, 2 }
            //}), Is.EqualTo(new int[,] {
            //    { 5, 0 }, { 7, 0 }, { 5, 2 }, { 6, 1 }, { 4, 4 }, { 7, 1 }
            //}));
            //Assert.That(_406QueueReconstructionByHeight(new int[,] { { 2, 4 }, { 3, 4 }, { 9, 0 }, { 0, 6 }, { 7, 1 }, { 6, 0 }, { 7, 3 }, { 2, 5 }, { 1, 1 }, { 8, 0 } }),
            //    Is.EqualTo(new int[,] { { 6, 0 }, { 1, 1 }, { 8, 0 }, { 7, 1 }, { 9, 0 }, { 2, 4 }, { 0, 6 }, { 2, 5 }, { 3, 4 }, { 7, 3 } }));

            #endregion

            #region "215 Kth Largest Element in an Array"

            //Assert.That(this._215KthLargestElementInAnArray(new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3 }, 8), Is.EqualTo(3));
            //Assert.That(this._215KthLargestElementInAnArray(new int[] { 3, 2, 1, 5, 6, 4 }, 2), Is.EqualTo(5));
            //Assert.That(this._215KthLargestElementInAnArray(new int[] { 3, 2, 3, 1, 2, 4, 5, 5, 6 }, 4), Is.EqualTo(4));

            #endregion

            #region "764. Largest Plus Sign"
            //Assert.That(this._764LargestPlusSign(2, new int[,] { { 0, 0 }, { 0, 1 }, { 1, 0 } }), Is.EqualTo(1));
            //Assert.That(this._764LargestPlusSign(5, new int[,] { { 0, 0 }, { 0, 3 }, { 1, 1 }, { 1, 4 }, { 2, 3 }, { 3, 0 }, { 4, 2 } }), Is.EqualTo(1));
            //Assert.That(this._764LargestPlusSign(5, new int[,] { { 4, 2 } }), Is.EqualTo(2));
            #endregion

            #region "542. 01 Matrix"
            //Assert.That(this._542_01Matrix(new int[,]
            //{
            //    { 0, 0, 0 },
            //    { 0, 1, 0 },
            //    { 1, 1, 1 },
            //}), Is.EqualTo(new int[,] {
            //    { 0, 0, 0 },
            //    { 0, 1, 0 },
            //    { 1, 2, 1 },
            //}));
            #endregion

            #region "752. Open the Lock"
            //Assert.That(this._752OpenTheLock(new string[] { "0201", "0101", "0102", "1212", "2002" }, "0202"), Is.EqualTo(6));
            #endregion

            #region "18. 4Sum"
            //int[][] inputs = new int[][]
            //{
            //    new int[] { -1, 0, -5, -2, -2, -4, 0, 1, -2 },
            //    new int[] { 5, 5, 3, 5, 1, -5, 1, -2 },
            //    new int[] { -5, 5, 4, -3, 0, 0, 4, -2 },
            //    new int[] { -3, -2, -1, 0, 0, 1, 2, 3 },
            //    new int[] { 1, 0, -1, 0, -2, 2 },
            //};
            //int[] targets = new int[] { -9, 4, 4, 0, 0 };
            //List<IList<IList<int>>> answers = new List<IList<IList<int>>>()
            //{
            //    new List<IList<int>>()
            //    {
            //        new List<int>() { -5, -4, -1, 1 },
            //        new List<int>() { -5, -4, 0, 0 },
            //        new List<int>() { -5, -2, -2, 0 },
            //        new List<int>() { -4, -2, -2, -1 },
            //    },
            //    new List<IList<int>>()
            //    {
            //        new List<int>() { -5, 1, 3, 5 },
            //    },
            //    new List<IList<int>>()
            //    {
            //        new List<int>() { -5, 0, 4, 5 },
            //        new List<int>() { -3, -2, 4, 5 },
            //    },
            //    new List<IList<int>>()
            //    {
            //        new List<int>() { -3, -2, 2, 3 },
            //        new List<int>() { -3, -1, 1, 3 },
            //        new List<int>() { -3, 0, 0, 3 },
            //        new List<int>() { -3, 0, 1, 2 },
            //        new List<int>() { -2, -1, 0, 3 },
            //        new List<int>() { -2, -1, 1, 2 },
            //        new List<int>() { -2, 0, 0, 2 },
            //        new List<int>() { -1, 0, 0, 1 },
            //    },
            //    new List<IList<int>>()
            //    {
            //        new List<int>() { -2, -1, 1, 2 },
            //        new List<int>() { -2, 0, 0, 2 },
            //        new List<int>() { -1, 0, 0, 1 },
            //    },
            //};
            //Assert.That(this._18FourSum2Pointer(inputs[0], targets[0]), Is.EquivalentTo(answers[0]));
            //Assert.That(this._18FourSum2Pointer(inputs[1], targets[1]), Is.EquivalentTo(answers[1]));
            //Assert.That(this._18FourSum2Pointer(inputs[2], targets[2]), Is.EquivalentTo(answers[2]));
            //Assert.That(this._18FourSum2Pointer(inputs[3], targets[3]), Is.EquivalentTo(answers[3]));
            //Assert.That(this._18FourSum2Pointer(inputs[4], targets[4]), Is.EquivalentTo(answers[4]));

            //Assert.That(this._18FourSumDP(inputs[0], targets[0]), Is.EquivalentTo(answers[0]));
            //Assert.That(this._18FourSumDP(inputs[1], targets[1]), Is.EquivalentTo(answers[1]));
            //Assert.That(this._18FourSumDP(inputs[2], targets[2]), Is.EquivalentTo(answers[2]));
            //Assert.That(this._18FourSumDP(inputs[3], targets[3]), Is.EquivalentTo(answers[3]));
            //Assert.That(this._18FourSumDP(inputs[4], targets[4]), Is.EquivalentTo(answers[4]));

            //for (int index = 0; index < inputs.Length; index++)
            //{
            //    StringBuilder sb = new StringBuilder();
            //    sb.Append("Printing result(s) for input: [");
            //    foreach (int i in inputs[index])
            //    {
            //        sb.AppendFormat("{0} ", i);
            //    }
            //    sb.AppendFormat("], target {0}", targets[index]);
            //    Console.WriteLine(sb.ToString());
            //    foreach (List<int> list in this._18FourSumDP(inputs[index], targets[index]))
            //    {
            //        sb = new StringBuilder();
            //        foreach (int num in list)
            //        {
            //            sb.AppendFormat("{0} ", num);
            //        }
            //        Console.WriteLine(sb.ToString());
            //    }
            //}
            #endregion

            #region "421. Maximum XOR of Two Numbers in an Array"
            //Assert.That(this._421MaximumXOROfTwoNumbersInAnArray(new int[] { 3, 10, 5, 25, 2, 8 }), Is.EqualTo(5 ^ 25));
            //Assert.That(this._421MaximumXOROfTwoNumbersInAnArray(new int[] { 3, 10, 5, 25, 2, 8, 30 }), Is.EqualTo(3 ^ 30));
            //Assert.That(this._421MaximumXOROfTwoNumbersInAnArray(new int[] { 3, 10, 5, 25, 2, 8, 30, 26 }), Is.EqualTo(5 ^ 26));
            #endregion

            #region "557. Reverse Words in a String III"
            //Assert.That(this._557ReverseWordsInAString3("Let's take LeetCode contest"), Is.EqualTo("s'teL ekat edoCteeL tsetnoc"));
            //Assert.That(this._557ReverseWordsInAString3(" Let's take LeetCode contest "), Is.EqualTo("s'teL ekat edoCteeL tsetnoc"));
            #endregion

            #region "151 Reverse Words in a String"
            //Assert.That(this._151ReverseWordsInAString("the  sky is blue"), Is.EqualTo("blue is sky the"));
            //Assert.That(this._151ReverseWordsInAString("the sky is blue"), Is.EqualTo("blue is sky the"));
            //Assert.That(this._151ReverseWordsInAString(" the sky is blue "), Is.EqualTo("blue is sky the"));
            #endregion

            #region "322 Coin Change"
            //Assert.That(this._322CoinChange(new int[] { 186, 419, 83, 408 }, 6249), Is.EqualTo(20));
            //Assert.That(this._322CoinChange(new int[] { 1, 2, 5 }, 0), Is.EqualTo(0));
            //Assert.That(this._322CoinChange(new int[] { 3 }, 2), Is.EqualTo(-1));
            //Assert.That(this._322CoinChange(new int[] { }, 11), Is.EqualTo(-1));
            //Assert.That(this._322CoinChange(new int[] { 1, 2, 5 }, 11), Is.EqualTo(3));
            #endregion

            #region "498 Diagonal Traverse"
            //Assert.That(this._498DiagonalTraverse(new int[,] {
            //    { 1, 2, 3 },
            //    { 4, 5, 6 }
            //}), Is.EqualTo(new int[] { 1, 2, 4, 5, 3, 6 }));
            //Assert.That(this._498DiagonalTraverse(new int[,] { }), Is.EqualTo(new int[] { }));
            //Assert.That(this._498DiagonalTraverse(null), Is.EqualTo(new int[] { }));
            //Assert.That(this._498DiagonalTraverse(new int[,] {
            //    { 00, 01, 02, 03, 04 },
            //    { 10, 11, 12, 13, 14 },
            //    { 20, 21, 22, 23, 24 },
            //    { 30, 31, 32, 33, 34 },
            //    { 40, 41, 42, 43, 44 }
            //}), Is.EqualTo(new int[] { 0, 1, 10, 20, 11, 2, 3, 12, 21, 30, 40, 31, 22, 13, 4, 14, 23, 32, 41, 42, 33, 24, 34, 43, 44 }));
            //Assert.That(this._498DiagonalTraverse(new int[,] {
            //    { 1, 2, 3 },
            //    { 4, 5, 6 },
            //    { 7, 8, 9 }
            //}), Is.EqualTo(new int[] { 1, 2, 4, 7, 5, 3, 6, 8, 9 }));
            #endregion

            #region "347 Top K Frequent Elements"
            //Assert.That(this._347TopKFrequentElements(new int[] { 5, 3, 1, 1, 1, 3, 73, 1 }, 3), Is.EqualTo(new List<int>() { 1, 3, 5 }));
            //Assert.That(this._347TopKFrequentElements(new int[] { 5, 3, 1, 1, 1, 3, 73, 1 }, 1), Is.EqualTo(new List<int>() { 1 }));
            //Assert.That(this._347TopKFrequentElements(new int[] { 1, 3, 2, 1, 2, 1 }, 2), Is.EqualTo(new List<int>() { 1, 2 }));
            //Assert.That(this._347TopKFrequentElements(new int[] { 1, 1, 1, 2, 2, 3 }, 2), Is.EqualTo(new List<int>() { 1, 2 }));
            //Assert.That(this._347TopKFrequentElements(new int[] { 1 }, 1), Is.EqualTo(new List<int>() { 1 }));
            #endregion

            #region "31 Next Permutation"
            //int[] input;

            //input = new int[] { 1, 2, 7, 4, 3, 1 };
            //this._31NextPermutation(input);
            //Assert.That(input, Is.EqualTo(new int[] { 1, 3, 1, 2, 4, 7 }));

            //input = new int[] { 1, 1, 5 };
            //this._31NextPermutation(input);
            //Assert.That(input, Is.EqualTo(new int[] { 1, 5, 1 }));

            //input = new int[] { 1, 2, 3 };
            //this._31NextPermutation(input);
            //Assert.That(input, Is.EqualTo(new int[] { 1, 3, 2 }));

            //input = new int[] { 3, 2, 1 };
            //this._31NextPermutation(input);
            //Assert.That(input, Is.EqualTo(new int[] { 1, 2, 3 }));
            #endregion

            #region "12 Integer to Roman"
            //Assert.That(this._12IntegerToRoman(4), Is.EqualTo("IV"));
            //Assert.That(this._12IntegerToRoman(3), Is.EqualTo("III"));
            //Assert.That(this._12IntegerToRoman(9), Is.EqualTo("IX"));
            //Assert.That(this._12IntegerToRoman(58), Is.EqualTo("LVIII"));
            //Assert.That(this._12IntegerToRoman(1994), Is.EqualTo("MCMXCIV"));
            //Assert.That(this._12IntegerToRoman(1), Is.EqualTo("I"));
            //Assert.That(this._12IntegerToRoman(3999), Is.EqualTo("MMMCMXCIX"));
            #endregion

            #region "8 String to Integer"
            //Assert.That(this._8StringtoInteger("-"), Is.EqualTo(0));
            //Assert.That(this._8StringtoInteger("+"), Is.EqualTo(0));
            //Assert.That(this._8StringtoInteger("       "), Is.EqualTo(0));
            //Assert.That(this._8StringtoInteger(""), Is.EqualTo(0));
            //Assert.That(this._8StringtoInteger("  0000000000012345678"), Is.EqualTo(12345678));
            //Assert.That(this._8StringtoInteger("-91283472332"), Is.EqualTo(Int32.MinValue));
            //Assert.That(this._8StringtoInteger("20000000000000000000"), Is.EqualTo(Int32.MaxValue));
            //Assert.That(this._8StringtoInteger("words and 987"), Is.EqualTo(0));
            //Assert.That(this._8StringtoInteger("4193 with words"), Is.EqualTo(4193));
            //Assert.That(this._8StringtoInteger("   -42"), Is.EqualTo(-42));
            //Assert.That(this._8StringtoInteger("-42"), Is.EqualTo(-42));
            #endregion

            #region "721 Accounts Merge"
            //Assert.That(this._721AccountsMerge(new List<List<string>>()
            //{
            //    new List<string>() { "Gabe","Gabe0@m.co","Gabe3@m.co","Gabe1@m.co" },
            //    new List<string>() { "Kevin","Kevin3@m.co","Kevin5@m.co","Kevin0@m.co" },
            //    new List<string>() { "Ethan","Ethan5@m.co","Ethan4@m.co","Ethan0@m.co" },
            //    new List<string>() { "Hanzo","Hanzo3@m.co","Hanzo1@m.co","Hanzo0@m.co" },
            //    new List<string>() { "Fern","Fern5@m.co","Fern1@m.co","Fern0@m.co" },
            //}), Is.EquivalentTo(new List<IList<string>>()
            //{
            //    new List<string>(){ "Ethan", "Ethan0@m.co", "Ethan4@m.co", "Ethan5@m.co" },
            //    new List<string>(){ "Gabe", "Gabe0@m.co", "Gabe1@m.co", "Gabe3@m.co" },
            //    new List<string>(){ "Hanzo", "Hanzo0@m.co", "Hanzo1@m.co", "Hanzo3@m.co" },
            //    new List<string>(){ "Kevin", "Kevin0@m.co", "Kevin3@m.co", "Kevin5@m.co" },
            //    new List<string>(){ "Fern", "Fern0@m.co", "Fern1@m.co", "Fern5@m.co" },
            //}));
            //Assert.That(this._721AccountsMerge(new List<List<string>>()
            //{
            //    new List<string>() { "John", "johnsmith@mail.com", "john_newyork@mail.com" },
            //    new List<string>() { "John", "johnsmith@mail.com", "john00@mail.com" },
            //    new List<string>() { "Mary", "mary@mail.com" },
            //    new List<string>() { "John", "johnnybravo@mail.com" },
            //}), Is.EquivalentTo(new List<List<string>>()
            //{
            //    new List<string>() { "John", "john00@mail.com", "john_newyork@mail.com", "johnsmith@mail.com" },
            //    new List<string>() { "Mary","mary@mail.com" },
            //    new List<string>() { "John","johnnybravo@mail.com" },
            //}));
            #endregion

            #region "80 Remove Duplicates from Sorted Array 2"
            //int[] input;

            //input = new int[] { 1, 1, 1, 2, 2, 3 };
            //Assert.That(this._80RemoveDuplicatesFromSortedArray2(input), Is.EqualTo(5));
            //Assert.That(input, Is.EqualTo(new int[] { 1, 1, 2, 2, 3, 3 }));

            //input = new int[] { 0, 0, 1, 1, 1, 1, 2, 3, 3 };
            //Assert.That(this._80RemoveDuplicatesFromSortedArray2(input), Is.EqualTo(7));
            //Assert.That(input, Is.EqualTo(new int[] { 0, 0, 1, 1, 2, 3, 3, 3, 3 }));
            #endregion

            #region "261 Graph Valid Tree"
            //Assert.True(this._261GraphValidTree(3, new int[,]
            //{
            //    { 0, 1 }, { 2, 0 }
            //}));
            //Assert.True(this._261GraphValidTree(1, new int[,] { }));
            //Assert.False(this._261GraphValidTree(2, new int[,] { }));
            //Assert.False(this._261GraphValidTree(4, new int[,]
            //{
            //    { 0, 1 }, { 2, 3 },
            //}));
            //Assert.False(this._261GraphValidTree(5, new int[,]
            //{
            //    { 0, 1 }, { 1, 2 }, { 2, 3 }, { 1, 3 }, { 1, 4 }
            //}));
            //Assert.True(this._261GraphValidTree(5, new int[,]
            //{
            //    { 0, 1 }, { 0, 2 }, { 0, 3 }, { 1, 4 }
            //}));
            #endregion

            #region "426 Convert Binary Search Tree to Sorted Doubly Linked List"
            //TreeNode root = new TreeNode();
            //StringBuilder sb = new StringBuilder();

            //root = this._426ConvertBinarySearchTreeToSortedDoublyLinkedList(new TreeNode(new List<int>() { 4, 2, 6, 1, 3, 5, 7 }));
            //int val = root.val;
            //sb = new StringBuilder();
            //while(true)
            //{
            //    sb.Append(root.val);
            //    root = root.right;
            //    if (root.val == val)
            //        break;
            //}
            //Assert.That(sb.ToString(), Is.EqualTo("1234567"));
            #endregion

            #region "285 Inorder Successor in BST"
            //TreeNode root = new TreeNode();

            //root = new TreeNode(new List<int>() { 5, 3, 6, 2, 4, -1, -1, 1, -1 });
            //Assert.That(this._285InorderSuccessorInBST(root, new TreeNode(1)).val, Is.EqualTo(2));

            //root = new TreeNode(new List<int>() { 2, 1, 3 });
            //Assert.That(this._285InorderSuccessorInBST(root, new TreeNode(1)).val, Is.EqualTo(2));
            //root = new TreeNode(new List<int>() { 5, 3, 6, 2, 4, -1, -1, 1, -1 });
            //Assert.Null(this._285InorderSuccessorInBST(root, new TreeNode(6)));
            #endregion

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
            Assert.That(this._253MeetingRooms2(new Interval[] { new Interval(65, 424), new Interval(351, 507), new Interval(314, 807), new Interval(387, 722), new Interval(19, 797), new Interval(259, 722), new Interval(165, 221), new Interval(136, 897) }), Is.EqualTo(7));
            Assert.That(this._253MeetingRooms2(new Interval[] { new Interval(2, 7) }), Is.EqualTo(1));
            Assert.That(this._253MeetingRooms2(new Interval[] { new Interval(0, 30), new Interval(5, 10), new Interval(15, 20) }), Is.EqualTo(2));
            Assert.That(this._253MeetingRooms2(new Interval[] { new Interval(2, 3), new Interval(4, 5) }), Is.EqualTo(1));
            Assert.That(this._253MeetingRooms2(new Interval[] { new Interval(5, 8), new Interval(6, 8) }), Is.EqualTo(2));
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
