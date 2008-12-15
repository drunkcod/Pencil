namespace Pencil.Core
{
	using System.Collections.Generic;

    public interface IMethod
    {
		string Name { get; }
        IEnumerable<IInstruction> Body { get; }
    }
}