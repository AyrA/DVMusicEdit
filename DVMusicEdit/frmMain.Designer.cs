
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
            this.browseForLocalfilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addHTTPStreamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importFromYoutubeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CmsAdd.SuspendLayout();
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
            this.lbPlaylists.SelectedIndexChanged += new System.EventHandler(this.lbPlaylists_SelectedIndexChanged);
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
            this.lvPlaylist.Size = new System.Drawing.Size(526, 269);
            this.lvPlaylist.TabIndex = 1;
            this.lvPlaylist.UseCompatibleStateImageBehavior = false;
            this.lvPlaylist.View = System.Windows.Forms.View.Details;
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
            this.btnAdd.Location = new System.Drawing.Point(346, 287);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(427, 287);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Location = new System.Drawing.Point(508, 287);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(75, 23);
            this.btnUp.TabIndex = 4;
            this.btnUp.Text = "Move &Up";
            this.btnUp.UseVisualStyleBackColor = true;
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Location = new System.Drawing.Point(589, 287);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(75, 23);
            this.btnDown.TabIndex = 5;
            this.btnDown.Text = "&Move Down";
            this.btnDown.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(589, 318);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "&Save all";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // CmsAdd
            // 
            this.CmsAdd.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.browseForLocalfilesToolStripMenuItem,
            this.addHTTPStreamToolStripMenuItem,
            this.importFromYoutubeToolStripMenuItem});
            this.CmsAdd.Name = "CmsAdd";
            this.CmsAdd.Size = new System.Drawing.Size(191, 70);
            // 
            // browseForLocalfilesToolStripMenuItem
            // 
            this.browseForLocalfilesToolStripMenuItem.Name = "browseForLocalfilesToolStripMenuItem";
            this.browseForLocalfilesToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.browseForLocalfilesToolStripMenuItem.Text = "Browse for local &file(s)";
            // 
            // addHTTPStreamToolStripMenuItem
            // 
            this.addHTTPStreamToolStripMenuItem.Name = "addHTTPStreamToolStripMenuItem";
            this.addHTTPStreamToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.addHTTPStreamToolStripMenuItem.Text = "Add &HTTP stream";
            // 
            // importFromYoutubeToolStripMenuItem
            // 
            this.importFromYoutubeToolStripMenuItem.Name = "importFromYoutubeToolStripMenuItem";
            this.importFromYoutubeToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.importFromYoutubeToolStripMenuItem.Text = "Import from &Youtube";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 353);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvPlaylist);
            this.Controls.Add(this.lbPlaylists);
            this.Name = "frmMain";
            this.Text = "Derail Valley Playlist Manager";
            this.CmsAdd.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem browseForLocalfilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addHTTPStreamToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importFromYoutubeToolStripMenuItem;
    }
}

