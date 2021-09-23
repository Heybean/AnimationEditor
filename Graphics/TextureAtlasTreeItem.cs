using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Data;
using System.Xml.Serialization;

namespace AnimationEditor.Graphics
{
    //[Serializable]
    //[XmlRoot("Item")]
    //[XmlInclude(typeof(TextureAtlasItem))]
    //[XmlInclude(typeof(WpfSprite))]
    public abstract class TextureAtlasTreeItem : INotifyPropertyChanged
    {
        private string _name;

        public ObservableCollection<object> Children { get; } = new ObservableCollection<object>();

        //[XmlAttribute("name")]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

        public abstract FileType FileType { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
