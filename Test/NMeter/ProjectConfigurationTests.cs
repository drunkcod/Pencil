namespace Pencil.Test.NMeter
{
    using NUnit.Framework;
    using NUnit.Framework.SyntaxHelpers;
    using Pencil.NMeter;

    [TestFixture]
    public class ProjectConfigurationTests
    {
        ProjectConfiguration ReadSampleProject()
        {
            return ProjectConfiguration.FromFile("SampleProject.xml");
        }

        [Test,Category("Fileystem")]
        public void Should_match_format_of_sample_project_file_for_BinPath()
        {
            ReadSampleProject().BinPath.ShouldEqual("Build");
        }
        [Test, Category("Fileystem")]
        public void Should_match_format_of_sample_project_file_for_ignored_assembly_names()
        {
            var ignored = new IgnoreFilterConfiguration();
            ignored.Names.Add(new IgnoreItem("mscorlib"));

            Assert.That(ReadSampleProject().IgnoreAssemblies.Names, Is.EquivalentTo(ignored.Names));
        }
        [Test, Category("Fileystem")]
        public void Should_match_format_of_sample_project_file_for_ignored_assembly_patterns()
        {
            var ignored = new IgnoreFilterConfiguration();
            ignored.Patterns.Add(new IgnoreItem("^System.*"));

            Assert.That(ReadSampleProject().IgnoreAssemblies.Patterns, Is.EquivalentTo(ignored.Patterns));
        }
    }
}
