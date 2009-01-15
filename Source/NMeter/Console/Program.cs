namespace Pencil.NMeter.Console
{
	using System;
	using System.Collections.Generic;
	using Pencil.Core;
	using Pencil.NMeter;
	using Pencil.IO;

	class Program
	{
		static void Main(string[] args)
		{
			var config = XmlConfiguration.FromFile(args[0]).Read<ProjectConfiguration>();

			var loader = new StaticAssemblyLoader();
			var assemblies = new List<IAssembly>();
			var fs = new FileSystem();
			fs.GetFiles(new Pencil.IO.Path(config.BinPath), "*.*")
			.ForEach(IsAssembly, path =>
			{
				var assembly = AssemblyLoader.LoadFrom(path.ToString());
				loader.Register(assembly);
				assemblies.Add(assembly);
			});
			var digraph = new DirectedGraph();
			var dependencies = new AssemblyDependencyGraph(digraph, loader, 
				IgnoreFilter.From(config.IgnoreAssemblies));
			assemblies.ForEach(dependencies.Add);

			new Pipe("dot", "-Tpng").Transfer(
				stream => new DotBuilder(stream).Write(digraph),
				stream => fs.WriteFile(new Path("output.png"), stream));
		}

		static bool IsAssembly(Pencil.IO.Path path)
		{
			var ext = path.GetExtension();
			return ext == ".dll" || ext == ".exe";
		}
	}
}