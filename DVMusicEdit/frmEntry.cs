using System;
using System.IO;
using System.Windows.Forms;

namespace DVMusicEdit
{
    public partial class frmEntry : Form
    {
        public PlaylistEntry EditItem { get; private set; }
        public frmEntry(PlaylistEntry Entry)
        {
            InitializeComponent();
            nudDuration.Maximum = int.MaxValue;
            EditItem = Entry;
            if (File.Exists(Entry.FileName))
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
        }

        private void btnDetect_Click(object sender, EventArgs e)
        {
            if (EditItem.IsStream)
            {
                Tools.Info("Network streams have no length.", Text);
            }
            else
            {
                try
                {
                    nudDuration.Value = (int)Math.Round(FFmpeg.GetDuration(EditItem.FileName).TotalSeconds);
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
    }
}
