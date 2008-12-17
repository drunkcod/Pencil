namespace Pencil.Core
{
    using System.IO;
    using System.Xml;
    using System.Xml.XPath;
    using System.Xml.Serialization;
	using System.Reflection;

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
            return (T)Deserialize(string.Empty, typeof(T));
        }

        public T ReadSection<T>()
        {
            return (T)Deserialize("*/", typeof(T));
        }

        object Deserialize(string prefix, System.Type type)
        {
            var serializer = new XmlSerializer(type);
            return serializer.Deserialize(ReadSubtree(prefix, type));
        }

        XmlReader ReadSubtree(string prefix, MemberInfo type)
        {
            return navigator.SelectSingleNode(prefix + GetRootElement(type)).ReadSubtree();
        }

        string GetRootElement(MemberInfo type)
        {
            var rootAttribute = type.GetCustomAttributes(typeof(XmlRootAttribute), false);
            if(rootAttribute.Length == 0)
                return type.Name;
            return ((XmlRootAttribute)rootAttribute[0]).ElementName;
        }
    }
}