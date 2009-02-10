namespace Pencil.Build.Tasks
{
	using System;
    using System.Text;
    using Pencil.IO;

    public abstract class CompilerBaseTask : ExecTaskBase
    {
        readonly IFileSystem fileSystem;
        readonly FileSet sources;
		readonly FileSet references;
		Path output = Path.Empty;

		public Path Output { get { return output; } set { output = value; } }
		public FileSet Sources { get { return sources; } }
		public FileSet References { get { return references; } }

        protected CompilerBaseTask(IFileSystem fileSystem, IExecutionEnvironment platform):
			base(platform)
        {
            this.fileSystem = fileSystem;
		    sources = new FileSet(fileSystem);
		    references = new FileSet(fileSystem);
        }

        public void Compile()
        {
            if(Sources.ChangedAfter(Output) || References.ChangedAfter(Output))
                CompileCore();
        }

        protected virtual void CompileCore()
        {
            Execute();
        }
    }
}
