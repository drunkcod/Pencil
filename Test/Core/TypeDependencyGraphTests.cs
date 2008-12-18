namespace Pencil.Test.Core
{
	using NUnit.Framework;
	using NUnit.Framework.SyntaxHelpers;
	using Pencil.Core;
	using Pencil.Test.Stubs;

	[TestFixture]
	public class TypeDependencyGraphTests
	{
		[Test]
		public void Add_should_create_node_for_type()
		{
			var digraph = new DirectedGraph();
			var graph = new TypeDependencyGraph(digraph);
			graph.Add(new TypeStub("MyType"));

			Assert.That(digraph.Nodes.Map(x => x.Label).ToList(), Is.EquivalentTo(new[]{ "MyType" }));
		}
		[Test]
		public void Add_should_create_nodes_for_dependencies()
		{
			var digraph = new DirectedGraph();
			var graph = new TypeDependencyGraph(digraph);
			graph.Add(new TypeStub("MyType"){ GetDependsOnHandler = () => new[]{new TypeStub("DateTime") } });

			Assert.That(digraph.Nodes.Map(x => x.Label).ToList(), Is.EquivalentTo(new[]{ "MyType", "DateTime" }));
		}
		[Test]
		public void Add_should_create_edges_between_dependencies()
		{
			var digraph = new DirectedGraph();
			var graph = new TypeDependencyGraph(digraph);
			graph.Add(new TypeStub("MyType"){ GetDependsOnHandler = () => new[]
			{
				new TypeStub("DateTime"), new TypeStub("Object")
			} });

            Assert.That(digraph.Edges.Map(x => x.ToString()).ToList(), Is.EquivalentTo(new[] { "0->1", "0->2" }));
		}
		[Test]
		public void Should_ignore_generated_types()
		{
			var digraph = new DirectedGraph();
			var graph = new TypeDependencyGraph(digraph);
			graph.Add(new TypeStub("Generated"){ GetIsGeneratedHandler = () => true });

			digraph.Nodes.ShouldBeEmpty();
		}
		[Test]
		public void Should_ignore_generated_dependent_on_types()
		{
			var digraph = new DirectedGraph();
			var graph = new TypeDependencyGraph(digraph);
			graph.Add(new TypeStub("MyType"){ GetDependsOnHandler = () => new[]
			{
				new TypeStub("Generated"){ GetIsGeneratedHandler = () => true }
			}});

			Assert.That(digraph.Nodes.Map(x => x.Label).ToList(), Is.EquivalentTo(new[]{ "MyType" }));
		}
		[Test]
		public void Should_support_filtering()
		{
			var digraph = new DirectedGraph();
			var graph = new TypeDependencyGraph(digraph, Filter.From<IType>(x => false));
			graph.Add(new TypeStub("MyType"));

			digraph.Nodes.ShouldBeEmpty();
		}
	}
}