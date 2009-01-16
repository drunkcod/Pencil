namespace Pencil.Test.Build
{
	using System.Collections.Generic;
    using System.IO;
    using NUnit.Framework;
    using NUnit.Framework.SyntaxHelpers;
    using Pencil.Build;
	using Pencil.Test.Stubs;

    [TestFixture]
    public class ProgramTests
    {
        public Program Program { get { return new Program(Logger.Null, x => null); } }

        [Test]
        public void BuildTarget_should_return_Failiure_if_target_not_in_Project()
        {
            var project = new ProjectStub();
            project.HasTargetHandler = x => false;
            Assert.That(Program.BuildTarget(project, "Target"), Is.EqualTo(Program.Failiure));
        }
        [Test]
        public void BuildTarget_should_return_Success_if_valid_target_and_successful_build()
        {
            var project = new ProjectStub();
            project.HasTargetHandler = x => true;
            Assert.That(Program.BuildTarget(project, "Target"), Is.EqualTo(Program.Success));
        }
        [Test]
        public void Should_build_all_specified_targets()
        {
            var project = new ProjectStub();
            var built = new List<string>();
            project.HasTargetHandler = x => true;
            project.RunHandler = built.Add;

			new Program(Logger.Null, x => project).Run(new[]{ "BuildFile", "Target1", "Target2" });

			built.ShouldEqual(new[]{ "Target1", "Target2" });
        }
    }
}