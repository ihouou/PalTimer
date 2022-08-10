using HFrame.EX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Pal98Timer
{
    public class NoneBoardFormEx : FormEx
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwdn, int wMsg, int mParam, int lParam);
        //常量
        public const int WM_SYSCOMMAND = 0x0112;

        //窗体移动
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        //改变窗体大小
        public const int WMSZ_LEFT = 0xF001;
        public const int WMSZ_RIGHT = 0xF002;
        public const int WMSZ_TOP = 0xF003;
        public const int WMSZ_TOPLEFT = 0xF004;
        public const int WMSZ_TOPRIGHT = 0xF005;
        public const int WMSZ_BOTTOM = 0xF006;
        public const int WMSZ_BOTTOMLEFT = 0xF007;
        public const int WMSZ_BOTTOMRIGHT = 0xF008;

        private const int BORDER_WIDTH = 5;
        private long lastmutick = 0;
        public NoneBoardFormEx()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            _bindEvents();
        }
        public NoneBoardFormEx(bool DoubleBuffered)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            if (DoubleBuffered)
            {
                this.DoubleBuffered = true;
                this.SetStyle(ControlStyles.ResizeRedraw, true);
            }
            _bindEvents();
        }
        private void _bindEvents()
        {
            this.MouseDown += delegate (object sender, MouseEventArgs e) {
                int mx = (this.Width - BORDER_WIDTH);
                int my = (this.Height - BORDER_WIDTH);
                int tar = 0;
                if (e.X <= BORDER_WIDTH)
                {
                    //left
                    if (e.Y <= BORDER_WIDTH)
                    {
                        tar = WMSZ_TOPLEFT;
                    }
                    else if (e.Y >= my)
                    {
                        tar = WMSZ_BOTTOMLEFT;
                    }
                    else
                    {
                        tar = WMSZ_LEFT;
                    }
                }
                else if (e.X >= mx)
                {
                    //right
                    if (e.Y <= BORDER_WIDTH)
                    {
                        tar = WMSZ_TOPRIGHT;
                    }
                    else if (e.Y >= my)
                    {
                        tar = WMSZ_BOTTOMRIGHT;
                    }
                    else
                    {
                        tar = WMSZ_RIGHT;
                    }
                }
                else
                {
                    //mid
                    if (e.Y <= BORDER_WIDTH)
                    {
                        tar = WMSZ_TOP;
                    }
                    else if (e.Y >= my)
                    {
                        tar = WMSZ_BOTTOM;
                    }
                    else
                    {
                        int x = this.Left;
                        int y = this.Top;
                        ReleaseCapture();
                        SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);//向Windows发送拖动窗体的消息
                        if (this.Left == x && this.Top == y)
                        {
                            this.OnMouseUp(e);
                            long now = DateTime.Now.Ticks;
                            long cha = now - lastmutick;
                            lastmutick = now;
                            if (cha < 3000000)
                            {
                                lastmutick = 0;
                                this.OnMouseDoubleClick(e);
                            }
                        }
                        return;
                    }
                }
                if (tar != 0)
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, WM_SYSCOMMAND, tar, 0);//向Windows发送拖动窗体的消息
                }
            };
            this.MouseMove += delegate (object sender, MouseEventArgs e) {
                int mx = (this.Width - BORDER_WIDTH);
                int my = (this.Height - BORDER_WIDTH);
                if (e.X <= BORDER_WIDTH)
                {
                    //left
                    if (e.Y <= BORDER_WIDTH)
                    {
                        this.Cursor = Cursors.SizeNWSE;
                    }
                    else if (e.Y >= my)
                    {
                        this.Cursor = Cursors.SizeNESW;
                    }
                    else
                    {
                        this.Cursor = Cursors.SizeWE;
                    }
                }
                else if (e.X >= mx)
                {
                    //right
                    if (e.Y <= BORDER_WIDTH)
                    {
                        this.Cursor = Cursors.SizeNESW;
                    }
                    else if (e.Y >= my)
                    {
                        this.Cursor = Cursors.SizeNWSE;
                    }
                    else
                    {
                        this.Cursor = Cursors.SizeWE;
                    }
                }
                else
                {
                    //mid
                    if (e.Y <= BORDER_WIDTH)
                    {
                        this.Cursor = Cursors.SizeNS;
                    }
                    else if (e.Y >= my)
                    {
                        this.Cursor = Cursors.SizeNS;
                    }
                    else
                    {
                        this.Cursor = Cursors.Arrow;
                    }
                }
            };
        }
        
    }
}
