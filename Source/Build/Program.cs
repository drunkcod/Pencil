namespace Pencil.Build
{
	using System;
	using System.CodeDom.Compiler;
	using System.Collections.Generic;
	using Pencil.IO;
	
	public class Program
	{
		public const int Success = 0;
		public const int Failiure = 1;

		readonly Logger output;
		readonly Converter<string,IProject> compiler;

 		public Program(Logger output, Converter<string,IProject> compiler)
		{
			this.output = output;
			this.compiler = compiler;
		}

		public int Run(string[] args)
		{
			var project = compiler(args[0]);
			project.Register<IFileSystem>(new FileSystem());
			project.Register<IExecutionEnvironment>(new ExecutionEnvironment(output.Target));
			for(int i = 1; i < args.Length; ++i)
				if(BuildTarget(project, args[i]) != Success)
					return Failiure;			
            output.Write("BUILD SUCCEEDED");
			return Success;
		}

		public int BuildTarget(IProject project, string target)
		{
            try
            {
                if(project.HasTarget(target))
                {
                    project.Run(target);
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

		public void ShowLogo()
		{
			output.Write("Pencil.Build version {0}", GetType().Assembly.GetName().Version);
			output.Write("Copyright (C) 2008 Torbjörn Gyllebring");
		}
	}
}