namespace Pencil.Test.Core
{
	using System;
	using NUnit.Framework;
	using Pencil.Core;
using System.Reflection;

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
            var method = GetMethod(GetType().GetMethod("DoStuff", new[]{ typeof(int), typeof(string)}));
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

			method.DeclaringType.ShouldEqual(TypeLoader.FromNative(GetType()));
		}
		[Test]
		public void IsGenerated_should_be_false_for_undecorated_method()
		{
			GetMyMethod().IsGenerated.ShouldBe(false);
		}
		[Test]
		public void IsGenerated_should_be_true_if_method_has_CompilerGeneratedAttribute()
		{
			var method = GetMethod(GetType().GetMethod("CompilerGeneratedMethod"));
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
			GetMethod(GetType().GetProperty("MyProperty").GetGetMethod()).IsSpecialName.ShouldBe(true);
		}
		[Test]
		public void IsConstructor_should_be_false_for_plain_method()
		{
			GetMyMethod().IsConstructor.ShouldBe(false);
		}
		[Test]
		public void IsConstructor_should_be_true_for_ctor()
		{
			GetMethod(GetType().GetConstructor(System.Type.EmptyTypes)).IsConstructor.ShouldBe(true);
		}
        [Test]
        public void Constructor_have_same_declaring_type_and_return_type() {
            var ctor = GetMethod(GetType().GetConstructor(System.Type.EmptyTypes));

            Assert.That(ctor.DeclaringType, Is.SameAs(ctor.ReturnType));
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
			GetMethod(GetType().GetMethod("MyMethod2"))
			.ToString().ShouldEqual("System.Void Pencil.Test.Core.MethodTests.MyMethod2(System.String, System.Object)");
		}

        IMethod GetMethod(MethodInfo method) {
            return TypeLoader.FromNative(method);
        }

        IMethod GetMethod(ConstructorInfo ctor) {
            return TypeLoader.FromNative(ctor);
        }

		IMethod GetMyMethod()
		{
			return GetMethod(GetType().GetMethod("MyMethod"));
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