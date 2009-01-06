namespace Pencil.Build.Tasks
{
	public class ExecTask : ExecTaskBase
	{
        public ExecTask() : base(new ExecutionEnvironment()) { }
		public Path Program { get; set; }
		public string CommandLine { get; set; }

		protected override Path GetProgramCore(){ return Program; }
		protected override string GetArgumentsCore(){ return CommandLine; }

	}
}