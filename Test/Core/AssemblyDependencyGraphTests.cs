namespace Pencil.Test.Core
{
    using System;
	using System.Reflection;
    using System.Collections.Generic;
    using NUnit.Framework;
    using NUnit.Framework.SyntaxHelpers;
    using Pencil.Core;
    using Pencil.Test.Stubs;

	[TestFixture]
	public class AssemblyDependencyGraphTests
	{
		AssemblyDependencyGraph NewDependencyGraph(DirectedGraph target)
		{
			return new AssemblyDependencyGraph(target, new AssemblyLoaderStub());
		}

		[Test]
		public void Should_add_node_with_assembly_name_as_label()
		{
			var digraph = new DirectedGraph();
			var graph = NewDependencyGraph(digraph);
			var assembly = new AssemblyStub("MyAssembly");
			graph.Add(assembly);

			Assert.That(digraph.Nodes.Map(x => x.Label).ToList(), Is.EquivalentTo(new[]{ assembly.Name.Name}));
		}
		[Test]
		public void Should_add_referenced_assemblies()
		{
            var digraph = new DirectedGraph();
            var graph = NewDependencyGraph(digraph);
			var root = new AssemblyStub("RootAssembly");
			var child1 = new AssemblyName("System");
			var child2 = new AssemblyName("System.Xml");

			root.GetReferencedAssembliesHandler = () => new []{ child1, child2};

			graph.Add(root);

            Assert.That(digraph.Nodes.Map(x => x.Label).ToList(), Is.EquivalentTo(new[] { root.Name.Name, child1.Name, child2.Name }));
		}
        [Test]
        public void Should_add_edges_from_dependant_to_dependee()
        {
            var digraph = new DirectedGraph();
            var graph = NewDependencyGraph(digraph);
            var root = new AssemblyStub("RootAssembly");
            var child1 = new AssemblyName("System");
            var child2 = new AssemblyName("System.Xml");

            root.GetReferencedAssembliesHandler = () => new[] { child1, child2 };

            graph.Add(root);

            Assert.That(digraph.Edges.Map(x => x.ToString()).ToList(), Is.EquivalentTo(new[] { "0->1", "0->2" }));
        }
        [Test]
        public void Should_not_add_same_assembly_twice()
        {
            var digraph = new DirectedGraph();
            var graph = NewDependencyGraph(digraph);
            var root1 = new AssemblyStub("Pencil.Build");
            var root2 = new AssemblyStub("Pencil.Test");
            var hub = new AssemblyName("Pencil.Core");

            root1.GetReferencedAssembliesHandler = () => new[] { hub };
            root2.GetReferencedAssembliesHandler = () => new[] { hub };

            graph.Add(root1);
            graph.Add(root2);

            Assert.That(digraph.Edges.Map(x => x.ToString()).ToList(), Is.EquivalentTo(new[] { "0->1", "2->1" }));
        }
		[Test]
		public void Should_load_referenced_assemblies()
		{
			var digraph = new DirectedGraph();
			var loader = new AssemblyLoaderStub();
            var graph = new AssemblyDependencyGraph(digraph, loader);
			var root = new AssemblyStub("RootAssembly");
			var system = new AssemblyName("System");
			var systemXml = new AssemblyName("System.Xml");
			var loaded = new List<AssemblyName>();
			loader.Loading += loaded.Add;
			root.GetReferencedAssembliesHandler = () => new []{ system, systemXml};

			graph.Add(root);

			Assert.That(loaded.Map(x => x.Name).ToList(), Is.EquivalentTo(new[] { system.Name, systemXml.Name }));
		}
	}
}