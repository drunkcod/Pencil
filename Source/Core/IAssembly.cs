namespace Pencil.Core
{
    using System.Collections.Generic;

    public interface IAssembly
    {
		string Name { get; }
		IEnumerable<IAssembly> ReferencedAssemblies { get; }
        IEnumerable<IModule> Modules { get; }

    }
}
