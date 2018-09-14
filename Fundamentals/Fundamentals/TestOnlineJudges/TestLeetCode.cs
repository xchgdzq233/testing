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

        #region "13 Roman to Integer"
        private int _13RomanToInteger(string s)
        {
            Dictionary<char, int> map = new Dictionary<char, int>();
            map.Add('I', 1);
            map.Add('V', 5);
            map.Add('X', 10);
            map.Add('L', 50);
            map.Add('C', 100);
            map.Add('D', 500);
            map.Add('M', 1000);

            int result = 0, last = 0;
            for (int i = s.Length - 1; i >= 0; i--)
            {
                int current = map[s[i]];
                if (current < last)
                    result -= current;
                else
                {
                    result += current;
                    last = current;
                }
            }

            return result;
        }
        #endregion

        #region "20 Valid Parentheses"
        private bool _20ValidParentheses(string s)
        {
            if (s.Length == 0) return true;

            Stack<char> st = new Stack<char>();
            for (int i = 0; i <= s.Length - 1; i++)
            {
                if (s[i] == '(')
                    st.Push(')');
                else if (s[i] == '[')
                    st.Push(']');
                else if (s[i] == '{')
                    st.Push('}');
                else
                {
                    if (st.Count == 0 || st.Pop() != s[i])
                        return false;
                }
            }

            return st.Count == 0;
        }

        private bool _20MyValidParentheses(string s)
        {
            Stack<char> st = new Stack<char>();
            for (int i = 0; i <= s.Length - 1; i++)
            {
                if (s[i] == '(' || s[i] == '[' || s[i] == '{')
                    st.Push(s[i]);
                else
                {
                    if (st.Count == 0)
                        return false;

                    char last = st.Pop();
                    if ((s[i] == ')' && last != '(') || (s[i] == ']' && last != '[') || (s[i] == '}' && last != '{'))
                        return false;
                }
            }
            return st.Count == 0;
        }
        #endregion

        #region "21 Merge Two Sorted Lists"
        private ListNode _21MergeTwoSortedLists(ListNode l1, ListNode l2)
        {
            return this.MergeTwoLists(l1, l2);
        }

        private ListNode MergeTwoLists(ListNode l1, ListNode l2)
        {
            if (l1 == null) return l2;
            if (l2 == null) return l1;

            if (l1.val < l2.val)
            {
                l1.next = MergeTwoLists(l1.next, l2);
                return l1;
            }
            l2.next = MergeTwoLists(l1, l2.next);
            return l2;
        }

        private ListNode _21MyMergeTwoSortedLists(ListNode l1, ListNode l2)
        {
            if (l1 == null) return l2;
            if (l2 == null) return l1;

            ListNode dummy = new ListNode(-1);
            ListNode current = dummy;

            while (l1 != null && l2 != null)
            {
                if (l1.val < l2.val)
                {
                    current.next = l1;
                    l1 = l1.next;
                }
                else
                {
                    current.next = l2;
                    l2 = l2.next;
                }
                current = current.next;
            }

            if (l1 != null)
                current.next = l1;
            if (l2 != null)
                current.next = l2;

            return dummy.next;
        }
        #endregion

        #region "206 Reverse Linked List"
        private ListNode _206ReverseLinkedList(ListNode head)
        {
            return this.ReverseList(head);
        }

        private ListNode ReverseList(ListNode head)
        {
            if (head == null || head.next == null)
                return head;

            ListNode dummy = ReverseList(head.next);
            head.next.next = head;
            head.next = null;
            return dummy;
        }

        private ListNode _206ReverseLinkedListIteratively(ListNode head)
        {
            if (head == null) return null;

            Stack<ListNode> s = new Stack<ListNode>();
            while (head != null)
            {
                s.Push(head);
                head = head.next;
            }

            ListNode dummy = new ListNode(-1);
            head = s.Pop();
            dummy.next = head;
            while (s.Count != 0)
            {
                head.next = s.Pop();
                head = head.next;
            }
            head.next = null;
            return dummy.next;
        }
        #endregion

        #region "88 Merge Sorted Array"
        private void _88MergeSortedArray(int[] nums1, int m, int[] nums2, int n)
        {
            m--;
            n--;
            for (int i = nums1.Length - 1; m >= 0 && n >= 0; i--)
            {
                if (nums1[m] > nums2[n])
                    nums1[i] = nums1[m--];
                else
                    nums1[i] = nums2[n--];
            }

            for (int i = 0; i <= n; i++)
                nums1[i] = nums2[i];
        }
        #endregion

        [Test]
        public void TestEasy()
        {
            #region "88 Merge Sorted Array"
            //int[] nums1 = null;

            //nums1 = new int[] { 2, 5, 6, 0, 0, 0 };
            //this._88MergeSortedArray(nums1, 3, new int[] { 1, 2, 3 }, 3);
            //Assert.That(nums1, Is.EquivalentTo(new int[] { 1, 2, 2, 3, 5, 6 }));

            //nums1 = new int[] { 1, 2, 3, 0, 0, 0 };
            //this._88MergeSortedArray(nums1, 3, new int[] { 2, 5, 6 }, 3);
            //Assert.That(nums1, Is.EquivalentTo(new int[] { 1, 2, 2, 3, 5, 6 }));
            #endregion

            #region "206 Reverse Linked List"
            //Assert.That(this._206ReverseLinkedList(new ListNode(new List<int>() { 1, 2, 3, 4, 5 })).GetThisAndAfterInString(), Is.EqualTo("5 4 3 2 1 "));
            #endregion

            #region "21 Merge Two Sorted Lists"
            //Assert.That(this._21MergeTwoSortedLists(new ListNode(new List<int>() { 1, 2, 4 }), new ListNode(new List<int>() { 1, 3, 4 })).GetThisAndAfterInString(), Is.EqualTo("1 1 2 3 4 4 "));
            #endregion

            #region "20 Valid Parentheses"
            //Assert.False(this._20ValidParentheses("["));
            //Assert.False(this._20ValidParentheses("]"));
            //Assert.True(this._20ValidParentheses("()"));
            //Assert.True(this._20ValidParentheses("()[]{}"));
            //Assert.False(this._20ValidParentheses("(]"));
            //Assert.False(this._20ValidParentheses("([)]"));
            //Assert.True(this._20ValidParentheses("{[]}"));
            #endregion

            #region "13 Roman to Integer"
            //Assert.That(this._13RomanToInteger("III"), Is.EqualTo(3));
            //Assert.That(this._13RomanToInteger("IV"), Is.EqualTo(4));
            //Assert.That(this._13RomanToInteger("IX"), Is.EqualTo(9));
            //Assert.That(this._13RomanToInteger("LVIII"), Is.EqualTo(58));
            //Assert.That(this._13RomanToInteger("MCMXCIV"), Is.EqualTo(1994));
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

        #region "285 Inorder Successor in BST"
        private TreeNode _285InorderSuccessorInBST(TreeNode root, TreeNode p)
        {
            if (root == null) return null;

            TreeNode[] result = new TreeNode[2];
            InorderSuccessorSub(root, p, result);
            return result[1] == null ? null : result[1];
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
            if (!helper(map, visited, 0))
                return false;
            return visited.Count == n;
        }

        private bool helper(List<List<int>> map, HashSet<int> visited, int current, int parent = -1)
        {
            if (visited.Contains(current)) return false;
            visited.Add(current);
            foreach (int i in map[current])
            {
                if (i == parent) continue;
                if (!helper(map, visited, i, current))
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
            int[] parent;

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
                List<string> account = new List<string>();
                account.Add(key);
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

        #region ""

        #endregion

        [Test]
        public void TestMedium()
        {
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

        #region "146 LRU Cache"
        public class LRUNode
        {
            public int key;
            public int val;
            public LRUNode left;
            public LRUNode right;

            public LRUNode()
            {
                this.key = -1;
                this.val = -1;
                this.left = null;
                this.right = null;
            }

            public LRUNode(int key, int val)
            {
                this.key = key;
                this.val = val;
                this.left = null;
                this.right = null;
            }
        }

        public class LRUCache
        {
            private int capacity;
            private Dictionary<int, LRUNode> map;
            private LRUNode first;
            private LRUNode last;

            public LRUCache(int capacity)
            {
                this.capacity = capacity;
                this.map = new Dictionary<int, LRUNode>();
                this.first = new LRUNode();
                this.last = new LRUNode();
                this.first.right = this.last;
                this.last.left = this.first;
            }

            private void Use(LRUNode node)
            {
                LRUNode left = node.left;
                if (left != null)
                {
                    LRUNode right = node.right;
                    left.right = right;
                    right.left = left;
                }

                this.last.left.right = node;
                node.left = this.last.left;
                node.right = this.last;
                this.last.left = node;
            }

            public int Get(int key)
            {
                if (!map.ContainsKey(key))
                    return -1;

                LRUNode result = this.map[key];
                this.Use(result);
                return result.val;
            }

            public void Put(int key, int value)
            {
                LRUNode node;
                if (this.map.ContainsKey(key))
                {
                    node = this.map[key];
                    node.val = value;
                }
                else
                {
                    node = new LRUNode(key, value);

                    if (this.capacity > 0)
                    {
                        map.Add(key, node);
                        this.capacity--;
                    }
                    else
                    {
                        LRUNode toRemove = this.first.right;
                        this.first.right = toRemove.right;
                        toRemove.right.left = this.first;
                        this.map.Remove(toRemove.key);

                        map.Add(key, node);
                    }
                }
                this.Use(node);
            }
        }
        #endregion

        #region "273 Integer to English Words"
        private string _273IntegerToEnglishWords(int num)
        {
            if (num == 0) return "Zero";

            numToWordMap = new Dictionary<int, string>()
            {
                { 1, "One" }, { 2, "Two" }, { 3, "Three" }, { 4, "Four" }, { 5, "Five" },
                { 6, "Six" }, { 7, "Seven" }, { 8, "Eight" }, { 9, "Nine" }, { 10, "Ten" },
                { 11, "Eleven" }, { 12, "Twelve" }, { 13, "Thirteen" }, { 14, "Fourteen" }, { 15, "Fifteen" },
                { 16, "Sixteen" }, { 17, "Seventeen" }, { 18, "Eighteen" }, { 19, "Nineteen"},
                { 20, "Twenty"}, { 30, "Thirty" }, { 40, "Forty" }, { 50, "Fifty" },
                { 60, "Sixty" }, { 70, "Seventy" }, { 80, "Eighty" }, { 90, "Ninety" },
            };

            int[] nums = new int[4];
            nums[0] = num / 1000000000;
            nums[1] = num % 1000000000 / 1000000;
            nums[2] = num % 1000000 / 1000;
            nums[3] = num % 1000;

            string[] words = new string[4];
            if (nums[0] != 0)
                words[0] = numToWordMap[nums[0]];

            for (int i = 1; i <= 3; i++)
                if (nums[i] != 0)
                    words[i] = NumToWord(nums[i]);

            string result = "";
            if (nums[0] != 0)
                result = words[0] + " Billion";
            if (nums[1] != 0)
                result += (result.Length == 0 ? "" : " ") + words[1] + " Million";
            if (nums[2] != 0)
                result += (result.Length == 0 ? "" : " ") + words[2] + " Thousand";
            if (nums[3] != 0)
                result += (result.Length == 0 ? "" : " ") + words[3];

            return result;
        }

        private Dictionary<int, string> numToWordMap;

        private string NumToWord(int num)
        {
            string result = "";
            if (num >= 100)
                result = numToWordMap[num / 100] + " Hundred";

            num %= 100;
            if (num / 10 == 1)
                return result + (result.Length == 0 ? "" : " ") + numToWordMap[num];

            if (num / 10 != 0)
            {
                result += (result.Length == 0 ? "" : " ") + numToWordMap[num / 10 * 10];
                num %= 10;
            }

            if (num > 0)
                result += (result.Length == 0 ? "" : " ") + numToWordMap[num];

            return result;
        }
        #endregion

        #region "23 Merge k Sorted Lists"
        private ListNode _23MergeKSortedLists(ListNode[] lists)
        {
            if (lists.Length == 0) return null;
            if (lists.Length == 1) return lists[0];

            return MergeListNodeList(lists, 0, lists.Length - 1);
        }

        private ListNode MergeListNodeList(ListNode[] lists, int low, int high)
        {
            if (low > high) return null;
            if (low == high) return lists[low];

            int mid = low + (high - low) / 2;
            return MergeTwoLists(MergeListNodeList(lists, low, mid), MergeListNodeList(lists, mid + 1, high));
        }

        private ListNode _23MyMergeKSortedLists(ListNode[] lists)
        {
            if (lists.Length == 0) return null;
            if (lists.Length == 1) return lists[0];

            dummy = new ListNode(-1);
            last = new ListNode(-1);
            last.next = dummy;

            int minIndex = -1, i = 0;

            while (true)
            {
                if (minIndex == -1)
                {
                    for (i = 0; i <= lists.Length - 1; i++)
                        if (lists[i] != null)
                        {
                            minIndex = i;
                            break;
                        }

                    if (minIndex == -1)
                        break;
                }

                ListNode current = lists[i];
                if (current != null)
                    if (current.val < lists[minIndex].val)
                        minIndex = i;
                i++;

                if (i == lists.Length)
                {
                    if (lists[minIndex] == null)
                        return null;

                    Insert(lists, minIndex);

                    minIndex = -1;
                    i = 0;
                }
            }

            return dummy.next;
        }

        private ListNode dummy;
        private ListNode last;

        private void Insert(ListNode[] lists, int index)
        {
            last.next.next = lists[index];
            lists[index] = lists[index].next;
            last.next = last.next.next;
        }
        #endregion

        #region "301 Remove Invalid Parentheses (didn't finish)"
        private IList<string> _301RemoveInvalidParentheses(string s)
        {
            IList<string> result = new List<string>();
            if (s.Length == 0) return result;

            Tuple<int, int> invalid = CalculateInvalid(s);
            //dfs(s, 0, invalid.Item1, invalid.Item2, result);
            return result;
        }

        private bool IsValid(string s)
        {
            int count = 0;
            for (int i = 0; i <= s.Length - 1; i++)
            {
                if (s[i] == '(') count++;
                else if (s[i] == ')') count--;

                if (count < 0) return false;
            }
            return count == 0;
        }

        private Tuple<int, int> CalculateInvalid(string s)
        {
            int left = 0, right = 0;
            for (int i = 0; i <= s.Length - 1; i++)
            {
                if (s[i] == '(')
                    left++;
                else if (s[i] == ')')
                {
                    if (left > 0)
                        left--;
                    else
                        right++;
                }
            }
            return new Tuple<int, int>(left, right);
        }
        #endregion

        #region "76 Minimum Window Substring"
        private string _76MinimumWindowSubstring(string s, string t)
        {
            if (t.Length > s.Length) return "";

            int[] map = new int[128];
            for (int i = 0; i <= t.Length - 1; i++)
                map[t[i]]++;

            int count = t.Length, start = 0, min = Int32.MaxValue;
            for (int i = 0, j = 0; i <= s.Length - 1; i++)
            {
                if (map[s[i]]-- > 0)
                    count--;

                while (count == 0)
                {
                    if (i - j + 1 < min)
                    {
                        min = i - j + 1;
                        start = j;
                    }

                    if (++map[s[j++]] > 0)
                        count++;
                }
            }

            return min == Int32.MaxValue ? "" : s.Substring(start, min);
        }
        private string _76MyMinimumWindowSubstring(string s, string t)
        {
            Dictionary<char, ListNode> map = CreateDictionary(t);
            ListNode dummy = new ListNode(-1);
            ListNode end = dummy;
            int start = GetInitialPosition(s, map, ref end);
            if (start == -1)
                return "";

            for (; start <= s.Length - 1; start++)
                if (map.ContainsKey(s[start]))
                    UpdateMap(map, s[start], start, ref end);

            StringBuilder result = new StringBuilder();
            for (int i = dummy.next.val; i <= end.val; i++)
                result.Append(s[i]);
            return result.ToString();
        }

        private void UpdateMap(Dictionary<char, ListNode> map, char c, int i, ref ListNode end)
        {
            ListNode oldNode = map[c];
            ListNode newNode = new ListNode(i);
            RemoveNode(oldNode);
            AddNodeToEnd(ref end, ref newNode);
        }

        private void RemoveNode(ListNode node)
        {
            ListNode pre = node.pre;
            ListNode next = node.next;
            pre.next = next;
            next.pre = pre;
        }

        private int GetInitialPosition(string s, Dictionary<char, ListNode> map, ref ListNode end)
        {
            int count = 0;

            for (int i = 0; i <= s.Length - 1; i++)
                if (map.ContainsKey(s[i]))
                {
                    ListNode node = new ListNode(i);
                    map[s[i]] = node;

                    AddNodeToEnd(ref end, ref node);

                    count++;
                    if (count == map.Keys.Count)
                        return ++i;
                }

            return -1;
        }

        private void AddNodeToEnd(ref ListNode end, ref ListNode node)
        {
            end.next = node;
            node.pre = end;
            end = end.next;
        }

        private Dictionary<char, ListNode> CreateDictionary(string t)
        {
            Dictionary<char, ListNode> result = new Dictionary<char, ListNode>();

            for (int i = 0; i <= t.Length - 1; i++)
                if (!result.ContainsKey(t[i]))
                    result.Add(t[i], new ListNode(-1));

            return result;
        }

        public class ListNode
        {
            public int val;
            public ListNode pre;
            public ListNode next;
            public ListNode(int val)
            {
                this.val = val;
                this.pre = null;
                this.next = null;
            }

            public ListNode(List<int> vals)
            {
                this.val = vals[0];
                this.pre = null;
                this.next = null;

                ListNode node = this;
                for (int i = 1; i <= vals.Count - 1; i++)
                {
                    node.next = new ListNode(vals[i]);
                    node.next.pre = node;
                    node = node.next;
                }
            }

            public string GetThisAndAfterInString()
            {
                StringBuilder sb = new StringBuilder();
                ListNode node = this;
                while (node != null)
                {
                    sb.Append(node.val);
                    sb.Append(" ");
                    node = node.next;
                }
                return sb.ToString();
            }

            public string GetThisAndBeforeInString()
            {
                StringBuilder sb = new StringBuilder();
                ListNode node = this;
                while (node != null)
                {
                    sb.Append(node.val);
                    sb.Append(" ");
                    node = node.pre;
                }
                return sb.ToString();
            }
        }
        #endregion

        #region "297 Serialize and Deserialize Binary Tree"
        public class Codec
        {
            public string serialize(TreeNode root)
            {
                if (root == null) return "";

                StringBuilder result = new StringBuilder();
                ToSb(result, root);
                result.Remove(result.Length - 1, 1);
                return result.ToString();
            }

            private void ToSb(StringBuilder result, TreeNode node)
            {
                if (node == null)
                {
                    result.Append("x ");
                    return;
                }
                result.Append(node.val);
                result.Append(" ");
                ToSb(result, node.left);
                ToSb(result, node.right);
            }

            // Decodes your encoded data to tree.
            public TreeNode deserialize(string data)
            {
                if (data.Length == 0) return null;

                List<string> nodes = data.Split(new char[] { ' ' }).ToList();
                return ToTreeNode(nodes);
            }

            private TreeNode ToTreeNode(List<string> nodes)
            {
                if (nodes.Count == 0)
                    return null;

                string val = nodes[0];
                nodes.RemoveAt(0);
                if (val == "x")
                    return null;

                TreeNode node = new TreeNode(Int32.Parse(val));
                node.left = ToTreeNode(nodes);
                node.right = ToTreeNode(nodes);
                return node;
            }
        }

        public class BfsCodec
        {
            // Encodes a tree to a single string.
            public string serialize(TreeNode root)
            {
                if (root == null) return "";

                StringBuilder result = new StringBuilder();
                Queue<TreeNode> q = new Queue<TreeNode>();
                q.Enqueue(root);

                while (q.Count != 0)
                {
                    Queue<TreeNode> row = new Queue<TreeNode>();
                    while (q.Count != 0)
                    {
                        TreeNode node = q.Dequeue();

                        if (node == null)
                            result.Append("x");
                        else
                        {
                            row.Enqueue(node.left);
                            row.Enqueue(node.right);
                            result.Append(node.val);
                        }

                        result.Append(" ");
                    }
                    q = row;
                }

                if (result.Length == 0)
                    return "";

                result.Remove(result.Length - 1, 1);
                return result.ToString();
            }

            // Decodes your encoded data to tree.
            public TreeNode deserialize(string data)
            {
                if (data.Length == 0) return null;

                string[] nodes = data.Split(new char[] { ' ' });
                TreeNode root = new TreeNode(Int32.Parse(nodes[0]));
                Queue<TreeNode> q = new Queue<TreeNode>();
                q.Enqueue(root);

                for (int i = 1; i <= nodes.Length - 1; i += 2)
                {
                    TreeNode node = q.Dequeue();
                    TreeNode left = null, right = null;
                    if (nodes[i] != "x")
                    {
                        left = new TreeNode(Int32.Parse(nodes[i]));
                        node.left = left;
                        q.Enqueue(left);
                    }

                    if (nodes[i + 1] != "x")
                    {
                        right = new TreeNode(Int32.Parse(nodes[i + 1]));
                        node.right = right;
                        q.Enqueue(right);
                    }
                }

                return root;
            }
        }
        #endregion

        #region "25 Reverse Nodes in k-Group"
        private ListNode _25ReverseNodesInKGroup(ListNode head, int k)
        {
            if (k == 0 || head == null) return head;

            ListNode dummy = new ListNode(-1);
            dummy.next = head;
            ListNode revertStartsNext = dummy;

            int total = 0, count = 0;
            while (head != null)
            {
                total++;
                head = head.next;
            }

            head = dummy.next;
            while (count + k <= total)
            {
                int reverted = 0;
                while (++reverted < k)
                    AddNodeAfter(revertStartsNext, RemoveNodeAfter(head));
                revertStartsNext = head;
                head = head.next;
                count += k;
            }

            return dummy.next;
        }

        private void AddNodeAfter(ListNode pre, ListNode node)
        {
            node.next = pre.next;
            pre.next = node;
        }

        private ListNode RemoveNodeAfter(ListNode node)
        {
            ListNode result = node.next;
            node.next = node.next.next;
            return result;
        }
        #endregion

        #region "269 Alien Dictionary"
        private string _269AlienDictionary(string[] words)
        {
            if (words.Length == 0) return "";

            Stack<char> s = new Stack<char>();
            int[] count = new int[26];
            foreach (string word in words)
                foreach (char c in word.ToCharArray())
                    count[c - 'a'] = 1;

            Dictionary<char, AlienListNode> map = new Dictionary<char, AlienListNode>();
            for (int i = 0; i < words.Length - 1; i++)
            {
                for (int j = 0; j < (int)Math.Min(words[i].Length, words[i + 1].Length); j++)
                    if (words[i][j] != words[i + 1][j])
                    {
                        if (!map.ContainsKey(words[i][j]))
                            map.Add(words[i][j], new AlienListNode(words[i][j]));
                        if (!map.ContainsKey(words[i + 1][j]))
                            map.Add(words[i + 1][j], new AlienListNode(words[i + 1][j]));
                        AlienListNode node = map[words[i][j]];
                        if (!node.children.ContainsKey(words[i + 1][j]))
                            node.children.Add(words[i + 1][j], map[words[i + 1][j]]);
                        break;
                    }
            }

            foreach (char c in map.Keys)
                if (!AlienListToWord(count, c, map, s))
                    return "";

            StringBuilder result = new StringBuilder();
            while (s.Count != 0)
                result.Append(s.Pop());
            for (int i = 0; i < 26; i++)
                if (count[i] == 1)
                    result.Append((char)('a' + i));

            return result.ToString();
        }

        private bool AlienListToWord(int[] count, char c, Dictionary<char, AlienListNode> map, Stack<char> s)
        {
            if (count[c - 'a'] == 2) return false;
            if (count[c - 'a'] == 3) return true;
            count[c - 'a'] = 2;

            foreach (char child in map[c].children.Keys)
                if (!AlienListToWord(count, child, map, s))
                    return false;

            count[c - 'a'] = 3;
            s.Push(c);
            return true;
        }

        private string _269AlienDictionary_1(string[] words)
        {
            if (words.Length == 0) return "";
            AlienTrieNode trie = new AlienTrieNode(words);

            Dictionary<char, AlienDoubleListNode> map = new Dictionary<char, AlienDoubleListNode>();
            AlienDoubleListNode dummy = new AlienDoubleListNode();

            Queue<AlienTrieNode> q = new Queue<AlienTrieNode>();
            q.Enqueue(trie);
            while (q.Count != 0)
            {
                Queue<AlienTrieNode> newQ = new Queue<AlienTrieNode>();

                while (q.Count != 0)
                {
                    AlienTrieNode node = q.Dequeue();

                    if (node.next.Count == 0) continue;
                    if (node.next.Count == 1)
                    {
                        newQ.Enqueue(node.next[0]);
                        continue;
                    }

                    AlienDoubleListNode listNode = null;
                    for (int i = 0; i <= node.next.Count - 1; i++)
                    {
                        newQ.Enqueue(node.next[i]);

                        char c = node.next[i].label;
                        if (!map.ContainsKey(node.next[i].label))
                            map.Add(c, new AlienDoubleListNode(c));
                        if (listNode == null)
                            listNode = map[c];
                        else
                        {
                            listNode.next = map[c];
                            listNode = listNode.next;
                        }
                    }
                }

                q = newQ;
            }

            return "";
        }

        public class AlienListNode
        {
            public char c;
            public Dictionary<char, AlienListNode> children;
            public AlienListNode()
            {
                this.c = '\0';
                this.children = new Dictionary<char, AlienListNode>();
            }
            public AlienListNode(char c)
            {
                this.c = c;
                this.children = new Dictionary<char, AlienListNode>();
            }
        }

        public class AlienDoubleListNode
        {
            public char label;
            public AlienDoubleListNode pre;
            public AlienDoubleListNode next;

            public AlienDoubleListNode()
            {
                this.label = '\0';
                this.pre = null;
                this.next = null;
            }

            public AlienDoubleListNode(char label)
            {
                this.label = label;
                this.pre = null;
                this.next = null;
            }
        }

        public class AlienTrieNode
        {
            public char label;
            public bool end;
            public int[] map;
            public List<AlienTrieNode> next;

            public AlienTrieNode()
            {
                this.label = '\0';
                this.end = false;
                this.map = new int[128];
                this.next = new List<AlienTrieNode>();
            }

            public AlienTrieNode(char label)
            {
                this.label = label;
                this.end = false;
                this.map = new int[128];
                this.next = new List<AlienTrieNode>();
            }

            public AlienTrieNode(string[] words)
            {
                if (words.Length != 0)
                {
                    this.label = '\0';
                    this.end = false;
                    this.map = new int[128];
                    this.next = new List<AlienTrieNode>();

                    AlienTrieNode trie = this;
                    for (int i = 0; i <= words.Length - 1; i++)
                    {
                        AlienTrieNode node = trie;
                        for (int j = 0; j <= words[i].Length - 1; j++)
                        {
                            if (node.map[words[i][j]] != 0)
                                node = node.next[node.map[words[i][j]] - 1];
                            else
                            {
                                AlienTrieNode newNode = new AlienTrieNode(words[i][j]);
                                node.next.Add(newNode);
                                node.map[words[i][j]] = node.next.Count;
                                node = newNode;
                            }
                        }
                        if (node.label != '\0')
                            node.end = true;
                    }
                }
            }
        }
        #endregion

        [Test]
        public void TestHard()
        {
            #region "269 Alien Dictionary"
            //Assert.That(this._269AlienDictionary(new string[] { "zy", "zx" }), Is.EqualTo("yxz"));
            //Assert.That(this._269AlienDictionary(new string[] { "z", "z" }), Is.EqualTo("z"));
            //Assert.That(this._269AlienDictionary(new string[] { "z", "x" }), Is.EqualTo("zx"));
            //Assert.That(this._269AlienDictionary(new string[] { "z", "x", "z" }), Is.EqualTo(""));
            //Assert.That(this._269AlienDictionary(new string[] { "wrt", "wrf", "er", "ett", "rftt" }), Is.EqualTo("wertf"));
            #endregion

            #region "25 Reverse Nodes in k-Group"
            //Assert.That(this._25ReverseNodesInKGroup(new ListNode(new List<int>() { 1, 2 }), 2).GetThisAndAfterInString(), Is.EqualTo("2 1 "));
            //Assert.That(this._25ReverseNodesInKGroup(new ListNode(new List<int>() { 1, 2, 3, 4, 5 }), 3).GetThisAndAfterInString(), Is.EqualTo("3 2 1 4 5 "));
            //Assert.That(this._25ReverseNodesInKGroup(new ListNode(new List<int>() { 1, 2, 3, 4, 5 }), 2).GetThisAndAfterInString(), Is.EqualTo("2 1 4 3 5 "));

            //ListNode head = new ListNode(new List<int>() { 1, 2, 3, 4, 5 });
            //ListNode tail = head;
            //while (tail.next != null)
            //    tail = tail.next;

            //Assert.That(head.GetThisAndAfterInString(), Is.EqualTo("1 2 3 4 5 "));
            //Assert.That(tail.GetThisAndBeforeInString(), Is.EqualTo("5 4 3 2 1 "));
            #endregion

            #region "297 Serialize and Deserialize Binary Tree"
            //Codec codec = new Codec();

            //Assert.Null(codec.deserialize(codec.serialize(null)));
            //Assert.That(codec.deserialize(codec.serialize(new TreeNode(new List<int>() { 1, 2, 3, -1, -1, 4, 5 }))).GetNodeBfs(), Is.EquivalentTo(new List<int>() { 1, 2, 3, 4, 5 }));
            #endregion

            #region "76 Minimum Window Substring"
            //Assert.That(this._76MinimumWindowSubstring("a", "aa"), Is.EqualTo(""));
            //Assert.That(this._76MinimumWindowSubstring("ADOBECODEBANC", "ABC"), Is.EqualTo("BANC"));
            //Assert.That(this._76MinimumWindowSubstring("aa", "aa"), Is.EqualTo("aa"));

            // 1st atempt
            //string s = "ADOBECODEBANC";
            //string t = "ABC";
            //Dictionary<char, ListNode> map = CreateDictionary(t);
            //Assert.That(map.Keys.ToList(), Is.EquivalentTo(new List<char>() { 'A', 'B', 'C' }));
            //foreach (KeyValuePair<char, ListNode> pair in map)
            //    Assert.That(pair.Value.val, Is.EqualTo(-1));

            //ListNode dummy = new ListNode(-1);
            //ListNode end = dummy;
            //int start = GetInitialPosition(s, map, ref end);
            //Assert.That(start, Is.EqualTo(6));
            //Assert.That(map['A'].val, Is.EqualTo(0));
            //Assert.That(map['B'].val, Is.EqualTo(3));
            //Assert.That(map['C'].val, Is.EqualTo(5));
            //Assert.That(dummy.GetThisAndAfterInString(), Is.EqualTo("-1 0 3 5 "));
            //Assert.That(end.GetThisAndBeforeInString(), Is.EqualTo("5 3 0 -1 "));

            //for (; start <= s.Length - 1; start++)
            //    if (map.ContainsKey(s[start]))
            //        UpdateMap(map, s[start], start, ref end);
            //Assert.That(dummy.GetThisAndAfterInString(), Is.EqualTo("-1 9 10 12 "));
            //Assert.That(end.GetThisAndBeforeInString(), Is.EqualTo("12 10 9 -1 "));

            //Assert.That(this._76MinimumWindowSubstring(s, t), Is.EqualTo("BANC"));
            #endregion

            #region "301 Remove Invalid Parentheses (didn't finish)"
            //Assert.True(this.IsValid(""));
            //Assert.True(this.IsValid("()"));
            //Assert.False(this.IsValid(")()"));
            //Assert.False(this.IsValid("(()"));
            //Assert.False(this.IsValid("())"));
            //Assert.False(this.IsValid(")("));
            //Assert.True(this.IsValid("((a))"));
            //Assert.True(this.IsValid("((a)(bd))"));

            //Assert.That(this.CalculateInvalid(")())("), Is.EqualTo(new Tuple<int, int>(1, 2)));
            //Assert.That(this.CalculateInvalid(")("), Is.EqualTo(new Tuple<int, int>(1, 1)));
            //Assert.That(this.CalculateInvalid("()())()"), Is.EqualTo(new Tuple<int, int>(0, 1)));
            //Assert.That(this.CalculateInvalid("(a)())()"), Is.EqualTo(new Tuple<int, int>(0, 1)));

            //Assert.That(this._301RemoveInvalidParentheses(")("), Is.EqualTo(new List<string>() { "" }));
            //Assert.That(this._301RemoveInvalidParentheses("()())()"), Is.EqualTo(new List<string>() { "()()()" }));
            //Assert.That(this._301RemoveInvalidParentheses("(a)())()"), Is.EqualTo(new List<string>() { "(a)()()" }));
            #endregion

            #region "23 Merge k Sorted Lists"
            //Assert.That(this._23MergeKSortedLists(new ListNode[] { null, new ListNode(1) }).GetThisAndAfterInString(), Is.EqualTo("1 "));
            //Assert.Null(this._23MergeKSortedLists(new ListNode[] { null, null }));
            //Assert.That(this._23MergeKSortedLists(new ListNode[]
            //{
            //    new ListNode(new List<int>() { 1, 4, 5 }),
            //    new ListNode(new List<int>() { 1, 3, 4 }),
            //    new ListNode(new List<int>() { 2, 6 })
            //}).GetThisAndAfterInString(), Is.EqualTo("1 1 2 3 4 4 5 6 "));
            #endregion

            #region "273 Integer to English Words"
            //Assert.That(this._273IntegerToEnglishWords(1000010), Is.EqualTo("One Million Ten"));
            //Assert.That(this._273IntegerToEnglishWords(1000), Is.EqualTo("One Thousand"));
            //Assert.That(this._273IntegerToEnglishWords(100), Is.EqualTo("One Hundred"));
            //Assert.That(this._273IntegerToEnglishWords(1001001001), Is.EqualTo("One Billion One Million One Thousand One"));

            //Assert.That(this._273IntegerToEnglishWords(1234567891), Is.EqualTo("One Billion Two Hundred Thirty Four Million Five Hundred Sixty Seven Thousand Eight Hundred Ninety One"));
            //Assert.That(this._273IntegerToEnglishWords(1234567), Is.EqualTo("One Million Two Hundred Thirty Four Thousand Five Hundred Sixty Seven"));
            //Assert.That(this._273IntegerToEnglishWords(12345), Is.EqualTo("Twelve Thousand Three Hundred Forty Five"));
            //Assert.That(this._273IntegerToEnglishWords(123), Is.EqualTo("One Hundred Twenty Three"));
            #endregion

            #region "146 LRU Cache"
            //LRUCache cache = new LRUCache(2);

            //cache.Put(1, 1);
            //cache.Put(2, 2);
            //Assert.That(cache.Get(1), Is.EqualTo(1));       // returns 1
            //cache.Put(3, 3);    // evicts key 2
            //Assert.That(cache.Get(2), Is.EqualTo(-1));       // returns -1 (not found)
            //cache.Put(4, 4);    // evicts key 1
            //Assert.That(cache.Get(1), Is.EqualTo(-1));       // returns -1 (not found)
            //Assert.That(cache.Get(3), Is.EqualTo(3));       // returns 3
            //Assert.That(cache.Get(4), Is.EqualTo(4));       // returns 4
            #endregion
        }
    }
}
