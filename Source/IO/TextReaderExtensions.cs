namespace Pencil.IO
{
	using System.IO;

	public static class TextReaderExtensions
	{
		public static void CopyTo(this TextReader source, TextWriter target)
		{
			for(int c = source.Read(); c != -1; c = source.Read())
				target.Write((char)c);
		}
	}
}