using log4net;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fundamentals.TestDataStructures
{
    public class GraphNode
    {
        //public static ILog logger = LogManager.GetLogger(typeof(GraphNode));

        public int val;
        public List<GraphNode> children;

        private GraphNode() { }
        public GraphNode(int val)
        {
            this.val = val;
            children = new List<GraphNode>();
        }

        public void GetGraphInDictionaryByDepth(Dictionary<int, List<GraphNode>> result, int depth = 0)
        {
            if (!result.ContainsKey(depth))
                result.Add(depth, new List<GraphNode>());
            result[depth].Add(this);

            foreach (GraphNode child in this.children)
                child.GetGraphInDictionaryByDepth(result, depth + 1);

            return;
        }

        public int GetHeight()
        {
            int height = 0;
            foreach (GraphNode child in this.children)
                height = (int)Math.Max(height, child.GetHeight());

            return height;
        }

        public MyTuple<int, int> GetChildHeightAndLongestDistance()
        {
            if (this.children.Count == 0) return new MyTuple<int, int>(0, 0);

            List<MyTuple<int, int>> lds = new List<MyTuple<int, int>>();
            foreach (GraphNode child in this.children)
                lds.Add(child.GetChildHeightAndLongestDistance());
            lds = lds.OrderBy(tuple => tuple.Item1).ThenBy(tuple => tuple.Item2).ToList();

            //StringBuilder sb = new StringBuilder();
            //for (int i = 0; i <= lds.Count - 1; i++)
            //    sb.AppendFormat("h{0}-l{1} ", lds[i].Item1, lds[i].Item2);
            //logger.InfoFormat("Printing children for node {1}: {0}", sb.ToString(), this.val);

            int longest = (int)Math.Max(lds[lds.Count - 1].Item1 + 1 + ((lds.Count == 1) ? 0 : lds[lds.Count - 2].Item1 + 1), lds[lds.Count - 1].Item2);

            return new MyTuple<int, int>(lds[lds.Count - 1].Item1 + 1, longest);
        }
    }

    [TestFixture]
    public class TestGraphs
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(TestGraphs));

        #region "get longest distance"
        private int GetLongestDistance(List<int> A)
        {
            GraphNode root = this.AssembleMultiNodeTree(A);

            //Dictionary<int, List<GraphNode>> d = new Dictionary<int, List<GraphNode>>();
            //root.GetGraphInDictionaryByDepth(d);
            //StringBuilder sb = new StringBuilder();
            //foreach (KeyValuePair<int, List<GraphNode>> p in d)
            //{
            //    sb.AppendFormat("Depth {0}: ", p.Key);
            //    foreach (GraphNode node in p.Value)
            //        sb.AppendFormat("{0}, ", node.val);
            //    sb.AppendLine();
            //}
            //logger.InfoFormat("Printing tree assembling result: \n{0}", sb.ToString());

            return root.GetChildHeightAndLongestDistance().Item2;
        }

        private GraphNode AssembleMultiNodeTree(List<int> A)
        {
            Dictionary<int, GraphNode> map = new Dictionary<int, GraphNode>();

            GraphNode root = new GraphNode(0);
            map.Add(0, root);

            for (int i = 1; i <= A.Count - 1; i++)
            {
                GraphNode node = new GraphNode(i);
                map.Add(i, node);
                GraphNode parent = map[A[i]];
                parent.children.Add(node);
            }

            return root;
        }
        #endregion

        [Test]
        public void TestMethod()
        {
            #region "get longest distance"
            //Assert.That(this.GetLongestDistance(new List<int>() { -1, 0, 1, 1, 2, 0, 5, 0, 3, 0, 0, 2, 3, 1, 12, 14, 0, 5, 9, 6, 16, 0, 13, 4, 17, 2, 1, 22, 14, 20, 10, 17, 0, 32, 15, 34, 10, 19, 3, 22, 29, 2, 36, 16, 15, 37, 38, 27, 31, 12, 24, 29, 17, 29, 32, 45, 40, 15, 35, 13, 25, 57, 20, 4, 44, 41, 52, 9, 53, 57, 18, 5, 44, 29, 30, 9, 29, 30, 8, 57, 8, 59, 59, 64, 37, 6, 54, 32, 40, 26, 15, 87, 49, 90, 6, 81, 73, 10, 8, 16 }), Is.EqualTo(14));
            Assert.That(this.GetLongestDistance(new List<int>() { -1, 0, 0, 0, 3 }), Is.EqualTo(3));
            #endregion
        }
    }
}
