namespace Pencil.NMeter
{
    using System.Xml.Serialization;

    public struct IgnoreItem
    {
        [XmlAttribute("Value")]
        public string Value;

        public IgnoreItem(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}