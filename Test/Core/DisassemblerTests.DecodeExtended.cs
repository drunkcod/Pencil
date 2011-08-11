namespace Pencil.Test.Core
{
    using NUnit.Framework;

    public partial class DisassemblerTests
    {
        [Test]
        public void Decode_arglist()
        {
            CheckDecode("arglist", 0xFE, 0x00);
        }
        [Test]
        public void Decode_ceq()
        {
            CheckDecode("ceq", 0xFE, 0x01);
        }
        [Test]
        public void Decode_cgt()
        {
            CheckDecode("cgt", 0xFE, 0x02);
        }
        [Test]
        public void Decode_cgt_un()
        {
            CheckDecode("cgt.un", 0xFE, 0x03);
        }
        [Test]
        public void Decode_clt()
        {
            CheckDecode("clt", 0xFE, 0x04);
        }
        [Test]
        public void Decode_clt_un()
        {
            CheckDecode("clt.un", 0xFE, 0x05);
        }
        [Test]
        public void Decode_ldftn()
        {
            SetResolveToken("SomeMethod");
            CheckDecode("ldftn SomeMethod", 0xFE, 0x06, 0, 0, 0, 0);
        }
        [Test]
        public void Decode_ldvirtfn()
        {
            SetResolveToken("VirtualFunction");
            CheckDecode("ldvirtftn VirtualFunction", 0xFE, 0x07, 0, 0, 0, 0);
        }
        [Test]
        public void Decode_ldarg()
        {
            CheckDecode("ldarg 5", 0xFE, 0x09, 5, 0);
        }
        [Test]
        public void Decode_ldarg_a()
        {
            CheckDecode("ldarg.a 7", 0xFE, 0x0A, 7, 0, 0, 0);
        }
        [Test]
        public void Decode_starg()
        {
            CheckDecode("starg 8", 0xFE, 0x0B, 8, 0, 0, 0);
        }
        [Test]
        public void Decode_ldloc()
        {
            CheckDecode("ldloc 4", 0xFE, 0x0C, 4, 0, 0, 0);
        }
        [Test]
        public void Decode_ldloca()
        {
            CheckDecode("ldloca 5", 0xFE, 0x0D, 5, 0, 0, 0);
        }
        [Test]
        public void Decode_stloc()
        {
            CheckDecode("stloc 6", 0xFE, 0x0E, 6, 0, 0, 0);
        }
        [Test]
        public void Decode_localloc()
        {
            CheckDecode("localloc", 0xFE, 0x0F);
        }
        [Test]
        public void Decode_endfilter()
        {
            CheckDecode("endfilter", 0xFE, 0x11);
        }
        [Test]
        public void Decode_unaligned()
        {
            CheckDecode("unaligned.", 0xFE, 0x12);
        }
        [Test]
        public void Decode_volatile()
        {
            CheckDecode("volatile.", 0xFE, 0x13);
        }
        [Test]
        public void Decode_tail()
        {
            CheckDecode("tail.", 0xFE, 0x14);
        }
        [Test]
        public void Decode_initobj()
        {
            SetResolveToken("SomeType");
            CheckDecode("initobj SomeType", 0xFE, 0x15, 0, 0, 0, 0);
        }
        [Test]
        public void Decode_constrained()
        {
            SetResolveToken("SomeType");
            CheckDecode("constrained. SomeType", 0xFE, 0x16, 0, 0, 0, 0);
        }
        [Test]
        public void Decode_cpblk()
        {
            CheckDecode("cpblk", 0xFE, 0x17);
        }
        [Test]
        public void Decode_initblk()
        {
            CheckDecode("initblk", 0xFE, 0x18);
        }
        [Test]
        public void Decode_rethrow()
        {
            CheckDecode("rethrow", 0xFE, 0x1A);
        }
        [Test]
        public void Decode_sizeof()
        {
            SetResolveToken("SomeType");
            CheckDecode("sizeof SomeType", 0xFE, 0x1C, 0, 0, 0, 0);
        }
        [Test]
        public void Decode_refanytype()
        {
            CheckDecode("refanytype", 0xFE, 0x1D);
        }
    }
}
