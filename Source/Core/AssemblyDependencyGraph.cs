namespace Pencil.Core
{
	using System;
	using System.Reflection;
    using System.Collections.Generic;

    public class AssemblyDependencyGraph : DependencyGraph<IAssembly>
    {
		IAssemblyLoader loader;
		IFilter<AssemblyName> filter;

        public AssemblyDependencyGraph(DirectedGraph graph, IAssemblyLoader loader): this(graph, loader, x => true){}

        public AssemblyDependencyGraph(DirectedGraph graph, IAssemblyLoader loader, Predicate<AssemblyName> filter):
			this(graph, loader, Filter.From(filter)){}

		public AssemblyDependencyGraph(DirectedGraph graph, IAssemblyLoader loader, IFilter<AssemblyName> filter): base(graph)
        {
			this.loader = loader;
			this.filter = filter;
        }

		protected override bool ShouldAdd(IAssembly item)
		{
            return base.ShouldAdd(item) && Include(item.Name);
		}

		protected override string GetLabel(IAssembly item){ return item.Name.Name; }

		protected override IEnumerable<IAssembly> GetDependencies(IAssembly item)
		{
            foreach(var reference in item.ReferencedAssemblies)
				if(Include(reference))
                   yield return loader.Load(reference);
		}


		bool Include(AssemblyName assemblyName){ return filter.Include(assemblyName); }
    }
}