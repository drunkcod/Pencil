using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace Pencil.Core
{
	public class MethodDecodeException : Exception
	{
		public MethodDecodeException(IMethod method, Exception inner):
			base(string.Format("Failed to Decode method {0}.", method.Name), inner)
		{}
	}

    public class PencilMethodBody
    {
        static readonly Instruction[] Empty = new Instruction[0];

        readonly ITypeLoader typeLoader;
        readonly MethodBase method;

        public PencilMethodBody(ITypeLoader typeLoader, MethodBase method) {
            this.typeLoader = typeLoader;
            this.method = method;
        }

        public IEnumerable<IMethod> Calls {
            get {
                return DecodeBody().Where(x => x.IsCall).Select(x => x.Operand as IMethod);
            }
        }

        public IEnumerable<Instruction> DecodeBody() {
            var body = method.GetMethodBody();
            if (body == null)
                return Empty;
            var tokens = new TokenResolver(typeLoader, method);
            var ir = new InstructionReader(tokens, body.GetILAsByteArray());
            return ir.ReadToEnd();
        }
    }

	public class PencilMethod : IMethod
	{
        readonly ITypeLoader typeLoader;
        readonly MethodBase method;
        readonly PencilMethodBody body;
        readonly IType returnType;

		internal PencilMethod(ITypeLoader typeLoader, MethodBase method, IType returnType, PencilMethodBody body)
		{
            this.typeLoader = typeLoader;
            this.method = method;
            this.body = body;
            this.returnType = returnType;
		}

		public string Name { get { return method.Name; } }

		public IType DeclaringType { get { return typeLoader.FromNative(method.DeclaringType); } }

        public ICollection<IMethodArgument> Arguments {
            get { return method.GetParameters().Map<ParameterInfo, IMethodArgument>(typeLoader.FromNative).ToList(); }
        }

        public IEnumerable<Instruction> Body { get { return body.DecodeBody(); } }
        
        public IEnumerable<IMethod> Calls { get { return body.Calls; } }

		public IType ReturnType { get { return returnType; } }

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

        public override bool Equals(object obj) {
            var other = obj as PencilMethod;
            return other != null && method.Equals(other.method);
        }
        public override int GetHashCode() {
            return method.GetHashCode();
        }
	}
}
