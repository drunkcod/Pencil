namespace Pencil.Core
{
    using System.Collections.Generic;

    public interface IModule
    {
        string Name { get; }
        IEnumerable<IType> Types { get; }
    }
}
