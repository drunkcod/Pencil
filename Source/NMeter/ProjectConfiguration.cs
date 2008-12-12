namespace Pencil.NMeter
{
    using System.IO;
    using System.Xml.Serialization;
    using System.Collections.Generic;

    [XmlRoot("Project")]
    public class ProjectConfiguration
    {
        public string BinPath { get; set; }
        public IgnoreFilterConfiguration IgnoreAssemblies{ get; set; }

        public static ProjectConfiguration FromFile(string path)
        {
            var serializer = new XmlSerializer(typeof(ProjectConfiguration));
            return serializer.Deserialize(File.OpenRead(path)) as ProjectConfiguration;
        }
    }
}