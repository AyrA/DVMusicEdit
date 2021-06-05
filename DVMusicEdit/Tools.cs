using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVMusicEdit
{
    public static class Tools
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        private static extern bool PathRelativePathTo(
            [Out] StringBuilder pszPath,
            [In] string pszFrom,
            [In] FileAttributes dwAttrFrom,
            [In] string pszTo,
            [In] FileAttributes dwAttrTo
        );
        const int MAX_PATH = 260;

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

        /// <summary>
        /// Makes a path relative
        /// </summary>
        /// <param name="PathToObject"></param>
        /// <param name="BasePath"></param>
        /// <returns></returns>
        public static string MakeRelative(string PathToObject, string BasePath)
        {
            PathToObject = Path.GetFullPath(PathToObject);
            BasePath = Path.GetFullPath(BasePath);

            if (Path.GetPathRoot(PathToObject) != Path.GetPathRoot(BasePath))
            {
                return PathToObject;
            }

            var AttrObject = File.GetAttributes(PathToObject);
            var AttrBase = File.GetAttributes(BasePath);

            //Base path is always a directory
            if (!AttrBase.HasFlag(FileAttributes.Directory))
            {
                BasePath = Path.GetDirectoryName(BasePath);
            }
            //Trim attributes to dir or file only
            AttrObject &= FileAttributes.Directory | FileAttributes.Normal;

            var Relative = new StringBuilder(MAX_PATH);
            if(!PathRelativePathTo(Relative,BasePath, FileAttributes.Directory,PathToObject, AttrObject))
            {
                //Return original path if problems arise
                return PathToObject;
            }
            var Parsed = Relative.ToString();
            //Remove leading ".\" if it's there
            if (Parsed.StartsWith($".{Path.DirectorySeparatorChar}"))
            {
                Parsed = Parsed.Substring(2);
            }
            return Parsed;
        }
    }
}
