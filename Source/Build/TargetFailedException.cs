namespace Pencil.Build
{
	using System;

	public class TargetFailedException : Exception
	{
		public TargetFailedException(Exception inner): base(string.Empty, inner){}
	}

}