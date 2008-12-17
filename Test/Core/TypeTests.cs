namespace Pencil.Test.Core
{
	using System;
	using System.IO;
	using System.Text;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Reflection;
    using Pencil.Core;
    using NUnit.Framework.SyntaxHelpers;
	using Type = Pencil.Core.Type;

    class SampleType
    {
        public void PublicMethod() { }
        protected void ProtectedMethod() { }
        private void PrivateMethod() { }

        public static void StaticPublicMethod() { }
        protected static void StaticProtectedMethod() { }
        private static void StaticPrivateMethod() { }
    }

	class DependentType
	{
		public SampleType DoStuff(){ return new SampleType(); }
		protected void WriteStuff(TextWriter writer){}
		void ReadStuff(TextReader reader){}
		void HiddenConstruction(){ new StringBuilder(); }
		void HiddenStaticCall(){ var foo = DateTime.Now; }
	}

	[System.Runtime.CompilerServices.CompilerGenerated]
	class GeneratedType
	{
		public static System.Type GetNestedType(){ return typeof(InnerType); }
		public class InnerType{}
	}

    [TestFixture]
    public class TypeTests
    {
		IType DependentType = Type.Wrap(typeof(DependentType));

        [Test]
        public void Should_have_same_methods_as_reflection()
        {
            var viaReflection = new List<string>();

            typeof(SampleType).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                .ForEach(x => viaReflection.Add(x.Name));

            Assert.That(Type.Wrap(typeof(SampleType)).Methods.Map(x => x.Name).ToList(), Is.EquivalentTo(viaReflection));
        }
		[Test]
		public void Should_have_readable_ToString()
		{
			Type.Wrap(typeof(SampleType)).ToString().ShouldEqual("SampleType");
		}
		[Test]
		public void IsGenerated_should_be_false_for_undecorated_Type()
		{
			Type.Wrap(typeof(SampleType)).IsGenerated.ShouldBe(false);
		}
		[Test]
		public void IsGenerated_should_be_true_for_Type_with_CompilerGeneratedAttribute()
		{
			Type.Wrap(typeof(GeneratedType)).IsGenerated.ShouldBe(true);
		}
		[Test]
		public void IsGenerated_should_be_true_for_class_nested_inside_generated_type()
		{
			Type.Wrap(GeneratedType.GetNestedType()).IsGenerated.ShouldBe(true);
		}
		[Test]
		public void DependsOn_should_be_empty_for_SampleType()
		{
			Type.Wrap(typeof(SampleType)).DependsOn.ShouldBeEmpty();
		}
		[Test]
		public void DependsOn_should_contain_method_argument_types()
		{
			DependentType.DependsOn.Map(x => x.Name).ShouldContain("TextWriter", "TextReader");
		}
		[Test]
		public void DependsOn_should_contain_return_value_types()
		{
			DependentType.DependsOn.Map(x => x.Name).ShouldContain("SampleType");
		}
		[Test]
		public void DependsOn_should_contain_constructed_types()
		{
			DependentType.DependsOn.Map(x => x.Name).ShouldContain("StringBuilder");
		}
		[Test]
		public void DependsOn_should_contain_called_types()
		{
			DependentType.DependsOn.Map(x => x.Name).ShouldContain("DateTime");
		}
		[Test]
		public void DependsOn_should_not_have_duplicates()
		{
			DependentType.DependsOn.Count(x => x.Equals(typeof(SampleType))).ShouldEqual(1);
		}
		[Test]
		public void Should_support_Equals_with_System_Type()
		{
			Type.Wrap(typeof(object)).Equals(typeof(object)).ShouldBe(true);
		}
    }
}
