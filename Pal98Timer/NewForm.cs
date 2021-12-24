using HFrame.ENT;
using HFrame.EX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Pal98Timer
{
    public partial class NewForm : NoneBoardFormEx
    {
        public const string CurrentVersion = "0.34";
        private TimerCore core;
        private KeyboardLib _keyboardHook = null;
        private bool IsKeyInEdit = false;
        private KeyChanger kc = new KeyChanger("");
        public int CurrentKeyCode = -1;

        private int PItemHeight = 24;
        private bool IsAutoLuck = false;
        private Dictionary<string, ToolStripMenuItem> CoreBtns;

        public NewForm()
        {
            _keyboardHook = new KeyboardLib();
            _keyboardHook.InstallHook(this.OnKeyPress);
            ApplyKeyChange();
            //ColorBoard.ins.LoadFromFile();
            InitializeComponent();
            ShowKCEnable();
            /*lblTitle.ForeColor = ColorBoard.ins.TitleForeColor;
            lblGameVersion.ForeColor = ColorBoard.ins.GameVersionForeColor;
            lblVersion.ForeColor = ColorBoard.ins.VersionForeColor;
            lblPointEnd.ForeColor = ColorBoard.ins.PointEndForeColor;
            lblT2.ForeColor = ColorBoard.ins.SmallClockForeColor;
            lblST.ForeColor = ColorBoard.ins.STForeColor;
            lblMTFront.ForeColor = ColorBoard.ins.MTForeColor;
            lblMTBack.ForeColor = ColorBoard.ins.MTForeColor;
            lblMore.ForeColor = ColorBoard.ins.MoreForeColor;
            lblLuck.ForeColor = ColorBoard.ins.LuckForeColor;
            lblColorEgg.ForeColor = ColorBoard.ins.ColorEggForeColor;*/



            //InitColorBoardEvent();
            CoreBtns = new Dictionary<string, ToolStripMenuItem>();
            List<string> cores = TimerCore.GetAllCores();
            foreach (string cn in cores)
            {
                ToolStripMenuItem ti = new ToolStripMenuItem();
                ti.Text = cn;
                ti.Click += delegate(object sender, EventArgs e) {
                    if (Confirm("确定更换内核么？这将重置计时器"))
                    {
                        LoadCore(TimerCore.GetCoreIns(cn));
                    }
                };
                mnMain.Items.Add(ti);
                CoreBtns.Add(cn, ti);
            }

            BindFontChange(lblMTFront, true, lblMTFront, lblMTBack);
            BindFontChange(lblMTBack, true, lblMTFront, lblMTBack);
            BindFontChange(lblTitle, false, lblTitle);
            BindFontChange(lblGameVersion, true, lblGameVersion);
            BindFontChange(lblVersion, true, lblVersion);
            BindFontChange(lblPointEnd, true, lblPointEnd);
            BindFontChange(lblT2, true, lblT2);
            BindFontChange(lblST, true, lblST);
            BindFontChange(lblMore, true, lblMore);
            BindFontChange(lblLuck, true, lblLuck);
            BindFontChange(lblColorEgg, true, lblColorEgg);

            this.BackgroundImageLayout = ImageLayout.Stretch;

            if (File.Exists("size"))
            {
                try
                {
                    string[] szs;
                    using (FileStream fileStream = new FileStream("size", FileMode.Open))
                    {
                        using (StreamReader sr = new StreamReader(fileStream, Encoding.Default))
                        {
                            szs = sr.ReadToEnd().Split('*');
                        }
                    }
                    this.Width = int.Parse(szs[0]);
                    this.Height = int.Parse(szs[1]);
                    scMain.SplitterDistance = int.Parse(szs[2]);
                }
                catch
                { }
            }

            if (File.Exists("bg.jpg"))
            {
                try
                {
                    this.BackgroundImage = Image.FromFile("bg.jpg");
                    pnMain.BackColor = Color.FromArgb(ColorBoard.ins.OPQ, 0, 0, 0);
                    pnPoints.BackColor = Color.FromArgb(ColorBoard.ins.OPQ, 0, 0, 0);
                    /*PictureBox pb = new PictureBox();
                    pb.Size = pnMain.Size;
                    pb.Location = pnMain.Location;
                    pb.Anchor = pnMain.Anchor;
                    pb.Image= Image.FromFile("bg.jpg");
                    this.Controls.Add(pb);
                    pnMain.Parent = pb;
                    pnMain.BackColor = Color.FromArgb(50, 0, 0, 0);
                    pnPoints.BackColor = Color.FromArgb(50, 0, 0, 0);*/
                }
                catch
                { }
            }

            lblVersion.Text = CurrentVersion;
            try
            {
                SetFormMoveControl(lblTitle);
                SetFormCloseControl(lblClose);
            }
            catch { }

            try
            {
                if (File.Exists("LastCore"))
                {
                    string lc = "";
                    using (FileStream fileStream = new FileStream("LastCore", FileMode.Open))
                    {
                        using (StreamReader sr = new StreamReader(fileStream, Encoding.Default))
                        {
                            lc = sr.ReadToEnd();
                        }
                    }
                    LoadCore(TimerCore.GetCoreIns(lc));
                }
                else
                {
                    throw new Exception("LoadDefaultCore");
                }
            }
            catch(Exception ex)
            {
                LoadCore(new 仙剑98柔情());
            }

        }

        public void BindFontChange(Control ctl,bool EnableRightClick, params Control[] effctl)
        {
            ColorBoard.ins.ShowWD(ctl);
            ctl.MouseWheel += delegate (object sender, MouseEventArgs e)
            {
                if (OnCtrlDown && effctl!=null && effctl.Length>0)
                {
                    int d = 1;
                    if (e.Delta < 0)
                    {
                        d = -1;
                    }
                    foreach (Control c in effctl)
                    {
                        Font last = c.Font;
                        float newsize = last.Size + d;
                        c.Font = new Font(last.FontFamily, last.Size + d, last.Style);
                        ColorBoard.ins.GetWD(c.Name)["Size"] = newsize;
                    }
                }
            };
            ctl.MouseClick += delegate (object sender, MouseEventArgs e) {
                if (e.Button == MouseButtons.Middle)
                {
                    if (effctl != null && effctl.Length > 0)
                    {
                        FontDialogEx fd = new FontDialogEx();
                        fd.SetLocation(this.Location.X, this.Location.Y);
                        fd.Font = ctl.Font;
                        fd.ShowColor = false;
                        fd.ShowDialog(this);
                        foreach (Control c in effctl)
                        {
                            //Font last = c.Font;
                            c.Font = new Font(fd.Font.FontFamily.Name, fd.Font.Size, fd.Font.Style);
                            ColorBoard.ins.GetWD(c.Name)["FontStyle"] = (int)(fd.Font.Style);
                            ColorBoard.ins.GetWD(c.Name)["FontName"] = fd.Font.FontFamily.Name;
                            ColorBoard.ins.GetWD(c.Name)["Size"] = fd.Font.Size;
                            /*if (last.Style == FontStyle.Bold)
                            {
                                c.Font = new Font(last, FontStyle.Regular);
                                ColorBoard.ins.GetWD(c.Name)["FontStyle"] = (int)FontStyle.Regular;
                            }
                            else
                            {
                                c.Font = new Font(last, FontStyle.Bold);
                                ColorBoard.ins.GetWD(c.Name)["FontStyle"] = (int)FontStyle.Bold;
                            }*/
                        }
                    }
                }
                if (EnableRightClick)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (effctl != null && effctl.Length > 0)
                        {
                            Color ori = ctl.ForeColor;
                            cdCommon.Color = ori;
                            cdCommon.SetLocation(this.Location.X, this.Location.Y);
                            cdCommon.ShowDialog(this);
                            foreach (Control c in effctl)
                            {
                                c.ForeColor = cdCommon.Color;
                                ColorBoard.ins.GetWD(c.Name)["ForeColor"] = cdCommon.Color.ToArgb();
                                //callback?.Invoke(cdCommon.Color);
                            }
                        }
                    }
                }
            };
        }

        public void LoadCore(TimerCore core)
        {
            //Success(core.GetType().Name);
            if (this.core != null)
            {
                try
                {
                    CoreBtns[this.core.GetType().Name].Checked = false;
                }
                catch { }
                this.core.Unload();
                this.core.UnloadUI(this);
            }
            try
            {
                CoreBtns[core.GetType().Name].Checked = true;
            }
            catch { }
            try
            {
                if (File.Exists("LastCore"))
                {
                    File.Delete("LastCore");
                }
                using (FileStream fileStream = new FileStream("LastCore", FileMode.Create))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.Default))
                    {
                        streamWriter.Write(core.GetType().Name);
                        streamWriter.Flush();
                    }
                }
            }
            catch { }
            this.core = core;
            this.core.LoadCore = LoadCore;
            this.core.InitUI(this);
            this.core.OnCurrentStepChanged = delegate (int curidx) 
            {
                UI(delegate () {
                    FlushAllPoint();
                    if (IsAutoLuck)
                    {
                        lblLuck.Text = MConfig.ins.Luck(true);
                    }
                });
            };
            _ResetAll();
            core.Start();
        }
        public void _ResetAll()
        {
            core.Reset();
            pnActions.Controls.Clear();
            lblGameVersion.Text = "";
            lblT2.Text = "";
            lblST.Text = "";
            lblMore.Text = "";
            lblMTFront.Text = "0:00:00";
            lblMTBack.Text = "00";
            lblPointEnd.Text = "";
            MConfig.ins.LoadConfig();
            ShowConfigs();
            InitCheckPoints();
        }
        public void ShowConfigs()
        {
            lblTitle.Text = MConfig.ins.Title;
            lblLuck.Text = MConfig.ins.Luck(true);
            lblColorEgg.Text = MConfig.ins.ColorEgg;
            /*lblMTFront.ForeColor = MConfig.ins.MainColor;
            lblMTBack.ForeColor = MConfig.ins.MainColor;
            lblST.ForeColor = MConfig.ins.MainColor;*/
        }
        public void InitCheckPoints()
        {
            core.InitCheckPoints();
            pnPoints.Controls.Clear();
            if (core.CheckPoints != null)
            {
                foreach (CheckPoint cp in core.CheckPoints)
                {
                    NewPointItem ti = new NewPointItem(this, core);
                    ti.InitShow(cp);
                    ti.Width = pnPoints.Width;
                    ti.Visible = true;
                    ti.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                    ti.Top = ti.Height * pnPoints.Controls.Count - pnPoints.Controls.Count;
                    ti.Flush();
                    pnPoints.Controls.Add(ti);
                    PItemHeight = ti.Height;
                }
            }
            if (pnPoints.Controls.Count > 0)
            {
                pnPoints.Height = pnPoints.Controls[0].Height * core.CheckPoints.Count - core.CheckPoints.Count + 1;
            }
            //pnPS.Width = this.Width + 20;
            //pnPoints.Width = this.Width+15;
        }
        public void FlushAllPoint()
        {
            foreach (NewPointItem ti in pnPoints.Controls)
            {
                ti.Flush();
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (Confirm("确定要重置计时器么？"))
            {
                _ResetAll();
            }
        }

        public bool OnCtrlDown = false;
        public bool OnCtrlDown2 = false;
        private void ApplyKeyChange()
        {
            try
            {
                if (File.Exists("keychange.txt"))
                {
                    string keychangestr = "";
                    using (FileStream fileStream = new FileStream("keychange.txt", FileMode.Open))
                    {
                        using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default))
                        {
                            keychangestr = streamReader.ReadToEnd();
                        }
                    }
                    kc = new KeyChanger(keychangestr);
                }
            }
            catch
            { }
        }
        public void OnKeyPress(KeyboardLib.HookStruct hookStruct, out bool handle)
        {
            //btnDebug.Text = hookStruct.vkCode.ToString();
            handle = false; //预设不拦截任何键 
            if (!IsKeyInEdit)
            {
                if (kc.IsEnable)
                {
                    if (kc.KeyMap.ContainsKey(hookStruct.vkCode))
                    {
                        int flag = 0;
                        if (hookStruct.flags >= 128) flag = 2;
                        int v = kc.KeyMap[hookStruct.vkCode];
                        handle = true;
                        KeyboardLib.keybd_event(v, KeyboardLib.MapVirtualKey((uint)v, 0), flag, 0);
                    }
                    else
                    {
                        if (((Keys)(hookStruct.vkCode)) == Keys.Enter && (OnCtrlDown || OnCtrlDown2) && this.core!=null && this.core.NeedBlockCtrlEnter())
                        {
                            handle = true;
                        }
                    }
                }
                else
                {
                    if (((Keys)(hookStruct.vkCode)) == Keys.Enter && (OnCtrlDown || OnCtrlDown2) && this.core != null && this.core.NeedBlockCtrlEnter())
                    {
                        handle = true;
                    }
                }
            }
            else
            {
                CurrentKeyCode = hookStruct.vkCode;
            }
            switch ((Keys)(hookStruct.vkCode))
            {
                /*case Keys.F12:
                    df.Visible = true;
                    break;*/
                case Keys.RControlKey:
                    if (hookStruct.flags >= 128)
                    {
                        OnCtrlDown2 = false;
                    }
                    else
                    {
                        OnCtrlDown2 = true;
                    }
                    break;
                case Keys.LControlKey:
                    if (hookStruct.flags >= 128)
                    {
                        OnCtrlDown = false;
                    }
                    else
                    {
                        OnCtrlDown = true;
                    }
                    break;
                case Keys.F1:
                    if (hookStruct.flags >= 128)
                    {
                        core.OnFunctionKey(1, this);
                    }
                    break;
                case Keys.F2:
                    if (hookStruct.flags >= 128)
                    {
                        //btnReset_Click(null, null);
                        core.OnFunctionKey(2, this);
                    }
                    break;
                case Keys.F3:
                    if (hookStruct.flags >= 128)
                    {
                        btnReset_Click(null, null);
                    }
                    break;
                case Keys.F4:
                    if (hookStruct.flags >= 128)
                    {
                        core.OnFunctionKey(4, this);
                    }
                    break;
                case Keys.F5:
                    if (hookStruct.flags >= 128)
                    {
                        core.OnFunctionKey(5, this);
                    }
                    break;
                case Keys.F6:
                    if (hookStruct.flags >= 128)
                    {
                        core.OnFunctionKey(6, this);
                    }
                    break;
                case Keys.F7:
                    if (hookStruct.flags >= 128)
                    {
                        core.OnFunctionKey(7, this);
                    }
                    break;
                case Keys.F8:
                    if (hookStruct.flags >= 128)
                    {
                        core.OnFunctionKey(8, this);
                    }
                    break;
                case Keys.F9:
                    if (hookStruct.flags >= 128)
                    {
                        core.OnFunctionKey(9, this);
                    }
                    break;
                case Keys.F10:
                    if (hookStruct.flags >= 128)
                    {
                        core.OnFunctionKey(10, this);
                    }
                    break;
                case Keys.F11:
                    if (hookStruct.flags >= 128)
                    {
                        if (kc != null)
                        {
                            kc.IsEnable = !kc.IsEnable;
                            ShowKCEnable();
                        }
                    }
                    break;
                case Keys.F12:
                    if (hookStruct.flags >= 128)
                    {
                        core.OnFunctionKey(12, this);
                    }
                    break;
            }
        }
        private void ShowKCEnable()
        {
            if (kc != null && kc.IsEnable)
            {
                btnData.ForeColor = Color.Orange;
            }
            else
            {
                btnData.ForeColor = Color.White;
            }
        }

        private void btnData_Click(object sender, EventArgs e)
        {
            mnData.Show(btnData, 0, btnData.Height);
        }

        private void btnKeyChange_Click(object sender, EventArgs e)
        {
            IsKeyInEdit = true;
            KeysForm kf = new KeysForm(this);
            kf.ShowDialog(this);
            ApplyKeyChange();
            ShowKCEnable();
            IsKeyInEdit = false;
        }
        
        private void tmMain_Tick(object sender, EventArgs e)
        {
            if (core != null)
            {
                bool isc = core.IsShowC();
                if (isc != lblB.Visible) lblB.Visible = isc;
                string cryerr = core.GetCriticalError();
                if (cryerr != "")
                {
                    Error(cryerr);
                }
                lblGameVersion.Text = core.GetGameVersion();
                lblPointEnd.Text = core.GetPointEnd();
                lblT2.Text = core.GetSmallWatch();
                lblST.Text = core.GetSecondWatch();
                string[] mt = core.GetMainWatch().Split('.');
                lblMTFront.Text = mt[0];
                lblMTBack.Text = mt[1];
                lblMore.Text = core.GetMoreInfo();
                if (pnPoints.Controls.Count > 0)
                {
                    if (core.CurrentStep != -1 && core.CurrentStep < pnPoints.Controls.Count)
                    {
                        ((NewPointItem)(pnPoints.Controls[core.CurrentStep])).Flush();
                    }
                }
                string aaction = core.GetAAction();
                if (aaction != "")
                {
                    string[] aaspli = aaction.Split('|');
                    foreach (string aas in aaspli)
                    {
                        Label lbi = new Label();
                        lbi.Text = aas;
                        lbi.Padding = new Padding(5);
                        lbi.Margin = new Padding(5);
                        lbi.BackColor = RandomBGColor();
                        lbi.AutoSize = true;
                        pnActions.Controls.Add(lbi);
                        pnActions.VerticalScroll.Value = pnActions.VerticalScroll.Maximum;
                    }
                    NewForm_Resize(null, null);
                }
                if (core.IsMakeSureCurPointView)
                {
                    int h = (core.CurrentStep - 1) * PItemHeight;
                    if (h < pnPS.VerticalScroll.Minimum) h = pnPS.VerticalScroll.Minimum;
                    if (h > pnPS.VerticalScroll.Maximum) h = pnPS.VerticalScroll.Maximum;
                    if (h != pnPS.VerticalScroll.Value)
                    {
                        pnPS.VerticalScroll.Value = h;
                    }
                }
            }
            else
            {
                if (lblB.Visible) lblB.Visible = false;
            }
        }

        private Color[] bgcolors = new Color[] {
            Color.FromArgb(202,81,0),
            Color.FromArgb(200,0,0),
            Color.FromArgb(0,122,204),
            Color.FromArgb(0,200,0)
        };
        private Color RandomBGColor()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            return bgcolors[r.Next(0, bgcolors.Length)];
        }

        private void lblMTFront_DoubleClick(object sender, EventArgs e)
        {
            SetTime();
        }

        private void lblMTBack_DoubleClick(object sender, EventArgs e)
        {
            SetTime();
        }

        private void SetTime()
        {
            TSSet tss = new TSSet(delegate (TimeSpan ts) {
                core.SetTS(ts);
            });
            tss.ShowDialog(this);
            tss.Dispose();
        }

        public Button NewMenuButton(int index)
        {
            Button tbt = new Button();
            tbt.AutoSize = true;
            tbt.FlatStyle = FlatStyle.Popup;
            tbt.Size = new Size(39, 22);
            pnMenu.Controls.Add(tbt);
            pnMenu.Controls.SetChildIndex(tbt, index);
            core.AddUIC(tbt);
            return tbt;
        }

        public ContextMenuStrip NewMenu(Button btn)
        {
            ContextMenuStrip cm = new ContextMenuStrip(this.components);
            btn.ContextMenuStrip = cm;
            btn.Click += delegate (object sender, EventArgs e) {
                cm.Show(btn, 0, btn.Height);
            };
            core.AddUIC(cm);
            return cm;
        }

        public ToolStripMenuItem NewMenuItem(ContextMenuStrip cm)
        {
            ToolStripMenuItem btn = new ToolStripMenuItem();
            cm.Items.Add(btn);
            core.AddUIT(btn);
            return btn;
        }
        public ToolStripMenuItem NewMenuItem()
        {
            ToolStripMenuItem btn = new ToolStripMenuItem();
            mnData.Items.Add(btn);
            core.AddUIT(btn);
            return btn;
        }

        private void NewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void NewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ColorBoard.ins.SaveToFile();
            if (!Confirm("确定退出计时器么？"))
            {
                e.Cancel = true;
                return;
            }
        }

        private void NewForm_ResizeEnd(object sender, EventArgs e)
        {
            /*if (File.Exists("size"))
            {
                File.Delete("size");
            }*/
            using (FileStream fileStream = new FileStream("size", FileMode.Create))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.Default))
                {
                    streamWriter.Write(this.Size.Width + "*" + this.Size.Height + "*" + scMain.SplitterDistance);
                    streamWriter.Flush();
                }
            }
        }

        private void scMain_SplitterMoved(object sender, SplitterEventArgs e)
        {
            using (FileStream fileStream = new FileStream("size", FileMode.Create))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.Default))
                {
                    streamWriter.Write(this.Size.Width + "*" + this.Size.Height + "*" + scMain.SplitterDistance);
                    streamWriter.Flush();
                }
            }
        }
        
        public delegate void OnColorBoardSet(Color color);
        public void BindRightClickForeColor(Control c, OnColorBoardSet callback)
        {
            c.MouseClick += delegate (object sender, MouseEventArgs e) {
                if (e.Button == MouseButtons.Right)
                {
                    Color ori = c.ForeColor;
                    cdCommon.Color = ori;
                    cdCommon.SetLocation(this.Location.X, this.Location.Y);
                    cdCommon.ShowDialog(this);
                    callback?.Invoke(cdCommon.Color);
                }
            };
        }

        /*public void InitColorBoardEvent()
        {
            BindRightClickForeColor(lblGameVersion, delegate (Color c) {
                ColorBoard.ins.GameVersionForeColor = c;
                lblGameVersion.ForeColor = c;
            });
            BindRightClickForeColor(lblVersion, delegate (Color c) {
                ColorBoard.ins.VersionForeColor = c;
                lblVersion.ForeColor = c;
            });
            BindRightClickForeColor(lblPointEnd, delegate (Color c) {
                ColorBoard.ins.PointEndForeColor = c;
                lblPointEnd.ForeColor = c;
            });
            BindRightClickForeColor(lblT2, delegate (Color c) {
                ColorBoard.ins.SmallClockForeColor = c;
                lblT2.ForeColor = c;
            });
            BindRightClickForeColor(lblST, delegate (Color c) {
                ColorBoard.ins.STForeColor = c;
                lblST.ForeColor = c;
            });
            BindRightClickForeColor(lblMTFront, delegate (Color c) {
                ColorBoard.ins.MTForeColor = c;
                lblMTFront.ForeColor = c;
                lblMTBack.ForeColor = c;
            });
            BindRightClickForeColor(lblMTBack, delegate (Color c) {
                ColorBoard.ins.MTForeColor = c;
                lblMTFront.ForeColor = c;
                lblMTBack.ForeColor = c;
            });
            BindRightClickForeColor(lblMore, delegate (Color c) {
                ColorBoard.ins.MoreForeColor = c;
                lblMore.ForeColor = c;
            });
            BindRightClickForeColor(lblLuck, delegate (Color c) {
                ColorBoard.ins.LuckForeColor = c;
                lblLuck.ForeColor = c;
            });
            BindRightClickForeColor(lblColorEgg, delegate (Color c) {
                ColorBoard.ins.ColorEggForeColor = c;
                lblColorEgg.ForeColor = c;
            });
        }*/

        public void OnPointTitleForeColorChanged()
        {
            foreach (NewPointItem ti in pnPoints.Controls)
            {
                ti.FlushTitleForeColor();
            }
        }
        public void OnPointBestForeColorChanged()
        {
            foreach (NewPointItem ti in pnPoints.Controls)
            {
                ti.FlushBestForeColor();
            }
        }
        public void OnPointCHAForeColorChanged()
        {
            foreach (NewPointItem ti in pnPoints.Controls)
            {
                ti.FlushCHAForeColor();
            }
        }
        public void OnPointCurrentFontChanged()
        {
            foreach (NewPointItem ti in pnPoints.Controls)
            {
                ti.FlushCurrentFont();
            }
        }
        public void OnPointCHAFontChanged()
        {
            foreach (NewPointItem ti in pnPoints.Controls)
            {
                ti.FlushCHAFont();
            }
        }
        public void OnPointOPTChanged()
        {
            pnMain.BackColor = Color.FromArgb(ColorBoard.ins.OPQ, 0, 0, 0);
            pnPoints.BackColor = Color.FromArgb(ColorBoard.ins.OPQ, 0, 0, 0);
            foreach (NewPointItem ti in pnPoints.Controls)
            {
                ti.FlushOPQ();
            }
        }

        private void NewForm_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            //Success(path);
        }

        private void NewForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void btnSetTitleColor_Click(object sender, EventArgs e)
        {
            Color ori = lblTitle.ForeColor;
            cdCommon.Color = ori;
            cdCommon.SetLocation(this.Location.X, this.Location.Y);
            cdCommon.ShowDialog(this);
            ColorBoard.ins.GetWD(lblTitle.Name)["ForeColor"] = cdCommon.Color.ToArgb();
            lblTitle.ForeColor = cdCommon.Color;
        }

        private void btnSetBGImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "图片文件(*.jpg,*.png,*.jpeg,*.gif,*.jiff,*.tif)|*.jgp;*.png;*.jpeg;*.gif;*.jiff;*.tif";
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    if (File.Exists("bg.jpg"))
                    {
                        this.BackgroundImage.Dispose();
                        this.BackgroundImage = null;
                        File.Delete("bg.jpg");
                    }
                    Image res = Image.FromFile(ofd.FileName);
                    res.Save("bg.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    this.BackgroundImage = res;
                }
                catch
                {
                }
            }
        }

        private void btnRemoveBGImage_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("bg.jpg"))
                {
                    try
                    {
                        this.BackgroundImage.Dispose();
                    }
                    catch
                    { }
                    this.BackgroundImage = null;
                    File.Delete("bg.jpg");
                }
            }
            catch
            {
            }
        }

        private void NewForm_Resize(object sender, EventArgs e)
        {
            /*if (pnPS.VerticalScroll.Visible)
            {
                pnPS.Width = this.Width + 6;
            }
            else
            {
                pnPS.Width = this.Width-2;
            }
            if (pnActions.VerticalScroll.Visible)
            {
                pnActions.Width = this.Width + 6;
            }*/
        }

        private void btnBGOPG25_Click(object sender, EventArgs e)
        {
            ColorBoard.ins.OPQ = 64;
            OnPointOPTChanged();
        }

        private void btnBGOPG50_Click(object sender, EventArgs e)
        {
            ColorBoard.ins.OPQ = 128;
            OnPointOPTChanged();
        }

        private void btnBGOPG75_Click(object sender, EventArgs e)
        {
            ColorBoard.ins.OPQ = 192;
            OnPointOPTChanged();
        }

        private void btnBGOPG100_Click(object sender, EventArgs e)
        {
            ColorBoard.ins.OPQ = 255;
            OnPointOPTChanged();
        }
        
        private void btnAutoLuck_Click(object sender, EventArgs e)
        {
            btnAutoLuck.Checked = !btnAutoLuck.Checked;
            IsAutoLuck = btnAutoLuck.Checked;
        }

        private void btnBGOPG0_Click(object sender, EventArgs e)
        {
            ColorBoard.ins.OPQ = 0;
            OnPointOPTChanged();
        }
    }

    /*[Serializable]
    public class WordDisplay
    {
        public float Size;
        public string FontName;
        public int FontStyle;
        public int ForeColor;
        public int BGColor;
    }*/

    public class MConfig
    {
        private static MConfig _ins = null;
        private MConfig()
        {
            LoadConfig();
        }
        public static MConfig ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new MConfig();
                }
                return _ins;
            }
        }
        public string Title = "";
        public Color MainColor = Color.Lime;
        public Color FasterColor = Color.Lime;
        public Color SlowerColor = Color.Red;
        public string ColorEgg = "";
        public string[] Lucks = new string[] { "" };
        public void LoadConfig(string cfgpath= "config.txt")
        {
            string cfgstr = "";
            if (File.Exists(cfgpath))
            {
                using (FileStream fileStream = new FileStream(cfgpath, FileMode.Open))
                {
                    using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default))
                    {
                        cfgstr = streamReader.ReadToEnd().Replace("\r", "");
                    }
                }
            }
            else
            {
            }

            if (cfgstr != "")
            {
                string[] spli = cfgstr.Split('\n');
                if (spli.Length > 0)
                {
                    this.Title = spli[0];
                }
                /*if (spli.Length > 1)
                {
                    switch (spli[1])
                    {
                        case "1":
                            MainColor = Color.Red;
                            break;
                        case "2":
                            MainColor = Color.FromArgb(255, 128, 0);
                            break;
                        case "3":
                            MainColor = Color.Yellow;
                            break;
                        case "4":
                            MainColor = Color.FromArgb(0, 192, 0);
                            break;
                        case "5":
                            MainColor = Color.Aqua;
                            break;
                        case "6":
                            MainColor = Color.Blue;
                            break;
                        case "7":
                            MainColor = Color.Fuchsia;
                            break;
                        case "8":
                            MainColor = Color.FromArgb(224, 224, 224);
                            break;
                        case "9":
                            MainColor = Color.Gray;
                            break;
                    }
                }
                if (spli.Length > 2)
                {
                    string fastcstr = spli[2].Trim().Replace("#", "");
                    try
                    {
                        FasterColor = Color.FromArgb(Convert.ToInt32(fastcstr, 16));
                    }
                    catch { }
                }
                if (spli.Length > 3)
                {
                    string slowcstr = spli[3].Trim().Replace("#", "");
                    try
                    {
                        SlowerColor = Color.FromArgb(Convert.ToInt32(slowcstr, 16));
                    }
                    catch { }
                }*/
                if (spli.Length > 4)
                {
                    this.ColorEgg = spli[4];
                }
                if (spli.Length > 5)
                {
                    this.Lucks = spli[5].Split('|');
                }
            }
        }
        private int LuckIdx = -1;
        public string Luck(bool IsReset = false)
        {
            if (IsReset || LuckIdx < 0)
            {
                Random r = new Random(DateTime.Now.Millisecond);
                LuckIdx = r.Next(0, Lucks.Length);
            }
            return Lucks[LuckIdx];
        }
    }

    public class ColorBoard
    {
        public const string FileName = "display.txt";
        public const string CommonFontName = "微软雅黑";
        public const string CommonFontNameN = "Consolas";
        public const int CommonFontStyle = (int)FontStyle.Regular;

        private static ColorBoard _ins = null;
        private ColorBoard()
        {
            Init();
        }
        public static ColorBoard ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new ColorBoard();
                }
                return _ins;
            }
        }
        public HObj Displays;
        public void Init()
        {
            InitDefault();
            if (File.Exists(FileName))
            {
                try
                {
                    using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            HObj t = new HObj(sr.ReadToEnd());
                            Displays.SetDatas(t);
                        }
                    }
                }
                catch
                {
                }
            }
        }
        public void InitDefault()
        {
            Displays = new HObj();
            Displays["OPQ"] = 255;
            Displays["FasterColor"] = Color.FromArgb(0, 255, 0).ToArgb();
            Displays["SlowerColor"] = Color.FromArgb(255, 255, 0).ToArgb();
            Displays["EqualColor"] = Color.FromArgb(255, 255, 255).ToArgb();
            Displays["lblTitle"] = new HObj("{Size:9,FontName:'"+ CommonFontName + "',FontStyle:"+ CommonFontStyle + ",ForeColor:"+ Color.FromArgb(200, 200, 200).ToArgb() + "}");
            Displays["lblGameVersion"] = new HObj("{Size:9,FontName:'" + CommonFontName + "',FontStyle:" + CommonFontStyle + ",ForeColor:" + Color.FromArgb(0, 192, 0).ToArgb() + "}");
            Displays["lblVersion"] = new HObj("{Size:9,FontName:'" + CommonFontName + "',FontStyle:" + CommonFontStyle + ",ForeColor:" + Color.FromArgb(255, 255, 255).ToArgb() + "}");
            Displays["lblPointEnd"] = new HObj("{Size:10.5,FontName:'" + CommonFontName + "',FontStyle:" + CommonFontStyle + ",ForeColor:" + Color.FromArgb(255, 128, 0).ToArgb() + "}");
            Displays["lblT2"] = new HObj("{Size:10.5,FontName:'" + CommonFontNameN + "',FontStyle:" + CommonFontStyle + ",ForeColor:" + Color.FromArgb(255, 255, 0).ToArgb() + "}");
            Displays["lblST"] = new HObj("{Size:9,FontName:'" + CommonFontNameN + "',FontStyle:" + CommonFontStyle + ",ForeColor:" + Color.FromArgb(0, 255, 0).ToArgb() + "}");
            Displays["lblMTFront"] = new HObj("{Size:21.75,FontName:'" + CommonFontNameN + "',FontStyle:" + CommonFontStyle + ",ForeColor:" + Color.FromArgb(0, 255, 0).ToArgb() + "}");
            Displays["lblMTBack"] = new HObj("{Size:15.75,FontName:'" + CommonFontNameN + "',FontStyle:" + CommonFontStyle + ",ForeColor:" + Color.FromArgb(0, 255, 0).ToArgb() + "}");
            Displays["lblMore"] = new HObj("{Size:10.5,FontName:'" + CommonFontName + "',FontStyle:" + CommonFontStyle + ",ForeColor:" + Color.FromArgb(255, 128, 0).ToArgb() + "}");
            Displays["lblLuck"] = new HObj("{Size:9,FontName:'" + CommonFontName + "',FontStyle:" + CommonFontStyle + ",ForeColor:" + Color.FromArgb(255, 255, 0).ToArgb() + "}");
            Displays["lblColorEgg"] = new HObj("{Size:9,FontName:'" + CommonFontName + "',FontStyle:" + CommonFontStyle + ",ForeColor:" + Color.FromArgb(255, 0, 255).ToArgb() + "}");
            Displays["lblPTitle"] = new HObj("{Size:10.5,FontName:'" + CommonFontName + "',FontStyle:" + CommonFontStyle + ",ForeColor:" + Color.FromArgb(255, 255, 255).ToArgb() + "}");
            Displays["lblPBest"] = new HObj("{Size:9,FontName:'" + CommonFontNameN + "',FontStyle:" + CommonFontStyle + ",ForeColor:" + Color.FromArgb(192, 192, 192).ToArgb() + "}");
            Displays["lblPCHA"] = new HObj("{Size:9,FontName:'" + CommonFontNameN + "',FontStyle:" + CommonFontStyle + ",ForeColor:" + Color.FromArgb(255, 255, 255).ToArgb() + "}");
            Displays["lblPCurrent"] = new HObj("{Size:10.5,FontName:'" + CommonFontNameN + "',FontStyle:" + CommonFontStyle + ",ForeColor:" + Color.FromArgb(255, 255, 255).ToArgb() + "}");
        }
        /*public void InitFromJson(string json)
        {
            Displays = new HObj(json);
        }*/

        public int OPQ
        {
            get
            {
                return Displays.GetValue<int>("OPQ");
            }
            set
            {
                Displays["OPQ"] = value;
            }
        }
        public Color PointFasterForeColor
        {
            get
            {
                return Color.FromArgb(Displays.GetValue<int>("FasterColor"));
            }
            set
            {
                Displays["FasterColor"] = value.ToArgb();
            }
        }
        public Color PointSlowerForeColor
        {
            get
            {
                return Color.FromArgb(Displays.GetValue<int>("SlowerColor"));
            }
            set
            {
                Displays["SlowerColor"] = value.ToArgb();
            }
        }
        public Color PointEqualForeColor
        {
            get
            {
                return Color.FromArgb(Displays.GetValue<int>("EqualColor"));
            }
            set
            {
                Displays["EqualColor"] = value.ToArgb();
            }
        }
        public HObj GetWD(string name)
        {
            return (HObj)Displays[name];
        }
        public void ShowWD(Control c)
        {
            if (Displays.HasValue(c.Name))
            {
                HObj wd = (HObj)Displays[c.Name];
                c.Font = new Font(wd.GetValue<string>("FontName"), wd.GetValue<float>("Size"), (FontStyle)(wd.GetValue<int>("FontStyle")));
                c.ForeColor = Color.FromArgb(wd.GetValue<int>("ForeColor"));
            }
        }

        /*public Color TitleForeColor = Color.FromArgb(255, 255, 255);
        public Color GameVersionForeColor = Color.FromArgb(0, 192, 0);
        public Color VersionForeColor = Color.FromArgb(255, 255, 255);
        public Color PointEndForeColor = Color.FromArgb(255, 128, 0);
        public Color SmallClockForeColor = Color.FromArgb(255, 255, 0);
        public Color STForeColor = Color.FromArgb(0, 255, 0);
        public Color MTForeColor = Color.FromArgb(0, 255, 0);
        public Color MoreForeColor = Color.FromArgb(255, 128, 0);
        public Color LuckForeColor = Color.FromArgb(255, 255, 0);
        public Color ColorEggForeColor = Color.FromArgb(255, 0, 255);

        public Color PointTitleForeColor = Color.FromArgb(255, 255, 255);
        public Color PointBestForeColor = Color.FromArgb(192, 192, 192);
        public Color PointFasterForeColor = Color.FromArgb(0, 255, 0);
        public Color PointSlowerForeColor = Color.FromArgb(255, 0, 0);
        public Color PointEqualForeColor = Color.FromArgb(169, 169, 169);

        public int OPQ = 255;*/

        public override string ToString()
        {
            return Displays.ToJson();
        }

        public void SaveToFile()
        {
            try
            {
                if (File.Exists(FileName))
                {
                    File.Delete(FileName);
                }
                using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(this.ToString());
                        //sw.Flush();
                    }
                }
            }
            catch
            { }
        }
    }
}
