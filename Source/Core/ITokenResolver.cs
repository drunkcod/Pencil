namespace Pencil.Core
{
    public interface ITokenResolver
    {
        object Resolve(int token);
		object ResolveMethod(int token);
    }
}
