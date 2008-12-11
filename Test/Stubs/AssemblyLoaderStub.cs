namespace Pencil.Test.Stubs
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using Pencil.Core;

	class AssemblyLoaderStub : IAssemblyLoader
	{
		public Action<AssemblyName> Loading = DoNothing;

		public IAssembly Load(AssemblyName name)
		{
			Loading(name);
			return new AssemblyStub(name);
		}

		static void DoNothing(AssemblyName name){}
	}
}