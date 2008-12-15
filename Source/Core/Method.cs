namespace Pencil.Core
{
	using System.Collections.Generic;
	using System.Reflection;
    using ReflectionModule = System.Reflection.Module;

	public class Method : IMethod
	{
        MethodBase method;
        
        public static Method Wrap(MethodInfo method)
		{
			return new Method(method);
		}

		internal Method(MethodBase method)
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

        public ICollection<IMethodArgument> Arguments
        {
            get { return method.GetParameters().Map<ParameterInfo, IMethodArgument>(MethodArgument.Wrap).ToList(); }
        }

		public IEnumerable<IInstruction> Body
		{
			get
			{
				var dissassembler = new Disassembler(new Module(method.Module));
				return dissassembler.Decode(method.GetMethodBody().GetILAsByteArray());
			}
		}
	}
}