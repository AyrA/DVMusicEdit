using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DVMusicEdit
{
    /// <summary>
    /// Tries to find a steam game installation folder
    /// </summary>
    public static class DerailValleyFinder
    {
        public const string GameName = "Derail Valley";

        /// <summary>
        /// Standard software key
        /// </summary>
        private const string KEY_SOFTWARE = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        /// <summary>
        /// Software key for 32 bit installations on 64 bit systems
        /// </summary>
        private const string KEY_SOFTWARE32 = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

        private static string[] _libraries;

        public static string[] Libraries
        {
            get
            {
                if (_libraries == null)
                {
                    ScanLib();
                }
                return (string[])_libraries.Clone();
            }
        }

        public static string FindDerailValley()
        {
            var str = FindDV(KEY_SOFTWARE);
            if (string.IsNullOrEmpty(str))
            {
                str = FindDV(KEY_SOFTWARE32);
            }
            return str;
        }

        private static void ScanLib()
        {
            _libraries = ScanLib(KEY_SOFTWARE).Concat(ScanLib(KEY_SOFTWARE32)).ToArray();
            Debug.Print("Found {0} libraries", _libraries.Length);
        }

        private static string FindDV(string masterKey)
        {
            Debug.Print("Trying to find derail valley at {0}", masterKey);
            //Derail Valley
            using (var Uninstall = Registry.LocalMachine.OpenSubKey(masterKey, false))
            {
                foreach (var keyName in Uninstall.GetSubKeyNames())
                {
                    using (var Software = Uninstall.OpenSubKey(keyName, false))
                    {
                        var keyValue = (string)Software.GetValue("DisplayName");
                        if (keyValue == null)
                        {
                            Debug.Print("Key {0} has no DisplayName value", Software.Name);
                            continue;
                        }
                        if (keyValue.ToLower() == GameName.ToLower())
                        {
                            var location = (string)Software.GetValue("InstallLocation");
                            Debug.Print("Registry Scan: Found {0} at {1}", keyValue, location);
                            return location;
                        }
                        Debug.Print("Registry Scan: Skipped {0}", keyValue);
                    }
                }
            }
            return null;
        }

        private static IEnumerable<string> ScanLib(string masterKey)
        {
            var libs = new List<string>();
            using (var uninstallKey = Registry.LocalMachine.OpenSubKey(masterKey, false))
            {
                foreach (var key in uninstallKey.GetSubKeyNames())
                {
                    using (var Software = uninstallKey.OpenSubKey(key, false))
                    {
                        var displayName = (string)Software.GetValue("DisplayName");
                        if (displayName?.ToLower() == "steam")
                        {
                            var installPath = (string)Software.GetValue("UninstallString");
                            if (!string.IsNullOrEmpty(installPath))
                            {
                                Debug.Print("Found steam at {0}", installPath);
                                installPath = Path.Combine(Path.GetDirectoryName(installPath), "steamapps");
                                if (Directory.Exists(installPath))
                                {
                                    //The main library itself is added too
                                    //because it's not contained in the configuration file.
                                    libs.Add(installPath);
                                    libs.AddRange(GetLibraryConfig(installPath));
                                }
                            }
                        }
                    }
                }
            }
            return libs;
        }

        private static string[] GetLibraryConfig(string masterLibrary)
        {
            //This matches: "<number>" "<anything>"
            //This captures: <anything>
            var R = new Regex("^\\s*\"path\"\\s+\"(.*)\"\\s*$");
            var libs = new List<string>();
            var masterFile = Path.Combine(masterLibrary, "libraryfolders.vdf");
            if (File.Exists(masterFile))
            {
                var lines = File.ReadAllText(masterFile).Split('\n');
                foreach (var line in lines)
                {
                    var M = R.Match(line);
                    if (M.Success)
                    {
                        //Note: The string is "escaped" using backslashes.
                        //For now, we just replace double backslashes, but if the escaping is ever a problem,
                        //We can probably load a JSON library and feed the escaped string through the decoder function.
                        libs.Add(Path.Combine(M.Groups[1].Value.Replace(@"\\", @"\"), "steamapps"));
                    }
                }
            }
            return libs.ToArray();
        }
    }
}
