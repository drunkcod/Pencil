using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Pencil.Core
{
    public static class TypeLoader
    {
        static Dictionary<System.Type, IType> typeCache = new Dictionary<System.Type, IType>();

        public static IType FromNative(System.Type type) {
            IType cached;
            if(typeCache.TryGetValue(type, out cached))
                return cached;
            cached = new Type(type);
            typeCache.Add(type, cached);
            return cached;
        }

        public static IMethod FromNative(MethodInfo method) {
            return new Method(FromNative(method.DeclaringType), method, FromNative(method.ReturnType));
        }

        public static IMethod FromNative(ConstructorInfo ctor) {
            return new Method(FromNative(ctor.DeclaringType), ctor, FromNative(ctor.DeclaringType));
        }

        public static IMethodArgument FromNative(ParameterInfo parameter) {
            return new MethodArgument(parameter.Name, FromNative(parameter.ParameterType));
        }
    }
}
