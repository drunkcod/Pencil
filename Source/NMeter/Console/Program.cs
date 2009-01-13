namespace Pencil.NMeter.Console
{
	using System;
	using System.Collections.Generic;
	using System.IO;
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
			Directory.GetFiles(config.BinPath, "*.*")
			.ForEach(IsAssembly, path =>
			{
				var assembly = AssemblyLoader.LoadFrom(path);
				loader.Register(assembly);
				assemblies.Add(assembly);
			});
			var digraph = new DirectedGraph();
			var dependencies = new AssemblyDependencyGraph(digraph, loader, IgnoreFilter.From(config.IgnoreAssemblies));
			assemblies.ForEach(dependencies.Add);

			new Pipe("dot", "-Tgif").Transfer(
				stream => new DotBuilder(stream).Write(digraph),
				stream =>
				{
					var output = File.OpenWrite("output.gif");
					var buffer = new byte[4096];
					for(int b; (b = stream.Read(buffer, 0, buffer.Length)) != 0;)
						output.Write(buffer, 0, b);
				});
		}

		static bool IsAssembly(string path)
		{
			var ext = Path.GetExtension(path);
			return ext == ".dll" || ext == ".exe";
		}
	}
}