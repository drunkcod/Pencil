namespace Pencil.Core
{
	using ReflectionAssembly = System.Reflection.Assembly;

	public static class AssemblyLoader
    {
        public static IAssembly Load(string assemblyPath) { return null; }

		public static IAssembly GetExecutingAssembly(){
            return new Assembly(ReflectionAssembly.GetCallingAssembly());
        }
    }
}
