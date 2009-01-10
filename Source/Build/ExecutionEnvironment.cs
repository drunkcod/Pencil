namespace Pencil.Build
{
	using System;
	using System.IO;
    using System.Diagnostics;

    sealed class ExecutionEnvironment : IExecutionEnvironment
    {
        public IProcess Start(string fileName, string arguments)
        {
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = fileName;
            startInfo.Arguments = arguments;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            return new ProcessAdapter(Process.Start(startInfo));
        }
		
		public TextWriter StandardOut { get { return Console.Out; } }
		public bool IsMono { get { return Type.GetType("Mono.Runtime") != null; } }
    }
}
