namespace Pencil.Build
{
	using FXPath = System.IO.Path;
	public class Path
	{
		string path;

		public Path(string path){ this.path = path; }		
		public Path Combine(string child){ return new Path(FXPath.Combine(path, child)); }
		public Path GetDirectory(){ return new Path(GetDirectoryName()); }
		
		public string GetDirectoryName(){ return FXPath.GetDirectoryName(path); }
		public string GetFileName(){ return FXPath.GetFileName(path); }

		public static Path operator+(Path path, string more){ return path.Combine(more); } 
		
		public override string ToString(){ return path; }
	}
}