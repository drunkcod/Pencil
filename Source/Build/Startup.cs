namespace Pencil.Build
{
	using System;
	using System.Diagnostics;
	using System.Collections.Generic;
	using Microsoft.CSharp;
	using Pencil.IO;
	using Pencil.Core;

	using Assembly = System.Reflection.Assembly;

	public static class Startup
	{
		static int Main(string[] args)
		{
			var logger = new Logger(Console.Out);
			var codeProvider = new CSharpCodeProvider(new Dictionary<string,string>(){{"CompilerVersion", "v3.5"}});
			var compiler = new ProjectCompiler(logger, codeProvider,
				GetReferencedAssemblies(args));
			var program = new Program(logger, compiler.ProjectFromFile);
			program.ShowLogo();
			var stopwatch = Stopwatch.StartNew();
            try
            {
				return program.Run(GetArguments(args));
            }
            finally
            {
                stopwatch.Stop();
                logger.Write("Total time: {0} seconds.", stopwatch.Elapsed.Seconds);
            }
		}

		public static IEnumerable<Path> GetReferencedAssemblies(string[] args)
		{
			yield return new Path(Assembly.GetExecutingAssembly().Location);
			yield return new Path(Assembly.GetAssembly(typeof(Path)).Location);
			foreach(var item in args)
				if("-r:".IsStartOf(item))
					yield return new Path(item.Substring("-r:".Length));
		}

		public static string[] GetArguments(string[] args)
		{
			return new List<string>(args.Filter(x => !x.StartsWith("-"))).ToArray();
		}
	}
}