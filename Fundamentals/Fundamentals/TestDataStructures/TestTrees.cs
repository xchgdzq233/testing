using log4net;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fundamentals.TestDataStructures
{
    public class MyTuple<T1, T2>
    {
        public T1 Item1 { get; private set; }
        public T2 Item2 { get; private set; }
        internal MyTuple(T1 item1, T2 item2)
        {
            this.Item1 = item1;
            this.Item2 = item2;
        }
    }

    public class TreeNode
    {
        private static ILog logger = LogManager.GetLogger(typeof(TreeNode));

        public int val { get; set; }
        public TreeNode left { get; set; }
        public TreeNode right { get; set; }

        public TreeNode()
        {
            this.val = Int32.MinValue;
            left = null;
            right = null;
        }

        public TreeNode(int value)
        {
            this.val = value;
            left = null;
            right = null;
        }

        public TreeNode(List<int> values)
        {
            Queue<TreeNode> q = new Queue<TreeNode>();
            this.val = values[0];
            q.Enqueue(this);

            for (int i = 1; i <= values.Count - 1 && q.Count != 0; i += 2)
            {
                TreeNode current = q.Dequeue();
                if (values[i] != -1)
                {
                    current.left = new TreeNode(values[i]);
                    q.Enqueue(current.left);
                }
                if (values[i + 1] != -1)
                {
                    current.right = new TreeNode(values[i + 1]);
                    q.Enqueue(current.right);
                }
            }
        }

        public List<int> GetNodeBfs()
        {
            List<int> result = new List<int>();
            StringBuilder sb = new StringBuilder();

            Queue<TreeNode> q = new Queue<TreeNode>();
            q.Enqueue(this);
            while (q.Count != 0)
            {
                TreeNode node = q.Dequeue();
                result.Add(node.val);
                sb.AppendFormat("{0} ", node.val);
                if (node.left != null) q.Enqueue(node.left);
                if (node.right != null) q.Enqueue(node.right);
            }

            logger.InfoFormat(sb.ToString());
            return result;
        }

        public void GetNodeBfs(Dictionary<int, List<TreeNode>> result, int height = 0)
        {
            if (!result.ContainsKey(height))
                result.Add(height, new List<TreeNode>() { this });
            else
                result[height].Add(this);

            if (this.left != null) this.left.GetNodeBfs(result, height + 1);
            if (this.right != null) this.right.GetNodeBfs(result, height + 1);
        }

        public List<TreeNode> GetNodesInOrder()
        {
            List<TreeNode> current = new List<TreeNode>
            {
                this
            };
            if (this.left != null) current.AddRange(this.left.GetNodesInOrder());
            if (this.right != null) current.AddRange(this.right.GetNodesInOrder());
            return current;
        }
    }

    public class TrieNode
    {
        public char val;
        public int count;
        public Dictionary<char, TrieNode> next;

        public TrieNode()
        {
            this.val = '\0';
            this.count = 0;
            this.next = new Dictionary<char, TrieNode>();
        }

        public TrieNode(char val)
        {
            this.val = val;
            this.count = 0;
            this.next = new Dictionary<char, TrieNode>();
        }

        /*
        public override bool Equals(object obj)
        {
            return this.val == (obj as TrieNode).val;
        }

        public override int GetHashCode()
        {
            return this.val.GetHashCode();
        }
        */

        public void AddWord(String word, int i = 0)
        {
            if (i > word.Length - 1) return;

            if (!next.ContainsKey(word[i]))
                this.next.Add(word[i], new TrieNode(word[i]));
            TrieNode node = this.next[word[i]];

            node.count++;
            node.AddWord(word, i + 1);
        }

        public List<String> GetWords(String previous = "", int parentCount = 0)
        {
            List<String> result = new List<String>();
            String current = "";

            if (this.val != '\0')
            {
                for (int i = 0; i <= parentCount - this.count - 1; i++)
                    result.Add(previous);

                current = previous + this.val;
                if (this.next.Count == 0)
                    result.Add(current);
            }

            foreach (KeyValuePair<char, TrieNode> pair in this.next)
                result.AddRange(pair.Value.GetWords(current, this.count));

            return result;
        }

        public bool ContainsWord(String input, int i = 0)
        {
            if (i > input.Length) return false;
            if (i == input.Length) return true;
            if (!this.next.ContainsKey(input[i])) return false;

            return this.next[input[i]].ContainsWord(input, i + 1);
        }
    }

    [TestFixture]
    public class TestTrees
    {
        private static ILog logger = LogManager.GetLogger(typeof(TestTrees));

        #region "is bst"
        private int IsBst(TreeNode root)
        {
            return IsBstInBound(root, Int32.MinValue, Int32.MaxValue) ? 1 : 0;
        }

        private bool IsBstInBound(TreeNode current, int min, int max)
        {
            if (current == null) return true;

            if (current.val > min && current.val < max && IsBstInBound(current.left, min, current.val) && IsBstInBound(current.right, current.val, max))
                return true;

            return false;
        }
        #endregion

        #region "find successor"
        public TreeNode FindSuccessor(TreeNode root, int data)
        {
            // find the node
            TreeNode theNode = root;
            while (theNode.val != data)
            {
                if (data < theNode.val)
                    theNode = theNode.left;
                else
                    theNode = theNode.right;
            }

            // has right node
            if (theNode.right != null)
            {
                theNode = theNode.right;
                while (theNode.left != null)
                    theNode = theNode.left;
                return theNode;
            }

            // no right node
            TreeNode successor = null;
            TreeNode ancestor = root;
            while (ancestor.val != data)
            {
                if (theNode.val < ancestor.val)
                {
                    successor = ancestor;
                    ancestor = ancestor.left;
                }
                else
                    ancestor = ancestor.right;
            }

            return successor;
        }
        #endregion

        #region "pre/in/post order traversal without recursion"
        private List<int> PreOrderTraversalWithoutRecursion(TreeNode A)
        {
            List<int> result = new List<int>();
            Stack<TreeNode> s = new Stack<TreeNode>();

            if (A == null) return result;
            s.Push(A);

            while (s.Count != 0)
            {
                A = s.Pop();

                while (A != null)
                {
                    result.Add(A.val);
                    if (A.right != null)
                        s.Push(A.right);
                    A = A.left;
                }
            }

            return result;
        }

        private List<int> InOrderTraversalWithoutRecursion(TreeNode A)
        {
            List<int> result = new List<int>();
            if (A == null) return result;

            Stack<TreeNode> s = new Stack<TreeNode>();

            while (A != null)
            {
                if (A != null)
                {
                    s.Push(A);
                    A = A.left;
                }

                while (A == null && s.Count != 0)
                {
                    TreeNode current = s.Pop();
                    result.Add(current.val);
                    if (current.right != null)
                    {
                        A = current.right;
                        break;
                    }
                }
            }

            return result;
        }

        private List<int> PostOrderTraversalWithoutRecursion(TreeNode A)
        {
            List<int> result = new List<int>();
            if (A == null) return result;

            Stack<TreeNode> s = new Stack<TreeNode>();
            s.Push(A);

            while (s.Count != 0)
            {
                A = s.Pop();
                result.Insert(0, A.val);
                if (A.left != null) s.Push(A.left);
                if (A.right != null) s.Push(A.right);
            }

            return result;
        }
        #endregion

        #region "least common ancestor"
        private int LeastCommonAncestor(TreeNode A, int B, int C)
        {
            List<int> bPath = GetNodePath(A, B);
            List<int> cPath = GetNodePath(A, C);

            if (bPath.Count == 0 || cPath.Count == 0) return -1;
            if (B == C) return B;

            for (int i = 0; i <= bPath.Count - 1 && i <= cPath.Count - 1; i++)
            {
                if (bPath[i] != cPath[i])
                    return bPath[i - 1];
                if (i == bPath.Count - 1 || i == cPath.Count - 1)
                    return bPath[i];
            }
            return -1;
        }

        public List<int> GetNodePath(TreeNode A, int val)
        {
            if (A == null) return new List<int>();

            if (A.val == val)
                return new List<int>() { A.val };

            List<int> result = this.GetNodePath(A.left, val);
            if (result.Count != 0)
            {
                result.Insert(0, A.val);
                return result;
            }
            result = this.GetNodePath(A.right, val);
            if (result.Count != 0)
            {
                result.Insert(0, A.val);
                return result;
            }

            return new List<int>();
        }
        #endregion

        #region "shortest unique prefix"
        private List<String> ShortestUniquePrefix(List<String> A)
        {
            TrieNode trie = new TrieNode();
            foreach (String s in A)
                trie.AddWord(s);

            List<String> result = new List<string>();
            foreach (String s in A)
            {
                String current = "";
                TrieNode node = trie;
                for (int i = 0; i <= s.ToCharArray().Length - 1; i++)
                {
                    current += s[i];
                    if (!node.next.ContainsKey(s[i]))
                        break;
                    node = node.next[s[i]];
                    if (node.count == 1)
                        break;
                }
                if (!String.IsNullOrEmpty(current))
                    result.Add(current);
            }

            return result;
        }
        #endregion

        #region "calculate hotel goodness"
        private List<int> CalculateHotelGoodness(String S, List<String> R)
        {
            TrieNode goodWords = this.GetTrieFromString(S, '_');

            Dictionary<int, List<int>> scores = new Dictionary<int, List<int>>();
            int maxScore = 0;
            List<int> result = new List<int>();

            for (int i = 0; i <= R.Count - 1; i++)
            {
                int goodness = 0;
                List<String> reviewWords = new List<String>(R[i].Split('_'));
                foreach (String reviewWord in reviewWords)
                    if (goodWords.ContainsWord(reviewWord))
                        goodness++;
                if (!scores.ContainsKey(goodness))
                    scores.Add(goodness, new List<int>());
                scores[goodness].Add(i);
                maxScore = Math.Max(maxScore, goodness);
            }

            foreach (KeyValuePair<int, List<int>> pair in scores)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("score <{0}>: ", pair.Key);
                foreach (int i in pair.Value)
                    sb.AppendFormat("\n\t{0} - {1}, ", i, R[i]);
                logger.InfoFormat(sb.ToString());
            }

            for (int i = maxScore; i >= 0; i--)
            {
                if (!scores.ContainsKey(i))
                    continue;
                result.AddRange(scores[i]);
            }

            return result;
        }

        private TrieNode GetTrieFromString(String input, Char c)
        {
            List<String> words = new List<String>(input.Split(c));
            TrieNode root = new TrieNode();
            foreach (String s in words)
                root.AddWord(s);
            return root;
        }
        #endregion

        #region "check tree height balanced"
        private int CheckTreeHeightBalanced(TreeNode A)
        {
            return GetHeight(A) >= 0 ? 1 : 0;
        }

        public int GetHeight(TreeNode A)
        {
            if (A == null)
                return 0;

            int left = GetHeight(A.left);
            if (left == -1) return -1;

            int right = GetHeight(A.right);
            if (right == -1) return -1;

            if (Math.Abs(left - right) > 1) return -1;
            return (int)Math.Max(left, right) + 1;
        }
        #endregion

        #region "invert tree"
        private TreeNode InvertTree(TreeNode A)
        {
            Queue<TreeNode> q = new Queue<TreeNode>();
            TreeNode dummy = new TreeNode(-1)
            {
                left = A
            };
            q.Enqueue(A);
            while (q.Count != 0)
            {
                A = q.Dequeue();
                TreeNode temp = A.left;
                A.left = A.right;
                A.right = temp;

                if (A.left != null) q.Enqueue(A.left);
                if (A.right != null) q.Enqueue(A.right);
            }
            return dummy.left;
        }
        #endregion

        #region "is symmetric binary tree"
        private int IsSymmetricBinaryTree(TreeNode A)
        {
            if ((A.left == null && A.right != null) || (A.left != null && A.right == null)) return 0;

            Queue<TreeNode> leftQ = new Queue<TreeNode>();
            Queue<TreeNode> rightQ = new Queue<TreeNode>();
            leftQ.Enqueue(A.left);
            rightQ.Enqueue(A.right);

            while (leftQ.Count != 0 || rightQ.Count != 0)
            {
                if (leftQ.Count == 0 || rightQ.Count == 0) return 0;

                TreeNode left = leftQ.Dequeue();
                TreeNode right = rightQ.Dequeue();

                if (left.val != right.val) return 0;

                if (left.left == null)
                {
                    if (right.right != null) return 0;
                }
                else
                {
                    leftQ.Enqueue(left.left);
                    rightQ.Enqueue(right.right);
                }

                if (left.right == null)
                {
                    if (right.left != null) return 0;
                }
                else
                {
                    leftQ.Enqueue(left.right);
                    rightQ.Enqueue(right.left);
                }
            }

            return 1;
        }
        #endregion

        #region "get order of people heights"
        private List<int> GetOrderOfPeopleHeights(List<int> A, List<int> B)
        {
            List<MyTuple<int, int>> o = A.Select((h, i) => new MyTuple<int, int>(h, B[i])).OrderByDescending(p => p.Item1).ToList();

            List<int> result = new List<int>();
            foreach (MyTuple<int, int> p in o)
                result.Insert(p.Item2, p.Item1);

            return result;
        }
        #endregion

        [Test]
        public void TestMethod()
        {
            TreeNode root = new TreeNode();
            TrieNode trie = new TrieNode();

            #region "get order of people heights"
            Assert.That(this.GetOrderOfPeopleHeights(new List<int>() { 5, 3, 2, 6, 1, 4 }, new List<int>() { 0, 1, 2, 0, 4, 2 }), Is.EqualTo(new List<int>() { 5, 3, 2, 6, 1, 4}));
            #endregion

            #region "is symmetric binary tree"
            //root = new TreeNode(new List<int>() { 1, 2, 2, 3, 4, 4, 3 });
            //Assert.That(this.IsSymmetricBinaryTree(root), Is.EqualTo(1));

            //root = new TreeNode(new List<int>() { 1, 2, 2, -1, 3, 3, -1});
            //Assert.That(this.IsSymmetricBinaryTree(root), Is.EqualTo(1));

            //root = new TreeNode(new List<int>() { 1, 2, 2, -1, 3, -1, 3 });
            //Assert.That(this.IsSymmetricBinaryTree(root), Is.EqualTo(0));
            #endregion

            #region "invert tree"
            //root = new TreeNode(new List<int>() { 1, 2, 3, 4, 5, 6, 7 });
            //Assert.That(this.InvertTree(root).GetNodeBfs(), Is.EqualTo(new List<int>() { 1, 3, 2, 7, 6, 5, 4 }));
            #endregion

            #region "check tree height balanced"
            //root = new TreeNode(new List<int>() { 1, 2, 3, 4, 5, -1, 6, 7, 8, -1, -1, -1, -1, -1, -1, -1, -1 });
            //Assert.That(this.CheckTreeHeightBalanced(root), Is.EqualTo(1));

            //root = new TreeNode(new List<int>() { 7, 3, 12, 5, 1, 2, 4, 11, 10, -1, 13, -1, -1, 6, 9, -1, -1, -1, 14, 15, -1, 8, -1, -1, -1, -1, -1, -1, -1, -1, -1 });
            //Assert.That(this.CheckTreeHeightBalanced(root), Is.EqualTo(0));

            //root = new TreeNode(1);
            //root.left = new TreeNode(2);
            //Assert.That(this.CheckTreeHeightBalanced(root), Is.EqualTo(1));

            //root = new TreeNode(0);
            //root.left = new TreeNode(1);
            //root.left.left = new TreeNode(2);
            //Assert.That(this.CheckTreeHeightBalanced(root), Is.EqualTo(0));
            #endregion

            #region "calculate hotel goodness"
            //Assert.That(this.CalculateHotelGoodness("coolsai_cool", new List<String>() { "sai_cool", "cool_coolsai", "cool_cool" }), Is.EqualTo(new List<int>() { 1, 2, 0 }));

            //Assert.That(this.CalculateHotelGoodness("cool_ice_wifi", new List<string>() { "water_is_cool", "cold_ice_drink", "cool_wifi_speed" }), Is.EqualTo(new List<int>() { 2, 0, 1 }));
            #endregion

            #region "shortest unique prefix"
            //List<String> input = new List<string>() { "zebra", "dog", "duck", "dove" };
            //List<String> output = this.ShortestUniquePrefix(input);
            //List<String> anwser = new List<string>() { "z", "dog", "du", "dov" };

            //for (int i = 0; i <= output.Count - 1; i++)
            //    Console.WriteLine("input {0}, output {1}, anwser {2}", input[i], output[i], anwser[i]);
            //Assert.That(output, Is.EqualTo(anwser));
            #endregion

            #region "test my Trie"
            //trie.AddWord("duck");
            //trie.AddWord("ducky");
            //foreach (String s in trie.GetWords())
            //    Console.WriteLine(s);
            #endregion

            #region "is bst"
            //root.val = 3;
            //root.left = new TreeNode(2);
            //root.right = new TreeNode(4);
            //root.left.left = new TreeNode(1);
            //root.left.right = new TreeNode(3);
            //Assert.That(this.IsBst(root), Is.EqualTo(0));
            #endregion

            #region "find successor"
            //root = new TreeNode(100);
            //root.left = new TreeNode(98);
            //root.right = new TreeNode(102);
            //root.left.left = new TreeNode(96);
            //root.left.right = new TreeNode(99);
            //root.left.left.right = new TreeNode(97);
            //root.right = new TreeNode(102);

            //Assert.That(this.FindSuccessor(root, 97).val, Is.EqualTo(98));
            #endregion

            #region "least common ancestor"
            //root = new TreeNode(3);
            //root.left = new TreeNode(5);
            //root.right = new TreeNode(1);
            //root.left.left = new TreeNode(6);
            //root.left.right = new TreeNode(2);
            //root.left.right.left = new TreeNode(7);
            //root.left.right.right = new TreeNode(4);
            //root.right.left = new TreeNode(0);
            //root.right.right = new TreeNode(8);
            //Assert.That(this.LeastCommonAncestor(root, 100, 100), Is.EqualTo(-1));
            //Assert.That(this.LeastCommonAncestor(root, 5, 4), Is.EqualTo(5));
            //Assert.That(this.LeastCommonAncestor(root, 5, 1), Is.EqualTo(3));

            //root = new TreeNode(1);
            //Assert.That(this.LeastCommonAncestor(root, 1, 1), Is.EqualTo(1));
            #endregion

            #region "pre/in/post order traversal without recursion"
            //root = new TreeNode(1);
            //root.left = new TreeNode(2);
            //root.right = new TreeNode(3);
            //root.left.left = new TreeNode(4);
            //root.left.right = new TreeNode(5);
            //root.right.left = new TreeNode(6);
            //root.left.left.left = new TreeNode(7);
            //root.left.left.right = new TreeNode(8);
            //root.left.right.right = new TreeNode(9);

            //List<int> result = new List<int>();

            //result = this.PreOrderTraversalWithoutRecursion(root);
            //Console.WriteLine("Pre-Order");
            //foreach (int i in result)
            //    Console.Write(i + " ");
            //Assert.That(result, Is.EqualTo(new List<int>() { 1, 2, 4, 7, 8, 5, 9, 3, 6 }));

            //result = this.InOrderTraversalWithoutRecursion(root);
            //Console.WriteLine("\nIn-Order");
            //foreach (int i in result)
            //    Console.Write(i + " ");
            //Assert.That(result, Is.EqualTo(new List<int>() { 7, 4, 8, 2, 5, 9, 1, 6, 3 }));

            //result = this.PostOrderTraversalWithoutRecursion(root);
            //Console.WriteLine("\nPost-Order");
            //foreach (int i in result)
            //    Console.Write(i + " ");
            //Assert.That(result, Is.EqualTo(new List<int>() { 7, 8, 4, 9, 5, 2, 6, 3, 1 }));
            #endregion
        }
    }
}
