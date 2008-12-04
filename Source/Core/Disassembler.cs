namespace Pencil.Core
{
	using System.Collections.Generic;

    public class Disassembler
    {
        ITokenResolver tokenResolver;

        public Disassembler(ITokenResolver tokenResolver)
        {
            this.tokenResolver = tokenResolver;
        }

        public IInstruction[] Decode(params byte[] ilBytes)
        {
			var result = new List<IInstruction>();
			for(int position = 0; position < ilBytes.Length;)
				result.Add(Instruction.GetNext(tokenResolver, ilBytes, ref position));
            return result.ToArray();
        }
    }
}
