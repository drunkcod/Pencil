using System;
using System.Reflection;

namespace Pencil.Core
{
    public interface IField
    {
        string Name { get; }
    }

	public class TokenResolver : ITokenResolver
	{
        readonly ITypeLoader typeLoader;
		readonly Module module;
		readonly Type[] typeArguments;
		readonly Type[] methodArguments;

		public TokenResolver(ITypeLoader typeLoader, Module module, Type type, MethodBase method)
		{
            this.typeLoader = typeLoader;
			this.module = module;
            if(type != null)
                this.typeArguments = type.GetGenericArguments();
            if(!(method.IsConstructor || method.IsSpecialName) && method.IsGenericMethod)
                this.methodArguments = method.GetGenericArguments();
		}

        public IType ResolveType(int token) {
            return typeLoader.FromNative(module.ResolveType(token, typeArguments, methodArguments));
        }

        public object ResolveField(int token) {
            var field = module.ResolveField(token, typeArguments, methodArguments);
            return new PencilField(field);
        }

        public string ResolveString(int token) {
            return module.ResolveString(token);
        }

		public IMethod ResolveMethod(int token)
		{
				var method = module.ResolveMethod(token, typeArguments, methodArguments);
				var ctor = method as System.Reflection.ConstructorInfo;
				if(ctor != null)
					return typeLoader.FromNative(ctor);
				var info = method as System.Reflection.MethodInfo;
				if(info != null)
					return typeLoader.FromNative(info);
				throw new NotSupportedException(method.GetType().Name + " not supported.");
		}
	}
}