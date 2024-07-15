using System.IO;
using System.Xml.Serialization;
using RXml.Abstraction.Model;

namespace RXml.Abstraction;

public static class XmlResourceSerializer
{
   public static string Serialize(Root obj)
   {
      var serializer = new XmlSerializer(typeof(Root));
      using var stream = new StringWriter();
      serializer.Serialize(stream, obj);
      return stream.ToString();
   }

   public static Root Deserialize(string xml)
   {
      if (string.IsNullOrEmpty(xml)) return new Root();

      var serializer = new XmlSerializer(typeof(Root));
      using var reader = new StringReader(xml);
      return (Root)serializer.Deserialize(reader);
   }
}