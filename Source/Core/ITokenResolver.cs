using System.Collections.Generic;

namespace Pencil.Core
{
    public interface ITokenResolver
    {
        string ResolveString(int token);
        IType ResolveType(int token);
        IField ResolveField(int token);
		IMethod ResolveMethod(int token);
    }
}
