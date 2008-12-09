namespace Pencil.Core
{
    using System.Collections.Generic;
    using ReflectionAssembly = System.Reflection.Assembly;

    public class Assembly : IAssembly
    {
        ReflectionAssembly assembly;
        public static IAssembly GetExecutingAssembly()
        {
            return new Assembly(ReflectionAssembly.GetCallingAssembly());
        }

        public string Name { get { return assembly.GetName().Name; } }

        public IEnumerable<IAssembly> ReferencedAssemblies
        {
            get { throw new System.NotImplementedException(); }
        }

        public IEnumerable<IModule> Modules
        {
            get { throw new System.NotImplementedException(); }
        }

        Assembly(ReflectionAssembly assembly)
        {
            this.assembly = assembly;
        }
    }
}
