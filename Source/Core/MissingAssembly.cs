namespace Pencil.Core
{
	using System.Reflection;
	using System.Collections.Generic;

	public class MissingAssembly : IAssembly
	{
		public AssemblyName Name { get { return new AssemblyName("Missing.Assembly"); } }

		public IEnumerable<AssemblyName> ReferencedAssemblies
		{
			get { return new AssemblyName[0]; }
		}

		public IEnumerable<IModule> Modules
		{
			get { return new IModule[0]; }
		}
	}
}