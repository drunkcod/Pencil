using System;
using System.Text;
using System.Collections.Generic;

namespace Pencil.Core
{
	public class InstructionReader
	{
        readonly ITokenResolver tokens;
        readonly ByteConverter stream;

        public InstructionReader(ITokenResolver tokens, byte[] bytes):
            this(tokens, new ByteConverter(bytes, 0)){}

        public InstructionReader(ITokenResolver tokens, ByteConverter stream) {
            this.tokens = tokens;
            this.stream = stream;
		}

		internal Instruction Next() {
			var offset = ReadOffset();
			return new Instruction(offset, ReadOperand(offset));
		}

        public IEnumerable<Instruction> ReadToEnd() {
            while (stream.HasData)
                yield return Next();
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
                    case ParameterType.Type: return tokens.ResolveType(stream.ReadInt32());
                    case ParameterType.Field: return tokens.ResolveField(stream.ReadInt32());
					case ParameterType.Token:
                        stream.ReadInt32();
                        return "<unsupported>";
					case ParameterType.String: return "\"{0}\"".InvariantFormat(tokens.ResolveString(stream.ReadInt32()));
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