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
			if(ShouldAdd(assembly))
                AddChildren(CreateNode(assembly.Name).Item, assembly);
        }

		bool ShouldAdd(IAssembly assembly)
		{
            var name = assembly.Name;
            return !assemblies.ContainsKey(name.Name) && filter(name);
		}

        Node GetOrCreate(AssemblyName assemblyName)
        {
            var node = CreateNode(assemblyName);
            if(node.Created)
				AddChildren(node.Item, loader.Load(assemblyName));
            return node.Item;
        }

		void AddChildren(Node current, IAssembly assembly)
		{
            foreach(var item in assembly.ReferencedAssemblies)
			{
				if(filter(item))
                    current.ConnectTo(GetOrCreate(item));
			}
		}

        struct CreateResult
        {
            public bool Created;
            public Node Item;
        }

		CreateResult CreateNode(AssemblyName assemblyName)
		{
            var result = new CreateResult();
            result.Created = !assemblies.TryGetValue(assemblyName.Name, out result.Item);
            if(result.Created)
            {
                result.Item = graph.AddNode(assemblyName.Name);
                assemblies.Add(assemblyName.Name, result.Item);
            }
            return result;
		}
    }
}