using Fundamentals.TestDataStructures;
using log4net;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundamentals.TestOnlineJudges.TestLeetCode
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

    public class TrieNode
    {
        public TrieNode[] next;
        public string word;

        public TrieNode()
        {
            this.next = new TrieNode[26];
            this.word = "";
        }

        public void BuildTrie(string[] words)
        {
            foreach (string word in words)
            {
                TrieNode node = this;
                foreach (char c in word)
                {
                    int index = c - 'a';
                    if (node.next[index] == null)
                    {
                        node.next[index] = new TrieNode();
                    }
                    node = node.next[index];
                }
                node.word = word;
            }
        }
    }

    [TestFixture]
    public class TestLeetCodeHard
    {
        private static ILog logger = LogManager.GetLogger(typeof(TestLeetCodeHard));

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

        #region "212 Word Search 2"
        private List<string> _212WordSearch2(char[,] board, string[] words)
        {
            TrieNode trie = new TrieNode();
            trie.BuildTrie(words);

            int row = board.GetUpperBound(0);
            int col = board.GetUpperBound(1);

            List<string> result = new List<string>();
            for (int i = 0; i <= row; ++i)
            {
                for (int j = 0; j <= col; ++j)
                {
                    FindWords(board, row, col, i, j, trie, result);
                }
            }

            return result;
        }

        private void FindWords(char[,] board, int row, int col, int i, int j, TrieNode node, List<string> result)
        {
            if (i < 0 || j < 0 || i > row || j > col)
            {
                return;
            }
            if (board[i, j] == '#')
            {
                return;
            }

            char c = board[i, j];
            TrieNode next = node.next[c - 'a'];
            if (next == null)
            {
                return;
            }

            if (!String.IsNullOrEmpty(next.word))
            {
                result.Add(next.word);
                next.word = "";
            }

            board[i, j] = '#';
            FindWords(board, row, col, i, j - 1, next, result);
            FindWords(board, row, col, i - 1, j, next, result);
            FindWords(board, row, col, i, j + 1, next, result);
            FindWords(board, row, col, i + 1, j, next, result);
            board[i, j] = c;
        }

        private List<string> _212MyWordSearch2(char[,] board, string[] words)
        {
            Dictionary<char, HashSet<string>> initials = new Dictionary<char, HashSet<string>>();
            foreach (string word in words)
            {
                if (!initials.ContainsKey(word[0]))
                {
                    initials.Add(word[0], new HashSet<string>());
                }
                if (!initials[word[0]].Contains(word))
                {
                    initials[word[0]].Add(word);
                }
            }

            List<string> result = new List<string>();
            int row = board.GetUpperBound(0);
            int col = board.GetUpperBound(1);

            for (int i = 0; i <= row; ++i)
            {
                for (int j = 0; j <= col; ++j)
                {
                    if (initials.ContainsKey(board[i, j]))
                    {
                        HashSet<string> candidates = initials[board[i, j]];
                        result.AddRange(WalkBoard(board, row, col, i, j, ref candidates, 0));
                        initials[board[i, j]] = candidates;
                    }
                }

            }
            return result;
        }

        private List<string> WalkBoard(char[,] board, int row, int col, int i, int j, ref HashSet<string> candidates, int index)
        {
            List<string> result = new List<string>();
            HashSet<string> newset = new HashSet<string>();
            foreach (string word in candidates)
            {
                if (index == word.Length)
                {
                    result.Add(word);
                }
                else
                {
                    newset.Add(word);
                }
            }
            candidates = newset;
            if (candidates.Count == 0)
            {
                return result;
            }

            if (i < 0 || j < 0 || i > row || j > col)
            {
                return result;
            }

            HashSet<string> forward = new HashSet<string>();
            newset = new HashSet<string>();
            foreach (string word in candidates)
            {
                if (word[index] == board[i, j])
                {
                    forward.Add(word);
                }
                else
                {
                    newset.Add(word);
                }
            }
            candidates = newset;
            if (forward.Count == 0)
            {
                return result;
            }

            char c = board[i, j];
            board[i, j] = '#';

            result.AddRange(WalkBoard(board, row, col, i, j - 1, ref forward, index + 1));
            result.AddRange(WalkBoard(board, row, col, i - 1, j, ref forward, index + 1));
            result.AddRange(WalkBoard(board, row, col, i, j + 1, ref forward, index + 1));
            result.AddRange(WalkBoard(board, row, col, i + 1, j, ref forward, index + 1));

            board[i, j] = c;
            candidates.UnionWith(forward);

            return result;
        }
        #endregion

        [Test]
        public void TestHard()
        {
            #region "212 Word Search 2"
            Assert.That(this._212WordSearch2(new char[,] {
                { 'a','a','a','a' },
                { 'a','a','a','a' },
                { 'a','a','a','a' },
                { 'a','a','a','a' },
                { 'b','c','d','e' },
                { 'f','g','h','i' },
                { 'j','k','l','m' },
                { 'n','o','p','q' },
                { 'r','s','t','u' },
                { 'v','w','x','y' },
                { 'z','z','z','z' },
            }, new string[] { "aaaaaaaaaaaaaaaa", "aaaaaaaaaaaaaaab", "aaaaaaaaaaaaaaac", "aaaaaaaaaaaaaaad", "aaaaaaaaaaaaaaae", "aaaaaaaaaaaaaaaf", "aaaaaaaaaaaaaaag", "aaaaaaaaaaaaaaah", "aaaaaaaaaaaaaaai", "aaaaaaaaaaaaaaaj", "aaaaaaaaaaaaaaak", "aaaaaaaaaaaaaaal", "aaaaaaaaaaaaaaam", "aaaaaaaaaaaaaaan", "aaaaaaaaaaaaaaao", "aaaaaaaaaaaaaaap", "aaaaaaaaaaaaaaaq", "aaaaaaaaaaaaaaar", "aaaaaaaaaaaaaaas", "aaaaaaaaaaaaaaat", "aaaaaaaaaaaaaaau", "aaaaaaaaaaaaaaav", "aaaaaaaaaaaaaaaw", "aaaaaaaaaaaaaaax", "aaaaaaaaaaaaaaay", "aaaaaaaaaaaaaaaz", "aaaaaaaaaaaaaaaa", "aaaaaaaaaaaaaaab", "aaaaaaaaaaaaaaac", "aaaaaaaaaaaaaaad", "aaaaaaaaaaaaaaae", "aaaaaaaaaaaaaaaf", "aaaaaaaaaaaaaaag", "aaaaaaaaaaaaaaah", "aaaaaaaaaaaaaaai", "aaaaaaaaaaaaaaaj", "aaaaaaaaaaaaaaak", "aaaaaaaaaaaaaaal", "aaaaaaaaaaaaaaam", "aaaaaaaaaaaaaaan", "aaaaaaaaaaaaaaao", "aaaaaaaaaaaaaaap", "aaaaaaaaaaaaaaaq", "aaaaaaaaaaaaaaar", "aaaaaaaaaaaaaaas", "aaaaaaaaaaaaaaat", "aaaaaaaaaaaaaaau", "aaaaaaaaaaaaaaav", "aaaaaaaaaaaaaaaw", "aaaaaaaaaaaaaaax", "aaaaaaaaaaaaaaay", "aaaaaaaaaaaaaaaz", "aaaaaaaaaaaaaaba", "aaaaaaaaaaaaaabb", "aaaaaaaaaaaaaabc", "aaaaaaaaaaaaaabd", "aaaaaaaaaaaaaabe", "aaaaaaaaaaaaaabf", "aaaaaaaaaaaaaabg", "aaaaaaaaaaaaaabh", "aaaaaaaaaaaaaabi", "aaaaaaaaaaaaaabj", "aaaaaaaaaaaaaabk", "aaaaaaaaaaaaaabl", "aaaaaaaaaaaaaabm", "aaaaaaaaaaaaaabn", "aaaaaaaaaaaaaabo", "aaaaaaaaaaaaaabp", "aaaaaaaaaaaaaabq", "aaaaaaaaaaaaaabr", "aaaaaaaaaaaaaabs", "aaaaaaaaaaaaaabt", "aaaaaaaaaaaaaabu", "aaaaaaaaaaaaaabv", "aaaaaaaaaaaaaabw", "aaaaaaaaaaaaaabx", "aaaaaaaaaaaaaaby", "aaaaaaaaaaaaaabz", "aaaaaaaaaaaaaaca", "aaaaaaaaaaaaaacb", "aaaaaaaaaaaaaacc", "aaaaaaaaaaaaaacd", "aaaaaaaaaaaaaace", "aaaaaaaaaaaaaacf", "aaaaaaaaaaaaaacg", "aaaaaaaaaaaaaach", "aaaaaaaaaaaaaaci", "aaaaaaaaaaaaaacj", "aaaaaaaaaaaaaack", "aaaaaaaaaaaaaacl", "aaaaaaaaaaaaaacm", "aaaaaaaaaaaaaacn", "aaaaaaaaaaaaaaco", "aaaaaaaaaaaaaacp", "aaaaaaaaaaaaaacq", "aaaaaaaaaaaaaacr", "aaaaaaaaaaaaaacs", "aaaaaaaaaaaaaact", "aaaaaaaaaaaaaacu", "aaaaaaaaaaaaaacv", "aaaaaaaaaaaaaacw", "aaaaaaaaaaaaaacx", "aaaaaaaaaaaaaacy", "aaaaaaaaaaaaaacz", "aaaaaaaaaaaaaada", "aaaaaaaaaaaaaadb", "aaaaaaaaaaaaaadc", "aaaaaaaaaaaaaadd", "aaaaaaaaaaaaaade", "aaaaaaaaaaaaaadf", "aaaaaaaaaaaaaadg", "aaaaaaaaaaaaaadh", "aaaaaaaaaaaaaadi", "aaaaaaaaaaaaaadj", "aaaaaaaaaaaaaadk", "aaaaaaaaaaaaaadl", "aaaaaaaaaaaaaadm", "aaaaaaaaaaaaaadn", "aaaaaaaaaaaaaado", "aaaaaaaaaaaaaadp", "aaaaaaaaaaaaaadq", "aaaaaaaaaaaaaadr", "aaaaaaaaaaaaaads", "aaaaaaaaaaaaaadt", "aaaaaaaaaaaaaadu", "aaaaaaaaaaaaaadv", "aaaaaaaaaaaaaadw", "aaaaaaaaaaaaaadx", "aaaaaaaaaaaaaady", "aaaaaaaaaaaaaadz", "aaaaaaaaaaaaaaea", "aaaaaaaaaaaaaaeb", "aaaaaaaaaaaaaaec", "aaaaaaaaaaaaaaed", "aaaaaaaaaaaaaaee", "aaaaaaaaaaaaaaef", "aaaaaaaaaaaaaaeg", "aaaaaaaaaaaaaaeh", "aaaaaaaaaaaaaaei", "aaaaaaaaaaaaaaej", "aaaaaaaaaaaaaaek", "aaaaaaaaaaaaaael", "aaaaaaaaaaaaaaem", "aaaaaaaaaaaaaaen", "aaaaaaaaaaaaaaeo", "aaaaaaaaaaaaaaep", "aaaaaaaaaaaaaaeq", "aaaaaaaaaaaaaaer", "aaaaaaaaaaaaaaes", "aaaaaaaaaaaaaaet", "aaaaaaaaaaaaaaeu", "aaaaaaaaaaaaaaev", "aaaaaaaaaaaaaaew", "aaaaaaaaaaaaaaex", "aaaaaaaaaaaaaaey", "aaaaaaaaaaaaaaez", "aaaaaaaaaaaaaafa", "aaaaaaaaaaaaaafb", "aaaaaaaaaaaaaafc", "aaaaaaaaaaaaaafd", "aaaaaaaaaaaaaafe", "aaaaaaaaaaaaaaff", "aaaaaaaaaaaaaafg", "aaaaaaaaaaaaaafh", "aaaaaaaaaaaaaafi", "aaaaaaaaaaaaaafj", "aaaaaaaaaaaaaafk", "aaaaaaaaaaaaaafl", "aaaaaaaaaaaaaafm", "aaaaaaaaaaaaaafn", "aaaaaaaaaaaaaafo", "aaaaaaaaaaaaaafp", "aaaaaaaaaaaaaafq", "aaaaaaaaaaaaaafr", "aaaaaaaaaaaaaafs", "aaaaaaaaaaaaaaft", "aaaaaaaaaaaaaafu", "aaaaaaaaaaaaaafv", "aaaaaaaaaaaaaafw", "aaaaaaaaaaaaaafx", "aaaaaaaaaaaaaafy", "aaaaaaaaaaaaaafz", "aaaaaaaaaaaaaaga", "aaaaaaaaaaaaaagb", "aaaaaaaaaaaaaagc", "aaaaaaaaaaaaaagd", "aaaaaaaaaaaaaage", "aaaaaaaaaaaaaagf", "aaaaaaaaaaaaaagg", "aaaaaaaaaaaaaagh", "aaaaaaaaaaaaaagi", "aaaaaaaaaaaaaagj", "aaaaaaaaaaaaaagk", "aaaaaaaaaaaaaagl", "aaaaaaaaaaaaaagm", "aaaaaaaaaaaaaagn", "aaaaaaaaaaaaaago", "aaaaaaaaaaaaaagp", "aaaaaaaaaaaaaagq", "aaaaaaaaaaaaaagr", "aaaaaaaaaaaaaags", "aaaaaaaaaaaaaagt", "aaaaaaaaaaaaaagu", "aaaaaaaaaaaaaagv", "aaaaaaaaaaaaaagw", "aaaaaaaaaaaaaagx", "aaaaaaaaaaaaaagy", "aaaaaaaaaaaaaagz", "aaaaaaaaaaaaaaha", "aaaaaaaaaaaaaahb", "aaaaaaaaaaaaaahc", "aaaaaaaaaaaaaahd", "aaaaaaaaaaaaaahe", "aaaaaaaaaaaaaahf", "aaaaaaaaaaaaaahg", "aaaaaaaaaaaaaahh", "aaaaaaaaaaaaaahi", "aaaaaaaaaaaaaahj", "aaaaaaaaaaaaaahk", "aaaaaaaaaaaaaahl", "aaaaaaaaaaaaaahm", "aaaaaaaaaaaaaahn", "aaaaaaaaaaaaaaho", "aaaaaaaaaaaaaahp", "aaaaaaaaaaaaaahq", "aaaaaaaaaaaaaahr", "aaaaaaaaaaaaaahs", "aaaaaaaaaaaaaaht", "aaaaaaaaaaaaaahu", "aaaaaaaaaaaaaahv", "aaaaaaaaaaaaaahw", "aaaaaaaaaaaaaahx", "aaaaaaaaaaaaaahy", "aaaaaaaaaaaaaahz", "aaaaaaaaaaaaaaia", "aaaaaaaaaaaaaaib", "aaaaaaaaaaaaaaic", "aaaaaaaaaaaaaaid", "aaaaaaaaaaaaaaie", "aaaaaaaaaaaaaaif", "aaaaaaaaaaaaaaig", "aaaaaaaaaaaaaaih", "aaaaaaaaaaaaaaii", "aaaaaaaaaaaaaaij", "aaaaaaaaaaaaaaik", "aaaaaaaaaaaaaail", "aaaaaaaaaaaaaaim", "aaaaaaaaaaaaaain", "aaaaaaaaaaaaaaio", "aaaaaaaaaaaaaaip", "aaaaaaaaaaaaaaiq", "aaaaaaaaaaaaaair", "aaaaaaaaaaaaaais", "aaaaaaaaaaaaaait", "aaaaaaaaaaaaaaiu", "aaaaaaaaaaaaaaiv", "aaaaaaaaaaaaaaiw", "aaaaaaaaaaaaaaix", "aaaaaaaaaaaaaaiy", "aaaaaaaaaaaaaaiz", "aaaaaaaaaaaaaaja", "aaaaaaaaaaaaaajb", "aaaaaaaaaaaaaajc", "aaaaaaaaaaaaaajd", "aaaaaaaaaaaaaaje", "aaaaaaaaaaaaaajf", "aaaaaaaaaaaaaajg", "aaaaaaaaaaaaaajh", "aaaaaaaaaaaaaaji", "aaaaaaaaaaaaaajj", "aaaaaaaaaaaaaajk", "aaaaaaaaaaaaaajl", "aaaaaaaaaaaaaajm", "aaaaaaaaaaaaaajn", "aaaaaaaaaaaaaajo", "aaaaaaaaaaaaaajp", "aaaaaaaaaaaaaajq", "aaaaaaaaaaaaaajr", "aaaaaaaaaaaaaajs", "aaaaaaaaaaaaaajt", "aaaaaaaaaaaaaaju", "aaaaaaaaaaaaaajv", "aaaaaaaaaaaaaajw", "aaaaaaaaaaaaaajx", "aaaaaaaaaaaaaajy", "aaaaaaaaaaaaaajz", "aaaaaaaaaaaaaaka", "aaaaaaaaaaaaaakb", "aaaaaaaaaaaaaakc", "aaaaaaaaaaaaaakd", "aaaaaaaaaaaaaake", "aaaaaaaaaaaaaakf", "aaaaaaaaaaaaaakg", "aaaaaaaaaaaaaakh", "aaaaaaaaaaaaaaki", "aaaaaaaaaaaaaakj", "aaaaaaaaaaaaaakk", "aaaaaaaaaaaaaakl", "aaaaaaaaaaaaaakm", "aaaaaaaaaaaaaakn", "aaaaaaaaaaaaaako", "aaaaaaaaaaaaaakp", "aaaaaaaaaaaaaakq", "aaaaaaaaaaaaaakr", "aaaaaaaaaaaaaaks", "aaaaaaaaaaaaaakt", "aaaaaaaaaaaaaaku", "aaaaaaaaaaaaaakv", "aaaaaaaaaaaaaakw", "aaaaaaaaaaaaaakx", "aaaaaaaaaaaaaaky", "aaaaaaaaaaaaaakz", "aaaaaaaaaaaaaala", "aaaaaaaaaaaaaalb", "aaaaaaaaaaaaaalc", "aaaaaaaaaaaaaald", "aaaaaaaaaaaaaale", "aaaaaaaaaaaaaalf", "aaaaaaaaaaaaaalg", "aaaaaaaaaaaaaalh", "aaaaaaaaaaaaaali", "aaaaaaaaaaaaaalj", "aaaaaaaaaaaaaalk", "aaaaaaaaaaaaaall", "aaaaaaaaaaaaaalm", "aaaaaaaaaaaaaaln", "aaaaaaaaaaaaaalo", "aaaaaaaaaaaaaalp", "aaaaaaaaaaaaaalq", "aaaaaaaaaaaaaalr", "aaaaaaaaaaaaaals", "aaaaaaaaaaaaaalt", "aaaaaaaaaaaaaalu", "aaaaaaaaaaaaaalv", "aaaaaaaaaaaaaalw", "aaaaaaaaaaaaaalx", "aaaaaaaaaaaaaaly", "aaaaaaaaaaaaaalz", "aaaaaaaaaaaaaama", "aaaaaaaaaaaaaamb", "aaaaaaaaaaaaaamc", "aaaaaaaaaaaaaamd", "aaaaaaaaaaaaaame", "aaaaaaaaaaaaaamf", "aaaaaaaaaaaaaamg", "aaaaaaaaaaaaaamh", "aaaaaaaaaaaaaami", "aaaaaaaaaaaaaamj", "aaaaaaaaaaaaaamk", "aaaaaaaaaaaaaaml", "aaaaaaaaaaaaaamm", "aaaaaaaaaaaaaamn", "aaaaaaaaaaaaaamo", "aaaaaaaaaaaaaamp", "aaaaaaaaaaaaaamq", "aaaaaaaaaaaaaamr", "aaaaaaaaaaaaaams", "aaaaaaaaaaaaaamt", "aaaaaaaaaaaaaamu", "aaaaaaaaaaaaaamv", "aaaaaaaaaaaaaamw", "aaaaaaaaaaaaaamx", "aaaaaaaaaaaaaamy", "aaaaaaaaaaaaaamz", "aaaaaaaaaaaaaana", "aaaaaaaaaaaaaanb", "aaaaaaaaaaaaaanc", "aaaaaaaaaaaaaand", "aaaaaaaaaaaaaane", "aaaaaaaaaaaaaanf", "aaaaaaaaaaaaaang", "aaaaaaaaaaaaaanh", "aaaaaaaaaaaaaani", "aaaaaaaaaaaaaanj", "aaaaaaaaaaaaaank", "aaaaaaaaaaaaaanl", "aaaaaaaaaaaaaanm", "aaaaaaaaaaaaaann", "aaaaaaaaaaaaaano", "aaaaaaaaaaaaaanp", "aaaaaaaaaaaaaanq", "aaaaaaaaaaaaaanr", "aaaaaaaaaaaaaans", "aaaaaaaaaaaaaant", "aaaaaaaaaaaaaanu", "aaaaaaaaaaaaaanv", "aaaaaaaaaaaaaanw", "aaaaaaaaaaaaaanx", "aaaaaaaaaaaaaany", "aaaaaaaaaaaaaanz", "aaaaaaaaaaaaaaoa", "aaaaaaaaaaaaaaob", "aaaaaaaaaaaaaaoc", "aaaaaaaaaaaaaaod", "aaaaaaaaaaaaaaoe", "aaaaaaaaaaaaaaof", "aaaaaaaaaaaaaaog", "aaaaaaaaaaaaaaoh", "aaaaaaaaaaaaaaoi", "aaaaaaaaaaaaaaoj", "aaaaaaaaaaaaaaok", "aaaaaaaaaaaaaaol", "aaaaaaaaaaaaaaom", "aaaaaaaaaaaaaaon", "aaaaaaaaaaaaaaoo", "aaaaaaaaaaaaaaop", "aaaaaaaaaaaaaaoq", "aaaaaaaaaaaaaaor", "aaaaaaaaaaaaaaos", "aaaaaaaaaaaaaaot", "aaaaaaaaaaaaaaou", "aaaaaaaaaaaaaaov", "aaaaaaaaaaaaaaow", "aaaaaaaaaaaaaaox", "aaaaaaaaaaaaaaoy", "aaaaaaaaaaaaaaoz", "aaaaaaaaaaaaaapa", "aaaaaaaaaaaaaapb", "aaaaaaaaaaaaaapc", "aaaaaaaaaaaaaapd", "aaaaaaaaaaaaaape", "aaaaaaaaaaaaaapf", "aaaaaaaaaaaaaapg", "aaaaaaaaaaaaaaph", "aaaaaaaaaaaaaapi", "aaaaaaaaaaaaaapj", "aaaaaaaaaaaaaapk", "aaaaaaaaaaaaaapl", "aaaaaaaaaaaaaapm", "aaaaaaaaaaaaaapn", "aaaaaaaaaaaaaapo", "aaaaaaaaaaaaaapp", "aaaaaaaaaaaaaapq", "aaaaaaaaaaaaaapr", "aaaaaaaaaaaaaaps", "aaaaaaaaaaaaaapt", "aaaaaaaaaaaaaapu", "aaaaaaaaaaaaaapv", "aaaaaaaaaaaaaapw", "aaaaaaaaaaaaaapx", "aaaaaaaaaaaaaapy", "aaaaaaaaaaaaaapz", "aaaaaaaaaaaaaaqa", "aaaaaaaaaaaaaaqb", "aaaaaaaaaaaaaaqc", "aaaaaaaaaaaaaaqd", "aaaaaaaaaaaaaaqe", "aaaaaaaaaaaaaaqf", "aaaaaaaaaaaaaaqg", "aaaaaaaaaaaaaaqh", "aaaaaaaaaaaaaaqi", "aaaaaaaaaaaaaaqj", "aaaaaaaaaaaaaaqk", "aaaaaaaaaaaaaaql", "aaaaaaaaaaaaaaqm", "aaaaaaaaaaaaaaqn", "aaaaaaaaaaaaaaqo", "aaaaaaaaaaaaaaqp", "aaaaaaaaaaaaaaqq", "aaaaaaaaaaaaaaqr", "aaaaaaaaaaaaaaqs", "aaaaaaaaaaaaaaqt", "aaaaaaaaaaaaaaqu", "aaaaaaaaaaaaaaqv", "aaaaaaaaaaaaaaqw", "aaaaaaaaaaaaaaqx", "aaaaaaaaaaaaaaqy", "aaaaaaaaaaaaaaqz", "aaaaaaaaaaaaaara", "aaaaaaaaaaaaaarb", "aaaaaaaaaaaaaarc", "aaaaaaaaaaaaaard", "aaaaaaaaaaaaaare", "aaaaaaaaaaaaaarf", "aaaaaaaaaaaaaarg", "aaaaaaaaaaaaaarh", "aaaaaaaaaaaaaari", "aaaaaaaaaaaaaarj", "aaaaaaaaaaaaaark", "aaaaaaaaaaaaaarl", "aaaaaaaaaaaaaarm", "aaaaaaaaaaaaaarn", "aaaaaaaaaaaaaaro", "aaaaaaaaaaaaaarp", "aaaaaaaaaaaaaarq", "aaaaaaaaaaaaaarr", "aaaaaaaaaaaaaars", "aaaaaaaaaaaaaart", "aaaaaaaaaaaaaaru", "aaaaaaaaaaaaaarv", "aaaaaaaaaaaaaarw", "aaaaaaaaaaaaaarx", "aaaaaaaaaaaaaary", "aaaaaaaaaaaaaarz", "aaaaaaaaaaaaaasa", "aaaaaaaaaaaaaasb", "aaaaaaaaaaaaaasc", "aaaaaaaaaaaaaasd", "aaaaaaaaaaaaaase", "aaaaaaaaaaaaaasf", "aaaaaaaaaaaaaasg", "aaaaaaaaaaaaaash", "aaaaaaaaaaaaaasi", "aaaaaaaaaaaaaasj", "aaaaaaaaaaaaaask", "aaaaaaaaaaaaaasl", "aaaaaaaaaaaaaasm", "aaaaaaaaaaaaaasn", "aaaaaaaaaaaaaaso", "aaaaaaaaaaaaaasp", "aaaaaaaaaaaaaasq", "aaaaaaaaaaaaaasr", "aaaaaaaaaaaaaass", "aaaaaaaaaaaaaast", "aaaaaaaaaaaaaasu", "aaaaaaaaaaaaaasv", "aaaaaaaaaaaaaasw", "aaaaaaaaaaaaaasx", "aaaaaaaaaaaaaasy", "aaaaaaaaaaaaaasz", "aaaaaaaaaaaaaata", "aaaaaaaaaaaaaatb", "aaaaaaaaaaaaaatc", "aaaaaaaaaaaaaatd", "aaaaaaaaaaaaaate", "aaaaaaaaaaaaaatf", "aaaaaaaaaaaaaatg", "aaaaaaaaaaaaaath", "aaaaaaaaaaaaaati", "aaaaaaaaaaaaaatj", "aaaaaaaaaaaaaatk", "aaaaaaaaaaaaaatl", "aaaaaaaaaaaaaatm", "aaaaaaaaaaaaaatn", "aaaaaaaaaaaaaato", "aaaaaaaaaaaaaatp", "aaaaaaaaaaaaaatq", "aaaaaaaaaaaaaatr", "aaaaaaaaaaaaaats", "aaaaaaaaaaaaaatt", "aaaaaaaaaaaaaatu", "aaaaaaaaaaaaaatv", "aaaaaaaaaaaaaatw", "aaaaaaaaaaaaaatx", "aaaaaaaaaaaaaaty", "aaaaaaaaaaaaaatz", "aaaaaaaaaaaaaaua", "aaaaaaaaaaaaaaub", "aaaaaaaaaaaaaauc", "aaaaaaaaaaaaaaud", "aaaaaaaaaaaaaaue", "aaaaaaaaaaaaaauf", "aaaaaaaaaaaaaaug", "aaaaaaaaaaaaaauh", "aaaaaaaaaaaaaaui", "aaaaaaaaaaaaaauj", "aaaaaaaaaaaaaauk", "aaaaaaaaaaaaaaul", "aaaaaaaaaaaaaaum", "aaaaaaaaaaaaaaun", "aaaaaaaaaaaaaauo", "aaaaaaaaaaaaaaup", "aaaaaaaaaaaaaauq", "aaaaaaaaaaaaaaur", "aaaaaaaaaaaaaaus", "aaaaaaaaaaaaaaut", "aaaaaaaaaaaaaauu", "aaaaaaaaaaaaaauv", "aaaaaaaaaaaaaauw", "aaaaaaaaaaaaaaux", "aaaaaaaaaaaaaauy", "aaaaaaaaaaaaaauz", "aaaaaaaaaaaaaava", "aaaaaaaaaaaaaavb", "aaaaaaaaaaaaaavc", "aaaaaaaaaaaaaavd", "aaaaaaaaaaaaaave", "aaaaaaaaaaaaaavf", "aaaaaaaaaaaaaavg", "aaaaaaaaaaaaaavh", "aaaaaaaaaaaaaavi", "aaaaaaaaaaaaaavj", "aaaaaaaaaaaaaavk", "aaaaaaaaaaaaaavl", "aaaaaaaaaaaaaavm", "aaaaaaaaaaaaaavn", "aaaaaaaaaaaaaavo", "aaaaaaaaaaaaaavp", "aaaaaaaaaaaaaavq", "aaaaaaaaaaaaaavr", "aaaaaaaaaaaaaavs", "aaaaaaaaaaaaaavt", "aaaaaaaaaaaaaavu", "aaaaaaaaaaaaaavv", "aaaaaaaaaaaaaavw", "aaaaaaaaaaaaaavx", "aaaaaaaaaaaaaavy", "aaaaaaaaaaaaaavz", "aaaaaaaaaaaaaawa", "aaaaaaaaaaaaaawb", "aaaaaaaaaaaaaawc", "aaaaaaaaaaaaaawd", "aaaaaaaaaaaaaawe", "aaaaaaaaaaaaaawf", "aaaaaaaaaaaaaawg", "aaaaaaaaaaaaaawh", "aaaaaaaaaaaaaawi", "aaaaaaaaaaaaaawj", "aaaaaaaaaaaaaawk", "aaaaaaaaaaaaaawl", "aaaaaaaaaaaaaawm", "aaaaaaaaaaaaaawn", "aaaaaaaaaaaaaawo", "aaaaaaaaaaaaaawp", "aaaaaaaaaaaaaawq", "aaaaaaaaaaaaaawr", "aaaaaaaaaaaaaaws", "aaaaaaaaaaaaaawt", "aaaaaaaaaaaaaawu", "aaaaaaaaaaaaaawv", "aaaaaaaaaaaaaaww", "aaaaaaaaaaaaaawx", "aaaaaaaaaaaaaawy", "aaaaaaaaaaaaaawz", "aaaaaaaaaaaaaaxa", "aaaaaaaaaaaaaaxb", "aaaaaaaaaaaaaaxc", "aaaaaaaaaaaaaaxd", "aaaaaaaaaaaaaaxe", "aaaaaaaaaaaaaaxf", "aaaaaaaaaaaaaaxg", "aaaaaaaaaaaaaaxh", "aaaaaaaaaaaaaaxi", "aaaaaaaaaaaaaaxj", "aaaaaaaaaaaaaaxk", "aaaaaaaaaaaaaaxl", "aaaaaaaaaaaaaaxm", "aaaaaaaaaaaaaaxn", "aaaaaaaaaaaaaaxo", "aaaaaaaaaaaaaaxp", "aaaaaaaaaaaaaaxq", "aaaaaaaaaaaaaaxr", "aaaaaaaaaaaaaaxs", "aaaaaaaaaaaaaaxt", "aaaaaaaaaaaaaaxu", "aaaaaaaaaaaaaaxv", "aaaaaaaaaaaaaaxw", "aaaaaaaaaaaaaaxx", "aaaaaaaaaaaaaaxy", "aaaaaaaaaaaaaaxz", "aaaaaaaaaaaaaaya", "aaaaaaaaaaaaaayb", "aaaaaaaaaaaaaayc", "aaaaaaaaaaaaaayd", "aaaaaaaaaaaaaaye", "aaaaaaaaaaaaaayf", "aaaaaaaaaaaaaayg", "aaaaaaaaaaaaaayh", "aaaaaaaaaaaaaayi", "aaaaaaaaaaaaaayj", "aaaaaaaaaaaaaayk", "aaaaaaaaaaaaaayl", "aaaaaaaaaaaaaaym", "aaaaaaaaaaaaaayn", "aaaaaaaaaaaaaayo", "aaaaaaaaaaaaaayp", "aaaaaaaaaaaaaayq", "aaaaaaaaaaaaaayr", "aaaaaaaaaaaaaays", "aaaaaaaaaaaaaayt", "aaaaaaaaaaaaaayu", "aaaaaaaaaaaaaayv", "aaaaaaaaaaaaaayw", "aaaaaaaaaaaaaayx", "aaaaaaaaaaaaaayy", "aaaaaaaaaaaaaayz", "aaaaaaaaaaaaaaza", "aaaaaaaaaaaaaazb", "aaaaaaaaaaaaaazc", "aaaaaaaaaaaaaazd", "aaaaaaaaaaaaaaze", "aaaaaaaaaaaaaazf", "aaaaaaaaaaaaaazg", "aaaaaaaaaaaaaazh", "aaaaaaaaaaaaaazi", "aaaaaaaaaaaaaazj", "aaaaaaaaaaaaaazk", "aaaaaaaaaaaaaazl", "aaaaaaaaaaaaaazm", "aaaaaaaaaaaaaazn", "aaaaaaaaaaaaaazo", "aaaaaaaaaaaaaazp", "aaaaaaaaaaaaaazq", "aaaaaaaaaaaaaazr", "aaaaaaaaaaaaaazs", "aaaaaaaaaaaaaazt", "aaaaaaaaaaaaaazu", "aaaaaaaaaaaaaazv", "aaaaaaaaaaaaaazw", "aaaaaaaaaaaaaazx", "aaaaaaaaaaaaaazy", "aaaaaaaaaaaaaazz", "aaaaaaaaaaaaaaaa", "aaaaaaaaaaaaaaab", "aaaaaaaaaaaaaaac", "aaaaaaaaaaaaaaad", "aaaaaaaaaaaaaaae", "aaaaaaaaaaaaaaaf", "aaaaaaaaaaaaaaag", "aaaaaaaaaaaaaaah", "aaaaaaaaaaaaaaai", "aaaaaaaaaaaaaaaj", "aaaaaaaaaaaaaaak", "aaaaaaaaaaaaaaal", "aaaaaaaaaaaaaaam", "aaaaaaaaaaaaaaan", "aaaaaaaaaaaaaaao", "aaaaaaaaaaaaaaap", "aaaaaaaaaaaaaaaq", "aaaaaaaaaaaaaaar", "aaaaaaaaaaaaaaas", "aaaaaaaaaaaaaaat", "aaaaaaaaaaaaaaau", "aaaaaaaaaaaaaaav", "aaaaaaaaaaaaaaaw", "aaaaaaaaaaaaaaax", "aaaaaaaaaaaaaaay", "aaaaaaaaaaaaaaaz", "aaaaaaaaaaaaaaba", "aaaaaaaaaaaaaabb", "aaaaaaaaaaaaaabc", "aaaaaaaaaaaaaabd", "aaaaaaaaaaaaaabe", "aaaaaaaaaaaaaabf", "aaaaaaaaaaaaaabg", "aaaaaaaaaaaaaabh", "aaaaaaaaaaaaaabi", "aaaaaaaaaaaaaabj", "aaaaaaaaaaaaaabk", "aaaaaaaaaaaaaabl", "aaaaaaaaaaaaaabm", "aaaaaaaaaaaaaabn", "aaaaaaaaaaaaaabo", "aaaaaaaaaaaaaabp", "aaaaaaaaaaaaaabq", "aaaaaaaaaaaaaabr", "aaaaaaaaaaaaaabs", "aaaaaaaaaaaaaabt", "aaaaaaaaaaaaaabu", "aaaaaaaaaaaaaabv", "aaaaaaaaaaaaaabw", "aaaaaaaaaaaaaabx", "aaaaaaaaaaaaaaby", "aaaaaaaaaaaaaabz", "aaaaaaaaaaaaaaca", "aaaaaaaaaaaaaacb", "aaaaaaaaaaaaaacc", "aaaaaaaaaaaaaacd", "aaaaaaaaaaaaaace", "aaaaaaaaaaaaaacf", "aaaaaaaaaaaaaacg", "aaaaaaaaaaaaaach", "aaaaaaaaaaaaaaci", "aaaaaaaaaaaaaacj", "aaaaaaaaaaaaaack", "aaaaaaaaaaaaaacl", "aaaaaaaaaaaaaacm", "aaaaaaaaaaaaaacn", "aaaaaaaaaaaaaaco", "aaaaaaaaaaaaaacp", "aaaaaaaaaaaaaacq", "aaaaaaaaaaaaaacr", "aaaaaaaaaaaaaacs", "aaaaaaaaaaaaaact", "aaaaaaaaaaaaaacu", "aaaaaaaaaaaaaacv", "aaaaaaaaaaaaaacw", "aaaaaaaaaaaaaacx", "aaaaaaaaaaaaaacy", "aaaaaaaaaaaaaacz", "aaaaaaaaaaaaaada", "aaaaaaaaaaaaaadb", "aaaaaaaaaaaaaadc", "aaaaaaaaaaaaaadd", "aaaaaaaaaaaaaade", "aaaaaaaaaaaaaadf", "aaaaaaaaaaaaaadg", "aaaaaaaaaaaaaadh", "aaaaaaaaaaaaaadi", "aaaaaaaaaaaaaadj", "aaaaaaaaaaaaaadk", "aaaaaaaaaaaaaadl", "aaaaaaaaaaaaaadm", "aaaaaaaaaaaaaadn", "aaaaaaaaaaaaaado", "aaaaaaaaaaaaaadp", "aaaaaaaaaaaaaadq", "aaaaaaaaaaaaaadr", "aaaaaaaaaaaaaads", "aaaaaaaaaaaaaadt", "aaaaaaaaaaaaaadu", "aaaaaaaaaaaaaadv", "aaaaaaaaaaaaaadw", "aaaaaaaaaaaaaadx", "aaaaaaaaaaaaaady", "aaaaaaaaaaaaaadz", "aaaaaaaaaaaaaaea", "aaaaaaaaaaaaaaeb", "aaaaaaaaaaaaaaec", "aaaaaaaaaaaaaaed", "aaaaaaaaaaaaaaee", "aaaaaaaaaaaaaaef", "aaaaaaaaaaaaaaeg", "aaaaaaaaaaaaaaeh", "aaaaaaaaaaaaaaei", "aaaaaaaaaaaaaaej", "aaaaaaaaaaaaaaek", "aaaaaaaaaaaaaael", "aaaaaaaaaaaaaaem", "aaaaaaaaaaaaaaen", "aaaaaaaaaaaaaaeo", "aaaaaaaaaaaaaaep", "aaaaaaaaaaaaaaeq", "aaaaaaaaaaaaaaer", "aaaaaaaaaaaaaaes", "aaaaaaaaaaaaaaet", "aaaaaaaaaaaaaaeu", "aaaaaaaaaaaaaaev", "aaaaaaaaaaaaaaew", "aaaaaaaaaaaaaaex", "aaaaaaaaaaaaaaey", "aaaaaaaaaaaaaaez", "aaaaaaaaaaaaaafa", "aaaaaaaaaaaaaafb", "aaaaaaaaaaaaaafc", "aaaaaaaaaaaaaafd", "aaaaaaaaaaaaaafe", "aaaaaaaaaaaaaaff", "aaaaaaaaaaaaaafg", "aaaaaaaaaaaaaafh", "aaaaaaaaaaaaaafi", "aaaaaaaaaaaaaafj", "aaaaaaaaaaaaaafk", "aaaaaaaaaaaaaafl", "aaaaaaaaaaaaaafm", "aaaaaaaaaaaaaafn", "aaaaaaaaaaaaaafo", "aaaaaaaaaaaaaafp", "aaaaaaaaaaaaaafq", "aaaaaaaaaaaaaafr", "aaaaaaaaaaaaaafs", "aaaaaaaaaaaaaaft", "aaaaaaaaaaaaaafu", "aaaaaaaaaaaaaafv", "aaaaaaaaaaaaaafw", "aaaaaaaaaaaaaafx", "aaaaaaaaaaaaaafy", "aaaaaaaaaaaaaafz", "aaaaaaaaaaaaaaga", "aaaaaaaaaaaaaagb", "aaaaaaaaaaaaaagc", "aaaaaaaaaaaaaagd", "aaaaaaaaaaaaaage", "aaaaaaaaaaaaaagf", "aaaaaaaaaaaaaagg", "aaaaaaaaaaaaaagh", "aaaaaaaaaaaaaagi", "aaaaaaaaaaaaaagj", "aaaaaaaaaaaaaagk", "aaaaaaaaaaaaaagl", "aaaaaaaaaaaaaagm", "aaaaaaaaaaaaaagn", "aaaaaaaaaaaaaago", "aaaaaaaaaaaaaagp", "aaaaaaaaaaaaaagq", "aaaaaaaaaaaaaagr", "aaaaaaaaaaaaaags", "aaaaaaaaaaaaaagt", "aaaaaaaaaaaaaagu", "aaaaaaaaaaaaaagv", "aaaaaaaaaaaaaagw", "aaaaaaaaaaaaaagx", "aaaaaaaaaaaaaagy", "aaaaaaaaaaaaaagz", "aaaaaaaaaaaaaaha", "aaaaaaaaaaaaaahb", "aaaaaaaaaaaaaahc", "aaaaaaaaaaaaaahd", "aaaaaaaaaaaaaahe", "aaaaaaaaaaaaaahf", "aaaaaaaaaaaaaahg", "aaaaaaaaaaaaaahh", "aaaaaaaaaaaaaahi", "aaaaaaaaaaaaaahj", "aaaaaaaaaaaaaahk", "aaaaaaaaaaaaaahl", "aaaaaaaaaaaaaahm", "aaaaaaaaaaaaaahn", "aaaaaaaaaaaaaaho", "aaaaaaaaaaaaaahp", "aaaaaaaaaaaaaahq", "aaaaaaaaaaaaaahr", "aaaaaaaaaaaaaahs", "aaaaaaaaaaaaaaht", "aaaaaaaaaaaaaahu", "aaaaaaaaaaaaaahv", "aaaaaaaaaaaaaahw", "aaaaaaaaaaaaaahx", "aaaaaaaaaaaaaahy", "aaaaaaaaaaaaaahz", "aaaaaaaaaaaaaaia", "aaaaaaaaaaaaaaib", "aaaaaaaaaaaaaaic", "aaaaaaaaaaaaaaid", "aaaaaaaaaaaaaaie", "aaaaaaaaaaaaaaif", "aaaaaaaaaaaaaaig", "aaaaaaaaaaaaaaih", "aaaaaaaaaaaaaaii", "aaaaaaaaaaaaaaij", "aaaaaaaaaaaaaaik", "aaaaaaaaaaaaaail", "aaaaaaaaaaaaaaim", "aaaaaaaaaaaaaain", "aaaaaaaaaaaaaaio", "aaaaaaaaaaaaaaip", "aaaaaaaaaaaaaaiq", "aaaaaaaaaaaaaair", "aaaaaaaaaaaaaais", "aaaaaaaaaaaaaait", "aaaaaaaaaaaaaaiu", "aaaaaaaaaaaaaaiv", "aaaaaaaaaaaaaaiw", "aaaaaaaaaaaaaaix", "aaaaaaaaaaaaaaiy", "aaaaaaaaaaaaaaiz", "aaaaaaaaaaaaaaja", "aaaaaaaaaaaaaajb", "aaaaaaaaaaaaaajc", "aaaaaaaaaaaaaajd", "aaaaaaaaaaaaaaje", "aaaaaaaaaaaaaajf", "aaaaaaaaaaaaaajg", "aaaaaaaaaaaaaajh", "aaaaaaaaaaaaaaji", "aaaaaaaaaaaaaajj", "aaaaaaaaaaaaaajk", "aaaaaaaaaaaaaajl", "aaaaaaaaaaaaaajm", "aaaaaaaaaaaaaajn", "aaaaaaaaaaaaaajo", "aaaaaaaaaaaaaajp", "aaaaaaaaaaaaaajq", "aaaaaaaaaaaaaajr", "aaaaaaaaaaaaaajs", "aaaaaaaaaaaaaajt", "aaaaaaaaaaaaaaju", "aaaaaaaaaaaaaajv", "aaaaaaaaaaaaaajw", "aaaaaaaaaaaaaajx", "aaaaaaaaaaaaaajy", "aaaaaaaaaaaaaajz", "aaaaaaaaaaaaaaka", "aaaaaaaaaaaaaakb", "aaaaaaaaaaaaaakc", "aaaaaaaaaaaaaakd", "aaaaaaaaaaaaaake", "aaaaaaaaaaaaaakf", "aaaaaaaaaaaaaakg", "aaaaaaaaaaaaaakh", "aaaaaaaaaaaaaaki", "aaaaaaaaaaaaaakj", "aaaaaaaaaaaaaakk", "aaaaaaaaaaaaaakl", "aaaaaaaaaaaaaakm", "aaaaaaaaaaaaaakn", "aaaaaaaaaaaaaako", "aaaaaaaaaaaaaakp", "aaaaaaaaaaaaaakq", "aaaaaaaaaaaaaakr", "aaaaaaaaaaaaaaks", "aaaaaaaaaaaaaakt", "aaaaaaaaaaaaaaku", "aaaaaaaaaaaaaakv", "aaaaaaaaaaaaaakw", "aaaaaaaaaaaaaakx", "aaaaaaaaaaaaaaky", "aaaaaaaaaaaaaakz", "aaaaaaaaaaaaaala", "aaaaaaaaaaaaaalb", "aaaaaaaaaaaaaalc", "aaaaaaaaaaaaaald", "aaaaaaaaaaaaaale", "aaaaaaaaaaaaaalf", "aaaaaaaaaaaaaalg", "aaaaaaaaaaaaaalh", "aaaaaaaaaaaaaali", "aaaaaaaaaaaaaalj", "aaaaaaaaaaaaaalk", "aaaaaaaaaaaaaall" }), Is.EquivalentTo(new List<string>() {
                "aaaaaaaaaaaaaaaa", "aaaaaaaaaaaaaaab", "aaaaaaaaaaaaaaac", "aaaaaaaaaaaaaaad", "aaaaaaaaaaaaaaae", "aaaaaaaaaaaaaabc", "aaaaaaaaaaaaaabf", "aaaaaaaaaaaaaacb", "aaaaaaaaaaaaaacd", "aaaaaaaaaaaaaacg", "aaaaaaaaaaaaaadc", "aaaaaaaaaaaaaade", "aaaaaaaaaaaaaadh", "aaaaaaaaaaaaaaed", "aaaaaaaaaaaaaaei" }));

            Assert.That(this._212WordSearch2(new char[,] {
                { 'a', 'b' },
                { 'c', 'd' },
            }, new string[] { "ab", "cb", "ad", "bd", "ac", "ca", "da", "bc", "db", "adcb", "dabc", "abb", "acb" }), Is.EquivalentTo(new List<string>() { "ab", "ac", "bd", "ca", "db" }));
            Assert.That(this._212WordSearch2(new char[,] {
                { 'o','a','t','n' },
                { 'e','t','a','e' },
                { 'i','h','k','r' },
                { 'i','f','l','v' },
            }, new string[] { "oath", "pea", "eat", "rain", "tao" }), Is.EquivalentTo(new List<string>() { "eat", "oath", "tao" }));
            Assert.That(this._212WordSearch2(new char[,] {
                { 'o','a','t','n' },
                { 'e','t','a','e' },
                { 'i','h','k','r' },
                { 'i','f','l','v' },
            }, new string[] { "oath", "pea", "eat", "rain" }), Is.EquivalentTo(new List<string>() { "eat", "oath" }));
            Assert.That(this._212WordSearch2(new char[,] {
                { 'o','a','a','n' },
                { 'e','t','a','e' },
                { 'i','h','k','r' },
                { 'i','f','l','v' },
            }, new string[] { "oath", "pea", "eat", "rain" }), Is.EquivalentTo(new List<string>() { "eat", "oath" }));
            #endregion

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
