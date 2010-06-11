using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
using Pencil.Core;
using Type = Pencil.Core.PencilType;

namespace Pencil.Test.Core
{
    class SampleType
    {
        public void PublicMethod() { ProtectedMethod();}
        protected void ProtectedMethod() { PrivateMethod();}
        private void PrivateMethod() { }

        public static void StaticPublicMethod() { StaticProtectedMethod(); }
        protected static void StaticProtectedMethod() { StaticPrivateMethod(); }
        private static void StaticPrivateMethod() { }
    }

	interface ISampleInterface {}
	class DependentType : ISampleInterface
	{
		public SampleType DoStuff()
		{
			ReadStuff(null);
			HiddenConstruction();
			HiddenStaticCall();
			return new SampleType();
		}
		protected void WriteStuff(TextWriter writer){}
		void ReadStuff(TextReader reader){ reader.GetType();}
		void HiddenConstruction(){ new StringBuilder(); }
		void HiddenStaticCall(){ var foo = DateTime.Now; foo.AddDays(1); }
		void CallSelf(){ if(selfField != null) CallSelf(); }

        DependentType[] selfField = new DependentType[0];
	}

	class GenericSample
	{
		public T DoStuff<T>(){ return default(T); }
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
        static ITypeLoader typeLoader = new DefaultTypeLoader();
        static IType Wrap(System.Type type) { return typeLoader.FromNative(type); }

		IType DependentType = Wrap(typeof(DependentType));

        [Test]
        public void Should_have_same_methods_as_reflection()
        {
            var viaReflection = new List<string>();

            typeof(SampleType).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                .ForEach(x => viaReflection.Add(x.Name));

            Assert.That(Wrap(typeof(SampleType)).Methods.Map(x => x.Name).ToList(), Is.EquivalentTo(viaReflection));
        }
		[Test]
		public void Should_have_readable_ToString()
		{
			Wrap(typeof(SampleType)).ToString().ShouldEqual("SampleType");
		}
		[Test]
		public void IsGenerated_should_be_false_for_undecorated_Type()
		{
			Wrap(typeof(SampleType)).IsGenerated.ShouldBe(false);
		}
		[Test]
		public void IsGenerated_should_be_true_for_Type_with_CompilerGeneratedAttribute()
		{
			Wrap(typeof(GeneratedType)).IsGenerated.ShouldBe(true);
		}
		[Test]
		public void IsGenerated_should_be_true_for_class_nested_inside_generated_type()
		{
			Wrap(GeneratedType.GetNestedType()).IsGenerated.ShouldBe(true);
		}
		[Test]
		public void DependsOn_should_be_empty_for_SampleType()
		{
			Wrap(typeof(SampleType)).DependsOn.ShouldBeEmpty();
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
		public void DependsOn_should_contain_implemented_interface()
		{
			DependentType.DependsOn.Map(x => x.Name).ShouldContain("ISampleInterface");
		}
		[Test]
		public void DependsOn_wont_have_duplicates()
		{
			DependentType.DependsOn.Count(x => x.Equals(typeof(SampleType))).ShouldEqual(1);
		}
		[Test]
		public void DependsOn_wont_contain_self()
		{
			DependentType.DependsOn.Count(x => x.Name.StartsWith("DependentType")).ShouldEqual(0);
		}
		[Test]
		public void DependsOn_wont_contain_generic_parameters()
		{
			Wrap(typeof(GenericSample)).DependsOn.Any(x => x.Name.StartsWith("T")).ShouldBe(false);
		}

        class WithInitializer
        {
            IList<int> list = new List<int>();
			void Foo(){ if(list == null) Foo();}
        }

        [Test]
        public void DependsOn_should_handle_automaticliy_initilized_members()
        {
            Assert.That(Wrap(typeof(WithInitializer)).DependsOn.Map(x => x.Name).ToList(), Is.EquivalentTo(new[] { "IList`1", "List`1" }));
        }
		[Test]
		public void Should_support_Equals_with_System_Type()
		{
			Wrap(typeof(object)).Equals(typeof(object)).ShouldBe(true);
		}

		enum MyEnum { None }

		[Test]
		public void DependsOn_should_be_empty_for_Enum_type()
		{
			Wrap(typeof(MyEnum)).DependsOn.ShouldBeEmpty();
		}

		interface IBase {}
		class Base : IBase {}
		class Derived : Base {}
		[Test]
		public void DependsOn_should_not_return_interface_implemented_by_base()
		{
            Assert.That(Wrap(typeof(Derived)).DependsOn.Map(x => x.Name).ToList(),
				Is.EquivalentTo(new[] { "Base" }));
		}

		class Thing<T>
		{
			public IEnumerable<T> Foo(){ return new T[0]; }
		}
		[Test]
		public void DependsOn_should_handle_nested_generics()
		{
            Assert.That(Wrap(typeof(Thing<>)).DependsOn.Map(x => x.FullName).ToList(),
				Is.EquivalentTo(new[] { "System.Collections.Generic.IEnumerable`1" }));
		}

		class EmptyAttribute : Attribute {}

		[Test]
		public void EmptyAttribute_should_only_depend_on_Attribute()
		{
			Wrap(typeof(EmptyAttribute)).DependsOn.All(x => x.Equals(typeof(Attribute))).ShouldBe(true);
		}
		[Test]
		public void ElementType_should_be_same_as_type_for_normal_type()
		{
			DependentType.ElementType.ShouldBeSameAs(DependentType);
		}
		[Test]
		public void ElementType_should_be_item_type_for_array()
		{
			Wrap(typeof(object[])).ElementType.Equals(typeof(object)).ShouldBe(true);
		}

		[Test]
		public void IsA_should_be_true_if_assignable_from_other_type()
		{
			Wrap(typeof(EmptyAttribute)).IsA<Attribute>().ShouldBe(true);
		}
		[Test]
		public void IsPublic_should_be_true_for_public_type()
		{
			Wrap(GetType()).IsPublic.ShouldBe(true);
		}

		public class PublicType{}
		[Test]
		public void IsPublic_should_be_true_for_public_type_nested_in_public_type()
		{
			Wrap(typeof(PublicType)).IsPublic.ShouldBe(true);
		}

		class PrivateType{}
		[Test]
		public void IsPublic_should_be_false_for_non_public_type()
		{
			Wrap(typeof(PrivateType)).IsPublic.ShouldBe(false);
		}

		class Outer
		{
			class Nested {}
		}
		[Test]
		public void NestedTypes_should_include_inner_classes()
		{
			Assert.That(Wrap(typeof(Outer)).NestedTypes.Map(x => x.Name).ToList(),
				Is.EquivalentTo(new[]{ "Nested"}));
		}
    }
}
