namespace Pencil.Build.Tasks
{
	using System;
    using System.Text;

	public enum OutputType
	{
		Library, Application, WindowsApplication, Module
	}

	public class CSharpCompilerTask : ExecTaskBase
	{
		FileSet sources = new FileSet();
		FileSet references = new FileSet();
		IFileSystem fileSystem;

		public FileSet Sources { get { return sources; } }
		public FileSet References { get { return references; } }
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
			if(IsRunningOnMono)
				return RuntimeDirectory + "gmcs.exe";
			return RuntimeDirectory + ".." + "v3.5" + "csc.exe";
		}

		protected override string GetArgumentsCore()
		{
			References.CopyTo(fileSystem, Output.GetDirectory());
			return CollectArguments();
		}

        string CollectArguments()
        {
            var arguments = new StringBuilder("/nologo")
            	.AppendFormat(" /out:{0}", Output)
            	.AppendFormat(" /debug{0}", Debug ? "+" : "-")
            	.AppendFormat(" /target:{0}", GetTargetType());
            using(var r = References.GetEnumerator())
            {
                if(r.MoveNext())
                    arguments.AppendFormat(" /r:{0}", r.Current);
                while(r.MoveNext())
                    arguments.AppendFormat(",{0}", r.Current);
            }
			sources.ForEach(x => arguments.AppendFormat(" {0}", x));
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