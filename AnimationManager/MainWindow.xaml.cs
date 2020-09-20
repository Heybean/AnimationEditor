using DungeonSphere.Graphics;
using Microsoft.Win32;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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

namespace AnimationManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameRender _game;
        private TreeView _atlasTreeView;
        private ProjectData _data;

        public MainWindow()
        {
            InitializeComponent();

            _data = new ProjectData();

            _game = (GameRender)FindName("gr_GameWindow");
            _atlasTreeView = (TreeView)FindName("trv_Atlas");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddAtlas_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.AddExtension = true;
            openFileDialog.Filter = "texture atlas files (*.atlas)|*.atlas";
            openFileDialog.Title = "Add Texture Atlas";

            if (openFileDialog.ShowDialog() == true)
            {
                AddTextureAtlas(openFileDialog.FileName);
            }
        }

        private void RemoveAtlas_Click(object sender, RoutedEventArgs e)
        {
            if (_atlasTreeView.SelectedItem != null)
            {
                _atlasTreeView.Items.Remove(_atlasTreeView.SelectedItem);
            }
        }

        private void AddTextureAtlas(string file)
        {
            //var texture = Utils.LoadTexture2D(_game.GraphicsDevice, file);
            var atlasName = Path.GetFileName(file);

            if (_data.AnimationAtlases.ContainsKey(atlasName))
            {
                MessageBox.Show("Cannot add '" + atlasName + "' because it already exists.", "Add Texture Atlas", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Load the atlas
            var atlas = new TextureAtlas();

            var animationsData = new AnimationsData();

            var item = new TreeViewItem();
            item.Header = file;
            _atlasTreeView.Items.Add(atlasName);

            //_data.TextureAtlases.Add(atlasName, null);
        }
    }
}
