namespace Pencil.NMeter
{
    using System.Xml.Serialization;

    [XmlRoot("Item")]
    public struct ConfigurationItem
    {
        [XmlAttribute("Value")]
        public string Value;

        public ConfigurationItem(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}