namespace Pencil.Build
{
    using System.Diagnostics;

    sealed class ProcessAdapter : IProcess
    {
        Process process;

		public static ProcessAdapter Start(ProcessStartInfo startInfo)
		{
			return new ProcessAdapter(Process.Start(startInfo));
		}
		
        ProcessAdapter(Process process)
        {
            this.process = process;
        }

        public bool HasExited { get { return process.HasExited; } }

        public int ExitCode { get { return process.ExitCode; } }

        public System.IO.TextReader StandardOutput
        {
            get { return process.StandardOutput; }
        }
    }
}
