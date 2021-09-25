using AnimationEditor.Controls;
using AnimationEditor.Graphics;
using AnimationEditor.IO;
using AnimationEditor.ViewModel;
using Heybean.Graphics;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace AnimationEditor
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
        private ComboBox _zoomScale;

        private bool _processingCommandLine;

        public TextureAtlasViewModel TextureAtlasViewModel { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            /*ProcessCommandLineArguments();

            if (_processingCommandLine)
                return;

            DataContext = MainWindowVM;

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
            _zoomScale = (ComboBox)FindName("combo_mainRenderScale");

            RenderOptions.SetBitmapScalingMode(_spriteOutline, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetBitmapScalingMode(_spriteDisplay, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetBitmapScalingMode(_originMarker, BitmapScalingMode.NearestNeighbor);

            _spritePreviewWindow = MainWindowVM.SpritePreviewWindow;

            _mainRenderScale = new ScaleTransform(3, 3);
            _zoomScale.SelectedIndex = 2;

            StartNewFile();*/
        }

        /// <summary>
        /// Prompt save if unsaved changes exists.
        /// </summary>
        /// <returns>True if save or no save was made. False if cancelled.</returns>
        private bool PromptUnsavedChanges()
        {
            // No unsaved changes detected
            /*if (!MainWindowVM.UnsavedChanges)
                return true;

            // Prompt for saving
            var result = MessageBox.Show(this, $"Do you want to save changes to {MainWindowVM.FileName}? Unsaved changes will be lost!", "Save File?", MessageBoxButton.YesNoCancel);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    return PerformSave();
                case MessageBoxResult.No:
                    return true;
                case MessageBoxResult.Cancel:
                    return false;
            }

            return false;*/
            return true;
        }

        private void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            /*if (!PromptUnsavedChanges())
                return;

            StartNewFile();*/
        }

        private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            /*if (!PromptUnsavedChanges())
                return;

            var openFileDialog = new OpenFileDialog()
            {
                Title = "Open File",
                Filter = "animation files (*.anim)|*.anim"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                MainWindowVM.Clear();
                MainWindowVM.SavePath = openFileDialog.FileName;
                MainWindowVM.FileName = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);

                var data = FileReader.Read(openFileDialog.FileName);
                LoadData(openFileDialog.FileName, data);
            }*/
        }

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //PerformSave();
        }

        private void SaveAsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //PerformSaveFile("Save File As");
        }

        private bool PerformSave()
        {
            /*if (MainWindowVM.SavePath.Length <= 0)
            {
                return PerformSaveFile("Save File");
            }
            else
            {
                FileWriter.Write(MainWindowVM.SavePath, TextureAtlasViewModel);
                MainWindowVM.UnsavedChanges = false;
                return true;
            }*/
            return true;
        }

        private bool PerformSaveFile(string title)
        {
            /*var saveFileDialog = new SaveFileDialog()
            {
                Title = title,
                Filter = "animation files (*.anim)|*.anim",
                AddExtension = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                MainWindowVM.UnsavedChanges = false;
                MainWindowVM.SavePath = saveFileDialog.FileName;
                MainWindowVM.FileName = System.IO.Path.GetFileNameWithoutExtension(saveFileDialog.FileName);

                FileWriter.Write(saveFileDialog.FileName, TextureAtlasViewModel);

                return true;
            }

            return false;*/
            return true;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            //Close();
        }

        private void UndoCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void RedoCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            /*_spritePreviewWindow.Owner = this;

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
            }*/
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            /*if (!PromptUnsavedChanges())
            {
                e.Cancel = true;
                return;
            }


            // Save settings
            var properties = GetProperties();
            AppPropertiesReaderWriter.Write(PropertiesFile, properties);

            _spritePreviewWindow.Close();*/
        }

        private void ApplyProperties(AppProperties properties)
        {
            /*Top = properties.MainTop;
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
            _zoomScale.SelectedIndex = properties.ZoomIndex;*/
        }

        public AppProperties GetProperties()
        {
            /*var properties = new AppProperties();
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
            properties.ZoomIndex = _zoomScale.SelectedIndex;
            return properties;*/
            return null;
        }

        private void TreeView_ItemsSelected(object sender, RoutedEventArgs e)
        {
            /*_propertiesPanel.DataContext = new TreeViewSelectedItems(TextureAtlasViewModel.SelectedItems, this.MainWindowVM);

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
                if (TextureAtlasViewModel.SelectedItems[0] is SpriteModel sprite)
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
            }*/
        }

        private void StartNewFile()
        {
            /*TextureAtlasViewModel = new TextureAtlasViewModel();

            if (_processingCommandLine)
                return;

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
            UpdateOriginMarker();*/
        }

        private void OriginX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<int?> e)
        {
            //UpdateOriginMarker();
        }

        private void OriginY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<int?> e)
        {
           // UpdateOriginMarker();
        }

        private void GameTickTimer_Tick(object sender, EventArgs e)
        {
            /*if (TextureAtlasViewModel.SelectedItems.Count == 1)
            {
                if (TextureAtlasViewModel.SelectedItems[0] is SpriteModel sprite)
                {
                    sprite.Update(_gameTickTimer.Interval.TotalMilliseconds);
                }
            }*/
        }

        private void UpdateMainRender()
        {
            /*int x = (int)(_mainRender.ActualWidth - _spriteDisplay.Width) / 2;
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
            Canvas.SetTop(_spriteOutline, y - 1);*/
        }

        private void UpdateOriginMarker()
        {
            // Update the origin marker position
            /*int? originx = _originX.Value;
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

            _spritePreviewWindow.UpdatePreviewRender();*/
        }
        
        private void SpritePreviewWindow_Click(object sender, RoutedEventArgs e)
        {
            /*if (_spritePreviewWindow.IsVisible)
            {
                _spritePreviewWindow.Visibility = Visibility.Hidden;
            }
            else
                _spritePreviewWindow.Visibility = Visibility.Visible;*/
        }

        private void LoadData(string filename, AnimationsFileData data)
        {
            /*StartNewFile();

            var rootFolder = System.IO.Path.GetDirectoryName(filename);
            foreach(var atlasData in data.Root.Atlases)
            {
                var atlas = AddTextureAtlas(rootFolder + "\\" + atlasData.File);

                // Create simple dictionary for quick atlas lookup
                var atlasDict = new Dictionary<string, SpriteModel>();
                foreach(SpriteModel sprite in atlas.Children)
                {
                    atlasDict.Add(sprite.Name, sprite);
                }

                RecreateStructure(atlasData, atlas, atlas, atlasDict);
            }*/
        }

        /*private void RecreateStructure(AnimationsFileData.Folder folderRoot, TextureAtlasTreeItem atlasItem, WpfTextureAtlas atlas, Dictionary<string, SpriteModel> atlasDict)
        {
            // Create the folder in the atlas
            foreach(var folderData in folderRoot.Folders)
            {
                var folder = new TextureAtlasTreeItem() { Name = folderData.Name };
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
        }*/

        private void MarkUnsavedChanges()
        {
            //MainWindowVM.UnsavedChanges = true;
        }

        private void ProcessCommandLineArguments()
        {
            /*if (App.Args.Length == 0)
                return;

            Console.WriteLine("Running Dungeon Sphere Animation Manager through command line...");
            _processingCommandLine = true;

            var args = App.Args;
            for(int i = 0; i < args.Length; i++)
            {
                var arg = args[i];

                // Is a command flag
                if (arg.StartsWith('-'))
                {
                    i = HandleOption(arg, i);
                }
            }

            // If running with command line arguments, completely kill the program. Do not allow window to open.
            Process.GetCurrentProcess().Kill();*/
        }

        /// <summary>
        /// Handles the option from the command line arguments. Returns the new index after processing occurs
        /// </summary>
        /// <param name="option">The option to process</param>
        /// <param name="index">The index of the option within the arguments</param>
        /// <returns>The new index after processing has occurred.</returns>
        private int HandleOption(string option, int index)
        {
            /*if (option.Length == 1)
                return index;

            var args = App.Args;

            option = option.Substring(1);

            switch(option)
            {
                case "open":
                    break;
                case "save":
                    break;
                case "refresh":
                    // Opens file, reads data, ands saves it back into same file. This is done to update any new sprites added into the atlas.
                    if (args.Length <= index + 1)
                    {
                        Console.WriteLine("\tExpected filename after " + option);
                        Process.GetCurrentProcess().Kill();
                    }
                    index++;
                    var filename = args[index];
                    Console.WriteLine("\tRefreshing file " + filename);

                    filename = System.IO.Path.GetFullPath(filename);

                    var data = FileReader.Read(filename);
                    LoadData(filename, data);
                    FileWriter.Write(filename, TextureAtlasViewModel);
                    break;
            }

            return index;*/
            return 0;
        }
    }
}
