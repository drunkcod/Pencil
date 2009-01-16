namespace Pencil.Build
{
	using System;
	using System.IO;

	public class Logger
	{
		public static readonly Logger Null = new Logger(TextWriter.Null);
		
		TextWriter target;
		string indentation = string.Empty;

		public Logger(TextWriter target)
		{
			this.target = target;
		}

		public TextWriter Target { get { return target; } }

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