namespace Pencil.IO
{
	using System.IO;
	
	public static class StreamExtensions
	{
		const int BufferSize = 4096;
		
		public static void CopyTo(this Stream from, Stream to)
		{
			var buffer = new byte[BufferSize];
			for(int count; (count = from.Read(buffer, 0, BufferSize)) != 0;)
				to.Write(buffer, 0, count);
		}
	}
}