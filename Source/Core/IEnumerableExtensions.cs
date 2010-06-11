namespace Pencil.Core
{
	using System;
	using System.Collections.Generic;

	public static class IEnumerableExtensions
	{
		public static bool All<T>(this IEnumerable<T> sequence, Predicate<T> predicate)
		{
			foreach(var item in sequence)
				if(!predicate(item))
					return false;
			return true;
		}

        public static bool Any<T>(this IEnumerable<T> sequence, Predicate<T> predicate) {
            foreach (var item in sequence)
                if (predicate(item))
                    return true;
            return false;
        }

		public static IEnumerable<TTo> Map<TFrom,TTo>(this IEnumerable<TFrom> sequence, Converter<TFrom,TTo> transform)
		{
			foreach(var item in sequence)
				yield return transform(item);
		}

        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action) {
            foreach (var item in sequence)
                action(item);
        }

        public static void ForEach<T>(this IEnumerable<T> sequence, Predicate<T> filter, Action<T> action)
        {
            foreach(var item in sequence)
				if(filter(item))
					action(item);
        }

		public static IEnumerable<T> Filter<T>(this IEnumerable<T> sequence, Predicate<T> filter)
		{
			foreach(var item in sequence)
				if(filter(item))
					yield return item;
		}

		public static List<T> ToList<T>(this IEnumerable<T> sequence)
		{
			return new List<T>(sequence);
		}

		public static int Count<T>(this IEnumerable<T> sequence)
		{
			return Count(sequence, x => true);
		}

		public static int Count<T>(this IEnumerable<T> sequence, Predicate<T> predicate)
		{
			int count = 0;
			foreach(var item in sequence)
				if(predicate(item))
					++count;
			return count;
		}
	}
}