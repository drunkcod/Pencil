namespace Pencil.Core
{
	using System;
	using System.Reflection;
    using System.Collections.Generic;

    public class AssemblyDependencyGraph
    {
        DirectedGraph graph;
		IAssemblyLoader loader;
        Dictionary<string, Node> assemblies = new Dictionary<string, Node>();
		Predicate<AssemblyName> filter;

        public AssemblyDependencyGraph(DirectedGraph graph, IAssemblyLoader loader): this(graph, loader, x => true){}

        public AssemblyDependencyGraph(DirectedGraph graph, IAssemblyLoader loader, Predicate<AssemblyName> filter)
        {
            this.graph = graph;
			this.loader = loader;
			this.filter = filter;
        }

        public void Add(IAssembly assembly)
        {
			if(!AlreadyAdded(assembly))
				AddChildren(GetOrCreateNoLoad(assembly.Name), assembly);
        }

		bool AlreadyAdded(IAssembly assembly)
		{
			return assemblies.ContainsKey(assembly.Name.Name);
		}

        Node GetOrCreate(AssemblyName assemblyName)
        {
            Node node;
            if(CreateNode(assemblyName, out node))
				AddChildren(node, loader.Load(assemblyName));
            return node;
        }

		void AddChildren(Node current, IAssembly assembly)
		{
            foreach(var item in assembly.ReferencedAssemblies)
			{
				if(filter(item))
	                current.ConnectTo(GetOrCreate(item));
			}
		}

		Node GetOrCreateNoLoad(AssemblyName assemblyName)
        {
            Node node;
			CreateNode(assemblyName, out node);
            return node;
        }

		bool CreateNode(AssemblyName assemblyName, out Node node)
		{
            if(!assemblies.TryGetValue(assemblyName.Name, out node))
            {
                node = graph.AddNode(assemblyName.Name);
                assemblies.Add(assemblyName.Name, node);
				return true;
            }
            return false;
		}
    }
}