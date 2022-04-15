using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using PalCloudLib;

namespace Pal98Timer
{
    public partial class GForm : NoneBoardFormEx
    {
        public const string CurrentVersion = "0.34";
        public const string bgpath = @"bg.png";
        private TimerCore core;
        private KeyboardLib _keyboardHook = null;
        private bool IsKeyInEdit = false;
        private KeyChanger kc = new KeyChanger("");
        public int CurrentKeyCode = -1;
        private bool IsAutoLuck = false;
        private Dictionary<string, ToolStripMenuItem> CoreBtns;

        public GRender rr;
        public GRender.GBtn btnPause;
        private GRender.GBtn btnReset;
        private GRender.GBtn btnData;
        private GRender.GBtn btnCloud;
        private ContextMenuStrip cmCloud;
        private ToolStripMenuItem btnCloudInit;
        private PCloud cloud;
        public GForm():base(true)
        {
            _keyboardHook = new KeyboardLib();
            _keyboardHook.InstallHook(this.OnKeyPress);
            ApplyKeyChange();
            InitializeComponent();
            SetFormCloseControl(lblClose);
            this.FormClosing += GForm_FormClosing;
            this.FormClosed += GForm_FormClosed;

            GBoard bb = new GBoard();
            bb.Load();
            rr = new GRender(this);
            rr.OnDBClickItem = OnDBClickItem;
            rr.OnMainTimerDBClicked = OnMainTimerDBClicked;
            rr.SetGBoard(bb);
            rr.SetBG(bgpath);

            cmCloud= new ContextMenuStrip(this.components);
            btnCloudInit = new ToolStripMenuItem();
            btnCloudInit.Text = "重新验证";
            btnCloudInit.Enabled = false;
            cmCloud.Items.Add(btnCloudInit);

            btnPause = rr.AddBtn("暂停", delegate (int x, int y, GRender.GBtn btn) { UIPause(); }, 9);
            btnReset = rr.AddBtn("重置", delegate (int x, int y, GRender.GBtn btn) { btnReset_Click(null, null); }, 10);
            btnData = rr.AddBtn("功能", delegate (int x, int y, GRender.GBtn btn) { mnData.Show(lblFunArea, x, lblFunArea.Height); }, 20);
            btnCloud = rr.AddBtn("云", delegate (int x, int y, GRender.GBtn btn) { cmCloud.Show(lblFunArea, x, lblFunArea.Height); }, 30);

            CoreBtns = new Dictionary<string, ToolStripMenuItem>();
            List<string> cores = TimerCore.GetAllCores();
            foreach (string cn in cores)
            {
                ToolStripMenuItem ti = new ToolStripMenuItem();
                ti.Text = cn;
                ti.Click += delegate (object sender, EventArgs e) {
                    if (Confirm("确定更换内核么？这将重置计时器"))
                    {
                        LoadCore(TimerCore.GetCoreIns(cn,this));
                    }
                };
                mnMain.Items.Add(ti);
                CoreBtns.Add(cn, ti);
            }
            try
            {
                if (File.Exists("LastCore"))
                {
                    string lc = "";
                    Encoding charset = TimerCore.GetFileEncodeType("LastCore");
                    using (FileStream fileStream = new FileStream("LastCore", FileMode.Open))
                    {
                        using (StreamReader sr = new StreamReader(fileStream, charset))
                        {
                            lc = sr.ReadToEnd();
                        }
                    }
                    LoadCore(TimerCore.GetCoreIns(lc,this));
                }
                else
                {
                    throw new Exception("LoadDefaultCore");
                }
            }
            catch (Exception ex)
            {
                LoadCore(new 仙剑98柔情(this));
            }
            
            rr.SetVersion(CurrentVersion);

            ShowKCEnable();
        }

        private void InitCloud()
        {
            if (cloud == null)
            {
                cloud = new PCloud(this.core.CoreName, delegate (int cid)
                {
                    if (cid < 0)
                    {
                        switch (cid)
                        {
                            case -2:
                                btnCloud.Text = "正在初始化";
                                UISetBtnCloudInitEnable(false);
                                UI(delegate () {
                                    try
                                    {
                                        core?.OnCloudPending();
                                    }
                                    catch { }
                                });
                                break;
                            case -3:
                                btnCloud.Text = "云";
                                UISetBtnCloudInitEnable(true);
                                UI(delegate () {
                                    try
                                    {
                                        core?.OnCloudFail();
                                    }
                                    catch { }
                                    Error(cloud.LastError);
                                });
                                break;
                            default:
                                btnCloud.Text = "云";
                                UISetBtnCloudInitEnable(true);
                                UI(delegate () {
                                    try
                                    {
                                        core?.OnCloudFail();
                                    }
                                    catch { }
                                });
                                break;
                        }
                    }
                    else
                    {
                        btnCloud.Text = "云ID:" + cid;
                        UISetBtnCloudInitEnable(false);
                        UI(delegate () {
                            try
                            {
                                core?.OnCloudOK();
                            }
                            catch { }
                        });
                    }
                });
                cloud.OnCloudTickBefore = delegate (int NextDo)
                  {
                      switch (NextDo)
                      {
                          case 0:
                              if (!core.CustomCloudLiteData())
                              {
                                  cloud.PutLiteData(core.ForCloudLiteData());
                              }
                              break;
                          case 1:
                              if (!core.CustomCloudBigData())
                              {
                                  cloud.PutBigData(core.ForCloudBigData());
                              }
                              break;
                          case 2:
                              if (!core.CustomCloudBigData())
                              {
                                  cloud.PutBigData(core.ForCloudBigData());
                              }
                              if (!core.CustomCloudLiteData())
                              {
                                  cloud.PutLiteData(core.ForCloudLiteData());
                              }
                              break;
                      }
                  };
                cloud.Start();
                btnCloudInit.Click += delegate(object sender, EventArgs e) {
                    InitCloud();
                };
            }
            else
            {
                cloud.Reset(this.core.CoreName);
                cloud.Start();
            }
        }
        public int CloudID()
        {
            if (cloud == null) return int.MinValue;
            return cloud.CloudID;
        }
        public void PutLiteData(string data)
        {
            if (cloud == null) return;
            cloud.PutLiteData(data);
        }
        public void PutBigData(string data)
        {
            if (cloud == null) return;
            cloud.PutBigData(data);
        }

        private void UISetBtnCloudInitEnable(bool isEnable)
        {
            UI(delegate () {
                if (btnCloudInit == null) return;
                btnCloudInit.Enabled = isEnable;
            });
        }
        public void OUpload(string LocalFileName, string RemoteFileName = "")
        {
            if (cloud == null || cloud.CloudID < 0) throw new Exception("版本不匹配");
            cloud.OUpload(LocalFileName, RemoteFileName);
        }

        public void ODownload(string RemoteFileName, string LocalFileName)
        {
            if (cloud == null || cloud.CloudID < 0) throw new Exception("版本不匹配");
            cloud.ODownload(RemoteFileName, LocalFileName);
        }

        private int HandPauseCount = 0;
        public void UIPause()
        {
            if (core != null)
            {
                if (!core.IsUIPause)
                {
                    HandPauseCount++;
                }
                SetUIPause(!core.IsUIPause);
                if (HandPauseCount > 0)
                {
                    btnPause.Text = "暂停 " + HandPauseCount;
                }
                else
                {
                    btnPause.Text = "暂停 ";
                }
            }
        }
        public void SetUIPause(bool isp)
        {
            if (core != null)
            {
                core.IsUIPause = isp;
                if (core.IsUIPause)
                {
                    btnPause.Red();
                }
                else
                {
                    btnPause.White();
                }
            }
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
                this.core.UnloadUI();
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
                    using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                    {
                        streamWriter.Write(core.GetType().Name);
                        streamWriter.Flush();
                    }
                }
            }
            catch { }
            this.core = core;
            InitCloud();
            this.core.LoadCore = LoadCore;
            this.core.InitUI();
            this.core.OnCurrentStepChanged = delegate (int curidx)
            {
                if (rr != null)
                {
                    rr.ItemIdx = curidx;
                }
                if (IsAutoLuck)
                {
                    rr?.SetBL(MConfig.ins.Luck(true));
                }
            };
            _ResetAll();
            if (rr != null)
            {
                rr.IsForceRefreshAll = true;
            }
            core.Start();
        }
        public void _ResetAll()
        {
            core.UnloadPlugins();
            core.LoadPlugins();
            HandPauseCount = 0;
            btnPause.Text = "暂停";
            btnPause.White();
            core.Reset();
            rr?.ClearAllDots();
            rr?.SetGameVersion("");
            rr?.SetSubTimer("");
            rr?.SetOutTimer("");
            rr?.SetMoreInfo("");
            rr?.SetMainTimer(new TimeSpan(0));
            rr.SetWillClear("");
            MConfig.ins.LoadConfig();
            ShowConfigs();
            InitCheckPoints();
        }
        public void ShowConfigs()
        {
            rr?.SetTitle(MConfig.ins.Title);
            rr?.SetBL(MConfig.ins.Luck(true));
            rr?.SetBR(MConfig.ins.ColorEgg);
            
            /*lblMTFront.ForeColor = MConfig.ins.MainColor;
            lblMTBack.ForeColor = MConfig.ins.MainColor;
            lblST.ForeColor = MConfig.ins.MainColor;*/
        }
        public void InitCheckPoints()
        {
            core.InitCheckPointsEx();
            rr?.ClearAllItem();
            if (core.CheckPoints != null)
            {
                foreach (CheckPoint cp in core.CheckPoints)
                {
                    var item = rr?.AddItem(cp.GetNickName(), cp.Best);
                    cp.SetUIItem(item);
                    item.Cur = cp.Current;
                }
                rr.ItemIdx = -1;
                //core.CurrentStep = 0;
                core.Jump(0);
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
                        if (((Keys)(hookStruct.vkCode)) == Keys.Enter && (OnCtrlDown || OnCtrlDown2) && this.core != null && this.core.NeedBlockCtrlEnter())
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
                        core.OnFunctionKey(1);
                    }
                    break;
                case Keys.F2:
                    if (hookStruct.flags >= 128)
                    {
                        core.OnFunctionKey(2);
                    }
                    break;
                case Keys.F3:
                    if (hookStruct.flags >= 128)
                    {
                        core.OnFunctionKey(3);
                    }
                    break;
                case Keys.F4:
                    if (hookStruct.flags >= 128)
                    {
                        core.OnFunctionKey(4);
                    }
                    break;
                case Keys.F5:
                    if (hookStruct.flags >= 128)
                    {
                        core.OnFunctionKey(5);
                    }
                    break;
                case Keys.F6:
                    if (hookStruct.flags >= 128)
                    {
                        core.OnFunctionKey(6);
                    }
                    break;
                case Keys.F7:
                    if (hookStruct.flags >= 128)
                    {
                        core.OnFunctionKey(7);
                    }
                    break;
                case Keys.F8:
                    if (hookStruct.flags >= 128)
                    {
                        core.OnFunctionKey(8);
                    }
                    break;
                case Keys.F9:
                    if (hookStruct.flags >= 128)
                    {
                        UIPause();
                        core.OnFunctionKey(9);
                    }
                    break;
                case Keys.F10:
                    if (hookStruct.flags >= 128)
                    {
                        btnReset_Click(null, null);
                        core.OnFunctionKey(10);
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
                        core.OnFunctionKey(11);
                    }
                    break;
                case Keys.F12:
                    if (hookStruct.flags >= 128)
                    {
                        core.OnFunctionKey(12);
                    }
                    break;
            }
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            if (Confirm("确定要重置计时器么？"))
            {
                _ResetAll();
            }
        }
        private void ShowKCEnable()
        {
            if (kc != null && kc.IsEnable)
            {
                btnData.Orange();
            }
            else
            {
                btnData.White();
            }
        }

        private void GForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void GForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Confirm("确定退出计时器么？"))
            {
                e.Cancel = true;
                return;
            }
        }

        public void OnMainTimerDBClicked()
        {
            SetTime();
        }
        private void SetTime()
        {
            TSSet tss = new TSSet(delegate (TimeSpan ts) {
                try
                {
                    core.SetTS(ts);
                }
                catch { }
            });
            tss.ShowDialog(this);
            tss.Dispose();
        }

        public void OnDBClickItem(GRender.GItem item)
        {
            if (core != null)
            {
                if (Confirm("确定转到【"+item.Name+"】节点么？"))
                {
                    //core.CurrentStep = item.Index;
                    core.Jump(item.Index);
                }
            }
        }

        private void tmMain_Tick(object sender, EventArgs e)
        {
            if (core != null)
            {
                /*if (core.CurrentStep >= 0 && core.CurrentStep < core.CheckPoints.Count)
                {
                    core.CheckPoints[core.CurrentStep].Current = core.GetMainWatch();
                }*/
                rr.IsC= core.IsShowC();

                string cryerr = core.GetCriticalError();
                if (cryerr != "")
                {
                    Error(cryerr);
                }
                rr.SetGameVersion(core.GetGameVersion());
                rr.SetWillClear(core.GetPointEnd());
                rr.SetSubTimer(core.GetSmallWatch());
                rr.SetOutTimer(core.GetSecondWatch());
                rr.SetMainTimer(core.GetMainWatch());
                rr.IsInCheck = core.IsMainWatchStar();
                rr.SetMoreInfo(core.GetMoreInfo());
                string aaction = core.GetAAction();
                if (aaction != "")
                {
                    string[] aaspli = aaction.Split('|');
                    foreach (string aas in aaspli)
                    {
                        rr.AddDot(aas);
                    }
                }
                if (core.HasPlugin(TimerPluginBase.TimerPlugin.EPluginPosition.BL))
                {
                    rr.SetBL(core.GetPluginResult(TimerPluginBase.TimerPlugin.EPluginPosition.BL));
                }
                if (core.HasPlugin(TimerPluginBase.TimerPlugin.EPluginPosition.BR))
                {
                    rr.SetBR(core.GetPluginResult(TimerPluginBase.TimerPlugin.EPluginPosition.BR));
                }
                if (core.HasPlugin(TimerPluginBase.TimerPlugin.EPluginPosition.Title))
                {
                    rr.SetTitle(core.GetPluginResult(TimerPluginBase.TimerPlugin.EPluginPosition.Title));
                }
            }
            else
            {
                rr.IsC = false;
                rr.IsInCheck = false;
            }
            if (rr != null && rr.Draw(delegate (Rectangle? rect) {
                if (rect == null)
                {
                    Invalidate();
                }
                else
                {
                    Invalidate(rect.Value);
                }
            }))
            {
                //Invalidate();
            }
        }

        private void lblConfig_Click(object sender, EventArgs e)
        {
            mnMain.Show(lblConfig, 0, lblConfig.Height);
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

        private void btnAutoLuck_Click(object sender, EventArgs e)
        {
            btnAutoLuck.Checked = !btnAutoLuck.Checked;
            IsAutoLuck = btnAutoLuck.Checked;
        }

        private void btnChangeStyle_Click(object sender, EventArgs e)
        {
            GEditForm ef = new GEditForm(this);
            ef.Show(this);
        }

        public GRender.GBtn NewMenuButton(int index)
        {
            /*Button tbt = new Button();
            tbt.AutoSize = true;
            tbt.FlatStyle = FlatStyle.Popup;
            tbt.Size = new Size(39, 22);
            pnMenu.Controls.Add(tbt);
            pnMenu.Controls.SetChildIndex(tbt, index);
            core.AddUIC(tbt);
            return tbt;*/
            GRender.GBtn b= rr.AddBtn(null, null, index);
            core.AddUIGB(b);
            return b;
        }

        public ContextMenuStrip NewMenu(GRender.GBtn btn)
        {
            /*ContextMenuStrip cm = new ContextMenuStrip(this.components);
            btn.ContextMenuStrip = cm;
            btn.Click += delegate (object sender, EventArgs e) {
                cm.Show(btn, 0, btn.Height);
            };
            core.AddUIC(cm);
            return cm;*/
            ContextMenuStrip cm = new ContextMenuStrip(this.components);
            btn.OnClicked = delegate (int x, int y, GRender.GBtn ctl) {
                cm.Show(lblFunArea, x, lblFunArea.Height);
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
        public ToolStripMenuItem NewCloudMenuItem()
        {
            return NewMenuItem(cmCloud);
        }

        PluginMgrForm pmf = null;
        private void btnPluginManage_Click(object sender, EventArgs e)
        {
            if (pmf == null)
            {
                pmf = new PluginMgrForm();
                this.CenterChild(pmf);
                pmf.Show(this);
                pmf.FormClosed += delegate(object sender1, FormClosedEventArgs e1) {
                    pmf.Dispose();
                    pmf = null;
                };
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            AboutForm af = new AboutForm();
            this.CenterChild(af);
            af.ShowDialog(this);
            af.Dispose();
        }
    }

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
        public void LoadConfig(string cfgpath = "config.txt")
        {
            string cfgstr = "";
            if (File.Exists(cfgpath))
            {
                Encoding charset = TimerCore.GetFileEncodeType(cfgpath);
                using (FileStream fileStream = new FileStream(cfgpath, FileMode.Open))
                {
                    using (StreamReader streamReader = new StreamReader(fileStream, charset))
                    {
                        cfgstr = streamReader.ReadToEnd().Replace("\r", "");
                    }
                }
            }
            else
            {
                cfgstr = "自动计时器\r\n彩蛋\r\n大吉|小吉";
                using (FileStream fs = new FileStream(cfgpath, FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        sw.Write(cfgstr);
                    }
                }
            }

            if (cfgstr != "")
            {
                bool isOldCfg = false;
                string[] spli = cfgstr.Split('\n');
                if (spli.Length > 0)
                {
                    this.Title = spli[0];
                }
                if (spli.Length > 4)
                {
                    this.ColorEgg = spli[4];
                    isOldCfg = true;
                }
                if (spli.Length > 5)
                {
                    this.Lucks = spli[5].Split('|');
                    isOldCfg = true;
                }
                if (!isOldCfg)
                {
                    if (spli.Length > 1)
                    {
                        this.ColorEgg = spli[1];
                    }
                    if (spli.Length > 2)
                    {
                        this.Lucks = spli[2].Split('|');
                    }
                }
                else
                {
                    string updatecfgstr = this.Title + "\r\n" + this.ColorEgg + "\r\n";
                    for (int i = 0; i < this.Lucks.Length; ++i)
                    {
                        updatecfgstr += this.Lucks[i];
                        if (i < (this.Lucks.Length - 1))
                        {
                            updatecfgstr += "|";
                        }
                    }
                    File.Delete(cfgpath);
                    using (FileStream fs = new FileStream(cfgpath, FileMode.Create))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                        {
                            sw.Write(updatecfgstr);
                        }
                    }
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
}
