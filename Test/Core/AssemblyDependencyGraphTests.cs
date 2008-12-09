namespace Pencil.Test.Core
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using NUnit.Framework.SyntaxHelpers;
    using Pencil.Core;

	class GraphBuilderStub
	{
		public Action<string> AddNodeHandler;

		public void AddNode(string label){ AddNodeHandler(label); }
	}

	class AssemblyStub : IAssembly
	{
		string name;

		public Func<IEnumerable<IAssembly>> GetReferencedAssembliesHandler = () => new IAssembly[0];

		public AssemblyStub(string name)
		{
			this.name = name;
		}

		public string Name { get { return name; } }
		public IEnumerable<IAssembly> ReferencedAssemblies { get { return GetReferencedAssembliesHandler(); } }
        public IEnumerable<IModule> Modules { get { throw new NotImplementedException(); } }

	}

	class AssemblyDependencyGraph
	{
		GraphBuilderStub graph;

		public AssemblyDependencyGraph(GraphBuilderStub graph)
		{
			this.graph = graph;
		}

		public void Add(IAssembly assembly)
		{
			graph.AddNode(assembly.Name);
			foreach(var item in assembly.ReferencedAssemblies)
				graph.AddNode(item.Name);
		}

        public DirectedGraph Result()
        {
            return new DirectedGraph();
        }
	}

	[TestFixture]
	public class AssemblyDependencyGraphTests
	{
		[Test]
		public void Should_add_node_with_assembly_name_as_label()
		{
			var nodes = new List<string>();
			var builder = new GraphBuilderStub();
			builder.AddNodeHandler = nodes.Add;
			var graph = new AssemblyDependencyGraph(builder);
			var assembly = new AssemblyStub("MyAssembly");
			graph.Add(assembly);

			Assert.That(nodes, Is.EquivalentTo(new[]{ assembly.Name}));
		}
		[Test]
		public void Should_add_referenced_assemblies()
		{
			var nodes = new List<string>();
			var builder = new GraphBuilderStub();
			builder.AddNodeHandler = nodes.Add;
			var graph = new AssemblyDependencyGraph(builder);
			var root = new AssemblyStub("RootAssembly");
			var child1 = new AssemblyStub("System");
			var child2 = new AssemblyStub("System.Xml");

			root.GetReferencedAssembliesHandler = () => new []{ child1, child2};

			graph.Add(root);

			Assert.That(nodes, Is.EquivalentTo(new[]{ root.Name, child1.Name, child2.Name}));
		}
        [Test]
        public void Should_add_edges_from_dependant_to_dependee()
        {
            var builder = new GraphBuilderStub();
            var graph = new AssemblyDependencyGraph(builder);
            var root = new AssemblyStub("RootAssembly");
            var child1 = new AssemblyStub("System");
            var child2 = new AssemblyStub("System.Xml");

            root.GetReferencedAssembliesHandler = () => new[] { child1, child2 };

            graph.Add(root);

            Assert.That(graph.Result().Edges.Map(x => x.ToString()), Is.EquivalentTo(new[] { "0->1", "0->2"}));
        }
	}
}