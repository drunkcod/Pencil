using Pencil.Build;
using Pencil.Build.Tasks;

public class PencilProject : Project
{
	public void Build()
	{
		var csc = New<CSharpCompilerTask>();
		csc.Sources.Add(@"Source\Build\*.cs");
		csc.Sources.Add(@"Source\Build\Tasks\*.cs");
		csc.OutputType = OutputType.Application;
		csc.Output = @"Build\Pencil.Build.exe";
		csc.Debug = false;
		csc.Execute();
	}

	public void Test()
	{
		var csc = New<CSharpCompilerTask>();
		csc.Sources.Add(@"Test\Build\*.cs");
		csc.References.Add(@"Build\Pencil.Build.dll");
		csc.References.Add(@"Tools\NUnit-2.4.8-net-2.0\bin\nunit.framework.dll");
		csc.OutputType = OutputType.Library;
		csc.Output = @"Build\Pencil.Test.Build.dll";
		csc.Debug = true;
		csc.Execute();

		var nunit = New<ExecTask>();
		nunit.Program = @"Tools\NUnit-2.4.8-net-2.0\bin\nunit-console.exe";
		nunit.CommandLine = csc.Output;
		nunit.Execute();
	}
}