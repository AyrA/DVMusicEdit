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
        private const int LIST_SIZE = 11;
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

        /// <summary>
        /// Loads the specified playlist from disk
        /// </summary>
        /// <param name="Index">
        /// Playlist index.
        /// To load index 0, use <see cref="ReloadRadioList"/> instead
        /// </param>
        /// <remarks>
        /// If the lists have not yet been loaded,
        /// <see cref="ReloadPlaylists"/> is called internally instead
        /// </remarks>
        public void ReloadPlaylist(int ListIndex)
        {
            if (ListIndex < 1 || ListIndex >= LIST_SIZE)
            {
                throw new ArgumentOutOfRangeException(nameof(ListIndex));
            }
            if (Playlists == null)
            {
                ReloadPlaylists();
            }
            else
            {
                SetList(ListIndex);
            }
        }

        /// <summary>
        /// Loads the specified playlist from disk
        /// </summary>
        /// <param name="Index">Playlist index.</param>
        private void SetList(int Index)
        {
            var ListFile = Path.Combine(MusicRootPath, string.Format("Playlist_{0:00}.pls", Index));
            var AltListFile = Path.Combine(MusicRootPath, string.Format("Playlist_{0:00}.m3u", Index));
            if (File.Exists(ListFile))
            {
                Playlists[Index] = Playlist.FromString(File.ReadAllText(ListFile));
            }
            else if (File.Exists(AltListFile))
            {
                Playlists[Index] = Playlist.FromFileList(File.ReadAllLines(AltListFile).Where(m => !m.StartsWith("#")).ToArray());
                try
                {
                    File.WriteAllText(ListFile, Playlists[Index].Serialize());
                    File.Delete(AltListFile);
                }
                catch
                {
                    Playlists[Index] = null;
                }
            }
            else
            {
                Playlists[Index] = null;
            }
        }

        /// <summary>
        /// Loads the radio stream list from disk
        /// </summary>
        public void ReloadRadioList()
        {
            var RadioList = Path.Combine(MusicRootPath, "Radio.pls");

            if (File.Exists(RadioList))
            {
                Playlists[0] = Playlist.FromString(File.ReadAllText(RadioList));
            }
        }

        /// <summary>
        /// Reloads all playlists from disk
        /// </summary>
        public void ReloadPlaylists()
        {
            Playlists = new Playlist[LIST_SIZE];

            ReloadRadioList();

            for (var i = 1; i <= LIST_SIZE - 1; i++)
            {
                SetList(i);
            }
        }
    }
}
