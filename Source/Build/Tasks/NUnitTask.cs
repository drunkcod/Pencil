namespace Pencil.Build.Tasks
{
	using System.Text;
	using Pencil.IO;
	
	public class NUnitTask : ExecTaskBase
	{
		Path binPath = Path.Empty;
		bool shadowCopy = true;
		bool showlogo = true;
		
		public NUnitTask(IExecutionEnvironment executionEnvironment): base(executionEnvironment){}

		public Path NUnitPath { get { return NUnitBinPath + "nunit-console.exe"; } }
		public Path NUnitBinPath { get { return binPath; } set { binPath = value; } }
		public Path Target { get; set; }
		public bool ShadowCopy { get { return shadowCopy; } set { shadowCopy = value; } }
		public bool ShowLogo { get { return showlogo; } set { showlogo = value; } }
		
		protected override Path GetProgramCore ()
		{
			return IsRunningOnMono ? new Path("mono"): NUnitPath;
		}

		protected override string GetArgumentsCore ()
		{
			var args = new StringBuilder(IsRunningOnMono ? NUnitPath.ToString() : string.Empty);
			args.AppendFormat(" {0}", Target);
			if(!ShadowCopy)
				args.AppendFormat(" {0}", "-noshadow");
			if(!ShowLogo)
				args.AppendFormat(" {0}", "-nologo");
			return args.ToString();
		}
	}
}