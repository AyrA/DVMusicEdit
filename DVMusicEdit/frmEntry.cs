using System;
using System.IO;
using System.Windows.Forms;

namespace DVMusicEdit
{
    public partial class frmEntry : Form
    {
        public PlaylistEntry EditItem { get; private set; }
        private string PlaylistFile;

        public frmEntry(PlaylistEntry Entry, string PlaylistDir)
        {
            InitializeComponent();
            PlaylistFile = PlaylistDir;
            nudDuration.Maximum = int.MaxValue;
            EditItem = Entry;
            if (File.Exists(Entry.GetFullPath(PlaylistDir)))
            {
                nudDuration.Value = Math.Max(0, Entry.Duration);
            }
            else if (Entry.IsStream)
            {
                nudDuration.Enabled = false;
            }
            else
            {
                throw new ArgumentException("The supplied argument is neither a local file nor an http(s) stream", nameof(EditItem.FileName));
            }
            tbTitle.Text = Entry.Title;
            Text += $" [{Entry.FileName}]";
        }

        private void NoFF()
        {
            Tools.Error("FFmpeg has not been downloaded yet. Use the \"More\" button in the main window to do so.", Text);
        }

        private void btnDetect_Click(object sender, EventArgs e)
        {
            if (EditItem.IsStream)
            {
                Tools.Info("Network streams have no length. This is normal", Text);
            }
            else if(!FFmpeg.IsReady)
            {
                NoFF();
            }
            else
            {
                try
                {
                    nudDuration.Value = (int)Math.Round(FFmpeg.GetDuration(EditItem.GetFullPath(PlaylistFile)).TotalSeconds);
                }
                catch (Exception ex)
                {
                    Tools.Error($"Length detection failed. Error: {ex.Message}", "Length detection failed");
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            EditItem.Duration = (int)nudDuration.Value;
            EditItem.Title = tbTitle.Text;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if(!FFmpeg.IsReady)
            {
                NoFF();
            }
            else
            {
                FFmpeg.PlayFileOrStream(EditItem.GetFullPath(PlaylistFile)).WaitForExit();
            }
        }
    }
}
