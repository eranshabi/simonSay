using System.IO;
using System.Xml.Serialization;

public class XmlConfigLoad : IConfigLoadStrategy
{
    const string XML_CONFIG_PATH = "Assets/Configs/config.xml";

    public GameConfig LoadConfig(ReadFileFunc readFileFunc)
    {
        string xmlAsString = readFileFunc(XML_CONFIG_PATH);
        XmlSerializer serializer = new XmlSerializer(typeof(GameConfig));
        using TextReader reader = new StringReader(xmlAsString);

        return (GameConfig)serializer.Deserialize(reader);
    }
}
