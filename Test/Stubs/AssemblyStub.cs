namespace Pencil.Test.Stubs
{
    using System;
    using System.Collections.Generic;
    using Pencil.Core;

    class AssemblyStub : IAssembly
    {
        string name;

        public Func<IEnumerable<IAssembly>> GetReferencedAssembliesHandler = () => new IAssembly[0];

        public AssemblyStub(string name)
        {
            this.name = name;
        }

        public string Name { get { return name; } }
        public IEnumerable<IAssembly> ReferencedAssemblies { get { return GetReferencedAssembliesHandler(); } }
        public IEnumerable<IModule> Modules { get { throw new NotImplementedException(); } }
    }
}
