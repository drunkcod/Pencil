using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Pencil.Core
{
    public static class TypeLoader
    {
        public static IType FromNative(System.Type type) {
            return new Type(type);
        }

        public static IMethod FromNative(MethodInfo method) {
            return new Method(FromNative(method.DeclaringType), method, FromNative(method.ReturnType));
        }

        public static IMethod FromNative(ConstructorInfo ctor) {
            return new Method(FromNative(ctor.DeclaringType), ctor, FromNative(ctor.DeclaringType));
        }
    }
}
