namespace Pencil.Core
{
	using System.Collections.Generic;
	using System.IO;
	using ReflectionAssembly = System.Reflection.Assembly;
	using AssemblyName = System.Reflection.AssemblyName;

	public class AssemblyLoader : IAssemblyLoader
    {
        public static IAssembly Load(string assemblyPath) { return null; }

		public static IAssembly LoadFrom(string path)
		{
			return new Assembly(new DefaultTypeLoader(), ReflectionAssembly.LoadFrom(path));
		}

		public static IAssembly GetExecutingAssembly()
		{
            return new Assembly(new DefaultTypeLoader(), ReflectionAssembly.GetCallingAssembly());
        }

		public IAssembly Load(AssemblyName assembly)
		{
			try
			{
				return new Assembly(new DefaultTypeLoader(), ReflectionAssembly.Load(assembly));
			}
			catch(FileLoadException)
			{
				return new MissingAssembly();
			}
			catch(FileNotFoundException)
			{
				return new MissingAssembly();
			}
		}
    }
}