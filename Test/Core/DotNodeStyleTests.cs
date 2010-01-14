using NUnit.Framework;
using Pencil.Dot;

namespace Pencil.Test.Dot
{
	[TestFixture]
	public class DotNodeStyleTests
	{
		[Test]
        public void Should_support_setting_FontSize()
        {
            var style = new DotNodeStyle();
            style.FontSize = 8;
			style.ToString().ShouldEqual("fontsize=8");
        }
        [Test]
        public void Should_support_box_shaped_nodes()
        {
            var style = new DotNodeStyle();
            style.Shape = NodeShape.Box;
            style.ToString().ShouldEqual("shape=box");
        }
        [Test]
        public void Should_support_setting_node_height()
        {
            var style = new DotNodeStyle();
            style.Height = 0.1;
            style.ToString().ShouldEqual("height=0.1");
        }
	}
}