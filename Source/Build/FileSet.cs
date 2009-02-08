namespace Pencil.Build
{
	using System;
	using System.Collections.Generic;
	using Pencil.IO;

	public class FileSet
	{
	    readonly IFileSystem fileSystem;
		readonly List<Path> items = new List<Path>();

        public FileSet(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }
        
        public FileSet(): this(new FileSystem()){}
        
        public IEnumerable<Path> Items 
        { 
            get 
            {
                foreach(var item in items)
                    if(item.ToString().Contains("*"))
                    {
                        var files = fileSystem.GetFiles(item.GetDirectory(), item.GetFileName());
                        foreach(var file in files)
                            yield return file;
                    }
                    else 
                        yield return item;              
            } 
        }

		public void Add(Path path){ items.Add(path); }

		public void CopyTo(Path destination)
		{
			fileSystem.EnsureDirectory(destination);
			foreach(var file in Items)
            {
                var target = destination + file.GetFileName();
                if(fileSystem.FileExists(target) || !fileSystem.FileExists(file))
                    return;
                fileSystem.CopyFile(file, target, true);
            }
		}
		
		public bool ChangedAfter(DateTime timestamp)
		{
			foreach(var item in Items)
				if(fileSystem.GetLastWriteTime(item) > timestamp)
					return true;
		    return false;
		}

		public IEnumerator<Path> GetEnumerator(){ return items.GetEnumerator(); }
	}
}
