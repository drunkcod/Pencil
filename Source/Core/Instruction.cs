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
                result.offset = offset << 8 | converter.ReadInt8();
            switch(result.GetOpcode().Parameter)
            {
                case ParameterType.Int8: result.operand = converter.ReadInt8(); break;
                case ParameterType.Int32: result.operand = converter.ReadInt32(); break;
                case ParameterType.Int64: result.operand = converter.ReadInt64(); break;
                case ParameterType.Float32: result.operand = converter.ReadFloat32(); break;
                case ParameterType.Float64: result.operand = converter.ReadFloat64(); break;              
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

        public object Operand
        {
            get { throw new NotImplementedException(); }
        }

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
                return opcode[offset];
            return extended[offset & 0xFF];
        }

        enum ParameterType
        {
            None, Int8, Int32, Int64, Float32, Float64, Method, Type, String, Field, Token
        }

        struct Opcode
        {
            internal static Opcode Reserved = new Opcode("<reserved>");
            public string Name;
            public ParameterType Parameter;

            public Opcode(string name): this(name, ParameterType.None){ }
            public Opcode(string name, ParameterType parameter)
            {
                Name = name;
                Parameter = parameter;
            }
        }
        #region Single Byte Opcodes
        static readonly Opcode[] opcode =
        {
            new Opcode("nop"),
            new Opcode("break"),
            new Opcode("ldarg.0"),
            new Opcode("ldarg.1"),
            new Opcode("ldarg.2"),
            new Opcode("ldarg.3"),
            new Opcode("ldloc.0"),
            new Opcode("ldloc.1"),
            new Opcode("ldloc.2"),
            new Opcode("ldloc.3"),
            new Opcode("stloc.0"),
            new Opcode("stloc.1"),
            new Opcode("stloc.2"),
            new Opcode("stloc.3"),
            new Opcode("ldarg.s", ParameterType.Int8),
            new Opcode("ldarga.s", ParameterType.Int8),
            new Opcode("starg.s", ParameterType.Int8),
            new Opcode("ldloc.s", ParameterType.Int8),
            new Opcode("ldloca.s", ParameterType.Int8),
            new Opcode("stloc.s", ParameterType.Int8),
            new Opcode("ldnull"),
            new Opcode("ldc.i4.m1"),
            new Opcode("ldc.i4.0"),
            new Opcode("ldc.i4.1"),
            new Opcode("ldc.i4.2"),
            new Opcode("ldc.i4.3"),
            new Opcode("ldc.i4.4"),
            new Opcode("ldc.i4.5"),
            new Opcode("ldc.i4.6"),
            new Opcode("ldc.i4.7"),
            new Opcode("ldc.i4.8"),
            new Opcode("ldc.i4.s", ParameterType.Int8),
            new Opcode("ldc.i4", ParameterType.Int32),
            new Opcode("ldc.i8", ParameterType.Int64),
            new Opcode("ldc.r4", ParameterType.Float32),
            new Opcode("ldc.r8", ParameterType.Float64),
            Opcode.Reserved,
            new Opcode("dup"),
            new Opcode("pop"),
            new Opcode("jmp", ParameterType.Method),
            new Opcode("call", ParameterType.Method),
            new Opcode("calli", ParameterType.Method),
            new Opcode("ret"),
            new Opcode("br.s", ParameterType.Int8),
            new Opcode("brfalse.s", ParameterType.Int8),
            new Opcode("brtrue.s", ParameterType.Int8),
            new Opcode("beq.s", ParameterType.Int8),
            new Opcode("bge.s", ParameterType.Int8),
            new Opcode("bgt.s", ParameterType.Int8),
            new Opcode("ble.s", ParameterType.Int8),
            new Opcode("blt.s", ParameterType.Int8),
            new Opcode("bne.un.s", ParameterType.Int8),
            new Opcode("bge.un.s", ParameterType.Int8),
            new Opcode("bgt.un.s", ParameterType.Int8),
            new Opcode("ble.un.s", ParameterType.Int8),
            new Opcode("blt.un.s", ParameterType.Int8),
            new Opcode("br", ParameterType.Int32),
            new Opcode("brfalse", ParameterType.Int32),
            new Opcode("brtrue", ParameterType.Int32),
            new Opcode("beq", ParameterType.Int32),
            new Opcode("bge", ParameterType.Int32),
            new Opcode("bgt", ParameterType.Int32),
            new Opcode("ble", ParameterType.Int32),
            new Opcode("blt", ParameterType.Int32),
            new Opcode("bne.un", ParameterType.Int32),  
            new Opcode("bge.un", ParameterType.Int32),
            new Opcode("bgt.un", ParameterType.Int32),
            new Opcode("ble.un", ParameterType.Int32),
            new Opcode("blt.un", ParameterType.Int32),
            new Opcode("switch"),
            new Opcode("ldind.i1"),
            new Opcode("ldind.u1"),
            new Opcode("ldind.i2"),
            new Opcode("ldind.u2"),
            new Opcode("ldind.i4"),
            new Opcode("ldind.u4"),
            new Opcode("ldind.i8"),
            new Opcode("ldind.i"),
            new Opcode("ldind.r4"),
            new Opcode("ldind.r8"),
            new Opcode("ldind.ref"),
            new Opcode("stind.ref"),
            new Opcode("stind.i1"),
            new Opcode("stind.i2"),
            new Opcode("stind.i4"),
            new Opcode("stind.i8"),
            new Opcode("stind.r4"),
            new Opcode("stind.r8"),
            new Opcode("add"),
            new Opcode("sub"),
            new Opcode("mul"),
            new Opcode("div"),
            new Opcode("div.un"),
            new Opcode("rem"),
            new Opcode("rem.un"),
            new Opcode("and"),
            new Opcode("or"),
            new Opcode("xor"),
            new Opcode("shl"),
            new Opcode("shr"),
            new Opcode("shr.un"),
            new Opcode("neg"),
            new Opcode("not"),
            new Opcode("conv.i1"),
            new Opcode("conv.i2"),
            new Opcode("conv.i4"),
            new Opcode("conv.i8"),
            new Opcode("conv.r4"),
            new Opcode("conv.r8"),
            new Opcode("conv.u4"),
            new Opcode("conv.u8"),
            new Opcode("callvirt", ParameterType.Method),
            new Opcode("cpobj", ParameterType.Type),
            new Opcode("ldobj", ParameterType.Type),
            new Opcode("ldstr", ParameterType.String),
            new Opcode("newobj", ParameterType.Method),
            new Opcode("castclass", ParameterType.Type),
            new Opcode("isinst", ParameterType.Type), 
            new Opcode("conv.r.un"),
            Opcode.Reserved,
            Opcode.Reserved,
            new Opcode("unbox", ParameterType.Type),
            new Opcode("throw"),
            new Opcode("ldfld", ParameterType.Field),
            new Opcode("ldflda", ParameterType.Field),
            new Opcode("stfld", ParameterType.Field),
            new Opcode("ldsfld", ParameterType.Field),
            new Opcode("ldsflda", ParameterType.Field),
            new Opcode("stsfld", ParameterType.Field),
            new Opcode("stobj", ParameterType.Type),
            new Opcode("conv.ovf.i1.un"),
            new Opcode("conv.ovf.i2.un"),
            new Opcode("conv.ovf.i4.un"),
            new Opcode("conv.ovf.i8.un"),
            new Opcode("conv.ovf.u1.un"),
            new Opcode("conv.ovf.u2.un"),
            new Opcode("conv.ovf.u4.un"),
            new Opcode("conv.ovf.u8.un"),
            new Opcode("conv.ovf.i.un"),
            new Opcode("conv.ovf.u.un"),
            new Opcode("box", ParameterType.Type),
            new Opcode("newarr", ParameterType.Type),
            new Opcode("ldlen"),
            new Opcode("ldelema", ParameterType.Type),
            new Opcode("ldelem.i1"),
            new Opcode("ldelem.u1"),
            new Opcode("ldelem.i2"),
            new Opcode("ldelem.u2"),
            new Opcode("ldelem.i4"),
            new Opcode("ldelem.u4"),
            new Opcode("ldelem.i8"),
            new Opcode("ldelem.i"),
            new Opcode("ldelem.r4"),
            new Opcode("ldelem.r8"),
            new Opcode("ldelem.ref"),
            new Opcode("stelem.i"),
            new Opcode("stelem.i1"),
            new Opcode("stelem.i2"),
            new Opcode("stelem.i4"),
            new Opcode("stelem.i8"),
            new Opcode("stelem.r4"),
            new Opcode("stelem.r8"),
            new Opcode("stelem.ref"),
            new Opcode("ldelem", ParameterType.Type),
            new Opcode("stelem", ParameterType.Type),
            new Opcode("unbox.any", ParameterType.Type),
            new Opcode("conv.ovf.i1"),
            new Opcode("conv.ovf.u1"),
            new Opcode("conv.ovf.i2"),
            new Opcode("conv.ovf.u2"),
            new Opcode("conv.ovf.i4"),
            new Opcode("conv.ovf.u4"),
            new Opcode("conv.ovf.i8"),
            new Opcode("conv.ovf.u8"),
            new Opcode("refanyval", ParameterType.Type),
            new Opcode("ckfinite"),
            new Opcode("mkrefany", ParameterType.Type),
            new Opcode("ldtoken", ParameterType.Token),
            new Opcode("conv.u2"),
            new Opcode("conv.u1"),
            new Opcode("conv.i"),
            new Opcode("conv.ovf.i"),
            new Opcode("conv.ovf.u"),
            new Opcode("add.ovf"),
            new Opcode("add.ovf.un"),
            new Opcode("mul.ovf"),
            new Opcode("mul.ovf.un"),
            new Opcode("sub.ovf"),
            new Opcode("sub.ovf.un"),
            new Opcode("endfinally"),
            new Opcode("leave", ParameterType.Int32),
            new Opcode("leave.s", ParameterType.Int8),
            new Opcode("stind.i"),
            new Opcode("conv.u")
        };
        #endregion
        #region Multi Byte Opcodes
        static readonly Opcode[] extended = 
        {
            new Opcode("arglist"),
            new Opcode("ceq"),
            new Opcode("cgt"),
            new Opcode("cgt.un"),
            new Opcode("clt"),
            new Opcode("clt.un"),
            new Opcode("ldftn", ParameterType.Method),
            new Opcode("ldvirtftn", ParameterType.Method),
            Opcode.Reserved,
            new Opcode("ldarg", ParameterType.Int32),
            new Opcode("ldarg.a", ParameterType.Int32),
            new Opcode("starg", ParameterType.Int32),
            new Opcode("ldloc", ParameterType.Int32),
            new Opcode("ldloca", ParameterType.Int32), 
            new Opcode("stloc", ParameterType.Int32),
            new Opcode("localloc"),
            Opcode.Reserved,
            new Opcode("endfilter"),
            new Opcode("unaligned."),
            new Opcode("volatile."),
            new Opcode("tail."),
            new Opcode("initobj", ParameterType.Type),
            new Opcode("constrained.", ParameterType.Type),
            new Opcode("cpblk"),
            new Opcode("initblk"),
            Opcode.Reserved,
            new Opcode("rethrow"),
            Opcode.Reserved,
            new Opcode("sizeof", ParameterType.Type),
            new Opcode("refanytype"),
            new Opcode("readonly.")
        };
        #endregion
    }
}
