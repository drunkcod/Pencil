namespace Pencil.Core
{
	class InstructionReader
	{
		ByteConverter stream;
		ITokenResolver tokens;

		public InstructionReader(ByteConverter stream, ITokenResolver tokens)
		{
			this.stream = stream;
			this.tokens = tokens;
		}

		public Instruction Next()
		{
			var offset = ReadOffset();
			return new Instruction(offset, ReadOperand(offset));
		}

		int ReadOffset()
		{
			int offset = stream.ReadByte();
			if(offset < Opcode.MultiByteTag)
				return Opcode.NormalizeOffset(offset);
			else
				return offset << 8 | stream.ReadSByte();
		}

		object ReadOperand(int offset)
		{
			switch(Opcode.FromOffset(offset).Parameter)
			{
				case ParameterType.SByte: return stream.ReadSByte();
				case ParameterType.Int32: return stream.ReadInt32();
				case ParameterType.Int64: return stream.ReadInt64();
				case ParameterType.Single: return stream.ReadSingle();
				case ParameterType.Double: return stream.ReadDouble();
				case ParameterType.Method: return tokens.ResolveMethod(stream.ReadInt32());
				case ParameterType.Type: goto case ParameterType.Token;
				case ParameterType.Field: goto case ParameterType.Token;
				case ParameterType.Token: return tokens.Resolve(stream.ReadInt32());
				case ParameterType.String: return "\"{0}\"".InvariantFormat(tokens.Resolve(stream.ReadInt32()));
			}
			return null;
		}
	}
}