namespace Pencil.Build
{
    public interface IFileSystem
    {
        bool DirectoryExists(string path);
        void CreateDirectory(string path);
        bool FileExists(string path);
        void CopyFile(string from, string to);
		void DeleteFile(string path);
		string[] GetFilesRecursive(string root, string pattern);
    }
}
