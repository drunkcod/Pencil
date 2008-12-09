namespace Pencil.Core
{
	using System.Collections.Generic;
	using System.Text;

	public class DotBuilder : DirectedGraph
	{
        string format = "{0}";
        StringBuilder result = new StringBuilder("digraph{");

		public override string ToString()
		{
            Begin();
            Nodes.ForEach(Append);
            Edges.ForEach(Append);
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