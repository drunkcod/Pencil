using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Pencil.Core
{
    public interface ITypeLoader
    {
        IType FromNative(System.Type type);
        IMethod FromNative(MethodInfo method);
        IMethod FromNative(ConstructorInfo method);
        IMethodArgument FromNative(ParameterInfo parameter);
        IField FromNative(FieldInfo field);
    }

    public class DefaultTypeLoader : ITypeLoader
    {
        readonly Dictionary<Type, IType> typeCache = new Dictionary<Type, IType>();

        public IType FromNative(Type type) {
            IType cached;
            if(typeCache.TryGetValue(type, out cached))
                return cached;
            cached = new PencilType(this, type);
            typeCache.Add(type, cached);
            return cached;
        }

        public IMethod FromNative(MethodInfo method) {
            var body = new PencilMethodBody(method);
            return new PencilMethod(this, method, FromNative(method.ReturnType), body);
        }

        public IMethod FromNative(ConstructorInfo ctor) {
            var body = new PencilMethodBody(ctor);
            return new PencilMethod(this, ctor, FromNative(ctor.DeclaringType), body);
        }

        public IMethodArgument FromNative(ParameterInfo parameter) {
            return new MethodArgument(parameter.Name, FromNative(parameter.ParameterType));
        }

        public IField FromNative(FieldInfo field) { return new PencilField(this, field); }
    }
}
