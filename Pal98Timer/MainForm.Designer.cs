namespace Pal98Timer
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.UITimer = new System.Windows.Forms.Timer(this.components);
            this.lblTime = new System.Windows.Forms.Label();
            this.pnMain = new System.Windows.Forms.Panel();
            this.pnC = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.btnPause = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLiteCtrl = new System.Windows.Forms.ToolStripMenuItem();
            this.btnReset = new System.Windows.Forms.ToolStripMenuItem();
            this.benData = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExportCurrent = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSetCurrentToBest = new System.Windows.Forms.ToolStripMenuItem();
            this.btnKeyChange = new System.Windows.Forms.ToolStripMenuItem();
            this.btnJLSave = new System.Windows.Forms.ToolStripMenuItem();
            this.btnJLLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSwitchToLite = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSwitchToClassic = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLiveWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCloud = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCloudInit = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPostRank = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCloudSave = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCloudLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.tiHead = new Pal98Timer.TItem();
            this.lblMore = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblV = new System.Windows.Forms.Label();
            this.lblST = new System.Windows.Forms.Label();
            this.txtEye = new System.Windows.Forms.RichTextBox();
            this.pnC.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // UITimer
            // 
            this.UITimer.Interval = 50;
            this.UITimer.Tick += new System.EventHandler(this.UITimer_Tick);
            // 
            // lblTime
            // 
            this.lblTime.BackColor = System.Drawing.Color.Black;
            this.lblTime.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTime.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lblTime.Location = new System.Drawing.Point(0, 620);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(368, 35);
            this.lblTime.TabIndex = 0;
            this.lblTime.Text = "00:00:00.00";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblTime.Click += new System.EventHandler(this.lblTime_Click);
            this.lblTime.DoubleClick += new System.EventHandler(this.lblTime_DoubleClick);
            // 
            // pnMain
            // 
            this.pnMain.Location = new System.Drawing.Point(0, 0);
            this.pnMain.Name = "pnMain";
            this.pnMain.Size = new System.Drawing.Size(256, 115);
            this.pnMain.TabIndex = 2;
            // 
            // pnC
            // 
            this.pnC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnC.AutoScroll = true;
            this.pnC.Controls.Add(this.pnMain);
            this.pnC.Location = new System.Drawing.Point(0, 78);
            this.pnC.Name = "pnC";
            this.pnC.Size = new System.Drawing.Size(273, 508);
            this.pnC.TabIndex = 3;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPause,
            this.btnLiteCtrl,
            this.btnReset,
            this.benData,
            this.btnCloud});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(368, 25);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // btnPause
            // 
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(44, 21);
            this.btnPause.Text = "暂停";
            this.btnPause.ToolTipText = "F8";
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnLiteCtrl
            // 
            this.btnLiteCtrl.Name = "btnLiteCtrl";
            this.btnLiteCtrl.Size = new System.Drawing.Size(44, 21);
            this.btnLiteCtrl.Text = "开始";
            this.btnLiteCtrl.ToolTipText = "F8";
            this.btnLiteCtrl.Click += new System.EventHandler(this.btnLiteCtrl_Click);
            // 
            // btnReset
            // 
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(44, 21);
            this.btnReset.Text = "重置";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // benData
            // 
            this.benData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnExportCurrent,
            this.btnSetCurrentToBest,
            this.btnKeyChange,
            this.btnJLSave,
            this.btnJLLoad,
            this.btnSwitchToLite,
            this.btnSwitchToClassic,
            this.btnLiveWindow});
            this.benData.Name = "benData";
            this.benData.Size = new System.Drawing.Size(44, 21);
            this.benData.Text = "功能";
            this.benData.Click += new System.EventHandler(this.benData_Click);
            // 
            // btnExportCurrent
            // 
            this.btnExportCurrent.Name = "btnExportCurrent";
            this.btnExportCurrent.Size = new System.Drawing.Size(184, 22);
            this.btnExportCurrent.Text = "导出本次成绩";
            this.btnExportCurrent.Click += new System.EventHandler(this.btnExportCurrent_Click);
            // 
            // btnSetCurrentToBest
            // 
            this.btnSetCurrentToBest.Name = "btnSetCurrentToBest";
            this.btnSetCurrentToBest.Size = new System.Drawing.Size(184, 22);
            this.btnSetCurrentToBest.Text = "设置本次成绩为最佳";
            this.btnSetCurrentToBest.Click += new System.EventHandler(this.btnSetCurrentToBest_Click);
            // 
            // btnKeyChange
            // 
            this.btnKeyChange.Name = "btnKeyChange";
            this.btnKeyChange.Size = new System.Drawing.Size(184, 22);
            this.btnKeyChange.Text = "改键位";
            this.btnKeyChange.Click += new System.EventHandler(this.btnKeyChange_Click);
            // 
            // btnJLSave
            // 
            this.btnJLSave.Name = "btnJLSave";
            this.btnJLSave.Size = new System.Drawing.Size(184, 22);
            this.btnJLSave.Text = "接力-存档";
            this.btnJLSave.Click += new System.EventHandler(this.btnJLSave_Click);
            // 
            // btnJLLoad
            // 
            this.btnJLLoad.Name = "btnJLLoad";
            this.btnJLLoad.Size = new System.Drawing.Size(184, 22);
            this.btnJLLoad.Text = "接力-接盘";
            this.btnJLLoad.Click += new System.EventHandler(this.btnJLLoad_Click);
            // 
            // btnSwitchToLite
            // 
            this.btnSwitchToLite.Name = "btnSwitchToLite";
            this.btnSwitchToLite.Size = new System.Drawing.Size(184, 22);
            this.btnSwitchToLite.Text = "切换至简版";
            this.btnSwitchToLite.ToolTipText = "F6";
            this.btnSwitchToLite.Click += new System.EventHandler(this.btnSwitchToLite_Click);
            // 
            // btnSwitchToClassic
            // 
            this.btnSwitchToClassic.Name = "btnSwitchToClassic";
            this.btnSwitchToClassic.Size = new System.Drawing.Size(184, 22);
            this.btnSwitchToClassic.Text = "切换至经典";
            this.btnSwitchToClassic.ToolTipText = "F6";
            this.btnSwitchToClassic.Click += new System.EventHandler(this.btnSwitchToClassic_Click);
            // 
            // btnLiveWindow
            // 
            this.btnLiveWindow.Name = "btnLiveWindow";
            this.btnLiveWindow.Size = new System.Drawing.Size(184, 22);
            this.btnLiveWindow.Text = "直播窗口";
            this.btnLiveWindow.Click += new System.EventHandler(this.btnLiveWindow_Click);
            // 
            // btnCloud
            // 
            this.btnCloud.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCloudInit,
            this.btnPostRank,
            this.btnCloudSave,
            this.btnCloudLoad});
            this.btnCloud.Name = "btnCloud";
            this.btnCloud.Size = new System.Drawing.Size(32, 21);
            this.btnCloud.Text = "云";
            // 
            // btnCloudInit
            // 
            this.btnCloudInit.Name = "btnCloudInit";
            this.btnCloudInit.Size = new System.Drawing.Size(148, 22);
            this.btnCloudInit.Text = "验证初始化";
            this.btnCloudInit.Click += new System.EventHandler(this.btnCloudInit_Click);
            // 
            // btnPostRank
            // 
            this.btnPostRank.Name = "btnPostRank";
            this.btnPostRank.Size = new System.Drawing.Size(148, 22);
            this.btnPostRank.Text = "开启成绩上传";
            this.btnPostRank.Click += new System.EventHandler(this.btnPostRank_Click);
            // 
            // btnCloudSave
            // 
            this.btnCloudSave.Name = "btnCloudSave";
            this.btnCloudSave.Size = new System.Drawing.Size(148, 22);
            this.btnCloudSave.Text = "云存档";
            this.btnCloudSave.Click += new System.EventHandler(this.btnCloudSave_Click);
            // 
            // btnCloudLoad
            // 
            this.btnCloudLoad.Name = "btnCloudLoad";
            this.btnCloudLoad.Size = new System.Drawing.Size(148, 22);
            this.btnCloudLoad.Text = "云读档";
            this.btnCloudLoad.Click += new System.EventHandler(this.btnCloudLoad_Click);
            // 
            // tiHead
            // 
            this.tiHead.Location = new System.Drawing.Point(0, 49);
            this.tiHead.Name = "tiHead";
            this.tiHead.Size = new System.Drawing.Size(256, 32);
            this.tiHead.TabIndex = 1;
            // 
            // lblMore
            // 
            this.lblMore.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMore.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMore.Location = new System.Drawing.Point(0, 589);
            this.lblMore.Name = "lblMore";
            this.lblMore.Size = new System.Drawing.Size(367, 21);
            this.lblMore.TabIndex = 6;
            this.lblMore.Text = "增压神器";
            this.lblMore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMore.Click += new System.EventHandler(this.lblMore_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblInfo.ForeColor = System.Drawing.Color.Green;
            this.lblInfo.Location = new System.Drawing.Point(5, 25);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(311, 21);
            this.lblInfo.TabIndex = 7;
            this.lblInfo.Text = "等待游戏运行";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblV
            // 
            this.lblV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblV.Location = new System.Drawing.Point(305, 25);
            this.lblV.Name = "lblV";
            this.lblV.Size = new System.Drawing.Size(62, 21);
            this.lblV.TabIndex = 8;
            this.lblV.Text = "--";
            this.lblV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblST
            // 
            this.lblST.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblST.BackColor = System.Drawing.Color.Black;
            this.lblST.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblST.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lblST.Location = new System.Drawing.Point(0, 610);
            this.lblST.Name = "lblST";
            this.lblST.Size = new System.Drawing.Size(368, 10);
            this.lblST.TabIndex = 9;
            this.lblST.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtEye
            // 
            this.txtEye.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEye.BackColor = System.Drawing.SystemColors.Control;
            this.txtEye.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEye.Location = new System.Drawing.Point(274, 49);
            this.txtEye.Name = "txtEye";
            this.txtEye.ReadOnly = true;
            this.txtEye.Size = new System.Drawing.Size(93, 537);
            this.txtEye.TabIndex = 10;
            this.txtEye.Text = "";
            this.txtEye.WordWrap = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 655);
            this.Controls.Add(this.lblST);
            this.Controls.Add(this.lblV);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lblMore);
            this.Controls.Add(this.tiHead);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.pnC);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.txtEye);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(16, 255);
            this.Name = "MainForm";
            this.Text = "仙剑98自动计时器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            this.pnC.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer UITimer;
        private System.Windows.Forms.Label lblTime;
        private TItem tiHead;
        private System.Windows.Forms.Panel pnMain;
        private System.Windows.Forms.Panel pnC;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem benData;
        private System.Windows.Forms.ToolStripMenuItem btnExportCurrent;
        private System.Windows.Forms.ToolStripMenuItem btnPause;
        private System.Windows.Forms.ToolStripMenuItem btnReset;
        private System.Windows.Forms.Label lblMore;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.ToolStripMenuItem btnCloud;
        private System.Windows.Forms.ToolStripMenuItem btnCloudInit;
        private System.Windows.Forms.ToolStripMenuItem btnSetCurrentToBest;
        private System.Windows.Forms.Label lblV;
        private System.Windows.Forms.ToolStripMenuItem btnPostRank;
        private System.Windows.Forms.ToolStripMenuItem btnKeyChange;
        private System.Windows.Forms.Label lblST;
        private System.Windows.Forms.ToolStripMenuItem btnJLSave;
        private System.Windows.Forms.ToolStripMenuItem btnJLLoad;
        private System.Windows.Forms.ToolStripMenuItem btnCloudLoad;
        public System.Windows.Forms.ToolStripMenuItem btnCloudSave;
        private System.Windows.Forms.ToolStripMenuItem btnSwitchToLite;
        private System.Windows.Forms.ToolStripMenuItem btnSwitchToClassic;
        private System.Windows.Forms.ToolStripMenuItem btnLiteCtrl;
        private System.Windows.Forms.ToolStripMenuItem btnLiveWindow;
        private System.Windows.Forms.RichTextBox txtEye;
    }
}

