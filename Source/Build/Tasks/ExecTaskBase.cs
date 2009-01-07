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
            var task = executionEnvironment.Start(GetProgramCore().ToString(), GetArgumentsCore());
			while(!task.HasExited)
				task.StandardOutput.CopyTo(executionEnvironment.StandardOut);
			if(task.ExitCode != 0)
				throw new Exception();
		}

		protected abstract Path GetProgramCore();
		protected abstract string GetArgumentsCore();
	}
}