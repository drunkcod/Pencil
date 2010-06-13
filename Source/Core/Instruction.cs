namespace Pencil.Core
{
    using System;

    public struct Instruction
    {
        int offset;
        object operand;

		internal Instruction(int offset, object operand)
		{
			this.offset = offset;
			this.operand = operand;
		}

        public bool IsCall { get { return Opcode.IsCall; } }

        public object Operand
		{
			get { return operand; }
		}

        public override string ToString()
        {
            string name = Opcode.Name;
            if(operand == null)
                return name;
            return "{0} {1}".InvariantFormat(name, operand);
        }

        public Opcode Opcode { get { return Opcode.FromOffset(offset); } }
    }
}
