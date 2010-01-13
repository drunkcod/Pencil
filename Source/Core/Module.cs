namespace Pencil.Core
{
	using System;
    using System.Collections.Generic;
    using ReflectionModule = System.Reflection.Module;

    class Module : IModule
    {
        ITypeLoader typeLoader;
        ReflectionModule module;

        public Module(ITypeLoader typeLoader, ReflectionModule module)
        {
            this.typeLoader = typeLoader;
            this.module = module;
        }

        public string Name { get { return module.Name; } }

        public IEnumerable<IType> Types
        {
            get { return module.GetTypes().Map<System.Type, IType>(typeLoader.FromNative); }
        }
    }
}
