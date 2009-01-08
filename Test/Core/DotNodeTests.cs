namespace Pencil.Test.Core
{
	using NUnit.Framework;
	using System.Drawing;
	using Pencil.Core;

	[TestFixture]
	public class DotNodeTests
	{
		[Test]
		public void Should_support_FillColor()
		{
			var node = new DotNode(0);
			node.Label = "PinkNode";
			node.Style.FillColor = Color.FromArgb(0xFF, 0x0F, 0xBE);

			node.ToString().ShouldEqual("0[label=\"PinkNode\" style=filled fillcolor=\"#FF0FBE\"]");
		}
		[Test]
		public void Should_support_BorderColor()
		{
			var node = new DotNode(0);
			node.Label = "NodeWithBorder";
			node.Style.BorderColor = Color.FromArgb(0xFF, 0xA0, 0xE0);
			
			node.ToString().ShouldEqual("0[label=\"NodeWithBorder\" color=\"#FFA0E0\"]");
		}
		[Test]
		public void Should_support_FontColor()
		{
			var node = new DotNode(0);
			node.Label = "ColoredText";
			node.Style.FontColor = Color.FromArgb(0, 255, 0);
			node.ToString().ShouldEqual("0[label=\"ColoredText\" fontcolor=\"#00FF00\"]");
		}
	}
}
