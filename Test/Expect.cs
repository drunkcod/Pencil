namespace Pencil.Test
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using NUnit.Framework;
	using Pencil.Core;

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
			Assert.AreEqual(expected, actual);
		}

		public static void ShouldBeSameAs(this object actual, object expected)
		{
			Assert.AreSame(expected, actual);
		}

		public static void ShouldEqual(this string actual, string expected)
		{
			Assert.AreEqual(expected, actual);
		}

        public static void ShouldEqual(this int actual, int expected)
        {
            Assert.AreEqual(expected, actual);
        }

		public static void ShouldEqual(this object actual, object expected)
		{
			Assert.AreEqual(expected, actual);
		}

		public static void ShouldBeEmpty(this IEnumerable sequence)
		{
			foreach(var item in sequence)
				Assert.Fail("Sequence not empty {0}.", item);
		}

		public static void ShouldContain<T>(this IEnumerable<T> sequence, params T[] items)
		{
			(sequence.ToList() as ICollection).ShouldContain(items);
		}

		public static void ShouldContain<T>(this ICollection collection, params T[] items)
		{
			Assert.That(items, Is.SubsetOf(collection));
		}
	}
}