namespace Pencil.Test.Stubs
{
	using System;
	using System.IO;
    using Pencil.Build;

    delegate TReturn Func<TArg0, TArg1, TReturn>(TArg0 arg0, TArg1 arg1);
    delegate void Action2<TArg0, TArg1>(TArg0 arg0, TArg1 arg1);

    class ExecutionEnvironmentStub : IExecutionEnvironment
    {
        public Func<string, string, IProcess> StartHandler;

        public IProcess Start(string fileName, string arguments)
        {
            return StartHandler(fileName, arguments);
        }

		public TextWriter StandardOut { get { return Console.Out; } }
    }
}
