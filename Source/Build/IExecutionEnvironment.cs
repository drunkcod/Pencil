namespace Pencil.Build
{
    public interface IExecutionEnvironment
    {
        IProcess Start(string fileName, string arguments);
    }
}
