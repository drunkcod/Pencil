namespace Pencil.Core
{
	using System;
	using System.Text;

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

		class SwitchTable
		{
			readonly int[] offsets;
			public SwitchTable(int[] offsets)
			{
				this.offsets = offsets;
			}

			public override string ToString()
			{
				var builder = new StringBuilder("[");
				var sep = string.Empty;
				for(int i = 0; i != offsets.Length; ++i)
				{
					builder.AppendFormat("{0}{1}", sep, offsets[i]);
					sep = ", ";
				}
				return builder.Append(']').ToString();
			}
		}

		object ReadOperand(int offset)
		{
			var opcode = Opcode.FromOffset(offset);
			try
			{
				switch(opcode.Parameter)
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
					case ParameterType.Array:
						var items = new int[stream.ReadInt32()];
						for(int i = 0; i != items.Length; ++i)
							items[i] = stream.ReadInt32();
						return new SwitchTable(items);
				}
				return null;
			}
			catch(ArgumentOutOfRangeException e)
			{
				throw new InvalidOperationException(
					"Error reading operand for {0}.".InvariantFormat(opcode.Name), e);
			}
		}
	}
}