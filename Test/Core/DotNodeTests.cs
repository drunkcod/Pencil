namespace Pencil.Test.Core
{
	using NUnit.Framework;
	using System.Drawing;
	using Pencil.Core;

	[TestFixture]
	public class DotNodeTests
	{
		[Test]
		public void Should_support_fill_color()
		{
			var node = new DotNode(0);
			node.Label = "PinkNode";
			node.FillColor = Color.FromArgb(0xFF, 0x0F, 0xBE);

			node.ToString().ShouldEqual("0[label=\"PinkNode\" style=filled fillcolor=\"#FF0FBE\"]");
		}
	}
}