using Range = Microsoft.VisualStudio.RpcContracts.Utilities.Range;

namespace RXmlExtension.Linter;

internal class RXmlDiagnosticInfo(Range range, string message, string errorCode)
{
   public string ErrorCode { get; } = errorCode;

   public string Message { get; } = message;

   public Range Range { get; } = range;
}