namespace Pencil.Core
{
    using System.Drawing;
	using System.Globalization;
    using System.Text;

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
        public NodeShape Shape { get; set; }
        public double Height { get; set; }

		public override string ToString(){ return AppendTo(new StringBuilder()).ToString().Trim(); }

		public StringBuilder AppendTo(StringBuilder target)
		{
			if(IsEmpty)
				return target;
			
			if(FontSize != 0)
                target.AppendFormat("fontsize={0} ", FontSize);
            if(Shape != NodeShape.Oval)
                target.AppendFormat("shape={0} ", Shape.ToString().ToLowerInvariant());
            if(Height != 0)
                target.AppendFormat("height={0} ", Height.ToString(CultureInfo.InvariantCulture));
			if(!FillColor.IsEmpty)
				AppendColor("fillcolor", FillColor, target.Append("style=filled "));
			if(!BorderColor.IsEmpty)
				AppendColor("color", BorderColor, target);
			if(!FontColor.IsEmpty)
				AppendColor("fontcolor", FontColor, target);
			target.Length -= 1;
			return target;
		}

		static void AppendColor(string name, Color color, StringBuilder target)
		{
			target.AppendFormat("{0}=\"#{1:X2}{2:X2}{3:X2}\" ", name, color.R, color.G, color.B);
		}
	}
}
