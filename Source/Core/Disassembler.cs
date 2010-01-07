namespace Pencil.Core
{
	using System.Collections.Generic;
using System.Reflection;

    public class Disassembler
    {
        ITokenResolver tokens;

        public static IEnumerable<IInstruction> Decode(MethodInfo method) 
        {
            var tokens = new TokenResolver(method.Module, method.DeclaringType, method);
			var il = method.GetMethodBody().GetILAsByteArray();
            var stream = new ByteConverter(il, 0);
			var ir = new InstructionReader(stream, tokens);
			while(stream.Position < il.Length)
				yield return ir.Next();
        }

        public Disassembler(ITokenResolver tokens)
        {
            this.tokens = tokens;
        }

        public IInstruction[] Decode(params byte[] il)
        {
			var result = new List<IInstruction>();
			var stream = new ByteConverter(il, 0);
			var ir = new InstructionReader(stream, tokens);
			while(stream.Position < il.Length)
				result.Add(ir.Next());
            return result.ToArray();
        }
    }
}
