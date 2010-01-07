namespace Pencil.Test.Build
{
	using System;
	using System.Collections.Generic;
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

		public class DoubleBuildBug : Project
		{
			public Action<string> RunHandler;
			public void Core(){}
			[DependsOn("Core")]
			public void Build(){}
			[DependsOn("Build"), DependsOn("Core")]
			public void Test(){}

			protected override void RunCore(string targetName)
			{
				RunHandler(targetName);
				base.RunCore(targetName);
			}
		}
		[Test]
		public void Wont_runt_same_target_multiple_times()
		{
			var targetsBuilt = new List<string>();
			var project = new DoubleBuildBug();
			project.RunHandler += targetsBuilt.Add;
			project.Run("Test");
			Assert.That(targetsBuilt, Is.EquivalentTo(new[]{ "Test", "Core", "Build" }));
		}
	}
}