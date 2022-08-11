namespace Pal98Timer
{
    partial class GEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GEditForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbItemHeight = new System.Windows.Forms.TrackBar();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSetBG = new System.Windows.Forms.Button();
            this.btnClearBG = new System.Windows.Forms.Button();
            this.gpBlock = new System.Windows.Forms.GroupBox();
            this.lblBlock = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.dlgColor = new System.Windows.Forms.ColorDialog();
            this.dlgFont = new System.Windows.Forms.FontDialog();
            this.dlgFile = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbItemHeight)).BeginInit();
            this.gpBlock.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.Controls.Add(this.btnSave);
            this.splitContainer1.Panel2.Controls.Add(this.btnSetBG);
            this.splitContainer1.Panel2.Controls.Add(this.btnClearBG);
            this.splitContainer1.Panel2.Controls.Add(this.gpBlock);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Size = new System.Drawing.Size(559, 700);
            this.splitContainer1.SplitterDistance = 338;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tbItemHeight);
            this.groupBox1.Location = new System.Drawing.Point(5, 309);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 72);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置行高";
            // 
            // tbItemHeight
            // 
            this.tbItemHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbItemHeight.AutoSize = false;
            this.tbItemHeight.LargeChange = 1;
            this.tbItemHeight.Location = new System.Drawing.Point(6, 20);
            this.tbItemHeight.Maximum = 30;
            this.tbItemHeight.Minimum = 20;
            this.tbItemHeight.Name = "tbItemHeight";
            this.tbItemHeight.Size = new System.Drawing.Size(188, 45);
            this.tbItemHeight.TabIndex = 0;
            this.tbItemHeight.Value = 20;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(130, 665);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSetBG
            // 
            this.btnSetBG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetBG.Location = new System.Drawing.Point(130, 264);
            this.btnSetBG.Name = "btnSetBG";
            this.btnSetBG.Size = new System.Drawing.Size(75, 23);
            this.btnSetBG.TabIndex = 3;
            this.btnSetBG.Text = "设置背景图";
            this.btnSetBG.UseVisualStyleBackColor = true;
            this.btnSetBG.Click += new System.EventHandler(this.btnSetBG_Click);
            // 
            // btnClearBG
            // 
            this.btnClearBG.Location = new System.Drawing.Point(5, 264);
            this.btnClearBG.Name = "btnClearBG";
            this.btnClearBG.Size = new System.Drawing.Size(75, 23);
            this.btnClearBG.TabIndex = 2;
            this.btnClearBG.Text = "清除背景图";
            this.btnClearBG.UseVisualStyleBackColor = true;
            this.btnClearBG.Click += new System.EventHandler(this.btnClearBG_Click);
            // 
            // gpBlock
            // 
            this.gpBlock.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpBlock.Controls.Add(this.lblBlock);
            this.gpBlock.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.gpBlock.Location = new System.Drawing.Point(5, 37);
            this.gpBlock.Name = "gpBlock";
            this.gpBlock.Size = new System.Drawing.Size(200, 211);
            this.gpBlock.TabIndex = 1;
            this.gpBlock.TabStop = false;
            this.gpBlock.Text = "--";
            this.gpBlock.Visible = false;
            // 
            // lblBlock
            // 
            this.lblBlock.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBlock.Font = new System.Drawing.Font("宋体", 9F);
            this.lblBlock.Location = new System.Drawing.Point(6, 22);
            this.lblBlock.Name = "lblBlock";
            this.lblBlock.Size = new System.Drawing.Size(188, 176);
            this.lblBlock.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(3, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 22);
            this.label2.TabIndex = 0;
            this.label2.Text = "移动鼠标到需要修改样式的位置";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // dlgColor
            // 
            this.dlgColor.FullOpen = true;
            // 
            // dlgFont
            // 
            this.dlgFont.AllowVerticalFonts = false;
            // 
            // dlgFile
            // 
            this.dlgFile.FileName = "选择背景图";
            this.dlgFile.Filter = "图片文件|*.png;*.jpg;*.jpeg;*.gif;*.bmp";
            // 
            // GEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 700);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(550, 500);
            this.Name = "GEditForm";
            this.Text = "编辑样式";
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbItemHeight)).EndInit();
            this.gpBlock.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gpBlock;
        private System.Windows.Forms.Label lblBlock;
        private System.Windows.Forms.Button btnClearBG;
        private System.Windows.Forms.Button btnSetBG;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ColorDialog dlgColor;
        private System.Windows.Forms.FontDialog dlgFont;
        private System.Windows.Forms.OpenFileDialog dlgFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TrackBar tbItemHeight;
    }
}