namespace Pencil.Core
{
	using System.Globalization;

	static class StringExtensions
	{
		public static string InvariantFormat(this string format, params object[] args)
		{
			return string.Format(CultureInfo.InvariantCulture, format, args);
		}

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }
	}
}