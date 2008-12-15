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
        public void DoStuff(int value, string s){}

		[Test]
		public void Calls_should_contain_called_methods()
		{
			var method = Method.Wrap(GetType().GetMethod("MyMethod"));
			Assert.That(method.Calls.Map(x => x.Name).ToList(),
				Is.EquivalentTo(new []{ "DoStuff", "get_Now", "AddDays", "WriteLine" }));
		}
        [Test]
        public void Arguments_should_contain_all_method_arguments()
        {
            var method = Method.Wrap(GetType().GetMethod("DoStuff", new[]{ typeof(int), typeof(string)}));
            Assert.That(method.Arguments.Map(x => x.Type.Name).ToList(),
                Is.EquivalentTo(new[]{ "Int32", "String" }));
        }
	}
}