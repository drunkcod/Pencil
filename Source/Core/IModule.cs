namespace Pencil.Core
{
    using System.Collections.Generic;

    public interface IModule
    {
        IEnumerable<IType> Types { get; }
    }
}
