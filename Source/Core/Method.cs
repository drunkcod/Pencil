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
        IType declaringType;
        MethodBase method;
		IType returnType;

		internal Method(IType declaringType, MethodBase method, IType returnType)
		{
            this.declaringType = declaringType;
            this.method = method;
			this.returnType = returnType;
		}

		public string Name { get { return method.Name; } }

		public IType DeclaringType { get { return declaringType; } }

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
            get { return method.GetParameters().Map<ParameterInfo, IMethodArgument>(TypeLoader.FromNative).ToList(); }
        }

		public IType ReturnType { get { return returnType; } }

		public IEnumerable<IInstruction> Body
		{
			get
			{
				try
				{
					return Disassembler.Decode(method);
				}
				catch(ArgumentOutOfRangeException e)
				{
					throw new MethodDecodeException(this, e);
				}
			}
		}

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
		
		public object Invoke(object target, params object[] args)
		{
		    return method.Invoke(target, args);
		}
	}
}
