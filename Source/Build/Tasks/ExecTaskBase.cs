namespace Pencil.Build.Tasks
{
	using System;
	using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Pencil.IO;

	public abstract class ExecTaskBase
	{
		IExecutionEnvironment platform;

		public Path Program { get { return GetProgramCore(); } }

        protected ExecTaskBase(IExecutionEnvironment platform)
        {
            this.platform = platform;
        }

		public void Execute()
		{
			platform.Run(Program.ToString(), GetArgumentsCore(), task =>
			{
				while(!task.HasExited)
					task.StandardOutput.CopyTo(platform.StandardOut);
				if(task.ExitCode != 0)
					throw new Exception();
			});
		}

		protected bool IsRunningOnMono { get { return platform.IsMono; } }
		protected Path RuntimeDirectory 
		{ 
			get { return new Path(RuntimeEnvironment.GetRuntimeDirectory()); } 
		}
		
		protected abstract Path GetProgramCore();
		protected abstract string GetArgumentsCore();
	}
}
