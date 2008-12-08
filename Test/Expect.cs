namespace Pencil.Test
{
	using System;
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
	}
}