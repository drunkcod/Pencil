namespace Pencil.Core
{
	using System.Collections.Generic;
	using System.Reflection;
    using ReflectionModule = System.Reflection.Module;

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
				return dissassembler.Decode(GetIL());
			}
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