namespace Pencil.Test.Core
{
	using NUnit.Framework;
	using Pencil.Core;

	[TestFixture]
	public class ArrayExtensionsTests
	{
		[Test]
		public void TryFind_should_return_false_if_element_not_present()
		{
			int value;
			Assert.IsFalse(new []{1, 2, 3}.TryFind( x => x == 4, out value));
		}
		[Test]
		public void TryFind_should_return_tru_if_element_present()
		{
			int value;
			Assert.IsTrue(new []{1, 2, 3}.TryFind( x => x == 2, out value));
		}
	}
}