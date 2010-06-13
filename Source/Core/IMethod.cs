using System.Collections.Generic;

namespace Pencil.Core
{
    public interface IMethod : IMember
    {
        ICollection<IMethodArgument> Arguments { get; }
		IType ReturnType { get; }
        IEnumerable<Instruction> Body { get; }
		IEnumerable<IMethod> Calls { get; }
		bool IsGenerated { get; }
		bool IsSpecialName { get; }
		bool IsConstructor { get; }
		
		object Invoke(object target, params object[] args);
    }
}
