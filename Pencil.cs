using Pencil.IO;
using Pencil.Build;
using Pencil.Build.Tasks;

public class PencilProject : Project
{
	Path Outdir { get { return new Path("Build") + (debugMode ? "Debug" : "Release"); } }
	readonly Path source = new Path("Source");
	IFileSystem FileSystem { get { return New<IFileSystem>(); } }
	bool debugMode = true;

    public void Core()
	{
		var csc = NewCSharpCompiler();
		csc.Sources.Add(source + "Core" + "*.cs");
		csc.Sources.Add(source + "NMeter" + "*.cs");
		csc.Sources.Add(source + "IO" + "*.cs");
		csc.References.Add(new Path("System.Drawing.dll"));
		csc.OutputType = OutputType.Library;
		csc.Output = Outdir + "Pencil.dll";
		csc.Execute();
	}

	[DependsOn("Core")]
	public void Build()
    {
		var csc = NewCSharpCompiler();
		var build = source + "Build";
        csc.Sources.Add(build + "*.cs");
        csc.Sources.Add(build + "Tasks" + "*.cs");
		csc.References.Add(Outdir + "Pencil.dll");
        csc.OutputType = OutputType.Application;
        csc.Output = Outdir + "Pencil.Build.exe";
        csc.Execute();
    }

	[DependsOn("Core")]
	public void Console()
	{
		var csc = NewCSharpCompiler();
		csc.Sources.Add(source + "NMeter" + "Console" + "*.cs");
		csc.References.Add(Outdir + "Pencil.dll");
		csc.OutputType = OutputType.Application;
		csc.Output = Outdir + "NMeter.Console.exe";
		csc.Execute();
	}

	[DependsOn("Core"), DependsOn("Build")]
	public void Test()
	{
		var csc = NewCSharpCompiler();
		var test = new Path("Test");
		var nunitDir = new Path("Tools") + "NUnit-2.4.8-net-2.0" + "bin";

		csc.Sources.Add(test + "*.cs");
		csc.Sources.Add(test + "Core" + "*.cs");
		csc.Sources.Add(test + "Build" + "*.cs");
		csc.Sources.Add(test + "Build" + "Tasks" + "*.cs");
		csc.Sources.Add(test + "NMeter" + "*.cs");
 		csc.Sources.Add(test + "Stubs" + "*.cs");
		csc.References.Add(new Path("System.Drawing.dll"));
		csc.References.Add(Outdir + "Pencil.dll");
		csc.References.Add(Outdir + "Pencil.Build.exe");
		csc.References.Add(nunitDir + "nunit.framework.dll");
		csc.OutputType = OutputType.Library;
		csc.Output = Outdir + "Pencil.Test.dll";
		csc.Execute();

		FileSystem.CopyFile(test + "SampleProject.xml", Outdir + "SampleProject.xml", true);

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

	public void Release()
	{
		debugMode = false;
	}

	CSharpCompilerTask NewCSharpCompiler()
	{
		var csc = New<CSharpCompilerTask>();
		csc.Debug = debugMode;
		return csc;
	}
}