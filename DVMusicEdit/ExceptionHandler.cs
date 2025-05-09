using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace DVMusicEdit
{
    internal class ExceptionHandler
    {
        private const string ErrUrl = "https://github.com/AyrA/DVMusicEdit/issues/";

        public static void ThreadExceptionHandler(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            bool openGH;
            if (e == null || e.Exception == null)
            {
                openGH = Msg("No error information was provided by the .NET runtime.");
            }
            else
            {
                var crashLog = string.Format("Type: {0}\r\nError: {1}\r\nLocation: {2}",
                    e.Exception.GetType().Name,
                    e.Exception.Message,
                    e.Exception.StackTrace);
                var log = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "crashlog.txt");
                try
                {
                    File.WriteAllText(log, crashLog);
                    crashLog += "\r\n\r\nThis information was also written to " + log;
                }
                catch
                {
                    crashLog += "\r\n\r\nWe were unable to write this error to a crash log file, but you can press CTRL+C now to copy the text in this dialog to your clipboard";
                    //NOOP
                }
                openGH = Msg(crashLog);
            }
            if (openGH)
            {
                try
                {
                    var psi = new ProcessStartInfo()
                    {
                        UseShellExecute = true,
                        FileName = ErrUrl
                    };
                    Process.Start(psi).Dispose();
                }
                catch
                {
                    try
                    {
                        Clipboard.Clear();
                        Clipboard.SetText(ErrUrl);
                        MessageBox.Show($"Unable to start your web browser. Please go manually to {ErrUrl}. The url was also copied to your clipboard.");
                    }
                    catch
                    {
                        MessageBox.Show($"Unable to start your web browser. Please go manually to {ErrUrl}");
                    }
                }
            }
        }

        private static bool Msg(string message)
        {
            var body = $"A problem prevents this application from continuing.\r\nDetails: {message}\r\n\r\nOpen github to report the crash?";
            return MessageBox.Show(body, "APPCRASH", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes;
        }
    }
}
