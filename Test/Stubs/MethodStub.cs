namespace Pencil.Test.Stubs
{
	using System;
	using System.Collections.Generic;
	using Pencil.Core;

	public class MethodStub : IMethod
	{
		string name;

		public Func<IEnumerable<Instruction>> GetBodyHandler = () => new Instruction[0];
		public Func<IType> GetDeclaringTypeHandler = () => null;
		public Func<bool> GetIsGeneratedHandler = () => false;
		public Func<bool> GetIsSpecialNameHandler = () => false;
		public Func<bool> GetIsConstructorHandler = () => false;
		public Func<IType> GetReturnTypeHandler = () => null;
		public Func<IEnumerable<IMethod>> GetCallsHandler = () => new IMethod[0];
		public Func<object, object[], object> InvokeHandler = (self, args) => null;

		public MethodStub(string name)
		{
			this.name = name;
		}

		public string Name { get {return name; } }
        public ICollection<IMethodArgument> Arguments { get { return new IMethodArgument[0]; } }
		public IType ReturnType { get { return GetReturnTypeHandler(); } }
		public IEnumerable<Instruction> Body { get { return GetBodyHandler(); } }
		public IEnumerable<IMethod> Calls { get { return GetCallsHandler(); } }
		public IType DeclaringType { get { return GetDeclaringTypeHandler(); } }
		public bool IsGenerated { get { return GetIsGeneratedHandler(); } }
		public bool IsSpecialName { get { return GetIsSpecialNameHandler(); } }
		public bool IsConstructor { get { return GetIsConstructorHandler(); } }
		public object Invoke(object instance, params object[] args)
		{ 
		    return InvokeHandler(instance, args); 
		}

        public override string ToString()
        {
            return name;
        }
	}
}
