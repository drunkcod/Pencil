namespace Pencil.Test.Core
{
	using System.Reflection;
	using NUnit.Framework;
	using Pencil.Core;

	[TestFixture]
	public class StaticAssemblyLoaderTests
	{
		[Test]
		public void Should_return_MissingAssembly_when_no_assemblies_loaded()
		{
			var loader = new StaticAssemblyLoader();
			loader.Load(new AssemblyName("Pencil.dll")).IsMissing.ShouldBe(true);
		}
		[Test]
		public void Should_remember_Registred_assemblies()
		{
			var loader = new StaticAssemblyLoader();
			var self = AssemblyLoader.GetExecutingAssembly();
			loader.Register(self);

			loader.Load(self.Name).ShouldBeSameAs(self);
		}
	}
}