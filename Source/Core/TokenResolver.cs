using System;
using System.Reflection;
using System.Collections.Generic;

namespace Pencil.Core
{
	public class TokenResolver : ITokenResolver
	{
        readonly ITypeLoader typeLoader;
		readonly Module module;
		readonly Type[] typeArguments;
		readonly Type[] methodArguments;

		public TokenResolver(ITypeLoader typeLoader, MethodBase method) {
            this.typeLoader = typeLoader;
			this.module = method.Module;
            var type = method.DeclaringType;
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
            return typeLoader.FromNative(field);
        }

        public string ResolveString(int token) {
            return module.ResolveString(token);
        }

		public IMethod ResolveMethod(int token)
		{
			var method = module.ResolveMethod(token, typeArguments, methodArguments);
            if (method.IsConstructor)
				return typeLoader.FromNative((ConstructorInfo)method);
			var info = method as System.Reflection.MethodInfo;
			if(info != null)
				return typeLoader.FromNative(info);
			throw new NotSupportedException(method.GetType().Name + " not supported.");
		}
	}
}