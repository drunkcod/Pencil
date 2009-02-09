namespace Pencil.IO
{
	using System;
	using System.Diagnostics;
	using System.IO;

	public class Pipe
	{
		readonly string workingDirectory;
		readonly string program;
		readonly string arguments;

		public Pipe(string workingDirectory, string program, string arguments)
		{
			this.workingDirectory = workingDirectory;
			this.program = program;
			this.arguments = arguments;
		}
		public Pipe(string program, string arguments): this(string.Empty, program, arguments)
		{}

		public void Transfer(Action<TextWriter> gatherInput, Action<Stream> handleOutput)
		{
			using(var process = StartProcess())
			{
				using(var input = process.StandardInput)
					gatherInput(input);
				handleOutput(process.StandardOutput.BaseStream);
			}
		}

		Process StartProcess()
		{
			var startInfo = new ProcessStartInfo(program);
			startInfo.Arguments = arguments;
			startInfo.WorkingDirectory = workingDirectory;
			startInfo.RedirectStandardInput = true;
			startInfo.RedirectStandardOutput = true;
			startInfo.UseShellExecute = false;
			return Process.Start(startInfo);
		}
	}
}