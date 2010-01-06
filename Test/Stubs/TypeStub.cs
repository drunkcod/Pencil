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
		public Func<bool> GetIsPublicHandler = () => false;
		public Func<IType> GetElementTypeHandler;
		public Func<IEnumerable<IMethod>> GetMethodsHandler = () => new IMethod[0];
		public Func<IEnumerable<IType>> GetNestedTypesHandler = () => new IType[0];
		public Converter<System.Type, bool> IsAHandler = x => false;

        string name;
		string fullName;

        public TypeStub(string name): this(string.Empty, name){}

        public TypeStub(string namespaceName, string name)
        {
            this.name = name;
			this.fullName = namespaceName + name;
			this.GetElementTypeHandler = () => this;
        }

        public string Name { get { return name; } }
		public string FullName { get { return fullName; } }
		public IType ElementType { get { return GetElementTypeHandler(); } }
		public IEnumerable<IMethod> Methods { get { return GetMethodsHandler(); } }
		public ICollection<IType> DependsOn { get { return GetDependsOnHandler(); } }
		public IEnumerable<IType> NestedTypes { get { return GetNestedTypesHandler(); } }
		public bool IsGenerated { get { return GetIsGeneratedHandler(); } }
		public bool IsGenericParameter { get { return GetIsGenericParameterHandler(); } }
		public bool IsPublic { get { return GetIsPublicHandler(); } }
		public bool IsA<T>(){ return IsAHandler(typeof(T)); }

        public override string ToString() {
            return name;
        }
	}
}