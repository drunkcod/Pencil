namespace Pencil.Core
{
	using System.Reflection;
	using System.Collections.Generic;

	public class MissingAssembly : IAssembly
	{
		AssemblyName name;

		public MissingAssembly(): this(new AssemblyName("Missing.Assembly")){}

		public MissingAssembly(AssemblyName name)
		{
			this.name = name;
		}

		public AssemblyName Name { get { return name; } }

		public IEnumerable<AssemblyName> ReferencedAssemblies
		{
			get { return new AssemblyName[0]; }
		}

		public IEnumerable<IModule> Modules
		{
			get { return new IModule[0]; }
		}

		public bool IsMissing { get { return true; } }
	}
}