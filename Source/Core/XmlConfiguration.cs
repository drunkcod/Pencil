namespace Pencil.Core
{
    using System.IO;
    using System.Xml;
    using System.Xml.XPath;
    using System.Xml.Serialization;

    public class XmlConfiguration
    {
        XPathNavigator navigator;

        public static XmlConfiguration FromFile(string path)
        {
            var configuration = new XmlConfiguration();
            configuration.Load(File.OpenText(path));
            return configuration;
        }

        public void Load(TextReader reader)
        {
            navigator = new XPathDocument(reader).CreateNavigator();
        }

        public T Read<T>()
        {
            return Deserialize<T>(ReadSubtree(string.Empty, typeof(T)));
        }

        public T ReadSection<T>()
        {
            return Deserialize<T>(ReadSubtree("*/", typeof(T)));
        }

        static T Deserialize<T>(XmlReader reader)
        {
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(reader);
        }

        XmlReader ReadSubtree(string prefix, System.Type type)
        {
            return navigator.SelectSingleNode(prefix + GetRootElement(type)).ReadSubtree();
        }

        string GetRootElement(System.Type type)
        {
            var rootAttribute = type.GetCustomAttributes(typeof(XmlRootAttribute), false);
            if(rootAttribute.Length == 0)
                return type.Name;
            return ((XmlRootAttribute)rootAttribute[0]).ElementName;
        }
    }

}
