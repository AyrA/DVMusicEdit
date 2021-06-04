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

        public Playlist[] Playlists { get; private set; }

        public DerailValley(string MainFolder)
        {
            MusicRootPath = Path.Combine(MainFolder, "DerailValley_Data", "StreamingAssets", "music");
            if (!Directory.Exists(MusicRootPath))
            {
                throw new DirectoryNotFoundException($"Not found: {MusicRootPath}");
            }
        }

        public void ReloadPlaylists()
        {
            Playlists = new Playlist[11];

            var RadioList = Path.Combine(MusicRootPath, "Radio.pls");

            if (File.Exists(RadioList))
            {
                Playlists[0] = Playlist.FromString(File.ReadAllText(RadioList));
            }

            for (var i = 1; i <= 10; i++)
            {
                var ListFile = Path.Combine(MusicRootPath, string.Format("Playlist_{0:00}.pls", i));
                var AltListFile = Path.Combine(MusicRootPath, string.Format("Playlist_{0:00}.m3u", i));
                if (File.Exists(ListFile))
                {
                    Playlists[i] = Playlist.FromString(File.ReadAllText(ListFile));
                }
                else if (File.Exists(AltListFile))
                {
                    Playlists[i] = Playlist.FromFileList(File.ReadAllLines(AltListFile).Where(m => !m.StartsWith("#")).ToArray());
                    try
                    {
                        File.WriteAllText(ListFile, Playlists[i].Serialize());
                        File.Delete(AltListFile);
                    }
                    catch
                    {
                        Playlists[i] = null;
                    }
                }
            }
        }
    }
}
