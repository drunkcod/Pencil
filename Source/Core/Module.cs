namespace Pencil.Core
{
	using System;
    using System.Collections.Generic;
    using ReflectionModule = System.Reflection.Module;

    class Module : IModule
    {
        ReflectionModule module;
        public Module(ReflectionModule module)
        {
            this.module = module;
        }

        public string Name { get { return module.Name; } }

        public IEnumerable<IType> Types
        {
            get { return module.GetTypes().Map<System.Type, IType>(TypeLoader.FromNative); }
        }
    }
}
