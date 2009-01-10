namespace Pencil.Test.Build
{
    using System.IO;
    using NUnit.Framework;
    using NUnit.Framework.SyntaxHelpers;
    using Pencil.Build;
	using Pencil.Test.Stubs;

    [TestFixture]
    public class ProgramTests
    {
        public Program Program { get { return new Program(new Logger(new StreamWriter(Stream.Null)), null); } }

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
    }
}
