namespace Pencil.Core
{
	using System.Globalization;

	public static class StringExtensions
	{
		public static string InvariantFormat(this string format, params object[] args)
		{
			return string.Format(CultureInfo.InvariantCulture, format, args);
		}

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

		public static bool IsStartOf(this string prefix, string s)
		{
			return !s.IsNullOrEmpty() && s.StartsWith(prefix);
		}
	}
}