using System;
using System.IO;
using AssemblyName = System.Reflection.AssemblyName;
using ReflectionAssembly = System.Reflection.Assembly;

namespace Pencil.Core
{
	public class AssemblyLoader : IAssemblyLoader
    {
        string binPath;

        public IAssembly LoadFrom(string path) {
            binPath = Path.GetDirectoryName(path);
            return new Assembly(new DefaultTypeLoader(), ReflectionAssembly.LoadFrom(path));
        }

        public IAssembly Load(string assemblyPath) { return null; }

		public static IAssembly GetExecutingAssembly() {
            return new Assembly(new DefaultTypeLoader(), ReflectionAssembly.GetCallingAssembly());
        }

		IAssembly IAssemblyLoader.Load(AssemblyName assembly) {
			try {
				return new Assembly(new DefaultTypeLoader(), Load(assembly));
			} catch(FileLoadException) {
				return new MissingAssembly(assembly);
			} catch(FileNotFoundException) {
                return new MissingAssembly(assembly);
			}
		}

        ReflectionAssembly Load(AssemblyName assembly) {
            try {
                return ReflectionAssembly.Load(assembly);
            } catch (FileLoadException) {
                var altPath = Path.Combine(binPath, assembly.Name + ".dll");
                if (File.Exists(altPath))
                    return ReflectionAssembly.LoadFrom(altPath);
                throw;
            }
        }
    }
}