using AnimationManager.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace AnimationManager.IO
{
    public class FileWriter
    {
        private ViewModel _viewModel;

        /// <summary>
        /// Writes out the data to XML file based on data in the view model
        /// </summary>
        public FileWriter(ViewModel data)
        {
            _viewModel = data;
        }

        public void Write(string filename)
        {
            var xml = new XmlSerializer(typeof(WpfTextureAtlas));
            using (var writer = new StreamWriter(filename))
            {
                xml.Serialize(writer, _viewModel.TextureAtlases);
            }
        }
    }
}
