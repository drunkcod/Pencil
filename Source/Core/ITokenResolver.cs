namespace Pencil.Core
{
    public interface ITokenResolver
    {
        string ResolveString(int token);
		IMethod ResolveMethod(int token);
    }
}
