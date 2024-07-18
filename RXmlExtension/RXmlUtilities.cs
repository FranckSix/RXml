using System.Collections.Concurrent;
using System.Xml;
using System.Xml.Schema;
using Microsoft;
using Microsoft.VisualStudio.Extensibility.Editor;
using Microsoft.VisualStudio.Extensibility.Languages;
using Microsoft.VisualStudio.RpcContracts.DiagnosticManagement;
using Microsoft.VisualStudio.Threading;
using RXmlExtension.Linter;
using Range = Microsoft.VisualStudio.RpcContracts.Utilities.Range;

namespace RXmlExtension;

internal static class RxmlUtilities
{
   public static async Task<IEnumerable<DocumentDiagnostic>> RunValidateOnFileAsync(XmlSchema schema, ITextViewSnapshot textView)
   {
      var lineQueue = new AsyncQueue<RXmlDiagnosticInfo>();

      var settings = new XmlReaderSettings
      {
         ValidationType = ValidationType.Schema
      };

      settings.Schemas.Add(schema);
      settings.ValidationEventHandler += (s, e) =>
      {
         var info = GetDiagnosticFromOutput(e);
         if (info != null) lineQueue.Enqueue(info);
      };

      var reader = XmlReader.Create(textView.Uri.ToString(), settings);
      new XmlDocument().Load(reader);

      var rxmlDiagnostics = await ProcessValidationQueueAsync(lineQueue);
      return CreateDocumentDiagnosticsForClosedDocument(textView.Uri, rxmlDiagnostics);
   }

   private static async Task<IEnumerable<RXmlDiagnosticInfo>> ProcessValidationQueueAsync(AsyncQueue<RXmlDiagnosticInfo> lineQueue)
   {
      Requires.NotNull(lineQueue, nameof(lineQueue));

      var diagnostics = new ConcurrentQueue<RXmlDiagnosticInfo>();

      while (lineQueue.Count > 0)
      {
         try
         {
            var msg = await lineQueue.DequeueAsync();
            diagnostics.Enqueue(msg);
         }
         catch (OperationCanceledException) // Something went wrong so break and return the current set
         {
            break;
         }
      }

      return diagnostics;
   }

   private static RXmlDiagnosticInfo? GetDiagnosticFromOutput(ValidationEventArgs outputLine)
   {
      Requires.NotNull(outputLine, nameof(outputLine));

      var line = outputLine.Exception.LineNumber - 1;
      var column = outputLine.Exception.LinePosition;

      return new RXmlDiagnosticInfo(
         range: new Range(startLine: line, startColumn: column),
         message: outputLine.Message,
         errorCode: outputLine.Severity.ToString());
   }

   private static IEnumerable<DocumentDiagnostic> CreateDocumentDiagnosticsForClosedDocument(Uri fileUri, IEnumerable<RXmlDiagnosticInfo> diagnostics)
   {
      return diagnostics.Select(diagnostic => new DocumentDiagnostic(fileUri, diagnostic.Range, diagnostic.Message)
      {
         ErrorCode = diagnostic.ErrorCode,
         Severity = DiagnosticSeverity.Warning,
         ProviderName = "RXml Validator",
      });
   }

}
