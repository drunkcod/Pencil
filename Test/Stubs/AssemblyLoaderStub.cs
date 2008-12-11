namespace Pencil.Test.Stubs
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using Pencil.Core;

	class AssemblyLoaderStub : IAssemblyLoader
	{
		Dictionary<string, IAssembly> assemblies = new Dictionary<string, IAssembly>();

		public Action<AssemblyName> Loading = DoNothing;

		public void Add(IAssembly assembly)
		{
			assemblies.Add(assembly.Name.Name, assembly);
		}

		public IAssembly Load(AssemblyName name)
		{
			Loading(name);
			IAssembly assembly;
			if(assemblies.TryGetValue(name.Name, out assembly))
				return assembly;
			return new AssemblyStub(name);
		}

		static void DoNothing(AssemblyName name){}
	}
}