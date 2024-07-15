using System;
using System.IO;
using Microsoft.CodeAnalysis;
using System.Text;
using System.Linq;
using RXml.Abstraction;
using System.Xml;

namespace RXml.Generator;

[Generator]
public class RXmlCodeGenerator : IIncrementalGenerator
{
   public void Initialize(IncrementalGeneratorInitializationContext context)
   {
      // Find all .resx files in the project
      var resxFiles = context.AdditionalTextsProvider
         .Where(file => file.Path.EndsWith(".rxml"))
         .Select((text, cancellationToken) => (name: Path.GetFileNameWithoutExtension(text.Path).Split('.').First(), content: text.GetText(cancellationToken)?.ToString()));

      context.RegisterSourceOutput(resxFiles, (spc, nameAndContent) =>
      {
         try
         {
            var resources = XmlResourceSerializer.Deserialize(nameAndContent.content);
            var className = $"XRes{nameAndContent.name}";

            var source = new StringBuilder($"public static class {className} \n{{\n");
            foreach (var resource in resources.Resources)
            {
               source.AppendLine($"\tpublic const string {resource.Key} = \"{resource.Key}\";");
            }
            source.AppendLine("}");

            spc.AddSource(className, source.ToString());
         }
         catch (Exception)
         {
            spc.AddSource(Guid.NewGuid().ToString(), Commentaire("Erreur de deserialisation XML" + nameAndContent.content));
            //throw;
         }
      });
   }

   private string Commentaire(string erreur) => $"/*{erreur}*/";
}