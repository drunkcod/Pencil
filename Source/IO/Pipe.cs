namespace Pencil.IO
{
	using System;
	using System.Diagnostics;
	using System.IO;

	public class Pipe
	{
		string program;
		string arguments;

		public Pipe(string program, string arguments)
		{
			this.program = program;
			this.arguments = arguments;
		}

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
			startInfo.RedirectStandardInput = true;
			startInfo.RedirectStandardOutput = true;
			startInfo.UseShellExecute = false;
			return Process.Start(startInfo);
		}
	}
}