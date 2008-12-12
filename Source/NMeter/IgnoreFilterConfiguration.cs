namespace Pencil.NMeter
{
	using System.IO;
    using System.Xml.Serialization;
    using System.Collections.Generic;

    [XmlRoot("Ignore")]
    public class IgnoreFilterConfiguration
    {
        List<IgnoreItem> names = new List<IgnoreItem>();
        List<IgnoreItem> patterns = new List<IgnoreItem>();

		public static IgnoreFilterConfiguration FromFile(string path)
		{
			var serializer = new XmlSerializer(typeof(IgnoreFilterConfiguration));
			return serializer.Deserialize(File.OpenRead(path)) as IgnoreFilterConfiguration;
		}

        [XmlElement("Name")]
        public List<IgnoreItem> Names { get { return names; } }
        [XmlElement("Pattern")]
        public List<IgnoreItem> Patterns { get { return patterns; } }
    }
}