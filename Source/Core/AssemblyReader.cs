namespace Pencil.Core
{
    public class AssemblyReader
    {
        IHandler handler;

        public AssemblyReader(IHandler handler)
        {
            this.handler = handler;
        }

        public void Read(IAssembly assembly)
        {
            handler.BeginAssembly(assembly);
            assembly.Modules.ForEach(Read);
            handler.EndAssembly();
        }

        void Read(IModule module)
        {
            handler.BeginModule(module);
            module.Types.ForEach(Read);
            handler.EndModule();
        }

        void Read(IType type)
        {
            handler.BeginType(type);
            type.Methods.ForEach(Read);
            handler.EndType();
        }

        void Read(IMethod method)
        {
            handler.BeginMethod(method);
            handler.EndMethod();
        }
    }
}