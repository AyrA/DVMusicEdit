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

        private static readonly string[] AcceptedExtensions = { ".ogg", ".mp3" };
        /// <summary>
        /// Directory that holds the playlist files
        /// </summary>
        public readonly string MusicRootPath;
        /// <summary>
        /// Directory that holds converted files
        /// </summary>
        public readonly string ConvertPath;

        public Playlist[] Playlists { get; private set; }

        public DerailValley(string MainFolder)
        {
            MusicRootPath = Path.Combine(MainFolder, "DerailValley_Data", "StreamingAssets", "music");
            ConvertPath = Path.Combine(MusicRootPath, "converted");
            if (!Directory.Exists(MusicRootPath))
            {
                throw new DirectoryNotFoundException($"Not found: {MusicRootPath}");
            }
            if (!Directory.Exists(ConvertPath))
            {
                Directory.CreateDirectory(ConvertPath);
            }
        }

        /// <summary>
        /// Gets a file name that is free for use as conversion target
        /// </summary>
        /// <param name="SourceFilename">Original file name (path not necessary)</param>
        /// <returns>Full file name for conversion target</returns>
        public string GetConvertedFilename(string SourceFilename)
        {
            var Basename = Path.GetFileNameWithoutExtension(SourceFilename) + ".ogg";
            var FullPath = Path.Combine(ConvertPath, Basename);
            int Count = 1;
            while (File.Exists(FullPath))
            {
                FullPath = Path.Combine(ConvertPath, Path.GetFileNameWithoutExtension(Basename) + $"_{Count++}.ogg");
            }
            return FullPath;
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

        private string GetPlaylistFile(int Index)
        {
            if (Index == 0)
            {
                return Path.Combine(MusicRootPath, "Radio.pls");
            }
            return Path.Combine(MusicRootPath, string.Format("Playlist_{0:00}.pls", Index));
        }

        /// <summary>
        /// Loads the specified playlist from disk
        /// </summary>
        /// <param name="Index">Playlist index.</param>
        private void SetList(int Index)
        {
            var ListFile = GetPlaylistFile(Index);
            var AltListFile = Path.ChangeExtension(ListFile, ".m3u");
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
                    Playlists[Index] = new Playlist();
                }
            }
            else
            {
                Playlists[Index] = new Playlist();
            }
            Playlists[Index].CreateRelativePaths(ListFile);
        }

        /// <summary>
        /// Loads the radio stream list from disk
        /// </summary>
        public void ReloadRadioList()
        {
            var RadioList = GetPlaylistFile(0);

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

        /// <summary>
        /// Saves all lists to disk
        /// </summary>
        /// <returns>
        /// true, if all lists saved and extra lists deleted.
        /// False if at least one entry failed to save or delete.
        /// </returns>
        public bool SaveLists()
        {
            var OK = true;
            for (var i = 0; i < Playlists.Length; i++)
            {
                var ListFile = GetPlaylistFile(i);
                var Current = Playlists[i];
                if (Current != null && Current.Entries.Length > 0)
                {
                    try
                    {
                        File.WriteAllText(ListFile, Current.Serialize());
                    }
                    catch
                    {
                        OK = false;
                    }
                }
                else if (File.Exists(ListFile))
                {
                    try
                    {
                        File.Delete(ListFile);
                    }
                    catch
                    {
                        OK = false;
                    }
                }
            }
            return OK;
        }

        /// <summary>
        /// Checks if the given file name is a valid media file
        /// </summary>
        /// <param name="Filename">File name</param>
        /// <returns>true, if valid</returns>
        /// <remarks>This only looks at the file name extension and not the content</remarks>
        public static bool IsAcceptedMediaType(string Filename)
        {
            if (string.IsNullOrEmpty(Filename))
            {
                throw new ArgumentNullException(nameof(Filename));
            }
            return AcceptedExtensions.Contains(Path.GetExtension(Filename.ToLower()));
        }
    }
}
