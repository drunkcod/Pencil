namespace Pencil.Build.Tasks
{
	using System;
	using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Pencil.IO;

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
			executionEnvironment.Run(Program.ToString(), GetArgumentsCore(), task =>
			{
				while(!task.HasExited)
					task.StandardOutput.CopyTo(executionEnvironment.StandardOut);
				if(task.ExitCode != 0)
					throw new Exception();
			});
		}

		protected bool IsRunningOnMono { get { return executionEnvironment.IsMono; } }
		protected Path RuntimeDirectory 
		{ 
			get { return new Path(RuntimeEnvironment.GetRuntimeDirectory()); } 
		}
		
		protected abstract Path GetProgramCore();
		protected abstract string GetArgumentsCore();
	}
}