namespace Pencil.Test.Stubs
{
    using System;
	using System.Collections.Generic;
    using Pencil.Build;
    using Pencil.IO;

    public class FileSystemStub : IFileSystem
    {
		public delegate void Action3<TArg0, TArg1, TArg2>(TArg0 arg0, TArg1 arg1, TArg2 arg2);

        public Action<string> CreateDirectoryHandler = Path => {};
        public Converter<string, bool> DirectoryExistsHandler = x => false;
        public Converter<Path, bool> FileExistsHandler = x => true;
        public Action3<Path, Path, bool> CopyFileHandler = (x,y,overwrite) => {};
		public Action<Path> DeleteFileHandler = path => {};
		public Func<Path,string,IEnumerable<Path>> GetFilesRecursiveHandler = (path, pattern) => new Path[0];
		public Converter<Path, DateTime> GetLastWriteTimeHandler = path => DateTime.Today;

        public void CreateDirectory(string path) { CreateDirectoryHandler(path); }
        public bool DirectoryExists(string path) { return DirectoryExistsHandler(path); }
        public bool FileExists(Path path) { return FileExistsHandler(path); }
        public System.IO.Stream OpenWrite(Path path){ return System.IO.Stream.Null; }
        public void CopyFile(Path from, Path to, bool overwrite) { CopyFileHandler(from, to, overwrite); }
		public void DeleteFile(Path path){ DeleteFileHandler(path); }
		public IEnumerable<Path> GetFiles(Path root, string pattern){ return GetFilesRecursiveHandler(root, pattern); }
		public IEnumerable<Path> GetFilesRecursive(Path root, string pattern){ return GetFilesRecursiveHandler(root, pattern); }
    	public DateTime GetLastWriteTime(Path path){ return GetLastWriteTimeHandler(path); }

    }
}
