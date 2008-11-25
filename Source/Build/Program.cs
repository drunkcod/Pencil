namespace Pencil.Build
{
	using System;
	using System.Collections.Generic;
	using System.CodeDom.Compiler;
	using System.Reflection;
	using System.Diagnostics;
	using System.IO;
	using System.Text;
	using Microsoft.CSharp;

	public static class Program
	{
		const int Success = 0;
		const int Failiure = 1;

		public static int Main(string[] args)
		{
			ShowLogo(Console.Out);

			var stopwatch = Stopwatch.StartNew();
			try
			{
				var project = CompileBuildFile(args[0]);
				var target = args[1];
				if(!project.HasTarget(target))
				{
					Console.WriteLine("Target \"{0}\" not found.", target);
					return Failiure;
				}
				Console.WriteLine("{0}:", target);
				project.Run(target);
				Console.WriteLine();
				Console.WriteLine("BUILD SUCCEEDED");
				return Success;
			}
			catch
			{
				Console.WriteLine("BUILD FAILED");
				return Failiure;
			}
			finally
			{
				stopwatch.Stop();
				Console.WriteLine();
				Console.WriteLine("Total time: {0} seconds.", stopwatch.Elapsed.Seconds);
			}
		}

		class BuildFailedException : Exception
		{
			public BuildFailedException(string message): base(message){}
		}

		static void ShowLogo(TextWriter writer)
		{

			writer.WriteLine("Pencil.Build 0.0");
			writer.WriteLine("Copyright (C) 2008 Torbjörn Gyllebring");
			writer.WriteLine();
		}

		static Project CompileBuildFile(string path)
		{
			var codeProvider = new CSharpCodeProvider(new Dictionary<string,string>(){{"CompilerVersion", "v3.5"}});
            var options = new CompilerParameters();
            options.GenerateExecutable = false;
            options.GenerateInMemory = true;
            options.ReferencedAssemblies.Add("Tools\\Pencil.Build.exe");

            var result = codeProvider.CompileAssemblyFromFile(options, path);
			if(result.NativeCompilerReturnValue == 0)
				foreach(var t in result.CompiledAssembly.GetTypes())
					if(typeof(Project).IsAssignableFrom(t))
						return (Project)t.GetConstructor(Type.EmptyTypes).Invoke(null);
			var message = new StringBuilder();
			foreach(var s in result.Output)
				message.AppendLine(s);
			throw new BuildFailedException(message.ToString());
		}
	}
}