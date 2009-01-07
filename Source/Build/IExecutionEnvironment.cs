namespace Pencil.Build
{
	using System.IO;

    public interface IExecutionEnvironment
    {
        IProcess Start(string fileName, string arguments);
		TextWriter StandardOut { get; }
    }
}
