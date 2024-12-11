using System;
using System.Diagnostics;
using System.IO;
using System.Security;

namespace DVMusicEdit
{
    public static class FFmpeg
    {
        /// <summary>
        /// Base path of all ffmpeg executables and libraries
        /// </summary>
        public static string BasePath
        {
            get
            {
                return Path.Combine(Tools.ApplicationPath, "ffmpeg");
            }
        }

        /// <summary>
        /// Full path to the converter
        /// </summary>
        public static string ConverterPath
        {
            get
            {
                return Path.Combine(BasePath, "ffmpeg.exe");
            }
        }
        /// <summary>
        /// Full path to the player
        /// </summary>
        public static string PlayerPath
        {
            get
            {
                return Path.Combine(BasePath, "ffplay.exe");
            }
        }
        /// <summary>
        /// Full path to the data extracter
        /// </summary>
        public static string ProbePath
        {
            get
            {
                return Path.Combine(BasePath, "ffprobe.exe");
            }
        }
        /// <summary>
        /// Checks if
        /// <see cref="ConverterPath"/>,
        /// <see cref="PlayerPath"/>, and
        /// <see cref="ProbePath" /> are available
        /// </summary>
        public static bool IsReady
        {
            get
            {
                return File.Exists(ConverterPath) &&
                    File.Exists(PlayerPath) &&
                    File.Exists(ProbePath);
            }
        }

        /// <summary>
        /// Gets the duration of a file
        /// </summary>
        /// <param name="Filename">media file name</param>
        /// <returns>Duration of the media file</returns>
        public static TimeSpan GetDuration(string Filename)
        {
            if (!File.Exists(Filename))
            {
                throw new FileNotFoundException("Cannot find the file on your disk. Is this a stream?", Filename);
            }
            var P = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = ProbePath,
                    Arguments = $"-i \"{Filename}\" -show_entries format=duration -v quiet -of csv=\"p=0\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            using (P)
            {
                if (P.Start())
                {
                    var Data = P.StandardOutput.ReadToEnd().Trim();
                    P.WaitForExit();
                    if (double.TryParse(Data, out double d))
                    {
                        return TimeSpan.FromSeconds(d);
                    }
                    throw new Exception("Cannot determine the media file length. Not a valid media file?");
                }
            }
            throw new Exception($"Cannot start {P.StartInfo.FileName}");
        }

        /// <summary>
        /// Plays a file or stream
        /// </summary>
        /// <param name="FileOrStream">Media file or http stream</param>
        /// <returns>Player process</returns>
        public static Process PlayFileOrStream(string FileOrStream)
        {
            var Stream = FileOrStream.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            if (Stream.IndexOfAny("<>|".ToCharArray()) >= 0)
            {
                throw new SecurityException("Possible file name based shell attack. Please sanitize name");
            }
            return Process.Start(PlayerPath, $"-vn \"{FileOrStream}\"");
        }

        /// <summary>
        /// Converts a file into OGG format
        /// </summary>
        /// <param name="Source">Source media file</param>
        /// <param name="Destination">OGG destination file</param>
        /// <returns>Converter process</returns>
        public static Process ConvertToOgg(string Source, string Destination)
        {
            if (!File.Exists(Source))
            {
                throw new FileNotFoundException("Cannot find the file on your disk. Is this a stream?", Source);
            }
            var P = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = ConverterPath,
                    Arguments = $"-i \"{Source}\" -codec:a libvorbis -vn -qscale:a 5 -y \"{Destination}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            if (!P.Start())
            {
                P.Dispose();
                throw new Exception("Unable to start the converter");
            }
            return P;
        }
    }
}
