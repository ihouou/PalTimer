namespace Pal98Timer
{
    partial class GForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GForm));
            this.tmMain = new System.Windows.Forms.Timer(this.components);
            this.mnData = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnKeyChange = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAutoLuck = new System.Windows.Forms.ToolStripMenuItem();
            this.btnShowPSInDots = new System.Windows.Forms.ToolStripMenuItem();
            this.btnEditBest = new System.Windows.Forms.ToolStripMenuItem();
            this.mnMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnChangeStyle = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPluginManage = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnData.SuspendLayout();
            this.mnMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmMain
            // 
            this.tmMain.Enabled = true;
            this.tmMain.Tick += new System.EventHandler(this.tmMain_Tick);
            // 
            // mnData
            // 
            this.mnData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnKeyChange,
            this.btnAutoLuck,
            this.btnShowPSInDots,
            this.btnEditBest});
            this.mnData.Name = "mnData";
            this.mnData.Size = new System.Drawing.Size(185, 92);
            // 
            // btnKeyChange
            // 
            this.btnKeyChange.Name = "btnKeyChange";
            this.btnKeyChange.Size = new System.Drawing.Size(184, 22);
            this.btnKeyChange.Text = "改键位";
            this.btnKeyChange.Click += new System.EventHandler(this.btnKeyChange_Click);
            // 
            // btnAutoLuck
            // 
            this.btnAutoLuck.Name = "btnAutoLuck";
            this.btnAutoLuck.Size = new System.Drawing.Size(184, 22);
            this.btnAutoLuck.Text = "自动换签";
            this.btnAutoLuck.Click += new System.EventHandler(this.btnAutoLuck_Click);
            // 
            // btnShowPSInDots
            // 
            this.btnShowPSInDots.Checked = true;
            this.btnShowPSInDots.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnShowPSInDots.Name = "btnShowPSInDots";
            this.btnShowPSInDots.Size = new System.Drawing.Size(184, 22);
            this.btnShowPSInDots.Text = "小统计显示节点间隔";
            this.btnShowPSInDots.Click += new System.EventHandler(this.btnShowPSInDots_Click);
            // 
            // btnEditBest
            // 
            this.btnEditBest.Name = "btnEditBest";
            this.btnEditBest.Size = new System.Drawing.Size(184, 22);
            this.btnEditBest.Text = "编辑最佳线";
            this.btnEditBest.Click += new System.EventHandler(this.btnEditBest_Click);
            // 
            // mnMain
            // 
            this.mnMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnChangeStyle,
            this.btnPluginManage,
            this.btnAbout,
            this.toolStripSeparator1});
            this.mnMain.Name = "mnMain";
            this.mnMain.Size = new System.Drawing.Size(125, 76);
            // 
            // btnChangeStyle
            // 
            this.btnChangeStyle.Name = "btnChangeStyle";
            this.btnChangeStyle.Size = new System.Drawing.Size(124, 22);
            this.btnChangeStyle.Text = "样式";
            this.btnChangeStyle.Click += new System.EventHandler(this.btnChangeStyle_Click);
            // 
            // btnPluginManage
            // 
            this.btnPluginManage.Name = "btnPluginManage";
            this.btnPluginManage.Size = new System.Drawing.Size(124, 22);
            this.btnPluginManage.Text = "插件管理";
            this.btnPluginManage.Click += new System.EventHandler(this.btnPluginManage_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(124, 22);
            this.btnAbout.Text = "关于";
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(121, 6);
            // 
            // GForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(327, 849);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GForm";
            this.Text = "自动计时器";
            this.mnData.ResumeLayout(false);
            this.mnMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tmMain;
        public System.Windows.Forms.ContextMenuStrip mnData;
        private System.Windows.Forms.ToolStripMenuItem btnKeyChange;
        private System.Windows.Forms.ToolStripMenuItem btnAutoLuck;
        private System.Windows.Forms.ContextMenuStrip mnMain;
        private System.Windows.Forms.ToolStripMenuItem btnChangeStyle;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem btnPluginManage;
        private System.Windows.Forms.ToolStripMenuItem btnAbout;
        private System.Windows.Forms.ToolStripMenuItem btnEditBest;
        private System.Windows.Forms.ToolStripMenuItem btnShowPSInDots;
    }
}