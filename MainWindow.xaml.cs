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

        public ViewModel ViewModel { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            _atlasTreeView = (TreeView)FindName("trv_Atlas");
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
        }

        private void TreeView_ItemsSelected(object sender, RoutedEventArgs e)
        {

        }

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

        private void StartNewFile()
        {
            ViewModel = new ViewModel();
            _atlasTreeView.Items.Clear();
            _atlasTreeView.DataContext = ViewModel;
        }
    }
}
