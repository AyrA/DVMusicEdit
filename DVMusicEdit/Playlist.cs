using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DVMusicEdit
{
    public class Playlist
    {
        private readonly List<PlaylistEntry> _entries;
        private readonly List<string> _problems = new List<string>();

        public bool HasProblems => _problems.Count > 0;

        public string[] Problems => _problems.ToArray();

        public PlaylistEntry[] Entries => _entries.ToArray();

        public int Count
        {
            get
            {
                return _entries == null ? 0 : _entries.Count;
            }
        }

        /// <summary>
        /// Creates an empty playlist
        /// </summary>
        public Playlist()
        {
            _entries = new List<PlaylistEntry>();
        }

        public static Playlist FromString(string rawPlaylist, string playlistDirectory, bool checkExists)
        {
            if (rawPlaylist == null)
            {
                throw new ArgumentNullException(nameof(rawPlaylist));
            }
            Playlist P = new Playlist();
            var lines = rawPlaylist.Trim().Split('\n');
            if (lines[0].Trim().ToLower() != "[playlist]")
            {
                throw new FormatException("This is not a pls style playlist");
            }
            var Parsed = new Dictionary<string, string>();
            foreach (var line in lines.Skip(1).Select(m => m.Trim()))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                var index = line.IndexOf("=");
                if (index > 0)
                {
                    Parsed[line.Substring(0, index).Trim().ToLower()] = line.Substring(index + 1).Trim();
                }
                else if (line.Length > 0 && !line.TrimStart().StartsWith("#"))
                {
                    P._problems.Add($"Skipping over invalid line in playlist: '{line}'");
                }
            }
            FillEntries(Parsed, P, playlistDirectory, checkExists);
            return P;
        }

        public static Playlist FromFileList(string[] mediaFiles)
        {
            var P = new Playlist();
            foreach (var s in mediaFiles)
            {
                P._entries.Add(new PlaylistEntry()
                {
                    FileName = s
                });
            }
            return P;
        }

        private static void FillEntries(Dictionary<string, string> parsed, Playlist P, string playlistDirectory, bool checkExists)
        {
            if (!parsed.TryGetValue("version", out string v))
            {
                P._problems.Add("Playlist lacks 'Version' field. Trying to read it under the assumption that the field is missing");
            }
            else if (v != "2")
            {
                P._problems.Add($"Incompatible playlist version. Expected '2', got '{v}'. Trying to read anyways as version 2");
            }

            var fileIndexes = parsed.Keys
                //Filter for keys named "File" followed  by digits
                .Where(m => Regex.IsMatch(m, @"^file\d+$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
                //Extract the digits
                .Select(m => int.TryParse(Regex.Match(m, @"\d+$").Value, out int i) ? i : -1)
                //Keep only entries where extraction was successful
                .Where(m => m > 0)
                //Order ascending
                .OrderBy(m => m)
                .ToArray();

            foreach (var i in fileIndexes)
            {
                parsed.TryGetValue($"file{i}", out string tempFilename);
                parsed.TryGetValue($"length{i}", out string tempLength);
                parsed.TryGetValue($"title{i}", out string tempTitle);
                if (string.IsNullOrEmpty(tempFilename))
                {
                    P._problems.Add($"Entry 'File{i}' is empty or missing from the playlist. Skipping over it");
                    continue;
                }
                if (checkExists)
                {
                    try
                    {
                        var fullPath = Path.Combine(playlistDirectory, tempFilename);
                        if (!File.Exists(fullPath))
                        {
                            throw new FileNotFoundException();
                        }
                    }
                    catch (FileNotFoundException)
                    {
                        P._problems.Add($"Entry 'File{i}': File does not exist");
                    }
                    catch (Exception ex)
                    {
                        P._problems.Add($"Entry 'File{i}': Unable to check if file exists. {ex.Message}");
                    }
                }
                if (!string.IsNullOrEmpty(tempLength))
                {
                    if (!double.TryParse(tempLength, out double length))
                    {
                        P._problems.Add($"Entry 'Length{i}' is present but not a number. Ignoring it");
                        length = 0;
                    }
                    if (length < 0)
                    {
                        P._problems.Add($"Entry 'Length{i}' is present but negative. Ignoring it");
                    }
                    if (length > 0)
                    {
                        P.AddItem(tempFilename, (int)length, tempTitle);
                    }
                    else //Zero is normal for network streams
                    {
                        P.AddItem(tempFilename, -1, tempTitle);
                    }
                }
                else
                {
                    P.AddItem(tempFilename, -1, tempTitle);
                }
            }
        }

        /// <summary>
        /// Creates relative file paths
        /// </summary>
        /// <param name="PlaylistFile">Playlist file name and path</param>
        public void CreateRelativePaths(string PlaylistFile)
        {
            for (var i = 0; i < _entries.Count; i++)
            {
                var N = _entries[i].FileName;
                //Don't bother if it's already relative
                if (Path.IsPathRooted(N))
                {
                    _entries[i].FileName = Tools.MakeRelative(N, PlaylistFile);
                }
            }
        }

        /// <summary>
        /// Adds a playlist item
        /// </summary>
        /// <param name="File">(required) File name. Either absolute, or relative to the list file</param>
        /// <param name="Length">(optional) Track length in seconds</param>
        /// <param name="Title">(optional) Track title</param>
        /// <remarks>Thread safe</remarks>
        public void AddItem(string File, int Length = -1, string Title = null)
        {
            lock (_entries)
            {
                _entries.Add(new PlaylistEntry()
                {
                    Duration = Length,
                    FileName = File,
                    Title = Title
                });
            }
        }

        /// <summary>
        /// Removes the item at the given position from the list
        /// </summary>
        /// <param name="Index">List index</param>
        /// <remarks>Thread safe</remarks>
        public PlaylistEntry RemoveItem(int Index)
        {
            lock (_entries)
            {
                if (_entries.Count == 0)
                {
                    throw new InvalidOperationException("The list is already empty");
                }
                if (Index < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(Index), "Index must be at least 0");
                }
                if (Index >= _entries.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(Index), $"Index must be at most {_entries.Count}");
                }
                var E = _entries[Index];
                _entries.RemoveAt(Index);
                return E;
            }
        }

        /// <summary>
        /// Clears all entries from the playlist
        /// </summary>
        /// <remarks>
        /// Thread safe.
        /// Will not throw if the list is empty
        /// </remarks>
        public void ClearList()
        {
            lock (_entries)
            {
                _entries.Clear();
            }
        }

        /// <summary>
        /// Moves the item at the given index up by one position
        /// </summary>
        /// <param name="Index">Current index of item</param>
        public void MoveUp(int Index)
        {
            if (Index < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(Index), "Invalid index. Must be at least 1");
            }
            lock (_entries)
            {
                _entries.Insert(Index - 1, RemoveItem(Index));
            }
        }

        /// <summary>
        /// Moves the item at the given index down by one position
        /// </summary>
        /// <param name="Index">Current index of item</param>
        /// <remarks>This simulates the effect by moving the item below one position up</remarks>
        public void MoveDown(int Index)
        {
            MoveUp(Index + 1);
        }

        /// <summary>
        /// Serializes this instance into a complete playlist file content string
        /// </summary>
        /// <returns>Playlist file content</returns>
        public string Serialize()
        {
            if (Entries == null)
            {
                throw new InvalidOperationException("Playlist.Entries has not been set");
            }
            if (Entries.Length == 0)
            {
                throw new InvalidOperationException("Playlist.Entries has no elements");
            }
            return string.Format("[playlist]\n{0}\nNumberOfEntries={1}\nVersion=2",
                string.Join("\n", Entries.Select((m, i) => m.Serialize(i + 1))),
                Entries.Length
            );
        }
    }
}
