using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Xml.Schema;
using Microsoft;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Documents;
using Microsoft.VisualStudio.Extensibility.Editor;
using Microsoft.VisualStudio.Extensibility.Helpers;
using Microsoft.VisualStudio.Threading;

namespace RXmlExtension.Validator;

[Experimental("VSEXTPREVIEW_OUTPUTWINDOW")]
internal class RXmlDiagnosticsServices : DisposableObject
{
   private readonly VisualStudioExtensibility _extensibility;
   private readonly Dictionary<Uri, CancellationTokenSource> _documentCancellationTokens;
   private readonly Task _initializeAsync;
   private OutputWindow? _outputWindow;
   private DiagnosticsReporter? _diagnosticsReporter;
   private XmlSchema? _schema;

   public RXmlDiagnosticsServices(VisualStudioExtensibility extensibility)
   {
      _extensibility = Requires.NotNull(extensibility);
      _documentCancellationTokens = [];
      _initializeAsync = Task.Run(InitializeAsync);
   }

   public async Task ProcessTextViewAsync(ITextViewSnapshot textViewSnapshot, CancellationToken cancellationToken)
   {
      CancellationTokenSource newCts = new CancellationTokenSource();
      lock (_documentCancellationTokens)
      {
         if (_documentCancellationTokens.TryGetValue(textViewSnapshot.Uri, out var cts)) _ = cts.CancelAsync();

         _documentCancellationTokens[textViewSnapshot.Uri] = newCts;
      }

      await ProcessDocumentAsync(textViewSnapshot, cancellationToken.CombineWith(newCts.Token).Token);
   }

   public async Task ClearEntriesForDocumentAsync(ITextViewSnapshot textView, CancellationToken cancellationToken)
   {
      lock (_documentCancellationTokens)
      {
         if (_documentCancellationTokens.TryGetValue(textView.Uri, out var cts))
         {
            _ = cts.CancelAsync();
            _documentCancellationTokens.Remove(textView.Uri);
         }
      }

      await _diagnosticsReporter!.ClearDiagnosticsAsync(textView.Document, cancellationToken);
   }

   protected override void Dispose(bool isDisposing)
   {
      base.Dispose(isDisposing);

      if (!isDisposing) return;

      _outputWindow?.Dispose();
      _diagnosticsReporter?.Dispose();
   }

   private async Task ProcessDocumentAsync(ITextViewSnapshot textView, CancellationToken cancellationToken)
   {
      Assumes.NotNull(_schema);

      // Wait for 1 second to see if any other changes are being sent.
      await Task.Delay(1000, cancellationToken);

      if (cancellationToken.IsCancellationRequested)
         return;

      try
      {
         var diagnostics = await RxmlUtilities.RunValidateOnFileAsync(_schema, textView);

         await _diagnosticsReporter!.ClearDiagnosticsAsync(textView.Uri, cancellationToken);
         await _diagnosticsReporter!.ReportDiagnosticsAsync(diagnostics, cancellationToken);
      }
      catch (Exception e)
      {
         if (_outputWindow is not null) await _outputWindow.Writer.WriteLineAsync(e.Message);
      }
   }

   private async Task InitializeAsync()
   {
      _outputWindow = await _extensibility.Views().Output.GetChannelAsync(nameof(ExtensibilityPoint) + Guid.NewGuid(), "%RXmlExtension.OpenRXmlEditorCommand.DisplayName%", default);
      Assumes.NotNull(_outputWindow);

      _diagnosticsReporter = _extensibility.Languages().GetDiagnosticsReporter(nameof(ExtensionEntrypoint));
      Assumes.NotNull(_diagnosticsReporter);

      var assembly = Assembly.GetExecutingAssembly();
      await using var stream = assembly.GetManifestResourceStream("RXmlExtension.Schema.rxml.xsd");
      if (stream != null)
         _schema = XmlSchema.Read(stream, null);
   }
}