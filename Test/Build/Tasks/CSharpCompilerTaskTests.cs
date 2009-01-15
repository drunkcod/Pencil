namespace Pencil.Test.Build.Tasks
{
    using System;
    using NUnit.Framework;
    using Pencil.Build;
    using Pencil.Build.Tasks;
    using Pencil.Test.Stubs;
    using Pencil.IO;

    [TestFixture]
    public class CSharpCompilerTaskTests
    {
        [Test]
        public void Should_create_target_directory_if_not_present()
        {
            var fileSystem = new FileSystemStub();
            var environment = new ExecutionEnvironmentStub();
            environment.StartHandler = (fileName, arguments) => new ProcessStub();

            var compiler = new CSharpCompilerTask(fileSystem, environment);
			var outDir = new Path("Build").Combine("Debug");
            compiler.Output = outDir.Combine("Pencil.Build.dll");
            Path createdDirectory = Path.Empty;

            fileSystem.DirectoryExistsHandler = x => false;
            fileSystem.EnsureDirectoryHandler = x => createdDirectory = x;
            compiler.Execute();

            outDir.ShouldEqual(createdDirectory);
        }
        [Test]
        public void Should_copy_referenced_assemblies()
        {
            var fileSystem = new FileSystemStub();
            var environment = new ExecutionEnvironmentStub();
            environment.StartHandler = (fileName, arguments) => new ProcessStub();

            var compiler = new CSharpCompilerTask(fileSystem, environment);
			var outDir = new Path("Build");
            compiler.Output = outDir.Combine("Bar.dll");
            compiler.References.Add(new Path("Foo.dll"));

            fileSystem.DirectoryExistsHandler = x => true;
            fileSystem.FileExistsHandler = x => x.Equals(new Path("Foo.dll"));
            bool copied = false;
            fileSystem.CopyFileHandler = (from, to, overwrite) =>
            {
                Assert.AreEqual(new Path("Foo.dll"), from);
                Assert.AreEqual(outDir + "Foo.dll", to);
                copied = true;
            };
            compiler.Execute();

            Assert.IsTrue(copied, "Referenced assembly not copied.");
        }
        [Test]
        public void Wont_copy_referenced_assemblies_already_present()
        {
            var fileSystem = new FileSystemStub();
            var environment = new ExecutionEnvironmentStub();
            environment.StartHandler = (fileName, arguments) => new ProcessStub();

            var compiler = new CSharpCompilerTask(fileSystem, environment);
            compiler.Output = new Path("Build").Combine("Bar.dll");
            compiler.References.Add(new Path("Foo.dll"));

            fileSystem.DirectoryExistsHandler = x => true;
            fileSystem.FileExistsHandler = x => true;
            fileSystem.CopyFileHandler = (from, to, overwrite) =>
            {
                Assert.Fail("Should not try to copy file already present.");
            };
            compiler.Execute();
        }
    }
}
