using System;
using System.Diagnostics;
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
            string DV = null;
            try
            {
                //Try to find DV via registry first
                DV = DerailValleyFinder.FindDerailValley();
            }
            catch
            {
                Debug.Print("Failed to find DV via registry key");
            }
            //If not found, scan all steam libraries
            if (string.IsNullOrEmpty(DV))
            {
                try
                {
                    DV = DerailValleyFinder.Libraries
                        .Select(m => Path.Combine(m, "common", DerailValleyFinder.GameName))
                        .FirstOrDefault(m => DerailValley.IsDVDirectory(m));
                }
                catch
                {
                    Debug.Print("Failed to find DV via steam");
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (DV == null)
            {
                Tools.Warn(@"Could not locate Derail Valley in your registry or steam library, or the information about its location in the registry is corrupt.
Please select the ""Derail Valley"" folder manually", "Derail Valley not found");
            }
            Application.Run(new frmMain(DV));
        }
    }
}
