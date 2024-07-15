using System.Collections.Generic;
using System.Xml.Serialization;

namespace RXml.Abstraction.Model;

[XmlRoot(ElementName = "Resources")]
public class Root
{

    [XmlElement(ElementName = "Res")]
    public List<Resource> Resources { get; set; }
}