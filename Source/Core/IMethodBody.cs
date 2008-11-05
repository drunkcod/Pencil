namespace Pencil.Core
{
    using System.Collections.Generic;

    public interface IMethodBody
    {
        IEnumerable<IInstruction> Instructions { get; }
    }
}
