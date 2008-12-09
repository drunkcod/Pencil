namespace Pencil.Core
{
    using System.Collections.Generic;

    public class DirectedGraph
    {
        List<Node> nodes = new List<Node>();
        int nextNodeId;

        public Node AddNode()
        {
            var node = new Node(nextNodeId++);
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
