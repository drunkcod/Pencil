namespace Pencil.Test.Stubs
{
	using System;
	using System.Collections.Generic;
	using Pencil.Core;

	public class MethodStub : IMethod
	{
		string name;

		public Func<IEnumerable<IInstruction>> GetBodyHandler = () => new IInstruction[0];

		public MethodStub(string name)
		{
			this.name = name;
		}

		public string Name { get {return name; } }
		public IEnumerable<IInstruction> Body { get { return GetBodyHandler(); } }
	}
}