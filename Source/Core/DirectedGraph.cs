namespace Pencil.Core
{
    using System.Collections.Generic;

    public class DirectedGraph
    {
        readonly List<Node> nodes = new List<Node>();
		readonly INodeFactory nodeFactory;

		public DirectedGraph(): this(new DotNodeFactory()){}
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

        public IEnumerable<Node> Nodes { get { return nodes; } }
        public IEnumerable<Edge> Edges
        {
            get
            {
                foreach(var node in Nodes)
                    foreach(var edge in node.Edges)
                        yield return edge;
            }
        }
    }
}