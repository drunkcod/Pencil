namespace Pencil.Core
{
	using System.Collections.Generic;
	using System.IO;

	public class DotBuilder
	{
        string format;
        TextWriter target;

		public DotBuilder(TextWriter target)
		{
			this.target = target;
		}

		public TextWriter Target { get { return target; } }

		public DotBuilder Write(DirectedGraph graph)
		{
            Begin();
            graph.Nodes.ForEach(Append);
            graph.Edges.ForEach(Append);
            End();
			return this;
		}

        void Begin()
        {
			target.Write("digraph{");
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
            target.Write(format, value);
            format = " {0}";
        }

        void End()
        {
            target.Write('}');
        }
	}
}