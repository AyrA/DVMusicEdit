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

        public static DownloadTask[] DownloadLinks
        {
            get
            {
                return new DownloadTask[]
                {
                    new DownloadTask("https://master.ayra.ch/LOGIN/pub/Applications/Tools/ffmpeg/ffmpeg.exe", ConverterPath),
                    new DownloadTask("https://master.ayra.ch/LOGIN/pub/Applications/Tools/ffmpeg/ffplay.exe", PlayerPath),
                    new DownloadTask("https://master.ayra.ch/LOGIN/pub/Applications/Tools/ffmpeg/ffprobe.exe", ProbePath)
                };
            }
        }

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

        public static Process PlayFileOrStream(string FileOrStream)
        {
            var Stream = FileOrStream.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            if (Stream.IndexOfAny("<>|".ToCharArray()) >= 0)
            {
                throw new SecurityException("Possible file name based shell attack. Please sanitize name");
            }
            return Process.Start(PlayerPath, $"-vn \"{FileOrStream}\"");
        }

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
