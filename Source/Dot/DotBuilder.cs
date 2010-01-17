using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Text;
using Pencil.Core;
using Xlnt.Stuff;

namespace Pencil.Dot
{
	public class DotBuilder
	{
        string format;
        TextWriter target;
		DotNodeStyle nodeStyle = new DotNodeStyle();
		DotEdgeStyle edgeStyle = new DotEdgeStyle();

		public DotBuilder(TextWriter target)
		{
			this.target = target;
		}

		public TextWriter Target { get { return target; } }
        public double RankSeparation { get; set; }
        public double NodeSeparation { get; set; }
		public RankDirection RankDirection { get; set; }
		public DotNodeStyle NodeStyle { get { return nodeStyle; } set { nodeStyle = value; } }
		public DotEdgeStyle EdgeStyle { get { return edgeStyle; } set { edgeStyle = value; } }
		
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
            var nodeStyle = NodeStyle.AppendTo(new StringBuilder());;
            if(nodeStyle.Length != 0)
                Append("node[{0}]", nodeStyle);
            var edgeStyle = EdgeStyle.AppendTo(new StringBuilder());;
            if(edgeStyle.Length != 0)
                Append("edge[{0}]", edgeStyle);
            if(RankSeparation != 0)
                Append("ranksep={0}", RankSeparation);
            if(NodeSeparation != 0)
                Append("nodesep={0}", NodeSeparation);
			if(RankDirection != RankDirection.TopBottom)
				Append("rankdir={0}", Encode(RankDirection));
        }

		static string Encode(RankDirection dir)
		{
			switch(dir)
			{
				case RankDirection.LeftRight: return "LR";
				default: return "TB";
			}
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