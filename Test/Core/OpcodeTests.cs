namespace Pencil.Test.Core
{
	using NUnit.Framework;
	using Pencil.Test;
	using Pencil.Core;
	using System;

	[TestFixture]
	public class OpcodeTests
	{
		[Test]
		public void FromName_should_throw_NotSupportedException_for_unknown_opcode()
		{
			Expect.Exception<NotSupportedException>(() => Opcode.FromName("foobar"));
		}
		[Test]
		public void IsCall_should_be_true_for_call()
		{
			Assert.IsTrue(Opcode.FromName("call").IsCall);
		}
		[Test]
		public void IsCall_should_be_true_for_calli()
		{
			Assert.IsTrue(Opcode.FromName("calli").IsCall);
		}
		[Test]
		public void IsCall_should_be_true_for_newobj()
		{
			Assert.IsTrue(Opcode.FromName("newobj").IsCall);
		}
		[Test]
		public void IsCall_should_be_true_for_callvirt()
		{
			Assert.IsTrue(Opcode.FromName("callvirt").IsCall);
		}
	}
}