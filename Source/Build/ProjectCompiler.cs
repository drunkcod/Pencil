namespace Pencil.Build
{
	using System;
	using System.Collections.Generic;
	using System.CodeDom.Compiler;
	using Pencil.Core;
	using Pencil.IO;

	class ProjectCompiler
	{
		readonly CodeDomProvider codeProvider;
		readonly Logger logger;
		readonly IEnumerable<Path> referencedAssemblies;

		public ProjectCompiler(Logger logger, CodeDomProvider codeProvider, 
			params Path[] referencedAssemblies)
		{
			this.logger = logger;
			this.codeProvider = codeProvider;
			this.referencedAssemblies = referencedAssemblies;
		}

		public IProject ProjectFromFile(string path)
		{
			var result = codeProvider.CompileAssemblyFromFile(GetCompilerParameters(), path);
			if(result.NativeCompilerReturnValue == 0)
				return GetProject(result.CompiledAssembly.GetTypes());
			throw new CompilationFailedException(result);
		}

		CompilerParameters GetCompilerParameters()
		{
			var options = new CompilerParameters();
			options.GenerateExecutable = false;
			options.GenerateInMemory = true;
			referencedAssemblies.ForEach(x => options.ReferencedAssemblies.Add(x.ToString()));
			return options;
		}

		IProject GetProject(System.Type[] types)
		{
			foreach(var item in types)
				if(typeof(IProject).IsAssignableFrom(item))
				{
					var project = item.GetConstructor(System.Type.EmptyTypes).Invoke(null) as Project;
					project.logger = logger;
					return project;
				}
			throw new InvalidOperationException(string.Format("{0} does not contain any Project."));
		}
	}
}