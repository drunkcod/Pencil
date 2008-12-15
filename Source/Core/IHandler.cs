namespace Pencil.Core
{
    public interface IHandler
    {
        void BeginAssembly(IAssembly assembly);
        void EndAssembly();

        void BeginModule(IModule module);
        void EndModule();

        void BeginType(IType type);
        void EndType();

        void BeginMethod(IMethod method);
        void EndMethod();
    }
}