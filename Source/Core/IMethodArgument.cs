namespace Pencil.Core
{
    public interface IMethodArgument
    {
		string Name { get; }
        IType Type { get; }
    }
}
