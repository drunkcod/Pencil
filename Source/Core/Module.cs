namespace Pencil.Core
{
	using System;
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
			var method = module.ResolveMethod(token);
			var ctor = method as System.Reflection.ConstructorInfo;
			if(ctor != null)
				return Method.Wrap(ctor);
			var info = method as System.Reflection.MethodInfo;
			if(info != null)
				return Method.Wrap(info);
			throw new NotSupportedException(method.GetType().Name + " not supported.");
        }
        #endregion
    }
}
