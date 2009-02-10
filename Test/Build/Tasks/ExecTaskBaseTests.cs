namespace Pencil.Test.Build.Tasks
{
    using Pencil.Build;
    using Pencil.Build.Tasks;
    using Pencil.IO;
    using Pencil.Test.Stubs;
    using NUnit.Framework;
    
    [TestFixture]
    public class ExecTaskBaseTests
    {
        class TestTask : ExecTaskBase
        {
            public TestTask(IExecutionEnvironment platform): base(platform)
            {}
            protected override Path GetProgramCore(){ return new Path("TestTask"); }
		    protected override  string GetArgumentsCore(){ return string.Empty; }
        }
    
        [Test]
        public void Execute_should_call_wait_for_exit_before_ExitCode()
        {//Since mono is broken and won't give us the ExitCode otherwise.
            var process = new ProcessStub();
            var waitForExitCalled = false;
            process.WaitForExitHandler = () => waitForExitCalled = true;
            process.GetExitCodeHandler = () =>
            { 
                Assert.IsTrue(waitForExitCalled);
                return 0;
            };
            var platform = new ExecutionEnvironmentStub();
            platform.RunHandler = (program, args, handler) => handler(process);
            var task = new TestTask(platform);
            task.Execute();
        }
    }
}
