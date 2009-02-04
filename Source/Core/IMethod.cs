namespace Pencil.Core
{
	using System.Collections.Generic;

    public interface IMethod
    {
		string Name { get; }
        ICollection<IMethodArgument> Arguments { get; }
		IType ReturnType { get; }
        IEnumerable<IInstruction> Body { get; }
		IEnumerable<IMethod> Calls { get; }
		IType DeclaringType { get; }
		bool IsGenerated { get; }
		bool IsSpecialName { get; }
		bool IsConstructor { get; }
		
		object Invoke(object target, params object[] args);
    }
}
