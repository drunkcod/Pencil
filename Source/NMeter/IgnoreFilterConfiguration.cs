namespace Pencil.NMeter
{
    using System.Xml.Serialization;
    using System.Collections.Generic;

    [XmlRoot("Ignore")]
    public class IgnoreFilterConfiguration
    {
        List<ConfigurationItem> names = new List<ConfigurationItem>();
        List<ConfigurationItem> patterns = new List<ConfigurationItem>();

        [XmlElement("Name")]
        public List<ConfigurationItem> Names { get { return names; } }
        [XmlElement("Pattern")]
        public List<ConfigurationItem> Patterns { get { return patterns; } }
    }
}