namespace Pencil.Build
{
    public interface IProject
    {
        bool HasTarget(string name);
        void Run(string target);

		IFileSystem FileSystem { get; set; }
		IExecutionEnvironment ExecutionEnvironment { get; set; }
    }
}