namespace Pencil.Build.Tasks
{
	using System;
	using System.Diagnostics;

	public abstract class ExecTaskBase
	{
        IExecutionEnvironment executionEnvironment;

        protected ExecTaskBase(IExecutionEnvironment executionEnvironment)
        {
            this.executionEnvironment = executionEnvironment;
        }

		public void Execute()
		{
            var task = executionEnvironment.Start(GetProgramCore(), GetArgumentsCore());
			while(!task.HasExited)
				task.StandardOutput.CopyTo(Console.Out);
			if(task.ExitCode != 0)
				throw new Exception();
		}

		protected abstract string GetProgramCore();
		protected abstract string GetArgumentsCore();
	}
}