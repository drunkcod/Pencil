using System.Collections.Generic;
using System.Reflection;

namespace Pencil.Core
{
    public class Disassembler
    {
        ITokenResolver tokens;

        public static IEnumerable<IInstruction> Decode(MethodBase method) 
        {
            var tokens = new TokenResolver(new DefaultTypeLoader(), method.Module, method.DeclaringType, method);
            var body = method.GetMethodBody();
            if(body == null)
                return new IInstruction[0];
            return Decode(tokens, body.GetILAsByteArray());
        }

        public Disassembler(ITokenResolver tokens)
        {
            this.tokens = tokens;
        }

        public IInstruction[] Decode(params byte[] il)
        {
			var result = new List<IInstruction>(Decode(tokens, il));
            return result.ToArray();
        }

        static IEnumerable<IInstruction> Decode(ITokenResolver tokens, byte[] il) {
            var stream = new ByteConverter(il, 0);
            var ir = new InstructionReader(stream, tokens);
            while(stream.Position < il.Length)
                yield return ir.Next();
        }

    }
}
