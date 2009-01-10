namespace Pencil.Build.Tasks
{
	using System.Text;
	
	public class NUnitTask : ExecTaskBase
	{
		Path binPath = Path.Empty;
		bool shadowCopy = true;
		
		public NUnitTask(IExecutionEnvironment executionEnvironment): base(executionEnvironment){}

		public Path NUnitPath { get { return NUnitBinPath + "nunit-console.exe"; } }
		public Path NUnitBinPath { get { return binPath; } set { binPath = value; } }
		public Path Target { get; set; }
		public bool ShadowCopy { get { return shadowCopy; } set { shadowCopy = value; } }
		
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
			return args.ToString();
		}
	}
}