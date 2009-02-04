namespace Pencil.Core
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Text;
    using ReflectionModule = System.Reflection.Module;

	public class MethodDecodeException : Exception
	{
		public MethodDecodeException(IMethod method, Exception inner):
			base(string.Format("Failed to Decode method {0}.", method.Name), inner)
		{}
	}

	public class Method : IMethod
	{
        MethodBase method;
		IType returnType;

        public static Method Wrap(MethodInfo method)
		{
			return new Method(method, method.ReturnType);
		}

        public static Method Wrap(ConstructorInfo ctor)
		{
			return new Method(ctor, ctor.DeclaringType);
		}

		Method(MethodBase method, System.Type returnType)
		{
			this.method = method;
			this.returnType = Type.Wrap(returnType);
		}

		public string Name { get { return method.Name; } }

		public IType DeclaringType { get { return Type.Wrap(method.DeclaringType); } }

		public IEnumerable<IMethod> Calls
		{
			get
			{
				foreach(var instruction in Body)
					if(instruction.IsCall)
						yield return instruction.Operand as IMethod;
			}
		}

        public ICollection<IMethodArgument> Arguments
        {
            get { return method.GetParameters().Map<ParameterInfo, IMethodArgument>(MethodArgument.Wrap).ToList(); }
        }

		public IType ReturnType { get { return returnType; } }

		public IEnumerable<IInstruction> Body
		{
			get
			{
				var dissassembler = new Disassembler(new TokenResolver(method.Module, method.DeclaringType, method));
				try
				{
					return dissassembler.Decode(GetIL());
				}
				catch(ArgumentOutOfRangeException e)
				{
					throw new MethodDecodeException(this, e);
				}
			}
		}

		public override string ToString()
		{
			return "{0} {1}({2})".InvariantFormat(ReturnType, Name, FormatArguments());
		}

		string FormatArguments()
		{
			if(Arguments.Count == 0)
				return string.Empty;
			var args = new StringBuilder();
			string format = "{0} {1}";
			foreach(var item in Arguments)
			{
				args.AppendFormat(format, item.Type, item.Name);
				format = ", {0} {1}";
			}
			return args.ToString();
		}

		byte[] GetIL()
		{
			var body = method.GetMethodBody();
			if(body == null)
				return new byte[0];
			return body.GetILAsByteArray();
		}

		public bool IsGenerated { get { return method.IsGenerated(); } }
		public bool IsSpecialName { get { return method.IsSpecialName; } }
		public bool IsConstructor { get { return method.IsConstructor; } }
	}
}