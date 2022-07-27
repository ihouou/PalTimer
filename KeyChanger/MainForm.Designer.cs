namespace KeyChanger
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.niMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnEnable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSetKeys = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExit = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBlockCE = new System.Windows.Forms.ToolStripMenuItem();
            this.cmMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // niMain
            // 
            this.niMain.ContextMenuStrip = this.cmMain;
            this.niMain.Icon = ((System.Drawing.Icon)(resources.GetObject("niMain.Icon")));
            this.niMain.Text = "改建器 by Houou";
            this.niMain.Visible = true;
            this.niMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.niMain_MouseDoubleClick);
            // 
            // cmMain
            // 
            this.cmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnEnable,
            this.btnBlockCE,
            this.toolStripSeparator1,
            this.btnSetKeys,
            this.btnExit});
            this.cmMain.Name = "cmMain";
            this.cmMain.Size = new System.Drawing.Size(160, 98);
            // 
            // btnEnable
            // 
            this.btnEnable.Name = "btnEnable";
            this.btnEnable.Size = new System.Drawing.Size(180, 22);
            this.btnEnable.Text = "启用改键";
            this.btnEnable.Click += new System.EventHandler(this.btnEnable_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // btnSetKeys
            // 
            this.btnSetKeys.Name = "btnSetKeys";
            this.btnSetKeys.Size = new System.Drawing.Size(180, 22);
            this.btnSetKeys.Text = "设置键位";
            this.btnSetKeys.Click += new System.EventHandler(this.btnSetKeys_Click);
            // 
            // btnExit
            // 
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(180, 22);
            this.btnExit.Text = "退出";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnBlockCE
            // 
            this.btnBlockCE.Name = "btnBlockCE";
            this.btnBlockCE.Size = new System.Drawing.Size(180, 22);
            this.btnBlockCE.Text = "屏蔽Ctrl+Enter";
            this.btnBlockCE.Click += new System.EventHandler(this.btnBlockCE_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 207);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Opacity = 0D;
            this.ShowInTaskbar = false;
            this.Text = "改建器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.cmMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon niMain;
        private System.Windows.Forms.ContextMenuStrip cmMain;
        private System.Windows.Forms.ToolStripMenuItem btnExit;
        private System.Windows.Forms.ToolStripMenuItem btnSetKeys;
        private System.Windows.Forms.ToolStripMenuItem btnEnable;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem btnBlockCE;
    }
}

