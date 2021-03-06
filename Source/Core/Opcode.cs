namespace Pencil.Core
{
	using System;

    [Flags]
	internal enum ParameterType : short
	{
		None    = 0,
        SByte   = 1,
        UInt16   = 1 << 2,
        Int32   = 1 << 3, 
        Int64   = 1 << 4,
        Single  = 1 << 5,
        Double  = 1 << 6,
        Method  = 1 << 7,
        Type    = 1 << 8,
        String  = 1 << 9,
        Field   = 1 << 10,
        Token   = 1 << 11,
        Array   = 1 << 12,
        Max = Array,
        Mask = (Max << 1) - 1, 
        Member = Method | Field
	}

    [Flags]
    enum OpFlags : short
    {
        None = 0,
        Call = ParameterType.Max << 1
    }

	public struct Opcode
	{
		internal const int MultiByteTag = 0xFE;
		internal static Opcode Reserved = new Opcode("<reserved>");

		public static Opcode FromName(string name)
		{
			Opcode opcode;
			Predicate<Opcode> hasName = x => x.Name == name;
			if(basic.TryFind(hasName, out opcode) || extended.TryFind(hasName, out opcode))
				return opcode;
			throw new NotSupportedException();
		}

		internal static Opcode FromOffset(int offset)
		{
			if(offset < MultiByteTag)
				return Opcode.basic[offset];
			return Opcode.extended[offset & 0xFF];
		}

		public readonly string Name;
        readonly short flags;

        internal ParameterType Parameter { get { return (ParameterType)(flags & (short)ParameterType.Mask); } }
        public bool IsCall { get { return (flags & (short)OpFlags.Call) != 0; } }

		Opcode(string name): this(name, ParameterType.None){ }
		Opcode(string name, ParameterType parameter): this(name, parameter, false){ }
		Opcode(string name, ParameterType parameter, bool isCall) {
			Name = name;
            flags = (short)((short)parameter | (short)(isCall ? OpFlags.Call : OpFlags.None));
		}

        public override string ToString() {
            return Name;
        }

		static Opcode Call(string name){ return new Opcode(name, ParameterType.Method, true); }

        public static Opcode Nop { get { return basic[0]; } }

		#region Single Byte Opcodes
		static readonly Opcode[] basic =
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
			new Opcode("ldarg.s", ParameterType.SByte),
			new Opcode("ldarga.s", ParameterType.SByte),
			new Opcode("starg.s", ParameterType.SByte),
			new Opcode("ldloc.s", ParameterType.SByte),
			new Opcode("ldloca.s", ParameterType.SByte),
			new Opcode("stloc.s", ParameterType.SByte),
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
			new Opcode("ldc.i4.s", ParameterType.SByte),
			new Opcode("ldc.i4", ParameterType.Int32),
			new Opcode("ldc.i8", ParameterType.Int64),
			new Opcode("ldc.r4", ParameterType.Single),
			new Opcode("ldc.r8", ParameterType.Double),
			Opcode.Reserved,
			new Opcode("dup"),
			new Opcode("pop"),
			new Opcode("jmp", ParameterType.Method),
			Opcode.Call("call"),
			Opcode.Call("calli"),
			new Opcode("ret"),
			new Opcode("br.s", ParameterType.SByte),
			new Opcode("brfalse.s", ParameterType.SByte),
			new Opcode("brtrue.s", ParameterType.SByte),
			new Opcode("beq.s", ParameterType.SByte),
			new Opcode("bge.s", ParameterType.SByte),
			new Opcode("bgt.s", ParameterType.SByte),
			new Opcode("ble.s", ParameterType.SByte),
			new Opcode("blt.s", ParameterType.SByte),
			new Opcode("bne.un.s", ParameterType.SByte),
			new Opcode("bge.un.s", ParameterType.SByte),
			new Opcode("bgt.un.s", ParameterType.SByte),
			new Opcode("ble.un.s", ParameterType.SByte),
			new Opcode("blt.un.s", ParameterType.SByte),
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
			new Opcode("switch", ParameterType.Array),
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
			Opcode.Call("callvirt"),
			new Opcode("cpobj", ParameterType.Type),
			new Opcode("ldobj", ParameterType.Type),
			new Opcode("ldstr", ParameterType.String),
			Opcode.Call("newobj"),
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
/*0xA5*/	new Opcode("unbox.any", ParameterType.Type),
/*0xA6*/    Opcode.Reserved,
/*0xA7*/    Opcode.Reserved,
/*0xA8*/    Opcode.Reserved,
/*0xA9*/    Opcode.Reserved,
/*0xAA*/    Opcode.Reserved,
/*0xAB*/    Opcode.Reserved,
/*0xAC*/    Opcode.Reserved,
/*0xAD*/    Opcode.Reserved,
/*0xAE*/    Opcode.Reserved,
/*0xAF*/    Opcode.Reserved,
/*0xB0*/    Opcode.Reserved,
/*0xB1*/    Opcode.Reserved,
/*0xB2*/    Opcode.Reserved,
/*0xB3*/	new Opcode("conv.ovf.i1"),
			new Opcode("conv.ovf.u1"),
			new Opcode("conv.ovf.i2"),
			new Opcode("conv.ovf.u2"),
			new Opcode("conv.ovf.i4"),
			new Opcode("conv.ovf.u4"),
			new Opcode("conv.ovf.i8"),
/*0xBA*/	new Opcode("conv.ovf.u8"),
/*0xBB*/    Opcode.Reserved,
/*0xBC*/    Opcode.Reserved,
/*0xBD*/    Opcode.Reserved,
/*0xBE*/    Opcode.Reserved,
/*0xBF*/    Opcode.Reserved,
/*0xC0*/    Opcode.Reserved,
/*0xC1*/    Opcode.Reserved,
/*0xC2*/	new Opcode("refanyval", ParameterType.Type),
/*0xC3*/    new Opcode("ckfinite"),
/*0xC4*/    Opcode.Reserved,
/*0xC5*/    Opcode.Reserved,
/*0xC6*/	new Opcode("mkrefany", ParameterType.Type),
/*0xC7*/    Opcode.Reserved,
/*0xC8*/    Opcode.Reserved,
/*0xC9*/    Opcode.Reserved,
/*0xCA*/    Opcode.Reserved,
/*0xCB*/    Opcode.Reserved,
/*0xCC*/    Opcode.Reserved,
/*0xCD*/    Opcode.Reserved,
/*0xCE*/    Opcode.Reserved,
/*0xCF*/    Opcode.Reserved,
/*0xD0*/	new Opcode("ldtoken", ParameterType.Token),
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
			new Opcode("leave.s", ParameterType.SByte),
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
			new Opcode("ldarg", ParameterType.UInt16),
			new Opcode("ldarg.a", ParameterType.Int32),
			new Opcode("starg", ParameterType.Int32),
			new Opcode("ldloc", ParameterType.Int32),
			new Opcode("ldloca", ParameterType.Int32),
			new Opcode("stloc", ParameterType.UInt16),
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