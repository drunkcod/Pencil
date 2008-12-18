namespace Pencil.Test.Stubs
{
	using System;
	using System.Collections.Generic;
	using Pencil.Core;

	public class TypeStub : IType
	{
		public Func<ICollection<IType>> GetDependsOnHandler = () => new IType[0];
		public Func<bool> GetIsGeneratedHandler = () => false;
		public Func<bool> GetIsGenericParameterHandler = () => false;

        string name;
        public TypeStub(string name)
        {
            this.name = name;
        }

        public string Name { get { return name; } }

		public Func<IEnumerable<IMethod>> GetMethodsHandler = () => new IMethod[0];
		public IEnumerable<IMethod> Methods { get { return GetMethodsHandler(); } }
		public ICollection<IType> DependsOn { get { return GetDependsOnHandler(); } }
		public bool IsGenerated { get { return GetIsGeneratedHandler(); } }
		public bool IsGenericParameter { get { return GetIsGenericParameterHandler(); } }
	}
}