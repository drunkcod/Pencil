namespace Pencil.Test.Core
{
    using Pencil.Core;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public partial class DisassemblerTests : ITokenResolver
    {
        void CheckDecode(string expected, params byte[] ilbytes)
        {
            var disassembler = new Disassembler(this);
            Assert.AreEqual(expected, disassembler.Decode(ilbytes)[0].ToString());
        }

        void SetResolveToken(string token)
        {
            resolveTokenResult = token;
        }

        object ITokenResolver.Resolve(int token)
        {
            return resolveTokenResult;
        }

        string resolveTokenResult;

    }
}
