using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using RXml.Abstraction.Model;

namespace RXml.Abstraction;

public class LocalisationService<T> : ILocalisationService<T>
{
   private readonly IEnumerable<Resource> _localizations = LoadLocalization($"{typeof(T).Name}.rxml");

   private static IEnumerable<Resource> LoadLocalization(string fileName)
   {
      var assembly = Assembly.GetEntryAssembly();
      var resourceNames = assembly.GetManifestResourceNames();
      var resourcePath = resourceNames.SingleOrDefault(r => r.Contains(fileName)) ?? string.Empty;
      var stream = assembly.GetManifestResourceStream(resourcePath);
      if (stream == null) return Array.Empty<Resource>();

      using var reader = new StreamReader(stream);
      var xml = reader.ReadToEnd();
      return XmlResourceSerializer.Deserialize(xml).Resources;
   }

   public string GetString(string key) => _localizations.FirstOrDefault(r => r.Key == key)?.Value.FirstOrDefault(v => v.Lang == CultureInfo.CurrentCulture.TwoLetterISOLanguageName)?.Text ?? string.Empty;

   public string this[string key] => GetString(key);
}