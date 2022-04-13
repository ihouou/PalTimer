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
            this.lblClose = new System.Windows.Forms.Label();
            this.lblConfig = new System.Windows.Forms.Label();
            this.lblFunArea = new System.Windows.Forms.Label();
            this.tmMain = new System.Windows.Forms.Timer(this.components);
            this.mnData = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnKeyChange = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAutoLuck = new System.Windows.Forms.ToolStripMenuItem();
            this.mnMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnChangeStyle = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnData.SuspendLayout();
            this.mnMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblClose
            // 
            this.lblClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClose.BackColor = System.Drawing.Color.Transparent;
            this.lblClose.Location = new System.Drawing.Point(292, 4);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(30, 30);
            this.lblClose.TabIndex = 0;
            this.lblClose.Tag = ":close";
            // 
            // lblConfig
            // 
            this.lblConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfig.BackColor = System.Drawing.Color.Transparent;
            this.lblConfig.Location = new System.Drawing.Point(256, 4);
            this.lblConfig.Name = "lblConfig";
            this.lblConfig.Size = new System.Drawing.Size(30, 30);
            this.lblConfig.TabIndex = 1;
            this.lblConfig.Tag = ":config";
            this.lblConfig.Click += new System.EventHandler(this.lblConfig_Click);
            // 
            // lblFunArea
            // 
            this.lblFunArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFunArea.BackColor = System.Drawing.Color.Transparent;
            this.lblFunArea.Location = new System.Drawing.Point(3, 782);
            this.lblFunArea.Name = "lblFunArea";
            this.lblFunArea.Size = new System.Drawing.Size(321, 33);
            this.lblFunArea.TabIndex = 2;
            this.lblFunArea.Tag = ":function";
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
            this.btnAutoLuck});
            this.mnData.Name = "mnData";
            this.mnData.Size = new System.Drawing.Size(125, 48);
            // 
            // btnKeyChange
            // 
            this.btnKeyChange.Name = "btnKeyChange";
            this.btnKeyChange.Size = new System.Drawing.Size(124, 22);
            this.btnKeyChange.Text = "改键位";
            this.btnKeyChange.Click += new System.EventHandler(this.btnKeyChange_Click);
            // 
            // btnAutoLuck
            // 
            this.btnAutoLuck.Name = "btnAutoLuck";
            this.btnAutoLuck.Size = new System.Drawing.Size(124, 22);
            this.btnAutoLuck.Text = "自动换签";
            this.btnAutoLuck.Click += new System.EventHandler(this.btnAutoLuck_Click);
            // 
            // mnMain
            // 
            this.mnMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnChangeStyle,
            this.toolStripSeparator1});
            this.mnMain.Name = "mnMain";
            this.mnMain.Size = new System.Drawing.Size(101, 32);
            // 
            // btnChangeStyle
            // 
            this.btnChangeStyle.Name = "btnChangeStyle";
            this.btnChangeStyle.Size = new System.Drawing.Size(100, 22);
            this.btnChangeStyle.Text = "样式";
            this.btnChangeStyle.Click += new System.EventHandler(this.btnChangeStyle_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(97, 6);
            // 
            // GForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(327, 849);
            this.Controls.Add(this.lblFunArea);
            this.Controls.Add(this.lblConfig);
            this.Controls.Add(this.lblClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GForm";
            this.Text = "GForm";
            this.mnData.ResumeLayout(false);
            this.mnMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblClose;
        private System.Windows.Forms.Label lblConfig;
        private System.Windows.Forms.Label lblFunArea;
        private System.Windows.Forms.Timer tmMain;
        public System.Windows.Forms.ContextMenuStrip mnData;
        private System.Windows.Forms.ToolStripMenuItem btnKeyChange;
        private System.Windows.Forms.ToolStripMenuItem btnAutoLuck;
        private System.Windows.Forms.ContextMenuStrip mnMain;
        private System.Windows.Forms.ToolStripMenuItem btnChangeStyle;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}