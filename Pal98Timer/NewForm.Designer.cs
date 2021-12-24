namespace Pal98Timer
{
    partial class NewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewForm));
            this.pnTop = new System.Windows.Forms.Panel();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblGameVersion = new System.Windows.Forms.Label();
            this.lblClose = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.mnMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnSetTitleColor = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSetBGImage = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRemoveBGImage = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBGOPG = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBGOPG0 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBGOPG25 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBGOPG50 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBGOPG75 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBGOPG100 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.pnMain = new System.Windows.Forms.Panel();
            this.pnMid = new System.Windows.Forms.Panel();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.pnActions = new System.Windows.Forms.FlowLayoutPanel();
            this.pnPS = new System.Windows.Forms.Panel();
            this.pnPoints = new System.Windows.Forms.Panel();
            this.pnBottom = new System.Windows.Forms.Panel();
            this.pnMenu = new System.Windows.Forms.FlowLayoutPanel();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnData = new System.Windows.Forms.Button();
            this.mnData = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnKeyChange = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAutoLuck = new System.Windows.Forms.ToolStripMenuItem();
            this.lblColorEgg = new System.Windows.Forms.Label();
            this.lblLuck = new System.Windows.Forms.Label();
            this.lblMore = new System.Windows.Forms.Label();
            this.pnTimers = new System.Windows.Forms.Panel();
            this.pnMT = new System.Windows.Forms.Panel();
            this.lblB = new System.Windows.Forms.Label();
            this.lblMTFront = new System.Windows.Forms.Label();
            this.lblMTBack = new System.Windows.Forms.Label();
            this.lblT2 = new System.Windows.Forms.Label();
            this.lblST = new System.Windows.Forms.Label();
            this.pnPointEnd = new System.Windows.Forms.Panel();
            this.lblPointEnd = new System.Windows.Forms.Label();
            this.tmMain = new System.Windows.Forms.Timer(this.components);
            this.cdCommon = new Pal98Timer.ColorDialogEx();
            this.pnTop.SuspendLayout();
            this.mnMain.SuspendLayout();
            this.pnMain.SuspendLayout();
            this.pnMid.SuspendLayout();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.pnPS.SuspendLayout();
            this.pnBottom.SuspendLayout();
            this.pnMenu.SuspendLayout();
            this.mnData.SuspendLayout();
            this.pnTimers.SuspendLayout();
            this.pnMT.SuspendLayout();
            this.pnPointEnd.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnTop
            // 
            this.pnTop.Controls.Add(this.lblVersion);
            this.pnTop.Controls.Add(this.lblGameVersion);
            this.pnTop.Controls.Add(this.lblClose);
            this.pnTop.Controls.Add(this.lblTitle);
            this.pnTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTop.Location = new System.Drawing.Point(0, 0);
            this.pnTop.Name = "pnTop";
            this.pnTop.Size = new System.Drawing.Size(268, 47);
            this.pnTop.TabIndex = 0;
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVersion.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblVersion.Location = new System.Drawing.Point(212, 27);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(54, 19);
            this.lblVersion.TabIndex = 3;
            this.lblVersion.Text = "0.27";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblGameVersion
            // 
            this.lblGameVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblGameVersion.AutoSize = true;
            this.lblGameVersion.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblGameVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lblGameVersion.Location = new System.Drawing.Point(3, 28);
            this.lblGameVersion.Name = "lblGameVersion";
            this.lblGameVersion.Size = new System.Drawing.Size(140, 17);
            this.lblGameVersion.TabIndex = 2;
            this.lblGameVersion.Text = "游戏版本：3.0.2014.628";
            // 
            // lblClose
            // 
            this.lblClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClose.BackColor = System.Drawing.Color.Red;
            this.lblClose.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClose.Location = new System.Drawing.Point(246, 0);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(22, 19);
            this.lblClose.TabIndex = 1;
            this.lblClose.Text = "x";
            this.lblClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.ContextMenuStrip = this.mnMain;
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.ForeColor = System.Drawing.Color.Silver;
            this.lblTitle.Location = new System.Drawing.Point(3, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(237, 23);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "仙剑98自动计时器";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mnMain
            // 
            this.mnMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSetTitleColor,
            this.btnSetBGImage,
            this.btnRemoveBGImage,
            this.btnBGOPG,
            this.toolStripSeparator1});
            this.mnMain.Name = "mnMain";
            this.mnMain.Size = new System.Drawing.Size(149, 98);
            // 
            // btnSetTitleColor
            // 
            this.btnSetTitleColor.Name = "btnSetTitleColor";
            this.btnSetTitleColor.Size = new System.Drawing.Size(148, 22);
            this.btnSetTitleColor.Text = "设置标题颜色";
            this.btnSetTitleColor.Click += new System.EventHandler(this.btnSetTitleColor_Click);
            // 
            // btnSetBGImage
            // 
            this.btnSetBGImage.Name = "btnSetBGImage";
            this.btnSetBGImage.Size = new System.Drawing.Size(148, 22);
            this.btnSetBGImage.Text = "更换背景图片";
            this.btnSetBGImage.Click += new System.EventHandler(this.btnSetBGImage_Click);
            // 
            // btnRemoveBGImage
            // 
            this.btnRemoveBGImage.Name = "btnRemoveBGImage";
            this.btnRemoveBGImage.Size = new System.Drawing.Size(148, 22);
            this.btnRemoveBGImage.Text = "删除背景图片";
            this.btnRemoveBGImage.Click += new System.EventHandler(this.btnRemoveBGImage_Click);
            // 
            // btnBGOPG
            // 
            this.btnBGOPG.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnBGOPG0,
            this.btnBGOPG25,
            this.btnBGOPG50,
            this.btnBGOPG75,
            this.btnBGOPG100});
            this.btnBGOPG.Name = "btnBGOPG";
            this.btnBGOPG.Size = new System.Drawing.Size(148, 22);
            this.btnBGOPG.Text = "背景不透明度";
            // 
            // btnBGOPG0
            // 
            this.btnBGOPG0.Name = "btnBGOPG0";
            this.btnBGOPG0.Size = new System.Drawing.Size(180, 22);
            this.btnBGOPG0.Text = "全透明";
            this.btnBGOPG0.Click += new System.EventHandler(this.btnBGOPG0_Click);
            // 
            // btnBGOPG25
            // 
            this.btnBGOPG25.Name = "btnBGOPG25";
            this.btnBGOPG25.Size = new System.Drawing.Size(180, 22);
            this.btnBGOPG25.Text = "25%";
            this.btnBGOPG25.Click += new System.EventHandler(this.btnBGOPG25_Click);
            // 
            // btnBGOPG50
            // 
            this.btnBGOPG50.Name = "btnBGOPG50";
            this.btnBGOPG50.Size = new System.Drawing.Size(180, 22);
            this.btnBGOPG50.Text = "50%";
            this.btnBGOPG50.Click += new System.EventHandler(this.btnBGOPG50_Click);
            // 
            // btnBGOPG75
            // 
            this.btnBGOPG75.Name = "btnBGOPG75";
            this.btnBGOPG75.Size = new System.Drawing.Size(180, 22);
            this.btnBGOPG75.Text = "75%";
            this.btnBGOPG75.Click += new System.EventHandler(this.btnBGOPG75_Click);
            // 
            // btnBGOPG100
            // 
            this.btnBGOPG100.Name = "btnBGOPG100";
            this.btnBGOPG100.Size = new System.Drawing.Size(180, 22);
            this.btnBGOPG100.Text = "不透明";
            this.btnBGOPG100.Click += new System.EventHandler(this.btnBGOPG100_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(145, 6);
            // 
            // pnMain
            // 
            this.pnMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnMain.BackColor = System.Drawing.Color.Black;
            this.pnMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnMain.Controls.Add(this.pnTop);
            this.pnMain.Controls.Add(this.pnMid);
            this.pnMain.Controls.Add(this.pnBottom);
            this.pnMain.ForeColor = System.Drawing.Color.White;
            this.pnMain.Location = new System.Drawing.Point(2, 2);
            this.pnMain.Name = "pnMain";
            this.pnMain.Size = new System.Drawing.Size(270, 821);
            this.pnMain.TabIndex = 1;
            // 
            // pnMid
            // 
            this.pnMid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnMid.Controls.Add(this.scMain);
            this.pnMid.Location = new System.Drawing.Point(0, 49);
            this.pnMid.Name = "pnMid";
            this.pnMid.Size = new System.Drawing.Size(268, 615);
            this.pnMid.TabIndex = 3;
            // 
            // scMain
            // 
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scMain.Location = new System.Drawing.Point(0, 0);
            this.scMain.Name = "scMain";
            this.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.pnActions);
            this.scMain.Panel1MinSize = 1;
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.pnPS);
            this.scMain.Size = new System.Drawing.Size(268, 615);
            this.scMain.SplitterDistance = 37;
            this.scMain.TabIndex = 2;
            this.scMain.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.scMain_SplitterMoved);
            // 
            // pnActions
            // 
            this.pnActions.AutoScroll = true;
            this.pnActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnActions.Location = new System.Drawing.Point(0, 0);
            this.pnActions.Name = "pnActions";
            this.pnActions.Size = new System.Drawing.Size(268, 37);
            this.pnActions.TabIndex = 0;
            // 
            // pnPS
            // 
            this.pnPS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnPS.AutoScroll = true;
            this.pnPS.Controls.Add(this.pnPoints);
            this.pnPS.Location = new System.Drawing.Point(0, 0);
            this.pnPS.Margin = new System.Windows.Forms.Padding(0);
            this.pnPS.Name = "pnPS";
            this.pnPS.Size = new System.Drawing.Size(268, 571);
            this.pnPS.TabIndex = 1;
            // 
            // pnPoints
            // 
            this.pnPoints.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnPoints.BackColor = System.Drawing.Color.Black;
            this.pnPoints.Location = new System.Drawing.Point(0, 0);
            this.pnPoints.Margin = new System.Windows.Forms.Padding(0);
            this.pnPoints.MinimumSize = new System.Drawing.Size(10, 10);
            this.pnPoints.Name = "pnPoints";
            this.pnPoints.Size = new System.Drawing.Size(268, 277);
            this.pnPoints.TabIndex = 0;
            // 
            // pnBottom
            // 
            this.pnBottom.Controls.Add(this.pnMenu);
            this.pnBottom.Controls.Add(this.lblColorEgg);
            this.pnBottom.Controls.Add(this.lblLuck);
            this.pnBottom.Controls.Add(this.lblMore);
            this.pnBottom.Controls.Add(this.pnTimers);
            this.pnBottom.Controls.Add(this.pnPointEnd);
            this.pnBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnBottom.Location = new System.Drawing.Point(0, 664);
            this.pnBottom.Name = "pnBottom";
            this.pnBottom.Size = new System.Drawing.Size(268, 155);
            this.pnBottom.TabIndex = 2;
            // 
            // pnMenu
            // 
            this.pnMenu.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnMenu.Controls.Add(this.btnReset);
            this.pnMenu.Controls.Add(this.btnData);
            this.pnMenu.Location = new System.Drawing.Point(3, 100);
            this.pnMenu.Name = "pnMenu";
            this.pnMenu.Size = new System.Drawing.Size(262, 28);
            this.pnMenu.TabIndex = 5;
            // 
            // btnReset
            // 
            this.btnReset.AutoSize = true;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnReset.Location = new System.Drawing.Point(3, 3);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(39, 22);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "重置";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnData
            // 
            this.btnData.AutoSize = true;
            this.btnData.ContextMenuStrip = this.mnData;
            this.btnData.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnData.Location = new System.Drawing.Point(48, 3);
            this.btnData.Name = "btnData";
            this.btnData.Size = new System.Drawing.Size(39, 22);
            this.btnData.TabIndex = 1;
            this.btnData.Text = "功能";
            this.btnData.Click += new System.EventHandler(this.btnData_Click);
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
            // lblColorEgg
            // 
            this.lblColorEgg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblColorEgg.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblColorEgg.ForeColor = System.Drawing.Color.Fuchsia;
            this.lblColorEgg.Location = new System.Drawing.Point(109, 131);
            this.lblColorEgg.Name = "lblColorEgg";
            this.lblColorEgg.Size = new System.Drawing.Size(156, 21);
            this.lblColorEgg.TabIndex = 4;
            this.lblColorEgg.Text = "毒门和爆炸门都惹不起啊！";
            this.lblColorEgg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLuck
            // 
            this.lblLuck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblLuck.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLuck.ForeColor = System.Drawing.Color.Yellow;
            this.lblLuck.Location = new System.Drawing.Point(3, 131);
            this.lblLuck.Name = "lblLuck";
            this.lblLuck.Size = new System.Drawing.Size(100, 21);
            this.lblLuck.TabIndex = 3;
            this.lblLuck.Text = "大象疯狂奶你";
            this.lblLuck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMore
            // 
            this.lblMore.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMore.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblMore.Location = new System.Drawing.Point(3, 74);
            this.lblMore.Name = "lblMore";
            this.lblMore.Size = new System.Drawing.Size(262, 23);
            this.lblMore.TabIndex = 2;
            this.lblMore.Text = "蜂1 蜜2 火2 血2 夜1 剑1";
            this.lblMore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnTimers
            // 
            this.pnTimers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnTimers.Controls.Add(this.pnMT);
            this.pnTimers.Controls.Add(this.lblT2);
            this.pnTimers.Controls.Add(this.lblST);
            this.pnTimers.Location = new System.Drawing.Point(-1, 22);
            this.pnTimers.Name = "pnTimers";
            this.pnTimers.Size = new System.Drawing.Size(269, 49);
            this.pnTimers.TabIndex = 1;
            // 
            // pnMT
            // 
            this.pnMT.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnMT.Controls.Add(this.lblB);
            this.pnMT.Controls.Add(this.lblMTFront);
            this.pnMT.Controls.Add(this.lblMTBack);
            this.pnMT.Location = new System.Drawing.Point(1, 16);
            this.pnMT.Name = "pnMT";
            this.pnMT.Size = new System.Drawing.Size(268, 33);
            this.pnMT.TabIndex = 3;
            // 
            // lblB
            // 
            this.lblB.BackColor = System.Drawing.Color.Red;
            this.lblB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblB.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblB.Location = new System.Drawing.Point(0, 0);
            this.lblB.Name = "lblB";
            this.lblB.Size = new System.Drawing.Size(21, 33);
            this.lblB.TabIndex = 3;
            this.lblB.Text = "C";
            this.lblB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblB.Visible = false;
            // 
            // lblMTFront
            // 
            this.lblMTFront.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMTFront.Font = new System.Drawing.Font("Consolas", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMTFront.ForeColor = System.Drawing.Color.Lime;
            this.lblMTFront.Location = new System.Drawing.Point(0, 0);
            this.lblMTFront.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblMTFront.Name = "lblMTFront";
            this.lblMTFront.Size = new System.Drawing.Size(234, 33);
            this.lblMTFront.TabIndex = 1;
            this.lblMTFront.Text = "0:00:00";
            this.lblMTFront.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lblMTFront.DoubleClick += new System.EventHandler(this.lblMTFront_DoubleClick);
            // 
            // lblMTBack
            // 
            this.lblMTBack.AutoSize = true;
            this.lblMTBack.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblMTBack.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMTBack.ForeColor = System.Drawing.Color.Lime;
            this.lblMTBack.Location = new System.Drawing.Point(234, 0);
            this.lblMTBack.Name = "lblMTBack";
            this.lblMTBack.Size = new System.Drawing.Size(34, 24);
            this.lblMTBack.TabIndex = 2;
            this.lblMTBack.Text = "00";
            this.lblMTBack.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblMTBack.DoubleClick += new System.EventHandler(this.lblMTBack_DoubleClick);
            // 
            // lblT2
            // 
            this.lblT2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblT2.Font = new System.Drawing.Font("Consolas", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblT2.ForeColor = System.Drawing.Color.Yellow;
            this.lblT2.Location = new System.Drawing.Point(0, 0);
            this.lblT2.Name = "lblT2";
            this.lblT2.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblT2.Size = new System.Drawing.Size(157, 16);
            this.lblT2.TabIndex = 2;
            this.lblT2.Text = "12.56s";
            this.lblT2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblST
            // 
            this.lblST.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblST.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblST.ForeColor = System.Drawing.Color.Lime;
            this.lblST.Location = new System.Drawing.Point(154, 0);
            this.lblST.Name = "lblST";
            this.lblST.Size = new System.Drawing.Size(116, 16);
            this.lblST.TabIndex = 0;
            this.lblST.Text = "+ 1:00:20.36";
            this.lblST.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnPointEnd
            // 
            this.pnPointEnd.Controls.Add(this.lblPointEnd);
            this.pnPointEnd.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnPointEnd.Location = new System.Drawing.Point(0, 0);
            this.pnPointEnd.Name = "pnPointEnd";
            this.pnPointEnd.Size = new System.Drawing.Size(268, 19);
            this.pnPointEnd.TabIndex = 0;
            // 
            // lblPointEnd
            // 
            this.lblPointEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPointEnd.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPointEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblPointEnd.Location = new System.Drawing.Point(0, 0);
            this.lblPointEnd.Name = "lblPointEnd";
            this.lblPointEnd.Size = new System.Drawing.Size(268, 19);
            this.lblPointEnd.TabIndex = 0;
            this.lblPointEnd.Text = "预计通关  00:12:30";
            this.lblPointEnd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tmMain
            // 
            this.tmMain.Enabled = true;
            this.tmMain.Interval = 90;
            this.tmMain.Tick += new System.EventHandler(this.tmMain_Tick);
            // 
            // cdCommon
            // 
            this.cdCommon.FullOpen = true;
            // 
            // NewForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(274, 825);
            this.Controls.Add(this.pnMain);
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NewForm";
            this.Text = "PalSpeedTimer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NewForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NewForm_FormClosed);
            this.ResizeEnd += new System.EventHandler(this.NewForm_ResizeEnd);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.NewForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.NewForm_DragEnter);
            this.Resize += new System.EventHandler(this.NewForm_Resize);
            this.pnTop.ResumeLayout(false);
            this.pnTop.PerformLayout();
            this.mnMain.ResumeLayout(false);
            this.pnMain.ResumeLayout(false);
            this.pnMid.ResumeLayout(false);
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            this.scMain.ResumeLayout(false);
            this.pnPS.ResumeLayout(false);
            this.pnBottom.ResumeLayout(false);
            this.pnMenu.ResumeLayout(false);
            this.pnMenu.PerformLayout();
            this.mnData.ResumeLayout(false);
            this.pnTimers.ResumeLayout(false);
            this.pnMT.ResumeLayout(false);
            this.pnMT.PerformLayout();
            this.pnPointEnd.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblClose;
        private System.Windows.Forms.Panel pnMain;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.Panel pnBottom;
        private System.Windows.Forms.Panel pnTimers;
        private System.Windows.Forms.Label lblMTFront;
        private System.Windows.Forms.Label lblMTBack;
        private System.Windows.Forms.Label lblColorEgg;
        private System.Windows.Forms.Label lblLuck;
        private System.Windows.Forms.ToolStripMenuItem btnKeyChange;
        private System.Windows.Forms.Panel pnPoints;
        private System.Windows.Forms.Panel pnPS;
        private System.Windows.Forms.Timer tmMain;
        public System.Windows.Forms.FlowLayoutPanel pnMenu;
        public System.Windows.Forms.ContextMenuStrip mnData;
        private System.Windows.Forms.FlowLayoutPanel pnActions;
        public System.Windows.Forms.Button btnData;
        public System.Windows.Forms.Button btnReset;
        public System.Windows.Forms.Panel pnMid;
        public System.Windows.Forms.Label lblGameVersion;
        public System.Windows.Forms.Label lblST;
        public System.Windows.Forms.Panel pnPointEnd;
        public System.Windows.Forms.Label lblT2;
        public System.Windows.Forms.Label lblMore;
        public System.Windows.Forms.Label lblPointEnd;
        public ColorDialogEx cdCommon;
        private System.Windows.Forms.ContextMenuStrip mnMain;
        private System.Windows.Forms.ToolStripMenuItem btnSetTitleColor;
        private System.Windows.Forms.ToolStripMenuItem btnSetBGImage;
        private System.Windows.Forms.ToolStripMenuItem btnRemoveBGImage;
        private System.Windows.Forms.ToolStripMenuItem btnBGOPG;
        private System.Windows.Forms.ToolStripMenuItem btnBGOPG25;
        private System.Windows.Forms.ToolStripMenuItem btnBGOPG50;
        private System.Windows.Forms.ToolStripMenuItem btnBGOPG75;
        private System.Windows.Forms.ToolStripMenuItem btnBGOPG100;
        private System.Windows.Forms.Panel pnMT;
        private System.Windows.Forms.ToolStripMenuItem btnAutoLuck;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Label lblB;
        private System.Windows.Forms.ToolStripMenuItem btnBGOPG0;
    }
}