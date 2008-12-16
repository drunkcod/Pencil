namespace Pencil.Core
{
	using System.Collections.Generic;

    public interface IMethod
    {
		string Name { get; }
        ICollection<IMethodArgument> Arguments { get; }
        IEnumerable<IInstruction> Body { get; }
		IType DeclaringType { get; }
		bool IsGenerated { get; }
		bool IsSpecialName { get; }
    }
}