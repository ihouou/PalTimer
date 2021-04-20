//using DotNetPusher.Pushers;
using HFrame.EX;
using HFrame.OS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Pal98Timer
{
    public partial class LiveWindow : FormEx
    {
        private bool IsGoing=true;
        private IntPtr GameMainWindow = IntPtr.Zero;
        private IntPtr GameCurrentWindow = IntPtr.Zero;
        private Image wimg = null;
        //private Bitmap rimg = null;
        private int Timeout = 50;
        private int width;
        private int height;
        private int Col1Off = 220;
        private int Col2Off = 140;
        //private int Col3Off = 100;
        private int Col4Off = 70;
        private int RowHeightOff = 18;
        private int PointFontSize = 12;
        private bool LockSize = true;

        private bool ShowGamePic = false;

        //private bool IsRTMPRunning = false;
        public string RTMPAddress = "";

        public string TarWindowTitle = "仙剑奇侠传 WIN-95 版 [补丁版本：3.0.2014.628]";

        public LiveWindow()
        {
            InitializeComponent();
            width = pbMain.Width;
            height = pbMain.Height;
            tmMain.Interval = Timeout;

            Start();
        }

        private void Start()
        {
            Run(delegate () {
                while (IsGoing)
                {
                    try
                    {
                        Image gameimg = null;
                        if (GameCurrentWindow == IntPtr.Zero)
                        {
                            GameCurrentWindow = User32.FindWindow(null, TarWindowTitle);
                        }
                        if (GameCurrentWindow != IntPtr.Zero)
                        {
                            try
                            {
                                if (ShowGamePic)
                                {
                                    gameimg = GetWindowCapture(GameCurrentWindow);
                                }
                            }
                            catch
                            {
                                GameCurrentWindow = IntPtr.Zero;
                            }
                            if (LockSize && gameimg != null)
                            {
                                if (gameimg.Width != width || gameimg.Height != height)
                                {
                                    UI(delegate ()
                                    {
                                        this.ClientSize = new Size(gameimg.Width, gameimg.Height);
                                    });
                                }
                            }
                        }

                        Image tmpimg = new Bitmap(width, height);
                        using (Graphics g = Graphics.FromImage(tmpimg))
                        {
                            if (ShowGamePic)
                            {
                                if (gameimg != null)
                                {
                                    g.DrawImage(gameimg, 0, 0);
                                }
                            }
                            g.DrawString(SI.ins.GameVersion, new Font("黑体", 10), new SolidBrush(Color.Green), 10, 10);
                            g.DrawString(SI.ins.Version + "  " + SI.ins.CloudID, new Font("黑体", 10), new SolidBrush(Color.White), 200, 10);
                            g.DrawString("蜂:" + SI.ins.FC + " " + "蜜:" + SI.ins.FM + " " + "火:" + SI.ins.HCG + " " + "血:" + SI.ins.XLL + " " + "夜:" + SI.ins.YXY + " " + "剑:" + SI.ins.LQJ, new Font("黑体", 12, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 128, 0)), 10, 30);
                            g.DrawString(SI.ins.Luck, new Font("黑体", 12, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 0, 0)), 10, 80);
                            g.DrawString("战斗时长：" + SI.ins.BattleLong, new Font("黑体", 12, FontStyle.Bold), new SolidBrush(Color.FromArgb(0, 255, 255)), 10, 50);
                            g.DrawString(SI.ins.MT, new Font("黑体", 18, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 255, 128)), 10, height - 50);
                            g.DrawString(SI.ins.ST, new Font("黑体", 10, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 255, 128)), 10, height - 65);
                            g.DrawString(SI.ins.ColorEgg, new Font("黑体", 12), new SolidBrush(Color.DeepPink), 10, height - 20);

                            if (SI.ins.ShowPointCount > SI.ins.cps.Count)
                            {
                                SI.ins.ShowPointCount = SI.ins.cps.Count;
                            }


                            //时间线
                            int cstep = SI.ins.CurrentStep;
                            if (cstep < 0) cstep = 0;
                            int RowYPos = height - RowHeightOff;
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

                            if (SI.ins.cps != null && SI.ins.cps.Count > 0)
                            {
                                for (int i = end; i >= start; --i)
                                {
                                    RowYPos -= RowHeightOff;
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
                                        g.DrawString(cp.Name, new Font("黑体", PointFontSize), new SolidBrush(Color.FromArgb(255, 255, 128)), width - Col1Off, RowYPos);
                                        g.DrawString(TItem.TimeSpanToStringLite(cp.Best), new Font("黑体", PointFontSize - 0), new SolidBrush(Color.FromArgb(255, 255, 180)), width - Col2Off, RowYPos);
                                        g.DrawString(">", new Font("黑体", PointFontSize, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 255, 128)), width - Col1Off - 20, RowYPos);

                                        if (chas < 0)
                                        {
                                            g.DrawString(GetChaStr(chas), new Font("黑体", PointFontSize, FontStyle.Bold), new SolidBrush(Color.FromArgb(0, 255, 0)), width - Col4Off, RowYPos);
                                        }
                                        else
                                        {
                                            g.DrawString(GetChaStr(chas), new Font("黑体", PointFontSize, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 0, 0)), width - Col4Off, RowYPos);
                                        }
                                    }
                                    else if (i < cstep)
                                    {
                                        g.DrawString(cp.Name, new Font("黑体", PointFontSize), new SolidBrush(Color.FromArgb(255, 255, 128)), width - Col1Off, RowYPos);
                                        g.DrawString(TItem.TimeSpanToStringLite(cp.Best), new Font("黑体", PointFontSize - 0), new SolidBrush(Color.FromArgb(255, 255, 180)), width - Col2Off, RowYPos);
                                        //TimeSpan cha = TimeSpan.MinValue;
                                        if (chas < 0)
                                        {
                                            g.DrawString(GetChaStr(chas), new Font("黑体", PointFontSize, FontStyle.Bold), new SolidBrush(Color.FromArgb(0, 255, 0)), width - Col4Off, RowYPos);
                                        }
                                        else
                                        {
                                            g.DrawString(GetChaStr(chas), new Font("黑体", PointFontSize, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 0, 0)), width - Col4Off, RowYPos);
                                        }
                                    }
                                    else
                                    {
                                        g.DrawString(cp.Name, new Font("黑体", PointFontSize), new SolidBrush(Color.FromArgb(255, 255, 128)), width - Col1Off, RowYPos);
                                        g.DrawString(TItem.TimeSpanToStringLite(cp.Best), new Font("黑体", PointFontSize - 0), new SolidBrush(Color.FromArgb(255, 255, 180)), width - Col2Off, RowYPos);
                                    }
                                }

                                CheckPoint lastcp = SI.ins.cps[SI.ins.cps.Count - 1];
                                if (bestcha != long.MinValue && lastcp.Best.TotalSeconds > 0)
                                {
                                    g.DrawString("预计通关  " + TItem.TimeSpanToStringLite(lastcp.Best.Add(new TimeSpan(bestcha * 1000 * 10000))), new Font("黑体", PointFontSize), new SolidBrush(Color.Orange), width - Col1Off + 20, height - RowHeightOff);
                                }
                                else
                                {
                                    g.DrawString("预计通关  " + TItem.TimeSpanToStringLite(lastcp.Best), new Font("黑体", PointFontSize), new SolidBrush(Color.Orange), width - Col1Off + 20, height - RowHeightOff);
                                }
                            }
                        }
                        //Bitmap trimg = new Bitmap(tmpimg);
                        wimg = tmpimg;
                        //rimg = trimg;
                    }
                    catch
                    { }
                    Thread.Sleep(Timeout);
                }
            });
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

        public Image GetWindowCapture(IntPtr hWnd)
        {
            /*Bitmap bmp = null;
            IntPtr hscrdc = Win32API.GetWindowDC(hWnd);
            IntPtr hmemdc = Win32API.CreateCompatibleDC(hscrdc);
            try
            {
                RECT rect = new RECT();
                Win32API.GetClientRect(hWnd, ref rect);
                int width = rect.right - rect.left;
                int height = rect.bottom - rect.top;
                //MessageBox.Show(width.ToString() + "/" + height.ToString());
                //this.pictureBox1.Width = width; this.pictureBox1.Height = height;
                IntPtr hbitmap = Win32API.CreateCompatibleBitmap(hscrdc, width, height);
                Win32API.SelectObject(hmemdc, hbitmap);
                Win32API.PrintWindow(hWnd, hmemdc, 0);
                bmp = Bitmap.FromHbitmap(hbitmap);
                //Clipboard.SetImage(bmp);
            }
            catch { }
            finally { Win32API.DeleteDC(hmemdc); Win32API.DeleteDC(hscrdc); }

            return bmp;*/

            //获得当前屏幕的大小
            RECT rect = new RECT();
            Win32API.GetWindowRect(hWnd, ref rect);
            RECT rect2 = new RECT();
            Win32API.GetClientRect(hWnd, ref rect2);
            int width = rect2.right - rect2.left;
            int height = rect2.bottom - rect2.top;
            //创建一个以当前屏幕为模板的图象
            //得到屏幕的DC
            IntPtr dc1 = Win32API.GetWindowDC(hWnd);
            //创建以屏幕大小为标准的位图 
            Image MyImage = new Bitmap(width, height);
            Graphics g2 = Graphics.FromImage(MyImage);
            //得到Bitmap的DC 
            IntPtr dc2 = g2.GetHdc();
            //调用此API函数，实现屏幕捕获
            Win32API.BitBlt(dc2, 0, 0, width, height, dc1, rect.right - rect.left - width-3, rect.bottom - rect.top - height-3, 13369376);
            //释放掉屏幕的DC
            Win32API.DeleteDC(dc1);
            //释放掉Bitmap的DC 
            g2.ReleaseHdc(dc2);

            return MyImage;
        }

        private void LiveWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsGoing = false;
        }

        private void tmMain_Tick(object sender, EventArgs e)
        {
            if (wimg != null)
            {
                try
                {
                    pbMain.Image = wimg;
                }
                catch { }
            }
        }

        private void LiveWindow_Load(object sender, EventArgs e)
        {
        }

        private void pbMain_SizeChanged(object sender, EventArgs e)
        {
        }

        private void pbMain_Resize(object sender, EventArgs e)
        {
            if (pbMain.Width > 10 && pbMain.Height > 10)
            {
                width = pbMain.Width;
                height = pbMain.Height;
            }
        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            /*if (e.Button == MouseButtons.Left)
            {
                if (ShowPointCount == 2)
                {
                    ShowPointCount = 100;
                }
                else
                {
                    ShowPointCount = 2;
                }
            }

            if (e.Button == MouseButtons.Right)
            {
                this.LockSize = !this.LockSize;
            }*/
        }

        private void btnPointsLite_Click(object sender, EventArgs e)
        {
            SI.ins.ShowPointCount = 2;
        }

        private void btnPointsAll_Click(object sender, EventArgs e)
        {
            SI.ins.ShowPointCount = 100;
        }

        private void btnLockSize_Click(object sender, EventArgs e)
        {
            this.LockSize = !this.LockSize;
            btnLockSize.Checked = this.LockSize;
        }

        private void btnRTMP_Click(object sender, EventArgs e)
        {
            SetRTMP sr = new SetRTMP(this);
            CenterChild(sr);
            sr.ShowDialog(this);
            if (RTMPAddress != "")
            {
                StartRTMP();
            }
        }

        private void StartRTMP()
        {
        }

        private void btnShowGamePic_Click(object sender, EventArgs e)
        {
            this.ShowGamePic = !this.ShowGamePic;
            btnShowGamePic.Checked = this.ShowGamePic;
        }

        /*private void StartRTMP()
        {
            if (!this.IsRTMPRunning)
            {
                this.IsRTMPRunning = true;

                var frameRate = 1000 / Timeout;
                var waitInterval = Timeout;
                var pusher = new Pusher();
                pusher.StartPush(RTMPAddress, width, height, frameRate);
                var stopEvent = new ManualResetEvent(false);

                var thread = new Thread(() =>
                {
                    var encoder = new DotNetPusher.Encoders.Encoder(width, height, frameRate, 1366*768);
                    encoder.FrameEncoded += (sender, e) =>
                    {
                        //A frame encoded.
                        var packet = e.Packet;
                        pusher.PushPacket(packet);
                        //Console.WriteLine($"Packet pushed, size:{packet.Size}.");
                    };
                    
                    try
                    {
                        while (!stopEvent.WaitOne(1) && IsGoing)
                        {
                            var start = Environment.TickCount;
                            encoder.AddImage(rimg);
                            
                            var timeUsed = Environment.TickCount - start;
                            var timeToWait = waitInterval - timeUsed;
                            Thread.Sleep(timeToWait < 0 ? 0 : timeToWait);
                        }
                        encoder.Flush();
                    }
                    finally
                    {
                        encoder.Dispose();
                        //ReleaseDC(IntPtr.Zero, screenDc);
                    }
                });

                thread.Start();

                //Console.ReadLine();

                //stopEvent.Set();

                //thread.Join();
                //pusher.StopPush();
                //pusher.Dispose();

                //Console.WriteLine("Stopped!");
                //Console.ReadLine();
            }
        }*/
    }
}
