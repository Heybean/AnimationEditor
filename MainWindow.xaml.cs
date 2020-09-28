using AnimationManager.Graphics;
using DungeonSphere.Graphics;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnimationManager
{
    public partial class MainWindow : Window
    {
        private TreeView _atlasTreeView;
        private Button _buttonRemoveAtlas;

        public ViewModel ViewModel { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            _atlasTreeView = (TreeView)FindName("trv_Atlas");
            _buttonRemoveAtlas = (Button)FindName("btn_RemoveAtlas");
            StartNewFile();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddAtlas_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                AddExtension = true,
                Filter = "texture atlas files (*.atlas)|*.atlas",
                Title = "Add Texture Atlas"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                AddTextureAtlas(openFileDialog.FileName);
            }
        }

        private void RemoveAtlas_Click(object sender, RoutedEventArgs e)
        {
            // Remove all selected texture atlases
            foreach(var x in ViewModel.SelectedItems.ToList())
            {
                if (x is WpfTextureAtlas)
                {
                    var atlas = x as WpfTextureAtlas;
                    ViewModel.TextureAtlases.Remove(atlas);
                    ViewModel.RegisteredTextureAtlases.Remove(atlas.Name);
                }
            }
        }

        private void TreeView_ItemsSelected(object sender, RoutedEventArgs e)
        {
            // Toggle Remove Atlas button based on which items are selected
            bool onlyTextureAtlasesSelected = true;
            foreach(var item in ViewModel.SelectedItems)
            {
                if (!(item is WpfTextureAtlas))
                {
                    onlyTextureAtlasesSelected = false;
                    break;
                }
            }

            _buttonRemoveAtlas.IsEnabled = onlyTextureAtlasesSelected;
        }

        /// <summary>
        /// Adds a single texture atlas into the project
        /// </summary>
        private void AddTextureAtlas(string file)
        {
            var atlasName = System.IO.Path.GetFileNameWithoutExtension(file);

            if (ViewModel.RegisteredTextureAtlases.Contains(atlasName))
            {
                MessageBox.Show("Cannot add '" + atlasName + "' because it already exists.", "Add Texture Atlas", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Load the atlas
            var atlas = new WpfTextureAtlas(file);
            ViewModel.RegisteredTextureAtlases.Add(atlasName);
            ViewModel.TextureAtlases.Add(atlas);
        }

        /// <summary>
        /// Removes a single texture atlas from the project
        /// </summary>
        private void RemoveTextureAtlas(WpfTextureAtlas atlas)
        {
            ViewModel.RegisteredTextureAtlases.Remove(atlas.Name);
            ViewModel.TextureAtlases.Remove(atlas);
        }

        private void StartNewFile()
        {
            ViewModel = new ViewModel();
            _atlasTreeView.Items.Clear();
            _atlasTreeView.DataContext = ViewModel;
        }
    }
}
