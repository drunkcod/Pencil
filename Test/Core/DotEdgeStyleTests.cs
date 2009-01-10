namespace Pencil.Test.Core
{
	using NUnit.Framework;
	using System.Drawing;
	using Pencil.Core;

	[TestFixture]
	public class DotEdgeStyleTests
	{
		[Test]
		public void Should_support_setting_ArrowSize()
		{
			var style = new DotEdgeStyle();
			style.ArrowSize = 0.7;

			style.ToString().ShouldEqual("arrowsize=0.7");
		}
		[Test]
		public void Should_support_setting_PenSize()
		{
			var style = new DotEdgeStyle();
			style.PenWidth = 0.75;

			style.ToString().ShouldEqual("penwidth=0.75");
		}
		[Test]
		public void Should_support_setting_Color()
		{
			var style = new DotEdgeStyle();
			style.Color = Color.FromArgb(0x30, 0x30, 0x30);

			style.ToString().ShouldEqual("color=\"#303030\"");
		}
	}
}