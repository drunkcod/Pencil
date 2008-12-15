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

		static string ToDot(DirectedGraph graph)
		{
			return new DotBuilder(new StringWriter()).Write(graph).Target.ToString();
		}
	}
}