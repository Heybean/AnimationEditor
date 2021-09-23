using AnimationEditor.Graphics;
using AnimationEditor.Model;
using Microsoft.Win32;
using PropertyTools;
using PropertyTools.Wpf;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace AnimationEditor.ViewModel
{
    public class TextureAtlasesViewModel : Observable
    {
        private TextureAtlasesModel _model;
        private ObservableCollection<TextureAtlas> _textureAtlases;
        private List<object> _selectedItems;

        public IEnumerable HierarchySource
        {
            get
            {
                yield return this.Root;
            }
        }

        public TreeListBoxNode Root { get; private set; }

        public ICollectionView TextureAtlasesCollectionView { get; }
        public IList<object> SelectedItems
        {
            get => _selectedItems;
            /*set
            {
                _selectedItems = (List<object>)value;
                OnPropertyChanged("SelectedItems");
            }*/
        }
        public ICommand AddAtlasCommand { get; }
        public ICommand RemoveAtlasCommand { get; }

        public TextureAtlasesViewModel()
        {
            _model = new TextureAtlasesModel();

            Root = new TreeListBoxNode { Name = "Untitled.anim" };

            _textureAtlases = new ObservableCollection<TextureAtlas>(_model.TextureAtlases.Values);
            TextureAtlasesCollectionView = CollectionViewSource.GetDefaultView(_textureAtlases);
            TextureAtlasesCollectionView.SortDescriptions.Add(new SortDescription("AtlasName", ListSortDirection.Ascending));

            _selectedItems = new List<object>();

            AddAtlasCommand = new RelayCommand(_ => AddAtlasExecute(null));
            RemoveAtlasCommand = new RelayCommand(x => RemoveAtlasExecute(x));
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
                        _textureAtlases.Add(newAtlas);
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
            bool removedAtlas = false;

            // Remove all selected texture atlases
            foreach (var x in SelectedItems.ToList())
            {
                if (x is TextureAtlas atlas)
                {
                    _model.RemoveTextureAtlas(atlas.AtlasName);
                    _textureAtlases.Remove(atlas);
                    removedAtlas = true;
                }
            }

            //if (removedAtlas)
            //    MainWindowVM.UnsavedChanges = true;
        }
    }
}
