using AnimationEditor.Graphics;
using AnimationEditor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace AnimationEditor.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private TextureAtlasesModel _textureAtlasesModel;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<TextureAtlas> TextureAtlases { get; set; }

        public MainViewModel(TextureAtlasesModel model)
        {
            _textureAtlasesModel = model;
            TextureAtlases = new ObservableCollection<TextureAtlas>(_textureAtlasesModel.TextureAtlases.Values);
        }

        public void Changed(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
