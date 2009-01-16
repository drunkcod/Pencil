namespace Pencil.Build
{
	using System;
	using System.IO;

    public interface IExecutionEnvironment
    {
        void Run(string fileName, string arguments, Action<IProcess> processHandler);
		TextWriter StandardOut { get; }
		bool IsMono { get; }
    }
}
