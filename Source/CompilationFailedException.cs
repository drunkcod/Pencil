namespace Pencil.Build
{
	using System;
	using System.CodeDom.Compiler;
	using System.Text;

	class CompilationFailedException : Exception
	{
		public CompilationFailedException(CompilerResults results): base(GetOutput(results)){}

        static string GetOutput(CompilerResults results)
        {
            var message = new StringBuilder();
            foreach(var s in results.Output)
                message.AppendLine(s);
            return message.ToString();
        }
	}
}