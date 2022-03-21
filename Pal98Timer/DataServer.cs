using HFrame.EX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;

namespace Pal98Timer
{
    public class DataServer
    {
        private TimerData td = new TimerData();
        private int port = 39263;
        private string host = "0.0.0.0";
        private int t = 0;
        private bool IsGoing = true;
        private SocketServer server;

        public DataServer()
        {
            IsGoing = true;
            server = new SocketServer(host, port);
            //处理从客户端收到的消息
            server.HandleRecMsg = delegate (byte[] bytes, SocketConnection client, SocketServer theServer)
            {
                string msg = Encoding.UTF8.GetString(bytes);
                //Console.WriteLine($"收到消息:{msg}");
            };

            //处理服务器启动后事件
            server.HandleServerStarted = delegate (SocketServer theServer)
            {
                //Console.WriteLine("服务已启动************");
            };

            //处理新的客户端连接后的事件
            server.HandleNewClientConnected = delegate (SocketServer theServer, SocketConnection theCon)
            {
                //Console.WriteLine($@"一个新的客户端接入，当前连接数：{theServer.ClientList.Count}");
            };

            //处理客户端连接关闭后的事件
            server.HandleClientClose = delegate (SocketConnection theCon, SocketServer theServer)
            {
                //Console.WriteLine($@"一个客户端关闭，当前连接数为：{theServer.ClientList.Count}");
            };

            //处理异常
            server.HandleException = delegate (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            };

            //服务器启动
            server.StartServer();

            FormEx.Run(delegate () {
                while (IsGoing)
                {
                    t++;
                    if (t >= 1000)
                    {
                        t = 0;
                    }
                    try
                    {
                        td.PreData();

                        if (t % 10 == 0)
                        {
                            DrawPng();
                        }

                        if (server.ClientList.Count > 0)
                        {
                            foreach (SocketConnection sc in server.ClientList)
                            {
                                try
                                {
                                    JavaScriptSerializer s = new JavaScriptSerializer();
                                    sc.Send(s.Serialize(td).Replace("凰","皇"), Encoding.UTF8);
                                    //sc.Send(ObjectToBytes(td));
                                }
                                catch (Exception ex)
                                {
                                    //Console.WriteLine(ex.Message);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //Console.WriteLine(e.Message);
                    }
                    Thread.Sleep(50);
                }
            });
        }
        public void X()
        {
            IsGoing = false;
            if (server != null)
            {
                server.StopServer();
            }
        }
        public void DrawPng()
        {
            PngDraw.Draw(td).Save(@"it.png",ImageFormat.Png);
            if (File.Exists(@"i.png"))
            {
                File.Delete(@"i.png");
            }
            File.Move(@"it.png", @"i.png");
        }
        /// <summary> 
        /// 将一个object对象序列化，返回一个byte[]         
        /// </summary> 
        /// <param name="obj">能序列化的对象</param>         
        /// <returns></returns> 
        public static byte[] ObjectToBytes(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                return ms.GetBuffer();
            }
        }
        public void SetData(TimerData td)
        {
            this.td = td;
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
        public void PreData()
        {
            this.BattleLong = SI.ins.BattleLong;
            this.CloudID = SI.ins.CloudID;
            this.ColorEgg = SI.ins.ColorEgg;
            this.CurrentStep = SI.ins.CurrentStep;
            this.FC = SI.ins.FC;
            this.FM = SI.ins.FM;
            this.GameVersion = SI.ins.GameVersion;
            this.HCG = SI.ins.HCG;
            this.LQJ = SI.ins.LQJ;
            this.Luck = SI.ins.Luck;
            this.MoreInfo = SI.ins.MoreInfo;
            this.MT = SI.ins.MT;
            this.ST = SI.ins.ST;
            this.Version = SI.ins.Version;
            this.XLL = SI.ins.XLL;
            this.YXY = SI.ins.YXY;
            this.cpls = new List<CheckPointLite>();

            int cstep = this.CurrentStep;
            if (cstep < 0) cstep = 0;
            int start = cstep - SI.ins.ShowPointCount / 2;
            int end = cstep + SI.ins.ShowPointCount / 2;
            while (start < 0)
            {
                start++;
                end++;
            }
            while (end >= SI.ins.cps.Count)
            {
                start--;
                end--;
            }
            long bestcha = long.MinValue;
            for (int i = end; i >= start; --i)
            {
                CheckPoint cp = SI.ins.cps[i];

                long chas = 0;
                if (cp.Current < cp.Best)
                {
                    chas = ((long)((cp.Best - cp.Current).TotalSeconds)) * -1;
                }
                else
                {
                    chas = ((long)((cp.Current - cp.Best).TotalSeconds));
                }

                if (i == cstep - 1)
                {
                    bestcha = chas;
                }

                if (cstep == i)
                {
                    this.cpls.Add(new CheckPointLite()
                    {
                        IsCurrent = true,
                        Name = cp.Name,
                        Best = TItem.TimeSpanToStringLite(cp.Best),
                        CHA = LiveWindow.GetChaStr(chas)
                    });
                }
                else if (i < cstep)
                {
                    this.cpls.Add(new CheckPointLite()
                    {
                        IsCurrent = false,
                        Name = cp.Name,
                        Best = TItem.TimeSpanToStringLite(cp.Best),
                        CHA = LiveWindow.GetChaStr(chas)
                    });
                }
                else
                {

                    this.cpls.Add(new CheckPointLite()
                    {
                        IsCurrent = false,
                        Name = cp.Name,
                        Best = TItem.TimeSpanToStringLite(cp.Best),
                        CHA = ""
                    });
                }
            }
            CheckPoint lastcp = SI.ins.cps[SI.ins.cps.Count - 1];
            if (bestcha != long.MinValue && lastcp.Best.TotalSeconds > 0)
            {
                this.WillClear = TItem.TimeSpanToStringLite(lastcp.Best.Add(new TimeSpan(bestcha * 1000 * 10000)));
            }
            else
            {
                this.WillClear = TItem.TimeSpanToStringLite(lastcp.Best);
            }
        }
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
    /// <summary>
    /// Socket服务端
    /// </summary>
    public class SocketServer
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">监听的IP地址</param>
        /// <param name="port">监听的端口</param>
        public SocketServer(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        /// <summary>
        /// 构造函数,监听IP地址默认为本机0.0.0.0
        /// </summary>
        /// <param name="port">监听的端口</param>
        public SocketServer(int port)
        {
            _ip = "0.0.0.0";
            _port = port;
        }

        #endregion

        #region 内部成员

        private Socket _socket = null;
        private string _ip = "";
        private int _port = 0;
        private bool _isListen = true;
        private void StartListen()
        {
            try
            {
                _socket.BeginAccept(asyncResult =>
                {
                    try
                    {
                        Socket newSocket = _socket.EndAccept(asyncResult);

                        //马上进行下一轮监听,增加吞吐量
                        if (_isListen)
                            StartListen();

                        SocketConnection newClient = new SocketConnection(newSocket, this)
                        {
                            HandleRecMsg = HandleRecMsg,
                            HandleClientClose = HandleClientClose,
                            HandleSendMsg = HandleSendMsg,
                            HandleException = HandleException
                        };

                        newClient.StartRecMsg();
                        ClientList.AddLast(newClient);

                        HandleNewClientConnected?.Invoke(this, newClient);
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

        #endregion

        #region 外部接口

        /// <summary>
        /// 开始服务，监听客户端
        /// </summary>
        public void StartServer()
        {
            try
            {
                //实例化套接字（ip4寻址协议，流式传输，TCP协议）
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //创建ip对象
                IPAddress address = IPAddress.Parse(_ip);
                //创建网络节点对象包含ip和port
                IPEndPoint endpoint = new IPEndPoint(address, _port);
                //将 监听套接字绑定到 对应的IP和端口
                _socket.Bind(endpoint);
                //设置监听队列长度为Int32最大值(同时能够处理连接请求数量)
                _socket.Listen(int.MaxValue);
                //开始监听客户端
                StartListen();
                HandleServerStarted?.Invoke(this);
            }
            catch (Exception ex)
            {
                HandleException?.Invoke(ex);
            }
        }

        public void StopServer()
        {
            if (_socket != null)
            {
                try
                {
                    _socket.Close();
                }
                catch { }
            }
        }

        /// <summary>
        /// 所有连接的客户端列表
        /// </summary>
        public LinkedList<SocketConnection> ClientList { get; set; } = new LinkedList<SocketConnection>();

        /// <summary>
        /// 关闭指定客户端连接
        /// </summary>
        /// <param name="theClient">指定的客户端连接</param>
        public void CloseClient(SocketConnection theClient)
        {
            theClient.Close();
        }

        #endregion

        #region 公共事件

        /// <summary>
        /// 异常处理程序
        /// </summary>
        public dHandleException HandleException { get; set; }
        #endregion

        #region 服务端事件

        /// <summary>
        /// 服务启动后执行
        /// </summary>
        public dHandleServerStarted HandleServerStarted { get; set; }
        /// <summary>
        /// 当新客户端连接后执行
        /// </summary>
        public dHandleNewClientConnected HandleNewClientConnected { get; set; }
        /// <summary>
        /// 服务端关闭客户端后执行
        /// </summary>
        public dHandleCloseClient HandleCloseClient { get; set; }
        #endregion

        #region 客户端连接事件

        /// <summary>
        /// 客户端连接接受新的消息后调用
        /// </summary>
        public dHandleRecMsg HandleRecMsg { get; set; }
        /// <summary>
        /// 客户端连接发送消息后回调
        /// </summary>
        public dHandleSendMsg HandleSendMsg { get; set; }
        /// <summary>
        /// 客户端连接关闭后回调
        /// </summary>
        public dHandleClientClose HandleClientClose { get; set; }
        #endregion
    }
    public delegate void dHandleException(Exception ex);
    public delegate void dHandleServerStarted(SocketServer ss);
    public delegate void dHandleNewClientConnected(SocketServer ss, SocketConnection sc);
    public delegate void dHandleCloseClient(SocketServer ss, SocketConnection sc);
    public delegate void dHandleRecMsg(byte[] b, SocketConnection sc, SocketServer ss);
    public delegate void dHandleSendMsg(byte[] b, SocketConnection sc, SocketServer ss);
    public delegate void dHandleClientClose(SocketConnection sc, SocketServer ss);
    /// <summary>
    /// Socket连接,双向通信
    /// </summary>
    public class SocketConnection
    {
        #region 构造函数

        public SocketConnection(Socket socket, SocketServer server)
        {
            _socket = socket;
            _server = server;
        }

        #endregion

        #region 私有成员

        private readonly Socket _socket;
        private bool _isRec = true;
        private SocketServer _server = null;
        private bool IsSocketConnected()
        {
            bool part1 = _socket.Poll(1000, SelectMode.SelectRead);
            bool part2 = (_socket.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

        #endregion

        #region 外部接口

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
                            HandleRecMsg?.Invoke(recBytes, this, _server);
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
                        HandleSendMsg?.Invoke(bytes, this, _server);
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
        /// 关闭当前连接
        /// </summary>
        public void Close()
        {
            try
            {
                _isRec = false;
                _socket.Disconnect(false);
                _server.ClientList.Remove(this);
                HandleClientClose?.Invoke(this, _server);
                _socket.Close();
                //_socket.Dispose();
                GC.Collect();
            }
            catch (Exception ex)
            {
                HandleException?.Invoke(ex);
            }
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 客户端连接接受新的消息后调用
        /// </summary>
        public dHandleRecMsg HandleRecMsg { get; set; }

        /// <summary>
        /// 客户端连接发送消息后回调
        /// </summary>
        public dHandleSendMsg HandleSendMsg { get; set; }

        /// <summary>
        /// 客户端连接关闭后回调
        /// </summary>
        public dHandleClientClose HandleClientClose { get; set; }

        /// <summary>
        /// 异常处理程序
        /// </summary>
        public dHandleException HandleException { get; set; }

        #endregion
    }
}
