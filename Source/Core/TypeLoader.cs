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
    }

    public class DefaultTypeLoader : ITypeLoader
    {
        static Dictionary<System.Type, IType> typeCache = new Dictionary<System.Type, IType>();

        public IType FromNative(System.Type type) {
            IType cached;
            if(typeCache.TryGetValue(type, out cached))
                return cached;
            cached = new Type(this, type);
            typeCache.Add(type, cached);
            return cached;
        }

        public IMethod FromNative(MethodInfo method) {
            return new Method(this, FromNative(method.DeclaringType), method, FromNative(method.ReturnType), () => DecodeBody(method));
        }

        public IMethod FromNative(ConstructorInfo ctor) {
            return new Method(this, FromNative(ctor.DeclaringType), ctor, FromNative(ctor.DeclaringType), () => DecodeBody(ctor));
        }

        public IMethodArgument FromNative(ParameterInfo parameter) {
            return new MethodArgument(parameter.Name, FromNative(parameter.ParameterType));
        }

        IInstruction[] DecodeBody(MethodBase method) {
            var tokens = new TokenResolver(this, method.Module, method.DeclaringType, method);
            var body = method.GetMethodBody();
            if(body == null)
                return new IInstruction[0];
            return new Disassembler(tokens).Decode(body.GetILAsByteArray()).ToArray();
        }
    }
}
