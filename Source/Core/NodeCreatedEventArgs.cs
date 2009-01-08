namespace Pencil.Core
{
	using System;

	public class NodeCreatedEventArgs<T> : EventArgs
	{
		readonly T item;

		public NodeCreatedEventArgs(T item)
		{
			this.item = item;
		}

		public T Item { get { return item; } }
	}
}