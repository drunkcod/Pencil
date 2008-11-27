namespace Pencil.Build.Tasks
{
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

	public enum OutputType
	{
		Library, Application, WindowsApplication, Module
	}

	public class CSharpCompilerTask : ExecTaskBase
	{
		List<string> sources = new List<string>();
		List<string> references = new List<string>();
        IFileSystem fileSystem;

		public List<string> Sources { get { return sources; } }
		public List<string> References { get { return references; } }
		public OutputType OutputType { get; set; }
		public string Output { get; set; }
		public bool Debug { get; set; }

        public CSharpCompilerTask() : this(new FileSystem(), new ExecutionEnvironment()) { }
        public CSharpCompilerTask(IFileSystem fileSystem, IExecutionEnvironment executionEnvironment) : base(executionEnvironment)
        {
            this.fileSystem = fileSystem;
        }

		protected override string GetProgramCore()
		{
			return Path.Combine(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), ".."), "v3.5\\csc.exe");
		}

		protected override string GetArgumentsCore()
		{
            EnsureOutputDirectory();
            CopyReferencedAssemblies();
            return CollectArguments();
		}

        void EnsureOutputDirectory()
        {
            string outputDirectory = Path.GetDirectoryName(Output);
            if(!fileSystem.DirectoryExists(outputDirectory))
                fileSystem.CreateDirectory(outputDirectory);
        }

        void CopyReferencedAssemblies()
        {
            References.ForEach(file =>
            {
                var target = Path.Combine(Path.GetDirectoryName(Output), Path.GetFileName(file));
                if(fileSystem.FileExists(target))
                    return;
                fileSystem.CopyFile(file, target);
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