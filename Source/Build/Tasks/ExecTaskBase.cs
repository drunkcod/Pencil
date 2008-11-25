namespace Pencil.Build.Tasks
{
	using System;
	using System.Diagnostics;

	public abstract class ExecTaskBase
	{

		public void Execute()
		{
			var startInfo = new ProcessStartInfo();
			startInfo.Arguments = GetArgumentsCore();
			startInfo.FileName = GetProgramCore();
			startInfo.UseShellExecute = false;
			startInfo.RedirectStandardOutput = true;
			var task = Process.Start(startInfo);
			while(!task.HasExited)
			{
				Console.WriteLine(task.StandardOutput.ReadToEnd());
			}
			if(task.ExitCode != 0)
			{
				throw new Exception();
			}
		}

		protected abstract string GetProgramCore();
		protected abstract string GetArgumentsCore();
	}
}