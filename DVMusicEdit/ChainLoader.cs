using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DVMusicEdit
{
    internal static class ChainLoader
    {
        public delegate void DownloadProgressHandler(string relativeName, string absoluteName, long loaded, long totalSize);

        private static readonly HttpClient _httpClient = new HttpClient(new HttpClientHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        });

        public static async Task LoadAsync(Uri url, string directory, DownloadProgressHandler progressCallback, CancellationToken ct)
        {
            var unixTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            directory = Path.GetFullPath(directory);
            var result = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, ct);
            result.EnsureSuccessStatusCode();
            using (var response = await result.Content.ReadAsStreamAsync())
            {
                using (var br = new BinaryReader(response, Encoding.UTF8))
                {
                    if (Encoding.ASCII.GetString(br.ReadBytes(5)) != "CHAIN")
                    {
                        throw new InvalidDataException("Response is not a chain file");
                    }
                    if (IPAddress.NetworkToHostOrder(br.ReadInt16()) != 1)
                    {
                        throw new InvalidDataException("Version in chain file is not supported");
                    }
                    ushort nameLength;
                    while (!ct.IsCancellationRequested)
                    {
                        nameLength = (ushort)IPAddress.NetworkToHostOrder(br.ReadInt16());
                        if (nameLength == 0)
                        {
                            break;
                        }
                        var relativeName = Encoding.UTF8.GetString(br.ReadBytes(nameLength));
                        var fullName = Path.GetFullPath(Path.Combine(directory, relativeName.TrimEnd('/').Replace('/', Path.DirectorySeparatorChar)));
                        if (!fullName.StartsWith(directory + Path.DirectorySeparatorChar))
                        {
                            throw new InvalidDataException($"Path '{relativeName}' points outside of '{directory}'");
                        }
                        if (relativeName.EndsWith("/"))
                        {
                            Directory.CreateDirectory(fullName);
                            //Discard lmod
                            br.ReadBytes(4);
                        }
                        else
                        {
                            var lmod = unixTime.AddSeconds((uint)IPAddress.NetworkToHostOrder(br.ReadInt32()));
                            var size = (long)(uint)IPAddress.NetworkToHostOrder(br.ReadInt32());
                            using (var fs = File.Create(fullName))
                            {
                                fs.SetLength(size);
                                while (fs.Position < fs.Length && !ct.IsCancellationRequested)
                                {
                                    progressCallback?.Invoke(relativeName, fullName, fs.Position, fs.Length);
                                    var diff = (int)Math.Min(1024 * 1024, fs.Length - fs.Position);
                                    var data = br.ReadBytes(diff);
                                    fs.Write(data, 0, data.Length);
                                }
                                progressCallback?.Invoke(relativeName, fullName, size, size);
                            }
                            File.SetLastWriteTimeUtc(fullName, lmod);
                        }
                    }
                }
            }
        }
    }
}
