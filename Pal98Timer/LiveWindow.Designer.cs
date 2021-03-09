namespace Pal98Timer
{
    partial class LiveWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LiveWindow));
            this.pbMain = new System.Windows.Forms.PictureBox();
            this.menuMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.时间线ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPointsLite = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPointsAll = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLockSize = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRTMP = new System.Windows.Forms.ToolStripMenuItem();
            this.tmMain = new System.Windows.Forms.Timer(this.components);
            this.btnShowGamePic = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pbMain)).BeginInit();
            this.menuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbMain
            // 
            this.pbMain.BackColor = System.Drawing.Color.Black;
            this.pbMain.ContextMenuStrip = this.menuMain;
            this.pbMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbMain.Location = new System.Drawing.Point(0, 0);
            this.pbMain.Name = "pbMain";
            this.pbMain.Size = new System.Drawing.Size(1141, 677);
            this.pbMain.TabIndex = 0;
            this.pbMain.TabStop = false;
            this.pbMain.SizeChanged += new System.EventHandler(this.pbMain_SizeChanged);
            this.pbMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbMain_MouseClick);
            this.pbMain.Resize += new System.EventHandler(this.pbMain_Resize);
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.时间线ToolStripMenuItem,
            this.btnLockSize,
            this.btnRTMP,
            this.btnShowGamePic});
            this.menuMain.Name = "contextMenuStrip1";
            this.menuMain.Size = new System.Drawing.Size(181, 114);
            // 
            // 时间线ToolStripMenuItem
            // 
            this.时间线ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPointsLite,
            this.btnPointsAll});
            this.时间线ToolStripMenuItem.Name = "时间线ToolStripMenuItem";
            this.时间线ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.时间线ToolStripMenuItem.Text = "时间点个数";
            // 
            // btnPointsLite
            // 
            this.btnPointsLite.Name = "btnPointsLite";
            this.btnPointsLite.Size = new System.Drawing.Size(100, 22);
            this.btnPointsLite.Text = "3";
            this.btnPointsLite.Click += new System.EventHandler(this.btnPointsLite_Click);
            // 
            // btnPointsAll
            // 
            this.btnPointsAll.Name = "btnPointsAll";
            this.btnPointsAll.Size = new System.Drawing.Size(100, 22);
            this.btnPointsAll.Text = "所有";
            this.btnPointsAll.Click += new System.EventHandler(this.btnPointsAll_Click);
            // 
            // btnLockSize
            // 
            this.btnLockSize.Checked = true;
            this.btnLockSize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnLockSize.Name = "btnLockSize";
            this.btnLockSize.Size = new System.Drawing.Size(180, 22);
            this.btnLockSize.Text = "窗口大小锁定";
            this.btnLockSize.Click += new System.EventHandler(this.btnLockSize_Click);
            // 
            // btnRTMP
            // 
            this.btnRTMP.Name = "btnRTMP";
            this.btnRTMP.Size = new System.Drawing.Size(180, 22);
            this.btnRTMP.Text = "直播推流";
            this.btnRTMP.Visible = false;
            this.btnRTMP.Click += new System.EventHandler(this.btnRTMP_Click);
            // 
            // tmMain
            // 
            this.tmMain.Enabled = true;
            this.tmMain.Interval = 50;
            this.tmMain.Tick += new System.EventHandler(this.tmMain_Tick);
            // 
            // btnShowGamePic
            // 
            this.btnShowGamePic.Name = "btnShowGamePic";
            this.btnShowGamePic.Size = new System.Drawing.Size(180, 22);
            this.btnShowGamePic.Text = "打开游戏画面";
            this.btnShowGamePic.Click += new System.EventHandler(this.btnShowGamePic_Click);
            // 
            // LiveWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1141, 677);
            this.Controls.Add(this.pbMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LiveWindow";
            this.Text = "直播窗口";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LiveWindow_FormClosing);
            this.Load += new System.EventHandler(this.LiveWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbMain)).EndInit();
            this.menuMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbMain;
        private System.Windows.Forms.Timer tmMain;
        private System.Windows.Forms.ContextMenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem 时间线ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btnPointsLite;
        private System.Windows.Forms.ToolStripMenuItem btnPointsAll;
        private System.Windows.Forms.ToolStripMenuItem btnLockSize;
        private System.Windows.Forms.ToolStripMenuItem btnRTMP;
        private System.Windows.Forms.ToolStripMenuItem btnShowGamePic;
    }
}