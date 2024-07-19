using Microsoft.VisualStudio.Extensibility.Editor;
using Microsoft.VisualStudio.Extensibility.UI;

namespace RXmlExtension.CustomEdit;

/// <summary>
/// A remote user control to use as tool window UI content.
/// </summary>
internal class CustomEditorContent : RemoteUserControl
{
   /// <summary>
   /// Initializes a new instance of the <see cref="CustomEditorContent" /> class.
   /// </summary>
   public CustomEditorContent(ITextViewSnapshot textView) : base(new CustomEditorData(textView))
   {
   }
}