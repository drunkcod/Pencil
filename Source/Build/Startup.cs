namespace Pencil.Build
{
	using System;
	using System.Diagnostics;
	using System.Collections.Generic;
	using System.Reflection;
	using Microsoft.CSharp;
	using Pencil.IO;

	static class Startup
	{
		static int Main(string[] args)
		{
			var logger = new Logger(Console.Out);
			var codeProvider = new CSharpCodeProvider(new Dictionary<string,string>(){{"CompilerVersion", "v3.5"}});
			var compiler = new ProjectCompiler(logger, codeProvider,
				new Path(Assembly.GetExecutingAssembly().Location),
				new Path(Assembly.GetAssembly(typeof(Path)).Location));
			var program = new Program(logger, compiler.ProjectFromFile);
			program.ShowLogo();
			var stopwatch = Stopwatch.StartNew();
            try
            {
				return program.Run(args);
            }
            finally
            {
                stopwatch.Stop();
                logger.Write("Total time: {0} seconds.", stopwatch.Elapsed.Seconds);
            }
		}
	}
}