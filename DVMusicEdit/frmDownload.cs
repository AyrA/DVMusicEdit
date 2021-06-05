using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVMusicEdit
{
    public partial class frmDownload : Form
    {
        private readonly WebClient WC;
        private readonly string Filename;

        public frmDownload(Uri URL, string Destination)
        {
            InitializeComponent();
            Filename = Destination;
            Text += $" [{Path.GetFileName(Destination)}]";
            WC = new WebClient();
            WC.DownloadProgressChanged += WC_DownloadProgressChanged;
            WC.DownloadFileCompleted += WC_DownloadFileCompleted;
            WC.DownloadFileAsync(URL, Destination);
        }

        private void WC_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            EndDownload(!e.Cancelled && e.Error == null);
        }

        private void EndDownload(bool success)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { EndDownload(success); });
            }
            DialogResult = success ? DialogResult.OK : DialogResult.Cancel;
            Close();
        }

        private void SetProgress(int Percentage)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { SetProgress(Percentage); });
            }
            else if (Percentage != pbStatus.Value)
            {
                if (pbStatus.Style != ProgressBarStyle.Continuous)
                {
                    pbStatus.Style = ProgressBarStyle.Continuous;
                }
                pbStatus.Value = Math.Min(100, Math.Max(0, Percentage));
            }
        }

        private void WC_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (e.TotalBytesToReceive > 0)
            {
                SetProgress(e.ProgressPercentage);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmDownload_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WC.IsBusy && !Tools.AskWarn("Abort the current download?", "Download in progress"))
            {
                try
                {
                    WC.CancelAsync();
                }
                catch
                {
                    //Don't care
                }
                while(File.Exists(Filename))
                {
                    try
                    {
                        File.Delete(Filename);
                    }
                    catch
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                }
                //If the download has completed, don't cancel the close event
                e.Cancel = WC.IsBusy;
            }
        }
    }
}
