using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace AnimationManager
{
    [Serializable]
    [XmlRoot]
    public class AppProperties
    {
        [XmlElement]
        public double MainTop { get; set; }
        [XmlElement]
        public double MainLeft { get; set; }
        [XmlElement]
        public double MainWidth { get; set; }
        [XmlElement]
        public double MainHeight { get; set; }
        [XmlElement]
        public bool Maximized { get; set; }
        [XmlElement]
        public double PreviewTop { get; set; }
        [XmlElement]
        public double PreviewLeft { get; set; }
        [XmlElement]
        public double PreviewWidth { get; set; }
        [XmlElement]
        public double PreviewHeight { get; set; }
        [XmlElement]
        public bool PreviewVisible { get; set; }
    }

    public class AppPropertiesReaderWriter
    {
        public static void Write(string filename, AppProperties properties)
        {
            var xml = new XmlSerializer(typeof(AppProperties));
            using (var writer = new StreamWriter(filename))
            {
                xml.Serialize(writer, properties);
            }
        }

        public static AppProperties Read(string filename)
        {
            var xml = new XmlSerializer(typeof(AppProperties));
            using (var reader = new StreamReader(filename))
            {
                var properties = (AppProperties)xml.Deserialize(reader);
                return properties;
            }
        }
    }
}
