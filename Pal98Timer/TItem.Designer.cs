namespace Pal98Timer
{
    partial class TItem
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblBest = new System.Windows.Forms.Label();
            this.lblCurrent = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(86, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "--";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.DoubleClick += new System.EventHandler(this.lblTitle_DoubleClick);
            // 
            // lblBest
            // 
            this.lblBest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBest.Location = new System.Drawing.Point(84, 0);
            this.lblBest.Name = "lblBest";
            this.lblBest.Size = new System.Drawing.Size(85, 25);
            this.lblBest.TabIndex = 0;
            this.lblBest.Text = "--";
            this.lblBest.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCurrent
            // 
            this.lblCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCurrent.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblCurrent.Location = new System.Drawing.Point(168, 0);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.Size = new System.Drawing.Size(107, 25);
            this.lblCurrent.TabIndex = 0;
            this.lblCurrent.Text = "--";
            this.lblCurrent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblCurrent);
            this.Controls.Add(this.lblBest);
            this.Controls.Add(this.lblTitle);
            this.Name = "TItem";
            this.Size = new System.Drawing.Size(275, 25);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.Label lblBest;
        public System.Windows.Forms.Label lblCurrent;


    }
}
