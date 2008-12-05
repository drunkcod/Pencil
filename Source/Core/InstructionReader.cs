namespace Pencil.Core
{
	using System.Globalization;
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
			var result = new Instruction(ReadOffset());
			result.Operand = ReadOperand(result.Opcode.Parameter);
			return result;
		}

		int ReadOffset()
		{
			int offset = stream.ReadByte();
			if(offset < 0xFE)
				return NormalizeOffset(offset);
			else
				return offset << 8 | stream.ReadSByte();
		}

		object ReadOperand(ParameterType parameterType)
		{
			switch(parameterType)
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
				case ParameterType.String: return string.Format(CultureInfo.InvariantCulture, "\"{0}\"", tokens.Resolve(stream.ReadInt32()));
			}
			return null;
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
	}
}