using System;
using System.Windows.Forms;

namespace DVMusicEdit
{
    public partial class frmMain : Form
    {
        private string DerailValleyPath;
        private DerailValley DV;

        public frmMain(string DVRoot)
        {
            InitializeComponent();
            Text += $" [{DVRoot}]";
            if (DVRoot != null)
            {
                SelectDV(DVRoot);
            }
        }

        private void SelectDV(string DVRoot)
        {
            DerailValleyPath = DVRoot;
            DV = new DerailValley(DVRoot);
            InitDV();
        }

        private void InitDV()
        {
            var Lists = DV.GetPlaylists();
            lbPlaylists.Items.Clear();
            lbPlaylists.Items.Add("Radio");
            for (var i = 1; i <= 10; i++)
            {
                var BaseStr = $"Tape #{i}";
                if (Lists[i] == null || Lists[i].Count == 0)
                {
                    BaseStr += " (empty)";
                }
                else
                {
                    BaseStr += $" ({Lists[i].Count} items)";
                }
                lbPlaylists.Items.Add(BaseStr);
            }
        }

        private void lbPlaylists_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbPlaylists.SelectedIndex >= 0)
            {
                var PL = DV.GetPlaylists()[lbPlaylists.SelectedIndex];
                lvPlaylist.Items.Clear();
                if (PL != null)
                {
                    foreach (var Entry in PL.Entries)
                    {
                        var Item = lvPlaylist.Items.Add(Entry.FileName);
                        if (Entry.Duration >= 0)
                        {
                            Item.SubItems.Add(TimeSpan.FromSeconds(Entry.Duration).ToString());
                        }
                        else
                        {
                            Item.SubItems.Add("<N/A>");
                        }
                        if (!string.IsNullOrEmpty(Entry.Title))
                        {
                            Item.SubItems.Add(Entry.Title);
                        }
                        else
                        {
                            Item.SubItems.Add("<N/A>");
                        }
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CmsAdd.Show(btnAdd, new System.Drawing.Point(0, btnAdd.Height));
        }
    }
}
