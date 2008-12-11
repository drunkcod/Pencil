namespace Pencil.Test
{
	using System;
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

		public static void ShouldEqual(this string actual, string expected)
		{
			Assert.AreEqual(expected, actual);
		}
	}
}