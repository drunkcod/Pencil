namespace Pencil.Test.Core
{
	using System;
	using NUnit.Framework;
	using NUnit.Framework.SyntaxHelpers;
	using Pencil.Core;

	[TestFixture]
	public class MethodTests
	{
		public void MyMethod()
		{
			DoStuff();
			var tomorrow = DateTime.Now.AddDays(1);
			Console.WriteLine(tomorrow);
		}

		void DoStuff(){}

		[Test]
		public void Calls_should_contain_called_methods()
		{
			var method = Method.Wrap(GetType().GetMethod("MyMethod"));
			Assert.That(method.Calls.Map(x => x.Name).ToList(),
				Is.EquivalentTo(new []{ "DoStuff", "get_Now", "AddDays", "WriteLine" }));
		}
	}
}