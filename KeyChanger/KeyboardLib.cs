﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace KeyChanger
{
    public class KeyboardLib
    {
        private const int WH_KEYBOARD_LL = 13; //键盘 

        //键盘处理事件委托 ,当捕获键盘输入时调用定义该委托的方法. 
        private delegate int HookHandle(int nCode, int wParam, IntPtr lParam);

        //客户端键盘处理事件 
        public delegate void ProcessKeyHandle(HookStruct param, out bool handle);

        //接收SetWindowsHookEx返回值 
        private static int _hHookValue = 0;

        //勾子程序处理事件 
        private HookHandle _KeyBoardHookProcedure;

        //Hook结构 
        [StructLayout(LayoutKind.Sequential)]
        public class HookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        //设置钩子 
        [DllImport("user32.dll")]
        private static extern int SetWindowsHookEx(int idHook, HookHandle lpfn, IntPtr hInstance, int threadId);

        //取消钩子 
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern bool UnhookWindowsHookEx(int idHook);

        //调用下一个钩子 
        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

        //获取当前线程ID 
        [DllImport("kernel32.dll")]
        private static extern int GetCurrentThreadId();

        //Gets the main module for the associated process. 
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string name);

        private IntPtr _hookWindowPtr = IntPtr.Zero;

        //构造器 
        public KeyboardLib() { }

        //外部调用的键盘处理事件 
        private static ProcessKeyHandle _clientMethod = null;

        /// <summary> 
        /// 安装勾子 
        /// </summary> 
        /// <param name="hookProcess">外部调用的键盘处理事件</param> 
        public void InstallHook(ProcessKeyHandle clientMethod)
        {
            _clientMethod = clientMethod;

            // 安装键盘钩子 
            if (_hHookValue == 0)
            {
                _KeyBoardHookProcedure = new HookHandle(OnHookProc);

                _hookWindowPtr = GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName);

                _hHookValue = SetWindowsHookEx(
                WH_KEYBOARD_LL,
                _KeyBoardHookProcedure,
                _hookWindowPtr,
                0);

                //如果设置钩子失败. 
                if (_hHookValue == 0) UninstallHook();
            }
        }

        //取消钩子事件 
        public void UninstallHook()
        {
            if (_hHookValue != 0)
            {
                bool ret = UnhookWindowsHookEx(_hHookValue);
                if (ret) _hHookValue = 0;
            }
        }

        //钩子事件内部调用,调用_clientMethod方法转发到客户端应用。 
        private static int OnHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                //转换结构 
                HookStruct hookStruct = (HookStruct)Marshal.PtrToStructure(lParam, typeof(HookStruct));

                if (_clientMethod != null)
                {
                    bool handle = false;
                    //调用客户提供的事件处理程序。 
                    _clientMethod(hookStruct, out handle);
                    if (handle) return 1; //1:表示拦截键盘,return 退出 
                }
            }
            return CallNextHookEx(_hHookValue, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", EntryPoint = "keybd_event")]
        public static extern void keybd_event(
            int bVk,
            int bScan,
            int dwFlags,  //这里是整数类型  0 为按下，2为释放
            int dwExtraInfo  //这里是整数类型 一般情况下设成为 0
        );

        [DllImport("user32.dll")]
        public static extern int MapVirtualKey(uint Ucode, uint uMapType);

    }
}
