namespace Pencil.Core
{
    public class Disassembler
    {
        ITokenResolver tokenResolver;

        public Disassembler(ITokenResolver tokenResolver)
        {
            this.tokenResolver = tokenResolver;
        }

        public IInstruction[] Decode(params byte[] ilBytes)
        {
            int position = 0;
            IInstruction[] result = { Instruction.GetNext(tokenResolver, ilBytes, ref position)};

            return result;
        }
    }
}
