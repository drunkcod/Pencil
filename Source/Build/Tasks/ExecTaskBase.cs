namespace Pencil.Build.Tasks
{
	using System;
	using System.Diagnostics;

	public abstract class ExecTaskBase
	{
		IExecutionEnvironment executionEnvironment;

		public Path Program { get { return GetProgramCore(); } }

        protected ExecTaskBase(IExecutionEnvironment executionEnvironment)
        {
            this.executionEnvironment = executionEnvironment;
        }

		public void Execute()
		{
            var task = executionEnvironment.Start(Program.ToString(), GetArgumentsCore());
			while(!task.HasExited)
				task.StandardOutput.CopyTo(executionEnvironment.StandardOut);
			if(task.ExitCode != 0)
				throw new Exception();
		}

		protected bool IsRunningOnMono { get { return executionEnvironment.IsMono; } }

		protected abstract Path GetProgramCore();
		protected abstract string GetArgumentsCore();
	}
}
