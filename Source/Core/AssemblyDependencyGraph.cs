namespace Pencil.Core
{
	using System.Reflection;
    using System.Collections.Generic;

    public class AssemblyDependencyGraph
    {
        DirectedGraph graph;
		IAssemblyLoader loader;
        Dictionary<AssemblyName, Node> assemblies = new Dictionary<AssemblyName, Node>();

        public AssemblyDependencyGraph(DirectedGraph graph, IAssemblyLoader loader)
        {
            this.graph = graph;
			this.loader = loader;
        }

        public void Add(IAssembly assembly)
        {
            var current = GetOrCreateNoLoad(assembly.Name);
            foreach(var item in assembly.ReferencedAssemblies)
                current.ConnectTo(GetOrCreate(item));
        }

        Node GetOrCreate(AssemblyName assemblyName)
        {
            Node node;
            if(!assemblies.TryGetValue(assemblyName, out node))
            {
                node = graph.AddNode(assemblyName.Name);
				loader.Load(assemblyName);
                assemblies.Add(assemblyName, node);
            }
            return node;
        }

		Node GetOrCreateNoLoad(AssemblyName assemblyName)
        {
            Node node;
            if(!assemblies.TryGetValue(assemblyName, out node))
            {
                node = graph.AddNode(assemblyName.Name);
                assemblies.Add(assemblyName, node);
            }
            return node;
        }
    }
}