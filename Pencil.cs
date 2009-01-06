using Pencil.Build;
using Pencil.Build.Tasks;

public class PencilProject : Project
{
    public void Build()
    {
        var csc = New<CSharpCompilerTask>();
		var source = new Path("Source").Combine("Build");
        csc.Sources.Add(source.Combine("*.cs").ToString());
        csc.Sources.Add(source.Combine("Tasks").Combine("*.cs").ToString());
        csc.OutputType = OutputType.Application;
        csc.Output = new Path("Build").Combine("Debug").Combine("Pencil.Build.exe").ToString();
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

		FileSystem.CopyFile(@"Test\SampleProject.xml", @"Build\Debug\SampleProject.xml");

		var nunit = New<ExecTask>();
		nunit.Program = @"Tools\NUnit-2.4.8-net-2.0\bin\nunit-console.exe";
		nunit.CommandLine = csc.Output + " /nologo";
		nunit.Execute();
	}

	public void Clean()
	{
		foreach(var file in FileSystem.GetFilesRecursive(".", "*.bak"))
			FileSystem.DeleteFile(file);
	}
}