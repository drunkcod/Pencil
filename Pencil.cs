using Pencil.Build;
using Pencil.Build.Tasks;

public class PencilProject : Project
{
	readonly Path outdir = new Path("Build") + "Debug";
	readonly Path source = new Path("Source");
	IFileSystem FileSystem { get { return New<IFileSystem>(); } }

    public void Core()
	{
		var csc = New<CSharpCompilerTask>();
		csc.Sources.Add(source + "Core" + "*.cs");
		csc.Sources.Add(source + "NMeter" + "*.cs");
		csc.Sources.Add(source + "IO" + "*.cs");
		csc.References.Add(new Path("System.Drawing.dll"));
		csc.OutputType = OutputType.Library;
		csc.Output = outdir + "Pencil.dll";
		csc.Debug = true;
		csc.Execute();
	}

	[DependsOn("Core")]
	public void Build()
    {
        var csc = New<CSharpCompilerTask>();
		var build = source + "Build";
        csc.Sources.Add(build + "*.cs");
        csc.Sources.Add(build + "Tasks" + "*.cs");
		csc.References.Add(outdir + "Pencil.dll");
        csc.OutputType = OutputType.Application;
        csc.Output = outdir + "Pencil.Build.exe";
        csc.Debug = false;
        csc.Execute();
    }

	[DependsOn("Core")]
	public void Console()
	{
		var csc = New<CSharpCompilerTask>();
		csc.Sources.Add(source + "NMeter" + "Console" + "*.cs");
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
		csc.References.Add(new Path("System.Drawing.dll"));
		csc.References.Add(outdir + "Pencil.dll");
		csc.References.Add(outdir + "Pencil.Build.exe");
		csc.References.Add(nunitDir + "nunit.framework.dll");
		csc.OutputType = OutputType.Library;
		csc.Output = outdir + "Pencil.Test.dll";
		csc.Debug = true;
		csc.Execute();

		FileSystem.CopyFile(test + "SampleProject.xml", outdir + "SampleProject.xml", true);

		var nunit = New<NUnitTask>();
		nunit.NUnitBinPath= nunitDir;
		nunit.Target = csc.Output;
		nunit.ShadowCopy = false;
		nunit.ShowLogo = false;
		nunit.Execute();
	}

	public void Clean()
	{
		foreach(var ext in new[]{ "*.bak", "*.pidb" })
		foreach(var file in FileSystem.GetFilesRecursive(new Path("."), ext))
			FileSystem.DeleteFile(file);
	}
}