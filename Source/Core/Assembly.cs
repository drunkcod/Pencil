namespace Pencil.Core
{
    using System.Collections.Generic;
    using ReflectionAssembly = System.Reflection.Assembly;
	using AssemblyName = System.Reflection.AssemblyName;

    public class Assembly : IAssembly
    {
        ReflectionAssembly assembly;

		public AssemblyName Name { get { return assembly.GetName(); } }

        public IEnumerable<AssemblyName> ReferencedAssemblies
        {
            get { return assembly.GetReferencedAssemblies(); }
        }

        public IEnumerable<IModule> Modules
        {
            get { throw new System.NotImplementedException(); }
        }

		public bool IsMissing { get { return false; } }

        internal Assembly(ReflectionAssembly assembly)
        {
            this.assembly = assembly;
        }
    }
}