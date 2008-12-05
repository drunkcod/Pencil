namespace Pencil.Core
{
    using System;
    using System.Globalization;

    struct Instruction : IInstruction
    {
        int offset;
        object operand;

		internal Instruction(int offset)
		{
			this.offset = offset;
			this.operand = null;
		}

        public bool IsCall { get { return Opcode.IsCall; } }

        public object Operand
		{
			get { return operand; }
			set { operand = value; }
		}

        public override string ToString()
        {
            string name = Opcode.Name;
            if(operand == null)
                return name;
            return string.Format(CultureInfo.InvariantCulture, "{0} {1}", name, operand);
        }

        internal Opcode Opcode
        {
			get
			{
				if(offset < 0xFE)
					return Opcode.basic[offset];
				return Opcode.extended[offset & 0xFF];
			}
        }
    }
}
