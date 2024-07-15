using Microsoft.Extensions.DependencyInjection;

namespace RXml.Abstraction.Service;

public static class LocalizationServiceExtensions
{
   public static IServiceCollection AddLocalizationService(this IServiceCollection services)
   {
      services.AddSingleton(typeof(ILocalisationService<>), typeof(LocalisationService<>));
      return services;
   }
}