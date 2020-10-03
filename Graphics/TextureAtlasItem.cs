using DungeonSphere.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Data;

namespace AnimationManager.Graphics
{
    [Serializable]
    public class TextureAtlasItem : INotifyPropertyChanged
    {
        public virtual ObservableCollection<object> Children { get; private set; } = new ObservableCollection<object>();

        public virtual string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

        private string _name;

        public event PropertyChangedEventHandler PropertyChanged;

        public TextureAtlasItem()
        {
        }


        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sort all items in the collection alphabetically
        /// </summary>
        protected void SortAlphabetically()
        {

        }
    }
}
