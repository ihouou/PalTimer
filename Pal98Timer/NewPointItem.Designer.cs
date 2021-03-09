namespace Pal98Timer
{
    partial class NewPointItem
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblPTitle = new System.Windows.Forms.Label();
            this.cmColorBoard = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnSetTitleColor = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSetBestColor = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSetFasterColor = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSetSlowerColor = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSetEqualColor = new System.Windows.Forms.ToolStripMenuItem();
            this.lblPCHA = new System.Windows.Forms.Label();
            this.lblPBest = new System.Windows.Forms.Label();
            this.lblPCurrent = new System.Windows.Forms.Label();
            this.cmColorBoard.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPTitle
            // 
            this.lblPTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPTitle.ContextMenuStrip = this.cmColorBoard;
            this.lblPTitle.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPTitle.Location = new System.Drawing.Point(0, 0);
            this.lblPTitle.Name = "lblPTitle";
            this.lblPTitle.Size = new System.Drawing.Size(155, 28);
            this.lblPTitle.TabIndex = 0;
            this.lblPTitle.Text = "进十年前";
            this.lblPTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPTitle.DoubleClick += new System.EventHandler(this.lblTitle_DoubleClick);
            // 
            // cmColorBoard
            // 
            this.cmColorBoard.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSetTitleColor,
            this.btnSetBestColor,
            this.btnSetFasterColor,
            this.btnSetSlowerColor,
            this.btnSetEqualColor});
            this.cmColorBoard.Name = "cmColorBoard";
            this.cmColorBoard.Size = new System.Drawing.Size(149, 114);
            // 
            // btnSetTitleColor
            // 
            this.btnSetTitleColor.Name = "btnSetTitleColor";
            this.btnSetTitleColor.Size = new System.Drawing.Size(148, 22);
            this.btnSetTitleColor.Text = "设置标题颜色";
            this.btnSetTitleColor.Click += new System.EventHandler(this.btnSetTitleColor_Click);
            // 
            // btnSetBestColor
            // 
            this.btnSetBestColor.Name = "btnSetBestColor";
            this.btnSetBestColor.Size = new System.Drawing.Size(148, 22);
            this.btnSetBestColor.Text = "设置最佳颜色";
            this.btnSetBestColor.Click += new System.EventHandler(this.btnSetBestColor_Click);
            // 
            // btnSetFasterColor
            // 
            this.btnSetFasterColor.Name = "btnSetFasterColor";
            this.btnSetFasterColor.Size = new System.Drawing.Size(148, 22);
            this.btnSetFasterColor.Text = "设置“快”颜色";
            this.btnSetFasterColor.Click += new System.EventHandler(this.btnSetFasterColor_Click);
            // 
            // btnSetSlowerColor
            // 
            this.btnSetSlowerColor.Name = "btnSetSlowerColor";
            this.btnSetSlowerColor.Size = new System.Drawing.Size(148, 22);
            this.btnSetSlowerColor.Text = "设置“慢”颜色";
            this.btnSetSlowerColor.Click += new System.EventHandler(this.btnSetSlowerColor_Click);
            // 
            // btnSetEqualColor
            // 
            this.btnSetEqualColor.Name = "btnSetEqualColor";
            this.btnSetEqualColor.Size = new System.Drawing.Size(148, 22);
            this.btnSetEqualColor.Text = "设置“等”颜色";
            this.btnSetEqualColor.Click += new System.EventHandler(this.btnSetEqualColor_Click);
            // 
            // lblPCHA
            // 
            this.lblPCHA.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPCHA.ContextMenuStrip = this.cmColorBoard;
            this.lblPCHA.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPCHA.Location = new System.Drawing.Point(146, 15);
            this.lblPCHA.Name = "lblPCHA";
            this.lblPCHA.Size = new System.Drawing.Size(75, 13);
            this.lblPCHA.TabIndex = 1;
            this.lblPCHA.Text = "-122:07";
            this.lblPCHA.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblPCHA.DoubleClick += new System.EventHandler(this.lblCHA_DoubleClick);
            // 
            // lblPBest
            // 
            this.lblPBest.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPBest.ContextMenuStrip = this.cmColorBoard;
            this.lblPBest.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPBest.ForeColor = System.Drawing.Color.Silver;
            this.lblPBest.Location = new System.Drawing.Point(151, 0);
            this.lblPBest.Name = "lblPBest";
            this.lblPBest.Size = new System.Drawing.Size(70, 19);
            this.lblPBest.TabIndex = 2;
            this.lblPBest.Text = "01:15:36";
            this.lblPBest.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblPBest.DoubleClick += new System.EventHandler(this.lblBest_DoubleClick);
            // 
            // lblPCurrent
            // 
            this.lblPCurrent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPCurrent.ContextMenuStrip = this.cmColorBoard;
            this.lblPCurrent.Font = new System.Drawing.Font("Consolas", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPCurrent.Location = new System.Drawing.Point(216, 0);
            this.lblPCurrent.Name = "lblPCurrent";
            this.lblPCurrent.Size = new System.Drawing.Size(111, 28);
            this.lblPCurrent.TabIndex = 3;
            this.lblPCurrent.Text = "01:15:36.23";
            this.lblPCurrent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblPCurrent.DoubleClick += new System.EventHandler(this.lblCurrent_DoubleClick);
            // 
            // NewPointItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.lblPTitle);
            this.Controls.Add(this.lblPCHA);
            this.Controls.Add(this.lblPCurrent);
            this.Controls.Add(this.lblPBest);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "NewPointItem";
            this.Size = new System.Drawing.Size(327, 28);
            this.cmColorBoard.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblPTitle;
        private System.Windows.Forms.Label lblPCHA;
        private System.Windows.Forms.Label lblPBest;
        private System.Windows.Forms.Label lblPCurrent;
        private System.Windows.Forms.ContextMenuStrip cmColorBoard;
        private System.Windows.Forms.ToolStripMenuItem btnSetTitleColor;
        private System.Windows.Forms.ToolStripMenuItem btnSetBestColor;
        private System.Windows.Forms.ToolStripMenuItem btnSetFasterColor;
        private System.Windows.Forms.ToolStripMenuItem btnSetSlowerColor;
        private System.Windows.Forms.ToolStripMenuItem btnSetEqualColor;
    }
}
