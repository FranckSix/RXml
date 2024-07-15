using System.Xml.Serialization;

namespace RXml.Abstraction.Model;

[XmlRoot(ElementName = "Value")]
public class Value
{

    [XmlAttribute(AttributeName = "lang")]
    public string Lang { get; set; }

    [XmlText]
    public string Text { get; set; }
}