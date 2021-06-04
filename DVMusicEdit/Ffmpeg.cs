using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DVMusicEdit
{
    public static class FFmpeg
    {
        public static string ConverterPath
        {
            get
            {
                return Path.Combine(Tools.ApplicationPath, "ffmpeg.exe");
            }
        }
        public static string PlayerPath
        {
            get
            {
                return Path.Combine(Tools.ApplicationPath, "ffplay.exe");
            }
        }
        public static string ProbePath
        {
            get
            {
                return Path.Combine(Tools.ApplicationPath, "ffprobe.exe");
            }
        }
        public static bool Ready
        {
            get
            {
                return File.Exists(ConverterPath) &&
                    File.Exists(PlayerPath) &&
                    File.Exists(ProbePath);
            }
        }

        public static string[] DownloadLinks
        {
            get
            {
                return new string[]
                {
                    "https://master.ayra.ch/LOGIN/pub/Applications/Tools/ffmpeg/ffmpeg.exe",
                    "https://master.ayra.ch/LOGIN/pub/Applications/Tools/ffmpeg/ffplay.exe",
                    "https://master.ayra.ch/LOGIN/pub/Applications/Tools/ffmpeg/ffprobe.exe"
                };
            }
        }

        public static Process PlayFileOrStream(string FileOrStream)
        {
            var Stream = FileOrStream.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            if (Stream.IndexOfAny("<>|".ToCharArray()) >= 0)
            {
                throw new SecurityException("Possible file name based shell attack. Please sanitize name");
            }
            return Process.Start(PlayerPath, $"\"{FileOrStream}\"");
        }
    }
}
