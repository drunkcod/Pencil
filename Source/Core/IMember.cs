namespace Pencil.Core
{
    public interface IMember
    {
        IType DeclaringType { get; }
        string Name { get; }
    }
}
