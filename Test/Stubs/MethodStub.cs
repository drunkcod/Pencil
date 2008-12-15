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
        public ICollection<IMethodArgument> Arguments { get { return new IMethodArgument[0]; } }

        public override string ToString()
        {
            return name;
        }
	}
}