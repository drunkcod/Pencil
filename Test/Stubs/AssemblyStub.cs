namespace Pencil.Test.Stubs
{
    using System;
	using System.Reflection;
    using System.Collections.Generic;
    using Pencil.Core;

    class AssemblyStub : IAssembly
    {
        AssemblyName name;

        public Func<IEnumerable<AssemblyName>> GetReferencedAssembliesHandler = () => new AssemblyName[0];

		public AssemblyStub(string name): this(new AssemblyName(name)){}
        public AssemblyStub(AssemblyName name)
        {
            this.name = name;
        }

        public AssemblyName Name { get { return name; } }
        public IEnumerable<AssemblyName> ReferencedAssemblies { get { return GetReferencedAssembliesHandler(); } }
        public IEnumerable<IModule> Modules { get { throw new NotImplementedException(); } }
		public bool IsMissing { get { return false; } }
    }
}
