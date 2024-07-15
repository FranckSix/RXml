using System.Collections.Generic;
using System.Xml.Serialization;

namespace RXml.Abstraction.Model;

[XmlRoot(ElementName = "Res")]
public class Resource
{

    [XmlElement(ElementName = "Value")]
    public List<Value> Value { get; set; }

    [XmlAttribute(AttributeName = "Key")]
    public string Key { get; set; }
}