namespace Pencil.Build
{
	using System;
	using System.Collections.Generic;
	using System.CodeDom.Compiler;
	using Pencil.IO;

	class ProjectCompiler
	{
		readonly CodeDomProvider codeProvider;
		readonly Logger logger;

		public ProjectCompiler(Logger logger, CodeDomProvider codeProvider)
		{
			this.logger = logger;
			this.codeProvider = codeProvider;
		}

		public IProject ProjectFromFile(string path)
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

		IProject GetProject(Type[] types)
		{
			foreach(var item in types)
				if(typeof(IProject).IsAssignableFrom(item))
				{
					var project = item.GetConstructor(Type.EmptyTypes).Invoke(null) as Project;
					project.logger = logger;
					return project;
				}
			throw new InvalidOperationException(string.Format("{0} does not contain any Project."));
		}
	}
}