using HFrame.ENT;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Pal98Timer
{
    public delegate void LoadCoreDel(TimerCore core);
    public abstract class TimerCore
    {
        public static List<string> GetAllCores()
        {
            List<string> res = new List<string>();

            var tct = typeof(TimerCore);
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (tct.IsAssignableFrom(type))
                    {
                        if (type.IsClass && !type.IsAbstract)
                        {
                            string[] t = type.ToString().Split('.');
                            res.Add(t[t.Length - 1]);
                        }
                    }
                }
            }

            return res;
        }
        public static TimerCore GetCoreIns(string name)
        {
            return CreateInstance<TimerCore>("Pal98Timer." + name, "Pal98Timer");
        }
        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullName">命名空间.类型名</param>
        /// <param name="assemblyName">程序集</param>
        /// <returns></returns>
        public static T CreateInstance<T>(string fullName, string assemblyName)
        {
            string path = fullName + "," + assemblyName;//命名空间.类型名,程序集
            Type o = Type.GetType(path);//加载类型
            object obj = Activator.CreateInstance(o, true);//根据类型创建实例
            return (T)obj;//类型转换并返回
        }

        public LoadCoreDel LoadCore = null;
        public bool IsMakeSureCurPointView = true;
        public string CMD5 = "none";
        public TimerCore()
        {
            CMD5 = GetFileMD5(this.GetType().Assembly.Location);
        }
        public static string GetFileMD5(string fileName)
        {
            string res = "none";
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
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
        public static string TimeSpanToString(TimeSpan ts)
        {
            return ts.Hours.ToString().PadLeft(2, '0') + ":" + ts.Minutes.ToString().PadLeft(2, '0') + ":" + ts.Seconds.ToString().PadLeft(2, '0') + "." + (ts.Milliseconds / 10).ToString().PadLeft(2, '0');
        }

        public static string TimeSpanToStringLite(TimeSpan ts)
        {
            return ts.Hours.ToString().PadLeft(2, '0') + ":" + ts.Minutes.ToString().PadLeft(2, '0') + ":" + ts.Seconds.ToString().PadLeft(2, '0');
        }

        public static string GetChaStr(long cha)
        {
            string res = "";
            if (cha < 0)
            {
                res = "-";
            }
            else
            {
                res = "+";
            }
            cha = Math.Abs(cha);
            if (cha < 60)
            {
                res += "0:" + cha.ToString().PadLeft(2, '0');
            }
            else
            {
                long mt = cha / 60;
                int st = (int)(cha % 60);
                res += mt.ToString() + ":" + st.ToString().PadLeft(2, '0');
            }
            return res;
        }

        public static TimeSpan ConvertTimeSpan(string str)
        {
            try
            {
                string[] spli = str.Replace(".", ":").Split(':');
                return new TimeSpan(0, int.Parse(spli[0]), int.Parse(spli[1]), int.Parse(spli[2]), (spli.Length > 3 ? (int.Parse(spli[3])) : 0));
            }
            catch
            {
                return new TimeSpan(0, 0, 0, 0, 0);
            }
        }

        public void Jump(int index)
        {
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
            CurrentStep = index;
        }

        protected string BestFile = "best.txt";
        protected Dictionary<string, CheckPointNewer> Best = null;
        protected void LoadBest()
        {
            if (File.Exists(BestFile))
            {
                string beststr = "";
                using (FileStream fileStream = new FileStream(BestFile, FileMode.Open))
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
        }
        protected CheckPointNewer GetBest(string Name,TimeSpan def=default(TimeSpan))
        {
            if (Best == null) return new CheckPointNewer()
            {
                Name = Name,
                NickName = "",
                BestTS = def
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
                    BestTS = def
                };
            }
        }
        protected HObj Data = new HObj();
        public List<CheckPoint> CheckPoints = null;
        public abstract void InitCheckPoints();
        protected int _CurrentStep = -1;
        public int CurrentStep
        {
            get
            {
                return _CurrentStep;
            }
            set
            {
                //SI.ins.CurrentStep = value;
                _CurrentStep = value;
                if (OnCurrentStepChangedInner != null)
                {
                    OnCurrentStepChangedInner(value);
                }
                if (OnCurrentStepChanged != null)
                {
                    OnCurrentStepChanged(value);
                }
            }
        }
        public delegate void CurrentStepChangeDel(int currentidx);
        public CurrentStepChangeDel OnCurrentStepChanged = null;
        protected CurrentStepChangeDel OnCurrentStepChangedInner = null;

        public abstract string GetMoreInfo();
        public abstract string GetSmallWatch();
        public abstract string GetPointEnd();
        public abstract string GetSecondWatch();
        public abstract string GetMainWatch();
        public abstract string GetGameVersion();
        public abstract void Reset();
        public abstract void InitUI(NewForm form);
        public abstract void Start();
        public abstract void SetTS(TimeSpan ts);
        public abstract void OnFunctionKey(int FunNo, NewForm form);
        public abstract string GetAAction();
        public abstract bool IsShowC();
        public abstract bool NeedBlockCtrlEnter();

        protected List<Control> CustomUIC = new List<Control>();
        protected List<ToolStripMenuItem> CustomUIT = new List<ToolStripMenuItem>();
        public virtual void UnloadUI(NewForm form)
        {
            if (CustomUIC != null)
            {
                foreach (Control c in CustomUIC)
                {
                    if (c is Button)
                    {
                        c.Parent.Controls.Remove(c);
                    }
                }
            }
            if (CustomUIT != null)
            {
                foreach (ToolStripMenuItem t in CustomUIT)
                {
                    t.GetCurrentParent().Items.Remove(t);
                }
            }
        }
        public void AddUIC(Control c)
        {
            CustomUIC.Add(c);
        }
        public void AddUIT(ToolStripMenuItem c)
        {
            CustomUIT.Add(c);
        }

        public abstract void Unload();
        public abstract string GetCriticalError();
    }


    public class PTimer
    {
        protected TimeSpan __CurrentTS = new TimeSpan(0);
        protected int _Status = 0;//0:stoped 1:started
        protected DateTime _LastTime;
        protected TimeSpan _BackupCurrent;
        protected bool HasMSet = false;
        //private System.Windows.Forms.Timer tm;
        private PTimer _Combine = null;
        private Stopwatch sw;
        public PTimer()
        {
            /*tm = new System.Windows.Forms.Timer();
            tm.Interval = 10;
            tm.Tick += tm_Tick;
            tm.Enabled = true;*/
            sw = new Stopwatch();
        }
        public void SetCombine(PTimer ct)
        {
            this._Combine = ct;
        }
        public void UnCombine()
        {
            this._Combine = null;
        }
        protected TimeSpan _CurrentTS
        {
            set
            {
                __CurrentTS = value;
                if (sw != null)
                {
                    sw.Reset();
                }
            }
            get
            {
                if (sw != null)
                {
                    return __CurrentTS.Add(sw.Elapsed);
                }
                return __CurrentTS;
            }
        }
        /*void tm_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            if (_Status == 1)
            {
                _CurrentTS = _CurrentTS.Add(now.Subtract(_LastTime));
            }
            _LastTime = now;
        }*/
        public TimeSpan CurrentTS
        {
            get
            {
                if (_Combine == null)
                {
                    return _CurrentTS;
                }
                else
                {
                    return _CurrentTS.Add(_Combine.CurrentTS);
                }
            }
        }
        public TimeSpan CurrentTSOnly
        {
            get
            {
                return _CurrentTS;
            }
        }
        public void SetTS(TimeSpan ts)
        {
            HasMSet = true;
            _BackupCurrent = new TimeSpan(_CurrentTS.Days, _CurrentTS.Hours, _CurrentTS.Minutes, _CurrentTS.Seconds, _CurrentTS.Milliseconds);
            this._CurrentTS = ts;
            if (_Combine != null)
            {
                _Combine.SetTS(new TimeSpan(0, 0, 0, 0, 0));
            }
        }
        public void UnSetTs()
        {
            if (HasMSet)
            {
                HasMSet = false;
                _CurrentTS = new TimeSpan(_BackupCurrent.Days, _BackupCurrent.Hours, _BackupCurrent.Minutes, _BackupCurrent.Seconds, _BackupCurrent.Milliseconds);
                if (_Combine != null)
                {
                    _Combine.UnSetTs();
                }
            }
        }
        public override string ToString()
        {
            return CurrentTS.Hours.ToString().PadLeft(2, '0') + ":" + CurrentTS.Minutes.ToString().PadLeft(2, '0') + ":" + CurrentTS.Seconds.ToString().PadLeft(2, '0') + "." + (CurrentTS.Milliseconds / 10).ToString().PadLeft(2, '0');
        }
        public void Start()
        {
            if (_Status != 1)
            {
                _Status = 1;
                //_LastTime = DateTime.Now;
                sw.Start();
            }
        }
        public void Stop()
        {
            _Status = 0;
            sw.Stop();
        }
        public void Reset()
        {
            _CurrentTS = new TimeSpan(0);
            sw.Reset();
        }
    }


    public delegate bool Checker();
    [Serializable]
    public class CheckPoint
    {
        public string Name;
        public string NickName = "";
        public TimeSpan Best;
        public TimeSpan Current;
        public Checker Check;
        public bool IsBegin = false;
        public bool IsEnd = false;
        public int Index = -1;
        public CheckPoint(int index, CheckPointNewer n)
        {
            this.Index = index;
            Current = new TimeSpan(0, 0, 0, 0, 0);
            Name = n.Name;
            NickName = n.NickName;
            Best = n.BestTS;
        }
        public string GetNickName()
        {
            if (NickName != "")
            {
                return NickName;
            }
            return Name;
        }
        public long GetCHA()
        {
            long chas = 0;
            if (Current.Ticks == 0)
            {
                return 0;
            }
            if (Current < Best)
            {
                chas = ((long)((Best - Current).TotalSeconds)) * -1;
            }
            else
            {
                chas = ((long)((Current - Best).TotalSeconds));
            }
            return chas;
        }
    }
    [Serializable]
    public class CheckPointNewer
    {
        public string Name;
        public string NickName = "";
        public TimeSpan BestTS;
    }
}
