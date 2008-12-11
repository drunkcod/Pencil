namespace Pencil.Test.NMeter
{
    using NUnit.Framework;
    using System.Reflection;
    using Pencil.NMeter;
    using System.Xml.Serialization;
    using System;

    [TestFixture]
    public class IgnoreFilterConfigurationTests
    {
        [Test]
        public void Should_return_true_if_no_ignored_items()
        {
            var ignore = new IgnoreFilterConfiguration();
            Assert.IsTrue(IgnoreFilter.From(ignore).Include(new AssemblyName("Foo.Bar.Core")));
        }
        [Test]
        public void Should_return_false_if_item_matches_by_name()
        {
            var ignore = new IgnoreFilterConfiguration();
            var name = new AssemblyName("Foo.Bar.Core");
            ignore.Names.Add(new IgnoreItem(name.Name));
            Assert.IsFalse(IgnoreFilter.From(ignore).Include(name));
        }
        [Test]
        public void Should_return_false_if_item_matchs_by_pattern()
        {
            var ignore = new IgnoreFilterConfiguration();
            var name = new AssemblyName("Foo.Bar.Core");
            ignore.Patterns.Add(new IgnoreItem(".*ar"));
            Assert.IsFalse(IgnoreFilter.From(ignore).Include(name));
        }
    }
}