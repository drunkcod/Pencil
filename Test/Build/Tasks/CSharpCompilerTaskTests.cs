namespace Pencil.Test.Build.Tasks
{
    using System;
    using NUnit.Framework;
    using Pencil.Build;
    using Pencil.Build.Tasks;
    using Pencil.Test.Stubs;

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
            compiler.Output = @"Build\Debug\Pencil.Build.dll";
            string createdDirectory = string.Empty;

            fileSystem.DirectoryExistsHandler = x => false;
            fileSystem.CreateDirectoryHandler = x => createdDirectory = x;
            compiler.Execute();

            Assert.AreEqual(@"Build\Debug", createdDirectory);
        }
        [Test]
        public void Should_copy_referenced_assemblies()
        {
            var fileSystem = new FileSystemStub();
            var environment = new ExecutionEnvironmentStub();
            environment.StartHandler = (fileName, arguments) => new ProcessStub();

            var compiler = new CSharpCompilerTask(fileSystem, environment);
            compiler.Output = @"Build\Bar.dll";
            compiler.References.Add("Foo.dll");

            fileSystem.DirectoryExistsHandler = x => true;
            fileSystem.FileExistsHandler = x => false;
            bool copied = false;
            fileSystem.CopyFileHandler = (from, to) => 
            {
                Assert.AreEqual("Foo.dll", from);
                Assert.AreEqual(@"Build\Foo.dll", to);
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
            compiler.Output = @"Build\Bar.dll";
            compiler.References.Add("Foo.dll");

            fileSystem.DirectoryExistsHandler = x => true;
            fileSystem.FileExistsHandler = x => true;
            fileSystem.CopyFileHandler = (from, to) =>
            {
                Assert.Fail("Should not try to copy file already present.");
            };
            compiler.Execute();
        }
    }
}
