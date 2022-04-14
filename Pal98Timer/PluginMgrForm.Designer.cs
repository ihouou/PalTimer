namespace Pal98Timer
{
    partial class PluginMgrForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginMgrForm));
            this.dvPlugins = new System.Windows.Forms.DataGridView();
            this.cEnable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dvPlugins)).BeginInit();
            this.SuspendLayout();
            // 
            // dvPlugins
            // 
            this.dvPlugins.AllowUserToAddRows = false;
            this.dvPlugins.AllowUserToDeleteRows = false;
            this.dvPlugins.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dvPlugins.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dvPlugins.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cEnable,
            this.cName,
            this.cCore,
            this.cVersion,
            this.cOK});
            this.dvPlugins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dvPlugins.Location = new System.Drawing.Point(0, 0);
            this.dvPlugins.Name = "dvPlugins";
            this.dvPlugins.RowHeadersVisible = false;
            this.dvPlugins.RowTemplate.Height = 23;
            this.dvPlugins.Size = new System.Drawing.Size(568, 315);
            this.dvPlugins.TabIndex = 0;
            this.dvPlugins.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dvPlugins_CellContentClick);
            // 
            // cEnable
            // 
            this.cEnable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.cEnable.HeaderText = "启用";
            this.cEnable.Name = "cEnable";
            this.cEnable.Width = 40;
            // 
            // cName
            // 
            this.cName.HeaderText = "名称";
            this.cName.Name = "cName";
            this.cName.ReadOnly = true;
            // 
            // cCore
            // 
            this.cCore.HeaderText = "支持内核";
            this.cCore.Name = "cCore";
            this.cCore.ReadOnly = true;
            // 
            // cVersion
            // 
            this.cVersion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cVersion.HeaderText = "协议版本";
            this.cVersion.Name = "cVersion";
            this.cVersion.ReadOnly = true;
            this.cVersion.Width = 78;
            // 
            // cOK
            // 
            this.cOK.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cOK.HeaderText = "校验";
            this.cOK.Name = "cOK";
            this.cOK.ReadOnly = true;
            this.cOK.Width = 54;
            // 
            // PluginMgrForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 315);
            this.Controls.Add(this.dvPlugins);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PluginMgrForm";
            this.Text = "插件管理";
            ((System.ComponentModel.ISupportInitialize)(this.dvPlugins)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dvPlugins;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cEnable;
        private System.Windows.Forms.DataGridViewTextBoxColumn cName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cCore;
        private System.Windows.Forms.DataGridViewTextBoxColumn cVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn cOK;
    }
}