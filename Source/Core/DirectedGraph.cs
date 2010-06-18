using System.Collections.Generic;
using System.Linq;
using Pencil.Dot;

namespace Pencil.Core
{
    public class DirectedGraph
    {
        readonly List<Node> nodes = new List<Node>();
        readonly Dictionary<Node, List<Node>> edges = new Dictionary<Node, List<Node>>();
		readonly INodeFactory nodeFactory;

		public DirectedGraph(INodeFactory nodeFactory)
		{
			this.nodeFactory = nodeFactory;
		}

        public Node AddNode()
        {
            var node = nodeFactory.Create();
            nodes.Add(node);
            return node;
        }

        public Node AddNode(string label)
        {
            var node = AddNode();
            node.Label = label;
            return node;
        }

        public void Connect(Node from, Node to) {
            var fromEdges = EdgesFor(from);
            if (fromEdges.Contains(to))
                return;
            fromEdges.Add(to);
        }

        public IEnumerable<Node> Nodes { get { return nodes; } }
        public IEnumerable<Edge> Edges {
            get {
                return nodes.SelectMany(x => EdgesFor(x).Select(y => new Edge(x.Id, y.Id)));
            }
        }

        List<Node> EdgesFor(Node node) {
            List<Node> value;
            if (edges.TryGetValue(node, out value))
                return value;
            return edges[node] = new List<Node>();
        }
    }
}