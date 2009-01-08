namespace Pencil.Test.Core
{
	using System;
	using NUnit.Framework;
	using Pencil.Core;

	class NodeFactoryStub : INodeFactory
	{
		public Func<Node> CreateHandler;
		public Node Create(){ return CreateHandler(); }
	}

	[TestFixture]
	public class DirectedGraphTests
	{
		[Test]
		public void Should_support_INodeFactory_injection()
		{
			var nodeFactory = new NodeFactoryStub();
			var createCalled = false;
			nodeFactory.CreateHandler = () =>
			{
				createCalled = true;
				return new Node();
			};
			var graph = new DirectedGraph(nodeFactory);
			graph.AddNode();

			createCalled.ShouldBe(true);
		}
	}
}