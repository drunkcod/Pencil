namespace Pencil.Core
{
    using System.Collections.Generic;

    public interface IType
    {
        string Name { get; }
		string FullName { get; }
		IType ElementType { get; }
        IEnumerable<IMethod> Methods { get; }
		ICollection<IType> DependsOn { get; }
		bool IsGenerated { get; }
		bool IsGenericParameter { get; }
    }
}
