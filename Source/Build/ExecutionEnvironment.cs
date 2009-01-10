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
		
        public IProcess Start(string fileName, string arguments)
        {
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = fileName;
            startInfo.Arguments = arguments;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            return ProcessAdapter.Start(startInfo);
        }
		
		public TextWriter StandardOut { get { return standardOut; } }
		public bool IsMono { get { return Type.GetType("Mono.Runtime") != null; } }
    }
}
