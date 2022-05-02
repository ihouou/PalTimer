using HFrame.ENT;
using HFrame.EX;
using HFrame.OS;
using PalCloudLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Pal98Timer
{
    public class 仙剑98官方Steam : TimerCore
    {
        private string GMD5 = "none";
        public IntPtr PalHandle;
        public IntPtr GameWindowHandle = IntPtr.Zero;
        private int PID = -1;
        private Process PalProcess;
        private long PALBaseAddr = -1;
        private bool _HasGameStart = false;
        private bool _IsFirstStarted = false;
        
        private PTimer ST = new PTimer();
        private PTimer LT = new PTimer();
        private DateTime InBattleTime;
        private DateTime OutBattleTime;
        public TimeSpan BattleLong = new TimeSpan(0);
        private bool HasUnCheated = false;
        private bool IsInUnCheat = false;

        private bool IsPause = false;

        private short MaxFC = 0;
        private short MaxFM = 0;
        private short MaxHCG = 0;
        private short MaxXLL = 0;
        private short MaxLQJ = 0;
        private short MaxYXY = 0;
        private bool IsInBattle = false;

        private bool IsDoMoreEndBattle = true;
        private string WillCopyRPG = "";

        private string cryerror = "";

        private DataServer obsdataserver = null;

        private Pal98SteamGameObject GameObj = new Pal98SteamGameObject();

        private List<string> NamedBattleRes = new List<string>();

        private bool IsShowSpeed = false;

        public 仙剑98官方Steam(GForm form) : base(form)
        {
            CoreName = "PAL98STM";
        }

        protected override void InitCheckPoints()
        {
            LoadBest();
            _CurrentStep = -1;
            Data = new HObj();
            Data["caiyi"] = false;
            CheckPoints = new List<CheckPoint>();
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("见石碑", new TimeSpan(0, 5, 54)))
            {
                Check = delegate ()
                {
                    if (PositionCheck(new int[3] { 19, 1840, 504 }, new int[3] { 19, 1840, 496 })
                        || PositionAroundCheck(19, 1840, 504))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("学功夫", new TimeSpan(0, 10, 53)))
            {
                Check = delegate ()
                {
                    if (GameObj.AreaBGM == 86)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("上船", new TimeSpan(0, 17, 57)))
            {
                Check = delegate ()
                {
                    //if (PositionCheck(new int[3] { 6, 1072, 1080 }, new int[3] { 6, 1088, 1088 }))
                    if (PositionAroundCheck(6, 1248, 1200, 2))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("出林家堡", new TimeSpan(0, 23, 56)))
            {
                Check = delegate ()
                {
                    if (PositionAroundCheck(40, 1616, 984, 5))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("出隐龙窟", new TimeSpan(0, 29, 22)))
            {
                Check = delegate ()
                {
                    if (PositionAroundCheck(49, 464, 1672, 5))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("生化危机", new TimeSpan(0, 35, 48)))
            {
                Check = delegate ()
                {
                    //if (PositionCheck(new int[3] { 62, 1152, 1264 }))
                    if (PositionAroundCheck(62, 1312, 1376, 2))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过鬼将军", new TimeSpan(0, 40, 41)))
            {
                Check = delegate ()
                {
                    if (GameObj.BossID == 472 && GameObj.BattleTotalBlood <= 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过赤鬼王", new TimeSpan(0, 44, 39)))
            {
                Check = delegate ()
                {
                    if (GameObj.BossID == 473 && GameObj.BattleTotalBlood <= 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("进扬州", new TimeSpan(0, 50, 16)))
            {
                Check = delegate ()
                {
                    //if (PositionCheck(new int[3] { 83, 320, 1056 }))
                    if (PositionAroundCheck(80, 416, 1456, 5))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("出扬州", new TimeSpan(0, 57, 28)))
            {
                Check = delegate ()
                {
                    //if (PositionCheck(new int[3] { 85, 1136, 536 }))
                    if (PositionAroundCheck(106, 224, 1072, 5))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("出麻烦洞", new TimeSpan(1, 2, 10)))
            {
                Check = delegate ()
                {
                    //if (PositionCheck(new int[3] { 107, 1520, 408 }))
                    if (PositionAroundCheck(107, 1680, 520, 5))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("进京城", new TimeSpan(1, 4, 12)))
            {
                Check = delegate ()
                {
                    //if (PositionCheck(new int[3] { 101, 256, 224 }))
                    if (PositionAroundCheck(101, 416, 336, 2))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过彩依", new TimeSpan(1, 12, 40)))
            {
                Check = delegate ()
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
                        if (GameObj.BossID == 468)
                        {
                            Data["caiyi"] = true;
                        }
                    }
                    else
                    {
                        if (GameObj.BossID != 468 || (GameObj.BossID == 468 && GameObj.BattleTotalBlood <= 0))
                        {
                            Data["caiyi"] = false;
                            return true;
                        }
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("进锁妖塔", new TimeSpan(1, 17, 47)))
            {
                Check = delegate ()
                {
                    //if (PositionCheck(new int[3] { 147, 1024, 448 }))
                    /*if (PositionAroundCheck(147, 1024, 448, 2))
                    {
                        return true;
                    }*/
                    if (PositionAroundCheck(164, 1136, 1096, 4) || GameObj.Area == 165 || PositionAroundCheck(147, 1184, 560, 2))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("剑柱", new TimeSpan(1, 27, 17)))
            {
                Check = delegate ()
                {
                    //if (PositionCheck(new int[3] { 147, 1024, 448 }))
                    //if (PositionAroundCheck(145, 1184, 272, 3))
                    if (PositionAroundCheck(146, 464, 1160, 3))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("拆塔", new TimeSpan(1, 34, 12)))
            {
                Check = delegate ()
                {
                    if (GameObj.BossID == 542 && GameObj.BattleTotalBlood <= 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过凤凰", new TimeSpan(1, 43, 12)))
            {
                Check = delegate ()
                {
                    if (GameObj.BossID == 464 && GameObj.BattleTotalBlood <= 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("进十年前", new TimeSpan(1, 50, 46)))
            {
                Check = delegate ()
                {
                    if (PositionAroundCheck(247, 1568, 1696, 5))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("水灵珠", new TimeSpan(2, 0, 50)))
            {
                Check = delegate ()
                {
                    if (GameObj.GetItemCount(0x109) > 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("祈雨", new TimeSpan(2, 11, 53)))
            {
                Check = delegate ()
                {
                    if (PositionCheck(new int[3] { 228, 1152, 1040 }))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("通关", new TimeSpan(2, 19, 53)))
            {
                Check = delegate ()
                {
                    if (GameObj.BossID == 546 && GameObj.BattleTotalBlood <= 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
            
            //Data["lazhu"] = false;
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
        private bool PositionAroundCheck(int Area, int X, int Y, int r = 1)
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

        public override string GetMoreInfo()
        {
            //return "蜂" + MaxFC + " 蜜" + MaxFM + " 火" + MaxHCG + " 血" + MaxXLL + " 夜" + MaxYXY + " 剑" + MaxLQJ;
            if (IsShowSpeed)
            {
                return GameObj.MaxGameSpeed.ToString("F3") + "   " + "蜂" + MaxFC + " 蜜" + MaxFM + " 火" + MaxHCG + " 血" + MaxXLL + " 夜" + MaxYXY + " 剑" + MaxLQJ;
            }
            else
            {
                return "蜂" + MaxFC + " 蜜" + MaxFM + " 火" + MaxHCG + " 血" + MaxXLL + " 夜" + MaxYXY + " 剑" + MaxLQJ;
            }
        }

        public override string GetSmallWatch()
        {
            return BattleLong.TotalSeconds.ToString("F2") + "s";
            //return MoveSpeed.ToString("F1");
        }
        
        public override string GetSecondWatch()
        {
            if (ST.CurrentTSOnly.Ticks == 0)
            {
                return "";
            }
            return ST.ToString();
        }

        public override TimeSpan GetMainWatch()
        {
            return MT.CurrentTS;
        }
        public override bool IsMainWatchStar()
        {
            return IsInUnCheat;
        }

        public override string GetGameVersion()
        {
            if (PID != -1)
            {
                return "游戏版本：" + PalPackVersion.ins.GetPalPackVersion(GMD5);
            }
            else
            {
                return "等待游戏运行";
            }
        }

        public override void Reset()
        {
            base.Reset();
            GameObj.ResetGameSpeed();
            HasAlertMutiPal = false;
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
            //InitCheckPoints();
            ST.Reset();
            WillAppendNamedBattle = "";
            NamedBattleRes = new List<string>();
        }
        
        private ToolStripMenuItem btnCloudSave;
        private ToolStripMenuItem btnCloudLoad;
        private ToolStripMenuItem btnLive;
        private ToolStripMenuItem btnSwitch;
        private ToolStripMenuItem btnOBSServer;
        private ToolStripMenuItem btnGameSpeedShow;
        private LiveWindow LiveView = null;
        public override void InitUI()
        {
            var btnExportCurrent = form.NewMenuItem();
            btnExportCurrent.Text = "导出本次成绩";
            btnExportCurrent.Click += delegate (object sender, EventArgs e) {
                ExportCurrent(GetRStr());
            };

            var btnSetCurrentToBest = form.NewMenuItem();
            btnSetCurrentToBest.Text = "设置本次成绩为最佳";
            btnSetCurrentToBest.Click += delegate (object sender, EventArgs e) {
                SaveBest(GetRStr());
            };

            var btnJLSave = form.NewMenuItem();
            btnJLSave.Text = "接力-存档";
            btnJLSave.Click += delegate (object sender, EventArgs e) {
                UI_SaveGameEx(form, delegate () {
                    form.Success("存档已导出到计时器目录下SRPG.bin");
                });
            };

            var btnJLLoad = form.NewMenuItem();
            btnJLLoad.Text = "接力-接盘";
            btnJLLoad.Click += delegate (object sender, EventArgs e) {
                try
                {
                    LoadGame();
                }
                catch (Exception ex)
                {
                    form.Error(ex.Message);
                    return;
                }
                SetUIPause(true);
                InfoShow isw = null;
                isw = new InfoShow(form, delegate ()
                {
                    isw.Dispose();
                });
                isw.lblInfo.Text = "存档导入成功，计时器已自动暂停，请读取游戏中“进度一”后关闭此窗口";
                isw.btnOK.Text = "我已读档";
                isw.ShowDialog(form);
                SetUIPause(false);
            };

            btnSwitch = form.NewMenuItem();
            btnSwitch.Text = "切换至简版";
            btnSwitch.Click += BtnSwitch_Click;

            btnLive = form.NewMenuItem();
            btnLive.Text = "直播窗口";
            btnLive.Click += delegate (object sender, EventArgs e) {
                if (LiveView == null)
                {
                    LiveView = new LiveWindow();
                    LiveView.TarWindowTitle = "Pal Win95 [v2019-04-03-gf3d3391 + steam] ";
                    LiveView.FormClosed += delegate (object sender1, FormClosedEventArgs e1) {
                        this.LiveView = null;
                    };
                    LiveView.Show();
                }
            };

            btnOBSServer = form.NewMenuItem();
            btnOBSServer.Text = "启用OBS0.6x插件";
            btnOBSServer.Checked = false;
            btnOBSServer.Click += delegate (object sender, EventArgs e) {
                btnOBSServer.Checked = !btnOBSServer.Checked;
            };
            btnOBSServer.CheckedChanged += delegate (object sender, EventArgs e) {
                if (btnOBSServer.Checked)
                {
                    StopDataServer();
                    obsdataserver = new DataServer();
                }
                else
                {
                    StopDataServer();
                }
            };

            btnGameSpeedShow = form.NewMenuItem();
            btnGameSpeedShow.Text = "显示游戏速度";
            btnGameSpeedShow.Checked = false;
            btnGameSpeedShow.Click += delegate (object sender, EventArgs e) {
                btnGameSpeedShow.Checked = !btnGameSpeedShow.Checked;
            };
            btnGameSpeedShow.CheckedChanged += delegate (object sender, EventArgs e) {
                IsShowSpeed = btnGameSpeedShow.Checked;
                GameObj.CalcGameSpeed = IsShowSpeed;
                if (!IsShowSpeed)
                {
                    GameObj.ResetGameSpeed();
                }
            };
            
            
            btnCloudSave = form.NewCloudMenuItem();
            btnCloudSave.Text = "云存档";
            btnCloudSave.Enabled = false;
            btnCloudSave.Click += delegate (object sender, EventArgs e) {
                string tn = GetTimeName();
                string fn = tn + ".bin";
                UI_SaveGameEx(form, delegate ()
                {
                    btnCloudSave.Enabled = false;
                    Upload uw = new Upload(btnCloudSave);
                    uw.btnOK.Enabled = false;
                    uw.txtStatus.Text = "正在上传...";

                    FormEx.Run(delegate ()
                    {
                        try
                        {
                            form.OUpload(System.Environment.CurrentDirectory + "\\" + fn);
                            if (File.Exists(System.Environment.CurrentDirectory + "\\" + fn))
                            {
                                File.Delete(System.Environment.CurrentDirectory + "\\" + fn);
                            }
                            form.UI(delegate ()
                            {
                                btnCloudSave.Enabled = true;
                                uw.txtStatus.Text = tn;
                                uw.btnOK.Enabled = true;
                            });
                        }
                        catch (Exception ex)
                        {
                            form.UI(delegate ()
                            {
                                btnCloudSave.Enabled = true;
                                uw.btnOK.Enabled = true;
                                uw.Dispose();
                                form.Error(ex.Message);
                            });
                        }
                    });
                    uw.ShowDialog(form);

                }, fn);
            };

            btnCloudLoad = form.NewCloudMenuItem();
            btnCloudLoad.Text = "云读档";
            btnCloudLoad.Enabled = false;
            btnCloudLoad.Click += delegate (object sender, EventArgs e) {
                Download dw = new Download(delegate (string c, Download d) {
                    LoadCloudSRPG(form, c, d);
                });
                dw.ShowDialog(form);
            };
        }

        private void BtnSwitch_Click(object sender, EventArgs e)
        {
            LoadCore(new 简版(form));
        }

        protected override void OnTick()
        {
            if (GetPalHandle())
            {
                CopyRPGIfHas();

                JudgePause();
                try
                {
                    FlushGameObject();
                }
                catch (Exception ex)
                {
                }


                try
                {
                    //if (GameObj.Enemies.Count > 0)
                    if (GameObj.BattleFlag)
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

            PreData();
        }

        private void PreData()
        {
            SI.ins.MT = MT.ToString();
            SI.ins.ST = ST.ToString();
            SI.ins.MoreInfo = this.GetMoreInfo();
            SI.ins.GameVersion = this.GetGameVersion();
            SI.ins.Version = GForm.CurrentVersion;
            SI.ins.BattleLong = BattleLong.TotalSeconds.ToString("F2") + "s";
            SI.ins.FC = MaxFC.ToString();
            SI.ins.FM = MaxFM.ToString();
            SI.ins.HCG = MaxHCG.ToString();
            SI.ins.XLL = MaxXLL.ToString();
            SI.ins.YXY = MaxYXY.ToString();
            SI.ins.LQJ = MaxLQJ.ToString();
            SI.ins.CloudID = form.CloudID().ToString();
            SI.ins.CurrentStep = CurrentStep;
            SI.ins.cps = CheckPoints;
            SI.ins.Luck = MConfig.ins.Luck();
            SI.ins.ColorEgg = MConfig.ins.ColorEgg;
        }
        private bool HasAlertMutiPal = false;
        public override bool IsShowC()
        {
            return false;
        }
        private bool GetPalHandle()
        {
            Process[] res = Process.GetProcessesByName("PAL");
            if (res.Length > 1)
            {
                if (!HasAlertMutiPal)
                {
                    cryerror = "检测到多个PAL.exe进程，请关闭其他的，只保留一个！";
                    HasAlertMutiPal = true;
                }
                return false;
            }

            HasAlertMutiPal = false;
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
                    PALBaseAddr = PalProcess.MainModule.BaseAddress.ToInt64();
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
                        PALBaseAddr = -1;
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
                PALBaseAddr = -1;
                return false;
            }
        }

        private void CalcPalMD5()
        {
            try
            {
                string dllmd5 = GetFileMD5(GetGameFilePath("PAL.exe"));
                string datamd5 = GetFileMD5(GetGameFilePath("Data.mkf"));
                string sssmd5 = GetFileMD5(GetGameFilePath("Sss.mkf"));
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
        private void CopyRPGIfHas()
        {
            if (WillCopyRPG == "") return;

            try
            {
                if (File.Exists(WillCopyRPG))
                {
                    string palpath = PalProcess.MainModule.FileName;
                    string[] spli = palpath.Split('\\');
                    spli[spli.Length - 1] = "SAVE\\1.RPG";
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
        
        private void FlushGameObject()
        {
            GameObj.Flush(PalHandle, PID, 0, PALBaseAddr);
            FlushPlugins(PalHandle, PID, 0, PALBaseAddr);
        }
        private Pal98SteamBattleItemWatch biw = new Pal98SteamBattleItemWatch();
        private string CurrentNamedBattle = "";
        private string WillAppendNamedBattle = "";
        private void BattleBegin()
        {
            BattleLong = new TimeSpan(0);
            InBattleTime = DateTime.Now;
            biw = new Pal98SteamBattleItemWatch();
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
                WillAppendNamedBattle = CurrentNamedBattle + "：" + BattleLong.TotalSeconds.ToString("F2") + "s";
                CurrentNamedBattle = "";
            }
        }
        private void BattleEndMore()
        {
            //战斗结束，强制再算差
            FlushGameObject();
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
        protected override void FillMoreTimerData(HObj exdata)
        {
            exdata["Idle"] = ST.ToString();
            exdata["Lite"] = LT.ToString();
            exdata["BeeHouse"] = MaxFC;
            exdata["BeeSheet"] = MaxFM;
            exdata["FireWorm"] = MaxHCG;
            exdata["DragonSword"] = MaxLQJ;
            exdata["BloodLink"] = MaxXLL;
            exdata["NightCloth"] = MaxYXY;
            exdata["GMD5"] = GMD5;

            string namedbattles = "";
            foreach (string nmb in NamedBattleRes)
            {
                namedbattles += nmb + "|";
            }
            if (namedbattles != "")
            {
                namedbattles = namedbattles.Substring(0, namedbattles.Length - 1);
            }
            exdata["NamedBattles"] = namedbattles;
        }
        private void CheckCheatBegin()
        {
            if (PositionCheck(new int[3] { 177, 1296, 728 }, new int[3] { 177, 1264, 728 }, new int[3] { 177, 1296, 712 }, new int[3] { 177, 1232, 728 }))
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
        public override void OnFunctionKey(int FunNo)
        {
            switch (FunNo)
            {
                case 8:
                    HandPause();
                    break;
                case 6:
                    if (form.Confirm("更换内核将会重置计时器，确认么？"))
                    {
                        BtnSwitch_Click(null, null);
                    };
                    break;
                    case 12:
                        /*DebugForm df = new DebugForm();
                        df.ShowData(GameObj);
                        df.Show();*/
                        break;
            }
        }

        public delegate void OnExSuccess();

        private void UI_SaveGameEx(FormEx f, OnExSuccess cb, string fn = "SRPG.bin")
        {
            SetUIPause(true);
            InfoShow isw = null;
            isw = new InfoShow(f, delegate ()
            {
                SuspendSaveListen();
                SetUIPause(false);
                isw.Dispose();
            });
            isw.lblInfo.Text = "计时器已暂停，请在游戏中存档";
            bool haserr = false;
            try
            {
                SaveGameEx(f, delegate (bool isok, string errstr)
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
                            f.Error(errstr);
                        }
                        else
                        {
                            f.Alert("操作中断");
                        }
                    }
                    SetUIPause(false);
                    isw.Dispose();
                }, fn);
            }
            catch (Exception ex)
            {
                haserr = true;
                f.Error(ex.Message);
            }
            if (haserr)
            {
                if (isw != null)
                {
                    isw.Dispose();
                }
                SetUIPause(false);
            }
            else
            {
                isw.ShowDialog(f);
            }
        }

        public delegate void ExEnd(bool IsOK, string ErrStr);

        private bool IsListenSave = false;

        private void SuspendSaveListen()
        {
            IsListenSave = false;
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
            return palpath;
        }

        private void SaveGameEx(FormEx f, ExEnd cb, string fn = "SRPG.bin")
        {
            if (!GetPalHandle()) throw new Exception("游戏没有在运行，无法保存");
            IsListenSave = true;
            FormEx.Run(delegate ()
            {
                Dictionary<int, DateTime> RPGs = new Dictionary<int, DateTime>();
                string palfolder = GetPalFolder() + "SAVE\\";
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
                    for (int i = 1; i <= 5; ++i)
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
                        f.UI(delegate ()
                        {
                            cb(false, "");
                        });
                    }
                    return;
                }

                SRPGobj so = new SRPGobj();
                //so.RPG = SaveObject.GetSaveBuffer(this.PalHandle);
                so.TimerStr = GetRStr();
                try
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
                }

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
                catch (Exception ex)
                {
                    if (cb != null)
                    {
                        f.UI(delegate ()
                        {
                            cb(false, ex.Message);
                        });
                    }
                }
                if (cb != null)
                {
                    f.UI(delegate ()
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
                if (!File.Exists(FilePath)) throw new Exception("计时器目录下找不到" + fn);
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
                MaxLQJ = ho.GetValue<short>("DragonSword");
                MaxXLL = ho.GetValue<short>("BloodLink");
                MaxYXY = ho.GetValue<short>("NightCloth");
                MT.SetTS(ConvertTimeSpan(ho.GetValue<string>("Current")));
                ST.SetTS(ConvertTimeSpan(ho.GetValue<string>("Idle")));
                HObj cps = ho.GetValue<HObj>("CheckPoints");
                for (int i = 0; i < cps.Count; ++i)
                {
                    HObj cc = cps.GetValue<HObj>(i);
                    //CheckPoints[i].Current = ConvertTimeSpan(cc.GetValue<string>("time"));
                    CheckPoints[i].SetCurrentTSForLoad(ConvertTimeSpan(cc.GetValue<string>("time")));
                }
                //((TItem)(pnMain.Controls[CurrentStep])).Flush();
                /*foreach (TItem ct in pnMain.Controls)
                {
                    ct.Flush();
                }*/
                Jump(ho.GetValue<int>("Step"));
                string nmbs = ho.GetValue<string>("NamedBattles");
                NamedBattleRes = new List<string>();
                string[] nmbspli = nmbs.Split('|');
                foreach (string nmb in nmbspli)
                {
                    NamedBattleRes.Add(nmb);
                }
                WillAppendNamedBattle = nmbs;
            }
            catch
            {
                throw;
            }
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
            if (form.CloudID() < 0) throw new Exception("云功能没有初始化");
            string res = form.CloudID().ToString().PadLeft(3, '0');
            DateTime now = DateTime.Now;
            res += tnbase[now.Month] + tnbase[now.Day] + tnbase[now.Hour] + tnbase[now.Minute] + tnbase[now.Second];
            return res;
        }

        public void LoadCloudSRPG(FormEx f, string code, Download dw)
        {
            FormEx.Run(delegate () {
                try
                {
                    string key = code + ".bin";
                    string localname = System.Environment.CurrentDirectory + "\\" + key;
                    form.ODownload(key, localname);
                    f.UI(delegate ()
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
                            isw = new InfoShow(f, delegate ()
                            {
                                isw.Dispose();
                            });
                            isw.lblInfo.Text = "存档导入成功，计时器已自动暂停，请读取游戏中“进度一”后关闭此窗口";
                            isw.btnOK.Text = "我已读档";
                            isw.ShowDialog(f);
                            SetUIPause(false);
                        }
                        catch (Exception ee)
                        {
                            dw.txtCode.Enabled = true;
                            dw.btnOK.Enabled = true;
                            f.Error(ee.Message);
                        }
                    });
                }
                catch (Exception ex)
                {
                    f.UI(delegate ()
                    {
                        dw.txtCode.Enabled = true;
                        dw.btnOK.Enabled = true;
                        f.Error(ex.Message);
                    });
                }
            });
        }

        public override string GetAAction()
        {
            if (WillAppendNamedBattle == "")
            {
                return "";
            }
            else
            {
                NamedBattleRes.Add(WillAppendNamedBattle);
                string res = WillAppendNamedBattle;
                WillAppendNamedBattle = "";
                return res;
            }
        }

        public override void Unload()
        {
            base.Unload();
            StopDataServer();
        }
        private void StopDataServer()
        {
            try
            {
                if (obsdataserver != null)
                {
                    obsdataserver.X();
                }
            }
            catch
            {
            }
            obsdataserver = null;
        }
        public override string GetCriticalError()
        {
            if (cryerror == "")
            {
                return "";
            }
            else
            {
                string tmp = cryerror;
                cryerror = "";
                return tmp;
            }
        }
        public override bool NeedBlockCtrlEnter()
        {
            return false;
        }
        public override void OnCloudOK()
        {
            btnCloudSave.Enabled = true;
            btnCloudLoad.Enabled = true;
        }
        public override void OnCloudFail()
        {
            btnCloudSave.Enabled = false;
            btnCloudLoad.Enabled = false;
        }
        public override void OnCloudPending()
        {
            btnCloudSave.Enabled = false;
            btnCloudLoad.Enabled = false;
        }
        public override string ForCloudLiteData()
        {
            return MT.CurrentTS.Ticks.ToString() + "," + ST.CurrentTS.Ticks.ToString() + "," + LT.CurrentTS.Ticks.ToString();
        }
    }

    public class Pal98SteamGameObject:MemoryReadBase
    {
        //public const long BaseAddrPTR = 0x00428000;
        public const long MoneyOffset = 0x486044;//!
        public const long XOffset = 0x486006;//!
        public const long YOffset = 0x486008;//!
        public const long AreaOffset = 0x486026;//!
        public const long CurrentBGMOffset = 0x24A;
        public const long AreaBGMOffset = 0x486030;
        public const long BattleEnemySlotOffset = 0x4C6C38;//!
        public const long ItemSlotOffset = 0x486308;//!
        //public const long BattleFlagOffset = 0x4C6BB8;//! 0为不在战斗 其他为在战斗
        public const long BattleFlagOffset = 0x4C6B74;//?
        public const long GameTimerOffset = 0x48603E;

        public short[] BossIDs = new short[] { 486, 546, 472, 473, 435, 542, 464, 463, 502, 468 };
        //486:蛇男 546:拜月 472:骷髅将军 473:赤鬼王 435:大蜘蛛 542:火龙 464:凤凰 463:麒麟 502:蛇女灵儿 468:彩衣

        public int Money = 0;
        public short X = 0;
        public short Y = 0;
        public int Area = 0;
        public short CurrentBGM = 0;//0x3胜利 0x1失败 0x4葫芦界面
        public short AreaBGM = 0;
        public bool BattleFlag = false;
        
        private IntPtr handle;
        private int PID;
        public long BaseAddr = 0x0;
        public long BattleEnemySlotAddr = 0x0;
        public long ItemSlotAddr = 0x0;

        public Dictionary<short, short> Items = new Dictionary<short, short>();
        public List<Pal98SteamEnemyObject> Enemies = new List<Pal98SteamEnemyObject>();
        public short BossID = -1;
        public short BattleTotalBlood = 0;

        public List<NamedBattle> NamedBattles = new List<NamedBattle>();

        public override string ToString()
        {
            string tmp = "";

            tmp += "area:" + this.Area + "\r\n";
            tmp += "x:" + this.X + "\r\n";
            tmp += "y:" + this.Y + "\r\n";
            tmp += "money:" + this.Money + "\r\n";
            tmp += "BGM:" + this.CurrentBGM + "\r\n";
            tmp += "AreaBGM:" + this.AreaBGM + "\r\n";
            tmp += "MaxGameSpeed:" + this.MaxGameSpeed + "\r\n";
            tmp += "\r\n";
            tmp += "是否有破天锤:" + ((this.GetItemCount(0x117) > 0) ? "是" : "否") + "\r\n";
            tmp += "是否有香蕉:" + ((this.GetItemCount(0x123) > 0) ? "是" : "否") + "\r\n";
            tmp += "\r\n";
            tmp += "Enemy:" + this.BattleTotalBlood + "\r\n";
            if (this.BossID > 0)
            {
                tmp += "Boss:" + this.BossID + " TotalBlood:" + this.BattleTotalBlood + "\r\n";
            }
            for (int i = 0; i < this.Enemies.Count; ++i)
            {
                tmp += "[" + (i + 1) + "] id:" + this.Enemies[i].ID + " blood:" + this.Enemies[i].Blood + "\r\n";
            }
            return tmp;
        }

        public Pal98SteamGameObject()
        {
            this.InitNamedBattles();
            ssw.Start();
        }

        public void InitNamedBattles()
        {
            NamedBattles.Add(new NamedBattle()
            {
                Name = "苗胖",
                Checker = delegate ()
                {
                    if (GetEnemyCount(485) == 1 && GetEnemyCount(495) == 2)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "皮鞭月如",
                Checker = delegate ()
                {
                    if (GetEnemyCount(480) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "擂台",
                Checker = delegate ()
                {
                    if (GetEnemyCount(483) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "蛇男",
                Checker = delegate ()
                {
                    if (GetEnemyCount(486) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "狐狸",
                Checker = delegate ()
                {
                    if (GetEnemyCount(469) == 1 && Enemies.Count == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "智障",
                Checker = delegate ()
                {
                    if (GetEnemyCount(482) == 1 && GetEnemyCount(451) == 1 && GetEnemyCount(453) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "老和尚",
                Checker = delegate ()
                {
                    if (GetEnemyCount(524) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "鬼将军",
                Checker = delegate ()
                {
                    if (GetEnemyCount(472) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "赤鬼王",
                Checker = delegate ()
                {
                    if (GetEnemyCount(473) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "石长老1",
                Checker = delegate ()
                {
                    if (GetEnemyCount(496) == 1 && GetEnemyCount(527) == 2)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "金蟾鬼母",
                Checker = delegate ()
                {
                    if (GetEnemyCount(500) == 1 && GetEnemyCount(465) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "石长老2",
                Checker = delegate ()
                {
                    if (GetEnemyCount(496) == 1 && Enemies.Count == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "林天南",
                Checker = delegate ()
                {
                    if (GetEnemyCount(525) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "嫂子",
                Checker = delegate ()
                {
                    if (GetEnemyCount(468) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "黑森林雷蜘蛛",
                Checker = delegate ()
                {
                    if (GetEnemyCount(498) == 1 && Enemies.Count == 1 && Area == 140)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "天鬼皇",
                Checker = delegate ()
                {
                    if (GetEnemyCount(529) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "镇狱冥王",
                Checker = delegate ()
                {
                    if (GetEnemyCount(519) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "风龙",
                Checker = delegate ()
                {
                    if (GetEnemyCount(544) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "土龙",
                Checker = delegate ()
                {
                    if (GetEnemyCount(541) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "雷龙",
                Checker = delegate ()
                {
                    if (GetEnemyCount(545) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "毒龙",
                Checker = delegate ()
                {
                    if (GetEnemyCount(539) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "水龙",
                Checker = delegate ()
                {
                    if (GetEnemyCount(543) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "金龙",
                Checker = delegate ()
                {
                    if (GetEnemyCount(540) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "火龙",
                Checker = delegate ()
                {
                    if (GetEnemyCount(542) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "凤凰",
                Checker = delegate ()
                {
                    if (GetEnemyCount(464) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "麒麟",
                Checker = delegate ()
                {
                    if (GetEnemyCount(463) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "水魔兽",
                Checker = delegate ()
                {
                    if (GetEnemyCount(547) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "盖罗娇",
                Checker = delegate ()
                {
                    if (GetEnemyCount(490) == 1 && GetEnemyCount(501) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "殿前",
                Checker = delegate ()
                {
                    if (GetEnemyCount(528) == 2)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "树精",
                Checker = delegate ()
                {
                    if (GetEnemyCount(462) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "拜月",
                Checker = delegate ()
                {
                    if (GetEnemyCount(546) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
        }

        public int GetEnemyCount(short EID)
        {
            short res = 0;
            if (this.Enemies != null)
            {
                foreach (Pal98SteamEnemyObject eo in this.Enemies)
                {
                    if (eo.ID == EID)
                    {
                        res++;
                    }
                }
            }
            return res;
        }

        public string GetNamedBattle()
        {
            foreach (NamedBattle nb in this.NamedBattles)
            {
                if (nb.Checker())
                {
                    return nb.Name;
                }
            }
            return "";
        }

        public override void Flush(IntPtr handle, int PID,int b32,long PALBaseAddr)
        {
            if (PID != this.PID)
            {
                this.PID = PID;
                this.handle = handle;
            }
            //Process p = Process.GetProcessById(this.PID);
            //BaseAddr = p.MainModule.BaseAddress.ToInt64();
            BaseAddr = PALBaseAddr;
            //BaseAddr = Readm<int>(this.handle, BaseAddrPTR);
            BattleEnemySlotAddr = BaseAddr + BattleEnemySlotOffset;
            ItemSlotAddr = BaseAddr + ItemSlotOffset;
            Money = Readm<int>(this.handle, BaseAddr + MoneyOffset);

            int area = Readm<int>(this.handle, BaseAddr + AreaOffset);
            short x= Readm<short>(this.handle, BaseAddr + XOffset);
            short y= Readm<short>(this.handle, BaseAddr + YOffset);
            bool IsInMapMove = (Area == area && (x != X || y != Y));
            X = x;
            Y = y;
            Area = area;
            CurrentBGM = Readm<short>(this.handle, BaseAddr+ CurrentBGMOffset);
            AreaBGM = Readm<short>(this.handle, BaseAddr + AreaBGMOffset);
            BattleFlag = (Readm<int>(this.handle, BaseAddr + BattleFlagOffset) != 0);

            FlushItems();
            FlushBattleEnemies();
            try
            {
                FlushGameSpeed(IsInMapMove);
            }
            catch {
            }
            this.SetFastSpeakIfCan();
        }

        private Stopwatch ssw = new Stopwatch();
        private ushort LastGameTimer = 0;
        private long LastTimerTick = 0;
        public double MaxGameSpeed = 0;
        private long GameSpeedValCount = 0;
        public bool CalcGameSpeed = false;
        private void FlushGameSpeed(bool IsInMapMove)
        {
            if (!CalcGameSpeed) return;
            if (!IsInMapMove) return;
            //获取游戏流逝时间
            ushort curtimer = Readm<ushort>(this.handle, BaseAddr + GameTimerOffset);
            long nowtick = ssw.ElapsedTicks;
            if (LastTimerTick > 0)
            {
                if (curtimer < LastGameTimer)
                {
                    double tickcha = (nowtick - LastTimerTick) / 10000;
                    double timercha = (LastGameTimer - curtimer) * 100;
                    double speed = timercha / tickcha;
                    //if (speed > MaxGameSpeed) MaxGameSpeed = speed;
                    //MaxGameSpeed = speed;
                    MaxGameSpeed = (MaxGameSpeed * GameSpeedValCount + speed) / (GameSpeedValCount + 1);
                    GameSpeedValCount++;
                }
            }
            LastTimerTick = nowtick;
            LastGameTimer = curtimer;
        }
        public void ResetGameSpeed() {
            bool sw = CalcGameSpeed;
            CalcGameSpeed = false;
            LastGameTimer = 0;
            LastTimerTick = 0;
            GameSpeedValCount = 0;
            MaxGameSpeed = 0;
            CalcGameSpeed = sw;
        }

        private void FlushItems()
        {
            Dictionary<short, short> tmp = new Dictionary<short, short>();
            long currentaddr = ItemSlotAddr;
            for (int i = 0; i < 251; ++i, currentaddr += 0x6)
            {
                short id = Readm<short>(this.handle, currentaddr);
                if (id <= 0) continue;

                short count = Readm<short>(this.handle, currentaddr + 0x2);
                if (count <= 0) continue;

                if (tmp.ContainsKey(id))
                {
                    tmp[id] = count;
                }
                else
                {
                    tmp.Add(id, count);
                }


            }
            Items = tmp;
        }

        private void FlushBattleEnemies()
        {
            BossID = -1;
            BattleTotalBlood = 0;
            List<Pal98SteamEnemyObject> tmp = new List<Pal98SteamEnemyObject>();
            if (this.BattleFlag)
            {
                long currentaddr = BattleEnemySlotAddr;
                for (int i = 0; i < 5; ++i, currentaddr += Pal98SteamEnemyObject.Length)
                {
                    if (Readm<short>(handle, currentaddr) <= 0)
                    {
                        continue;
                    }
                    Pal98SteamEnemyObject c = new Pal98SteamEnemyObject(handle, currentaddr);
                    if (c.Blood > 0)
                    {
                        BattleTotalBlood += c.Blood;
                    }
                    //if (BossIDs.Contains<short>(c.ID))
                    if (ArrayContains<short>(BossIDs, c.ID))
                    {
                        BossID = c.ID;
                    }
                    tmp.Add(c);
                }
            }
            Enemies = tmp;
        }
        
        public short GetItemCount(short ItemID)
        {
            if (Items.ContainsKey(ItemID))
            {
                return Items[ItemID];
            }
            else
            {
                return 0;
            }
        }
        
        public void SetFastSpeakIfCan() {
            byte[] readbuf = new byte[3];
            int sizeofRead;
            try
            {
                if (Kernel32.ReadProcessMemory(handle, new IntPtr(BaseAddr + 0x650FE), readbuf, 3, out sizeofRead))
                {
                    if (readbuf[0] == 0x41 && readbuf[1] == 0x0F && readbuf[2] == 0x45)
                    {
                        byte[] writebuf = new byte[3] { 0x90, 0x41, 0x8B };
                        int sizeofWrite;
                        Kernel32.WriteProcessMemory(handle, new IntPtr(BaseAddr + 0x650FE), writebuf, 3, out sizeofWrite);
                    }
                }
            }
            catch { }
        }
    }

    public class Pal98SteamEnemyObject
    {
        public const int Length = 0xC8;

        private const int IDOffset = 0x0;
        private const int BloodOffset = 0x18;

        public short ID;
        public short Blood;

        public Pal98SteamEnemyObject(IntPtr handle, long HeadAddr)
        {
            ID = MemoryReadBase.Readm<short>(handle, HeadAddr + IDOffset);
            //ID -= 398;
            Blood = MemoryReadBase.Readm<short>(handle, HeadAddr + BloodOffset);
        }
    }

    public class Pal98SteamBattleItemWatch
    {
        private List<short> ids = new List<short>();
        private Dictionary<short, short> BattleGetItemWatch = new Dictionary<short, short>();
        private Dictionary<short, short> BattleUseItemWatch = new Dictionary<short, short>();
        private Dictionary<short, short> BattleLastItemCount = new Dictionary<short, short>();
        public Pal98SteamBattleItemWatch()
        {
            ids = new List<short>();
            BattleGetItemWatch = new Dictionary<short, short>();
            BattleUseItemWatch = new Dictionary<short, short>();
            BattleLastItemCount = new Dictionary<short, short>();
        }
        public void Insert(short id, short Count)
        {
            ids.Add(id);
            BattleGetItemWatch.Add(id, 0);
            BattleUseItemWatch.Add(id, 0);
            BattleLastItemCount.Add(id, Count);
        }
        public void SetCount(short id, short Count)
        {
            short cha = (short)(Count - BattleLastItemCount[id]);
            BattleLastItemCount[id] = Count;
            if (cha > 0)
            {
                BattleGetItemWatch[id] = (short)(BattleGetItemWatch[id] + cha);
            }
            else if (cha < 0)
            {
                BattleUseItemWatch[id] = (short)(BattleUseItemWatch[id] - cha);
            }
        }
        public void SetCount(Pal98SteamGameObject GameObj)
        {
            foreach (short id in ids)
            {
                SetCount(id, GameObj.GetItemCount(id));
            }
        }
        public short UsedCount(short id)
        {
            return BattleUseItemWatch[id];
        }
        public short GettedCount(short id)
        {
            return BattleGetItemWatch[id];
        }
    }
}
