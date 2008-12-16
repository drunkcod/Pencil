namespace Pencil.NMeter
{
    using System.Xml.Serialization;

    [XmlRoot("Project")]
    public class ProjectConfiguration
    {
        public string BinPath { get; set; }
        public IgnoreFilterConfiguration IgnoreAssemblies{ get; set; }
    }
}