namespace Pencil.Core
{
	public class NullFilter<T> : IFilter<T>
	{
		public bool Include(T item){ return true; }
	}
}