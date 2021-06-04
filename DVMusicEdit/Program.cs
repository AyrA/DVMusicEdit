using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DVMusicEdit
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var DV = Steam.Libraries
                .Select(m => Path.Combine(m, "common", "Derail Valley"))
                .FirstOrDefault(m => Directory.Exists(m));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (DV == null)
            {
                MessageBox.Show("Could not locate Derail Valley in your steam library. Please select the install folder manually");
            }
            Application.Run(new frmMain(DV));
        }
    }
}
