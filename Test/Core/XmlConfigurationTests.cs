namespace Pencil.Test.Core
{
    using System.IO;
    using NUnit.Framework;
    using Pencil.Core;
    using Pencil.NMeter;

    [TestFixture]
    public class XmlConfigurationTests
    {
        [Test]
        public void Can_extract_Item()
        {
            var config = new XmlConfiguration();
            config.Load(new StringReader("<Item Value=\"Bar\"/>"));

            config.Read<ConfigurationItem>().Value.ShouldEqual("Bar");
        }

        public class Thing
        {
            public int Value { get; set; }
        }

        [Test]
        public void Can_extract_item_without_XmlRoot()
        {
            var config = new XmlConfiguration();
            config.Load(new StringReader("<Thing><Value>42</Value></Thing>"));

            config.Read<Thing>().Value.ShouldEqual(42);
        }
        [Test]
        public void ReadSection_should_read_child_nodes()
        {
            var config = new XmlConfiguration();
            config.Load(new StringReader("<Root><Item Value=\"Hello World!\"/><Thing><Value>42</Value></Thing></Root>"));

            config.ReadSection<Thing>().Value.ShouldEqual(42);
        }
    }
}
