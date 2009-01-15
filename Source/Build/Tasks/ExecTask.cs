namespace Pencil.Build.Tasks
{
	using Pencil.IO;
	
	public class ExecTask : ExecTaskBase
	{
		Path program;
		
        public ExecTask(IExecutionEnvironment executionEnvironment) : base(executionEnvironment) { }
		new public Path Program { get { return base.Program; } set { program = value; } }
		public string CommandLine { get; set; }

		protected override Path GetProgramCore(){ return program; }
		protected override string GetArgumentsCore(){ return CommandLine; }
	}
}