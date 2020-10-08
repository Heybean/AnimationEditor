using AnimationManager.Controls;
using AnimationManager.Graphics;
using AnimationManager.IO;
using DungeonSphere.Graphics;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
        private const string PropertiesFile = "app_prop.xml";

        private TreeView _atlasTreeView;
        private Button _buttonRemoveAtlas;
        private Canvas _mainRender;
        private ScaleTransform _mainRenderScale;
        private DispatcherTimer _gameTickTimer;
        private Rectangle _spriteOutline;
        private Rectangle _spriteDisplay;
        private DockPanel _propertiesPanel;
        private NumericUpDown _originX;
        private NumericUpDown _originY;
        private Image _originMarker;
        private SpritePreviewWindow _spritePreviewWindow;

        private MainWindowViewModel MainWindowViewModel { get; set; } = new MainWindowViewModel();

        public TextureAtlasViewModel TextureAtlasViewModel { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = MainWindowViewModel;

            _gameTickTimer = new DispatcherTimer();
            _gameTickTimer.Tick += GameTickTimer_Tick;
            _gameTickTimer.Interval = TimeSpan.FromSeconds(1 / 60.0);
            _gameTickTimer.IsEnabled = true;

            _atlasTreeView = (TreeView)FindName("trv_Atlas");
            _buttonRemoveAtlas = (Button)FindName("btn_RemoveAtlas");
            _mainRender = (Canvas)FindName("cv_MainRender");
            _spriteOutline = (Rectangle)FindName("rect_SpriteOutline");
            _spriteDisplay = (Rectangle)FindName("rect_SpriteDisplay");
            _propertiesPanel = (DockPanel)FindName("dock_Properties");
            _originMarker = (Image)FindName("img_Origin");
            _originX = (NumericUpDown)FindName("num_OriginX");
            _originY = (NumericUpDown)FindName("num_OriginY");

            RenderOptions.SetBitmapScalingMode(_spriteOutline, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetBitmapScalingMode(_spriteDisplay, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetBitmapScalingMode(_originMarker, BitmapScalingMode.NearestNeighbor);

            _mainRenderScale = new ScaleTransform(3, 3);

            _spritePreviewWindow = MainWindowViewModel.SpritePreviewWindow;

            StartNewFile();
        }

        /// <summary>
        /// Prompt save if unsaved changes exists.
        /// </summary>
        /// <returns>True if save or no save was made. False if cancelled.</returns>
        private bool PromptUnsavedChanges()
        {
            if (!MainWindowViewModel.UnsavedChanges)
                return false;

            return false;
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            //if (!PromptUnsavedChanges())
            //    return;

            StartNewFile();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Title = "Open File",
                Filter = "animation files (*.anim)|*.anim"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                MainWindowViewModel.Clear();
                MainWindowViewModel.SavePath = openFileDialog.FileName;
                MainWindowViewModel.FileName = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);

                var data = FileReader.Read(openFileDialog.FileName);
                LoadData(openFileDialog.FileName, data);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            PerformSaveFile("Save File As");
        }

        private bool PerformSaveFile(string title)
        {
            var saveFileDialog = new SaveFileDialog()
            {
                Title = title,
                Filter = "animation files (*.anim)|*.anim",
                AddExtension = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                MainWindowViewModel.UnsavedChanges = false;
                MainWindowViewModel.SavePath = saveFileDialog.FileName;
                MainWindowViewModel.FileName = System.IO.Path.GetFileNameWithoutExtension(saveFileDialog.FileName);

                FileWriter.Write(saveFileDialog.FileName, TextureAtlasViewModel);

                return true;
            }

            return false;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            _spritePreviewWindow.Owner = this;

            if (File.Exists(PropertiesFile))
            {
                var properties = AppPropertiesReaderWriter.Read(PropertiesFile);
                ApplyProperties(properties);
            }
            else
            {
                WindowState = WindowState.Maximized;
                _spritePreviewWindow.Show();
                var position = _mainRender.TransformToAncestor(this).Transform(new Point(0, 0));
                _spritePreviewWindow.Left = position.X + _mainRender.ActualWidth - _spritePreviewWindow.Width;
                _spritePreviewWindow.Top = position.Y + 50;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            // Save settings
            var properties = GetProperties();
            AppPropertiesReaderWriter.Write(PropertiesFile, properties);

            _spritePreviewWindow.Close();
        }

        private void ApplyProperties(AppProperties properties)
        {
            Top = properties.MainTop;
            Left = properties.MainLeft;
            Width = properties.MainWidth;
            Height = properties.MainHeight;
            if (properties.Maximized)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
            _spritePreviewWindow.Top = properties.PreviewTop;
            _spritePreviewWindow.Left = properties.PreviewLeft;
            _spritePreviewWindow.Width = properties.PreviewWidth;
            _spritePreviewWindow.Height = properties.PreviewHeight;
            if (properties.PreviewVisible)
                _spritePreviewWindow.Show();
        }

        public AppProperties GetProperties()
        {
            var properties = new AppProperties();
            properties.MainTop = Top;
            properties.MainLeft = Left;
            properties.MainWidth = Width;
            properties.MainHeight = Height;
            properties.Maximized = WindowState == WindowState.Maximized;
            properties.PreviewTop = _spritePreviewWindow.Top;
            properties.PreviewLeft = _spritePreviewWindow.Left;
            properties.PreviewWidth = _spritePreviewWindow.Width;
            properties.PreviewHeight = _spritePreviewWindow.Height;
            properties.PreviewVisible = _spritePreviewWindow.IsVisible;
            return properties;
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
            foreach(var x in TextureAtlasViewModel.SelectedItems.ToList())
            {
                if (x is WpfTextureAtlas atlas)
                {
                    RemoveTextureAtlas(atlas);
                }
            }
        }

        private void TreeView_ItemsSelected(object sender, RoutedEventArgs e)
        {
            _propertiesPanel.DataContext = new TreeViewSelectedItems(TextureAtlasViewModel.SelectedItems);

            // Toggle Remove Atlas button based on which items are selected
            bool onlyTextureAtlasesSelected = true;
            foreach(var item in TextureAtlasViewModel.SelectedItems)
            {
                if (!(item is WpfTextureAtlas))
                {
                    onlyTextureAtlasesSelected = false;
                    break;
                }
            }

            _buttonRemoveAtlas.IsEnabled = onlyTextureAtlasesSelected;

            if (TextureAtlasViewModel.SelectedItems.Count == 1) // Single Item Selected
            {
                if (TextureAtlasViewModel.SelectedItems[0] is WpfSprite sprite)
                {
                    _spriteOutline.StrokeThickness = 1;

                    _spriteDisplay.Width = sprite.Regions[0].width;
                    _spriteDisplay.Height = sprite.Regions[0].height;
                    _spriteDisplay.Fill = sprite.Regions[0].ImageBrush;

                    UpdateMainRender();
                    UpdateOriginMarker();

                    _spritePreviewWindow.SetSprite(sprite);
                }
            }
            else
            {
                _spriteDisplay.Fill = null;
                _spriteOutline.StrokeThickness = 0;
            }
        }

        /// <summary>
        /// Adds a single texture atlas into the project. Texture files that can't be added are placed in invalidFiles (optional)
        /// </summary>
        /// <returns>The atlas that was created</returns>
        private WpfTextureAtlas AddTextureAtlas(string file, List<string> invalidFiles = null)
        {
            var atlasName = System.IO.Path.GetFileNameWithoutExtension(file);

            if (TextureAtlasViewModel.RegisteredTextureAtlases.Contains(atlasName))
            {
                if (invalidFiles != null)
                    invalidFiles.Add(atlasName);
                return null;
            }

            // Load the atlas
            var atlas = new WpfTextureAtlas(file);
            TextureAtlasViewModel.RegisteredTextureAtlases.Add(atlasName);
            TextureAtlasViewModel.TextureAtlases.Add(atlas);

            return atlas;
        }

        /// <summary>
        /// Removes a single texture atlas from the project
        /// </summary>
        private void RemoveTextureAtlas(WpfTextureAtlas atlas)
        {
            TextureAtlasViewModel.RegisteredTextureAtlases.Remove(atlas.Name);
            TextureAtlasViewModel.TextureAtlases.Remove(atlas);
        }

        private void StartNewFile()
        {
            TextureAtlasViewModel = new TextureAtlasViewModel();
            _atlasTreeView.DataContext = TextureAtlasViewModel;
            _propertiesPanel.DataContext = null;
            _spritePreviewWindow.SetSprite(null);

            _spriteDisplay.Width = 0;
            _spriteDisplay.Height = 0;
            _spriteDisplay.Fill = null;

            _spriteOutline.Width = 0;
            _spriteOutline.Height = 0;
            _spriteOutline.StrokeThickness = 0;

            UpdateMainRender();
            UpdateOriginMarker();
        }

        private void OriginX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<int?> e)
        {
            UpdateOriginMarker();
        }

        private void OriginY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<int?> e)
        {
            UpdateOriginMarker();
        }

        private void GameTickTimer_Tick(object sender, EventArgs e)
        {
            if (TextureAtlasViewModel.SelectedItems.Count == 1)
            {
                if (TextureAtlasViewModel.SelectedItems[0] is WpfSprite sprite)
                {
                    sprite.Update(_gameTickTimer.Interval.TotalMilliseconds);
                }
            }
        }

        private void MainRender_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateMainRender();
            UpdateOriginMarker();
        }

        private void UpdateMainRender()
        {
            int x = (int)(_mainRender.ActualWidth - _spriteDisplay.Width) / 2;
            int y = (int)(_mainRender.ActualHeight - _spriteDisplay.Height) / 2;

            _mainRenderScale.CenterX = _spriteDisplay.Width / 2;
            _mainRenderScale.CenterY = _spriteDisplay.Height / 2;

            _spriteDisplay.RenderTransform = _mainRenderScale;

            Canvas.SetLeft(_spriteDisplay, x);
            Canvas.SetTop(_spriteDisplay, y);

            // Render the outline
            _spriteOutline.Width = _spriteDisplay.Width * _mainRenderScale.ScaleX + 2;
            _spriteOutline.Height = _spriteDisplay.Height * _mainRenderScale.ScaleY + 2;

            x = (int)(_mainRender.ActualWidth - _spriteDisplay.Width * _mainRenderScale.ScaleX) / 2;
            y = (int)(_mainRender.ActualHeight - _spriteDisplay.Height * _mainRenderScale.ScaleY) / 2;

            Canvas.SetLeft(_spriteOutline, x - 1);
            Canvas.SetTop(_spriteOutline, y - 1);
        }

        private void UpdateOriginMarker()
        {
            // Update the origin marker position
            int? originx = _originX.Value;
            int? originy = _originY.Value;

            if (originx == null || originy == null)
                return;

            int x = (int)(_mainRender.ActualWidth - _spriteDisplay.Width * _mainRenderScale.ScaleX) / 2;
            int y = (int)(_mainRender.ActualHeight - _spriteDisplay.Height * _mainRenderScale.ScaleY) / 2;

            x -= (int)_originMarker.Source.Width / 2;
            y -= (int)_originMarker.Source.Height / 2;

            x += (int)(originx * _mainRenderScale.ScaleX);
            y += (int)(originy * _mainRenderScale.ScaleY);

            Canvas.SetLeft(_originMarker, x);
            Canvas.SetTop(_originMarker, y);

            _spritePreviewWindow.UpdatePreviewRender();
        }
        
        private void SpritePreviewWindow_Click(object sender, RoutedEventArgs e)
        {
            if (_spritePreviewWindow.IsVisible)
            {
                _spritePreviewWindow.Visibility = Visibility.Hidden;
            }
            else
                _spritePreviewWindow.Visibility = Visibility.Visible;
        }

        private void LoadData(string filename, AnimationsFileData data)
        {
            StartNewFile();

            var rootFolder = System.IO.Path.GetDirectoryName(filename);
            foreach(var atlasData in data.Root.Atlases)
            {
                var atlas = AddTextureAtlas(rootFolder + "\\" + atlasData.File);

                // Create simple dictionary for quick atlas lookup
                var atlasDict = new Dictionary<string, WpfSprite>();
                foreach(WpfSprite sprite in atlas.Children)
                {
                    atlasDict.Add(sprite.Name, sprite);
                }

                RecreateStructure(atlasData, atlas, atlas, atlasDict);
            }
        }

        private void RecreateStructure(AnimationsFileData.Folder folderRoot, TextureAtlasItem atlasItem, WpfTextureAtlas atlas, Dictionary<string, WpfSprite> atlasDict)
        {
            // Create the folder in the atlas
            foreach(var folderData in folderRoot.Folders)
            {
                var folder = new TextureAtlasItem() { Name = folderData.Name };
                atlasItem.Children.Add(folder);
                RecreateStructure(folderData, folder, atlas, atlasDict);
            }

            // Find the sprite in the atlasDict
            foreach(var spriteData in folderRoot.Sprites)
            {
                WpfSprite sprite;
                atlasDict.TryGetValue(spriteData.Name, out sprite);
                if (sprite == null)
                    continue;

                // Update the sprite
                sprite.FPS = spriteData.FPS;
                sprite.OriginX = spriteData.OriginX;
                sprite.OriginY = spriteData.OriginY;
                sprite.HorizontalAlignment = (SpriteHorizontalAlignment)Enum.Parse(typeof(SpriteHorizontalAlignment), spriteData.HorizontalAlignment, true);
                sprite.VerticalAlignment = (SpriteVerticalAlignment)Enum.Parse(typeof(SpriteVerticalAlignment), spriteData.VerticalAlignment, true);

                // Remove sprite and place in its proper position
                if (!atlasItem.Children.Contains(sprite))
                {
                    atlas.Children.Remove(sprite);
                    atlasItem.Children.Add(sprite);
                }
            }
        }
    }
}
