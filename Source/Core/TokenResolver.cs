namespace Pencil.Core
{
	using System;
	using ReflectionModule = System.Reflection.Module;
	using MethodBase = System.Reflection.MethodBase;
	using SystemType = System.Type;

	public class TokenResolver : ITokenResolver
	{
        readonly ITypeLoader typeLoader;
		readonly ReflectionModule module;
		readonly SystemType[] typeArguments;
		readonly SystemType[] methodArguments;

		public TokenResolver(ITypeLoader typeLoader, ReflectionModule module, SystemType type, MethodBase method)
		{
            this.typeLoader = typeLoader;
			this.module = module;
            if(type != null)
                this.typeArguments = type.GetGenericArguments();
            if(!(method.IsConstructor || method.IsSpecialName) && method.IsGenericMethod)
                this.methodArguments = method.GetGenericArguments();
		}

		public object Resolve(int token)
		{
		    return null;
		}

        public IType ResolveType(int token) {
            return typeLoader.FromNative(module.ResolveType(token, typeArguments, methodArguments));
        }

        public object ResolveField(int token) {
            var field = module.ResolveField(token, typeArguments, methodArguments);
            return string.Format("{0} {1}::{2}", field.FieldType.FullName, field.DeclaringType.FullName, field.Name);
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