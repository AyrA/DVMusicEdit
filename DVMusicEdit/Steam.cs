using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var Libs = new List<string>();
            var MasterFile = Path.Combine(MasterLibrary, "libraryfolders.vdf");
            return Libs.ToArray();
        }
    }
}
