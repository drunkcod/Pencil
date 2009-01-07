namespace Pencil.Test.Build
{
    using System.IO;
    using NUnit.Framework;
    using NUnit.Framework.SyntaxHelpers;
    using Pencil.Build;

    [TestFixture]
    public class ProgramTests
    {
        class DummyProject : IProject
        {
            public bool HasTargetReturns { get; set; }

            public bool HasTarget(string name) { return HasTargetReturns; }
            public void Run(string target) { }

			public IFileSystem FileSystem { get; set; }
			public IExecutionEnvironment ExecutionEnvironment { get; set; }
        }

        public Program Program { get { return new Program(new Logger(new StreamWriter(Stream.Null))); } }

        [Test]
        public void BuildTarget_should_return_Failiure_if_target_not_in_Project()
        {
            var project = new DummyProject();
            project.HasTargetReturns = false;
            Assert.That(Program.BuildTarget(project, "Target"), Is.EqualTo(Program.Failiure));
        }
        [Test]
        public void BuildTarget_should_return_Success_if_valid_target_and_successful_build()
        {
            var project = new DummyProject();
            project.HasTargetReturns = true;
            Assert.That(Program.BuildTarget(project, "Target"), Is.EqualTo(Program.Success));
        }
    }
}
