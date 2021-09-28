using AnimationEditor.Graphics;
using AnimationEditor.Model;
using Heybean.Graphics;
using Microsoft.Win32;
using PropertyTools;
using PropertyTools.Wpf;
using System;
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
    public class TextureAtlasesViewModel : ViewModelBase
    {
        private Node _selectedItem;
        private TextureAtlasesModel _model;
        private bool _atlasSelected;

        public IEnumerable AtlasRoot
        {
            get
            {
                yield return Root;
            }
        }

        public Node Root
        {
            get; private set;
        }

        public bool AtlasSelected
        {
        get => _atlasSelected;
            set
            {
                _atlasSelected = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddAtlasCommand { get; }
        public ICommand RemoveAtlasCommand { get; }
        public ICommand SelectedItemsChangedCommand { get; }

        public delegate void SelectionChangedEventHandler(object sender, EventArgs e);
        public event SelectionChangedEventHandler SelectionChanged;

        public TextureAtlasesViewModel()
        {
            _model = new TextureAtlasesModel();

            Root = new Node { Name = "Untitled.anim" };

            AddAtlasCommand = new RelayCommand(_ => AddAtlasExecute(null));
            RemoveAtlasCommand = new RelayCommand(x => RemoveAtlasExecute(x));
            SelectedItemsChangedCommand = new RelayCommand(x => SelectedItemsChangedExecute(x));
        }

        public void Reset()
        {
            _model = new TextureAtlasesModel();
            Root.SubNodes.Clear();
            Root.Name = "Untitled.anim";
        }

        public TextureAtlasModel AddTextureAtlas(string filename)
        {
            var newAtlas = _model.AddTextureAtlas(filename);
            Root.SubNodes.Add(newAtlas);
            Root.ShowChildren = true;

            return newAtlas;
        }

        private void AddAtlasExecute(object parameters)
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
                    }
                }

                Root.SortSubNodes();

                if (addedAtlas)
                    MarkFileModified();

                if (invalidFiles.Count > 0)
                {
                    MessageBox.Show("Cannot add '" + string.Join("', '", invalidFiles) + "' because it already exists.", "Add Texture Atlas", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void RemoveAtlasExecute(object parameters)
        {
            bool removedAtlas = false;
            var selectedItems = (IList<object>)parameters;

            if (selectedItems == null)
                return;

            // Remove all selected texture atlases
            foreach (var x in selectedItems.ToList())
            {
                if (x is TextureAtlasModel atlas)
                {
                    _model.RemoveTextureAtlas(atlas.Name);
                    Root.SubNodes.Remove(atlas);
                    removedAtlas = true;
                }
            }

            Root.SortSubNodes();

            if (removedAtlas)
                MarkFileModified();
        }

        private void SelectedItemsChangedExecute(object parameters)
        {
            bool hasAtlas = false;
            var list = (IList<object>)parameters;
            foreach(var obj in list)
            {
                if (obj is TextureAtlasModel)
                {
                    hasAtlas = true;
                    break;
                }
            }

            AtlasSelected = hasAtlas;

            SelectionChanged?.Invoke(this, new EventArgs() { Parameters = parameters });
        }

        public static void LoadData(TextureAtlasesViewModel viewModel, string filename, AnimationsFileData data)
        {
            var rootFolder = Path.GetDirectoryName(filename);
            viewModel.Root.Name = Path.GetFileNameWithoutExtension(filename) + ".anim";

            var atlasDict = new Dictionary<string, SpriteModel>();
            foreach (var atlasData in data.Root.Atlases)
            {
                var atlas = viewModel.AddTextureAtlas(rootFolder + "\\" + atlasData.File);
                foreach (SpriteModel sprite in atlas.SubNodes)
                {
                    atlasDict.Add(sprite.Name, sprite);
                }

                RecreateStructure(atlasData, viewModel.Root, atlasDict);
            }
        }

        private static void RecreateStructure(AnimationsFileData.Folder parentFolder, Node parentNode, Dictionary<string, SpriteModel> atlasDict)
        {
            Node node = new Node(parentNode);
            foreach (var folder in parentFolder.Folders)
            {
                Node folderNode = new Node(node);
                RecreateStructure(folder, folderNode, atlasDict);
                node.SubNodes.Add(folderNode);
            }

            foreach (var spriteData in parentFolder.Sprites)
            {
                SpriteModel sprite;
                atlasDict.TryGetValue(spriteData.Name, out sprite);
                if (sprite == null)
                    continue;

                // Update the sprite with data from file
                sprite.FPS = spriteData.FPS;
                sprite.OriginX = spriteData.OriginX;
                sprite.OriginY = spriteData.OriginY;
                sprite.HAlign = (SpriteHorizontalAlignment)Enum.Parse(typeof(SpriteHorizontalAlignment), spriteData.HorizontalAlignment, true);
                sprite.VAlign = (SpriteVerticalAlignment)Enum.Parse(typeof(SpriteVerticalAlignment), spriteData.VerticalAlignment, true);

                node.SubNodes.Add(sprite);
            }
        }
    }
}
