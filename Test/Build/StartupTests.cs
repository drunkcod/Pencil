namespace Pencil.Test.Build
{
	using NUnit.Framework;
	using Pencil.Build;
	using Pencil.IO;

	[TestFixture]
	public class StartupTests
	{
		[Test]
		public void Should_extract_referenced_assembly_paths_from_args()
		{
			Startup.GetReferencedAssemblies(new[]
			{
				"-r:MyAssembly.dll"
			}).ShouldContain(new Path("MyAssembly.dll"));
		}
		[Test]
		public void Should_remove_options_from_args()
		{
			Startup.GetArguments(new[]
			{
				"-r:MyAssembly.dll", "Build.cs"
			}).ShouldEqual(new[]{ "Build.cs" });
		}
	}
}