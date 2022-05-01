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
        }

        private void AddPicture(string imgpath)
        {
            if (File.Exists(imgpath))
            {
                APB pb = new APB();
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Image = Image.FromFile(imgpath);
                pb.Width = pb.Image.Width;
                pb.Height = pb.Image.Height;
                pb.Left = 0;
                pb.Top = 0;

                pb.MouseEnter+= delegate (object sender, EventArgs e) {
                    ((APB)sender).Focus();
                };
                pb.MouseWheel += PbMain_MouseWheel;

                //拖动位置
                pb.MouseDown += PbMain_MouseDown;
                pb.MouseUp += PbMain_MouseUp;
                pb.MouseMove += PbMain_MouseMove;
                

                pnMain.Controls.Add(pb);
                pb.BringToFront();
            }
        }

        private void PbMain_MouseMove(object sender, MouseEventArgs e)
        {
            APB pb = sender as APB;
            if (pb.isdown)
            {
                pb.Location = new Point(pb.Location.X + e.X - pb.offx, pb.Location.Y + e.Y - pb.offy);
            }
        }

        private void PbMain_MouseUp(object sender, MouseEventArgs e)
        {
            APB pb = sender as APB;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    pb.isdown = false;
                    break;
                case MouseButtons.Middle:
                    pb.BringToFront();
                    break;
                case MouseButtons.Right:
                    pnMain.Controls.Remove(pb);
                    pb.Image.Dispose();
                    pb.Dispose();
                    break;
            }
        }
        private void PbMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                APB pb = sender as APB;
                pb.offx = e.X;
                pb.offy = e.Y;
                pb.isdown = true;
            }
        }

        private void PbMain_MouseWheel(object sender, MouseEventArgs e)
        {
            APB pb = sender as APB;
            //缩放
            if (e.Delta > 0)
            {
                pb.zoom += 0.1D;
            }
            else
            {
                pb.zoom -= 0.1D;
            }
            if (pb.zoom < 0.1D) pb.zoom = 0.1D;
            pb.Width = (int)(pb.zoom * pb.Image.Width);
            pb.Height = (int)(pb.zoom * pb.Image.Height);
        }
        public void SetData(GForm mainform, string imgpath)
        {
            //根据主窗口的大小生成视觉控件
            mf = mainform;
            AddPicture(imgpath);
            pnMain.Width = mf.Width;
            pnMain.Height = mf.Height;
            Width = mf.Width + 180;
            int h= mf.Height + 45;
            //if (h < 400) h = 400;
            Height = h;
        }

        private GForm mf;
        public Image GetResult()
        {
            //给外面返回结果
            return res;
        }

        public void x()
        {
            //bg?.Dispose();
            foreach (APB pb in pnMain.Controls)
            {
                pb.Image.Dispose();
            }
        }

        private Image res;
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

        private void btnAddImg_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                AddPicture(ofd.FileName);
            }
        }

        private void btnBGColor_Click(object sender, EventArgs e)
        {
            colorDlg.Color = pnMain.BackColor;
            if (colorDlg.ShowDialog(this) == DialogResult.OK)
            {
                pnMain.BackColor = colorDlg.Color;
            }
        }


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


    public class APB : PictureBox
    {
        public double zoom = 1.0D;
        public bool isdown = false;
        public int offx = 0;
        public int offy = 0;
    }
}
