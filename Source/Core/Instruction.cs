namespace Pencil.Core
{
    using System;
    using System.Globalization;

    struct Instruction : IInstruction
    {
        int offset;
        object operand;

        static internal Instruction GetNext(ITokenResolver tokenResolver, byte[] ilstream, ref int position)
        {
            var result = new Instruction();
            var converter = new ByteConverter(ilstream, position);
            int offset = converter.ReadByte();
            if(offset < 0xFE)
                result.offset = NormalizeOffset(offset);
            else
                result.offset = offset << 8 | converter.ReadSByte();
            switch(result.GetOpcode().Parameter)
            {
                case ParameterType.SByte: result.operand = converter.ReadSByte(); break;
                case ParameterType.Int32: result.operand = converter.ReadInt32(); break;
                case ParameterType.Int64: result.operand = converter.ReadInt64(); break;
                case ParameterType.Single: result.operand = converter.ReadSingle(); break;
                case ParameterType.Double: result.operand = converter.ReadDouble(); break;
                case ParameterType.Method: result.operand = tokenResolver.Resolve(converter.ReadInt32()); break;
                case ParameterType.Type: goto case ParameterType.Method;
                case ParameterType.Field: goto case ParameterType.Method;
                case ParameterType.Token: goto case ParameterType.Method;
                case ParameterType.String: result.operand = string.Format(CultureInfo.InvariantCulture, "\"{0}\"", tokenResolver.Resolve(converter.ReadInt32())); break;
            }
            return result;
        }

        static int NormalizeOffset(int offset)
        {
            var fudge = new[] { 0xA5, 0xBA, 0xC3, 0xC6, 0xFF };
            var sugar = new[] { 0xB3 - 0xA5 - 1, 0xC2 - 0xBA - 1, 0xC6 - 0xC3 - 1, 0xD0 - 0xC6 - 1, 0 };
            var x = 0;
            for(int i = 0; offset > fudge[i]; ++i)
                x += sugar[i];
            return offset - x;
        }

        public bool IsCall
        {
            get { throw new NotImplementedException(); }
        }

        public object Operand { get { return operand; } }

        public override string ToString()
        {
            string name = GetOpcode().Name;
            if(operand == null)
                return name;
            return string.Format(CultureInfo.InvariantCulture, "{0} {1}", name, operand);
        }

        private Opcode GetOpcode()
        {
            if(offset < 0xFE)
                return Opcode.basic[offset];
            return Opcode.extended[offset & 0xFF];
        }
    }
}
