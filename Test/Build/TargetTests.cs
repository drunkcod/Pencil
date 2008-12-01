namespace Pencil.Test.Build
{
	using NUnit.Framework;
	using System.Collections.Generic;
	using Pencil.Build;
	using System;

	class TargetStub : Target
	{
		IEnumerable<string> dependencies;

		public Action ExecuteHandler;

		public TargetStub(IProject project, IEnumerable<string> dependencies) : base(project)
		{
			this.dependencies = dependencies;
		}

		public override IEnumerable<string> GetDependencies()
		{
			return dependencies;
		}

		protected override void ExecuteCore(){ ExecuteHandler(); }
	}

	class ProjectStub : IProject
	{
		public Converter<string,bool> HasTargetHandler;
		public Action<string> RunHandler;

		public bool HasTarget(string name){ return HasTargetHandler(name); }
        public void Run(string target){ RunHandler(target); }
	}

	[TestFixture]
	public class TargetTests
	{
		[Test]
		public void Should_run_dependencies_before_self()
		{
			var targetsExecuted = new List<string>();
			var dependencies = new List<string>();
			dependencies.Add("Core");
			dependencies.Add("Build");

			var project = new ProjectStub();
			project.RunHandler = targetsExecuted.Add;
			var target = new TargetStub(project, dependencies);
			target.ExecuteHandler = () => Assert.AreEqual(dependencies, targetsExecuted);
			target.Execute();
		}
	}
}