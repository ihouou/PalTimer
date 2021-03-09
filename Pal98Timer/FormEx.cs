using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace HFrame.EX
{
    public class FormEx : Form
    {
        public SynchronizationContext SyncContext = null;
        public FormEx()
        {
            SyncContext = SynchronizationContext.Current;
        }
        public delegate object UIGET();
        public object UI(UIGET mth)
        {
            object res = null;
            SyncContext.Send(
                delegate(object a)
                {
                    try
                    {
                        res = mth();
                    }
                    catch (Exception ex)
                    {
                        Error(ex.Message);
                        Environment.Exit(0);
                    }
                },
                null);
            return res;
        }
        public delegate void UISET();
        public object UI(UISET mth)
        {
            SyncContext.Send(
                delegate(object a)
                {
                    try
                    {
                        mth();
                    }
                    catch (Exception ex)
                    {
                        Error(ex.Message);
                        Environment.Exit(0);
                    }
                },
                null);
            return null;
        }
        public static Dictionary<string, Thread> Threads = new Dictionary<string, Thread>();
        public delegate void THREADFUNC();
        public static void Run(THREADFUNC thdf)
        {
            string thdguid = Guid.NewGuid().ToString();
            Thread thd = new Thread(
                new ThreadStart(
                    delegate()
                    {
                        try
                        {
                            thdf();
                        }
                        catch { }
                        Threads.Remove(thdguid);
                    }
                    ));
            thd.Name = "(thd)" + thdguid;
            Threads.Add(thdguid, thd);
            thd.Start();
        }

        public void Error(string Content, string Title = "错误")
        {
            MessageBoxEx.Show(this, Content, Title, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }

        public void Alert(string Content, string Title = "警告")
        {
            MessageBoxEx.Show(this, Content, Title, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        }

        public void Success(string Content, string Title = "成功")
        {
            MessageBoxEx.Show(this, Content, Title, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        public void Info(string Content, string Title = "提示")
        {
            MessageBoxEx.Show(this, Content, Title, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        public bool Confirm(string Content, string Title = "询问")
        {
            DialogResult dr = MessageBoxEx.Show(this, Content, Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                return true;
            }
            return false;
        }

        public delegate void CommonCallBack(bool isok, string msg, object data1, object data2);

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FormEx
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "FormEx";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormEx_FormClosed);
            this.ResumeLayout(false);

        }

        private void FormEx_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        public void EnableAll(bool IsEnable, Control.ControlCollection p = null)
        {
            Control.ControlCollection cc = p;
            {
                if (cc == null)
                {
                    cc = this.Controls;
                }
            }
            foreach (Control c in cc)
            {
                if (c.Tag == null || c.Tag.ToString() != "EC")
                {
                    c.Enabled = IsEnable;
                }
                if (c.HasChildren)
                {
                    EnableAll(IsEnable, c.Controls);
                }
            }
        }

        public void OpenURL(string url)
        {
            if (url != "")
            {
                System.Diagnostics.Process.Start(url);
            }
        }

        public static void CenterForm(FormEx parent, FormEx child)
        {
            if (parent != null && child != null)
            {
                child.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                child.Left = parent.Left + (parent.Width - child.Width) / 2;
                child.Top = parent.Top + (parent.Height - child.Height) / 2;
            }
            else
            {
                if (child != null)
                {
                    child.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                }
            }
        }

        public void CenterChild(FormEx child)
        {
            CenterForm(this, child);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int wndproc);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        public const int GWL_STYLE = -16;
        public const int WS_DISABLED = 0x8000000;

        public void SetControlEnabled(Control c, bool enabled)
        {
            if (enabled)
            {
                SetWindowLong(c.Handle, GWL_STYLE, (~WS_DISABLED) & GetWindowLong(c.Handle, GWL_STYLE));
            }
            else
            {
                System.Drawing.Color t = c.ForeColor;
                SetWindowLong(c.Handle, GWL_STYLE, WS_DISABLED | GetWindowLong(c.Handle, GWL_STYLE));
                c.ForeColor = t;
            }
        }

    }
}
