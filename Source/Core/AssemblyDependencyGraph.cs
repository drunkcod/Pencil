namespace Pencil.Core
{
	using System.Reflection;
    using System.Collections.Generic;

    public class AssemblyDependencyGraph
    {
        DirectedGraph graph;
        Dictionary<AssemblyName, Node> assemblies = new Dictionary<AssemblyName, Node>();

        public AssemblyDependencyGraph(DirectedGraph graph)
        {
            this.graph = graph;
        }

        public void Add(IAssembly assembly)
        {
            var current = GetOrCreate(assembly.Name);
            foreach(var item in assembly.ReferencedAssemblies)
                current.ConnectTo(GetOrCreate(item));
        }

        Node GetOrCreate(AssemblyName assemblyName)
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