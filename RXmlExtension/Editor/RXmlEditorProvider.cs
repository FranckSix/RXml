using System.Diagnostics.CodeAnalysis;
using Microsoft;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Editor;
using RXmlExtension.Validator;

namespace RXmlExtension.Editor;

[VisualStudioContribution]
[Experimental("VSEXTPREVIEW_OUTPUTWINDOW")]
internal class RXmlEditorProvider(RXmlDiagnosticsServices diagnosticsProvider)
   : ExtensionPart, ITextViewOpenClosedListener, ITextViewChangedListener
{
   private readonly RXmlDiagnosticsServices _diagnosticsProvider = Requires.NotNull(diagnosticsProvider, nameof(diagnosticsProvider));

   public TextViewExtensionConfiguration TextViewExtensionConfiguration => new()
   {
      AppliesTo = [DocumentFilter.FromGlobPattern("**/*.rxml", true)]
   };

   public async Task TextViewChangedAsync(TextViewChangedArgs args, CancellationToken cancellationToken) =>
      await _diagnosticsProvider.ProcessTextViewAsync(args.AfterTextView, cancellationToken);

   public async Task TextViewClosedAsync(ITextViewSnapshot textView, CancellationToken cancellationToken) =>
      await _diagnosticsProvider.ClearEntriesForDocumentAsync(textView, cancellationToken);

   public async Task TextViewOpenedAsync(ITextViewSnapshot textView, CancellationToken cancellationToken) =>
      await _diagnosticsProvider.ProcessTextViewAsync(textView, cancellationToken);
}
