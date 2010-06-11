using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using ReflectionModule = System.Reflection.Module;

namespace Pencil.Core
{
	public class MethodDecodeException : Exception
	{
		public MethodDecodeException(IMethod method, Exception inner):
			base(string.Format("Failed to Decode method {0}.", method.Name), inner)
		{}
	}

    class MethodBody
    {
        readonly ITypeLoader typeLoader;
        readonly MethodBase method;

        public MethodBody(ITypeLoader typeLoader, MethodBase method) {
            this.typeLoader = typeLoader;
            this.method = method;
        }

        public IEnumerable<IMethod> Calls {
            get {
                return DecodeBody().Where(x => x.IsCall).Select(x => x.Operand as IMethod);
            }
        }

        public IInstruction[] DecodeBody() {
            var tokens = new TokenResolver(typeLoader, method.Module, method.DeclaringType, method);
            var body = method.GetMethodBody();
            if (body == null)
                return new IInstruction[0];
            return new Disassembler(tokens).Decode(body.GetILAsByteArray()).ToArray();
        }
    }


	public class PencilMethod : IMethod
	{
        ITypeLoader typeLoader;
        IType declaringType;
        MethodBase method;
        MethodBody body;
        IType returnType;

		internal PencilMethod(ITypeLoader typeLoader, IType declaringType, MethodBase method, IType returnType, MethodBody body)
		{
            this.typeLoader = typeLoader;
            this.declaringType = declaringType;
            this.method = method;
			this.returnType = returnType;
            this.body = body;
		}

		public string Name { get { return method.Name; } }

		public IType DeclaringType { get { return declaringType; } }

		public IEnumerable<IMethod> Calls { get { return body.Calls; } }

        public ICollection<IMethodArgument> Arguments
        {
            get { return method.GetParameters().Map<ParameterInfo, IMethodArgument>(typeLoader.FromNative).ToList(); }
        }

		public IType ReturnType { get { return returnType; } }

		public IEnumerable<IInstruction> Body { get { return body.DecodeBody(); } }

		public override string ToString()
		{
			return "{0} {1}.{2}({3})".InvariantFormat(ReturnType.FullName, DeclaringType.FullName, Name, FormatArguments());
		}

		string FormatArguments()
		{
			if(Arguments.Count == 0)
				return string.Empty;
			var args = new StringBuilder();
			string format = "{0}";
			foreach(var item in Arguments)
			{
				args.AppendFormat(format, item.Type.FullName);
				format = ", {0}";
			}
			return args.ToString();
		}

		public bool IsGenerated { get { return method.IsGenerated(); } }
		public bool IsSpecialName { get { return method.IsSpecialName; } }
		public bool IsConstructor { get { return method.IsConstructor; } }
		
		public object Invoke(object target, params object[] args)
		{
		    return method.Invoke(target, args);
		}
	}
}
