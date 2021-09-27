using AnimationEditor.Graphics;
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
        public static void Write(string filename, Node root)
        {
            // Find the relative paths for each texture atlas
            string directory = Path.GetDirectoryName(filename);
            foreach(TextureAtlasModel atlas in root.SubNodes)
            {
                atlas.RelativePath = Path.GetRelativePath(directory, atlas.Filename);
            }

            using (var writer = new XmlTextWriter(filename, null))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartElement("Animations");

                WriteChildren(writer, root);

                writer.WriteEndElement();
            }
        }

        private static void WriteChildren(XmlTextWriter writer, Node parentNode)
        {
            foreach(var node in parentNode.SubNodes)
            {
                if (node is FolderModel folder)
                {
                    writer.WriteStartElement("Folder");
                    writer.WriteAttributeString("name", folder.Name);
                    WriteChildren(writer, folder);
                    writer.WriteEndElement();
                }
                else if (node is TextureAtlasModel atlas)
                {
                    writer.WriteStartElement("Atlas");
                    writer.WriteAttributeString("name", atlas.Name);
                    writer.WriteAttributeString("file", atlas.RelativePath);
                    WriteChildren(writer, atlas);
                    writer.WriteEndElement();
                }
                else if (node is SpriteModel sprite)
                {
                    writer.WriteStartElement("Sprite");
                    writer.WriteAttributeString("name", sprite.Name);
                    writer.WriteAttributeString("fps", sprite.FPS.ToString());
                    writer.WriteAttributeString("originx", sprite.OriginX.ToString());
                    writer.WriteAttributeString("originy", sprite.OriginY.ToString());
                    writer.WriteAttributeString("halign", sprite.HAlign.ToString());
                    writer.WriteAttributeString("valign", sprite.VAlign.ToString());
                    writer.WriteEndElement();
                }
            }
        }
    }
}
