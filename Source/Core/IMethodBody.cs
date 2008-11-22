namespace Pencil.Core
{
    using System.Collections.Generic;

    public interface IMethodBody
    {
        IEnumerator<IInstruction> GetEnumerator();
    }
}
