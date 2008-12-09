namespace Pencil.Test.Core
{
    using NUnit.Framework;
    using ReflectionAssembly = System.Reflection.Assembly;
    using Pencil.Core;

    [TestFixture]
    public class AssemblyTests
    {
        [Test]
        public void Should_be_able_to_wrap_Reflection_Assembly()
        {
            Assert.AreEqual(
                ReflectionAssembly.GetExecutingAssembly().GetName().Name,
                Assembly.GetExecutingAssembly().Name);
        }
    }
}
