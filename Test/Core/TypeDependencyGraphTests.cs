namespace Pencil.Test.Core
{
	using NUnit.Framework;
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

		class MyType
		{
			public System.DateTime[] Foo(){ return null; }
		}

		[Test]
		public void Should_use_element_type_for_arrays()
		{
			var digraph = new DirectedGraph();
			var graph = new TypeDependencyGraph(digraph);
			graph.Add(Type.Wrap(typeof(MyType)));
			Assert.That(digraph.Nodes.Map(x => x.Label).ToList(), Is.EquivalentTo(new[]{ "MyType", "DateTime" }));
		}

        class TypeWithArray
        {
            System.DateTime[] dates = new System.DateTime[]{ System.DateTime.Now };
            public System.DateTime Today() { return dates[0]; }
        }
        [Test]
        public void Wont_add_duplicate_edge_for_type_and_type_array()
        {
            var digraph = new DirectedGraph();
            var graph = new TypeDependencyGraph(digraph);
            graph.Add(Type.Wrap(typeof(TypeWithArray)));
            Assert.That(digraph.Edges.Map(x => x.ToString()).ToList(), Is.EquivalentTo(new[] { "0->1" }));
        }

		class EmptyType {}

		[Test]
		public void Should_raise_NodeCreated_when_adding_node()
		{
            var digraph = new DirectedGraph();
            var graph = new TypeDependencyGraph(digraph);
			var nodeCreatedRaised = false;
			graph.NodeCreated += (sender, e) =>
			{
				Assert.AreSame(sender, graph);
				e.Item.Equals(typeof(EmptyType)).ShouldBe(true);
				nodeCreatedRaised = true;
			};
            graph.Add(Type.Wrap(typeof(EmptyType)));
			nodeCreatedRaised.ShouldBe(true);
		}

		[Test]
		public void Should_create_distinct_nodes_for_types_with_same_name()
		{
			var digraph = new DirectedGraph();
            var graph = new TypeDependencyGraph(digraph);
			graph.Add(new TypeStub("NamespaceOne", "Foo"));
			graph.Add(new TypeStub("NamespaceTwo", "Foo"));

			digraph.Nodes.Count().ShouldEqual(2);
		}
	}
}