using System;
using Pencil.IO;
using Pencil.Build;
using Pencil.Build.Tasks;
using Pencil.Unit;

public class PencilProject : Project
{
	Path Outdir { get { return new Path("Build") + (debugMode ? "Debug" : "Release"); } }
	readonly Path source = new Path("Source");
	IFileSystem FileSystem { get { return New<IFileSystem>(); } }
	bool debugMode = true;

    public void Core()
	{
		var csc = NewCSharpCompiler();
		csc.Sources.Add(source + "Core" + "*.cs")
			.Add(source + "NMeter" + "*.cs")
			.Add(source + "IO" + "*.cs");
		csc.References.Add(new Path("System.Drawing.dll"));
		csc.OutputType = OutputType.Library;
		csc.Output = Outdir + "Pencil.dll";
        csc.Compile();
	}

	[DependsOn("Core")]
	public void Build()
    {
		var csc = NewCSharpCompiler();
		var build = source + "Build";
        csc.Sources.Add(build + "*.cs")
			.Add(build + "Tasks" + "*.cs");
		csc.References.Add(Outdir + "Pencil.dll");
        csc.OutputType = OutputType.Application;
        csc.Output = Outdir + "Pencil.Build.exe";
    	csc.Compile();
    }

	[DependsOn("Core")]
	public void Console()
	{
		var csc = NewCSharpCompiler();
		csc.Sources.Add(source + "NMeter" + "Console" + "*.cs");
		csc.References.Add(Outdir + "Pencil.dll");
		csc.OutputType = OutputType.Application;
		csc.Output = Outdir + "NMeter.Console.exe";
		csc.Compile();
	}

	[DependsOn("Core"), DependsOn("Build")]
	public void BuildTest()
	{
		var csc = NewCSharpCompiler();
		var test = new Path("Test");
		var nunitDir = new Path("Tools") + "NUnit-2.4.8-net-2.0" + "bin";

		csc.Sources.Add(test + "*.cs")
			.Add(test + "Core" + "*.cs")
			.Add(test + "Build" + "*.cs")
			.Add(test + "Build" + "Tasks" + "*.cs")
			.Add(test + "NMeter" + "*.cs")
 			.Add(test + "Stubs" + "*.cs");
		csc.References.Add(new Path("System.Drawing.dll"))
			.Add(Outdir + "Pencil.dll")
			.Add(Outdir + "Pencil.Build.exe")
			.Add(nunitDir + "nunit.framework.dll");
		csc.OutputType = OutputType.Library;
		csc.Output = Outdir + "Pencil.Test.dll";
		csc.Compile();
	}

	[DependsOn("BuildTest")]
	public void Test()
	{
		var test = new Path("Test");
		var nunitDir = new Path("Tools") + "NUnit-2.4.8-net-2.0" + "bin";
		FileSystem.CopyFile(test + "SampleProject.xml", Outdir + "SampleProject.xml", true);

		var nunit = New<NUnitTask>();
		nunit.NUnitBinPath= nunitDir;
		nunit.Target = Outdir + "Pencil.Test.dll";
		nunit.ShadowCopy = false;
		nunit.ShowLogo = false;

		if(FileSystem.GetLastWriteTime(new Path("TestResult.xml"))
		    < FileSystem.GetLastWriteTime(nunit.Target))
    		nunit.Execute();
	}
	[DependsOn("Build")]
	public void FSharpCompilerTask()
	{
		var fsc = NewFSharpCompiler();
		fsc.Sources.Add(source + "Core" + "Funky.fs")
			.Add(source + "Build" + "Tasks" + "FSharpCompilerTask.fs");
		fsc.References.Add(fsc.BinPath + "FSharp.Core.dll");
		fsc.References.Add(Outdir + "Pencil.dll");
		fsc.References.Add(Outdir + "Pencil.Build.exe");
		fsc.OutputType = OutputType.Library;
		fsc.Output = Outdir + "Pencil.Build.FSharpCompilerTask.dll";
        fsc.Compile();
	}
    [DependsOn("Core")]
	public void Unit()
	{
		var fsc = NewFSharpCompiler();
		fsc.Sources.Add(source + "Core" + "Funky.fs")
			.Add(source + "Unit" + "Syntax.fs")
			.Add(source + "Unit" + "Suite.fs")
			.Add(source + "Unit" + "TextWriterTestResults.fs")
			.Add(source + "Unit" + "TextWriterRunner.fs");
		fsc.References.Add(fsc.BinPath + "FSharp.Core.dll")
			.Add(Outdir + "Pencil.dll");
		fsc.OutputType = OutputType.Library;
		fsc.Output = Outdir + "Pencil.Unit.dll";
		fsc.Compile();
	}

	[DependsOn("Unit"), DependsOn("BuildTest"), DependsOn("FSharpCompilerTask")]
	public void TestFs()
	{
		var fsc = NewFSharpCompiler();
		var test = new Path("Test");
		var nunitDir = new Path("Tools") + "NUnit-2.4.8-net-2.0" + "bin";

		fsc.Sources.Add(test + "Build" + "FileSetTests.fs")
			.Add(test + "Build" + "Tasks" + "CompilerBaseTaskTests.fs")
			.Add(test + "Build" + "Tasks" + "FSharpCompilerTaskTests.fs")
			.Add(test + "Unit" + "BeMatcherTests.fs")
			.Add(test + "Unit" + "ContainMatcherTests.fs")
			.Add(test + "Unit" + "SyntaxTests.fs")
			.Add(test + "Unit" + "TextWriterRunnerTests.fs");

		fsc.References.Add(Outdir + "Pencil.dll")
			.Add(Outdir + "Pencil.Build.exe")
			.Add(Outdir + "Pencil.Unit.dll")
			.Add(Outdir + "Pencil.Build.FSharpCompilerTask.dll")
			.Add(Outdir + "Pencil.Test.dll");
		fsc.OutputType = OutputType.Library;
		fsc.Output = Outdir + "Pencil.Test.FSharp.dll";
    	fsc.Compile();
        TextWriterRunner.Run(fsc.Output.ToString(), System.Console.Out);
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
		csc.Optimize = !debugMode;
		return csc;
	}

	FSharpCompilerTask NewFSharpCompiler()
	{
		var fsc = New<FSharpCompilerTask>();
		fsc.BinPath = new Path(Environment.GetEnvironmentVariable("FSharp"));
		fsc.Debug = debugMode;
		fsc.Optimize = !debugMode;
		return fsc;
	}
}
