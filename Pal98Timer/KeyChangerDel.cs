using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Pal98Timer
{
    public class KeyChangerDel
    {
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern IntPtr SendMessage(int hWnd, int msg, IntPtr wParam, IntPtr lParam);
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern int FindWindow(string lpClassName, string lpWindowName);

        private const int TIMERCALL = 0x8822;
        private const int TSTAT = 1;
        private const int TEDIT = 2;
        private const int TEXIT = 3;
        private const int TENABLE = 4;
        private const int TDISABLE = 5;
        private const int TBLOCKCTRLENTER = 6;

        private const string tar = "KeyChanger";
        private static int call(int act,int data=0)
        {
            int hwnd = FindWindow(null, "改建器");
            if (hwnd != 0)
            {
                return SendMessage(hwnd, TIMERCALL, (IntPtr)act, (IntPtr)data).ToInt32();
            }
            return 0;
        }

        private static Process kcp = null;
        public static void Open()
        {
            if (kcp == null)
            {
                Process[] pss = Process.GetProcessesByName(tar);
                if (pss.Length <= 0)
                {
                    string delpath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\" + tar + ".exe";
                    if (File.Exists(delpath))
                    {
                        kcp = new Process();

                        kcp.StartInfo.FileName = delpath;
                        //kcp.StartInfo.UseShellExecute = false;
                        //kcp.StartInfo.RedirectStandardOutput = true;
                        //kcp.StartInfo.CreateNoWindow = true;
                        try
                        {
                            kcp.Start();
                        }
                        catch
                        {
                            try
                            {
                                kcp.Close();
                            }
                            catch { }
                            try
                            {
                                kcp.Dispose();
                            }
                            catch { }
                            kcp = null;
                        }
                    }
                }
            }
        }
        public static void Close()
        {
            try
            {
                call(TEXIT);
            }
            catch { }
            if (kcp != null)
            {
                try
                {
                    kcp.Close();
                }
                catch { }
                try
                {
                    kcp.Dispose();
                }
                catch { }
                kcp = null;
            }
        }
        public static bool IsEnable()
        {
            return call(TSTAT) == 1;
        }
        public static void Edit()
        {
            call(TEDIT);
        }
        public static void BlockCtrlEnter(bool isenable)
        {
            call(TBLOCKCTRLENTER, (isenable ? 1 : 0));
        }
        public static void Enable()
        {
            call(TENABLE);
        }
        public static void Disable()
        {
            call(TDISABLE);
        }
    }
}
