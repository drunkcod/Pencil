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
        csc.Output = @"Build\Debug\Pencil.Build.exe";
        csc.Debug = false;
        csc.Execute();
    }
    
    public void Core()
	{
		var csc = New<CSharpCompilerTask>();
		csc.Sources.Add(@"Source\Core\*.cs");
		csc.OutputType = OutputType.Library;
		csc.Output = @"Build\Debug\Pencil.Core.dll";
		csc.Debug = true;
		csc.Execute();
	}

	public void Test()
	{
		var csc = New<CSharpCompilerTask>();
		csc.Sources.Add(@"Test\Core\*.cs");
		csc.References.Add(@"Build\Debug\Pencil.Core.dll");
		csc.References.Add(@"Tools\NUnit-2.4.8-net-2.0\bin\nunit.framework.dll");
		csc.OutputType = OutputType.Library;
		csc.Output = @"Build\Pencil.Test.dll";
		csc.Debug = true;
		csc.Execute();

		var nunit = New<ExecTask>();
		nunit.Program = @"Tools\NUnit-2.4.8-net-2.0\bin\nunit-console.exe";
		nunit.CommandLine = @"Build\Pencil.Test.dll";
		nunit.Execute();
	}
}