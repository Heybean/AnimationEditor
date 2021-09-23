using AnimationEditor.Graphics;
using AnimationEditor.Model;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace AnimationEditor.ViewModel
{
    public class TextureAtlasesViewModel : ViewModelBase
    {
        private TextureAtlasesModel _model;

        public ObservableCollection<TextureAtlas> TextureAtlases { get; }

        public ICollectionView TextureAtlasesCollectionView { get; }

        public ICommand AddAtlasCommand { get; }
        public ICommand RemoveAtlasCommand { get; }

        public TextureAtlasesViewModel()
        {
            _model = new TextureAtlasesModel();

            TextureAtlases = new ObservableCollection<TextureAtlas>(_model.TextureAtlases.Values);
            TextureAtlasesCollectionView = CollectionViewSource.GetDefaultView(TextureAtlases);
            TextureAtlasesCollectionView.SortDescriptions.Add(new SortDescription("AtlasName", ListSortDirection.Ascending));

            AddAtlasCommand = new RelayCommand(_ => AddAtlasExecute(null));
            RemoveAtlasCommand = new RelayCommand(_ => RemoveAtlasExecute(null));
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
                    var newAtlas = _model.AddTextureAtlas(filename);
                    if (newAtlas == null)
                    {
                        invalidFiles.Add(Path.GetFileName(filename));
                    }
                    else
                    {
                        addedAtlas = true;
                        TextureAtlases.Add(newAtlas);
                    }
                }

                //if (addedAtlas)
                //    MainWindowVM.UnsavedChanges = true;
                //TextureAtlases = new ObservableCollection<TextureAtlas>(_model.TextureAtlases.Values);

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
