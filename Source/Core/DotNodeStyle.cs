namespace Pencil.Core
{
    using System.Drawing;
    using System.Text;
    
	public class DotNodeStyle
	{
		Color fillColor = Color.White;
		Color borderColor = Color.Black;
		Color fontColor = Color.Black;

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

		public StringBuilder AppendTo(StringBuilder target)
		{
			if(FillColor != Color.White)
				AppendColor("fillcolor", FillColor, target.Append(" style=filled"));
			if(BorderColor != Color.Black)
				AppendColor("color", BorderColor, target);
			if(FontColor != Color.Black)
				AppendColor("fontcolor", FontColor, target);
			return target;
		}
		
		static void AppendColor(string name, Color color, StringBuilder target)
		{
			target.AppendFormat(" {0}=\"#{1:X2}{2:X2}{3:X2}\"", name, color.R, color.G, color.B);
		}
	}

}
