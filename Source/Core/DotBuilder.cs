namespace Pencil.Core
{
	using System.Collections.Generic;
	using System.Text;

	public class DotBuilder
	{
		int nextNodeId;
        List<Node> nodes = new List<Node>();
        string format = "{0}";
        StringBuilder result = new StringBuilder("digraph{");

		public Node AddNode()
		{
            var node = new Node(nextNodeId++);
            nodes.Add(node);
			return node;
		}

		public override string ToString()
		{
            Begin();
            nodes.ForEach(Append);
            return Finalize();
		}

        void Begin()
        {
            result = new StringBuilder("digraph{");
            format = "{0}";
        }

        void Append(Edge edge)
        {
            Append(edge.ToString());
        }

        void Append(Node node)
        {
            foreach(var edge in node.Edges)
                Append(edge);
            if(node.IsEmpty)
                return;
            Append(node.ToString());
        }

        void Append(string value)
        {
            result.AppendFormat(format, value);
            format = " {0}";
        }

        string Finalize()
        {
            result.Append('}');
            return result.ToString();
        }
	}
}