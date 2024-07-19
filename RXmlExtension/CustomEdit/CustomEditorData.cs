using Microsoft.VisualStudio.Extensibility.UI;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.Extensibility.Editor;
using RXmlExtension.CustomEdit.Model;

namespace RXmlExtension.CustomEdit;

/// <summary>
/// ViewModel for the CustomEditorContent remote user control.
/// </summary>
[DataContract]
internal class CustomEditorData : INotifyPropertyChanged
{
   private ResourceViewModel _currentResource;

   public CustomEditorData(ITextViewSnapshot textView)
   {
      BtnTestAsyncCommand = new AsyncCommand(BtnTestAsync);
      BtnSaveAsyncCommand = new AsyncCommand(BtnSaveAsync);
      BtnCancelAsyncCommand = new AsyncCommand(BtnCancelAsync);
      Langues = CultureInfo
         .GetCultures(CultureTypes.AllCultures)
         .Select(c => c.TwoLetterISOLanguageName)
         .Distinct()
         .OrderByDescending(c => c).ToList();
   }

   [DataMember]
   public ObservableCollection<ResourceViewModel> ResourcesKey { get; } = [];

   [DataMember]
   public ResourceViewModel? CurrentResource
   {
      get => _currentResource;
      set
      {
         _currentResource = value;
         OnPropertyChanged(nameof(CurrentResource));
      }
   }

   [DataMember]
   public IEnumerable<string> Langues { get; }


   [DataMember]
   public AsyncCommand BtnSaveAsyncCommand { get; }
   private async Task BtnSaveAsync(object? sender, CancellationToken cancellationToken)
   {
      await Task.CompletedTask;
   }

   [DataMember]
   public AsyncCommand BtnCancelAsyncCommand { get; }
   private async Task BtnCancelAsync(object? arg1, CancellationToken arg2)
   {
      await Task.CompletedTask;
   }

   [DataMember] public AsyncCommand BtnTestAsyncCommand { get; }

   private async Task BtnTestAsync(object? sender, CancellationToken cancellationToken)
   {
      ResourcesKey.Add(CreateMock("toto"));
      ResourcesKey.Add(CreateMock("Titi"));
      await Task.CompletedTask;
   }

   private ResourceViewModel CreateMock(string value)
   {
      return new ResourceViewModel
      {
         Key = value,
         Resources =
         [
            new ResourceLangueViewModel { Lang = "fr", Value = value },
            new ResourceLangueViewModel { Lang = "en", Value = value }
         ]
      };
   }

   public event PropertyChangedEventHandler? PropertyChanged;
   protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}