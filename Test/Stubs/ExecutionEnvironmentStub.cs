namespace Pencil.Test.Stubs
{
	using System;
	using System.IO;
    using Pencil.Build;

    delegate TReturn Func<TArg0, TArg1, TReturn>(TArg0 arg0, TArg1 arg1);
    delegate void Action2<TArg0, TArg1>(TArg0 arg0, TArg1 arg1);

    class ExecutionEnvironmentStub : IExecutionEnvironment
    {
        public Action<string, string, Action<IProcess>> RunHandler = (x, y, z) => {};
		public Func<bool> IsMonoHandler = () => false;

        public void Run(string fileName, string arguments, Action<IProcess> processHandler)
        {
            RunHandler(fileName, arguments, processHandler);
        }

		public TextWriter StandardOut { get { return Console.Out; } }
		public bool IsMono { get { return IsMonoHandler(); } }
    }
}
