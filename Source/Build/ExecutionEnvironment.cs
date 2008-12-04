namespace Pencil.Build
{
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
    }
}
