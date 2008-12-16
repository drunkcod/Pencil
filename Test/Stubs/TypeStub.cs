namespace Pencil.Test.Stubs
{
	using System;
	using System.Collections.Generic;
	using Pencil.Core;

	public class TypeStub : IType
	{
        string name;
        public TypeStub(string name)
        {
            this.name = name;
        }

        public string Name { get { return name; } }

		public Func<IEnumerable<IMethod>> GetMethodsHandler = () => new IMethod[0];
		public IEnumerable<IMethod> Methods { get { return GetMethodsHandler(); } }
		public bool IsGenerated { get { return false; } }
	}
}