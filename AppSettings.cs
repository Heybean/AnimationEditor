using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml.Serialization;

namespace AnimationEditor
{
    [Serializable]
    [XmlRoot]
    public sealed class AppSettings : NotifyBase
    {
        private const string Filename = "app_prop.xml";

        private WindowState _winState = WindowState.Maximized;
        private WindowState _prevWinState;

        [XmlElement]
        public double MainTop { get; set; }
        [XmlElement]
        public double MainLeft { get; set; }
        [XmlElement]
        public double MainWidth { get; set; } = 1200;
        [XmlElement]
        public double MainHeight { get; set; } = 800;
        [XmlElement]
        public WindowState WinState
        {
            get => _winState;
            set
            {
                if (value == WindowState.Minimized && _winState != WindowState.Minimized)
                    _prevWinState = _winState;
                _winState = value;
            }
        }

        [XmlElement]
        public double PreviewTop { get; set; }
        [XmlElement]
        public double PreviewLeft { get; set; }
        [XmlElement]
        public double PreviewWidth { get; set; } = 200;
        [XmlElement]
        public double PreviewHeight { get; set; } = 200;
        [XmlElement]
        public Visibility PreviewVisibility { get; set; } = Visibility.Visible;
        [XmlElement]
        public int MainZoomIndex { get; set; } = 2;
        [XmlElement]
        public int PreviewZoomIndex { get; set; } = 0;
        [XmlElement]
        public double MainSplitter1 { get; set; } = 200;
        [XmlElement]
        public double MainSplitter2 { get; set; } = 200;
        [XmlElement]
        public double ControlsSplitter { get; set; } = 200;

        [XmlIgnore]
        public bool SettingsLoaded { get; set; } = false;

        private static AppSettings instance = null;
        private static readonly object padlock = new object();

        public static AppSettings Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                        instance = new AppSettings();
                    return instance;
                }
            }
        }

        public static void Write()
        {
            var xml = new XmlSerializer(typeof(AppSettings));
            using (var writer = new StreamWriter(Filename))
            {
                WindowState currWinState = Instance.WinState;

                if (Instance.WinState == WindowState.Minimized)
                {
                    Instance.WinState = Instance._prevWinState;
                }

                xml.Serialize(writer, Instance);

                Instance.WinState = currWinState;
            }
        }

        public static void Read()
        {
            if (!File.Exists(Filename))
                return;

            try
            {
                var xml = new XmlSerializer(typeof(AppSettings));
                using (var reader = new StreamReader(Filename))
                {
                    instance = (AppSettings)xml.Deserialize(reader);
                    instance.SettingsLoaded = true;
                }
            }
            catch
            {
                // Do nothing
            }
        }
    }
}
