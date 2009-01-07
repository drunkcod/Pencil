namespace Pencil.Build
{
	using System;
	using System.Diagnostics;
	using System.IO;

	public class Program
	{
		public const int Success = 0;
		public const int Failiure = 1;

		static int Main(string[] args)
		{
			var program = new Program(Console.Out);
			program.ShowLogo();
			var stopwatch = Stopwatch.StartNew();
            try
            {
				var compiler = new ProjectCompiler();
                return program.BuildTarget(compiler.ProjectFromFile(args[0]), args[1]);
            }
            finally
            {
                stopwatch.Stop();
                Console.WriteLine();
                Console.WriteLine("Total time: {0} seconds.", stopwatch.Elapsed.Seconds);
            }
		}

		TextWriter output;

 		public Program(TextWriter output)
		{
			this.output = output;
		}

		public int BuildTarget(IProject project, string target)
		{
            try
            {
                if(project.HasTarget(target))
                {
                    project.Run(target);
                    output.WriteLine();
                    output.WriteLine("BUILD SUCCEEDED");
                    return Success;
                }
                else
                    output.WriteLine("Target \"{0}\" not found.", target);
            }
            catch(TargetFailedException e)
            {
				var error = e.InnerException;
                output.WriteLine("BUILD FAILED - {0}", error.Message);
				output.WriteLine(error.StackTrace);
            }
            return Failiure;
        }

		void ShowLogo()
		{
			output.WriteLine("Pencil.Build {0}", GetType().Assembly.GetName().Version);
			output.WriteLine("Copyright (C) 2008 Torbjörn Gyllebring");
			output.WriteLine();
		}
	}
}