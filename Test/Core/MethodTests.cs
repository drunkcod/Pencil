namespace Pencil.Test.Core
{
	using System;
	using NUnit.Framework;
	using NUnit.Framework.SyntaxHelpers;
	using Pencil.Core;

	[TestFixture]
	public class MethodTests
	{
		[Test]
		public void Calls_should_contain_called_methods()
		{
			var method = GetMyMethod();
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
		[Test]
		public void ReturnType_should_return_correct_type()
		{
			GetMyMethod().ReturnType.Equals(typeof(int)).ShouldBe(true);
		}
		[Test]
		public void Should_have_proper_DeclaringMethod()
		{
			var method = GetMyMethod();

			method.DeclaringType.ShouldEqual(Pencil.Core.Type.Wrap(GetType()));
		}
		[Test]
		public void IsGenerated_should_be_false_for_undecorated_method()
		{
			GetMyMethod().IsGenerated.ShouldBe(false);
		}
		[Test]
		public void IsGenerated_should_be_true_if_method_has_CompilerGeneratedAttribute()
		{
			var method = Method.Wrap(GetType().GetMethod("CompilerGeneratedMethod"));
			method.IsGenerated.ShouldBe(true);
		}
		[Test]
		public void IsSpecialName_should_be_false_for_normal_method()
		{
			GetMyMethod().IsSpecialName.ShouldBe(false);
		}
		[Test]
		public void IsSpecialName_should_be_true_for_property()
		{
			Method.Wrap(GetType().GetProperty("MyProperty").GetGetMethod()).IsSpecialName.ShouldBe(true);
		}
		[Test]
		public void IsConstructor_should_be_false_for_plain_method()
		{
			GetMyMethod().IsConstructor.ShouldBe(false);
		}
		[Test]
		public void IsConstructor_should_be_true_for_ctor()
		{
			Method.Wrap(GetType().GetConstructor(System.Type.EmptyTypes)).IsConstructor.ShouldBe(true);
		}
		[Test]
		public void ToString_should_include_return_type_and_name()
		{
            GetMyMethod().ToString().ShouldEqual("System.Int32 Pencil.Test.Core.MethodTests.MyMethod()");
		}

		public void MyMethod2(string foo, object bar){}
		[Test]
		public void ToString_should_include_arguments()
		{
			Method.Wrap(GetType().GetMethod("MyMethod2"))
			.ToString().ShouldEqual("System.Void Pencil.Test.Core.MethodTests.MyMethod2(System.String, System.Object)");
		}

		Method GetMyMethod()
		{
			return Method.Wrap(GetType().GetMethod("MyMethod"));
		}

		public int MyMethod()
		{
			DoStuff();
			var tomorrow = DateTime.Now.AddDays(1);
			Console.WriteLine(tomorrow);
			return 42;
		}

		[System.Runtime.CompilerServices.CompilerGenerated]
		public void CompilerGeneratedMethod(){}

		void DoStuff(){}

        public void DoStuff(int value, string s){}

		public bool MyProperty { get { return true; } }
	}
}