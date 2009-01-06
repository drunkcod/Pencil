namespace Pencil.Build
{
	using FXPath = System.IO.Path;
	public class Path
	{
		string path;

		public Path(string path){ this.path = path; }		
		public Path Combine(string child){ return new Path(FXPath.Combine(path, child)); }

		public override string ToString(){ return path; }
	}
}