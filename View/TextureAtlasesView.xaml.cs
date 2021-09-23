using AnimationEditor.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnimationEditor.View
{
    /// <summary>
    /// Interaction logic for TextureAtlasesView.xaml
    /// </summary>
    public partial class TextureAtlasesView : UserControl
    {
        private TextureAtlasesViewModel _viewModel;

        public TextureAtlasesView()
        {
            InitializeComponent();

            _viewModel = new TextureAtlasesViewModel();
            DataContext = _viewModel;
        }
    }
}
