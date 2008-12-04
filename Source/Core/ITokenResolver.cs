namespace Pencil.Core
{
    public interface ITokenResolver
    {
        object Resolve(int token);
		IMethod ResolveMethod(int token);
    }
}
