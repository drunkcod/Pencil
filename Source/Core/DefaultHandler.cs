namespace Pencil.Core
{
	public class DefaultHandler : IHandler
	{
		IAssembly assembly;
		IModule module;
		IType type;
		IMethod method;

		public void BeginAssembly(IAssembly assembly)
		{
			this.assembly = assembly;
			BeginAssemblyCore();
		}
        public void EndAssembly(){}

        public void BeginModule(IModule module)
		{
			this.module = module;
			BeginModuleCore();
		}
        public void EndModule(){}

        public void BeginType(IType type)
		{
			this.type = type;
			BeginTypeCore();
		}
        public void EndType(){}

        public void BeginMethod(IMethod method)
		{
			this.method = method;
			BeginMethodCore();
		}
        public void EndMethod(){}

		protected IAssembly Assembly { get { return assembly; } }
		protected IModule Module { get { return module; } }
		protected IType Type { get { return type; } }
		protected IMethod Method { get { return method; } }

		protected virtual void BeginAssemblyCore(){}
		protected virtual void BeginModuleCore(){}
		protected virtual void BeginTypeCore(){}
		protected virtual void BeginMethodCore(){}
	}
}