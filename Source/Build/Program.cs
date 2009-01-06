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
                return program.BuildTarget(ProjectFromFile(args[0]), args[1]);
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
            catch(TargetInvocationException e)
            {
                output.WriteLine("BUILD FAILED - {0}", e.InnerException.Message);
				output.WriteLine(e.InnerException.StackTrace);
            }
            return Failiure;
        }

		class CompilationFailedException : Exception
		{
			public CompilationFailedException(CompilerResults results): base(GetOutput(results)){}

            static string GetOutput(CompilerResults results)
            {
                var message = new StringBuilder();
                foreach(var s in results.Output)
                    message.AppendLine(s);
                return message.ToString();
            }
		}

		void ShowLogo()
		{
			output.WriteLine("Pencil.Build {0}", Assembly.GetExecutingAssembly().GetName().Version);
			output.WriteLine("Copyright (C) 2008 Torbjörn Gyllebring");
			output.WriteLine();
		}

		static Project ProjectFromFile(string path)
		{
			var codeProvider = new CSharpCodeProvider(new Dictionary<string,string>(){{"CompilerVersion", "v3.5"}});
            var options = new CompilerParameters();
            options.GenerateExecutable = false;
            options.GenerateInMemory = true;
            options.ReferencedAssemblies.Add(new Path("Tools").Combine("Pencil.Build.exe").ToString());

            var result = codeProvider.CompileAssemblyFromFile(options, path);
			if(result.NativeCompilerReturnValue != Success)
                throw new CompilationFailedException(result);

			foreach(var t in result.CompiledAssembly.GetTypes())
				if(typeof(Project).IsAssignableFrom(t))
				{
					var project = t.GetConstructor(Type.EmptyTypes).Invoke(null) as Project;
					project.logger = new Logger(Console.Out);
					return project;
				}

            throw new InvalidOperationException(string.Format("{0} does not contain any Project."));
		}
	}
}