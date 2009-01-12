namespace Pencil.Core
{
	using System.Drawing;
	using System.Text;
	using System.Globalization;

	class DotStyleWriter
	{
		StringBuilder target;

		public DotStyleWriter(StringBuilder target)
		{
			this.target = target;
		}

		public DotStyleWriter Append(string name, int value)
		{
			if(value != 0)
				target.AppendFormat("{0}={1} ", name, value);
			return this;
		}

		public DotStyleWriter Append(string name, double value)
		{
			if(value != 0)
				target.AppendFormat(CultureInfo.InvariantCulture, "{0}={1} ", name, value);
			return this;
		}

		public DotStyleWriter Append(string name, Color color)
		{
			if(!color.IsEmpty)
				target.AppendFormat("{0}=\"#{1:X2}{2:X2}{3:X2}\" ", name, color.R, color.G, color.B);
			return this;
		}
	}
}