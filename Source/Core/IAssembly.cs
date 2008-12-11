namespace Pencil.Core
{
	using System.Reflection;
    using System.Collections.Generic;

    public interface IAssembly
    {
		AssemblyName Name { get; }
		IEnumerable<AssemblyName> ReferencedAssemblies { get; }
        IEnumerable<IModule> Modules { get; }
    }
}
