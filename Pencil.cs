using System.IO;
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
		csc.Sources.Add(@"Source\NMeter\*.cs");
		csc.OutputType = OutputType.Library;
		csc.Output = @"Build\Debug\Pencil.dll";
		csc.Debug = true;
		csc.Execute();
	}

	[DependsOn("Core")]
	public void Console()
	{
		var csc = New<CSharpCompilerTask>();
		csc.Sources.Add(@"Source\NMeter\Console\*.cs");
		csc.References.Add(@"Build\Debug\Pencil.dll");
		csc.OutputType = OutputType.Application;
		csc.Output = @"Build\Debug\NMeter.Console.exe";
		csc.Debug = true;
		csc.Execute();
	}

	[DependsOn("Core"), DependsOn("Build")]
	public void Test()
	{
		var csc = New<CSharpCompilerTask>();
		csc.Sources.Add(@"Test\*.cs");
		csc.Sources.Add(@"Test\Core\*.cs");
		csc.Sources.Add(@"Test\Build\*.cs");
        csc.Sources.Add(@"Test\Build\Tasks\*.cs");
        csc.Sources.Add(@"Test\NMeter\*.cs");
        csc.Sources.Add(@"Test\Stubs\*.cs");
        csc.References.Add(@"Build\Debug\Pencil.dll");
        csc.References.Add(@"Build\Debug\Pencil.Build.exe");
		csc.References.Add(@"Tools\NUnit-2.4.8-net-2.0\bin\nunit.framework.dll");
		csc.OutputType = OutputType.Library;
		csc.Output = @"Build\Debug\Pencil.Test.dll";
		csc.Debug = true;
		csc.Execute();

		File.Copy(@"Test\SampleProject.xml", @"Build\Debug\SampleProject.xml", true);

		var nunit = New<ExecTask>();
		nunit.Program = @"Tools\NUnit-2.4.8-net-2.0\bin\nunit-console.exe";
		nunit.CommandLine = csc.Output + " /nologo";
		nunit.Execute();
	}

	public void Clean()
	{
		foreach(var file in Directory.GetFiles(".", "*.bak", SearchOption.AllDirectories))
			File.Delete(file);
	}
}