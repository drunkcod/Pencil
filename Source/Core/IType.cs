namespace Pencil.Core
{
    using System.Collections.Generic;

    public interface IType
    {
        string Name { get; }
        IEnumerable<IMethod> Methods { get; }
		bool IsGenerated { get; }
    }
}
