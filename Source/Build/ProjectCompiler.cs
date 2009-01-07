namespace Pencil.Build
{
	using System;
	using System.Collections.Generic;
	using System.CodeDom.Compiler;
	using Microsoft.CSharp;

	class ProjectCompiler
	{
		readonly CodeDomProvider codeProvider = new CSharpCodeProvider(new Dictionary<string,string>(){{"CompilerVersion", "v3.5"}});

		public Project ProjectFromFile(string path)
		{
			var result = codeProvider.CompileAssemblyFromFile(GetCompilerParameters(), path);
			if(result.NativeCompilerReturnValue == 0)
				return GetProject(result.CompiledAssembly.GetTypes());
			throw new CompilationFailedException(result);
		}

		static CompilerParameters GetCompilerParameters()
		{
			var options = new CompilerParameters();
			options.GenerateExecutable = false;
			options.GenerateInMemory = true;
			options.ReferencedAssemblies.Add(new Path("Tools").Combine("Pencil.Build.exe").ToString());
			return options;
		}

		static Project GetProject(Type[] types)
		{
			foreach(var item in types)
				if(typeof(Project).IsAssignableFrom(item))
				{
					var project = item.GetConstructor(Type.EmptyTypes).Invoke(null) as Project;
					project.logger = new Logger(Console.Out);
					return project;
				}
			throw new InvalidOperationException(string.Format("{0} does not contain any Project."));
		}
	}
}