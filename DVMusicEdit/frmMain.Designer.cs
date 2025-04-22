
namespace DVMusicEdit
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lbPlaylists = new System.Windows.Forms.ListBox();
            this.lvPlaylist = new System.Windows.Forms.ListView();
            this.chFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.CmsAdd = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsAddLocal = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsAddStream = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsAddYoutube = new System.Windows.Forms.ToolStripMenuItem();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.OFD = new System.Windows.Forms.OpenFileDialog();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnMore = new System.Windows.Forms.Button();
            this.CmsMore = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsDownloadFfmpeg = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsDownloadYoutubedl = new System.Windows.Forms.ToolStripMenuItem();
            this.FBD = new System.Windows.Forms.FolderBrowserDialog();
            this.CmsAdd.SuspendLayout();
            this.CmsMore.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbPlaylists
            // 
            this.lbPlaylists.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbPlaylists.FormattingEnabled = true;
            this.lbPlaylists.Location = new System.Drawing.Point(12, 12);
            this.lbPlaylists.Name = "lbPlaylists";
            this.lbPlaylists.Size = new System.Drawing.Size(120, 316);
            this.lbPlaylists.TabIndex = 0;
            this.lbPlaylists.SelectedIndexChanged += new System.EventHandler(this.LbPlaylists_SelectedIndexChanged);
            // 
            // lvPlaylist
            // 
            this.lvPlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvPlaylist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chFile,
            this.chTime,
            this.chTitle});
            this.lvPlaylist.FullRowSelect = true;
            this.lvPlaylist.HideSelection = false;
            this.lvPlaylist.Location = new System.Drawing.Point(138, 12);
            this.lvPlaylist.Name = "lvPlaylist";
            this.lvPlaylist.Size = new System.Drawing.Size(726, 269);
            this.lvPlaylist.TabIndex = 1;
            this.lvPlaylist.UseCompatibleStateImageBehavior = false;
            this.lvPlaylist.View = System.Windows.Forms.View.Details;
            this.lvPlaylist.DoubleClick += new System.EventHandler(this.LvPlaylist_DoubleClick);
            this.lvPlaylist.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LvPlaylist_KeyDown);
            // 
            // chFile
            // 
            this.chFile.Text = "File/URL";
            this.chFile.Width = 200;
            // 
            // chTime
            // 
            this.chTime.Text = "Duration";
            // 
            // chTitle
            // 
            this.chTitle.Text = "Title";
            this.chTitle.Width = 200;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(465, 287);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(627, 287);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Location = new System.Drawing.Point(708, 287);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(75, 23);
            this.btnUp.TabIndex = 6;
            this.btnUp.Text = "Move &Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.BtnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Location = new System.Drawing.Point(789, 287);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(75, 23);
            this.btnDown.TabIndex = 7;
            this.btnDown.Text = "&Move Down";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.BtnDown_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(789, 318);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "&Save all...";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // CmsAdd
            // 
            this.CmsAdd.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsAddLocal,
            this.cmsAddStream,
            this.cmsAddYoutube});
            this.CmsAdd.Name = "CmsAdd";
            this.CmsAdd.Size = new System.Drawing.Size(177, 70);
            // 
            // cmsAddLocal
            // 
            this.cmsAddLocal.Name = "cmsAddLocal";
            this.cmsAddLocal.Size = new System.Drawing.Size(176, 22);
            this.cmsAddLocal.Text = "Browse for local &file(s)";
            this.cmsAddLocal.Click += new System.EventHandler(this.CmsAddLocal_Click);
            // 
            // cmsAddStream
            // 
            this.cmsAddStream.Name = "cmsAddStream";
            this.cmsAddStream.Size = new System.Drawing.Size(176, 22);
            this.cmsAddStream.Text = "Add &HTTP stream";
            this.cmsAddStream.Click += new System.EventHandler(this.CmsAddStream_Click);
            // 
            // cmsAddYoutube
            // 
            this.cmsAddYoutube.Name = "cmsAddYoutube";
            this.cmsAddYoutube.Size = new System.Drawing.Size(176, 22);
            this.cmsAddYoutube.Text = "Import from &Youtube";
            this.cmsAddYoutube.Click += new System.EventHandler(this.CmsAddYoutube_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(708, 318);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 9;
            this.btnReset.Text = "&Reset...";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlay.Location = new System.Drawing.Point(384, 287);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(75, 23);
            this.btnPlay.TabIndex = 2;
            this.btnPlay.Text = "&Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.BtnPlay_Click);
            // 
            // OFD
            // 
            this.OFD.Filter = "Common media files|*.mp3;*.ogg;*.wav;*.webm;*.mp4;*.m4a;*.flac;*.aac;*.wma|All fi" +
    "les|*.*";
            this.OFD.Multiselect = true;
            this.OFD.Title = "Select media files";
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Location = new System.Drawing.Point(546, 287);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // btnMore
            // 
            this.btnMore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMore.Location = new System.Drawing.Point(627, 318);
            this.btnMore.Name = "btnMore";
            this.btnMore.Size = new System.Drawing.Size(75, 23);
            this.btnMore.TabIndex = 8;
            this.btnMore.Text = "M&ore...";
            this.btnMore.UseVisualStyleBackColor = true;
            this.btnMore.Click += new System.EventHandler(this.BtnMore_Click);
            // 
            // CmsMore
            // 
            this.CmsMore.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsDownloadFfmpeg,
            this.cmsDownloadYoutubedl});
            this.CmsMore.Name = "CmsMore";
            this.CmsMore.Size = new System.Drawing.Size(177, 48);
            // 
            // cmsDownloadFfmpeg
            // 
            this.cmsDownloadFfmpeg.Name = "cmsDownloadFfmpeg";
            this.cmsDownloadFfmpeg.Size = new System.Drawing.Size(176, 22);
            this.cmsDownloadFfmpeg.Text = "Download &Ffmpeg";
            this.cmsDownloadFfmpeg.Click += new System.EventHandler(this.CmsDownloadFfmpeg_Click);
            // 
            // cmsDownloadYoutubedl
            // 
            this.cmsDownloadYoutubedl.Name = "cmsDownloadYoutubedl";
            this.cmsDownloadYoutubedl.Size = new System.Drawing.Size(176, 22);
            this.cmsDownloadYoutubedl.Text = "Download &Youtube-dl";
            this.cmsDownloadYoutubedl.Click += new System.EventHandler(this.CmsDownloadYoutubedl_Click);
            // 
            // FBD
            // 
            this.FBD.Description = "Select \"Derail Valley\" folder";
            this.FBD.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 353);
            this.Controls.Add(this.btnMore);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvPlaylist);
            this.Controls.Add(this.lbPlaylists);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(700, 300);
            this.Name = "frmMain";
            this.Text = "Derail Valley Playlist Manager";
            this.Shown += new System.EventHandler(this.FrmMain_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmMain_KeyDown);
            this.CmsAdd.ResumeLayout(false);
            this.CmsMore.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbPlaylists;
        private System.Windows.Forms.ListView lvPlaylist;
        private System.Windows.Forms.ColumnHeader chFile;
        private System.Windows.Forms.ColumnHeader chTime;
        private System.Windows.Forms.ColumnHeader chTitle;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ContextMenuStrip CmsAdd;
        private System.Windows.Forms.ToolStripMenuItem cmsAddLocal;
        private System.Windows.Forms.ToolStripMenuItem cmsAddStream;
        private System.Windows.Forms.ToolStripMenuItem cmsAddYoutube;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.OpenFileDialog OFD;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnMore;
        private System.Windows.Forms.ContextMenuStrip CmsMore;
        private System.Windows.Forms.ToolStripMenuItem cmsDownloadFfmpeg;
        private System.Windows.Forms.ToolStripMenuItem cmsDownloadYoutubedl;
        private System.Windows.Forms.FolderBrowserDialog FBD;
    }
}

