using AnimationEditor.IO;
using AnimationEditor.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace AnimationEditor
{
    /// <summary>
    /// Process command line stuff
    /// </summary>
    public class CLProcessor
    {
        public static void ProcessCommandLineArguments(string[] args)
        {
            if (args.Length == 0)
                return;

            Console.WriteLine("Running Dungeon Sphere Animation Manager through command line...");

            for(int i = 0; i < args.Length; i++)
            {
                var arg = args[i];

                // Is a command flag
                if (arg.StartsWith('-'))
                {
                    i = HandleOption(arg, i);
                }
            }

            // If running with command line arguments, completely kill the program. Do not allow window to open.
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Handles the option from the command line arguments. Returns the new index after processing occurs
        /// </summary>
        /// <param name="option">The option to process</param>
        /// <param name="index">The index of the option within the arguments</param>
        /// <returns>The new index after processing has occurred.</returns>
        private static int HandleOption(string option, int index)
        {
            if (option.Length == 1)
                return index;

            var args = App.Args;

            option = option.Substring(1);

            switch(option)
            {
                case "open":
                    break;
                case "save":
                    break;
                case "refresh":
                    // Opens file, reads data, ands saves it back into same file. This is done to update any new sprites added into the atlas.
                    if (args.Length <= index + 1)
                    {
                        Console.WriteLine("\tExpected filename after " + option);
                        Process.GetCurrentProcess().Kill();
                    }
                    index++;
                    var filename = args[index];
                    Console.WriteLine("\tRefreshing file " + filename);

                    filename = System.IO.Path.GetFullPath(filename);

                    var data = FileReader.Read(filename);
                    var vm = new TextureAtlasesViewModel();
                    TextureAtlasesViewModel.LoadData(vm, filename, data);
                    FileWriter.Write(filename, vm.Root);
                    break;
            }

            return index;
        }
    }
}
