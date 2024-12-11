using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVMusicEdit
{
    public partial class frmDownload : Form
    {
        private readonly CancellationTokenSource cts;
        private readonly Task downloadTask;

        public frmDownload(Uri url)
        {
            InitializeComponent();
            cts = new CancellationTokenSource();
            downloadTask = ChainLoader.LoadAsync(
                url,
                Path.GetDirectoryName(Application.ExecutablePath),
                StatusReport, cts.Token);
            downloadTask.ContinueWith(task =>
            {
                Invoke((MethodInvoker)delegate
                {
                    DialogResult = task.IsFaulted ? DialogResult.Cancel : DialogResult.OK;
                    Close();
                });
            });
        }

        private void StatusReport(string relativeName, string absoluteName, long loaded, long totalSize)
        {
            Text = $"Downloading '{relativeName}'";
            if (totalSize == 0)
            {
                SetProgress(0);
            }
            else
            {
                SetProgress((int)(loaded * 100 / totalSize));
            }
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

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmDownload_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cts.IsCancellationRequested || downloadTask.IsCompleted)
            {
                return;
            }
            else if (Tools.AskWarn("Abort the current download?", "Download in progress"))
            {
                cts.Cancel();
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
