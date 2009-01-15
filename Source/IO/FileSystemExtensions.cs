namespace Pencil.IO
{
	public static class FileSystemExtensions
	{
		public static void WriteFile(this IFileSystem fileSystem, Path path,System.IO.Stream data)
		{
			using(var file = fileSystem.OpenWrite(path))
				data.CopyTo(file);
		}
	}
}