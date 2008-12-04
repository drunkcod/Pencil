namespace Pencil.Core
{
	using System.Collections.Generic;

    public interface IMethod
    {
        IEnumerable<IInstruction> Body { get; }
    }
}
