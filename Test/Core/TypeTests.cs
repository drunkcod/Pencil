namespace Pencil.Test.Core
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Reflection;
    using Pencil.Core;
    using NUnit.Framework.SyntaxHelpers;

    class SampleType
    {
        public void PublicMethod() { }
        protected void ProtectedMethod() { }
        private void PrivateMethod() { }

        public static void StaticPublicMethod() { }
        protected static void StaticProtectedMethod() { }
        private static void StaticPrivateMethod() { }
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
    }
}
