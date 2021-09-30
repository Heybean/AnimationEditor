using AnimationEditor.Controls;
using AnimationEditor.Graphics;
using AnimationEditor.IO;
using AnimationEditor.View;
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
        public MainWindow()
        {
            InitializeComponent();

            //LoadAppProperties();
        }

       /* public void LoadAppProperties()
        {
            var properties = AppPropertiesReaderWriter.Read();
            if (properties != null)
                SetAppProperties(properties);
        }

        public void SaveAppProperties()
        {
            AppPropertiesReaderWriter.Write(GetAppProperties());
        }

        private void SetAppProperties(AppSettings properties)
        {
            Top = properties.MainTop;
            Left = properties.MainLeft;
            Width = properties.MainWidth;
            Height = properties.MainHeight;
            if (properties.Maximized)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;

            foreach(var win in Application.Current.Windows)
            {
                if (win is SpritePreviewView spr)
                {
                    spr.Left = properties.PreviewLeft;
                    spr.Top = properties.PreviewTop;
                    spr.Width = properties.PreviewWidth;
                    spr.Height = properties.PreviewHeight;
                    spr.Visibility = properties.PreviewVisible ? Visibility.Visible : Visibility.Hidden;
                }
            }

            if (DataContext is MainViewModel vm)    // This break mvvm, will need to find better way to do this later
                vm.SetAppProperties(properties);
        }

        public AppSettings GetAppProperties()
        {
            var properties = new AppSettings();
            properties.MainTop = Top;
            properties.MainLeft = Left;
            properties.MainWidth = Width;
            properties.MainHeight = Height;
            properties.Maximized = WindowState == WindowState.Maximized;

            foreach (var win in Application.Current.Windows)
            {
                if (win is SpritePreviewView spr)
                {
                    properties.PreviewLeft = spr.Left;
                    properties.PreviewTop = spr.Top;
                    properties.PreviewWidth = spr.Width;
                    properties.PreviewHeight = spr.Height;
                    properties.PreviewVisible = spr.Visibility == Visibility.Visible;
                }
            }

            if (DataContext is MainViewModel vm)
                vm.GetAppProperties(ref properties);

            return properties;
        }*/
    }
}
