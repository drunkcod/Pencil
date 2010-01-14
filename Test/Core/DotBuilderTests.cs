using System.IO;
using NUnit.Framework;
using Pencil.Core;
using Pencil.Dot;

namespace Pencil.Test.Core
{
	[TestFixture]
	public class DotBuilderTests
	{
        static DirectedGraph EmptyGraph() { return new DirectedGraph(new DotNodeFactory()); }

		[Test]
		public void ToString_should_be_valid_if_empty()
		{
			ToDot(EmptyGraph()).ShouldEqual("digraph{}");
		}
		[Test]
		public void Should_support_connecting_nodes()
		{
			var builder = EmptyGraph();
			builder.AddNode().ConnectTo(builder.AddNode());

			ToDot(builder).ShouldEqual("digraph{0->1}");
		}
        [Test]
        public void Should_render_multiple_edges_correctly()
        {
            var builder = EmptyGraph();
            var n0 = builder.AddNode();
            n0.ConnectTo(builder.AddNode());
            n0.ConnectTo(builder.AddNode());

            ToDot(builder).ShouldEqual("digraph{0->1 0->2}");
        }
        [Test]
        public void Should_render_node_labels_correctly()
        {
            var builder = EmptyGraph();
            builder.AddNode().Label = "Pencil.Core.dll";
            ToDot(builder).ShouldEqual("digraph{0[label=\"Pencil.Core.dll\"]}");
        }
        [Test]
        public void Should_support_setting_RankSeparation()
        {
            var dot = new DotBuilder(new StringWriter());
            dot.RankSeparation = 0.12;
            WriteEmpty(dot).ShouldEqual("digraph{ranksep=0.12}");
        }
		[Test]
		public void Should_support_setting_RankDirection()
		{
            var dot = new DotBuilder(new StringWriter());
            dot.RankDirection = RankDirection.LeftRight;
            WriteEmpty(dot).ShouldEqual("digraph{rankdir=LR}");
		}
        [Test]
        public void Should_support_setting_NodeSeparation()
        {
            var dot = new DotBuilder(new StringWriter());
            dot.NodeSeparation = 0.12;
            WriteEmpty(dot).ShouldEqual("digraph{nodesep=0.12}");
        }
		[Test]
		public void Should_support_setting_NodeStyle()
		{
            var dot = new DotBuilder(new StringWriter());
			var style = new DotNodeStyle();
			style.Height = 0.1;
            dot.NodeStyle = style;
            WriteEmpty(dot).ShouldEqual("digraph{node[height=0.1]}");
		}
		[Test]
		public void Should_support_setting_EdgeStyle()
		{
            var dot = new DotBuilder(new StringWriter());
			var style = new DotEdgeStyle();
			style.ArrowSize = 0.1;
            dot.EdgeStyle = style;
            WriteEmpty(dot).ShouldEqual("digraph{edge[arrowsize=0.1]}");
		}

        static string ToDot(DirectedGraph graph)
		{
			return new DotBuilder(new StringWriter()).Write(graph).Target.ToString();
		}

        static string WriteEmpty(DotBuilder dot)
        {
            return dot.Write(EmptyGraph()).Target.ToString();
        }
	}
}