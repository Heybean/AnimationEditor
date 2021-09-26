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

        //public TextureAtlasViewModel TextureAtlasViewModel { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
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
