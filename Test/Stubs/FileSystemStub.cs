namespace Pencil.Test.Stubs
{
    using System;
	using System.Collections.Generic;
    using Pencil.Build;

    class FileSystemStub : IFileSystem
    {
		public delegate void Action3<TArg0, TArg1, TArg2>(TArg0 arg0, TArg1 arg1, TArg2 arg2);

        public Action<string> CreateDirectoryHandler = Path => {};
        public Converter<string, bool> DirectoryExistsHandler;
		public Action<Path> EnsureDirectoryHandler = path => {};
        public Converter<Path, bool> FileExistsHandler;
        public Action3<Path, Path, bool> CopyFileHandler;
		public Action<Path> DeleteFileHandler = path => {};
		public Func<Path,string,IEnumerable<Path>> GetFilesRecursiveHandler = (path, pattern) => new Path[0];

        public void CreateDirectory(string path) { CreateDirectoryHandler(path); }
        public bool DirectoryExists(string path) { return DirectoryExistsHandler(path); }
		public void EnsureDirectory(Path path){ EnsureDirectoryHandler(path); }
        public bool FileExists(Path path) { return FileExistsHandler(path); }
        public void CopyFile(Path from, Path to, bool overwrite) { CopyFileHandler(from, to, overwrite); }
		public void DeleteFile(Path path){ DeleteFileHandler(path); }
		public IEnumerable<Path> GetFilesRecursive(Path root, string pattern){ return GetFilesRecursiveHandler(root, pattern); }
    }
}