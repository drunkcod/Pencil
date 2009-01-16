namespace Pencil.Build
{
	using System;
	using System.IO;
    using System.Diagnostics;

    sealed class ExecutionEnvironment : IExecutionEnvironment
    {
		readonly TextWriter standardOut;

		public ExecutionEnvironment(TextWriter standardOut)
		{
			this.standardOut = standardOut;
		}
		
        public void Run(string fileName, string arguments, Action<IProcess> processHandler)
        {
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = fileName;
            startInfo.Arguments = arguments;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            using(var process = Process.Start(startInfo))
            	 processHandler(new ProcessAdapter(process));
        }
		
		public TextWriter StandardOut { get { return standardOut; } }
		public bool IsMono { get { return Type.GetType("Mono.Runtime") != null; } }
    }
}
