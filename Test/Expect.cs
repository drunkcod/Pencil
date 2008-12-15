namespace Pencil.Test
{
	using System;
	using System.Collections;
	using NUnit.Framework;

	static class Expect
	{
		public static void Exception<T>(Action action) where T : Exception
		{
			try
			{
				action();
			}
			catch(T){}
		}

		public static void ShouldBe(this bool actual, bool expected)
		{
			Assert.AreEqual(actual, expected);
		}

		public static void ShouldBeSameAs(this object actual, object expected)
		{
			Assert.AreSame(expected, actual);
		}

		public static void ShouldEqual(this string actual, string expected)
		{
			Assert.AreEqual(expected, actual);
		}

        public static void ShouldEqual<T>(this T actual, T expected)
        {
            Assert.AreEqual(expected, actual);
        }

		public static void ShouldBeEmpty(this IEnumerable sequence)
		{
			foreach(var item in sequence)
				Assert.Fail("Sequence not empty.");
		}
	}
}