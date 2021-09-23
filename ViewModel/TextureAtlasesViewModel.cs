using AnimationEditor.Graphics;
using AnimationEditor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace AnimationEditor.ViewModel
{
    public class TextureAtlasesViewModel : INotifyPropertyChanged
    {
        private TextureAtlasesModel _model;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<TextureAtlas> TextureAtlases { get; set; }

        public ICommand AddAtlasCommand { get; set; }
        public ICommand RemoveAtlasCommand { get; set; }

        public TextureAtlasesViewModel()
        {
            _model = new TextureAtlasesModel();
            TextureAtlases = new ObservableCollection<TextureAtlas>(_model.TextureAtlases.Values);
            AddAtlasCommand = new RelayCommand(_ => AddAtlasClicked(null));
            RemoveAtlasCommand = new RelayCommand(_ => RemoveAtlasClicked(null));
        }

        public void Changed(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void AddAtlasClicked(object sender)
        {

        }

        private void RemoveAtlasClicked(object sender)
        {

        }
    }
}
