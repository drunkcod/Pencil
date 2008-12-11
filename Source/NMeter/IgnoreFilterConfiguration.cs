namespace Pencil.NMeter
{
    using System.Xml.Serialization;
    using System.Collections.Generic;

    public struct IgnoreItem
    {
        [XmlAttribute("Value")]
        public string Value;

        public IgnoreItem(string value)
        {
            Value = value;
        }
    }

    [XmlRoot("Ignore")]
    public class IgnoreFilterConfiguration
    {
        List<IgnoreItem> names = new List<IgnoreItem>();
        List<IgnoreItem> patterns = new List<IgnoreItem>();
        
        [XmlElement("Name")]
        public List<IgnoreItem> Names { get { return names; } }
        [XmlElement("Pattern")]
        public List<IgnoreItem> Patterns { get { return patterns; } }
    }
}