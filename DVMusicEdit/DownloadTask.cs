using System;
using System.IO;

namespace DVMusicEdit
{
    public class DownloadTask
    {
        public Uri URL { get; private set; }
        public string Filename { get; private set; }

        public bool DownloadRequired
        {
            get
            {
                return !File.Exists(Filename);
            }
        }

        public DownloadTask(string URL,string Filename)
        {
            this.Filename = Filename;
            this.URL = new Uri(URL);
        }
    }
}
