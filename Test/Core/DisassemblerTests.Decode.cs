namespace Pencil.Test.Core
{
    using System;
    using NUnit.Framework;

    public partial class DisassemblerTests
    {
        [Test]
        public void Decode_nop()
        {
            CheckDecode("nop", 0x00);
        }
        [Test]
        public void Decode_break()
        {
            CheckDecode("break", 0x01);
        }
        [Test]
        public void Decode_ldarg_0()
        {
            CheckDecode("ldarg.0", 0x02);
        }
        [Test]
        public void Decode_ldarg_1()
        {
            CheckDecode("ldarg.1", 0x03);
        }
        [Test]
        public void Decode_ldarg_2()
        {
            CheckDecode("ldarg.2", 0x04);
        }
        [Test]
        public void Decode_ldarg_3()
        {
            CheckDecode("ldarg.3", 0x05);
        }
        [Test]
        public void Decode_ldloc_0()
        {
            CheckDecode("ldloc.0", 0x06);
        }
        [Test]
        public void Decode_ldloc_1()
        {
            CheckDecode("ldloc.1", 0x07);
        }
        [Test]
        public void Decode_ldloc_2()
        {
            CheckDecode("ldloc.2", 0x08);
        }
        [Test]
        public void Decode_ldloc_3()
        {
            CheckDecode("ldloc.3", 0x09);
        }
        [Test]
        public void Decode_stloc_0()
        {
            CheckDecode("stloc.0", 0x0A);
        }
        [Test]
        public void Decode_stloc_1()
        {
            CheckDecode("stloc.1", 0x0B);
        }
        [Test]
        public void Decode_stloc_2()
        {
            CheckDecode("stloc.2", 0x0C);
        }
        [Test]
        public void Decode_stloc_3()
        {
            CheckDecode("stloc.3", 0x0D);
        }
        [Test]
        public void Decode_ldarg_s()
        {
            CheckDecode("ldarg.s 7", 0x0E, 0x07);
        }
        [Test]
        public void Decode_ldarga_s()
        {
            CheckDecode("ldarga.s 3", 0x0F, 0x03);
        }
        [Test]
        public void Decode_starg_s()
        {
            CheckDecode("starg.s 1", 0x10, 0x1);
        }
        [Test]
        public void Decode_ldloc_s()
        {
            CheckDecode("ldloc.s 9", 0x11, 0x09);
        }
        [Test]
        public void Decode_ldloca_s()
        {
            CheckDecode("ldloca.s 2", 0x12, 0x02);
        }
        [Test]
        public void Decode_stloc_s()
        {
            CheckDecode("stloc.s 4", 0x13, 0x04);
        }
        [Test]
        public void Decode_ldnull()
        {
            CheckDecode("ldnull", 0x14);
        }
        [Test]
        public void Decode_ldc_i4_m1()
        {
            CheckDecode("ldc.i4.m1", 0x15);
        }
        [Test]
        public void Decode_ldc_i4_0()
        {
            CheckDecode("ldc.i4.0", 0x16);
        }
        [Test]
        public void Decode_ldc_i4_1()
        {
            CheckDecode("ldc.i4.1", 0x17);
        }
        [Test]
        public void Decode_ldc_i4_2()
        {
            CheckDecode("ldc.i4.2", 0x18);
        }
        [Test]
        public void Decode_ldc_i4_3()
        {
            CheckDecode("ldc.i4.3", 0x19);
        }
        [Test]
        public void Decode_ldc_i4_4()
        {
            CheckDecode("ldc.i4.4", 0x1A);
        }
        [Test]
        public void Decode_ldc_i4_5()
        {
            CheckDecode("ldc.i4.5", 0x1B);
        }
        [Test]
        public void Decode_ldc_i4_6()
        {
            CheckDecode("ldc.i4.6", 0x1C);
        }
        [Test]
        public void Decode_ldc_i4_7()
        {
            CheckDecode("ldc.i4.7", 0x1D);
        }
        [Test]
        public void Decode_ldc_i4_8()
        {
            CheckDecode("ldc.i4.8", 0x1E);
        }
        [Test]
        public void Decode_ldc_i4_s()
        {
            CheckDecode("ldc.i4.s 42", 0x1F, 42);
        }
        [Test]
        public void Decode_ldc_i4()
        {
            CheckDecode("ldc.i4 16909060", 0x20, 4, 3, 2, 1);
        }
        [Test]
        public void Decode_ldc_i8()
        {
            CheckDecode("ldc.i8 72623859790382856", 0x21, 8, 7, 6, 5, 4, 3, 2, 1);
        }
        [Test]
        public void Decode_ldc_r4()
        {
            byte[] bytes = { 0x22, 0, 0, 0, 0 };
            BitConverter.GetBytes(3.1415f).CopyTo(bytes, 1);
            CheckDecode("ldc.r4 3.1415", bytes);
        }
        [Test]
        public void Decode_ldc_r8()
        {
            byte[] bytes = { 0x23, 0, 0, 0, 0, 0, 0, 0, 0 };
            BitConverter.GetBytes(2.73).CopyTo(bytes, 1);
            CheckDecode("ldc.r8 2.73", bytes);
        }
        [Test]
        public void Decode_dup()
        {
            CheckDecode("dup", 0x25);
        }
        [Test]
        public void Decode_pop()
        {
            CheckDecode("pop", 0x26);
        }
        [Test]
        public void Decode_jmp()
        {
            SetResolveToken("instance void Foo::Bar()");
            CheckDecode("jmp instance void Foo::Bar()", 0x27, 0, 0, 0, 0);
        }
        [Test]
        public void Decode_call()
        {
            SetResolveToken("instance void Bar::Baz()");
            CheckDecode("call instance void Bar::Baz()", 0x28, 0, 0, 0, 0);
        }
        [Test]
        public void Decode_calli()
        {
            SetResolveToken("instance void Baz::xyzzy()");
            CheckDecode("calli instance void Baz::xyzzy()", 0x29, 0, 0, 0, 0);
        }
        [Test]
        public void Decode_ret()
        {
            CheckDecode("ret", 0x2A);
        }
        [Test]
        public void Decode_br_s()
        {
            CheckDecode("br.s -3", 0x2B, 253);
        }
        [Test]
        public void Decode_brfalse_s()
        {
            CheckDecode("brfalse.s -7", 0x2C, 249);
        }
        [Test]
        public void Decode_brtrue_s()
        {
            CheckDecode("brtrue.s 5", 0x2D, 5);
        }
        [Test]
        public void Decode_beq_s()
        {
            CheckDecode("beq.s 3", 0x2E, 3);
        }
        [Test]
        public void Decode_bge_s()
        {
            CheckDecode("bge.s 127", 0x2F, 127);
        }
        [Test]
        public void Decode_bgt_s()
        {
            CheckDecode("bgt.s 42", 0x30, 42);
        }
        [Test]
        public void Decode_ble_s()
        {
            CheckDecode("ble.s 81", 0x31, 81);
        }
        [Test]
        public void Decode_blt_s()
        {
            CheckDecode("blt.s 2", 0x32, 2);
        }
        [Test]
        public void Decode_bne_un_s()
        {
            CheckDecode("bne.un.s 1", 0x33, 1);
        }
        [Test]
        public void Decode_bge_un_s()
        {
            CheckDecode("bge.un.s 2", 0x34, 2);
        }
        [Test]
        public void Decode_bgt_un_s()
        {
            CheckDecode("bgt.un.s 3", 0x35, 3);
        }
        [Test]
        public void Decode_ble_un_s()
        {
            CheckDecode("ble.un.s 4", 0x36, 4);
        }
        [Test]
        public void Decode_blt_un_s()
        {
            CheckDecode("blt.un.s 5", 0x37, 5);
        }
        [Test]
        public void Decode_br()
        {
            CheckDecode("br 256", 0x38, 0, 1, 0, 0);
        }
        [Test]
        public void Decode_brfalse()
        {
            CheckDecode("brfalse -1", 0x39, 0xFF, 0xFF, 0xFF, 0xFF);
        }
        [Test]
        public void Decode_brtrue()
        {
            CheckDecode("brtrue -2", 0x3A, 0xFE, 0xFF, 0xFF, 0xFF);
        }
        [Test]
        public void Decode_beq()
        {
            CheckDecode("beq -3", 0x3B, 0xFD, 0xFF, 0xFF, 0xFF);
        }
        [Test]
        public void Decode_bge()
        {
            CheckDecode("bge -4", 0x3C, 0xFC, 0xFF, 0xFF, 0xFF);
        }
        [Test]
        public void Decode_bgt()
        {
            CheckDecode("bgt -5", 0x3D, 0xFB, 0xFF, 0xFF, 0xFF);
        }
        [Test]
        public void Decode_ble()
        {
            CheckDecode("ble -6", 0x3E, 0xFA, 0xFF, 0xFF, 0xFF);
        }
        [Test]
        public void Decode_blt()
        {
            CheckDecode("blt -7", 0x3F, 0xF9, 0xFF, 0xFF, 0xFF);
        }
        [Test]
        public void Decode_bne_un()
        {
            CheckDecode("bne.un -8", 0x40, 0xF8, 0xFF, 0xFF, 0xFF);
        }
        [Test]
        public void Decode_bge_un()
        {
            CheckDecode("bge.un -9", 0x41, 0xF7, 0xFF, 0xFF, 0xFF);
        }
        [Test]
        public void Decode_bgt_un()
        {
            CheckDecode("bgt.un -10", 0x42, 0xF6, 0xFF, 0xFF, 0xFF);
        }
        [Test]
        public void Decode_ble_un()
        {
            CheckDecode("ble.un -11", 0x43, 0xF5, 0xFF, 0xFF, 0xFF);
        }
        [Test]
        public void Decode_blt_un()
        {
            CheckDecode("blt.un -12", 0x44, 0xF4, 0xFF, 0xFF, 0xFF);
        }
        [Test]
        public void Decode_switch()
        {
            CheckDecode("switch [7, 42]", 0x45, 2, 0, 0, 0, 7, 0, 0, 0, 42, 0, 0, 0);
        }
        [Test]
        public void Decode_ldind_i1()
        {
            CheckDecode("ldind.i1", 0x46);
        }
        [Test]
        public void Decode_ldind_u1()
        {
            CheckDecode("ldind.u1", 0x47);
        }
        [Test]
        public void Decode_ldind_i2()
        {
            CheckDecode("ldind.i2", 0x48);
        }
        [Test]
        public void Decode_ldind_u2()
        {
            CheckDecode("ldind.u2", 0x49);
        }
        [Test]
        public void Decode_ldind_i4()
        {
            CheckDecode("ldind.i4", 0x4A);
        }
        [Test]
        public void Decode_ldind_u4()
        {
            CheckDecode("ldind.u4", 0x4B);
        }
        [Test]
        public void Decode_ldind_i8()
        {
            CheckDecode("ldind.i8", 0x4C);
        }
        [Test]
        public void Decode_ldind_i()
        {
            CheckDecode("ldind.i", 0x4D);
        }
        [Test]
        public void Decode_ldind_r4()
        {
            CheckDecode("ldind.r4", 0x4E);
        }
        [Test]
        public void Decode_ldind_r8()
        {
            CheckDecode("ldind.r8", 0x4F);
        }
        [Test]
        public void Decode_ldind_ref()
        {
            CheckDecode("ldind.ref", 0x50);
        }
        [Test]
        public void Decode_stind_ref()
        {
            CheckDecode("stind.ref", 0x51);
        }
        [Test]
        public void Decode_stind_i1()
        {
            CheckDecode("stind.i1", 0x52);
        }
        [Test]
        public void Decode_stind_i2()
        {
            CheckDecode("stind.i2", 0x53);
        }
        [Test]
        public void Decode_stind_i4()
        {
            CheckDecode("stind.i4", 0x54);
        }
        [Test]
        public void Decode_stind_i8()
        {
            CheckDecode("stind.i8", 0x55);
        }
        [Test]
        public void Decode_stind_r4()
        {
            CheckDecode("stind.r4", 0x56);
        }
        [Test]
        public void Decode_stind_r8()
        {
            CheckDecode("stind.r8", 0x57);
        }
        [Test]
        public void Decode_add()
        {
            CheckDecode("add", 0x58);
        }
        [Test]
        public void Decode_sub()
        {
            CheckDecode("sub", 0x59);
        }
        [Test]
        public void Decode_mul()
        {
            CheckDecode("mul", 0x5A);
        }
        [Test]
        public void Decode_div()
        {
            CheckDecode("div", 0x5B);
        }
        [Test]
        public void Decode_div_un()
        {
            CheckDecode("div.un", 0x5C);
        }
        [Test]
        public void Decode_rem()
        {
            CheckDecode("rem", 0x5D);
        }
        [Test]
        public void Decode_rem_un()
        {
            CheckDecode("rem.un", 0x5E);
        }
        [Test]
        public void Decode_and()
        {
            CheckDecode("and", 0x5F);
        }
        [Test]
        public void Decode_or()
        {
            CheckDecode("or", 0x60);
        }
        [Test]
        public void Decode_xor()
        {
            CheckDecode("xor", 0x61);
        }
        [Test]
        public void Decode_shl()
        {
            CheckDecode("shl", 0x62);
        }
        [Test]
        public void Decode_shr()
        {
            CheckDecode("shr", 0x63);
        }
        [Test]
        public void Decode_shr_un()
        {
            CheckDecode("shr.un", 0x64);
        }
        [Test]
        public void Decode_neg()
        {
            CheckDecode("neg", 0x65);
        }
        [Test]
        public void Decode_not()
        {
            CheckDecode("not", 0x66);
        }
        [Test]
        public void Decode_conv_i1()
        {
            CheckDecode("conv.i1", 0x67);
        }
        [Test]
        public void Decode_conv_i2()
        {
            CheckDecode("conv.i2", 0x68);
        }
        [Test]
        public void Decode_conv_i4()
        {
            CheckDecode("conv.i4", 0x69);
        }
        [Test]
        public void Decode_conv_i8()
        {
            CheckDecode("conv.i8", 0x6A);
        }
        [Test]
        public void Decode_conv_r4()
        {
            CheckDecode("conv.r4", 0x6B);
        }
        [Test]
        public void Decode_conv_r8()
        {
            CheckDecode("conv.r8", 0x6C);
        }
        [Test]
        public void Decode_conv_u4()
        {
            CheckDecode("conv.u4", 0x6D);
        }
        [Test]
        public void Decode_conv_u8()
        {
            CheckDecode("conv.u8", 0x6E);
        }
        [Test]
        public void Decode_callvirt()
        {
            SetResolveToken("instance void A::B()");
            CheckDecode("callvirt instance void A::B()", 0x6F, 1, 2, 3, 4);
        }
        [Test]
        public void Decode_cpobj()
        {
            SetResolveToken("Foo.Bar");
            CheckDecode("cpobj Foo.Bar", 0x70, 0, 1, 2, 3);
        }
        [Test]
        public void Decode_ldobj()
        {
            SetResolveToken("Bar.Baz");
            CheckDecode("ldobj Bar.Baz", 0x71, 0, 1, 2, 3);
        }
        [Test]
        public void Decode_ldstr()
        {
            SetResolveToken("Hello World!");
            CheckDecode("ldstr \"Hello World!\"", 0x72, 4, 3, 2, 1);
        }
        [Test]
        public void Decode_newobj()
        {
            SetResolveToken("instance void C::.ctor()");
            CheckDecode("newobj instance void C::.ctor()", 0x73, 1, 2, 3, 4);
        }
        [Test]
        public void Decode_castclass()
        {
            SetResolveToken("class Foo.Bar");
            CheckDecode("castclass class Foo.Bar", 0x74, 1, 2, 3, 4);
        }
        [Test]
        public void Decode_isinst()
        {
            SetResolveToken("class Bar.Baz");
            CheckDecode("isinst class Bar.Baz", 0x75, 1, 2, 3, 4);
        }
        [Test]
        public void Decode_conv_r_un()
        {
            CheckDecode("conv.r.un", 0x76);
        }
        [Test]
        public void Decode_unbox()
        {
            SetResolveToken("class A.B");
            CheckDecode("unbox class A.B", 0x79, 1, 2, 3, 4);
        }
        [Test]
        public void Decode_throw()
        {
            CheckDecode("throw", 0x7A);
        }
        [Test]
        public void Decode_ldfld()
        {
            SetResolveToken("int32 Foo.Bar::node");
            CheckDecode("ldfld int32 Foo.Bar::node", 0x7B, 1, 2, 3, 4);
        }
        [Test]
        public void Decode_ldflda()
        {
            SetResolveToken("int8 Bar.Baz::xyzzy");
            CheckDecode("ldflda int8 Bar.Baz::xyzzy", 0x7C, 1, 2, 3, 4);
        }
        [Test]
        public void Decode_stfld()
        {
            SetResolveToken("float32 A.B::c");
            CheckDecode("stfld float32 A.B::c", 0x7D, 1, 2, 3, 4);
        }
        [Test]
        public void Decode_ldsfld()
        {
            SetResolveToken("int32 A.B::node");
            CheckDecode("ldsfld int32 A.B::node", 0x7E, 1, 2, 3, 4);
        }
        [Test]
        public void Decode_ldsflda()
        {
            SetResolveToken("int32 C.D::xyzzy");
            CheckDecode("ldsflda int32 C.D::xyzzy", 0x7F, 1, 2, 3, 4);
        }
        [Test]
        public void Decode_stsfld()
        {
            SetResolveToken("int8 E.F::byte");
            CheckDecode("stsfld int8 E.F::byte", 0x80, 1, 2, 3, 4);
        }
        [Test]
        public void Decode_stobj()
        {
            SetResolveToken("class Foo.Baz");
            CheckDecode("stobj class Foo.Baz", 0x81, 1, 2, 3, 4);
        }
        [Test]
        public void Decode_conv_ovf_i1_un()
        {
            CheckDecode("conv.ovf.i1.un", 0x82);
        }
        [Test]
        public void Decode_conv_ovf_i2_un()
        {
            CheckDecode("conv.ovf.i2.un", 0x83);
        }
        [Test]
        public void Decode_conv_ovf_i4_un()
        {
            CheckDecode("conv.ovf.i4.un", 0x84);
        }
        [Test]
        public void Decode_conv_ovf_i8_un()
        {
            CheckDecode("conv.ovf.i8.un", 0x85);
        }
        [Test]
        public void Decode_conv_ovf_u1_un()
        {
            CheckDecode("conv.ovf.u1.un", 0x86);
        }
        [Test]
        public void Decode_conv_ovf_u2_un()
        {
            CheckDecode("conv.ovf.u2.un", 0x87);
        }
        [Test]
        public void Decode_conv_ovf_u4_un()
        {
            CheckDecode("conv.ovf.u4.un", 0x88);
        }
        [Test]
        public void Decode_conv_ovf_u8_un()
        {
            CheckDecode("conv.ovf.u8.un", 0x89);
        }
        [Test]
        public void Decode_conv_ovf_i_un()
        {
            CheckDecode("conv.ovf.i.un", 0x8A);
        }
        [Test]
        public void Decode_conv_ovf_u_un()
        {
            CheckDecode("conv.ovf.u.un", 0x8B);
        }
        [Test]
        public void Deocde_box()
        {
            SetResolveToken("class A.B");
            CheckDecode("box class A.B", 0x8C, 0, 1, 2, 3);
        }
        [Test]
        public void Decode_newarr()
        {
            SetResolveToken("class C.D");
            CheckDecode("newarr class C.D", 0x8D, 0, 1, 2, 3);
        }
        [Test]
        public void Decode_ldlen()
        {
            CheckDecode("ldlen", 0x8E);
        }
        [Test]
        public void Decode_ldelema()
        {
            SetResolveToken("class Foo.Bar");
            CheckDecode("ldelema class Foo.Bar", 0x8F, 0, 1, 2, 3);
        }
        [Test]
        public void Decode_ldelem_i1()
        {
            CheckDecode("ldelem.i1", 0x90);
        }
        [Test]
        public void Decode_ldelem_u1()
        {
            CheckDecode("ldelem.u1", 0x91);
        }
        [Test]
        public void Decode_ldelem_i2()
        {
            CheckDecode("ldelem.i2", 0x92);
        }
        [Test]
        public void Decode_ldelem_u2()
        {
            CheckDecode("ldelem.u2", 0x93);
        }
        [Test]
        public void Decode_ldelem_i4()
        {
            CheckDecode("ldelem.i4", 0x94);
        }
        [Test]
        public void Decode_ldelem_u4()
        {
            CheckDecode("ldelem.u4", 0x95);
        }
        [Test]
        public void Decode_ldelem_i8()
        {
            CheckDecode("ldelem.i8", 0x96);
        }
        [Test]
        public void Decode_ldelem_i()
        {
            CheckDecode("ldelem.i", 0x97);
        }
        [Test]
        public void Decode_ldelem_r4()
        {
            CheckDecode("ldelem.r4", 0x98);
        }
        [Test]
        public void Decode_ldelem_r8()
        {
            CheckDecode("ldelem.r8", 0x99);
        }
        [Test]
        public void Decode_ldelem_ref()
        {
            CheckDecode("ldelem.ref", 0x9A);
        }
        [Test]
        public void Decode_stelem_i()
        {
            CheckDecode("stelem.i", 0x9B);
        }
        [Test]
        public void Decode_stelem_i1()
        {
            CheckDecode("stelem.i1", 0x9C);
        }
        [Test]
        public void Decode_stelem_i2()
        {
            CheckDecode("stelem.i2", 0x9D);
        }
        [Test]
        public void Decode_stelem_i4()
        {
            CheckDecode("stelem.i4", 0x9E);
        }
        [Test]
        public void Decode_stelem_i8()
        {
            CheckDecode("stelem.i8", 0x9F);
        }
        [Test]
        public void Decode_stelem_r4()
        {
            CheckDecode("stelem.r4", 0xA0);
        }
        [Test]
        public void Decode_stelem_r8()
        {
            CheckDecode("stelem.r8", 0xA1);
        }
        [Test]
        public void Decode_stelem_ref()
        {
            CheckDecode("stelem.ref", 0xA2);
        }
        [Test]
        public void Decode_ldelem()
        {
            SetResolveToken("class A.B");
            CheckDecode("ldelem class A.B", 0xA3, 1, 2, 3, 4);
        }
        [Test]
        public void Decode_stelem()
        {
            SetResolveToken("class Foo.Bar");
            CheckDecode("stelem class Foo.Bar", 0xA4, 0, 1, 2, 3);
        }
        [Test]
        public void Decode_unbox_any()
        {
            SetResolveToken("class C.D");
            CheckDecode("unbox.any class C.D", 0xA5, 1, 2, 3, 4);
        }
        [Test]
        public void Decode_conv_ovf_i1()
        {
            CheckDecode("conv.ovf.i1", 0xB3);
        }
        [Test]
        public void Decode_conv_ovf_u1()
        {
            CheckDecode("conv.ovf.u1", 0xB4);
        }
        [Test]
        public void Decode_conv_ovf_i2()
        {
            CheckDecode("conv.ovf.i2", 0xB5);
        }
        [Test]
        public void Decode_conv_ovf_u2()
        {
            CheckDecode("conv.ovf.u2", 0xB6);
        }
        [Test]
        public void Decode_conv_ovf_i4()
        {
            CheckDecode("conv.ovf.i4", 0xB7);
        }
        [Test]
        public void Decode_conv_ovf_u4()
        {
            CheckDecode("conv.ovf.u4", 0xB8);
        }
        [Test]
        public void Decode_conv_ovf_i8()
        {
            CheckDecode("conv.ovf.i8", 0xB9);
        }
        [Test]
        public void Decode_conv_ovf_u8()
        {
            CheckDecode("conv.ovf.u8", 0xBA);
        }
        [Test]
        public void Decode_refanyval()
        {
            SetResolveToken("class Foo.Bar");
            CheckDecode("refanyval class Foo.Bar", 0xC2, 1, 2, 3, 4);
        }
        [Test]
        public void Deocde_ckfinite()
        {
            CheckDecode("ckfinite", 0xC3);
        }
        [Test]
        public void Decode_mkrefany()
        {
            SetResolveToken("class A.B");
            CheckDecode("mkrefany class A.B", 0xC6, 3, 2, 1, 0);
        }
        [Test]
        public void Decode_ldtoken()
        {
            SetResolveToken("<anytoken>");
            CheckDecode("ldtoken <anytoken>", 0xD0, 0, 1, 2, 3);
        }
        [Test]
        public void Decode_conv_u2()
        {
            CheckDecode("conv.u2", 0xD1);
        }
        [Test]
        public void Decode_conv_u1()
        {
            CheckDecode("conv.u1", 0xD2);
        }
        [Test]
        public void Decode_conv_i()
        {
            CheckDecode("conv.i", 0xD3);
        }
        [Test]
        public void Decode_conv_ovf_i()
        {
            CheckDecode("conv.ovf.i", 0xD4);
        }
        [Test]
        public void Decode_conv_ovf_u()
        {
            CheckDecode("conv.ovf.u", 0xD5);
        }
        [Test]
        public void Decode_add_ovf()
        {
            CheckDecode("add.ovf", 0xD6);
        }
        [Test]
        public void Decode_add_ovf_un()
        {
            CheckDecode("add.ovf.un", 0xD7);
        }
        [Test]
        public void Decode_mul_ovf()
        {
            CheckDecode("mul.ovf", 0xD8);
        }
        [Test]
        public void Decode_mul_ovf_un()
        {
            CheckDecode("mul.ovf.un", 0xD9);
        }
        [Test]
        public void Decode_sub_ovf()
        {
            CheckDecode("sub.ovf", 0xDA);
        }
        [Test]
        public void Decode_sub_ovf_un()
        {
            CheckDecode("sub.ovf.un", 0xDB);
        }
        [Test]
        public void Decode_endfinally()
        {
            CheckDecode("endfinally", 0xDC);
        }
        [Test]
        public void Decode_leave()
        {
            CheckDecode("leave 1", 0xDD, 1, 0, 0, 0);
        }
        [Test]
        public void Decode_leave_s()
        {
            CheckDecode("leave.s 7", 0xDE, 7);
        }
        [Test]
        public void Decode_stind_i()
        {
            CheckDecode("stind.i", 0xDF);
        }
        [Test]
        public void Decode_conv_u()
        {
            CheckDecode("conv.u", 0xE0);
        }
    }
}
