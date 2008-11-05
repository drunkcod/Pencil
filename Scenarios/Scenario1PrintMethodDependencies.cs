namespace Pencil.Scenarios.Scenario1
{
    using System;
    using Pencil.Core;

	class PrintMethodRefrences
	{
		public void Run(string assemblyPath)
		{
			var assembly = AssemblyLoader.Load(assemblyPath);
			foreach(var module in assembly.Modules)
				foreach(var type in module.Types)
					foreach(var method in type.Methods)
					{
						Console.WriteLine("{0} calls:", method);
						foreach(var instruction in method.Body.Instructions)
							if(instruction.IsCall)
								Console.WriteLine(instruction.Operand);
					}
		}
	}
}