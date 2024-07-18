using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Extensibility;
using RXmlExtension.Validator;

namespace RXmlExtension;

/// <summary>
/// Extension entrypoint for the VisualStudio.Extensibility extension.
/// </summary>
[VisualStudioContribution]
internal class ExtensionEntrypoint : Extension
{
   /// <inheritdoc/>
   public override ExtensionConfiguration ExtensionConfiguration => new()
   {
      LoadedWhen = ActivationConstraint.SolutionState(SolutionState.FullyLoaded),
      Metadata = new ExtensionMetadata(
         id: "RXmlExtension.fde68009-8d22-44a3-8541-dc38bea8dd5b",
         version: ExtensionAssemblyVersion,
         publisherName: "FranckSix",
         displayName: "RXmlExtension",
         description: "RXml editor"),
   };

   [Experimental("VSEXTPREVIEW_OUTPUTWINDOW")]
   protected override void InitializeServices(IServiceCollection serviceCollection)
   {
      base.InitializeServices(serviceCollection);
      serviceCollection.AddScoped<RXmlDiagnosticsServices>();
   }
}