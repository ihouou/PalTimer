using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
//using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using HFrame.ENT;
using HFrame.EX;
using HFrame.OS;
using PalCloudLib;

namespace Pal98Timer
{
    public partial class MainForm : FormEx
    {
        public const string CurrentVersion = "0.26";

        public PCloud cloud;

        private string CMD5 = "none";
        private string GMD5 = "none";
        private string CloudID = "";

        private PTimer MT = new PTimer();
        private PTimer ST = new PTimer();
        private PTimer LT = new PTimer();
        private bool IsLiteMode = false;
        private bool IsLitePause = true;
        public IntPtr PalHandle;
        public IntPtr GameWindowHandle=IntPtr.Zero;
        private int PID=-1;
        private Process PalProcess;
        private List<CheckPoint> CheckPoints;
        private int CheckInterval = 99;
        private int CurrentStep
        {
            get {
                return _CurrentStep;
            }
            set {
                SI.ins.CurrentStep = value;
                _CurrentStep = value;
            }
        }
        private int _CurrentStep = -1;
        private HObj Data = new HObj();
        private bool _HasGameStart = false;
        private bool _IsFirstStarted = false;
        private GameObject GameObj = new GameObject();

        private bool HasUnCheated = false;
        private bool IsInUnCheat = false;

        private bool IsPause = false;
        private bool IsUIPause = false;

        private short MaxFC = 0;
        private short MaxFM = 0;
        private short MaxHCG = 0;
        private short MaxXLL = 0;
        private short MaxLQJ = 0;
        private short MaxYXY = 0;

        private int IsShowMore = 0;

        private bool IsInBattle = false;

        private bool IsDoMoreEndBattle = true;

        public Color FasterColor = Color.Green;
        public Color SlowerColor = Color.Red;

        private bool IsPostRank = false;
        private bool IsPostRankForce = false;

        DebugForm df = new DebugForm();

        private DateTime InBattleTime;
        private DateTime OutBattleTime;
        public TimeSpan BattleLong = new TimeSpan(0);

        private KeyboardLib _keyboardHook = null;
        private bool IsKeyInEdit = false;
        private KeyChanger kc = new KeyChanger("");
        public int CurrentKeyCode = -1;

        private string WillCopyRPG = "";

        private string CustomTitle = "";
        private string ColorEgg = "增压神器";

        private string[] Lucks =new string[]{
            "捅马蜂窝",
            "脚底抹油",
            "盗圣附体",
            "超人饶命",
            "七龙家养",
            "免疫夺魂",
            "凤爷饶命",
            "脆皮武士",
            "碾压三爹",
            "大象疯狂奶你",
            "我要通关",
            "" };
        private string CurrentLuck = "鼻祖附体";

        private LiveWindow LiveView = null;

        private DataServer obsdataserver = new DataServer();

        private string DefaultEyeWord = "战斗时间记录\n";

        public MainForm()
        {
            ForLuck();
            cloud = new PCloud();
            //MessageBox.Show(cloud.MyMD5);

            _keyboardHook = new KeyboardLib();
            _keyboardHook.InstallHook(this.OnKeyPress);
            ApplyKeyChange();

            //MT.SetCombine(ST);
            CMD5 = GetFileMD5(this.GetType().Assembly.Location);
            InitializeComponent();

            txtEye.SelectionAlignment = HorizontalAlignment.Right;
            txtEye.Text = DefaultEyeWord;
            //txtEye.Text = "神马\t10:24 s\n神图\t55:03 s";

            //this.BackColor = Color.White;
            //this.TransparencyKey = Color.White;

            ShowKCEnable();

            SwitchToClassic();

            btnCloudSave.Enabled = false;
            btnCloudLoad.Enabled = false;

            //btnCloudSave.Visible = false;
            //btnCloudLoad.Visible = false;

            lblV.Text = CurrentVersion;
            SI.ins.Version = CurrentVersion;

            //btnDebug.Visible = false;

            tiHead.lblTitle.Text = "记录点";
            tiHead.lblTitle.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            tiHead.lblBest.Text = "最佳";
            tiHead.lblBest.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            tiHead.lblCurrent.Text = "当前时间";
            tiHead.lblCurrent.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);

            InitUI();

            InitCheckPoints();

            InitCheckPointsUI();

            lblMore_Click(null, null);
            UITimer.Enabled = true;
            Begin();
        }

        private void InitUI()
        {
            string cfgstr = "";
            if (File.Exists("config.txt"))
            {
                using (FileStream fileStream = new FileStream("config.txt", FileMode.Open))
                {
                    using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default))
                    {
                        cfgstr = streamReader.ReadToEnd().Replace("\r","");
                    }
                }
            }
            else
            {
            }

            if (cfgstr != "")
            {
                string[] spli=cfgstr.Split('\n');
                if (spli.Length > 0)
                {
                    this.CustomTitle = spli[0];
                    this.Text = this.CustomTitle;
                }
                if (spli.Length > 1)
                {
                    switch (spli[1])
                    {
                        case "1":
                            lblTime.ForeColor = Color.Red;
                            break;
                        case "2":
                            lblTime.ForeColor = Color.FromArgb(255, 128, 0);
                            break;
                        case "3":
                            lblTime.ForeColor = Color.Yellow;
                            break;
                        case "4":
                            lblTime.ForeColor = Color.FromArgb(0, 192, 0);
                            break;
                        case "5":
                            lblTime.ForeColor = Color.Aqua;
                            break;
                        case "6":
                            lblTime.ForeColor = Color.Blue;
                            break;
                        case "7":
                            lblTime.ForeColor = Color.Fuchsia;
                            break;
                        case "8":
                            lblTime.ForeColor = Color.FromArgb(224, 224, 224);
                            break;
                        case "9":
                            lblTime.ForeColor = Color.Gray;
                            break;
                    }
                    lblST.ForeColor = lblTime.ForeColor;
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
                }
                if (spli.Length > 4)
                {
                    this.ColorEgg = spli[4];
                }
            }

            SI.ins.ColorEgg = this.ColorEgg;
        }

        private void InitCheckPointsUI()
        {
            pnMain.Controls.Clear();
            foreach (CheckPoint cp in CheckPoints)
            {
                TItem ti = new TItem(this);
                ti.InitShow(cp);
                //ti.Anchor = (AnchorStyles.Left | AnchorStyles.Right);
                ti.Width = pnMain.Width;
                ti.Top = ti.Height * pnMain.Controls.Count - pnMain.Controls.Count;
                ti.lblCurrent.BackColor = Color.White;
                ti.Visible = true;
                pnMain.Controls.Add(ti);
            }
            pnMain.Top = 0;
            if (pnMain.Controls.Count > 0)
            {
                pnMain.Height = pnMain.Controls[0].Height * CheckPoints.Count - CheckPoints.Count + 1;
            }
        }

        public void Jump(int index)
        {
            if (Confirm("确定跳转到“" + CheckPoints[index].Name + "”么？"))
            {
                _jump(index);
            }
        }

        private void _jump(int index)
        {
            CurrentStep = index;
            for (int i = 0; i < CheckPoints.Count; ++i)
            {
                if (i < index)
                {
                    CheckPoints[i].IsBegin = true;
                    CheckPoints[i].IsEnd = true;
                }
                else if (i == index)
                {
                    CheckPoints[i].IsBegin = true;
                    CheckPoints[i].IsEnd = false;
                }
                else
                {
                    CheckPoints[i].IsBegin = false;
                    CheckPoints[i].IsEnd = false;
                }
            }
        }

        private void UITimer_Tick(object sender, EventArgs e)
        {
            if (PID != -1)
            {
                SI.ins.GameVersion = "游戏版本："+PalPackVersion.ins.GetPalPackVersion(GMD5);
            }
            else
            {
                SI.ins.GameVersion = "等待游戏运行";
            }
            lblInfo.Text = SI.ins.GameVersion;
            if (IsLiteMode)
            {
                lblTime.Text = LT.ToString();
                lblST.Text = "";
                SI.ins.ST = "";
            }
            else
            {
                if (IsInUnCheat)
                {
                    SI.ins.MT = "*" + MT.ToString() + "*";
                }
                else
                {
                    if (HasUnCheated)
                    {
                        SI.ins.MT = " " + MT.ToString() + " ";
                    }
                    else
                    {
                        SI.ins.MT = "-" + MT.ToString() + "-";
                    }
                }
                lblTime.Text = SI.ins.MT;
                if (ST.CurrentTS.Ticks > 0)
                {
                    lblST.Text = "+ " + ST.ToString();
                    SI.ins.ST = "+ " + ST.ToString();
                }
                else
                {
                    lblST.Text = "";
                    SI.ins.ST = "";
                }
                if (CurrentStep >= 0)
                {
                    if (CurrentStep > 0)
                    {
                        ((TItem)(pnMain.Controls[CurrentStep - 1])).Flush();
                    }
                    if (CurrentStep < pnMain.Controls.Count)
                    {
                        ((TItem)(pnMain.Controls[CurrentStep])).Flush();
                    }
                }
                if (df.Visible)
                {
                    df.ShowData(GameObj);
                }


                SI.ins.BattleLong = BattleLong.TotalSeconds.ToString("F2") + "s";
                SI.ins.FC = MaxFC.ToString();
                SI.ins.FM = MaxFM.ToString();
                SI.ins.HCG = MaxHCG.ToString();
                SI.ins.XLL = MaxXLL.ToString();
                SI.ins.YXY = MaxYXY.ToString();
                SI.ins.LQJ = MaxLQJ.ToString();

                SI.ins.MoreInfo = SI.ins.BattleLong + " " + "蜂" + MaxFC + "蜜" + MaxFM + "火" + MaxHCG + "血" + MaxXLL + "夜" + MaxYXY + "剑" + MaxLQJ;

                if (IsShowMore == 1)
                {
                    lblMore.Text = SI.ins.MoreInfo;
                }
                else if (IsShowMore == 2)
                {
                    lblMore.Text = CurrentLuck;
                }
                else
                {
                    lblMore.Text = this.ColorEgg;
                }

                if (WillAppendNamedBattle != "")
                {
                    txtEye.AppendText(WillAppendNamedBattle + "\n");
                    WillAppendNamedBattle = "";
                }

                MakeSureCurrentView();
            }


        }

        private void ForLuck()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            CurrentLuck=Lucks[r.Next(0, Lucks.Length - 1)];
            SI.ins.Luck = CurrentLuck;
        }

        private void JudgePause()
        {
            if (IsUIPause)
            {
                IsPause = true;
                return;
            }
            IntPtr hWnd = User32.GetForegroundWindow();    //获取活动窗口句柄  
            int calcID = 0;    //进程ID  
            int calcTD = 0;    //线程ID  
            calcTD = User32.GetWindowThreadProcessId(hWnd, out calcID);
            if (calcID == PID)
            {
                IsPause = false;
            }
            else
            {
                IsPause = true;
            }
        }

        private void Begin()
        {
            Run(delegate() {
                while (true)
                {
                    if (GetPalHandle())
                    {
                        CopyRPGIfHas();

                        JudgePause();
                        try
                        { 
                            FlushGameObject();
                        }
                        catch(Exception ex)
                        {
                        }


                        try
                        {
                            if (GameObj.Enemies.Count > 0)
                            {
                                if (!IsInBattle)
                                {
                                    BattleBegin();
                                }
                                IsInBattle = true;
                                IsDoMoreEndBattle = true;
                                Battling();
                            }
                            else
                            {
                                if (!IsDoMoreEndBattle)
                                {
                                    BattleEndMore();
                                    IsDoMoreEndBattle = true;
                                }
                                if (IsInBattle)
                                {
                                    BattleEnd();
                                    IsDoMoreEndBattle = false;
                                }
                                IsInBattle = false;
                            }
                        }
                        catch { }

                        if (HasStartGame())
                        {
                            ST.Stop();
                            if (!_IsFirstStarted)
                            {
                                _IsFirstStarted = true;
                            }
                            if (!HasUnCheated)
                            {
                                if (!IsInUnCheat)
                                {
                                    CheckCheatBegin();
                                    CheckCheatEnd();
                                }
                                else
                                {
                                    CheckCheatEnd();
                                }
                            }

                            if (IsInUnCheat)
                            {
                                MT.Stop();
                            }
                            else
                            {
                                MT.Start();
                                Checking();
                            }
                        }
                        else
                        {
                            MT.Stop();
                        }
                    }
                    else
                    {
                        _HasGameStart = false;
                        MT.Stop();

                        if (_IsFirstStarted)
                        {
                            ST.Start();
                        }
                    }
                    Thread.Sleep(CheckInterval);
                }
            });
        }

        private void CalcPalMD5()
        {
            try
            {
                string dllmd5 = GetFileMD5(GetGameFilePath("Pal.dll"));
                string datamd5 = GetFileMD5(GetGameFilePath("DATA.MKF"));
                string sssmd5 = GetFileMD5(GetGameFilePath("SSS.MKF"));
                GMD5 = dllmd5 + "_" + datamd5 + "_" + sssmd5;
            }
            catch
            {
                GMD5 = "none";
            } 
        }

        private string GetGameFilePath(string fn)
        {
            string palpath = PalProcess.MainModule.FileName;
            string[] spli = palpath.Split('\\');
            spli[spli.Length - 1] = fn;
            palpath = "";
            foreach (string s in spli)
            {
                palpath += s + "\\";
            }
            if (palpath != "")
            {
                palpath = palpath.Substring(0, palpath.Length - 1);
            }
            return palpath;
        }

        private bool GetPalHandle()
        {
            Process[] res = Process.GetProcessesByName("Pal");
            if (res.Length > 0)
            {
                if (PID == -1)
                {
                    //PalHandle = res[0].Handle;
                    PalProcess = res[0];
                    //GameWindowHandle = User32.FindWindow(null, "仙剑奇侠传 WIN-95 版 [补丁版本：3.0.2014.628]");
                    GameWindowHandle = res[0].MainWindowHandle;
                    PID = PalProcess.Id;
                    PalHandle = new IntPtr(Kernel32.OpenProcess(0x1F0FFF, false, PID));

                    CalcPalMD5();

                    return true;
                }
                else
                {
                    if (PID == res[0].Id)
                    {
                        if (GMD5 == "none")
                        {
                            CalcPalMD5();
                        }
                        return true;
                    }
                    else
                    {
                        PalHandle = IntPtr.Zero;
                        GameWindowHandle = IntPtr.Zero;
                        PalProcess = null;
                        PID = -1;
                        GMD5 = "none";
                        return false;
                    }
                }
            }
            else
            {
                PalHandle = IntPtr.Zero;
                GameWindowHandle = IntPtr.Zero;
                PalProcess = null;
                PID = -1;
                GMD5 = "none";
                return false;
            }
        }

        private void Checking()
        {
            if (CurrentStep < 0 && CheckPoints.Count>0)
            {
                CurrentStep = 0;
                CheckPoints[0].IsBegin = true;
            }

            if (CurrentStep < CheckPoints.Count)
            {
                CheckPoints[CurrentStep].Current = MT.CurrentTSOnly;
                if (CheckPoints[CurrentStep].Check())
                {
                    CheckPoints[CurrentStep].Current = new TimeSpan(MT.CurrentTSOnly.Ticks);
                    CheckPoints[CurrentStep].IsEnd = true;
                    CurrentStep++;
                    if (CurrentStep >= CheckPoints.Count)
                    {
                        OnLastCheckPointEnd();
                    }
                    else
                    {
                        CheckPoints[CurrentStep].IsBegin = true;
                    }
                    PostCloudRank();
                }
            }
            else
            {
                OnLastCheckPointEnd();
            }
        }

        private void MakeSureCurrentView()
        {
            int h = (CurrentStep - 1) * 24;
            if (h < pnC.VerticalScroll.Minimum) h = pnC.VerticalScroll.Minimum;
            if (h > pnC.VerticalScroll.Maximum) h = pnC.VerticalScroll.Maximum;
            if (h != pnC.VerticalScroll.Value)
            {
                pnC.VerticalScroll.Value = h;
            }
        }

        private void OnLastCheckPointEnd()
        {
            MT.Stop();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_keyboardHook != null) _keyboardHook.UninstallHook();
            Environment.Exit(0);
        }

        private void FlushGameObject()
        {
            GameObj.Flush(PalHandle,PID);
        }

        private bool HasStartGame()
        {
            if (!_HasGameStart)
            {
                if (GameObj.Area != 0)
                {
                    _HasGameStart = true;
                    if (IsPause)
                    {
                        return false;
                    }
                    PostCloudRank();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (IsPause)
                {
                    return false;
                }
                return true;
            }
        }

        private bool PositionCheck(params int[][] PositionList)
        {
            foreach (int[] p in PositionList)
            {
                if (GameObj.Area == p[0] && GameObj.X == p[1] && GameObj.Y == p[2])
                {
                    return true;
                }
            }
            return false;
        }

        private bool PositionAroundCheck(int Area,int X,int Y,int r=1)
        {
            if (GameObj.Area == Area)
            {
                if (GameObj.X >= (X - 16 * r) && GameObj.X <= (X + 16 * r)
                    && GameObj.Y >= (Y - 8 * r) && GameObj.Y <= (Y + 8 * r))
                {
                    return true;
                }
            }
            return false;
        }

        private void CheckCheatBegin()
        {
            if (PositionCheck(new int[3] { 177, 1088, 608 }, new int[3] { 177, 1120, 608 }, new int[3] { 177, 1120, 592 }))
            {
                IsInUnCheat = true;
            }
        }

        private void CheckCheatEnd()
        {
            if (GameObj.GetItemCount(0x123) > 0)
            {
                IsInUnCheat = false;
                HasUnCheated = true;
            }
        }

        private Dictionary<string, CheckPointNewer> Best = null;
        private CheckPointNewer GetBest(string Name)
        {
            if (Best == null) return new CheckPointNewer() {
                Name = Name,
                NickName = "",
                BestTS = new TimeSpan(0, 0, 0, 0, 0)
            };
            if (Best.ContainsKey(Name))
            {
                return Best[Name];
            }
            else
            {
                return new CheckPointNewer()
                {
                    Name = Name,
                    NickName = "",
                    BestTS = new TimeSpan(0, 0, 0, 0, 0)
                };
            }
        }
        public static TimeSpan ConvertTimeSpan(string str)
        {
            try
            {
                string[] spli = str.Replace(".",":").Split(':');
                return new TimeSpan(0, int.Parse(spli[0]), int.Parse(spli[1]), int.Parse(spli[2]), (spli.Length > 3 ? (int.Parse(spli[3])) : 0));
            }
            catch
            {
                return new TimeSpan(0, 0, 0, 0, 0); 
            }
        }
        private void InitCheckPoints()
        {
            if (File.Exists("best.txt"))
            {
                string beststr = "";
                using (FileStream fileStream = new FileStream("best.txt", FileMode.Open))
                {
                    using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default))
                    {
                        beststr = streamReader.ReadToEnd();
                    }
                }
                List<object> tmp = (new HObj(beststr)).GetValue<HObj>("CheckPoints").ToList();
                Best = new Dictionary<string, CheckPointNewer>();
                foreach (object o in tmp)
                {
                    HObj co = o as HObj;
                    string name = co.GetValue<string>("name");
                    string nickname = "";
                    if (co.HasValue("des"))
                    {
                        nickname = co.GetValue<string>("des");
                    }
                    if (Best.ContainsKey(name))
                    {
                        Best[name] = new CheckPointNewer()
                        {
                            Name = name,
                            NickName = nickname,
                            BestTS = ConvertTimeSpan(co.GetValue<string>("time"))
                        };
                    }
                    else
                    {
                        Best.Add(name, new CheckPointNewer()
                        {
                            Name = name,
                            NickName = nickname,
                            BestTS = ConvertTimeSpan(co.GetValue<string>("time"))
                        });
                    }
                }
            }
            else
            {
                Best = null;
            }


            CurrentStep = -1;
            Data = new HObj();
            CheckPoints = new List<CheckPoint>();
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("见石碑"))
            {
                Check = delegate()
                {
                    if (PositionCheck(new int[3] { 19, 1696, 384 }, new int[3] { 19, 1680, 376 })
                        || PositionAroundCheck(19, 1696, 384))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("学功夫"))
            {
                Check = delegate()
                {
                    if (GameObj.AreaBGM == 86)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("上船"))
            {
                Check = delegate()
                {
                    //if (PositionCheck(new int[3] { 6, 1072, 1080 }, new int[3] { 6, 1088, 1088 }))
                    if (PositionAroundCheck(6, 1072, 1080,2))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("出林家堡"))
            {
                Check = delegate()
                {
                    if (PositionAroundCheck(40, 1456, 872, 5))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("出隐龙窟"))
            {
                Check = delegate()
                {
                    if (PositionAroundCheck(49, 304, 1560, 5))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("生化危机"))
            {
                Check = delegate()
                {
                    //if (PositionCheck(new int[3] { 62, 1152, 1264 }))
                    if (PositionAroundCheck(62, 1152, 1264,2))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过鬼将军"))
            {
                Check = delegate()
                {
                    if (GameObj.BossID == 75 && GameObj.BattleTotalBlood <= 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过赤鬼王"))
            {
                Check = delegate()
                {
                    if (GameObj.BossID == 76 && GameObj.BattleTotalBlood <= 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("进扬州"))
            {
                Check = delegate()
                {
                    //if (PositionCheck(new int[3] { 83, 320, 1056 }))
                    if(PositionAroundCheck(80,256,1344,5))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("出扬州"))
            {
                Check = delegate()
                {
                    //if (PositionCheck(new int[3] { 85, 1136, 536 }))
                    if (PositionAroundCheck(106, 64, 960,5))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("出麻烦洞"))
            {
                Check = delegate()
                {
                    //if (PositionCheck(new int[3] { 107, 1520, 408 }))
                    if (PositionAroundCheck(107, 1520, 408,5))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("进京城"))
            {
                Check = delegate()
                {
                    //if (PositionCheck(new int[3] { 101, 256, 224 }))
                    if (PositionAroundCheck(101, 272, 216, 2))
                    {
                        return true;
                    }
                    return false;
                }
            });
            //Data["lazhu"] = false;
            Data["caiyi"] = false;
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过彩依"))
            {
                Check = delegate()
                {
                    /*if (!Data.GetValue<bool>("lazhu"))
                    {
                        if (GameObj.GetItemCount(0x51) > 0)
                        {
                            Data["lazhu"] = true;
                        }
                    }
                    else
                    {
                        if (GameObj.GetItemCount(0x51) <= 0)
                        {
                            Data["lazhu"] = false;
                            return true;
                        }
                    }
                    return false;*/
                    if (!Data.GetValue<bool>("caiyi"))
                    {
                        if (GameObj.BossID == 71)
                        {
                            Data["caiyi"] = true;
                        }
                    }
                    else
                    {
                        if (GameObj.BossID != 71 || (GameObj.BossID == 71 && GameObj.BattleTotalBlood <= 0))
                        {
                            Data["caiyi"] = false;
                            return true;
                        }
                    }
                    return false;
                }
            });
            /*CheckPoints.Add(new CheckPoint(CheckPoints.Count)
            {
                Name = "蝶恋",
                Best = GetBest("蝶恋"),
                Check = delegate()
                {
                    if (GameObj.AreaBGM == 13)
                    {
                        return true;
                    }
                    return false;
                }
            });*/
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("进锁妖塔"))
            {
                Check = delegate()
                {
                    //if (PositionCheck(new int[3] { 147, 1024, 448 }))
                    /*if (PositionAroundCheck(147, 1024, 448, 2))
                    {
                        return true;
                    }*/
                    if (PositionAroundCheck(164, 1024, 992, 4) || GameObj.Area == 165 || PositionAroundCheck(147, 1024, 448, 2))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("剑柱"))
            {
                Check = delegate()
                {
                    //if (PositionCheck(new int[3] { 147, 1024, 448 }))
                    if (PositionAroundCheck(146, 304, 1048, 3))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("拆塔"))
            {
                Check = delegate()
                {
                    if (GameObj.BossID == 144 && GameObj.BattleTotalBlood <= 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过凤凰"))
            {
                Check = delegate()
                {
                    if (GameObj.BossID == 67 && GameObj.BattleTotalBlood <= 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
            /*CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("进火洞"))
            {
                Check = delegate()
                {
                    //if (PositionCheck(new int[3] { 213, 112, 1496 }))
                    if (PositionAroundCheck(213, 112, 1496, 2))
                    {
                        return true;
                    }
                    return false;
                }
            });*/
            /*CheckPoints.Add(new CheckPoint(CheckPoints.Count)
            {
                Name = "过火麒麟",
                Best = GetBest("过火麒麟"),
                Check = delegate()
                {
                    if (GameObj.BossID == 66 && GameObj.BattleTotalBlood <= 0)
                    {
                        return true;
                    }
                    return false;
                }
            });*/
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("进十年前"))
            {
                Check = delegate()
                {
                    if (PositionAroundCheck(247, 1408, 1584, 5))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("水灵珠"))
            {
                Check = delegate()
                {
                    if (GameObj.GetItemCount(0x109) > 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
            /*CheckPoints.Add(new CheckPoint(CheckPoints.Count)
            {
                Name = "傀儡蛊OK",
                Best = GetBest("傀儡蛊OK"),
                Check = delegate()
                {
                    if (GameObj.GetItemCount(0x98) >= 36)
                    {
                        return true;
                    }
                    return false;
                }
            });*/
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("祈雨"))
            {
                Check = delegate()
                {
                    if (PositionCheck(new int[3] { 228, 992, 928 }))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("通关"))
            {
                Check = delegate()
                {
                    if (GameObj.BossID == 149 && GameObj.BattleTotalBlood <= 0)
                    {
                        return true;
                    }
                    return false;
                }
            });

            SI.ins.cps = this.CheckPoints;
        }

        public string GetRStr()
        {
            HObj exdata = new HObj();
            exdata["Current"] = MT.ToString();
            exdata["Idle"] = ST.ToString();
            exdata["Lite"] = LT.ToString();
            exdata["Step"] = CurrentStep;
            exdata["BeeHouse"] = MaxFC;
            exdata["BeeSheet"] = MaxFM;
            exdata["FireWorm"] = MaxHCG;
            exdata["DragonSword"] = MaxLQJ;
            exdata["BloodLink"] = MaxXLL;
            exdata["NightCloth"] = MaxYXY;
            exdata["OSTime"] = DateTime.Now.Ticks.ToString();
            exdata["GMD5"] = GMD5;
            HObj cps = new HObj();
            foreach (CheckPoint c in CheckPoints)
            {
                HObj cur = new HObj();
                cur["name"] = c.Name;
                cur["des"] = c.NickName;
                cur["time"] = TItem.TimeSpanToString(c.Current);
                cps.Add(cur);
            }
            exdata["CheckPoints"] = cps;

            return exdata.ToJson();
        }

        private void btnExportCurrent_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            string filename = now.ToString("yyyyMMddHHmmss");
            string ext = GetRStr();

            try
            {
                using (FileStream fileStream = new FileStream(filename + ".txt", FileMode.Append))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.Default))
                    {
                        streamWriter.Write(ext);
                        streamWriter.Flush();
                    }
                }
                MessageBox.Show(this, "已将此次成绩保存至" + filename + ".txt", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, "保存失败："+ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            SetUIPause(!IsUIPause);
        }

        private void SetUIPause(bool ispause)
        {
            IsUIPause = ispause;
            if (IsUIPause)
            {
                btnPause.ForeColor = Color.Red;
            }
            else
            {
                btnPause.ForeColor = Color.Black;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (Confirm("确定要重置计时器么？"))
            {
                if (IsLiteMode)
                {
                    SetLitePause(true);
                    LT.SetTS(new TimeSpan(0));
                }
                else
                {
                    ResetAll();
                }
            }
        }

        private void ResetAll()
        {
            ForLuck();
            HasUnCheated = false;
            IsInUnCheat = false;
            ST.Stop();
            _IsFirstStarted = false;
            MaxFC = 0;
            MaxFM = 0;
            MaxHCG = 0;
            MaxLQJ = 0;
            MaxXLL = 0;
            MaxYXY = 0;
            BattleLong = new TimeSpan(0);
            InitCheckPoints();
            InitCheckPointsUI();
            MT.Reset();
            ST.Reset();
            WillAppendNamedBattle = "";
            txtEye.Text = DefaultEyeWord;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Confirm("确定退出计时器么？"))
            {
                e.Cancel = true;
                return;
            }
        }

        private void lblMore_Click(object sender, EventArgs e)
        {
            IsShowMore++;
            if (IsShowMore==1)
            {
                lblMore.ForeColor = Color.Blue;
            }
            else if (IsShowMore == 2)
            {
                lblMore.ForeColor = Color.Red;
            }
            else
            {
                IsShowMore = 0;
                lblMore.ForeColor = Color.Black;
            }
        }

        private BattleItemWatch biw = new BattleItemWatch();
        private string CurrentNamedBattle = "";
        private string WillAppendNamedBattle = "";
        private void BattleBegin()
        {
            BattleLong = new TimeSpan(0);
            InBattleTime = DateTime.Now;
            biw = new BattleItemWatch();
            if (CurrentStep <= 5)
            {
                //战斗前记录下个数
                biw.Insert(0x73, GameObj.GetItemCount(0x73));//蜂
                biw.Insert(0x83, GameObj.GetItemCount(0x83));//蜜
                biw.Insert(0x8F, GameObj.GetItemCount(0x8F));//火
            }
            biw.Insert(0xB8, GameObj.GetItemCount(0xB8));//龙泉剑
            biw.Insert(0xA2, GameObj.GetItemCount(0xA2));//血玲珑
            biw.Insert(0xD4, GameObj.GetItemCount(0xD4));//夜行衣
            CurrentNamedBattle = GameObj.GetNamedBattle();
        }

        private void Battling()
        {
            BattleLong = DateTime.Now - InBattleTime;
            /*if (CurrentStep <= 5)
            {
                //战斗中每隔100毫秒算下差
                biw.SetCount(GameObj);
            }*/
            biw.SetCount(GameObj);
        }

        private void BattleEnd()
        {
            OutBattleTime = DateTime.Now;
            BattleLong = OutBattleTime - InBattleTime;
            /*if (CurrentStep <= 5)
            {
                //战斗结束，强制再算差
                biw.SetCount(GameObj);
            }*/
            biw.SetCount(GameObj);

            if (CurrentNamedBattle != "")
            {
                WillAppendNamedBattle = CurrentNamedBattle + "\t" + BattleLong.TotalSeconds.ToString("F2") + "s";
                CurrentNamedBattle = "";
            }
        }

        private void BattleEndMore()
        {
            //战斗结束，强制再算差
            biw.SetCount(GameObj);
            if (CurrentStep <= 5)
            {
                //将算出来的差加入显示
                MaxFC += biw.GettedCount(0x73);
                MaxFM += biw.GettedCount(0x83);
                MaxHCG += biw.GettedCount(0x8F);
            }
            MaxLQJ += biw.GettedCount(0xB8);
            MaxXLL += biw.GettedCount(0xA2);
            MaxYXY += biw.GettedCount(0xD4);
        }

        public void SetMainClock(TimeSpan ts)
        {
            if (IsLiteMode)
            {
                LT.SetTS(ts);
            }
            else
            {
                MT.SetTS(ts);
            }
        }

        private void lblTime_DoubleClick(object sender, EventArgs e)
        {
            if (IsLiteMode)
            {
                if (IsLitePause)
                {
                    TSSet tss = new TSSet(this);
                    tss.ShowDialog(this);
                }
                else
                {
                    SetLitePause(true);
                    TSSet tss = new TSSet(this);
                    tss.ShowDialog(this);
                    SetLitePause(false);
                }
            }
            else
            {
                if (IsUIPause)
                {
                    TSSet tss = new TSSet(this);
                    tss.ShowDialog(this);
                }
                else
                {
                    SetUIPause(true);
                    TSSet tss = new TSSet(this);
                    tss.ShowDialog(this);
                    SetUIPause(false);
                }
            }
        }

        public static string GetFileMD5(string fileName)
        {
            string res = "none";
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open,FileAccess.Read,FileShare.Read);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                res = sb.ToString().ToUpper();
            }
            catch (Exception ex)
            {
                res += ":" + ex.Message;
            }
            return res;
        }

        private void btnCloudInit_Click(object sender, EventArgs e)
        {
            InitCloud();
        }

        private void BeginCloud()
        {
            Run(delegate() {
                while (CloudID != "")
                {
                    try
                    {
                        PCloudQS qs = new PCloudQS();
                        qs.Add("do", "hb");
                        qs.Add("id", CloudID);
                        qs.Add("t", DateTime.Now.Ticks.ToString());
                        qs.Add("m", MT.CurrentTS.Ticks.ToString());
                        qs.Add("s", ST.CurrentTS.Ticks.ToString());
                        qs.Add("l", LT.CurrentTS.Ticks.ToString());
                        string ret = cloud.CloudGet(qs);
                        if (!IsPostRankForce)
                        {
                            if (ret == "F")
                            {
                                IsPostRankForce = true;
                                PostCloudRank();
                            }
                        }
                        else
                        {
                            if (ret == "C")
                            {
                                IsPostRankForce = false;
                            }
                        }
                    }
                    catch { }
                    Thread.Sleep(1000);
                }
            });
        }

        private void PostCloudRank()
        {
            if (CloudID != "" && (IsPostRankForce || IsPostRank))
            {
                Run(delegate()
                {
                    string ext = GetRStr();
                    try
                    {
                        PCloudQS qs = new PCloudQS();
                        qs.Add("do", "cp");
                        qs.Add("id", CloudID);
                        qs.Add("t", DateTime.Now.Ticks.ToString());
                        qs.Add("m", MT.CurrentTS.Ticks.ToString());
                        qs.Add("s", ST.CurrentTS.Ticks.ToString());
                        qs.Add("l", LT.CurrentTS.Ticks.ToString());
                        cloud.CloudPost(qs, "data=" + ext.Replace("\"", "'") + "");
                    }
                    catch { }
                });
            }
        }

        private void btnSetCurrentToBest_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            string filename = now.ToString("yyyyMMddHHmmss");
            string ext = GetRStr();
            try
            {
                if (File.Exists("best.txt"))
                {
                    File.Move("best.txt", "best" + filename + ".txt");
                }
                using (FileStream fileStream = new FileStream("best.txt", FileMode.Append))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.Default))
                    {
                        streamWriter.Write(ext);
                        streamWriter.Flush();
                    }
                }
                if (Confirm("保存成功，确定要重置计时器么？"))
                {
                    ResetAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "保存失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitCloud();
        }

        private void InitCloud()
        {
            btnCloud.Enabled = false;
            btnCloud.Text = "初始化中...";
            Run(delegate()
            {
                string initres = "";
                int tmp = 0;
                try
                {
                    cloud.Init();

                    PCloudQS qs = new PCloudQS();
                    qs.Add("do", "fid");
                    qs.Add("t", DateTime.Now.Ticks.ToString());
                    qs.Add("m", MT.CurrentTS.Ticks.ToString());
                    qs.Add("s", ST.CurrentTS.Ticks.ToString());
                    qs.Add("l", LT.CurrentTS.Ticks.ToString());
                    initres = cloud.CloudGet(qs);
                }
                catch (Exception ex)
                {
                    initres = ex.Message;
                }
                if (!int.TryParse(initres, out tmp))
                {
                    CloudID = "";
                    SI.ins.CloudID = "云端未认证";
                    UI(delegate()
                    {
                        MessageBox.Show("初始化云端失败：" + initres, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnCloud.Enabled = true;
                        btnCloudInit.Enabled = true;
                        btnCloud.Text = "云";
                    });
                }
                else
                {
                    CloudID = initres;
                    SI.ins.CloudID = "云ID: "+ CloudID;
                    PostCloudRank();
                    BeginCloud();
                    UI(delegate()
                    {
                        btnCloudInit.Enabled = false;
                        btnCloud.Enabled = true;
                        btnCloudSave.Enabled = true;
                        btnCloudLoad.Enabled = true;
                        btnCloud.Text = "云ID:" + CloudID;
                    });
                }
            });
        }

        private void btnPostRank_Click(object sender, EventArgs e)
        {
            if (IsPostRank)
            {
                IsPostRank = false;
                btnPostRank.Text = "开启成绩上传";
            }
            else
            {
                IsPostRank = true;
                btnPostRank.Text = "停止成绩上传";
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
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

        private void ApplyKeyChange()
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

        private void ShowKCEnable()
        {
            if (kc != null && kc.IsEnable)
            {
                benData.ForeColor = Color.Orange;
            }
            else
            {
                benData.ForeColor = Color.Black;
            }
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
                        KeyboardLib.keybd_event(v, KeyboardLib.MapVirtualKey((uint)v, 0), flag, 0);
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
                case Keys.F8:
                    if (hookStruct.flags >= 128)
                    {
                        if (!IsLiteMode)
                        {
                            SetUIPause(!IsUIPause);
                        }
                        else
                        {
                            ActLiteCtrl();
                        }
                    }
                    break;
                case Keys.F4:
                    if (hookStruct.flags >= 128)
                    {
                        if (!IsLiteMode)
                        {
                            lblMore_Click(null, null);
                        }
                    }
                    break;
                case Keys.F6:
                    if (hookStruct.flags >= 128)
                    {
                        if (IsLiteMode)
                        {
                            SwitchToClassic();
                        }
                        else
                        {
                            SwitchToLite();
                        }
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
                case Keys.F2:
                    if (hookStruct.flags >= 128)
                    {
                        btnReset_Click(null, null);
                    }
                    break;
            }
            /*if (hookStruct.vkCode == 165)
            {
                KeyboardLib.keybd_event(164, KeyboardLib.MapVirtualKey(164, 0), flag, 0);
                handle = true;
            }*/
        }

        private void benData_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void btnDebug_Click(object sender, EventArgs e)
        {
        }

        /*private void SaveGame(string fn = "SRPG.bin")
        {
            if (!GetPalHandle()) throw new Exception("游戏没有在运行，无法保存");
            SRPGobj so = new SRPGobj();
            so.RPG = SaveObject.GetSaveBuffer(this.PalHandle);
            so.TimerStr = GetRStr();

            string FilePath = fn;
            try
            {
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                }
                using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, so);
                }
            }
            catch 
            {
                throw;
            }
        }*/

        public delegate void ExEnd(bool IsOK, string ErrStr);

        private bool IsListenSave = false;

        private void SuspendSaveListen()
        {
            IsListenSave = false;
        }

        private void SaveGameEx(ExEnd cb, string fn = "SRPG.bin")
        {
            if (!GetPalHandle()) throw new Exception("游戏没有在运行，无法保存");
            IsListenSave = true;
            Run(delegate()
            {
                Dictionary<int, DateTime> RPGs = new Dictionary<int, DateTime>();
                string palfolder = GetPalFolder();
                for (int i = 1; i <= 5; ++i)
                {
                    string p = palfolder + i + ".RPG";
                    if (File.Exists(p))
                    {
                        FileInfo fi = new FileInfo(p);
                        RPGs.Add(i, fi.LastWriteTime);
                    }
                }
                Thread.Sleep(300);
                string ChangedFile = "";
                while (IsListenSave && ChangedFile == "")
                {
                    for (int i = 0; i <= 5; ++i)
                    {
                        string p = palfolder + i + ".RPG";
                        if (File.Exists(p))
                        {
                            if (RPGs.ContainsKey(i))
                            {
                                FileInfo fi = new FileInfo(p);
                                if (fi.LastWriteTime > RPGs[i])
                                {
                                    ChangedFile = p;
                                    break;
                                }
                            }
                            else
                            {
                                ChangedFile = p;
                                break;
                            }
                        }
                    }
                    Thread.Sleep(300);
                }

                if (!IsListenSave)
                {
                    if (cb != null)
                    {
                        UI(delegate()
                        {
                            cb(false, "");
                        });
                    }
                    return;
                }

                SRPGobj so = new SRPGobj();
                so.RPG = SaveObject.GetSaveBuffer(this.PalHandle);
                so.TimerStr = GetRStr();
                /*try
                {
                    using (FileStream rfs = new FileStream(ChangedFile, FileMode.Open))
                    {
                        long size = rfs.Length;
                        so.RPG = new byte[size];
                        rfs.Read(so.RPG, 0, so.RPG.Length);
                    }
                }
                catch (Exception ex)
                {
                    cb(false, ex.Message);
                }*/

                string FilePath = fn;
                try
                {
                    if (File.Exists(FilePath))
                    {
                        File.Delete(FilePath);
                    }
                    using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        bf.Serialize(fs, so);
                    }
                }
                catch(Exception ex)
                {
                    if (cb != null)
                    {
                        UI(delegate()
                        {
                            cb(false, ex.Message);
                        });
                    }
                }
                if (cb != null)
                {
                    UI(delegate()
                    {
                        cb(true, "");
                    });
                }
            });
        }

        private void LoadGame(string fn = "SRPG.bin", string rn = "1.RPG")
        {
            SRPGobj so = null;
            string FilePath = fn;
            try
            {
                if (!File.Exists(FilePath)) throw new Exception("计时器目录下找不到SRPG.bin");
                using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    BinaryFormatter bf = new BinaryFormatter();
                    so = bf.Deserialize(fs) as SRPGobj;
                }
            }
            catch 
            {
                throw;
            }

            if (so != null)
            {
                string tmppath = rn;
                try
                {
                    if (File.Exists(tmppath))
                    {
                        File.Delete(tmppath);
                    }
                    using (FileStream fileStream = new FileStream(tmppath, FileMode.OpenOrCreate))
                    {
                        using (BinaryWriter Writer = new BinaryWriter(fileStream))
                        {
                            Writer.Write(so.RPG);
                            Writer.Flush();
                        }
                    }
                }
                catch 
                {
                    throw;
                }

                SetTimerFromString(so.TimerStr);

                WillCopyRPG = tmppath;
            }
        }

        public void SetTimerFromString(string json)
        {
            HObj ho = new HObj(json);
            try
            {
                //CurrentStep = ho.GetValue<int>("Step");
                MaxFC = ho.GetValue<short>("BeeHouse");
                MaxFM = ho.GetValue<short>("BeeSheet");
                MaxHCG = ho.GetValue<short>("FireWorm");
                _jump(ho.GetValue<int>("Step"));
                MT.SetTS(ConvertTimeSpan(ho.GetValue<string>("Current")));
                ST.SetTS(ConvertTimeSpan(ho.GetValue<string>("Idle")));
                HObj cps = ho.GetValue<HObj>("CheckPoints");
                for (int i = 0; i < cps.Count; ++i)
                {
                    HObj cc = cps.GetValue<HObj>(i);
                    CheckPoints[i].Current = ConvertTimeSpan(cc.GetValue<string>("time"));
                }
                //((TItem)(pnMain.Controls[CurrentStep])).Flush();
                foreach (TItem ct in pnMain.Controls)
                {
                    ct.Flush();
                }
            }
            catch
            {
                throw;
            }
        }

        private void CopyRPGIfHas()
        {
            if (WillCopyRPG == "") return;

            try
            {
                if (File.Exists(WillCopyRPG))
                {
                    string palpath = PalProcess.MainModule.FileName;
                    string[] spli = palpath.Split('\\');
                    spli[spli.Length - 1] = "1.RPG";
                    palpath = "";
                    foreach (string s in spli)
                    {
                        palpath += s + "\\";
                    }
                    if (palpath != "")
                    {
                        palpath = palpath.Substring(0, palpath.Length - 1);
                    }
                    if (File.Exists(palpath))
                    {
                        if (File.Exists(palpath + ".bak"))
                        {
                            File.Delete(palpath + ".bak");
                        }
                        File.Move(palpath, palpath + ".bak");
                    }
                    File.Move(WillCopyRPG, palpath);
                }
            }
            catch { }

            WillCopyRPG = "";
        }

        private string GetPalFolder()
        {
            string palpath = PalProcess.MainModule.FileName;
            string[] spli = palpath.Split('\\');
            spli[spli.Length - 1] = "";
            palpath = "";
            foreach (string s in spli)
            {
                palpath += s + "\\";
            }
            if (palpath != "")
            {
                palpath = palpath.Substring(0, palpath.Length - 1);
            }
            return palpath + "\\";
        }

        private void btnDebugSave_Click(object sender, EventArgs e)
        {
            //SaveGame();
        }

        private void btnDebugLoad_Click(object sender, EventArgs e)
        {
            //LoadGame();
        }

        public delegate void OnExSuccess();

        private void UI_SaveGameEx(OnExSuccess cb, string fn = "SRPG.bin")
        {
            SetUIPause(true);
            InfoShow isw = null;
            isw = new InfoShow(this, delegate()
            {
                SuspendSaveListen();
                SetUIPause(false);
                isw.Dispose();
            });
            isw.lblInfo.Text = "计时器已暂停，请在游戏中存档";
            SaveGameEx(delegate(bool isok, string errstr)
            {
                if (isok)
                {
                    if (cb != null)
                    {
                        cb();
                    }
                }
                else
                {
                    if (errstr != "")
                    {
                        Error(errstr);
                    }
                    else
                    {
                        Alert("操作中断");
                    }
                }
                SetUIPause(false);
                isw.Dispose();
            },fn);
            isw.ShowDialog(this);
        }

        private void btnJLSave_Click(object sender, EventArgs e)
        {
            UI_SaveGameEx(delegate() {
                Success("存档已导出到计时器目录下SRPG.bin");
            });
            /*try
            {
                SaveGame();
                Success("存档已导出到计时器目录下SRPG.bin");
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }*/
        }

        private void btnJLLoad_Click(object sender, EventArgs e)
        {
            try
            {
                LoadGame();
            }
            catch (Exception ex)
            {
                Error(ex.Message);
                return;
            }
            SetUIPause(true);
            InfoShow isw = null;
            isw = new InfoShow(this, delegate()
            {
                isw.Dispose();
            });
            isw.lblInfo.Text = "存档导入成功，计时器已自动暂停，请读取游戏中“进度一”后关闭此窗口";
            isw.btnOK.Text = "我已读档";
            isw.ShowDialog(this);
            SetUIPause(false);
        }

        private void btnCloudSave_Click(object sender, EventArgs e)
        {
            string tn = GetTimeName();
            string fn = tn + ".bin";
            UI_SaveGameEx(delegate()
            {
                btnCloudSave.Enabled = false;
                Upload uw = new Upload(this);
                uw.btnOK.Enabled = false;
                uw.txtStatus.Text = "正在上传...";

                Run(delegate()
                {
                    try
                    {
                        cloud.OUpload(System.Environment.CurrentDirectory + "\\" + fn);
                        if (File.Exists(System.Environment.CurrentDirectory + "\\" + fn))
                        {
                            File.Delete(System.Environment.CurrentDirectory + "\\" + fn);
                        }
                        UI(delegate()
                        {
                            btnCloudSave.Enabled = true;
                            uw.txtStatus.Text = tn;
                            uw.btnOK.Enabled = true;
                        });
                    }
                    catch (Exception ex)
                    {
                        UI(delegate()
                        {
                            btnCloudSave.Enabled = true;
                            uw.btnOK.Enabled = true;
                            uw.Dispose();
                            Error(ex.Message);
                        });
                    }
                });
                uw.ShowDialog(this);

            }, fn);
        }

        private void btnCloudLoad_Click(object sender, EventArgs e)
        {
            Download dw = new Download(this);
            dw.ShowDialog(this);
        }

        public void LoadCloudSRPG(string code,Download dw)
        {
            Run(delegate() {
                try
                {
                    string key = code + ".bin";
                    string localname = System.Environment.CurrentDirectory + "\\" + key;
                    cloud.ODownload(key, localname);
                    UI(delegate()
                    {
                        try
                        {
                            LoadGame(key);
                            if (File.Exists(localname))
                            {
                                File.Delete(localname);
                            }
                            dw.txtCode.Enabled = true;
                            dw.btnOK.Enabled = true;
                            dw.Dispose();

                            SetUIPause(true);
                            InfoShow isw = null;
                            isw = new InfoShow(this, delegate()
                            {
                                isw.Dispose();
                            });
                            isw.lblInfo.Text = "存档导入成功，计时器已自动暂停，请读取游戏中“进度一”后关闭此窗口";
                            isw.btnOK.Text = "我已读档";
                            isw.ShowDialog(this);
                            SetUIPause(false);
                        }
                        catch (Exception ee)
                        {
                            dw.txtCode.Enabled = true;
                            dw.btnOK.Enabled = true;
                            Error(ee.Message);
                        }
                    });
                }
                catch (Exception ex)
                {
                    UI(delegate()
                    {
                        dw.txtCode.Enabled = true;
                        dw.btnOK.Enabled = true;
                        Error(ex.Message);
                    });
                }
            });
        }

        private string[] tnbase = new string[60] { 
            "0","1","2","3","4","5","6","7","8","9",
            "A","B","C","D","E","F","G","H","I","J",
            "K","L","M","N","P","Q","R","S","T",
            "U","V","W","X","Y","Z","a","b","c","d",
            "e","f","g","h","i","j","k","m","n",
            "o","p","q","r","s","t","u","v","w","x",
            "y","z"
        };
        private string GetTimeName()
        {
            if (CloudID == "") throw new Exception("云功能没有初始化");
            string res = "";
            DateTime now = DateTime.Now;
            if (CloudID.Length < 2)
            {
                res = "0" + CloudID;
            }
            else
            {
                res = CloudID;
            }
            res += tnbase[now.Month] + tnbase[now.Day] + tnbase[now.Hour] + tnbase[now.Minute] + tnbase[now.Second];
            return res;
        }

        private void SwitchToLite()
        {
            IsLiteMode = true;
            btnPause.Visible = false;
            btnLiteCtrl.Visible = true;
            btnExportCurrent.Visible = false;
            btnSetCurrentToBest.Visible = false;
            btnJLSave.Visible = false;
            btnJLLoad.Visible = false;
            btnSwitchToLite.Visible = false;
            btnSwitchToClassic.Visible = true;
            btnPostRank.Visible = false;
            btnCloudSave.Visible = false;
            btnCloudLoad.Visible = false;
            btnLiveWindow.Visible = false;
            lblInfo.Visible = false;
            lblMore.Visible = false;
            tiHead.Visible = false;
            pnC.Visible = false;
            if (this.CustomTitle != "")
            {
                this.Text = "[简]" + this.CustomTitle;
            }
            this.MinimumSize = new Size(16, 130);
            this.MaximumSize = new Size(272, 130);
            this.Size = new Size(272, 130);
        }

        private void SwitchToClassic()
        {
            IsLiteMode = false;
            btnPause.Visible = true;
            btnLiteCtrl.Visible = false;
            btnExportCurrent.Visible = true;
            btnSetCurrentToBest.Visible = true;
            btnJLSave.Visible = true;
            btnJLLoad.Visible = true;
            btnSwitchToLite.Visible = true;
            btnSwitchToClassic.Visible = false;
            btnPostRank.Visible = true;
            btnCloudSave.Visible = true;
            btnCloudLoad.Visible = true;
            btnLiveWindow.Visible = true;
            lblInfo.Visible = true;
            lblMore.Visible = true;
            tiHead.Visible = true;
            pnC.Visible = true;
            if (this.CustomTitle != "")
            {
                this.Text = this.CustomTitle;
            }
            this.MinimumSize = new Size(16, 255);
            this.MaximumSize = new Size(0, 0);
            this.Size = new Size(272, 694);
        }

        private void btnLiteCtrl_Click(object sender, EventArgs e)
        {
            ActLiteCtrl();
        }

        private void ActLiteCtrl()
        {
            SetLitePause(!IsLitePause);
        }

        private void SetLitePause(bool IsPause)
        {
            if (IsPause)
            {
                IsLitePause = true;
                btnLiteCtrl.Text = "开始";
                LT.Stop();
            }
            else
            {
                IsLitePause = false;
                btnLiteCtrl.Text = "暂停";
                LT.Start();
            }
        }

        private void btnSwitchToLite_Click(object sender, EventArgs e)
        {
            SwitchToLite();
        }

        private void btnSwitchToClassic_Click(object sender, EventArgs e)
        {
            SwitchToClassic();
        }

        private void btnLiveWindow_Click(object sender, EventArgs e)
        {
            if (LiveView == null)
            {
                LiveView = new LiveWindow();
                LiveView.FormClosed += delegate(object sender1, FormClosedEventArgs e1) {
                    this.LiveView = null;
                };
                LiveView.Show();
            }
        }

        private void lblTime_Click(object sender, EventArgs e)
        {

        }
    }
}
