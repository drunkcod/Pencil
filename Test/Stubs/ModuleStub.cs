namespace Pencil.Test.Stubs
{
	using System;
	using System.Collections.Generic;
	using Pencil.Core;

	public class ModuleStub : IModule
	{
		public Func<IEnumerable<IType>> GetTypesHandler = () => new IType[0];
		public IEnumerable<IType> Types { get { return GetTypesHandler(); } }
	}
}