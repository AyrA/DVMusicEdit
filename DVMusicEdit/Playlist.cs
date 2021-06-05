using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVMusicEdit
{
    public class Playlist
    {
        private List<PlaylistEntry> _entries;

        public PlaylistEntry[] Entries { get => _entries.ToArray(); }

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

        public static Playlist FromString(string RawPlaylist)
        {
            if (RawPlaylist == null)
            {
                throw new ArgumentNullException(nameof(RawPlaylist));
            }
            Playlist P = new Playlist();
            var Lines = RawPlaylist.Trim().Split('\n');
            if (Lines[0].Trim().ToLower() != "[playlist]")
            {
                throw new FormatException("This is not a pls style playlist");
            }
            var Parsed = new Dictionary<string, string>();
            foreach (var L in Lines.Skip(1).Select(m => m.Trim()))
            {
                var I = L.IndexOf("=");
                if (I >= 0)
                {
                    Parsed[L.Substring(0, I).Trim().ToLower()] = L.Substring(I + 1).Trim();
                }
                else if (L.Length > 0 && L[0] != '#')
                {
                    throw new FormatException($"Invalid line in playlist: {L}");
                }
            }
            FillEntries(Parsed, P);
            return P;
        }

        public static Playlist FromFileList(string[] MediaFiles)
        {
            var P = new Playlist();
            foreach (var s in MediaFiles)
            {
                P._entries.Add(new PlaylistEntry()
                {
                    FileName = s
                });
            }
            return P;
        }

        private static void FillEntries(Dictionary<string, string> Parsed, Playlist P)
        {
            if (!Parsed.TryGetValue("version", out string v))
            {
                throw new FormatException("Playlist lacks version information");
            }
            else if (v != "2")
            {
                throw new FormatException($"Incompatible playlist version. Expected 2, got {v}");
            }
            string countTemp;
            int count;
            if (!Parsed.TryGetValue("numberofentries", out countTemp))
            {
                count = -1;
                //Guess the count
                for (var i = 1; count < 0; i++)
                {
                    if (!Parsed.ContainsKey($"file{i}"))
                    {
                        count = i - 1;
                    }
                }
                //If zero, then a "File1" was not found.
                if (count == 0)
                {
                    //Playlist is empty, don't bother reading further
                    return;
                }
            }
            else if (!int.TryParse(countTemp, out count) || count < 1)
            {
                throw new Exception("NumberOfEntries is not a valid number");
            }
            for (var i = 1; i <= count; i++)
            {
                Parsed.TryGetValue($"file{i}", out string tempFilename);
                Parsed.TryGetValue($"length{i}", out string tempLength);
                Parsed.TryGetValue($"title{i}", out string tempTitle);
                if (string.IsNullOrEmpty(tempFilename))
                {
                    throw new FormatException($"File{i} is empty or missing from the playlist");
                }
                if (!string.IsNullOrEmpty(tempLength))
                {
                    if (!double.TryParse(tempLength, out double length))
                    {
                        throw new FormatException($"Length{i} is present but not a number");
                    }
                    if (length < 0)
                    {
                        throw new FormatException($"Length{i} is present but negative");
                    }
                    else if (length > 0)
                    {
                        P.AddItem(tempFilename, (int)length, tempTitle);
                    }
                    else
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
