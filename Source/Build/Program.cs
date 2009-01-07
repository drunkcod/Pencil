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
			var logger = new Logger(Console.Out);
			var program = new Program(logger);
			program.ShowLogo();
			var stopwatch = Stopwatch.StartNew();
            try
            {
				var compiler = new ProjectCompiler(logger);
				var project = compiler.ProjectFromFile(args[0]);
				project.FileSystem = new FileSystem();
				project.ExecutionEnvironment = new ExecutionEnvironment();
                return program.BuildTarget(project, args[1]);
            }
            finally
            {
                stopwatch.Stop();
                Console.WriteLine("Total time: {0} seconds.", stopwatch.Elapsed.Seconds);
            }
		}

		Logger output;

 		public Program(Logger output)
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
                    output.Write("BUILD SUCCEEDED");
                    return Success;
                }
                else
                    output.Write("Target \"{0}\" not found.", target);
            }
            catch(TargetFailedException e)
            {
				var error = e.InnerException;
                output.Write("BUILD FAILED - {0}", error.Message);
				output.Write(error.StackTrace);
            }
            return Failiure;
        }

		void ShowLogo()
		{
			output.Write("Pencil.Build {0}", GetType().Assembly.GetName().Version);
			output.Write("Copyright (C) 2008 Torbjörn Gyllebring");
		}
	}
}