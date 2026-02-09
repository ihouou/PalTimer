using HFrame.ENT;
using HFrame.EX;
using HFrame.OS;
using PalCloudLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Pal98Timer
{

    public class 仙剑98柔情DX9 : TimerCore
    {
        public override bool IsShowC()
        {
            Process[] res = Process.GetProcessesByName("Pal98Robot");
            if (res.Length > 0)
            {
                return true;
            }
            return false;
        }
        private string GMD5 = "none";
        private string DX9Version = "未知";
        public IntPtr PalHandle;
        public IntPtr GameWindowHandle = IntPtr.Zero;
        private int PID = -1;
        private Process PalProcess;
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
        private short MaxTLF = 0;
        private short MaxQTJ = 0;
        private bool IsInBattle = false;

        private bool IsDoMoreEndBattle = true;
        private string WillCopyRPG = "";

        private string cryerror = "";
        
        private GameObject GameObj = new GameObject();

        private List<string> NamedBattleRes = new List<string>();

        private bool IsShowSpeed = false;
        private bool HasAlertMutiPal = false;

        private float MoveSpeed = 0;
        private DateTime LastFlushTime = DateTime.Now;
        private BattleItemWatch biw = new BattleItemWatch();
        private string CurrentNamedBattle = "";
        private string WillAppendNamedBattle = "";

        private DateTime? InitialDetectionTime = null;  // 首次检测到游戏的时间
        private bool HasConfirmedDX9 = false;  // 是否已确认DX9标题
        private const int DX9TitleGracePeriodSeconds = 10;  // DX9标题出现的宽限期（秒）

        private int TotalMonsterCount = 0;  // 撞怪总数
        
        // 战斗中实时显示用的临时变量（功能2）
        private short CurrentBattleHCG = 0;  // 当前战斗中获得的火虫草
        private short CurrentBattleXLL = 0;  // 当前战斗中获得的血玲珑
        private short CurrentBattleLQJ = 0;  // 当前战斗中获得的龙泉剑

        public 仙剑98柔情DX9(GForm form) : base(form)
        {
            CoreName = "PAL98DX9";
        }

        protected override void InitCheckPoints()
        {
            // 如果本地存在bestPAL98.txt且不存在bestPAL98DX9.txt，则复制
            string sourceBestFile = "bestPAL98.txt";
            string targetBestFile = "bestPAL98DX9.txt";
            if (File.Exists(sourceBestFile) && !File.Exists(targetBestFile))
            {
                File.Copy(sourceBestFile, targetBestFile);
            }
            
            LoadBest();
            _CurrentStep = -1;
            Data = new HObj();
            Data["caiyi"] = false;
            CheckPoints = new List<CheckPoint>();
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("见石碑", new TimeSpan(0, 6, 5)))
            {
                Check = delegate ()
                {
                    if (PositionCheck(new int[3] { 19, 1696, 384 }, new int[3] { 19, 1680, 376 })
                        || PositionAroundCheck(19, 1696, 384))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("学功夫", new TimeSpan(0, 11, 13)))
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
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("上船", new TimeSpan(0, 18, 37)))
            {
                Check = delegate ()
                {
                    //if (PositionCheck(new int[3] { 6, 1072, 1080 }, new int[3] { 6, 1088, 1088 }))
                    if (PositionAroundCheck(6, 1072, 1080, 2))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("出林家堡", new TimeSpan(0, 24, 53)))
            {
                Check = delegate ()
                {
                    if (PositionAroundCheck(40, 1456, 872, 5))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("出隐龙窟", new TimeSpan(0, 30, 46)))
            {
                Check = delegate ()
                {
                    if (PositionAroundCheck(49, 304, 1560, 5))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("生化危机", new TimeSpan(0, 37, 56)))
            {
                Check = delegate ()
                {
                    //if (PositionCheck(new int[3] { 62, 1152, 1264 }))
                    if (PositionAroundCheck(62, 1152, 1264, 2))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过鬼将军", new TimeSpan(0, 43, 25)))
            {
                Check = delegate ()
                {
                    if (GameObj.BossID == 75 && GameObj.BattleTotalBlood <= 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过赤鬼王", new TimeSpan(0, 47, 45)))
            {
                Check = delegate ()
                {
                    if (GameObj.BossID == 76 && GameObj.BattleTotalBlood <= 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("进扬州", new TimeSpan(0, 54, 0)))
            {
                Check = delegate ()
                {
                    //if (PositionCheck(new int[3] { 83, 320, 1056 }))
                    if (PositionAroundCheck(80, 256, 1344, 5))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("出扬州", new TimeSpan(1, 1, 53)))
            {
                Check = delegate ()
                {
                    //if (PositionCheck(new int[3] { 85, 1136, 536 }))
                    if (PositionAroundCheck(106, 64, 960, 5))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("出麻烦洞", new TimeSpan(1, 7, 26)))
            {
                Check = delegate ()
                {
                    //if (PositionCheck(new int[3] { 107, 1520, 408 }))
                    if (PositionAroundCheck(107, 1520, 408, 5))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("进京城", new TimeSpan(1, 9, 32)))
            {
                Check = delegate ()
                {
                    //if (PositionCheck(new int[3] { 101, 256, 224 }))
                    if (PositionAroundCheck(101, 272, 216, 2))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过彩依", new TimeSpan(1, 19, 47)))
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
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("进锁妖塔", new TimeSpan(1, 25, 33)))
            {
                Check = delegate ()
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
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("剑柱", new TimeSpan(1, 37, 27)))
            {
                Check = delegate ()
                {
                    //if (PositionCheck(new int[3] { 147, 1024, 448 }))
                    if (PositionAroundCheck(146, 304, 1048, 3))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("拆塔", new TimeSpan(1, 44, 22)))
            {
                Check = delegate ()
                {
                    if (GameObj.BossID == 144 && GameObj.BattleTotalBlood <= 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过凤凰", new TimeSpan(1, 54, 11)))
            {
                Check = delegate ()
                {
                    if (GameObj.BossID == 67 && GameObj.BattleTotalBlood <= 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("进十年前", new TimeSpan(2, 3, 17)))
            {
                Check = delegate ()
                {
                    if (PositionAroundCheck(247, 1408, 1584, 5))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("水灵珠", new TimeSpan(2, 14, 1)))
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
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("祈雨", new TimeSpan(2, 27, 8)))
            {
                Check = delegate ()
                {
                    if (PositionCheck(new int[3] { 228, 992, 928 }))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("通关", new TimeSpan(2, 37, 32)))
            {
                Check = delegate ()
                {
                    if (GameObj.BossID == 149 && GameObj.BattleTotalBlood <= 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
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

        public override string GetGameVersion()
        {
            if (PID != -1)
            {
                return "仙剑98原版 新补丁 " + DX9Version;
            }
            else
            {
                return "等待游戏运行";
            }
        }

        public override void Reset()
        {
            base.Reset();
            MoveSpeed = 0;
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
            MaxQTJ = 0;
            MaxTLF = 0;
            BattleLong = new TimeSpan(0);
            ST.Reset();
            WillAppendNamedBattle = "";
            NamedBattleRes = new List<string>();
            InitialDetectionTime = null;
            HasConfirmedDX9 = false;
            TotalMonsterCount = 0;  // 重置撞怪计数器
        }

        public override bool NeedBlockCtrlEnter()
        {
            return false;
        }

        public override string GetMoreInfo()
        {
            // 在战斗中显示实时数据（功能2）
            int displayHCG = MaxHCG + (IsInBattle ? CurrentBattleHCG : (int)0);
            int displayXLL = MaxXLL + (IsInBattle ? CurrentBattleXLL : (int)0);
            int displayLQJ = MaxLQJ + (IsInBattle ? CurrentBattleLQJ : (int)0);
            
            if (IsShowSpeed)
            {
                return MoveSpeed.ToString("F2") + "   " + "蜂" + MaxFC + " 蜜" + MaxFM + " 火" + displayHCG + " 血" + displayXLL + " 夜" + MaxYXY + " 剑" + displayLQJ + ((MaxTLF > 0) ? (" 土" + MaxTLF) : "") + ((MaxQTJ > 0) ? (" 甲" + MaxQTJ) : "") + " 怪" + TotalMonsterCount;
            }
            else
            {
                return "蜂" + MaxFC + " 蜜" + MaxFM + " 火" + displayHCG + " 血" + displayXLL + " 夜" + MaxYXY + " 剑" + displayLQJ + ((MaxTLF > 0) ? (" 土" + MaxTLF) : "") + ((MaxQTJ > 0) ? (" 甲" + MaxQTJ) : "") + " 怪" + TotalMonsterCount;
            }
        }

        public override string GetSmallWatch()
        {
            return BattleLong.TotalSeconds.ToString("F2") + "s";
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

        public override string GetPointEnd()
        {
            return "预计通关 " + GetWillClearStr();
        }

        public override string GetPointSpan()
        {
            if (PointSpanName == "") return "--";
            return PointSpanName + " " + GetPointSpanStr();
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

        public delegate void OnExSuccess();
        public delegate void ExEnd(bool IsOK, string ErrStr);

        private bool IsListenSave = false;
        private string[] tnbase = new string[60] {
            "0","1","2","3","4","5","6","7","8","9",
            "A","B","C","D","E","F","G","H","I","J",
            "K","L","M","N","P","Q","R","S","T",
            "U","V","W","X","Y","Z","a","b","c","d",
            "e","f","g","h","i","j","k","m","n",
            "o","p","q","r","s","t","u","v","w","x",
            "y","z"
        };

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
            return palpath + "\\";
        }

        private void UI_SaveGameEx(FormEx f,OnExSuccess cb, string fn = "SRPG.bin")
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

        private void SaveGameEx(FormEx f,ExEnd cb, string fn = "SRPG.bin")
        {
            if (!GetPalHandle()) throw new Exception("游戏没有在运行，无法保存");
            IsListenSave = true;
            FormEx.Run(delegate ()
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
                        f.UI(delegate ()
                        {
                            cb(false, "");
                        });
                    }
                    return;
                }

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
                MaxFC = ho.GetValue<short>("BeeHouse");
                MaxFM = ho.GetValue<short>("BeeSheet");
                MaxHCG = ho.GetValue<short>("FireWorm");
                MaxLQJ = ho.GetValue<short>("DragonSword");
                MaxXLL = ho.GetValue<short>("BloodLink");
                MaxYXY = ho.GetValue<short>("NightCloth");
                MaxTLF = ho.GetValue<short>("EarthPaper");
                MaxQTJ = ho.GetValue<short>("CuArmor");
                MT.SetTS(ConvertTimeSpan(ho.GetValue<string>("Current")));
                ST.SetTS(ConvertTimeSpan(ho.GetValue<string>("Idle")));
                HObj cps = ho.GetValue<HObj>("CheckPoints");
                for (int i = 0; i < cps.Count; ++i)
                {
                    HObj cc = cps.GetValue<HObj>(i);
                    CheckPoints[i].SetCurrentTSForLoad(ConvertTimeSpan(cc.GetValue<string>("time")));
                }
                Jump(ho.GetValue<int>("Step"));
                string nmbs= ho.GetValue<string>("NamedBattles");
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

        private string GetTimeName()
        {
            if (form.CloudID() < 0) throw new Exception("云功能没有初始化");
            string res = form.CloudID().ToString().PadLeft(3, '0');
            DateTime now = DateTime.Now;
            res += tnbase[now.Month] + tnbase[now.Day] + tnbase[now.Hour] + tnbase[now.Minute] + tnbase[now.Second];
            return res;
        }

        public void LoadCloudSRPG(FormEx f,string code, Download dw)
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
                            isw.lblInfo.Text = "存档导入成功，计时器已自动暂停，请读取游戏中\"进度一\"后关闭此窗口";
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

        private ToolStripMenuItem btnCloudSave;
        private ToolStripMenuItem btnCloudLoad;
        private ToolStripMenuItem btnSwitch;
        private ToolStripMenuItem btnGameSpeedShow;

        public override void InitUI()
        {
            var btnExportCurrent = form.NewMenuItem();
            btnExportCurrent.Text = "导出本次成绩";
            btnExportCurrent.Click += delegate(object sender, EventArgs e) {
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
                UI_SaveGameEx(form,delegate() {
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
                isw.lblInfo.Text = "存档导入成功，计时器已自动暂停，请读取游戏中'进度一'后关闭此窗口";
                isw.btnOK.Text = "我已读档";
                isw.ShowDialog(form);
                SetUIPause(false);
            };

            btnSwitch= form.NewMenuItem();
            btnSwitch.Text = "切换至简版";
            btnSwitch.Click += delegate (object sender, EventArgs e) {
                if (form.Confirm("更换内核将会重置计时器，确认么？"))
                {
                    LoadCore(new 简版(form));
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
            };

            btnCloudSave = form.NewCloudMenuItem();
            btnCloudSave.Text = "云存档";
            btnCloudSave.Enabled = false;
            btnCloudSave.Click += delegate (object sender, EventArgs e) {
                string tn = GetTimeName();
                string fn = tn + ".bin";
                UI_SaveGameEx(form,delegate ()
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
                Download dw = new Download(delegate(string c,Download d) {
                    LoadCloudSRPG(form, c, d);
                });
                dw.ShowDialog(form);
            };
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

            PreData();
        }

        private void PreData()
        {
            SI.ins.MT = MT.ToString();
            SI.ins.ST = ST.ToString();
            SI.ins.MoreInfo = this.GetMoreInfo();
            SI.ins.GameVersion = this.GetGameVersion();
            SI.ins.Version = GForm.CurrentVersion;
            SI.ins.BattleLong= BattleLong.TotalSeconds.ToString("F2") + "s";
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

        private bool GetPalHandle()
        {
            Process[] res = Process.GetProcessesByName("Pal");
            
            // 功能2: 过滤已退出的进程
            if (res.Length > 1)
            {
                var aliveProcesses = res.Where(p => {
                    try { return !p.HasExited; }
                    catch { return false; }
                }).ToArray();
                
                if (aliveProcesses.Length > 1)
                {
                    if (!HasAlertMutiPal)
                    {
                        //cryerror = "检测到多个Pal.exe进程，请关闭其他的，只保留一个！";
                        HasAlertMutiPal = true;
                    }
                    return false;
                }
                res = aliveProcesses;
            }

            HasAlertMutiPal = false;
            if (res.Length > 0)
            {
                if (PID == -1)
                {
                    IntPtr tempHandle = res[0].MainWindowHandle;
                    
                    // 处理窗口句柄可能为空的情况（窗口转换期间）
                    if (tempHandle == IntPtr.Zero)
                    {
                        // 窗口正在转换，给予宽限期
                        if (InitialDetectionTime == null)
                        {
                            InitialDetectionTime = DateTime.Now;
                        }
                        
                        TimeSpan elapsedTime = DateTime.Now - InitialDetectionTime.Value;
                        if (elapsedTime.TotalSeconds < DX9TitleGracePeriodSeconds)
                        {
                            return false;  // 继续等待，不显示错误
                        }
                        else
                        {
                            cryerror = "请使用仙剑98 DX9移植版！窗口句柄无效";
                            return false;
                        }
                    }
                    
                    StringBuilder sb = new StringBuilder(256);
                    User32.GetWindowText(tempHandle, sb, sb.Capacity);
                    string windowTitle = sb.ToString();
                    
                    // 处理窗口标题为空或仅包含空白字符的情况（窗口转换期间）
                    if (string.IsNullOrWhiteSpace(windowTitle))
                    {
                        // 窗口标题为空，可能正在转换
                        if (InitialDetectionTime == null)
                        {
                            InitialDetectionTime = DateTime.Now;
                        }
                        
                        TimeSpan elapsedTime = DateTime.Now - InitialDetectionTime.Value;
                        if (elapsedTime.TotalSeconds < DX9TitleGracePeriodSeconds)
                        {
                            return false;  // 继续等待，不显示错误
                        }
                        else
                        {
                            cryerror = "请使用仙剑98 DX9移植版！无法获取窗口标题";
                            return false;
                        }
                    }
                    
                    // 检查是否包含DX9标识
                    bool hasDX9Title = (windowTitle.Contains("仙剑奇侠传") && windowTitle.Contains("DX9移植版")) ||
                        (windowTitle.Contains("仙剑奇侠传") && windowTitle.Contains("新补丁")) ||
                        (windowTitle.Contains("仙剑奇侠传") && windowTitle.Contains("(v")) ||
                                       (windowTitle.Contains("仙剑") && windowTitle.Contains("DX9"));
                    
                    // 检查是否是基础游戏标题（PAL.DLL还未修改标题，或VB4初始窗口）
                    bool isBaseGameTitle = windowTitle.Contains("仙剑奇侠传") || 
                                          windowTitle.StartsWith("PAL98") || 
                                          windowTitle.StartsWith("Pal98") ||
                                          windowTitle.Equals("sdf", StringComparison.OrdinalIgnoreCase);  // VB4初始窗口
                    
                    if (hasDX9Title)
                    {
                        // 找到DX9标题，提取版本号并连接
                        int versionStartIndex = windowTitle.IndexOf("(v");
                        if (versionStartIndex != -1)
                        {
                            int versionEndIndex = windowTitle.IndexOf(")", versionStartIndex);
                            if (versionEndIndex != -1)
                            {
                                DX9Version = windowTitle.Substring(versionStartIndex + 2, versionEndIndex - versionStartIndex - 2);
                            }
                        }
                        else
                        {
                            int versionStartIndex_old1 = windowTitle.IndexOf("(新补丁");
                            if (versionStartIndex_old1 != -1)
                            {
                                int versionEndIndex = windowTitle.IndexOf(" 测试版)", versionStartIndex_old1);
                                if (versionEndIndex != -1)
                                {
                                    DX9Version = windowTitle.Substring(versionStartIndex_old1 + 4, versionEndIndex - versionStartIndex_old1 - 3);
                                }
                            }
                            else 
                            {
                                int versionStartIndex_old2 = windowTitle.IndexOf("(");
                                if (versionStartIndex_old2 != -1)
                                {
                                    int versionEndIndex = windowTitle.IndexOf(")", versionStartIndex_old2);
                                    if (versionEndIndex != -1)
                                    {
                                        DX9Version = windowTitle.Substring(versionStartIndex_old2, versionEndIndex - versionStartIndex_old2 - 3);
                                    }
                                }
                            }
                        }
                        
                        PalProcess = res[0];
                        GameWindowHandle = res[0].MainWindowHandle;
                        PID = PalProcess.Id;
                        PalHandle = new IntPtr(Kernel32.OpenProcess(0x1F0FFF, false, PID));
                        CalcPalMD5();
                        HasConfirmedDX9 = true;
                        InitialDetectionTime = null;  // 重置初始检测时间
                        return true;
                    }
                    else if (isBaseGameTitle)
                    {
                        // 检测到基础游戏标题，给PAL.DLL时间加载和修改标题
                        if (InitialDetectionTime == null)
                        {
                            InitialDetectionTime = DateTime.Now;
                        }
                        
                        TimeSpan elapsedTime = DateTime.Now - InitialDetectionTime.Value;
                        
                        if (elapsedTime.TotalSeconds < DX9TitleGracePeriodSeconds)
                        {
                            // 在宽限期内，暂时接受，并继续检测
                            // 但不设置PID，这样下次还会重新检查标题
                            return false;  // 返回false但不设置错误，继续等待
                        }
                        else
                        {
                            // 超过宽限期仍未出现DX9标题，显示错误
                            //cryerror = "请使用仙剑98 新补丁DX9移植版！检测到基础游戏但未找到DX9标识";
                            return false;
                        }
                    }
                    else
                    {
                        // 既不是DX9标题也不是基础游戏标题
                        //cryerror = "请使用仙剑98 新补丁DX9移植版！";
                        return false;
                    }
                }
                else
                {
                    if (PID == res[0].Id)
                    {
                        // 已连接到游戏，但如果还没确认DX9，继续检查标题
                        if (!HasConfirmedDX9)
                        {
                            IntPtr tempHandle = res[0].MainWindowHandle;
                            StringBuilder sb = new StringBuilder(256);
                            User32.GetWindowText(tempHandle, sb, sb.Capacity);
                            string windowTitle = sb.ToString();
                            
                            bool hasDX9Title = (windowTitle.Contains("仙剑奇侠传") && windowTitle.Contains("DX9移植版")) || 
                                               (windowTitle.Contains("仙剑") && windowTitle.Contains("DX9"));
                            
                            if (hasDX9Title)
                            {
                                // 提取版本号
                                int versionStartIndex = windowTitle.IndexOf("(v");
                                if (versionStartIndex != -1)
                                {
                                    int versionEndIndex = windowTitle.IndexOf(")", versionStartIndex);
                                    if (versionEndIndex != -1)
                                    {
                                        DX9Version = windowTitle.Substring(versionStartIndex + 2, versionEndIndex - versionStartIndex - 2);
                                    }
                                }
                                HasConfirmedDX9 = true;
                            }
                        }
                        
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
                        DX9Version = "未知";
                        InitialDetectionTime = null;
                        HasConfirmedDX9 = false;
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
                DX9Version = "未知";
                InitialDetectionTime = null;
                HasConfirmedDX9 = false;
                return false;
            }
        }

        private void CalcPalMD5()
        {
            try
            {
                string dllmd5 = GetFileMD5(GetGameFilePath("Pal.dll"));
                string datamd5 = GetFileMD5(GetGameFilePath("DATA.MKF"));
                string sssmd5 = GetFileMD5(GetGameFilePath("SSS.MKF"));
                string vb40032md5 = GetFileMD5(GetGameFilePath("VB40032.dll"));
                GMD5 = dllmd5 + "_" + datamd5 + "_" + sssmd5 + "_" + vb40032md5;
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
            short lastx = GameObj.X;
            short lasty = GameObj.Y;
            short lastarea = GameObj.Area;

            GameObj.Flush(PalHandle, PID, 0, 0);
            FlushPlugins(PalHandle, PID, 0, 0);

            try
            {
                float speed = 0;
                if (GameObj.Area == lastarea)
                {
                    DateTime now = DateTime.Now;
                    if (now.Second % 5 == 0)
                    {
                        MoveSpeed = 0;
                    }
                    TimeSpan ts = now - LastFlushTime;
                    LastFlushTime = now;
                    short xslot = (short)(Math.Abs(GameObj.X - lastx) / 16);
                    short yslot = (short)(Math.Abs(GameObj.Y - lasty) / 8);
                    short scha = xslot;
                    if (yslot > scha)
                    {
                        scha = yslot;
                    }
                    float muti = (float)(1000 / ts.TotalMilliseconds);
                    speed = muti * scha;
                }
                else
                {
                    speed = 0;
                }
                if (speed > MoveSpeed)
                {
                    MoveSpeed = speed;
                }
            }
            catch
            { }
        }

        private void BattleBegin()
        {
            BattleLong = new TimeSpan(0);
            InBattleTime = DateTime.Now;
            biw = new BattleItemWatch();
            TotalMonsterCount++;  // 撞怪计数器增加
            
            // 重置战斗中实时显示的临时变量（功能2）
            CurrentBattleHCG = 0;
            CurrentBattleXLL = 0;
            CurrentBattleLQJ = 0;
            
            if (CurrentStep <= 5)
            {
                //战斗前记录下个数
                biw.Insert(0x73, GameObj.GetItemCount(0x73));//蜂
                biw.Insert(0x83, GameObj.GetItemCount(0x83));//蜜
                biw.Insert(0x8F, GameObj.GetItemCount(0x8F));//火
            }
            else
            {
                // 如果不在前5关，也要记录火虫草的初始值以便实时统计
                biw.Insert(0x8F, GameObj.GetItemCount(0x8F));//火
            }
            biw.Insert(0xB8, GameObj.GetItemCount(0xB8));//龙泉剑
            biw.Insert(0xA2, GameObj.GetItemCount(0xA2));//血玲珑
            biw.Insert(0xD4, GameObj.GetItemCount(0xD4));//夜行衣

            biw.Insert(0x47, GameObj.GetItemCount(0x47));//土灵符
            biw.Insert(0xD5, GameObj.GetItemCount(0xD5));//青铜甲
            CurrentNamedBattle = GameObj.GetNamedBattle();
        }

        private void Battling()
        {
            BattleLong = DateTime.Now - InBattleTime;
            biw.SetCount(GameObj);
            
            // 实时更新火、血、剑的数量（功能2）
            CurrentBattleHCG = biw.GettedCount(0x8F);  // 火虫草
            CurrentBattleXLL = biw.GettedCount(0xA2);  // 血玲珑
            CurrentBattleLQJ = biw.GettedCount(0xB8);  // 龙泉剑
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
            biw.SetCount(GameObj);
            if (CurrentStep <= 5)
            {
                //将算出来的差加入显示
                MaxFC += biw.GettedCount(0x73);
                MaxFM += biw.GettedCount(0x83);
                // MaxHCG 已在Battling中实时更新，这里累加最终值
                MaxHCG += biw.GettedCount(0x8F);
            }
            // MaxLQJ 和 MaxXLL 已在Battling中实时更新，这里累加最终值
            MaxLQJ += biw.GettedCount(0xB8);
            MaxXLL += biw.GettedCount(0xA2);
            MaxYXY += biw.GettedCount(0xD4);

            MaxTLF += biw.GettedCount(0x47);
            MaxQTJ += biw.GettedCount(0xD5);
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
            exdata["Idle"] = ST.ToFullString();
            exdata["Lite"] = LT.ToFullString();
            exdata["BeeHouse"] = MaxFC;
            exdata["BeeSheet"] = MaxFM;
            exdata["FireWorm"] = MaxHCG;
            exdata["DragonSword"] = MaxLQJ;
            exdata["BloodLink"] = MaxXLL;
            exdata["NightCloth"] = MaxYXY;
            exdata["EarthPaper"] = MaxTLF;
            exdata["CuArmor"] = MaxQTJ;
            exdata["GMD5"] = GMD5;
            exdata["DX9Version"] = DX9Version;
            exdata["TotalMonsterCount"] = TotalMonsterCount;  // 保存撞怪总数

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
                        LoadCore(new 简版(form));
                    }
                    break;
            }
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
            return MT.CurrentTS.Ticks.ToString() + "," + ST.CurrentTS.Ticks.ToString() + "," + BattleLong.Ticks.ToString();
        }

        // Override LoadPlugins to also load PAL98 plugins for compatibility
        public override void LoadPlugins()
        {
            // First load PAL98DX9-specific plugins
            base.LoadPlugins();
            
            // Then also load PAL98 plugins for compatibility
            // This allows PAL98 plugins (like bottom-left money/item display) to work with DX9
            string pluginPath = TimerPluginPackageInfo.GetPluginDir();
            if (!System.IO.Directory.Exists(pluginPath)) return;
            
            System.IO.DirectoryInfo root = new System.IO.DirectoryInfo(pluginPath);
            System.IO.FileInfo[] files = root.GetFiles();
            foreach (var f in files)
            {
                string sn = f.FullName.Replace(pluginPath, "");
                if (sn.StartsWith("PAL98.") && sn.EndsWith(".tpg"))
                {
                    string tpsfile = f.FullName;
                    if (System.IO.File.Exists(tpsfile))
                    {
                        try
                        {
                            // Use reflection to call private _loadOnePlugin method
                            var method = typeof(TimerCore).GetMethod("_loadOnePlugin", 
                                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                            if (method != null)
                            {
                                method.Invoke(this, new object[] { tpsfile });
                            }
                        }
                        catch
                        { }
                    }
                }
            }
        }
    }
}
