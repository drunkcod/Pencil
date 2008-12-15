namespace Pencil.Test.Stubs
{
	using System;
	using System.Collections.Generic;
	using Pencil.Core;

	public class ModuleStub : IModule
	{
        string name;

        public ModuleStub(string name)
        {
            this.name = name;
        }

		public Func<IEnumerable<IType>> GetTypesHandler = () => new IType[0];
        public string Name { get { return name; } }
        public IEnumerable<IType> Types { get { return GetTypesHandler(); } }
	}
}