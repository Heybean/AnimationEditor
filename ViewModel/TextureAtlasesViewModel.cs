using AnimationEditor.Graphics;
using AnimationEditor.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
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
            AddAtlasCommand = new RelayCommand(_ => AddAtlasExecute(null));
            RemoveAtlasCommand = new RelayCommand(_ => RemoveAtlasExecute(null));
        }

        public void Changed(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void AddAtlasExecute(object sender)
        {
            var openFileDialog = new OpenFileDialog
            {
                AddExtension = true,
                Filter = "texture atlas files (*.atlas)|*.atlas",
                Title = "Add Texture Atlas",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var invalidFiles = new List<string>();
                bool addedAtlas = false;
                foreach (var filename in openFileDialog.FileNames)
                {
                    //AddTextureAtlas(filename, invalidFiles);
                    if (!_model.AddTextureAtlas(filename))
                    {
                        invalidFiles.Add(Path.GetFileName(filename));
                    }
                    else
                    {
                        addedAtlas = true;
                    }
                }

                //if (addedAtlas)
                //    MainWindowVM.UnsavedChanges = true;

                if (invalidFiles.Count > 0)
                {
                    MessageBox.Show("Cannot add '" + string.Join("', '", invalidFiles) + "' because it already exists.", "Add Texture Atlas", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void RemoveAtlasExecute(object sender)
        {

        }
    }
}
