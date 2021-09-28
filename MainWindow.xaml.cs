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
        //private SpritePreviewWindow _spritePreviewWindow;
        private ComboBox _zoomScale;

        private bool _processingCommandLine;

        public MainWindow()
        {
            InitializeComponent();
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
