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
        private Node _selectedItem;
        private TextureAtlasesModel _model;

        public IEnumerable HierarchySource
        {
            get
            {
                yield return Root;
            }
        }

        public Node Root { get; private set; }

        public Node SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetValue(ref _selectedItem, value);
            }
        }

        public ICommand AddAtlasCommand { get; }
        public ICommand RemoveAtlasCommand { get; }

        public TextureAtlasesViewModel()
        {
            
            _model = new TextureAtlasesModel();

            Root = new Node { Name = "Untitled.anim" };
            SelectedItem = Root;

            /*_textureAtlases = new ObservableCollection<TextureAtlas>(_model.TextureAtlases.Values);
            TextureAtlasesCollectionView = CollectionViewSource.GetDefaultView(_textureAtlases);
            TextureAtlasesCollectionView.SortDescriptions.Add(new SortDescription("AtlasName", ListSortDirection.Ascending));*/

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
                        Root.SubNodes.Add(newAtlas);
                        Root.ShowChildren = true;
                        //_textureAtlases.Add(newAtlas);
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
            var selectedItems = (IList<object>)sender;

            if (selectedItems == null)
                return;

            // Remove all selected texture atlases
            foreach (var x in selectedItems.ToList())
            {
                if (x is TextureAtlas atlas)
                {
                    _model.RemoveTextureAtlas(atlas.Name);
                    Root.SubNodes.Remove(atlas);
                    removedAtlas = true;
                }
            }

            //if (removedAtlas)
            //    MainWindowVM.UnsavedChanges = true;
        }
    }
}
