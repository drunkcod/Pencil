namespace Pencil.Core
{
	using System.Collections.Generic;
	using System.Reflection;

	public class Method : IMethod
	{
		public static Method From(MethodInfo method)
		{
			return new Method(method);
		}

		MethodBase method;

		private Method(MethodBase method)
		{
			this.method = method;
		}

		public string Name { get { return method.Name; } }

		public IEnumerable<IMethod> Calls
		{
			get
			{
				foreach(var instruction in Body)
					if(instruction.IsCall)
						yield return instruction.Operand as IMethod;
			}
		}

		public IEnumerable<IInstruction> Body
		{
			get
			{
				var dissassembler = new Disassembler(new ReflectionTokenResolver(method.Module));
				return dissassembler.Decode(method.GetMethodBody().GetILAsByteArray());
			}
		}

		class ReflectionTokenResolver : ITokenResolver
		{
			Module module;

			public ReflectionTokenResolver(Module module)
			{
				this.module = module;
			}

			public object Resolve(int token)
			{
				return null;
			}

			public IMethod ResolveMethod(int token)
			{
				return new Method(module.ResolveMethod(token));
			}
		}
	}
}