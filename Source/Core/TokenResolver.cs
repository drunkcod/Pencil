namespace Pencil.Core
{
	using System;
	using ReflectionModule = System.Reflection.Module;
	using MethodBase = System.Reflection.MethodBase;
	using SystemType = System.Type;

	public class TokenResolver : ITokenResolver
	{
		ReflectionModule module;
		SystemType[] typeArguments;
		SystemType[] methodArguments;

		public TokenResolver(ReflectionModule module, SystemType type, MethodBase method)
		{
			this.module = module;
			this.typeArguments = type.GetGenericArguments();
            if(!(method.IsConstructor || method.IsSpecialName))
                this.methodArguments = method.GetGenericArguments();
		}

		public object Resolve(int token)
		{
		    return null;
		}

        public IType ResolveType(int token) {
            return Type.Wrap(module.ResolveType(token, typeArguments, methodArguments));
        }

        public object ResolveField(int token) {
            var field = module.ResolveField(token);
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
					return Method.Wrap(ctor);
				var info = method as System.Reflection.MethodInfo;
				if(info != null)
					return Method.Wrap(info);
				throw new NotSupportedException(method.GetType().Name + " not supported.");
		}
	}
}