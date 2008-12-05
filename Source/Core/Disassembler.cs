namespace Pencil.Core
{
	using System.Collections.Generic;

    public class Disassembler
    {
        ITokenResolver tokens;

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
