using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CLROBS;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing;
using System.Drawing.Imaging;
using Color = System.Drawing.Color;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Script.Serialization;

namespace Pal98TimerOBSPlugin
{
    class Pal98TimerSource : AbstractImageSource
    {
        private Object textureLock = new Object();
        private Texture texture = null;
        private XElement config;
        private TimerData td = new TimerData();
        private int Col1Off = 220;
        private int Col2Off = 140;
        //private int Col3Off = 100;
        private int Col4Off = 70;
        private int RowHeightOff = 18;
        private int PointFontSize = 12;

        private string host = "";
        private int port = 39263;
        private SocketClient client;

        private int t = 0;

        public Pal98TimerSource(XElement configData)
        {
            this.config = configData;
            InitSocket();
        }

        public static string ReadFile(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        private void InitSocket()
        {
            if (client == null)
            {
                try
                {
                    host = ReadFile("sip.txt").Trim();
                }
                catch
                { }
                if (host == "")
                {
                    host = "127.0.0.1";
                }
                client = new SocketClient(host, port);
                //绑定当收到服务器发送的消息后的处理事件
                client.HandleRecMsg = new Action<byte[], SocketClient>((bytes, theClient) =>
                {
                    lock (textureLock)
                    {
                        try
                        {
                            string msg = Encoding.UTF8.GetString(bytes);
                            JavaScriptSerializer s = new JavaScriptSerializer();
                            td = s.Deserialize<TimerData>(msg);

                            //td = BytesToObject<TimerData>(bytes);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                });

                //绑定向服务器发送消息后的处理事件
                client.HandleSendMsg = new Action<byte[], SocketClient>((bytes, theClient) =>
                {
                    /*string msg = Encoding.UTF8.GetString(bytes);
                    Console.WriteLine($"向服务器发送消息:{msg}");*/
                });

                client.HandleClientClose = new Action<SocketClient>(sc => {
                    client = null;
                });

                //开始运行客户端
                client.StartClient();
            }
        }

        private void LoadTexture()
        {
            t++;
            if (t >= 1000)
            {
                t = 0;
            }
            InitSocket();

            if (t % 10 == 0)
            {
                DrawTimer();
            }
        }

        private void DrawTimer()
        {
            lock (textureLock)
            {
                /*if (client != null)
                {
                    try
                    {
                        client.Send("r:" + td.Luck);
                    }
                    catch (Exception ex)
                    {
                        client.Send("e:" + ex.Message);
                    }
                }*/
                if (texture != null)
                {
                    texture.Dispose();
                    texture = null;
                }

                int width = config.GetInt("width", 960);
                int height = config.GetInt("height", 600);

                /*td.GameVersion = "111";
                td.Version = "222";
                td.Luck = "asdasd";
                td.BattleLong = "5.23";
                td.MT = "00:00:00.36";
                td.ST = "00:00:00.36";
                td.ColorEgg = "惹不起";*/

                using (Bitmap bp = new Bitmap(width, height))
                {
                    using (Graphics g = Graphics.FromImage(bp))
                    {
                        try
                        {
                            g.DrawString(td.GameVersion, new Font("黑体", 10), new SolidBrush(Color.Green), 10, 10);
                            g.DrawString(td.Version + "  " + td.CloudID, new Font("黑体", 10), new SolidBrush(Color.White), 200, 10);
                            g.DrawString("蜂:" + td.FC + " " + "蜜:" + td.FM + " " + "火:" + td.HCG + " " + "血:" + td.XLL + " " + "夜:" + td.YXY + " " + "剑:" + td.LQJ, new Font("黑体", 12, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 128, 0)), 10, 30);
                            g.DrawString(td.Luck, new Font("黑体", 12, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 0, 0)), 10, 80);
                            g.DrawString("战斗时长：" + td.BattleLong, new Font("黑体", 12, FontStyle.Bold), new SolidBrush(Color.FromArgb(0, 255, 255)), 10, 50);
                            g.DrawString(td.MT, new Font("黑体", 18, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 255, 128)), 10, height - 50);
                            g.DrawString(td.ST, new Font("黑体", 10, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 255, 128)), 10, height - 65);
                            g.DrawString(td.ColorEgg, new Font("黑体", 12), new SolidBrush(Color.DeepPink), 10, height - 20);


                            //时间线
                            int RowYPos = height - RowHeightOff;
                            for (int i = 0; i < td.cpls.Count; ++i)
                            {
                                RowYPos -= RowHeightOff;
                                CheckPointLite cp = td.cpls[i];

                                if (cp.IsCurrent)
                                {
                                    g.DrawString(">", new Font("黑体", PointFontSize, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 255, 128)), width - Col1Off - 20, RowYPos);
                                }
                                g.DrawString(cp.Name, new Font("黑体", PointFontSize), new SolidBrush(Color.FromArgb(255, 255, 128)), width - Col1Off, RowYPos);
                                g.DrawString(cp.Best, new Font("黑体", PointFontSize - 0), new SolidBrush(Color.FromArgb(255, 255, 180)), width - Col2Off, RowYPos);
                                if (cp.CHA.IndexOf("-") >= 0)
                                {
                                    g.DrawString(cp.CHA, new Font("黑体", PointFontSize, FontStyle.Bold), new SolidBrush(Color.FromArgb(0, 255, 0)), width - Col4Off, RowYPos);
                                }
                                else if (cp.CHA.IndexOf("+") >= 0)
                                {
                                    g.DrawString(cp.CHA, new Font("黑体", PointFontSize, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 0, 0)), width - Col4Off, RowYPos);
                                }
                            }
                            g.DrawString("预计通关  " + td.WillClear, new Font("黑体", PointFontSize), new SolidBrush(Color.Orange), width - Col1Off + 20, height - RowHeightOff);
                        }
                        catch { }
                    }

                    WriteableBitmap wb = new WriteableBitmap(GetBitmapSource(bp));

                    texture = GS.CreateTexture((UInt32)wb.PixelWidth, (UInt32)wb.PixelHeight, GSColorFormat.GS_BGRA, null, false, false);
                    texture.SetImage(wb.BackBuffer, GSImageFormat.GS_IMAGEFORMAT_BGRA, (UInt32)(wb.PixelWidth * 4));

                    config.Parent.SetInt("cx", wb.PixelWidth);
                    config.Parent.SetInt("cy", wb.PixelHeight);

                    Size.X = (float)wb.PixelWidth;
                    Size.Y = (float)wb.PixelHeight;

                }

            }
        }

        /// <summary> 
        /// 将一个序列化后的byte[]数组还原         
        /// </summary>
        /// <param name="Bytes"></param>         
        /// <returns></returns> 
        public static T BytesToObject<T>(byte[] Bytes)
        {
            using (MemoryStream ms = new MemoryStream(Bytes))
            {
                IFormatter formatter = new BinaryFormatter();

                return (T)formatter.Deserialize(ms);
            }
        }

        public static BitmapSource GetBitmapSource(Bitmap bmp)
        {
            BitmapFrame bf = null;

            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                bf = BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);

            }
            return bf;
        }

        override public void Render(float x, float y, float width, float height)
        {
            lock (textureLock)
            {
                if (texture != null)
                {
                    GS.DrawSprite(texture, 0xFFFFFFFF, x, y, x + width, y + height);
                }
            }
            UpdateSettings();
        }

        public void Dispose()
        {
            lock (textureLock)
            {
                if (texture != null)
                {
                    texture.Dispose();
                    texture = null;
                }
            }
        }
        override public void UpdateSettings()
        {
            //XElement dataElement = config.GetElement("data");
            LoadTexture();
        }

        private string GetChaStr(long cha)
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


        public static string TimeSpanToString(TimeSpan ts)
        {
            return ts.Hours.ToString().PadLeft(2, '0') + ":" + ts.Minutes.ToString().PadLeft(2, '0') + ":" + ts.Seconds.ToString().PadLeft(2, '0') + "." + (ts.Milliseconds / 10).ToString().PadLeft(2, '0');
        }

        public static string TimeSpanToStringLite(TimeSpan ts)
        {
            return ts.Hours.ToString().PadLeft(2, '0') + ":" + ts.Minutes.ToString().PadLeft(2, '0') + ":" + ts.Seconds.ToString().PadLeft(2, '0');
        }
    }

    [Serializable]
    public class TimerData
    {
        public string MT = "";
        public string ST = "";
        public string MoreInfo = "";
        public string GameVersion = "";
        public string Version = "";
        public string BattleLong = "";
        public string FC = "";
        public string FM = "";
        public string HCG = "";
        public string XLL = "";
        public string YXY = "";
        public string LQJ = "";
        public string CloudID = "云端未认证";
        public int CurrentStep = -1;
        public List<CheckPointLite> cpls = new List<CheckPointLite>();
        public string Luck = "";
        public string ColorEgg = "";
        public string WillClear = "";
    }
    [Serializable]
    public class CheckPointLite
    {
        public bool IsCurrent = false;
        public string Name = "";
        public string Best = "";
        public string CHA = "";
        public CheckPointLite()
        {
        }
    }

    /// <summary>
    /// Socket客户端
    /// </summary>
    public class SocketClient
    {
        #region 构造函数

        /// <summary>
        /// 构造函数,连接服务器IP地址默认为本机127.0.0.1
        /// </summary>
        /// <param name="port">监听的端口</param>
        public SocketClient(int port)
        {
            _ip = "127.0.0.1";
            _port = port;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">监听的IP地址</param>
        /// <param name="port">监听的端口</param>
        public SocketClient(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        #endregion

        #region 内部成员

        private Socket _socket = null;
        private string _ip = "";
        private int _port = 0;
        private bool _isRec = true;
        private bool IsSocketConnected()
        {
            bool part1 = _socket.Poll(1000, SelectMode.SelectRead);
            bool part2 = (_socket.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 开始接受客户端消息
        /// </summary>
        public void StartRecMsg()
        {
            try
            {
                byte[] container = new byte[1024 * 1024 * 2];
                _socket.BeginReceive(container, 0, container.Length, SocketFlags.None, asyncResult =>
                {
                    try
                    {
                        int length = _socket.EndReceive(asyncResult);

                        //马上进行下一轮接受，增加吞吐量
                        if (length > 0 && _isRec && IsSocketConnected())
                            StartRecMsg();

                        if (length > 0)
                        {
                            byte[] recBytes = new byte[length];
                            Array.Copy(container, 0, recBytes, 0, length);

                            //处理消息
                            HandleRecMsg?.Invoke(recBytes, this);
                        }
                        else
                            Close();
                    }
                    catch (Exception ex)
                    {
                        HandleException?.Invoke(ex);
                        Close();
                    }
                }, null);
            }
            catch (Exception ex)
            {
                HandleException?.Invoke(ex);
                Close();
            }
        }

        #endregion

        #region 外部接口

        /// <summary>
        /// 开始服务，连接服务端
        /// </summary>
        public void StartClient()
        {
            try
            {
                //实例化 套接字 （ip4寻址协议，流式传输，TCP协议）
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //创建 ip对象
                IPAddress address = IPAddress.Parse(_ip);
                //创建网络节点对象 包含 ip和port
                IPEndPoint endpoint = new IPEndPoint(address, _port);
                //将 监听套接字  绑定到 对应的IP和端口
                _socket.BeginConnect(endpoint, asyncResult =>
                {
                    try
                    {
                        _socket.EndConnect(asyncResult);
                        //开始接受服务器消息
                        StartRecMsg();

                        HandleClientStarted?.Invoke(this);
                    }
                    catch (Exception ex)
                    {
                        HandleException?.Invoke(ex);
                    }
                }, null);
            }
            catch (Exception ex)
            {
                HandleException?.Invoke(ex);
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="bytes">数据字节</param>
        public void Send(byte[] bytes)
        {
            try
            {
                _socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, asyncResult =>
                {
                    try
                    {
                        int length = _socket.EndSend(asyncResult);
                        HandleSendMsg?.Invoke(bytes, this);
                    }
                    catch (Exception ex)
                    {
                        HandleException?.Invoke(ex);
                    }
                }, null);
            }
            catch (Exception ex)
            {
                HandleException?.Invoke(ex);
            }
        }

        /// <summary>
        /// 发送字符串（默认使用UTF-8编码）
        /// </summary>
        /// <param name="msgStr">字符串</param>
        public void Send(string msgStr)
        {
            Send(Encoding.UTF8.GetBytes(msgStr));
        }

        /// <summary>
        /// 发送字符串（使用自定义编码）
        /// </summary>
        /// <param name="msgStr">字符串消息</param>
        /// <param name="encoding">使用的编码</param>
        public void Send(string msgStr, Encoding encoding)
        {
            Send(encoding.GetBytes(msgStr));
        }

        /// <summary>
        /// 传入自定义属性
        /// </summary>
        public object Property { get; set; }

        /// <summary>
        /// 关闭与服务器的连接
        /// </summary>
        public void Close()
        {
            try
            {
                _isRec = false;
                _socket.Disconnect(false);
                HandleClientClose?.Invoke(this);
            }
            catch (Exception ex)
            {
                HandleException?.Invoke(ex);
            }
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 客户端连接建立后回调
        /// </summary>
        public Action<SocketClient> HandleClientStarted { get; set; }

        /// <summary>
        /// 处理接受消息的委托
        /// </summary>
        public Action<byte[], SocketClient> HandleRecMsg { get; set; }

        /// <summary>
        /// 客户端连接发送消息后回调
        /// </summary>
        public Action<byte[], SocketClient> HandleSendMsg { get; set; }

        /// <summary>
        /// 客户端连接关闭后回调
        /// </summary>
        public Action<SocketClient> HandleClientClose { get; set; }

        /// <summary>
        /// 异常处理程序
        /// </summary>
        public Action<Exception> HandleException { get; set; }

        #endregion
    }
}
