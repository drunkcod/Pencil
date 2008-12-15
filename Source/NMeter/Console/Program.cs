namespace Pencil.NMeter.Console
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using Pencil.Core;
	using Pencil.NMeter;

	class Program
	{
		static void Main(string[] args)
		{
			var config = ProjectConfiguration.FromFile(args[0]);

			var loader = new StaticAssemblyLoader();
			var assemblies = new List<IAssembly>();
			foreach(var path in Directory.GetFiles(config.BinPath, "*.*"))
				if(IsAssembly(path))
				{
					var assembly = AssemblyLoader.LoadFrom(path);
					loader.Register(assembly);
					assemblies.Add(assembly);
				}

			var digraph = new DirectedGraph();
			var dependencies = new AssemblyDependencyGraph(digraph, loader, IgnoreFilter.From(config.IgnoreAssemblies));
			assemblies.ForEach(dependencies.Add);
			Console.WriteLine(new DotBuilder().ToString(digraph));
			return;
		}

		static bool IsAssembly(string path)
		{
			var ext = Path.GetExtension(path);
			return ext == ".dll" || ext == ".exe";
		}
	}
}