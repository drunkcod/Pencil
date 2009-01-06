namespace Pencil.Test.Stubs
{
    using System;
    using Pencil.Build;

    class FileSystemStub : IFileSystem
    {
        public Action<string> CreateDirectoryHandler;
        public Converter<string, bool> DirectoryExistsHandler;
        public Converter<string, bool> FileExistsHandler;
        public Action2<string, string> CopyFileHandler;
		public Action<string> DeleteFileHandler;
		public Func<string,string,string[]> GetFilesRecursiveHandler;

        public void CreateDirectory(string path) { CreateDirectoryHandler(path); }
        public bool DirectoryExists(string path) { return DirectoryExistsHandler(path); }
        public bool FileExists(string path) { return FileExistsHandler(path); }
        public void CopyFile(string from, string to) { CopyFileHandler(from, to); }
		public void DeleteFile(string path){ DeleteFileHandler(path); }
		public string[] GetFilesRecursive(string root, string pattern){ return GetFilesRecursiveHandler(root, pattern); }
    }
}