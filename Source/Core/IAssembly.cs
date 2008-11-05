namespace Pencil.Core
{
    using System.Collections.Generic;

    public interface IAssembly
    {
        IEnumerable<IModule> Modules { get; }
    }
}
