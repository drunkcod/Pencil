namespace Pencil.IO
{
    using System.IO;
	using System.Collections.Generic;

    public sealed class FileSystem : IFileSystem
    {
        public bool DirectoryExists(string path) { return Directory.Exists(path); }
        public void CreateDirectory(string path) { Directory.CreateDirectory(path); }
        public bool FileExists(Path path) { return File.Exists(path.ToString()); }
        public Stream OpenWrite(Path path){ return File.OpenWrite(path.ToString()); }
        public void CopyFile(Path from, Path to, bool overwrite) { File.Copy(from.ToString(), to.ToString(), overwrite); }
		public void DeleteFile(Path path) { File.Delete(path.ToString()); }
		public IEnumerable<Path> GetFiles(Path root, string pattern)
		{
			foreach(var item in Directory.GetFiles(root.ToString(), pattern))
				yield return new Path(item);
		}
		public IEnumerable<Path> GetFilesRecursive(Path root, string pattern)
		{
			foreach(var item in Directory.GetFiles(root.ToString(), pattern, SearchOption.AllDirectories))
				yield return new Path(item);
		}
    }
}