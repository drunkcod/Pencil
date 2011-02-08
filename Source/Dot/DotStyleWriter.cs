using System.Drawing;
using System.Text;
using System.Globalization;

namespace Pencil.Dot
{
	class DotStyleWriter
	{
        const string KeyValueFormat = "{0}={1} ";
		StringBuilder target;

		public DotStyleWriter(StringBuilder target)
		{
			this.target = target;
		}

		public DotStyleWriter Append(string name, int value)
		{
			if(value != 0)
				target.AppendFormat(KeyValueFormat, name, value);
			return this;
		}

		public DotStyleWriter Append(string name, double value)
		{
			if(value != 0)
				target.AppendFormat(CultureInfo.InvariantCulture, KeyValueFormat, name, value);
			return this;
		}

        public DotStyleWriter Append(string name, string value) {
            if(!string.IsNullOrEmpty(value))
               target.AppendFormat(KeyValueFormat, name, value);
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