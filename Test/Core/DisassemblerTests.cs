namespace Pencil.Test.Core
{
    using Pencil.Core;
    using NUnit.Framework;
	using NUnit.Framework.SyntaxHelpers;
    using System;
	using System.Collections.Generic;
    using Pencil.Test.Stubs;

    [TestFixture]
    public partial class DisassemblerTests : ITokenResolver
    {
		[Test]
		public void Decode_should_parse_whole_stream()
		{
			var disassembler = new Disassembler(this);
			var ilbytes = new byte[]{ 0, 1, 20 };
			var expected = new[]{ "nop", "break", "ldnull" };
			Assert.That(disassembler.Decode(ilbytes).Map(x => x.ToString()).ToList(), Is.EquivalentTo(expected));
		}

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

		IMethod ITokenResolver.ResolveMethod(int token)
        {
            return new MethodStub(resolveTokenResult);
        }

        string resolveTokenResult;
    }
}
