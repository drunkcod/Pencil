namespace Pencil.Test.Core
{
	using NUnit.Framework;
	using Pencil.Core;

	[TestFixture]
	public class DotBuilderTests
	{
		[Test]
		public void ToString_should_be_valid_if_empty()
		{
			Assert.AreEqual("digraph{}", new DotBuilder().ToString());
		}
		[Test]
		public void Should_support_connecting_nodes()
		{
			var builder = new DotBuilder();
			builder.AddNode().ConnectTo(builder.AddNode());

			Assert.AreEqual("digraph{0->1}", builder.ToString());
		}
        [Test]
        public void Should_render_multiple_edges_correctly()
        {
            var builder = new DotBuilder();
            var n0 = builder.AddNode();
            n0.ConnectTo(builder.AddNode());
            n0.ConnectTo(builder.AddNode());

            Assert.AreEqual("digraph{0->1 0->2}", builder.ToString());
        }
        [Test]
        public void Should_render_node_labels_correctly()
        {
            var builder = new DotBuilder();
            builder.AddNode().Label = "Pencil.Core.dll";
            Assert.AreEqual("digraph{0[label=\"Pencil.Core.dll\"]}", builder.ToString());
        }
	}
}