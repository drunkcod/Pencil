namespace Pencil.Core
{
	using System.Collections.Generic;

	public class TypeDependencyGraph : DependencyGraph<IType>
	{
		IFilter<IType> filter;

		public TypeDependencyGraph(DirectedGraph graph) : this(graph, new NullFilter<IType>()){}
		public TypeDependencyGraph(DirectedGraph graph, IFilter<IType> filter) : base(graph)
		{
			this.filter = filter;
		}

		protected override bool Recursive { get { return false; } }
		protected override string GetLabel(IType item){ return item.Name; }
		protected override IEnumerable<IType> GetDependencies(IType item){ return item.DependsOn; }
		protected override bool ShouldAddCore(IType item){ return !item.IsGenerated && filter.Include(item); }
	}
}