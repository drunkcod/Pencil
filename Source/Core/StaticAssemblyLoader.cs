namespace Pencil.Core
{
	using System.Reflection;
	using System.Collections.Generic;

	public class StaticAssemblyLoader : IAssemblyLoader
	{
		Dictionary<string, IAssembly> assemblies = new Dictionary<string, IAssembly>();

		public IAssembly Load(AssemblyName name)
		{
			IAssembly registred;
			if(assemblies.TryGetValue(name.Name, out registred))
				return registred;
			return new MissingAssembly(name);
		}

		public void Register(IAssembly assembly)
		{
			assemblies.Add(assembly.Name.Name, assembly);
		}
	}
}