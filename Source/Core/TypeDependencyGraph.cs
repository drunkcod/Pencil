namespace Pencil.Core
{
	using System.Collections.Generic;

	public class TypeDependencyGraph : DependencyGraph<IType>
	{
		public TypeDependencyGraph(DirectedGraph graph) : base(graph)
		{}

		protected override bool Recursive { get { return false; } }
		protected override string GetLabel(IType item){ return item.Name; }
		protected override IEnumerable<IType> GetDependencies(IType item){ return item.DependsOn; }
	}
}