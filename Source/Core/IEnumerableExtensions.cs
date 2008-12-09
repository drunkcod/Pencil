namespace Pencil.Core
{
	using System;
	using System.Collections.Generic;

	public static class IEnumerableExtensions
	{
		public static IEnumerable<TTo> Map<TFrom,TTo>(this IEnumerable<TFrom> sequence, Converter<TFrom,TTo> transform)
		{
			foreach(var item in sequence)
				yield return transform(item);
		}

        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach(var item in sequence)
                action(item);
        }

		public static List<T> ToList<T>(this IEnumerable<T> sequence)
		{
			return new List<T>(sequence);
		}
	}
}