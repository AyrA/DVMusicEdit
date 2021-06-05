using System;
using System.Collections.Generic;
using System.IO;

namespace DVMusicEdit
{
    public class PlaylistEntry
    {
        public string FileName { get; set; }
        public int Duration { get; set; }
        public string Title { get; set; }

        public bool IsStream
        {
            get
            {
                return
                    FileName.ToLower().StartsWith("http://") ||
                    FileName.ToLower().StartsWith("https://");
            }
        }
        public bool IsValidTime
        {
            get
            {
                return IsStream || Duration > 0;
            }
        }

        public override string ToString()
        {
            return $"Playlist Entry: {FileName}";
        }

        /// <summary>
        /// Checks if the mentioned file exists relative to the playlist file
        /// </summary>
        /// <param name="PlaylistFileDir">Full path and name of the playlist file</param>
        /// <returns>true, if file exists</returns>
        public bool Exists(string PlaylistFileDir)
        {
            if (string.IsNullOrEmpty(PlaylistFileDir))
            {
                throw new ArgumentNullException(nameof(PlaylistFileDir));
            }
            if (string.IsNullOrEmpty(FileName))
            {
                throw new InvalidOperationException("FileName property must be specified");
            }
            return File.Exists(GetFullPath(PlaylistFileDir));
        }

        /// <summary>
        /// Gets the absolute path of the <see cref="FileName"/>
        /// </summary>
        /// <param name="PlaylistFileDirectory">
        /// Full path and name of the playlist directory (without the playlist file name)
        /// </param>
        /// <returns>Absolute path</returns>
        /// <remarks>
        /// This will not check if the file actually exists.
        /// Use <see cref="Exists(string)"/> instead.
        /// </remarks>
        public string GetFullPath(string PlaylistFileDirectory)
        {
            if (string.IsNullOrEmpty(PlaylistFileDirectory))
            {
                throw new ArgumentNullException(nameof(PlaylistFileDirectory));
            }
            if (string.IsNullOrEmpty(FileName))
            {
                throw new InvalidOperationException("FileName property must be specified");
            }
            return Path.Combine(PlaylistFileDirectory, FileName);
        }

        /// <summary>
        /// Serializes this instance into a valid PLS entry
        /// </summary>
        /// <param name="PlaylistIndex">Index in the playlist. This starts at 1.</param>
        /// <returns>PLS entry</returns>
        public string Serialize(int PlaylistIndex)
        {
            if (string.IsNullOrEmpty(FileName))
            {
                throw new InvalidOperationException("FileName property must be specified");
            }
            if (PlaylistIndex < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(PlaylistIndex), "Index must be 1 or bigger");
            }
            var segments = new List<string>();
            segments.Add($"File{PlaylistIndex}={FileName}");
            if (Duration > 0)
            {
                segments.Add($"Length{PlaylistIndex}={Duration}");
            }
            if (!string.IsNullOrEmpty(Title))
            {
                segments.Add($"Title{PlaylistIndex}={Title}");
            }
            else
            {
                segments.Add($"Title{PlaylistIndex}={Path.GetFileNameWithoutExtension(FileName)}");
            }
            return string.Join("\n", segments);
        }
    }
}
