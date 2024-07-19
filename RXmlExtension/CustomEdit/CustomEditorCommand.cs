using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;

namespace RXmlExtension.CustomEdit;

/// <summary>
/// A command for showing a tool window.
/// </summary>
[VisualStudioContribution]
public class CustomEditorCommand : Command
{
   /// <inheritdoc />
   public override CommandConfiguration CommandConfiguration => new(displayName: "%RXmlExtension.OpenRXmlEditorCommand.DisplayName%")
   {
      Placements = [CommandPlacement.KnownPlacements.ToolsMenu],
      Icon = new CommandIconConfiguration(ImageMoniker.KnownValues.Edit, IconSettings.IconAndText),
      EnabledWhen = ActivationConstraint.ClientContext(ClientContextKey.Shell.ActiveSelectionFileName, ".rxml"),
   };

   /// <inheritdoc />
   public override async Task ExecuteCommandAsync(IClientContext context, CancellationToken cancellationToken)
   {
      var textView = await context.GetActiveTextViewAsync(cancellationToken);
      await Extensibility.Shell().ShowDialogAsync(new CustomEditorContent(textView!), cancellationToken);
   }
}