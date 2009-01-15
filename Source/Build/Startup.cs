namespace Pencil.Build
{
	using System;
	using System.Diagnostics;
	using System.Collections.Generic;
	using Microsoft.CSharp;
	
	static class Startup
	{
		static int Main(string[] args)
		{
			var logger = new Logger(Console.Out);
			var program = new Program(logger, new CSharpCodeProvider(new Dictionary<string,string>(){{"CompilerVersion", "v3.5"}}));
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