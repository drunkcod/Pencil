namespace Pencil.Test.Core
{
	using NUnit.Framework;
	using Pencil.Core;

	[TestFixture]
	public class DotNodeFactoryTests
	{
		[Test]
		public void Should_raise_NodeCreated_on_Create()
		{
			var factory = new DotNodeFactory();

			var nodeCreatedRaised = false;
			factory.NodeCreated += (sender, e) =>
			{
				Assert.AreSame(factory, sender);
				nodeCreatedRaised = true;
			};
			factory.Create();

			nodeCreatedRaised.ShouldBe(true);
		}
	}
}