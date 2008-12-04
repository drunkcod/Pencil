namespace Pencil.Build
{
	using System;
	using System.IO;

	public class Logger
	{
		TextWriter target;
		string indentation = string.Empty;

		public Logger(TextWriter target)
		{
			this.target = target;
		}

		public void Write(string format, params object[] args)
		{
			target.Write(indentation);
			target.WriteLine(format, args);
		}

		public IDisposable Indent()
		{
			string old = indentation;
			indentation = old + "\t";
			return new Finally(() => indentation = old);
		}

		sealed class Finally: IDisposable
		{
			Action action;

			public Finally(Action action)
			{
				this.action = action;
			}
			public void Dispose(){ action();}
		}
	}
}