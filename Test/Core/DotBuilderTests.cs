namespace Pencil.Test.Core
{
	using System.IO;
	using NUnit.Framework;
	using Pencil.Core;

	[TestFixture]
	public class DotBuilderTests
	{
		[Test]
		public void ToString_should_be_valid_if_empty()
		{
			ToDot(new DirectedGraph()).ShouldEqual("digraph{}");
		}
		[Test]
		public void Should_support_connecting_nodes()
		{
			var builder = new DirectedGraph();
			builder.AddNode().ConnectTo(builder.AddNode());

			ToDot(builder).ShouldEqual("digraph{0->1}");
		}
        [Test]
        public void Should_render_multiple_edges_correctly()
        {
            var builder = new DirectedGraph();
            var n0 = builder.AddNode();
            n0.ConnectTo(builder.AddNode());
            n0.ConnectTo(builder.AddNode());

            ToDot(builder).ShouldEqual("digraph{0->1 0->2}");
        }
        [Test]
        public void Should_render_node_labels_correctly()
        {
            var builder = new DirectedGraph();
            builder.AddNode().Label = "Pencil.Core.dll";
            ToDot(builder).ShouldEqual("digraph{0[label=\"Pencil.Core.dll\"]}");
        }
        [Test]
        public void Should_support_setting_FontSize()
        {
            var dot = new DotBuilder(new StringWriter());
            dot.FontSize = 8;
            WriteEmpty(dot).ShouldEqual("digraph{node[fontsize=8]}");
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
        public void Should_support_box_shaped_nodes()
        {
            var dot = new DotBuilder(new StringWriter());
            dot.NodeShape = NodeShape.Box;
            WriteEmpty(dot).ShouldEqual("digraph{node[shape=box]}");
        }
        [Test]
        public void Should_support_setting_node_height()
        {
            var dot = new DotBuilder(new StringWriter());
            dot.NodeHeight = 0.1;
            WriteEmpty(dot).ShouldEqual("digraph{node[height=0.1]}");
        }

        static string ToDot(DirectedGraph graph)
		{
			return new DotBuilder(new StringWriter()).Write(graph).Target.ToString();
		}

        static string WriteEmpty(DotBuilder dot)
        {
            return dot.Write(new DirectedGraph()).Target.ToString();
        }
	}
}