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
    }
}
