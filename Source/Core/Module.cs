namespace Pencil.Core
{
    using System.Collections.Generic;
    using ReflectionModule = System.Reflection.Module;

    class Module : IModule, ITokenResolver
    {
        ReflectionModule module;
        public Module(ReflectionModule module)
        {
            this.module = module;
        }

        public string Name { get { return module.Name; } }

        public IEnumerable<IType> Types
        {
            get { return module.GetTypes().Map<System.Type, IType>(Type.Wrap); }
        }

        #region ITokenResolver Members
        public object Resolve(int token)
        {
            return null;
        }

        public IMethod ResolveMethod(int token)
        {
            return new Method(module.ResolveMethod(token));
        }
        #endregion
    }
}
