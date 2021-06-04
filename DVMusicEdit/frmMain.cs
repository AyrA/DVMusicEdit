using System;
using System.Linq;
using System.Windows.Forms;

namespace DVMusicEdit
{
    public partial class frmMain : Form
    {
        private enum PlaylistType
        {
            Radio,
            Tape
        }

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
            DV.ReloadPlaylists();
            lbPlaylists.Items.Clear();
            lbPlaylists.Items.Add("Radio");
            for (var i = 1; i <= 10; i++)
            {
                var BaseStr = $"Tape #{i}";
                if (DV.Playlists[i] == null || DV.Playlists[i].Count == 0)
                {
                    BaseStr += " (empty)";
                }
                else
                {
                    BaseStr += $" ({DV.Playlists[i].Count} items)";
                }
                lbPlaylists.Items.Add(BaseStr);
            }
            SetEditControls(false);
        }

        private void SetEditControls(bool enabled)
        {
            btnAdd.Enabled = btnDelete.Enabled =
                btnUp.Enabled = btnDown.Enabled =
                btnReset.Enabled = btnSave.Enabled = enabled;
        }

        private void lbPlaylists_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbPlaylists.SelectedIndex >= 0)
            {
                var PL = DV.Playlists[lbPlaylists.SelectedIndex];
                RenderList(PL);
                SetListFunction(lbPlaylists.SelectedIndex == 0 ? PlaylistType.Radio : PlaylistType.Tape);
                SetEditControls(true);
            }
            else
            {
                SetEditControls(false);
            }
        }

        private void SetListFunction(PlaylistType ListType)
        {
            switch (ListType)
            {
                case PlaylistType.Radio:
                    cmsAddLocal.Visible = cmsAddYoutube.Visible = false;
                    cmsAddStream.Visible = true;
                    break;
                default:
                    cmsAddLocal.Visible = cmsAddYoutube.Visible = true;
                    cmsAddStream.Visible = false;
                    break;
            }
        }

        private void RenderList(Playlist PL)
        {
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
            if (lvPlaylist.Items.Count > 0)
            {
                lvPlaylist.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CmsAdd.Show(btnAdd, new System.Drawing.Point(0, btnAdd.Height));
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var Indexes = lvPlaylist.SelectedIndices
                .OfType<int>()
                .OrderBy(m => m)
                .Reverse()
                .ToArray();
            var PL = DV.Playlists[lbPlaylists.SelectedIndex];
            foreach (var I in Indexes)
            {
                PL.RemoveItem(I);
            }
            RenderList(PL);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            var Indexes = lvPlaylist.SelectedIndices
                .OfType<int>()
                .OrderBy(m => m)
                .ToArray();
            var PL = DV.Playlists[lbPlaylists.SelectedIndex];
            if (Indexes.Contains(0))
            {
                Tools.Warn("Cannot move first item further up", "Cannot move first item");
            }
            else
            {
                foreach (var I in Indexes)
                {
                    PL.MoveUp(I);
                }
                RenderList(PL);
                //Keep the moved item selected
                foreach (var I in Indexes)
                {
                    lvPlaylist.Items[I - 1].Selected = true;
                }
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            var Indexes = lvPlaylist.SelectedIndices
                .OfType<int>()
                .OrderBy(m => m)
                .Reverse()
                .ToArray();
            if (Indexes.Contains(lvPlaylist.Items.Count - 1))
            {
                Tools.Warn("Cannot move last item further down", "Cannot move last item");
            }
            else
            {
                var PL = DV.Playlists[lbPlaylists.SelectedIndex];
                foreach (var I in Indexes)
                {
                    PL.MoveDown(I);
                }
                RenderList(PL);
                //Keep the moved item selected
                foreach (var I in Indexes)
                {
                    lvPlaylist.Items[I + 1].Selected = true;
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (lbPlaylists.SelectedIndex >= 0)
            {
                if (Tools.AskWarn("Reset this playlist to what is stored in the file?", "Reset playlist") == DialogResult.Yes)
                {
                    if (lbPlaylists.SelectedIndex > 0)
                    {
                        DV.ReloadPlaylist(lbPlaylists.SelectedIndex);
                    }
                    else
                    {
                        DV.ReloadRadioList();
                    }
                    RenderList(DV.Playlists[lbPlaylists.SelectedIndex]);
                }
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (lvPlaylist.SelectedItems.Count == 0)
            {
                return;
            }
            if (lvPlaylist.SelectedItems.Count > 1)
            {
                Tools.Info("Only the first selected file will play", "Multiple files selected");
            }
            var allOK = true;
            var Tasks = FFmpeg.DownloadLinks.Where(m => m.DownloadRequired).ToArray();
            if (Tasks.Length > 0)
            {
                MessageBox.Show("Playback is handled through FFmpeg but it's missing and will be downloaded now.");
            }
            foreach (var T in Tasks)
            {
                using (var f = new frmDownload(T.URL, T.Filename))
                {
                    allOK &= f.ShowDialog() == DialogResult.OK;
                }
            }
            if (allOK)
            {
                FFmpeg.PlayFileOrStream(lvPlaylist.SelectedItems[0].Text);
            }
        }
    }
}
