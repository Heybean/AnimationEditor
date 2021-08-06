using Heybean.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace AnimationManager.IO
{
    public class FileReader
    {
        public static AnimationsFileData Read(string filename)
        {
            /*var xml = new XmlSerializer(typeof(AnimationsFileData.Animations));

            using (var reader = new FileStream(filename, FileMode.Open))
            {
                var data = (AnimationsFileData.Animations)xml.Deserialize(reader);
                var afd = new AnimationsFileData()
                {
                    Root = data
                };
                return afd;
            }*/

            return new AnimationsFileData(filename);
        }
    }
}
