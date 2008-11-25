namespace Pencil.Build.Tasks
{
	public class ExecTask : ExecTaskBase
	{
		public string Program { get; set; }
		public string CommandLine { get; set; }

		protected override string GetProgramCore(){ return Program; }
		protected override string GetArgumentsCore(){ return CommandLine; }

	}
}