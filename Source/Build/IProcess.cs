namespace Pencil.Build
{
    using System.IO;

    public interface IProcess
    {
        bool HasExited { get; }
        int ExitCode { get; }
        TextReader StandardOutput { get; }

    }
}
