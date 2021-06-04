using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DVMusicEdit
{
    public static class Steam
    {
        private static string[] _libraries;

        public static string[] Libraries
        {
            get
            {
                ScanLib();
                return (string[])_libraries.Clone();
            }
        }

        public static void ScanLib()
        {
            ScanLib(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            ScanLib(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall");
        }

        private static void ScanLib(string MasterKey)
        {
            var Libs = new List<string>();
            using (var Uninstall = Registry.LocalMachine.OpenSubKey(MasterKey, false))
            {
                foreach (var K in Uninstall.GetSubKeyNames())
                {
                    using (var Software = Uninstall.OpenSubKey(K, false))
                    {
                        var N = (string)Software.GetValue("DisplayName");
                        if (N == "Steam")
                        {
                            var P = (string)Software.GetValue("UninstallString");
                            if (!string.IsNullOrEmpty(P))
                            {
                                P = Path.Combine(Path.GetDirectoryName(P), "steamapps");
                                if (Directory.Exists(P))
                                {
                                    //The main library itself is added too because it's not contained in the configuration file.
                                    Libs.Add(P);
                                    Libs.AddRange(GetLibraryConfig(P));
                                }
                            }
                        }
                    }
                }
            }
            _libraries = Libs.ToArray();
        }

        private static string[] GetLibraryConfig(string MasterLibrary)
        {
            //This matches: "<number>" "<anything>"
            //This captures: <anything>
            var R = new Regex("^\"\\d+\"\\s+\"(.*)\"$");
            var Libs = new List<string>();
            var MasterFile = Path.Combine(MasterLibrary, "libraryfolders.vdf");
            if(File.Exists(MasterFile))
            {
                var Lines = File.ReadAllText(MasterFile).Split('\n');
                foreach(var Line in Lines)
                {
                    var L = Line.Trim();
                    var M = R.Match(L);
                    if (M.Success)
                    {
                        //Note: The string is "escaped" using backslashes.
                        //For now, we just replace double backslashes, but if the escaping is ever a problem,
                        //We can probably load a JSON library and feed the escaped string through the decoder function.
                        Libs.Add(M.Groups[1].Value.Replace(@"\\", @"\"));
                    }
                }
            }
            return Libs.ToArray();
        }
    }
}
