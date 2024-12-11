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
            //Try to find DV via registry first
            var DV = DerailValleyFinder.FindDerailValley();
            //If not found, scan all steam libraries
            if (string.IsNullOrEmpty(DV))
            {
                DV = DerailValleyFinder.Libraries
                    .Select(m => Path.Combine(m, "common", DerailValleyFinder.GameName))
                    .FirstOrDefault(m => Directory.Exists(m));
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (DV == null)
            {
                Tools.Warn("Could not locate Derail Valley in your steam library. Please select the \"Derail Valley\" folder manually", "Derail Valley not found");
            }
            Application.Run(new frmMain(DV));
        }
    }
}
