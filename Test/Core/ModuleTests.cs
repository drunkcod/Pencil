using System.Collections.Generic;
using NUnit.Framework;
using Pencil.Core;
using Xlnt.Stuff;

namespace Pencil.Test.Core
{
    [TestFixture]
    public class ModuleTests
    {
        [Test]
        public void Should_return_same_types_as_reflection()
        {
            var reflectedTypes = new List<string>();
            var types = new List<string>();

            System.Reflection.Assembly.GetExecutingAssembly().GetModules().ForEach(
                x => x.GetTypes().ForEach(y => reflectedTypes.Add(y.Name)));

            AssemblyLoader.GetExecutingAssembly().Modules.ForEach(m => types.AddRange(m.Types.Map(t => t.Name)));

            Assert.That(types, Is.EquivalentTo(reflectedTypes));
        }
    }
}
