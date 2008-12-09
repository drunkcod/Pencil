namespace Pencil.Core
{
    using System.Collections.Generic;

    public class AssemblyDependencyGraph
    {
        DirectedGraph graph;
        Dictionary<IAssembly, Node> assemblies = new Dictionary<IAssembly, Node>();

        public AssemblyDependencyGraph(DirectedGraph graph)
        {
            this.graph = graph;
        }

        public void Add(IAssembly assembly)
        {
            var current = GetOrCreate(assembly);
            foreach(var item in assembly.ReferencedAssemblies)
                current.ConnectTo(GetOrCreate(item));
        }

        Node GetOrCreate(IAssembly assembly)
        {
            Node node;
            if(!assemblies.TryGetValue(assembly, out node))
            {
                node = graph.AddNode(assembly.Name);
                assemblies.Add(assembly, node);
            }
            return node;
        }
    }
}