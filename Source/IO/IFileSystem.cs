namespace Pencil.IO
{
	using System.Collections.Generic;

    public interface IFileSystem
    {
        bool DirectoryExists(string path);
        void CreateDirectory(string path);
		void EnsureDirectory(Path path);
        bool FileExists(Path path);
        System.IO.Stream OpenWrite(Path path);
        void CopyFile(Path from, Path to, bool overwrite);
		void DeleteFile(Path path);
		IEnumerable<Path> GetFiles(Path root, string pattern); 
		IEnumerable<Path> GetFilesRecursive(Path root, string pattern);
    }
}