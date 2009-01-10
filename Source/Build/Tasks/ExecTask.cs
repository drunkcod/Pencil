namespace Pencil.Build.Tasks
{
	public class ExecTask : ExecTaskBase
	{
		Path program;
		
        public ExecTask() : base(new ExecutionEnvironment()) { }
		new public Path Program { get { return base.Program; } set { program = value; } }
		public string CommandLine { get; set; }

		protected override Path GetProgramCore(){ return program; }
		protected override string GetArgumentsCore(){ return CommandLine; }

	}
}