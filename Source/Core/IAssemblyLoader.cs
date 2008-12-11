namespace Pencil.Core
{
	using System.Reflection;

	public interface IAssemblyLoader
	{
		IAssembly Load(AssemblyName name);
	}
}