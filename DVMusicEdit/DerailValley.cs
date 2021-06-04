using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVMusicEdit
{
    public class DerailValley
    {
        private readonly string MusicRootPath;

        public DerailValley(string MainFolder)
        {
            MusicRootPath = Path.Combine(MainFolder, "DerailValley_Data", "StreamingAssets", "music");
            if (!Directory.Exists(MusicRootPath))
            {
                throw new DirectoryNotFoundException($"Not found: {MusicRootPath}");
            }
        }

        public Playlist[] GetPlaylists()
        {
            var Lists = new Playlist[11];

            var RadioList = Path.Combine(MusicRootPath, "Radio.pls");

            if (File.Exists(RadioList))
            {
                Lists[0] = Playlist.FromString(File.ReadAllText(RadioList));
            }

            for (var i = 1; i <= 10; i++)
            {
                var ListFile = Path.Combine(MusicRootPath, string.Format("Playlist_{0:00}.pls", i));
                var AltListFile = Path.Combine(MusicRootPath, string.Format("Playlist_{0:00}.m3u", i));
                if (File.Exists(ListFile))
                {
                    Lists[i] = Playlist.FromString(File.ReadAllText(ListFile));
                }
                else if (File.Exists(AltListFile))
                {
                    Lists[i] = Playlist.FromFileList(File.ReadAllLines(AltListFile).Where(m => !m.StartsWith("#")).ToArray());
                    try
                    {
                        File.WriteAllText(ListFile, Lists[i].Serialize());
                        File.Delete(AltListFile);
                    }
                    catch
                    {
                        Lists[i] = null;
                    }
                }
            }
            return Lists;
        }
    }
}
