namespace Pal98Timer
{
    partial class TSSet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TSSet));
            this.txtHour = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMin = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSec = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMs = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtHour
            // 
            this.txtHour.Location = new System.Drawing.Point(18, 31);
            this.txtHour.Name = "txtHour";
            this.txtHour.Size = new System.Drawing.Size(44, 21);
            this.txtHour.TabIndex = 0;
            this.txtHour.Text = "0";
            this.txtHour.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtHour.Validating += new System.ComponentModel.CancelEventHandler(this.txtHour_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(68, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "小时";
            // 
            // txtMin
            // 
            this.txtMin.Location = new System.Drawing.Point(107, 31);
            this.txtMin.Name = "txtMin";
            this.txtMin.Size = new System.Drawing.Size(44, 21);
            this.txtMin.TabIndex = 1;
            this.txtMin.Text = "0";
            this.txtMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtMin.Validating += new System.ComponentModel.CancelEventHandler(this.txtMin_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(157, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "分";
            // 
            // txtSec
            // 
            this.txtSec.Location = new System.Drawing.Point(190, 31);
            this.txtSec.Name = "txtSec";
            this.txtSec.Size = new System.Drawing.Size(44, 21);
            this.txtSec.TabIndex = 2;
            this.txtSec.Text = "0";
            this.txtSec.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSec.Validating += new System.ComponentModel.CancelEventHandler(this.txtSec_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(240, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "秒";
            // 
            // txtMs
            // 
            this.txtMs.Location = new System.Drawing.Point(275, 31);
            this.txtMs.Name = "txtMs";
            this.txtMs.Size = new System.Drawing.Size(44, 21);
            this.txtMs.TabIndex = 3;
            this.txtMs.Text = "0";
            this.txtMs.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtMs.Validating += new System.ComponentModel.CancelEventHandler(this.txtMs_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(325, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "毫秒";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(190, 97);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(287, 97);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // TSSet
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(374, 132);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMs);
            this.Controls.Add(this.txtSec);
            this.Controls.Add(this.txtMin);
            this.Controls.Add(this.txtHour);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TSSet";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "设置时间";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtHour;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSec;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}