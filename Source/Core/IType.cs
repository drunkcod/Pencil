namespace Pencil.Core
{
    using System.Collections.Generic;

    public interface IType
    {
        IEnumerable<IMethod> Methods { get; }
    }
}
