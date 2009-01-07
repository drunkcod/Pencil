namespace Pencil.Test.Build
{
	using NUnit.Framework;
	using Pencil.Build;
	using Pencil.Build.Tasks;

	[TestFixture]
	public class ProjectTests
	{
		[Test]
		public void Should_be_able_to_create_CSharpCompilerTask()
		{
			var project = new Project();

			Assert.IsNotNull(project.New<CSharpCompilerTask>());
		}
	}
}