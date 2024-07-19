using System.Runtime.Serialization;

namespace RXmlExtension.CustomEdit.Model;

[DataContract]
internal class ResourceLangueViewModel
{
   public string Lang { get; set; }
   public string? Value { get; set; }
}