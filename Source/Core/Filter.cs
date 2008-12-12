namespace Pencil.Core
{
	using System;

	public interface IFilter<T>
	{
		bool Include(T item);
	}

	public class Filter
	{
		public static IFilter<T> From<T>(Predicate<T> predicate)
		{
			return new Filter<T>(predicate);
		}
	}

	class Filter<T> : IFilter<T>
	{
		Predicate<T> predicate;

		public Filter(Predicate<T> predicate)
		{
			this.predicate = predicate;
		}

		public bool Include(T item){ return predicate(item); }
	}
}