using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVMusicEdit
{
    public static class Tools
    {
        public static string ApplicationPath
        {
            get
            {
                //If you ever need this without a Forms reference,
                //Open your own process via the Process class
                //and get the full file path from the StartInfo structure
                return Application.StartupPath;
            }
        }

        public static void Info(string Msg, string Title)
        {
            MessageBox.Show(Msg, Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Warn(string Msg, string Title)
        {
            MessageBox.Show(Msg, Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static bool AskWarn(string Msg, string Title)
        {
            return MessageBox.Show(Msg, Title, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes;
        }

        public static bool AskInfo(string Msg, string Title)
        {
            return MessageBox.Show(Msg, Title, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes;
        }

        public static bool AskError(string Msg, string Title)
        {
            return MessageBox.Show(Msg, Title, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes;
        }

        public static void Error(string Msg, string Title)
        {
            MessageBox.Show(Msg, Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
