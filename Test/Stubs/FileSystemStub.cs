namespace Pencil.Test.Stubs
{
    using System;
	using System.Collections.Generic;
    using Pencil.Build;

    class FileSystemStub : IFileSystem
    {
		public delegate void Action3<TArg0, TArg1, TArg2>(TArg0 arg0, TArg1 arg1, TArg2 arg2);

        public Action<string> CreateDirectoryHandler;
        public Converter<string, bool> DirectoryExistsHandler;
        public Converter<Path, bool> FileExistsHandler;
        public Action3<Path, Path, bool> CopyFileHandler;
		public Action<Path> DeleteFileHandler;
		public Func<Path,string,IEnumerable<Path>> GetFilesRecursiveHandler;

        public void CreateDirectory(string path) { CreateDirectoryHandler(path); }
        public bool DirectoryExists(string path) { return DirectoryExistsHandler(path); }
        public bool FileExists(Path path) { return FileExistsHandler(path); }
        public void CopyFile(Path from, Path to, bool overwrite) { CopyFileHandler(from, to, overwrite); }
		public void DeleteFile(Path path){ DeleteFileHandler(path); }
		public IEnumerable<Path> GetFilesRecursive(Path root, string pattern){ return GetFilesRecursiveHandler(root, pattern); }
    }
}