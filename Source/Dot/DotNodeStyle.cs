using System.Drawing;
using System.Globalization;
using System.Text;

namespace Pencil.Dot
{
	public class DotNodeStyle
	{
		Color fillColor;
		Color borderColor;
		Color fontColor;

		public Color FillColor
		{
			get { return fillColor; }
			set { fillColor = value; }
		}

		public Color BorderColor
		{
			get { return borderColor; }
			set { borderColor = value; }
		}

		public Color FontColor
		{
			get { return fontColor; }
			set { fontColor = value; }
		}

		public bool IsEmpty
		{
			get
			{
				return FontSize == 0
					&& Shape == NodeShape.Oval
					&& Height == 0
					&& FillColor.IsEmpty
					&& BorderColor.IsEmpty
					&& FontColor.IsEmpty;
			}
		}

		public int FontSize { get; set; }
        public string FontName { get; set; }
        public NodeShape Shape { get; set; }
        public double Height { get; set; }

		public override string ToString(){ return AppendTo(new StringBuilder()).ToString(); }

		public StringBuilder AppendTo(StringBuilder target)
		{
			if(IsEmpty)
				return target;
			var style = new DotStyleWriter(target)
				.Append("fontsize", FontSize);
            if(Shape != NodeShape.Oval)
                target.AppendFormat("shape={0} ", Shape.ToString().ToLowerInvariant());            
            style.Append("height", Height);
			if(!FillColor.IsEmpty)
				target.Append("style=filled ");
			style.Append("fillcolor", FillColor)
				.Append("color", BorderColor)
				.Append("fontcolor", FontColor);
			target.Length -= 1;
			return target;
		}
	}
}