namespace Pencil.Build
{
	using System;
	using System.Collections.Generic;
	
	public class FileSet
	{
		readonly List<Path> items = new List<Path>();

		public void Add(Path path){ items.Add(path); }

		public void CopyTo(IFileSystem fileSystem, Path destination)
		{
			fileSystem.EnsureDirectory(destination);
			items.ForEach(file =>
            {
                var target = destination + file.GetFileName();
                if(fileSystem.FileExists(target) || !fileSystem.FileExists(file))
                    return;
                fileSystem.CopyFile(file, target, true);
            });

		}

		public void ForEach(Action<Path> action){ items.ForEach(action); }
		public IEnumerator<Path> GetEnumerator(){ return items.GetEnumerator(); }
	}
}