namespace Pal98Timer
{
    partial class BestEditItem
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
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtDes = new System.Windows.Forms.TextBox();
            this.numHour = new System.Windows.Forms.NumericUpDown();
            this.numMin = new System.Windows.Forms.NumericUpDown();
            this.numSec = new System.Windows.Forms.NumericUpDown();
            this.btnDrop = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSec)).BeginInit();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(-1, -1);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 21);
            this.txtName.TabIndex = 1;
            // 
            // txtDes
            // 
            this.txtDes.Location = new System.Drawing.Point(100, -1);
            this.txtDes.Name = "txtDes";
            this.txtDes.Size = new System.Drawing.Size(100, 21);
            this.txtDes.TabIndex = 3;
            // 
            // numHour
            // 
            this.numHour.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numHour.Location = new System.Drawing.Point(203, -1);
            this.numHour.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numHour.Name = "numHour";
            this.numHour.Size = new System.Drawing.Size(42, 21);
            this.numHour.TabIndex = 4;
            // 
            // numMin
            // 
            this.numMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numMin.Location = new System.Drawing.Point(243, -1);
            this.numMin.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numMin.Name = "numMin";
            this.numMin.Size = new System.Drawing.Size(35, 21);
            this.numMin.TabIndex = 6;
            // 
            // numSec
            // 
            this.numSec.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numSec.Location = new System.Drawing.Point(276, -1);
            this.numSec.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numSec.Name = "numSec";
            this.numSec.Size = new System.Drawing.Size(35, 21);
            this.numSec.TabIndex = 8;
            // 
            // btnDrop
            // 
            this.btnDrop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnDrop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDrop.Location = new System.Drawing.Point(312, -2);
            this.btnDrop.Name = "btnDrop";
            this.btnDrop.Size = new System.Drawing.Size(25, 23);
            this.btnDrop.TabIndex = 10;
            this.btnDrop.Text = "x";
            this.btnDrop.UseVisualStyleBackColor = false;
            this.btnDrop.Click += new System.EventHandler(this.btnDrop_Click);
            // 
            // BestEditItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.btnDrop);
            this.Controls.Add(this.numSec);
            this.Controls.Add(this.numMin);
            this.Controls.Add(this.numHour);
            this.Controls.Add(this.txtDes);
            this.Controls.Add(this.txtName);
            this.Name = "BestEditItem";
            this.Size = new System.Drawing.Size(337, 20);
            ((System.ComponentModel.ISupportInitialize)(this.numHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSec)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtDes;
        private System.Windows.Forms.NumericUpDown numHour;
        private System.Windows.Forms.NumericUpDown numMin;
        private System.Windows.Forms.NumericUpDown numSec;
        private System.Windows.Forms.Button btnDrop;
    }
}
