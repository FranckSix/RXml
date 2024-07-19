using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace RXmlExtension.CustomEdit.Model;

[DataContract]
internal class ResourceViewModel : INotifyPropertyChanged
{
   private string? _key;
   private ObservableCollection<ResourceLangueViewModel>? _resources;

   [DataMember]
   public string? Key
   {
      get => _key;
      set
      {
         _key = value;
         OnPropertyChanged(nameof(Key));
      }
   }

   [DataMember]
   public ObservableCollection<ResourceLangueViewModel>? Resources
   {
      get => _resources;
      set
      {
         _resources = value;
         OnPropertyChanged(nameof(Resources));
      }
   }

   [DataMember] public int Count => Resources?.Count ?? 0;

   public event PropertyChangedEventHandler? PropertyChanged;
   protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}