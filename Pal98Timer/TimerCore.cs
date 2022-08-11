using HFrame.ENT;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using TimerPluginBase;

namespace Pal98Timer
{
    public delegate void LoadCoreDel(TimerCore core);
    /// <summary>
    /// 计时内核
    /// </summary>
    public abstract class TimerCore
    {
        /// <summary>
        /// 主计时器
        /// </summary>
        protected PTimer MT = new PTimer();
        /// <summary>
        /// 枚举出所有继承于TimerCore的类的短名
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 通过继承于TimerCore的类短名实例化TimerCore
        /// </summary>
        /// <param name="name">短类名</param>
        /// <param name="fm">主界面实例</param>
        /// <returns></returns>
        public static TimerCore GetCoreIns(string name,GForm fm)
        {
            return CreateInstance<TimerCore>("Pal98Timer." + name, "Pal98Timer", fm);
        }
        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullName">命名空间.类型名</param>
        /// <param name="assemblyName">程序集</param>
        /// <param name="fm">主界面实例</param>
        /// <returns></returns>
        public static T CreateInstance<T>(string fullName, string assemblyName, GForm fm)
        {
            string path = fullName + "," + assemblyName;//命名空间.类型名,程序集
            Type o = Type.GetType(path);//加载类型
            object obj = Activator.CreateInstance(o,  fm);//根据类型创建实例
            return (T)obj;//类型转换并返回
        }

        /// <summary>
        /// 用于给其他地方方便调用加载内核的委托
        /// </summary>
        public LoadCoreDel LoadCore = null;
        /// <summary>
        /// 是否让当前节点处于视野内（即将废弃）
        /// </summary>
        public bool IsMakeSureCurPointView = true;
        /// <summary>
        /// 本程序的MD5
        /// </summary>
        public string CMD5 = "none";
        /// <summary>
        /// 主界面
        /// </summary>
        protected GForm form = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="form"></param>
        public TimerCore(GForm form)
        {
            this.form = form;
            CMD5 = GetFileMD5(this.GetType().Assembly.Location);
        }
        /// <summary>
        /// 计算文件的MD5
        /// </summary>
        /// <param name="fileName">文件的路径</param>
        /// <returns></returns>
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
        /// <summary>
        /// TimeSpan友好化字符串
        /// </summary>
        /// <param name="ts"></param>
        /// <returns>HH:mm:ss.ff</returns>
        public static string TimeSpanToString(TimeSpan ts)
        {
            return ts.Hours.ToString().PadLeft(2, '0') + ":" + ts.Minutes.ToString().PadLeft(2, '0') + ":" + ts.Seconds.ToString().PadLeft(2, '0') + "." + (ts.Milliseconds / 10).ToString().PadLeft(2, '0');
        }
        /// <summary>
        /// TimeSpan友好化字符串（简）
        /// </summary>
        /// <param name="ts"></param>
        /// <returns>HH:mm:ss</returns>
        public static string TimeSpanToStringLite(TimeSpan ts)
        {
            return ts.Hours.ToString().PadLeft(2, '0') + ":" + ts.Minutes.ToString().PadLeft(2, '0') + ":" + ts.Seconds.ToString().PadLeft(2, '0');
        }
        /// <summary>
        /// 秒数友好化字符串
        /// </summary>
        /// <param name="cha">秒</param>
        /// <returns>+m:ss</returns>
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
        /// <summary>
        /// 把友好化的字符串转成TimeSpan
        /// </summary>
        /// <param name="str">HH:mm:ss[.ff]</param>
        /// <returns></returns>
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
        /// <summary>
        /// 跳转到第n个时间节点
        /// </summary>
        /// <param name="index"></param>
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
            SendPluginsEvent("Jump", index);
        }

        /// <summary>
        /// 用于保存文件的内核名
        /// </summary>
        public string CoreName = "";
        /// <summary>
        /// 从最佳里加载的时间线
        /// </summary>
        protected Dictionary<string, CheckPointNewer> Best = null;
        /// <summary>
        /// 从文件里加载最佳时间线
        /// </summary>
        protected void LoadBest()
        {
            string BestFile = "best" + CoreName + ".txt";
            if (File.Exists(BestFile))
            {
                string beststr = "";
                Encoding charset = GetFileEncodeType(BestFile);
                using (FileStream fileStream = new FileStream(BestFile, FileMode.Open))
                {
                    using (StreamReader streamReader = new StreamReader(fileStream, charset))
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
                SendPluginsEvent("LoadBest", beststr);
            }
            else
            {
                Best = null;
            }
        }
        /// <summary>
        /// 通过节点名称获取对应的最佳时间
        /// </summary>
        /// <param name="Name">节点name</param>
        /// <param name="def">如无该节点则使用这个最佳时间</param>
        /// <returns></returns>
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
        
        
        /// <summary>
        /// 保存最佳文件
        /// </summary>
        /// <param name="str"></param>
        protected void SaveBest(string str)
        {
            DateTime now = DateTime.Now;
            string filename = "best" + CoreName + ".txt";
            string snow= now.ToString("yyyyMMddHHmmss");
            try
            {
                if (File.Exists(filename))
                {
                    File.Move(filename, "best" + CoreName + snow + ".txt");
                }
                using (FileStream fileStream = new FileStream(filename, FileMode.Create))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                    {
                        streamWriter.Write(str);
                        //streamWriter.Flush();
                    }
                }
                if (form.Confirm("保存成功，确定要重置计时器么？"))
                {
                    form._ResetAll();
                }
                SendPluginsEvent("SaveBest", filename);
            }
            catch (Exception ex)
            {
                form.Error("保存失败：" + ex.Message);
            }
        }

        public void SaveBest()
        {
            DateTime now = DateTime.Now;
            string filename = "best" + CoreName + ".txt";
            string snow = now.ToString("yyyyMMddHHmmss");

            if (File.Exists(filename))
            {
                File.Move(filename, "best" + CoreName + snow + ".txt");
            }
            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    streamWriter.Write(GetRStr());
                    //streamWriter.Flush();
                }
            }
            SendPluginsEvent("SaveBest", filename);
        }
        /// <summary>
        /// 导出当前成绩
        /// </summary>
        /// <param name="str"></param>
        protected void ExportCurrent(string str)
        {
            DateTime now = DateTime.Now;
            string filename = now.ToString("yyyyMMddHHmmss");

            try
            {
                using (FileStream fileStream = new FileStream(CoreName + filename + ".txt", FileMode.Create))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                    {
                        streamWriter.Write(str);
                        //streamWriter.Flush();
                    }
                }
                form.Success("已将此次成绩保存至" + CoreName + filename + ".txt");
                SendPluginsEvent("ExportCurrent", CoreName + filename + ".txt");
            }
            catch (Exception ex)
            {
                form.Error("保存失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 临时数据
        /// </summary>
        protected HObj Data = new HObj();
        /// <summary>
        /// 时间节点
        /// </summary>
        public List<CheckPoint> CheckPoints = null;
        /// <summary>
        /// 初始化时间节点
        /// </summary>
        protected abstract void InitCheckPoints();
        /// <summary>
        /// 即将通关时间
        /// </summary>
        protected TimeSpan WillClear = new TimeSpan(0);
        /// <summary>
        /// 最佳通关时间
        /// </summary>
        protected TimeSpan BestClear = new TimeSpan(0);
        /// <summary>
        /// 供主界面调用的节点初始化
        /// </summary>
        public void InitCheckPointsEx()
        {
            InitCheckPoints();
            if (CheckPoints!=null && CheckPoints.Count > 0)
            {
                BestClear = new TimeSpan(CheckPoints[CheckPoints.Count - 1].Best.Ticks);
                WillClear = new TimeSpan(BestClear.Ticks);
            }
            SendPluginsEvent("InitCheckPoints", null);
        }
        /// <summary>
        /// 当前节点序号（别乱改）
        /// </summary>
        protected int _CurrentStep = -1;
        public string AAction = "";
        /// <summary>
        /// 当前节点序号
        /// </summary>
        public int CurrentStep
        {
            get
            {
                return _CurrentStep;
            }
            set
            {
                //SI.ins.CurrentStep = value;
                bool IsAdd = (value > _CurrentStep);
                _CurrentStep = value;

                if (_CurrentStep > 0 && _CurrentStep <= CheckPoints.Count)
                {
                    long cha = CheckPoints[_CurrentStep - 1].GetCHA() * 1000 * 10000;
                    if ((BestClear.Ticks + cha) <= 0)
                    {
                        WillClear = new TimeSpan(BestClear.Ticks);
                    }
                    else
                    {
                        WillClear = new TimeSpan(BestClear.Ticks + cha);
                    }
                    if (_CurrentStep == 1)
                    {
                        PointSpanName = "[0~1]";
                        PointSpan = new TimeSpan(CheckPoints[0].Current.Ticks);
                        if (form.IsShowPSInDots && IsAdd && PointSpan.Ticks > 0)
                        {
                            AAction += "|[开始~" + CheckPoints[0].Name + "] " + GetPointSpanStr();
                        }
                    }
                    else
                    {
                        PointSpanName = "[" + (_CurrentStep - 1) + "~" + (_CurrentStep) + "]";
                        TimeSpan tts = new TimeSpan(CheckPoints[_CurrentStep - 1].Current.Ticks - CheckPoints[_CurrentStep - 2].Current.Ticks);
                        if (tts.Ticks < 0)
                        {
                            PointSpan = new TimeSpan(0);
                        }
                        else
                        {
                            PointSpan = tts;
                        }
                        if (form.IsShowPSInDots && IsAdd && PointSpan.Ticks > 0)
                        {
                            AAction += "|[" + CheckPoints[_CurrentStep - 2].Name + "~" + CheckPoints[_CurrentStep - 1].Name + "] " + GetPointSpanStr();
                        }
                    }
                }
                else
                {
                    PointSpanName = "";
                    PointSpan = new TimeSpan(0);
                }

                OnCurrentStepChangedInner?.Invoke(value);
                OnCurrentStepChanged?.Invoke(value);
            }
        }
        /// <summary>
        /// 区间间隔
        /// </summary>
        protected TimeSpan PointSpan = new TimeSpan(0);
        /// <summary>
        /// 区间间隔名
        /// </summary>
        protected string PointSpanName = "";
        /// <summary>
        /// 检测节点是否触发的逻辑
        /// </summary>
        protected void Checking()
        {
            if (CurrentStep < 0 && CheckPoints.Count > 0)
            {
                CheckPoints[0].IsBegin = true;
                CurrentStep = 0;
            }

            if (CurrentStep < CheckPoints.Count)
            {
                CheckPoints[CurrentStep].Current = MT.CurrentTSOnly;
                if (CheckPoints[CurrentStep].Check())
                {
                    CheckPoints[CurrentStep].Current = new TimeSpan(MT.CurrentTSOnly.Ticks);
                    CheckPoints[CurrentStep].IsEnd = true;
                    //CurrentStep++;
                    int nextstep = CurrentStep + 1;
                    if (nextstep >= CheckPoints.Count)
                    {
                        OnCheckPointEnd();
                    }
                    else
                    {
                        CheckPoints[nextstep].IsBegin = true;
                    }
                    CurrentStep = nextstep;
                    //PostCloudRank();
                }
            }
            else
            {
                OnCheckPointEnd();
            }
        }
        private bool _hasCallPointEnd = false;
        protected virtual void OnCheckPointEnd()
        {
            MT.Stop();
            if (!_hasCallPointEnd)
            {
                _hasCallPointEnd = true;
                form.CallCloudFinishOne();
                SendPluginsEvent("OnCheckPointEnd", null);
            }
        }
        /// <summary>
        /// 是否在UI层面上暂停计时（比如手动暂停）
        /// </summary>
        public bool IsUIPause = false;
        /// <summary>
        /// 暂停，暂停次数不加
        /// </summary>
        /// <param name="isp"></param>
        protected void SetUIPause(bool isp)
        {
            form.SetUIPause(isp);
            SendPluginsEvent("SetUIPause", isp);
        }
        /// <summary>
        /// 手动暂停，暂停次数会加1
        /// </summary>
        protected void HandPause()
        {
            form.UIPause();
            SendPluginsEvent("HandPause", null);
        }
        public delegate void CurrentStepChangeDel(int currentidx);
        /// <summary>
        /// 节点序号改变时的事件
        /// </summary>
        public CurrentStepChangeDel OnCurrentStepChanged = null;
        /// <summary>
        /// 节点序号改变事件2
        /// </summary>
        protected CurrentStepChangeDel OnCurrentStepChangedInner = null;

        /// <summary>
        /// 主时间下方信息
        /// </summary>
        /// <returns></returns>
        public abstract string GetMoreInfo();
        /// <summary>
        /// 主时间左上方小时间
        /// </summary>
        /// <returns></returns>
        public abstract string GetSmallWatch();
        /// <summary>
        /// 预计通关
        /// </summary>
        /// <returns></returns>
        public virtual string GetPointEnd()
        {
            //PointSpan
            return "预计通关 " + GetWillClearStr();
        }
        public virtual string GetPointSpan()
        {
            if (PointSpanName == "") return "--";
            return PointSpanName + " " + GetPointSpanStr();
        }
        /// <summary>
        /// 获取预计结束的时长字符串
        /// </summary>
        /// <returns></returns>
        public string GetWillClearStr()
        {
            return TimeSpanToStringLite(WillClear);
        }
        /// <summary>
        /// 获取上一区间时长字符串
        /// </summary>
        /// <returns></returns>
        public string GetPointSpanStr()
        {
            return Math.Floor(PointSpan.TotalMinutes) + ":" + PointSpan.Seconds.ToString().PadLeft(2, '0') + "." + Math.Floor(0.1D * PointSpan.Milliseconds).ToString().PadLeft(2, '0');
        }
        /// <summary>
        /// 主时间右上方小时间
        /// </summary>
        /// <returns></returns>
        public abstract string GetSecondWatch();
        /// <summary>
        /// 主时间
        /// </summary>
        /// <returns></returns>
        public virtual TimeSpan GetMainWatch()
        {
            return MT.CurrentTS;
        }
        /// <summary>
        /// 主时间是否标星花
        /// </summary>
        /// <returns></returns>
        public abstract bool IsMainWatchStar();
        /// <summary>
        /// 游戏版本
        /// </summary>
        /// <returns></returns>
        public abstract string GetGameVersion();
        /// <summary>
        /// 重置
        /// </summary>
        public virtual void Reset()
        {
            AAction = "";
            MT.Reset();
            _hasCallPointEnd = false;
            PointSpanName = "";
            PointSpan = new TimeSpan(0);
        }
        /// <summary>
        /// 初始化界面
        /// </summary>
        public abstract void InitUI();
        /// <summary>
        /// 检测间隔（毫秒）
        /// </summary>
        protected int CheckInterval = 70;
        /// <summary>
        /// 是否继续检测线程？unload的时候设置为false就行
        /// </summary>
        private bool IsAllRun = true;
        /// <summary>
        /// 计时内核开始
        /// </summary>
        public void Start()
        {
            HFrame.EX.FormEx.Run(delegate () {
                while (IsAllRun)
                {
                    try
                    {
                        OnTick();
                    }
                    catch { }
                    System.Threading.Thread.Sleep(CheckInterval);
                }
            });
            SendPluginsEvent("Start", null);
        }
        /// <summary>
        /// 每次Tick调用的逻辑
        /// </summary>
        protected abstract void OnTick();
        /// <summary>
        /// 卸载本内核
        /// </summary>
        public virtual void Unload()
        {
            IsAllRun = false;
        }
        /// <summary>
        /// 设置主时间
        /// </summary>
        /// <param name="ts"></param>
        public void SetTS(TimeSpan ts)
        {
            MT.SetTS(ts);
            try
            {
                CheckPoints[CurrentStep].Current = ts;
                SendPluginsEvent("SetTS", ts);
            }
            catch { }
        }
        /// <summary>
        /// 当按下Fx快捷键通知计时内核
        /// </summary>
        /// <param name="FunNo">对应F几</param>
        public abstract void OnFunctionKey(int FunNo);
        /// <summary>
        /// 顶部豆豆增加
        /// </summary>
        /// <returns>返回空字符串为不增加，否则增加</returns>
        public abstract string GetAAction();
        /// <summary>
        /// 是否显示主时间左侧的红C
        /// </summary>
        /// <returns></returns>
        public abstract bool IsShowC();
        /// <summary>
        /// 是否需要屏蔽掉Ctrl+Enter按键组合
        /// </summary>
        /// <returns></returns>
        public abstract bool NeedBlockCtrlEnter();
        public virtual bool NeedBlockFunctionKey(int fnno)
        {
            return false;
        }
        /// <summary>
        /// 本内核添加的Form控件
        /// </summary>
        protected List<Control> CustomUIC = new List<Control>();
        /// <summary>
        /// 本内核添加的MenuItem控件
        /// </summary>
        protected List<ToolStripMenuItem> CustomUIT = new List<ToolStripMenuItem>();
        /// <summary>
        /// 本内核添加的Grender按钮
        /// </summary>
        protected List<GRender.GBtn> CustomUIGB = new List<GRender.GBtn>();
        /// <summary>
        /// 卸载本内核界面
        /// </summary>
        public virtual void UnloadUI()
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
            if (CustomUIGB != null)
            {
                foreach (GRender.GBtn b in CustomUIGB)
                {
                    b.Remove();
                }
            }
        }
        /// <summary>
        /// 添加属于本内核的Grender按钮
        /// </summary>
        /// <param name="b"></param>
        public void AddUIGB(GRender.GBtn b)
        {
            CustomUIGB.Add(b);
        }
        /// <summary>
        /// 添加属于本内核的Form控件
        /// </summary>
        /// <param name="c"></param>
        public void AddUIC(Control c)
        {
            CustomUIC.Add(c);
        }
        /// <summary>
        /// 添加属于本内核的MenuItem控件
        /// </summary>
        /// <param name="c"></param>
        public void AddUIT(ToolStripMenuItem c)
        {
            CustomUIT.Add(c);
        }
        /// <summary>
        /// 获取崩溃信息（如果有的话）
        /// </summary>
        /// <returns></returns>
        public abstract string GetCriticalError();
        /// <summary>
        /// 获取文件的字符集
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Encoding GetFileEncodeType(string filename)
        {
            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    BinaryReader br = new BinaryReader(fs);

                    byte[] buffer = br.ReadBytes(2);

                    if (buffer[0] >= 0xEF)
                    {
                        if (buffer[0] == 0xEF && buffer[1] == 0xBB)
                        {
                            return Encoding.UTF8;
                        }
                        else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                        {
                            return Encoding.BigEndianUnicode;
                        }
                        else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                        {
                            return Encoding.Unicode;
                        }
                        else
                        {
                            return Encoding.Default;
                        }
                    }
                    else
                    {
                        return Encoding.Default;
                    }
                }
            }
            catch
            {
                return Encoding.Default;
            }
        }

        /// <summary>
        /// 当成功获取到云ID的时候触发
        /// </summary>
        public virtual void OnCloudOK()
        { }
        /// <summary>
        /// 当云端通讯出现错误时触发
        /// </summary>
        public virtual void OnCloudFail()
        { }
        /// <summary>
        /// 当云端正在获取ID的时候触发
        /// </summary>
        public virtual void OnCloudPending()
        { }
        /// <summary>
        /// 是否自定义传输云端小数据
        /// </summary>
        /// <returns></returns>
        public virtual bool CustomCloudLiteData()
        {
            return false;
        }
        /// <summary>
        /// 是否自定义传输云端大数据
        /// </summary>
        /// <returns></returns>
        public virtual bool CustomCloudBigData()
        {
            return false;
        }
        /// <summary>
        /// 云端自动获取的小数据
        /// </summary>
        /// <returns></returns>
        public virtual string ForCloudLiteData()
        {
            return MT.CurrentTS.Ticks.ToString();
        }
        /// <summary>
        /// 云端自动获取的大数据
        /// </summary>
        /// <returns></returns>
        public virtual string ForCloudBigData()
        {
            return GetTimerJson();
        }
        /// <summary>
        /// 获取计时器当前的所有状态json
        /// </summary>
        /// <returns></returns>
        public string GetTimerJson()
        {
            HObj exdata = new HObj();
            exdata["Current"] = MT.ToString();
            exdata["Step"] = CurrentStep;
            exdata["OSTime"] = DateTime.Now.Ticks.ToString();
            if (CheckPoints != null)
            {
                HObj cps = new HObj();
                foreach (CheckPoint c in CheckPoints)
                {
                    HObj cur = new HObj();
                    cur["name"] = c.Name;
                    cur["des"] = c.NickName.Replace("\"","").Replace("'","");
                    cur["time"] = TItem.TimeSpanToString(c.Current);
                    cps.Add(cur);
                }
                exdata["CheckPoints"] = cps;
            }
            FillMoreTimerData(exdata);
            return exdata.ToJson();
        }
        /// <summary>
        /// 在获取计时器当前所有状态json的时候，每个内核可以再自定义的填充一些数据
        /// </summary>
        /// <param name="data"></param>
        protected virtual void FillMoreTimerData(HObj exdata)
        { }
        /// <summary>
        /// GetTimerJson的兼容性替代
        /// </summary>
        /// <returns></returns>
        public string GetRStr()
        {
            return GetTimerJson();
        }

        protected Dictionary<TimerPlugin.EPluginPosition, TimerPlugin> Plugins = new Dictionary<TimerPlugin.EPluginPosition, TimerPlugin>();
        /// <summary>
        /// 加载此内核能用的所有插件
        /// </summary>
        public void LoadPlugins()
        {
            string pluginPath = TimerPluginPackageInfo.GetPluginDir();
            if (!Directory.Exists(pluginPath)) return;
            DirectoryInfo root = new DirectoryInfo(pluginPath);
            FileInfo[] files = root.GetFiles();
            foreach (var f in files)
            {
                string sn = f.FullName.Replace(pluginPath, "");
                if (sn.StartsWith(CoreName + ".") && sn.EndsWith(".tpg"))
                {
                    string tpsfile = f.FullName;
                    if (File.Exists(tpsfile))
                    {
                        try
                        {
                            _loadOnePlugin(tpsfile);
                        }
                        catch
                        { }
                    }
                }
            }
        }
        private void _loadOnePlugin(string tpgPath)
        {
            TimerPluginPackageInfo ti = new TimerPluginPackageInfo(tpgPath);
            if (!ti.Enable || !ti.IsOK || ti.Version!=TimerPlugin.Version.ToString()) return;
            //string dllpath = tpgPath + ".dll";
            //ti.SaveDll(dllpath);
            //System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFrom(dllpath);
            System.Reflection.Assembly asm = System.Reflection.Assembly.Load(ti.Data);
            TimerPlugin p = (TimerPlugin)System.Activator.CreateInstance(asm.GetType(ti.ClassName + ".Main"));
            TimerPlugin.EPluginPosition pos = p.GetPosition();
            if (!Plugins.ContainsKey(pos))
            {
                p.OnLoad();
                Plugins.Add(pos, p);
            }
        }
        /// <summary>
        /// 卸载此内核能用的所有插件
        /// </summary>
        public void UnloadPlugins()
        {
            foreach (var kv in Plugins)
            {
                try
                {
                    kv.Value.OnUnload();
                }
                catch { }
            }
            Plugins?.Clear();
            GC.Collect();
        }
        /// <summary>
        /// 刷新所有插件的数据
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="PID"></param>
        /// <param name="BaseAddr32"></param>
        /// <param name="BaseAddr64"></param>
        protected void FlushPlugins(IntPtr handle, int PID, int BaseAddr32, long BaseAddr64)
        {
            foreach (var kv in Plugins)
            {
                try
                {
                    kv.Value.Flush(handle, PID, BaseAddr32, BaseAddr64);
                }
                catch
                { }
            }
        }
        /// <summary>
        /// 向所有插件发送数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public void SendPluginsEvent(string name,object data)
        {
            foreach (var kv in Plugins)
            {
                try
                {
                    kv.Value.OnEvent(name, data);
                }
                catch
                { }
            }
        }
        /// <summary>
        /// 判断是否有此位置的插件
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool HasPlugin(TimerPlugin.EPluginPosition pos)
        {
            if (Plugins.ContainsKey(pos) && Plugins[pos] != null)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取此位置插件的值
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public string GetPluginResult(TimerPlugin.EPluginPosition pos)
        {
            if (HasPlugin(pos))
            {
                return Plugins[pos].GetResult();
            }
            return null;
        }

        public int GetX86ModuleBaseAddr(int pid, string moduleName = "")
        {
            int ret = 0;
            string delpath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\ModuleAddrX86Delegate.exe";

            using (Process dp = new Process())
            {
                dp.StartInfo.FileName = delpath;
                if (moduleName == "")
                {
                    dp.StartInfo.Arguments = pid.ToString();
                }
                else
                {
                    dp.StartInfo.Arguments = pid.ToString() + " " + moduleName;
                }
                dp.StartInfo.UseShellExecute = false;
                dp.StartInfo.RedirectStandardOutput = true;
                dp.StartInfo.CreateNoWindow = true;
                dp.Start();
                dp.WaitForExit();
                if (dp.ExitCode == 0)
                {
                    ret = int.Parse(dp.StandardOutput.ReadLine());
                }
                dp.Close();
            }
            return ret;
        }
        public long GetX64ModuleBaseAddr(int pid, string moduleName = "")
        {
            long ret = 0;
            string delpath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\ModuleAddrX64Delegate.exe";

            using (Process dp = new Process())
            {
                dp.StartInfo.FileName = delpath;
                if (moduleName == "")
                {
                    dp.StartInfo.Arguments = pid.ToString();
                }
                else
                {
                    dp.StartInfo.Arguments = pid.ToString() + " " + moduleName;
                }
                dp.StartInfo.UseShellExecute = false;
                dp.StartInfo.RedirectStandardOutput = true;
                dp.StartInfo.CreateNoWindow = true;
                dp.Start();
                dp.WaitForExit();
                if (dp.ExitCode == 0)
                {
                    ret = long.Parse(dp.StandardOutput.ReadLine());
                }
                dp.Close();
            }
            return ret;
        }
    }

    /// <summary>
    /// 秒表
    /// </summary>
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
    /// <summary>
    /// 时间节点
    /// </summary>
    [Serializable]
    public class CheckPoint
    {
        public string Name;
        public string NickName = "";
        public TimeSpan Best;
        private TimeSpan _current;
        public TimeSpan Current
        {
            get
            {
                return _current;
            }
            set
            {
                _current = value;
                if (_uiItem != null)
                {
                    _uiItem.Cur = value;
                }
            }
        }
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

        private GRender.GItem _uiItem = null;
        public void SetUIItem(GRender.GItem i)
        {
            _uiItem = i;
        }
        public void SetCurrentTSForLoad(TimeSpan ts)
        {
            Current = ts;
            if (_uiItem != null) _uiItem.Cur = ts;
        }
    }
    /// <summary>
    /// 时间节点（仅数据）
    /// </summary>
    [Serializable]
    public class CheckPointNewer
    {
        public string Name;
        public string NickName = "";
        public TimeSpan BestTS;
    }

    public class TItem
    {
        public static string TimeSpanToString(TimeSpan ts)
        {
            return ts.Hours.ToString().PadLeft(2, '0') + ":" + ts.Minutes.ToString().PadLeft(2, '0') + ":" + ts.Seconds.ToString().PadLeft(2, '0') + "." + (ts.Milliseconds / 10).ToString().PadLeft(2, '0');
        }

        public static string TimeSpanToStringLite(TimeSpan ts)
        {
            return ts.Hours.ToString().PadLeft(2, '0') + ":" + ts.Minutes.ToString().PadLeft(2, '0') + ":" + ts.Seconds.ToString().PadLeft(2, '0');
        }
    }

    public class TimerPluginPackageInfo
    {
        public static string GetPluginDir()
        {
            return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\plugins\\";
        }
        public string FileName;
        public bool Enable
        {
            get { return _enable; }
            set
            {
                _enable = value;
                using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite))
                {
                    byte[] e = new byte[fs.Length];
                    fs.Read(e, 0, e.Length);
                    if (_enable)
                    {
                        e[0] = 100;
                    }
                    else
                    {
                        e[0] = 200;
                    }
                    fs.Seek(0, SeekOrigin.Begin);
                    fs.Write(e, 0, e.Length);
                    fs.Flush();
                }
            }
        }
        private bool _enable;
        public string ClassName;
        public string Version;
        public string Des;
        public string Core;
        private string Sign;
        private string DllMD5;
        public byte[] Data;
        public bool IsOK
        {
            get { return _isok; }
        }
        private bool _isok = false;
        public void SaveDll(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                fs.Write(Data, 0, Data.Length);
                fs.Flush();
            }
        }
        public TimerPluginPackageInfo(string tpgPath)
        {
            if (!File.Exists(tpgPath)) return;
            FileName = tpgPath;
            List<long> zeroPos = new List<long>();
            using (FileStream fs = new FileStream(tpgPath, FileMode.Open, FileAccess.Read))
            {
                long len = fs.Length;
                for (long i = 0L; i < len; ++i)
                {
                    if (fs.ReadByte() == 0)
                    {
                        zeroPos.Add(i);
                        if (zeroPos.Count == 4) break;
                    }
                }

                byte[] bEnable = new byte[1];
                byte[] bClass = new byte[zeroPos[0] - 1];
                byte[] bVersion = new byte[zeroPos[1] - zeroPos[0] - 1];
                byte[] bDes = new byte[zeroPos[2] - zeroPos[1] - 1];
                byte[] bSign = new byte[zeroPos[3] - zeroPos[2] - 1];
                Data = new byte[len - bVersion.LongLength - bDes.LongLength - bSign.LongLength - bClass.LongLength - 4L - 1L];

                fs.Seek(0, SeekOrigin.Begin);
                fs.Read(bEnable, 0, bEnable.Length);
                fs.Seek(1, SeekOrigin.Begin);
                fs.Read(bClass, 0, bClass.Length);
                fs.Seek((int)zeroPos[0] + 1, SeekOrigin.Begin);
                fs.Read(bVersion, 0, bVersion.Length);
                fs.Seek((int)zeroPos[1] + 1, SeekOrigin.Begin);
                fs.Read(bDes, 0, bDes.Length);
                fs.Seek((int)zeroPos[2] + 1, SeekOrigin.Begin);
                fs.Read(bSign, 0, bSign.Length);
                fs.Seek((int)zeroPos[3] + 1, SeekOrigin.Begin);
                fs.Read(Data, 0, Data.Length);

                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(Data);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                DllMD5 = sb.ToString().ToUpper();

                byte enable = bEnable[0];
                _enable = (enable != 200);

                Version = Encoding.UTF8.GetString(bVersion);
                Des = Encoding.UTF8.GetString(bDes);
                Sign = Encoding.UTF8.GetString(bSign);
                ClassName = Encoding.UTF8.GetString(bClass);
                Core = ClassName.Substring(0, ClassName.IndexOf('.'));
                
            }
            CheckSign();
        }
        public void CheckSign()
        {
            if (RSAVerify(PublicKey(), Version + " " + DllMD5, Sign))
            {
                _isok = true;
            }
            else
            {
                _isok = false;
            }
        }
        public static string PublicKey()
        {
            return @"<RSAKeyValue><Modulus>1svEvynNYZr/YlUZB9a7txNfEFNPN9jDj7nPlIEqpP3SoHaLI8cCYbyjTMuqvthcOWURqgxlKbIqk9YpX0mzQv308JconeRjGJHB06WoYZ2BCJieQ2AUn5hzWHtBcbRdnLMRyZpRZMIrds9gRU40BQvOXq8roVl4lJCjGE1OXYE=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        }
        public static bool RSAVerify(string key, string content, string sign)
        {
            try
            {
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider(1024);
                rsa.FromXmlString(key);
                return rsa.VerifyData(Encoding.UTF8.GetBytes(content), new System.Security.Cryptography.SHA1CryptoServiceProvider(), Convert.FromBase64String(sign));
            }
            catch { }
            return false;
        }
    }
}
