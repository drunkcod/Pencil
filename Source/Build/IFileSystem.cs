namespace Pencil.Build
{
	using System.Collections.Generic;

    public interface IFileSystem
    {
        bool DirectoryExists(string path);
        void CreateDirectory(string path);
        bool FileExists(Path path);
        void CopyFile(Path from, Path to, bool overwrite);
		void DeleteFile(Path path);
		IEnumerable<Path> GetFilesRecursive(Path root, string pattern);
    }
}