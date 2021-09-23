using AnimationEditor.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Xml.Serialization;

namespace AnimationEditor.ViewModel
{
    /// <summary>
    /// Holds information on current working data
    /// </summary>
    //[XmlRoot("Animations")]
    //[XmlInclude(typeof(WpfTextureAtlas))]
    public class TextureAtlasViewModel
    {
        //[XmlElement("Atlases")]
        public ObservableCollection<WpfTextureAtlas> TextureAtlases { get; set; } = new ObservableCollection<WpfTextureAtlas>();
        //[XmlIgnore]
        public ICollectionView TextureAtlasesView { get; private set; }
        //[XmlIgnore]
        public HashSet<string> RegisteredTextureAtlases { get; } = new HashSet<string>();
        //[XmlIgnore]
        public IList<object> SelectedItems { get; set; } = new List<object>();

        public TextureAtlasViewModel()
        {
            TextureAtlasesView = CollectionViewSource.GetDefaultView(TextureAtlases);
            TextureAtlasesView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }
    }
}
