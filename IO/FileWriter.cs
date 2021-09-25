﻿using AnimationEditor.Graphics;
using AnimationEditor.Model;
using AnimationEditor.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace AnimationEditor.IO
{
    public class FileWriter
    {
        public static void Write(string filename, IEnumerable root)
        {
            var directory = Path.GetDirectoryName(filename);

            // Find the relative paths for each texture atlas
            /*foreach(Node item in atlases)
            {
                if (item is TextureAtlasModel atlas)
                {
                    atlas.RelativePath = Path.GetRelativePath(directory, atlas.Filename);
                }
            }*/

            WriteNode(root);

            /*using (var writer = new XmlTextWriter(filename, null))
            {
                writer.WriteStartElement("Animations");
                foreach(var atlas in viewModel.TextureAtlases)
                {
                    writer.Formatting = Formatting.Indented;
                    writer.WriteStartElement("Atlas");
                    writer.WriteAttributeString("name", atlas.Name);
                    writer.WriteAttributeString("file", atlas.RelativePath);
                    WriteChildren(writer, atlas.Children);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }*/
        }

        private static void WriteNode(IEnumerable nodes)
        {
            foreach(Node item in nodes)
            {
                WriteNode(item.SubNodes);
            }
        }

        private static void WriteChildren(XmlTextWriter writer, IList<object> list)
        {
            /*foreach (var child in list)
            {
                if (child is TextureAtlasTreeItem folder)
                {
                    writer.WriteStartElement("Folder");
                    writer.WriteAttributeString("name", folder.Name);
                    WriteChildren(writer, folder.Children);
                    writer.WriteEndElement();
                }
                else if (child is SpriteModel sprite)
                {
                    writer.WriteStartElement("Sprite");
                    writer.WriteAttributeString("name", sprite.Name);
                    writer.WriteAttributeString("fps", sprite.FPS.ToString());
                    writer.WriteAttributeString("originx", sprite.OriginX.ToString());
                    writer.WriteAttributeString("originy", sprite.OriginY.ToString());
                    writer.WriteAttributeString("halign", sprite.HorizontalAlignment.ToString());
                    writer.WriteAttributeString("valign", sprite.VerticalAlignment.ToString());
                    writer.WriteEndElement();
                }
            }*/
        }
    }
}
