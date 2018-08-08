using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Fundamentals.TestDataStructures
{
    public class TreeNode
    {
        public int val { get; set; }
        public TreeNode left { get; set; }
        public TreeNode right { get; set; }

        public TreeNode()
        {
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
                if (values[i] != -1) current.left = new TreeNode(values[i]);
                if (values[i + 1] != -1) current.right = new TreeNode(values[i + 1]);
                q.Enqueue(current.left);
                q.Enqueue(current.right);
            }
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
            List<TreeNode> current = new List<TreeNode>();
            current.Add(this);
            if (this.left != null) current.AddRange(this.left.GetNodesInOrder());
            if (this.right != null) current.AddRange(this.right.GetNodesInOrder());
            return current;
        }
    }

    [TestFixture]
    public class TestTrees
    {
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

        [Test]
        public void TestMethod()
        {
            TreeNode root = new TreeNode();

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
