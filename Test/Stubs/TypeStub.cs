namespace Pencil.Test.Stubs
{
	using System;
	using System.Collections.Generic;
	using Pencil.Core;

	public class TypeStub : IType
	{
		public Func<IEnumerable<IMethod>> GetMethodsHandler = () => new IMethod[0];
		public IEnumerable<IMethod> Methods { get { return GetMethodsHandler(); } }
	}
}