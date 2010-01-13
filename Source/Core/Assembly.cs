namespace Pencil.Core
{
    using System.Collections.Generic;
    using ReflectionAssembly = System.Reflection.Assembly;
	using AssemblyName = System.Reflection.AssemblyName;

    public class Assembly : IAssembly
    {
        ITypeLoader typeLoader;
        ReflectionAssembly assembly;

		public AssemblyName Name { get { return assembly.GetName(); } }

        public IEnumerable<AssemblyName> ReferencedAssemblies
        {
            get { return assembly.GetReferencedAssemblies(); }
        }

        public IEnumerable<IModule> Modules
        {
            get 
            {
                foreach(var module in assembly.GetModules())
                    yield return new Module(typeLoader, module);
            }
        }

		public bool IsMissing { get { return false; } }

        internal Assembly(ITypeLoader typeLoader, ReflectionAssembly assembly)
        {
            this.typeLoader = typeLoader;
            this.assembly = assembly;
        }
    }
}