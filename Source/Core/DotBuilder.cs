namespace Pencil.Core
{
	using System.Collections.Generic;
	using System.IO;
    using System.Globalization;

	public class DotBuilder
	{
        string format;
        TextWriter target;

		public DotBuilder(TextWriter target)
		{
			this.target = target;
		}

		public TextWriter Target { get { return target; } }
        public int FontSize { get; set; }
        public double RankSeparation { get; set; }
        public double NodeSeparation { get; set; }

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
            if(FontSize != 0)
                Append("node[fontsize={0}]", FontSize);
            if(RankSeparation != 0)
                Append("ranksep={0}", RankSeparation);
            if(NodeSeparation != 0)
                Append("nodesep={0}", NodeSeparation);
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

        void Append(string format, object value)
        {
            Append(string.Format(CultureInfo.InvariantCulture, format, value));
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