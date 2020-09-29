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
using System.Windows.Threading;

namespace AnimationManager
{
    public partial class MainWindow : Window
    {
        private TreeView _atlasTreeView;
        private Button _buttonRemoveAtlas;
        private Canvas _mainRender;
        private DispatcherTimer _gameTickTimer;

        public ViewModel ViewModel { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            _gameTickTimer = new DispatcherTimer();
            _gameTickTimer.Tick += GameTickTimer_Tick;
            _gameTickTimer.Interval = TimeSpan.FromSeconds(1 / 60.0);
            _gameTickTimer.IsEnabled = true;

            _atlasTreeView = (TreeView)FindName("trv_Atlas");
            _buttonRemoveAtlas = (Button)FindName("btn_RemoveAtlas");
            _mainRender = (Canvas)FindName("cv_MainRender");
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
                Title = "Add Texture Atlas",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var invalidFiles = new List<string>();
                foreach (var filename in openFileDialog.FileNames)
                {
                    AddTextureAtlas(filename, invalidFiles);
                }

                if (invalidFiles.Count > 0)
                {
                    MessageBox.Show("Cannot add '" + string.Join("', '", invalidFiles)  + "' because it already exists.", "Add Texture Atlas", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
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
                    RemoveTextureAtlas(atlas);
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

            if (ViewModel.SelectedItems.Count == 1)
            {
                if (ViewModel.SelectedItems[0] is WpfSprite)
                {
                    var sprite = ViewModel.SelectedItems[0] as WpfSprite;
                    _mainRender.Children.Clear();
                    var rect = new Rectangle();
                    rect.Width = sprite.Regions[0].width;
                    rect.Height = sprite.Regions[0].height;
                    rect.Fill = sprite.Regions[0].ImageBrush;
                    rect.RenderTransform = new ScaleTransform(20, 20, 1, 1);
                    RenderOptions.SetBitmapScalingMode(rect, BitmapScalingMode.NearestNeighbor);
                    _mainRender.Children.Add(rect);
                }
            }
        }

        /// <summary>
        /// Adds a single texture atlas into the project.
        /// </summary>
        private void AddTextureAtlas(string file, List<string> invalidFiles = null)
        {
            var atlasName = System.IO.Path.GetFileNameWithoutExtension(file);

            if (ViewModel.RegisteredTextureAtlases.Contains(atlasName))
            {
                //
                if (invalidFiles != null)
                    invalidFiles.Add(atlasName);
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

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            DrawMainRender();
        }

        private void GameTickTimer_Tick(object sender, EventArgs e)
        {

        }

        private void DrawMainRender()
        {
            /*bool drawFinished = false;

            while (!drawFinished)
            {
                // Draw currently selected sprite if only one is selected
                if (ViewModel.SelectedItems.Count == 1)
                {
                    if (ViewModel.SelectedItems[0] is WpfSprite)
                    {
                        var sprite = ViewModel.SelectedItems[0] as WpfSprite;
                        _mainRender.Children.Clear();
                    }
                }
                drawFinished = true;
            }*/

        }
    }
}
