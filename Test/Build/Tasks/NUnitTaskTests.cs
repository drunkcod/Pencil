namespace Pencil.Test.Build.Tasks
{
	using System;
	using NUnit.Framework;
	using Pencil.Build;
	using Pencil.Build.Tasks;
	using Pencil.Test.Stubs;

	class NullProcess : IProcess
	{
		public bool HasExited { get { return true; } }
        public int ExitCode { get { return 0; } }
        public System.IO.TextReader StandardOutput { get { return null; } }
	}

	[TestFixture]
	public class NUnitTaskTests
	{
		[Test]
		public void Should_use_mono_for_program_if_running_on_mono()
		{
			var envionrment = new ExecutionEnvironmentStub();
			envionrment.IsMonoHandler = () => true;
			var nunit = new NUnitTask(envionrment);

			nunit.Program.ShouldEqual(new Path("mono"));
		}
		[Test]
		public void Should_use_nunit_console_for_program_if_not_on_mono()
		{
			var envionrment = new ExecutionEnvironmentStub();
			envionrment.IsMonoHandler = () => false;
			var nunit = new NUnitTask(envionrment);

			nunit.Program.ToString().EndsWith("nunit-console.exe").ShouldBe(true);
		}
		[Test]
		public void NUnitPath_should_be_based_on_NUnitBinPath()
		{
			var envionrment = new ExecutionEnvironmentStub();
			envionrment.IsMonoHandler = () => false;
			var nunit = new NUnitTask(envionrment);
			var binPath = new Path("NUnit") + "bin";
			nunit.NUnitBinPath = binPath;
			nunit.NUnitPath.ShouldEqual(binPath + "nunit-console.exe");
		}
		[Test]
		public void Arguments_should_start_with_NUnitPath_on_mono()
		{
			var environment = new ExecutionEnvironmentStub();
			environment.IsMonoHandler = () => true;
			var nunit = new NUnitTask(environment);

			environment.StartHandler += (p, args) => 
			{
				args.StartsWith(nunit.NUnitPath.ToString()).ShouldBe(true);
				return new NullProcess();
			};
			nunit.Execute();
		}
		[Test]
		public void Arguments_should_contain_Target()
		{
			var environment = new ExecutionEnvironmentStub();
			var nunit = new NUnitTask(environment);
			nunit.Target = new Path("MyTests.dll");
			environment.StartHandler += (p, args) => 
			{
				args.Contains("MyTests.dll").ShouldBe(true);
				return new NullProcess();
			};
			nunit.Execute();
		}
		[Test]
		public void Should_support_disabling_shadow_copy()
		{
			var environment = new ExecutionEnvironmentStub();
			var nunit = new NUnitTask(environment);
			nunit.ShadowCopy = false;
			environment.StartHandler += (p, args) => 
			{
				args.Contains("-noshadow").ShouldBe(true);
				return new NullProcess();
			};
			nunit.Execute();
		}
	}
}
