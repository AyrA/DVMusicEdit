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

        public override string ToString()
        {
            return $"Playlist Entry: {FileName}";
        }

        /// <summary>
        /// Checks if the mentioned file exists relative to the playlist file
        /// </summary>
        /// <param name="PlaylistFilePath">Full path and name of the playlist file</param>
        /// <returns>true, if file exists</returns>
        public bool Exists(string PlaylistFilePath)
        {
            if (string.IsNullOrEmpty(PlaylistFilePath))
            {
                throw new ArgumentNullException(nameof(PlaylistFilePath));
            }
            if (string.IsNullOrEmpty(FileName))
            {
                throw new InvalidOperationException("FileName property must be specified");
            }
            return File.Exists(GetFullPath(PlaylistFilePath));
        }

        /// <summary>
        /// Gets the absolute path of the <see cref="FileName"/>
        /// </summary>
        /// <param name="PlaylistFilePath">Full path and name of the playlist file</param>
        /// <returns>Absolute path</returns>
        /// <remarks>
        /// This will not check if the file actually exists.
        /// Use <see cref="Exists(string)"/> instead.
        /// </remarks>
        public string GetFullPath(string PlaylistFilePath)
        {
            if (string.IsNullOrEmpty(PlaylistFilePath))
            {
                throw new ArgumentNullException(nameof(PlaylistFilePath));
            }
            if (string.IsNullOrEmpty(FileName))
            {
                throw new InvalidOperationException("FileName property must be specified");
            }
            return Path.Combine(Path.GetDirectoryName(PlaylistFilePath), FileName);
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
