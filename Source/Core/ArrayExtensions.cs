namespace Pencil.Core
{
	using System;

	static class ArrayExtensions
	{
		public static bool TryFind<T>(this T[] items, Predicate<T> predicate, out T value)
		{
			for(int i = 0; i != items.Length; ++i)
			{
				value = items[i];
				if(predicate(value))
					return true;
			}
			value = default(T);
			return false;
		}
	}
}