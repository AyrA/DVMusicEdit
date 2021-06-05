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
            btnPlay.Enabled = btnEdit.Enabled =
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
                var NeedTimes = PL.Entries.Any(m => !m.IsValidTime);
                if (NeedTimes && !Tools.AskInfo("Some files seem to be missing the runtime. Do you want to automatically scan for them now?", "Missing times"))
                {
                    NeedTimes = false;
                }
                else
                {
                    NeedTimes = RequireFfmpeg();
                }
                foreach (var Entry in PL.Entries)
                {
                    var Item = lvPlaylist.Items.Add(Entry.FileName);
                    if (NeedTimes && !Entry.IsValidTime)
                    {
                        try
                        {
                            Entry.Duration = (int)FFmpeg.GetDuration(Entry.FileName).TotalSeconds;
                        }
                        catch (Exception ex)
                        {
                            NeedTimes = Tools.AskError($"Can't determine the time of {Entry.FileName}.\r\nReason: {ex.Message}\r\nContinue trying for the remaining files?", "Error determining time");
                        }
                    }
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

        private bool RequireFfmpeg()
        {
            var allOK = true;
            var Tasks = FFmpeg.DownloadLinks.Where(m => m.DownloadRequired).ToArray();
            if (Tasks.Length > 0)
            {
                MessageBox.Show("This operation needs FFmpeg but it's missing and will be downloaded now.");
            }
            foreach (var T in Tasks)
            {
                using (var f = new frmDownload(T.URL, T.Filename))
                {
                    allOK &= f.ShowDialog() == DialogResult.OK;
                }
            }
            return allOK;
        }

        private void EditEntry()
        {
            if (lvPlaylist.SelectedItems.Count == 0)
            {
                return;
            }
            if (lvPlaylist.SelectedItems.Count > 1)
            {
                Tools.Info("Only the first selected file will be edited", "Multiple files selected");
            }
            var Index = lvPlaylist.SelectedItems[0].Index;
            var List = DV.Playlists[lbPlaylists.SelectedIndex];
            EditEntry(List, Index);
        }

        private void EditEntry(Playlist List, int Index)
        {
            var Entry = List.Entries[Index];
            using (var F = new frmEntry(Entry))
            {
                if (F.ShowDialog() == DialogResult.OK)
                {
                    if (Entry.IsStream || Entry.Duration == 0)
                    {
                        Entry.Duration = -1;
                    }
                    RenderList(List);
                    lvPlaylist.Items[Index].Selected = true;
                }
            }
        }

        private void MoveEntriesUp()
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

        private void MoveEntriesDown()
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

        private void DeleteSelectedItems()
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

        private void ResetPlaylists()
        {
            if (lbPlaylists.SelectedIndex >= 0)
            {
                if (Tools.AskWarn("Reset this playlist to what is stored in the file?", "Reset playlist"))
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

        private void SavePlaylists()
        {
            if (Tools.AskWarn("Write all changes? This overwrites any existing list", "Overwrite existing lists"))
            {
                if (!DV.SaveLists())
                {
                    Tools.Error(@"Failed to save all your changes.
Most likely reason is that one of the playlist files is in use by another application.
Try to close any media player that may be accessing the lists and make sure derail valley is not running, then try again.
Do not close this application or you lose your changes.", "Failed to save files");
                }
                //Keep note of where the user was
                var CurrentList = lbPlaylists.SelectedIndex;
                var CurrentItems = lvPlaylist.SelectedIndices.OfType<int>().ToArray();
                InitDV();
                //Restore user position
                RenderList(DV.Playlists[CurrentList]);
                lbPlaylists.SelectedIndex = CurrentList;
                foreach (var Index in CurrentItems)
                {
                    if (lvPlaylist.Items.Count > Index)
                    {
                        lvPlaylist.Items[Index].Selected = true;
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CmsAdd.Show(btnAdd, new System.Drawing.Point(0, btnAdd.Height));
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteSelectedItems();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            MoveEntriesUp();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            MoveEntriesDown();

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetPlaylists();
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
            if (RequireFfmpeg())
            {
                FFmpeg.PlayFileOrStream(lvPlaylist.SelectedItems[0].Text);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditEntry();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SavePlaylists();
        }

        private void cmsAddLocal_Click(object sender, EventArgs e)
        {
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                var Files = (string[])OFD.FileNames.Clone();
                if(Files.Any(DerailValley.IsAcceptedMediaType))
                {

                }
            }
        }

        private void cmsAddStream_Click(object sender, EventArgs e)
        {
            var Radio = DV.Playlists[0];
            if (Clipboard.ContainsText())
            {
                string Text;
                try
                {
                    Text = Clipboard.GetText().Trim();
                }
                catch (Exception ex)
                {
                    Tools.Error($"Failed to get clipboard contents.\r\n{ex.Message}", "Clipboard error");
                    return;
                }
                var Entry = new PlaylistEntry()
                {
                    FileName = Text
                };
                if (!Entry.IsStream)
                {
                    Tools.Error($"Unsupported stream type. Stream URL must start with http(s)://", "Clipboard error");
                    return;
                }
                if (Radio.Entries.Any(m => m.FileName == Text))
                {
                    Tools.Warn($"You already have {Text} in the list", "Duplicate item");
                    return;
                }
                Radio.AddItem(Entry.FileName);
                //Add and select temp item
                RenderList(Radio);
                lvPlaylist.Items[lvPlaylist.Items.Count - 1].Selected = true;
                //Edit said temp item
                EditEntry(Radio, Radio.Entries.Length - 1);
            }
            else
            {
                Tools.Warn("Your clipboard doesn't contains any text. Please copy the stream URL first.", "Clipboard import failed");
            }
        }

        private void cmsAddYoutube_Click(object sender, EventArgs e)
        {
            Tools.Error("This is currently not implemented and will be part of a later version", "Missing feature");
        }

        private void lvPlaylist_DoubleClick(object sender, EventArgs e)
        {
            var Item = lvPlaylist.SelectedItems.OfType<ListViewItem>().FirstOrDefault();
            if (Item != null)
            {
                EditEntry(DV.Playlists[lbPlaylists.SelectedIndex], Item.Index);
            }
        }

        private void lvPlaylist_KeyDown(object sender, KeyEventArgs e)
        {
            //Return early if no playlist is selected
            if (lbPlaylists.SelectedIndex < 0)
            {
                return;
            }
            switch (e.KeyCode)
            {
                case Keys.Insert:
                    if (btnAdd.Enabled)
                    {
                        e.Handled = e.SuppressKeyPress = true;
                        CmsAdd.Show(MousePosition);
                    }
                    break;
                case Keys.Delete:
                    if (btnDelete.Enabled)
                    {
                        e.Handled = e.SuppressKeyPress = true;
                        DeleteSelectedItems();
                    }
                    break;
                case Keys.Up:
                    if (e.Modifiers == Keys.Alt)
                    {
                        e.Handled = e.SuppressKeyPress = true;
                        MoveEntriesUp();
                    }
                    break;
                case Keys.Down:
                    if (e.Modifiers == Keys.Alt)
                    {
                        e.Handled = e.SuppressKeyPress = true;
                        MoveEntriesDown();
                    }
                    break;
                case Keys.Enter:
                    e.Handled = e.SuppressKeyPress = true;
                    EditEntry();
                    break;
            }
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.S:
                    if (e.Modifiers == Keys.Control)
                    {
                        e.Handled = e.SuppressKeyPress = true;
                        SavePlaylists();
                    }
                    break;
                case Keys.R:
                    if (e.Modifiers == Keys.Control)
                    {
                        e.Handled = e.SuppressKeyPress = true;
                        ResetPlaylists();
                    }
                    break;
            }
        }
    }
}
