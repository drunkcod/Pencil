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
                AssemblyLoader.GetExecutingAssembly().Name.Name);
        }
        [Test]
        public void Should_return_same_modules_as_reflection()
        {
            Assert.That(
                AssemblyLoader.GetExecutingAssembly().Modules.Map(x => x.Name).ToList(), 
                Is.EquivalentTo(ReflectionAssembly.GetExecutingAssembly().GetModules().Map(x => x.Name).ToList()));
        }
    }
}
