namespace Pencil.Test
{
	using NUnit.Framework;
	using Pencil.IO;
	using FXPath = System.IO.Path;
	
	[TestFixture]
	public class PathTests
	{
		[Test]
		public void Should_use_Path_DirectorySeparatorChar_for_subdirectories()
		{
			var path = new Path("Parent").Combine("Child");
			Assert.AreEqual(FXPath.Combine("Parent", "Child"), path.ToString());
		}
	}
}
