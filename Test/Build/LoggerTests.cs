namespace Pencil.Test.Build
{
	using System;
	using System.IO;
    using NUnit.Framework;
	using Pencil.Build;

	[TestFixture]
	public class LoggerTests
	{
		[Test]
		public void Should_support_using_for_Indent()
		{
			var output = new StringWriter();
			var logger = new Logger(output);
			logger.Write("1");
			using(logger.Indent())
				logger.Write("2");
			logger.Write("3");
			Assert.AreEqual("1\r\n\t2\r\n3\r\n", output.ToString());
		}
		[Test]
		public void Write_should_support_formatting()
		{
			var output = new StringWriter();
			var logger = new Logger(output);
			logger.Write("{0}+{1}", 1, 2);
			Assert.AreEqual("1+2\r\n", output.ToString());
		}
	}
}