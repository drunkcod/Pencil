using System.Text;
using System.Drawing;
using Pencil.Core;

namespace Pencil.Dot
{
	public class DotEdgeStyle
	{
		public double ArrowSize { get; set; }
		public double PenWidth { get; set; }
		public Color Color { get; set; }

		public bool IsEmpty 
		{ 
			get
			{
				return ArrowSize == 0
					&& PenWidth == 0
					&& Color.IsEmpty;
			}
		}

		public override string ToString(){ return AppendTo(new StringBuilder()).ToString(); }

		public StringBuilder AppendTo(StringBuilder target)
		{
			if(IsEmpty)
				return target;
			new DotStyleWriter(target)
				.Append("arrowsize", ArrowSize)
				.Append("penwidth", PenWidth)
				.Append("color", Color);
			target.Length -= 1;
			return target;
		}
	}
}