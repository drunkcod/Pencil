namespace Pencil.Test.Core
{
    using System;
    using NUnit.Framework;
    using Pencil.Core;

    [TestFixture]
    public class ByteConverterTests
    {
        [Test]
        public void Should_throw_OutOfRangeException_if_insufficent_bytes_to_ReadInt32()
        {
            var converter = new ByteConverter(new byte[] { 1, 2, 3 }, 0);
			Expect.Exception<InvalidOperationException>(() => converter.ReadInt32());
        }
    }
}