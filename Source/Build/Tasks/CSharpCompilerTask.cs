namespace Pencil.Build.Tasks
{
	using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;

	public enum OutputType
	{
		Library, Application, WindowsApplication, Module
	}

	public class CSharpCompilerTask : ExecTaskBase
	{
		List<Path> sources = new List<Path>();
		List<Path> references = new List<Path>();
		IFileSystem fileSystem;

		public List<Path> Sources { get { return sources; } }
		public List<Path> References { get { return references; } }
		public OutputType OutputType { get; set; }
		public Path Output { get; set; }
		public bool Debug { get; set; }

        public CSharpCompilerTask(IFileSystem fileSystem, IExecutionEnvironment executionEnvironment):
			base(executionEnvironment)
        {
            this.fileSystem = fileSystem;
        }

		protected override Path GetProgramCore()
		{
			var runtime = new Path(RuntimeEnvironment.GetRuntimeDirectory());
			if(RunningOnMono)
				return runtime.Combine("gmcs.exe");
			return runtime.Combine("..").Combine("v3.5").Combine("csc.exe");
		}

		static bool RunningOnMono { get { return Type.GetType("Mono.Runtime") != null; } }

		protected override string GetArgumentsCore()
		{
            EnsureOutputDirectory();
            CopyReferencedAssemblies();
            return CollectArguments();
		}

        void EnsureOutputDirectory()
        {
            string outputDirectory = Output.GetDirectoryName();
            if(!fileSystem.DirectoryExists(outputDirectory))
                fileSystem.CreateDirectory(outputDirectory);
        }

        void CopyReferencedAssemblies()
        {
            References.ForEach(file =>
            {
                var target = Output.GetDirectory() + file.GetFileName();
                if(fileSystem.FileExists(target) || !fileSystem.FileExists(file))
                    return;
                fileSystem.CopyFile(file, target, true);
            });
        }

        string CollectArguments()
        {
            var arguments = new StringBuilder("/nologo");
            arguments.AppendFormat(" /out:{0}", Output);
            arguments.AppendFormat(" /debug{0}", Debug ? "+" : "-");
            arguments.AppendFormat(" /target:{0}", GetTargetType());
            using(var r = References.GetEnumerator())
            {
                if(r.MoveNext())
                    arguments.AppendFormat(" /r:{0}", r.Current);
                while(r.MoveNext())
                    arguments.AppendFormat(",{0}", r.Current);
            }
            using(var s = Sources.GetEnumerator())
            {
                while(s.MoveNext())
                    arguments.AppendFormat(" {0}", s.Current);
            }
            return arguments.ToString();
        }

		string GetTargetType()
		{
			switch(OutputType)
			{
				case OutputType.Application: return "exe";
				case OutputType.WindowsApplication: return "winexe";
				case OutputType.Module: return "module";
				default: return "library";
			}
		}
	}
}
