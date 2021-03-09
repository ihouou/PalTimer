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
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private const int VM_NCLBUTTONDOWN = 0XA1;//定义鼠标左键按下
        private const int HTCAPTION = 2;

        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 0x10;
        private const int HTBOTTOMRIGHT = 17;

        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_MINIMIZEBOX = 0x00020000;  // Winuser.h中定义  
                CreateParams cp = base.CreateParams;
                cp.Style = cp.Style | WS_MINIMIZEBOX;   // 允许最小化操作  
                return cp;
            }
        }

        public NoneBoardFormEx()
        {
            this.FormBorderStyle = FormBorderStyle.None;
        }

        public void SetFormCloseControl(Control c)
        {
            c.Click += C_Click;
        }

        private void C_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void SetFormMoveControl(Control c)
        {
            c.MouseDown += C_MouseDown;
        }

        private void C_MouseDown(object sender, MouseEventArgs e)
        {
            //为当前应用程序释放鼠标捕获
            ReleaseCapture();
            //发送消息 让系统误以为在标题栏上按下鼠标
            SendMessage((IntPtr)this.Handle, VM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        protected override void WndProc(ref Message m)
        {
            //base.WndProc(ref m);
            switch (m.Msg)

            {

                case 0x0084:

                    base.WndProc(ref m);

                    Point vPoint = new Point((int)m.LParam & 0xFFFF,

                     (int)m.LParam >> 16 & 0xFFFF);

                    vPoint = PointToClient(vPoint);

                    if (vPoint.X <= 5)

                        if (vPoint.Y <= 5)

                            m.Result = (IntPtr)HTTOPLEFT;

                        else if (vPoint.Y >= ClientSize.Height - 5)


                            m.Result = (IntPtr)HTBOTTOMLEFT;

                        else m.Result = (IntPtr)HTLEFT;

                    else if (vPoint.X >= ClientSize.Width - 5)


                        if (vPoint.Y <= 5)

                            m.Result = (IntPtr)HTTOPRIGHT;

                        else if (vPoint.Y >= ClientSize.Height - 5)


                            m.Result = (IntPtr)HTBOTTOMRIGHT;

                        else m.Result = (IntPtr)HTRIGHT;

                    else if (vPoint.Y <= 5)


                        m.Result = (IntPtr)HTTOP;

                    else if (vPoint.Y >= ClientSize.Height - 5)


                        m.Result = (IntPtr)HTBOTTOM;

                    break;

                case 0x0201://鼠标左键按下的消息 用于实现拖动窗口功能

                    m.Msg = 0x00A1;//更改消息为非客户区按下鼠标

                    m.LParam = IntPtr.Zero;//默认值

                    m.WParam = new IntPtr(2);//鼠标放在标题栏内

                    base.WndProc(ref m);

                    break;

                default:

                    base.WndProc(ref m);

                    break;

            }
        }

    }
}
