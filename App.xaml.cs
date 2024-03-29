﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AnimationEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string[] Args;

        void App_Startup(object sender, StartupEventArgs e)
        {
            Args = e.Args;

            CLProcessor.ProcessCommandLineArguments(Args);

            AppSettings.Read();
        }
    }
}
