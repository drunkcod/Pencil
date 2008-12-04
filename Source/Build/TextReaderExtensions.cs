namespace Pencil.Build
{
	using System.IO;

	static class TextReaderExtensions
	{
		public static void CopyTo(this TextReader source, TextWriter target)
		{
			for(int c = source.Read(); c != -1; c = source.Read())
				target.Write((char)c);
		}
	}
}