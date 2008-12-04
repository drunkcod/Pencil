namespace Pencil.Build
{
    using System.IO;

    sealed class FileSystem : IFileSystem
    {
        public bool DirectoryExists(string path) { return Directory.Exists(path); }
        public void CreateDirectory(string path) { Directory.CreateDirectory(path); }
        public bool FileExists(string path) { return File.Exists(path); }
        public void CopyFile(string from, string to) { File.Copy(from, to); }
    }
}
