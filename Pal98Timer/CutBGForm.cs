using HFrame.EX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Pal98Timer
{
    public partial class CutBGForm : Form
    {
        public CutBGForm()
        {
            InitializeComponent();

            pbMain.MouseEnter += delegate(object sender, EventArgs e) {
                pbMain.Focus();
            };
            pbMain.MouseWheel += PbMain_MouseWheel;

            //拖动位置
            pbMain.MouseDown += PbMain_MouseDown;
            pbMain.MouseUp += PbMain_MouseUp;
            pbMain.MouseMove += PbMain_MouseMove;
        }

        private void PbMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (isdown)
            {
                pbMain.Location = new Point(pbMain.Location.X + e.X - offset.X, pbMain.Location.Y + e.Y - offset.Y);
            }
        }

        private void PbMain_MouseUp(object sender, MouseEventArgs e)
        {
            isdown = false;
        }
        private bool isdown = false;
        private Point offset = new Point(0, 0);
        private void PbMain_MouseDown(object sender, MouseEventArgs e)
        {
            offset.X = e.X;
            offset.Y = e.Y;
            isdown = true;
        }

        private void PbMain_MouseWheel(object sender, MouseEventArgs e)
        {
            //缩放
            if (e.Delta > 0)
            {
                zoom += 0.1D;
            }
            else
            {
                zoom -= 0.1D;
            }
            pbMain.Width = (int)(zoom * bg.Width);
            pbMain.Height = (int)(zoom * bg.Height);
        }
        private double zoom = 1.0D;
        public void SetData(GForm mainform, string imgpath)
        {
            //根据主窗口的大小生成视觉控件
            mf = mainform;
            if (File.Exists(imgpath))
            {
                bg = Image.FromFile(imgpath);
                pbMain.Width = bg.Width;
                pbMain.Height = bg.Height;
                pbMain.Image = bg;
            }
            pnMain.Width = mf.Width;
            pnMain.Height = mf.Height;
        }

        private GForm mf;
        private Image bg;
        public Image GetResult()
        {
            //给外面返回结果
            return res;
        }

        public void x()
        {
            bg?.Dispose();
        }

        private Image res;
        /*private void MakeResult()
        {
            //根据调整的图片生成最终的背景图
            //截图吧
            using (Graphics g1 = pnMain.CreateGraphics())
            {
                res = new Bitmap(pnMain.Width, pnMain.Height, g1);
                using (Graphics g2 = Graphics.FromImage(res))
                {
                    IntPtr dc1 = g1.GetHdc();
                    IntPtr dc2 = g2.GetHdc();
                    BitBlt(dc2, 0, 0, pnMain.Width, pnMain.Height, dc1, 0, 0, 13369376);
                    g1.ReleaseHdc(dc1);
                    g2.ReleaseHdc(dc2);
                }
            }
        }*/
        private void MakeResult()
        {
            IntPtr hdc = GetWindowDC(pnMain.Handle);
            LPPOINT lphook = new LPPOINT();
            lphook.x = 0;
            lphook.y = 0;


            ClientToScreen(pnMain.Handle, ref lphook);
            ScreenToClient(pnMain.Handle, ref lphook);
            Rectangle rect = new Rectangle(lphook.x, lphook.y, pnMain.Width, pnMain.Height);
            if (hdc != IntPtr.Zero)
            {
                IntPtr hdcMem = CreateCompatibleDC(hdc);
                if (hdcMem != IntPtr.Zero)
                {
                    IntPtr hbitmap = CreateCompatibleBitmap(hdc, pnMain.Width, pnMain.Height);
                    if (hbitmap != IntPtr.Zero)
                    {
                        IntPtr ip = SelectObject(hdcMem, hbitmap);
                        if (ip != IntPtr.Zero)
                        {
                            bool a = PrintWindow(pnMain.Handle, hdcMem, 1);
                            DeleteObject(hbitmap);
                            Image tempImg = Image.FromHbitmap(hbitmap);
                            Bitmap b = new Bitmap(tempImg);
                            res = b.Clone(rect, b.PixelFormat);
                            b.Dispose();
                            tempImg.Dispose();
                        }
                    }
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //确认了背景
            MakeResult();

            DialogResult = DialogResult.OK;
            this.Close();
        }

        /*[System.Runtime.InteropServices.DllImportAttribute("gdi32.dll ")]
        private static extern bool BitBlt(
        IntPtr hdcDest, // handle to destination DC 
        int nXDest, // x-coord of destination upper-left corner 
        int nYDest, // y-coord of destination upper-left corner 
        int nWidth, // width of destination rectangle 
        int nHeight, // height of destination rectangle 
        IntPtr hdcSrc, // handle to source DC 
        int nXSrc, // x-coordinate of source upper-left corner 
        int nYSrc, // y-coordinate of source upper-left corner 
        System.Int32 dwRop // raster operation code 
        );*/
        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern bool BitBlt(
             IntPtr hdcDest, // 目标 DC的句柄  
             int nXDest,
           int nYDest,
           int nWidth,
           int nHeight,
           IntPtr hdcSrc,  // 源DC的句柄  
             int nXSrc,
           int nYSrc,
           System.Int32 dwRop  // 光栅的处理数值  
             );

        [System.Runtime.InteropServices.DllImportAttribute("User32.dll")]
        private static extern IntPtr GetWindowDC(
             IntPtr hwnd
             );

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern IntPtr CreateCompatibleDC(
             IntPtr hdc
             );

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern IntPtr CreateCompatibleBitmap(
            IntPtr hdc,
            int nWidth,     // width of bitmap, in pixels  
            int nHeight // height of bitmap, in pixels  
             );

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern IntPtr SelectObject(
            IntPtr hdc,// handle to DC  
            IntPtr hgdiobj   // handle to object  
             );

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern bool DeleteObject(
            IntPtr hObject  // handle to graphic object  
             );

        [System.Runtime.InteropServices.DllImportAttribute("User32.dll")]
        private static extern bool PrintWindow(
              IntPtr hwnd,               // Window to copy  
              IntPtr hdcBlt,             // HDC to print into  
              int nFlags              // Optional flags  
             );

        [System.Runtime.InteropServices.DllImportAttribute("User32.dll")]
        private static extern int ReleaseDC(
              IntPtr hWnd,  // handle to window  
              IntPtr hDC     // handle to DC  
             );

        [System.Runtime.InteropServices.DllImportAttribute("User32.dll")]
        private static extern bool ScreenToClient(
                IntPtr hWnd,        // handle to window  
                ref LPPOINT lpPoint   // screen coordinates  
             );

        [System.Runtime.InteropServices.DllImportAttribute("User32.dll")]
        private static extern bool ClientToScreen(
                IntPtr hWnd,        // handle to window  
                ref LPPOINT lpPoint   // screen coordinates  
             );

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
        public struct LPPOINT
        {
            public int x;
            public int y;
        }
    }
}
