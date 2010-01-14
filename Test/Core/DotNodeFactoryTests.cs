using NUnit.Framework;
using Pencil.Dot;

namespace Pencil.Test.Dot
{
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