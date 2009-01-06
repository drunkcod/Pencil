using Pencil.Build;
using Pencil.Build.Tasks;

public class PencilProject : Project
{
	readonly Path outdir = new Path("Build") + "Debug";
    
	public void Build()
    {
        var csc = New<CSharpCompilerTask>();
		var source = new Path("Source") + "Build";
        csc.Sources.Add(source + "*.cs");
        csc.Sources.Add(source + "Tasks" + "*.cs");
        csc.OutputType = OutputType.Application;
        csc.Output = outdir + "Pencil.Build.exe";
        csc.Debug = false;
        csc.Execute();
    }

    public void Core()
	{
		var csc = New<CSharpCompilerTask>();
		var source = new Path("Source");
		csc.Sources.Add(source + "Core" + "*.cs");
		csc.Sources.Add(source + "NMeter" + "*.cs");
		csc.OutputType = OutputType.Library;
		csc.Output = outdir + "Pencil.dll";
		csc.Debug = true;
		csc.Execute();
	}

	[DependsOn("Core")]
	public void Console()
	{
		var csc = New<CSharpCompilerTask>();
		csc.Sources.Add(new Path("Source") + "NMeter" + "Console" + "*.cs");
		csc.References.Add(outdir + "Pencil.dll");
		csc.OutputType = OutputType.Application;
		csc.Output = outdir + "NMeter.Console.exe";
		csc.Debug = true;
		csc.Execute();
	}

	[DependsOn("Core"), DependsOn("Build")]
	public void Test()
	{
		var csc = New<CSharpCompilerTask>();
		var test = new Path("Test");
		var nunitDir = new Path("Tools") + "NUnit-2.4.8-net-2.0" + "bin";
		
		csc.Sources.Add(test + "*.cs");
		csc.Sources.Add(test + "Core" + "*.cs");
		csc.Sources.Add(test + "Build" + "*.cs");
        csc.Sources.Add(test + "Build" + "Tasks" + "*.cs");
        csc.Sources.Add(test + "NMeter" + "*.cs");
        csc.Sources.Add(test + "Stubs" + "*.cs");
        csc.References.Add(outdir + "Pencil.dll");
        csc.References.Add(outdir + "Pencil.Build.exe");
		csc.References.Add(nunitDir + "nunit.framework.dll");
		csc.OutputType = OutputType.Library;
		csc.Output = outdir + "Pencil.Test.dll";
		csc.Debug = true;
		csc.Execute();

		FileSystem.CopyFile(test.Combine("SampleProject.xml").ToString(), outdir.Combine("SampleProject.xml").ToString());

		var nunit = New<ExecTask>();
		nunit.Program = nunitDir.Combine("nunit-console.exe").ToString();
		nunit.CommandLine = csc.Output.ToString() + " /nologo";
		nunit.Execute();
	}

	public void Clean()
	{
		foreach(var file in FileSystem.GetFilesRecursive(".", "*.bak"))
			FileSystem.DeleteFile(file);
	}
}