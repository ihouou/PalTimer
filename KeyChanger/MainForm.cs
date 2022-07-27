using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace KeyChanger
{
    public partial class MainForm : Form
    {
        private const int TIMERCALL = 0x8822;
        private const int TSTAT = 1;
        private const int TEDIT = 2;
        private const int TEXIT = 3;
        private const int TENABLE = 4;
        private const int TDISABLE = 5;
        private const int TBLOCKCTRLENTER = 6;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == TIMERCALL)
            {
                int dt = m.LParam.ToInt32();
                switch (m.WParam.ToInt32())
                {
                    case TSTAT:
                        m.Result = (IntPtr)(kc.IsEnable ? 1 : 0);
                        break;
                    case TEDIT:
                        OpenSetting();
                        break;
                    case TEXIT:
                        Exit();
                        break;
                    case TENABLE:
                        kc.IsEnable = true;
                        ShowKCEnable();
                        break;
                    case TDISABLE:
                        kc.IsEnable = false;
                        ShowKCEnable();
                        break;
                    case TBLOCKCTRLENTER:
                        bool val = (dt == 1);
                        if (BlockCtrlEnter != val)
                        {
                            BlockCtrlEnter = val;
                            ShowBCEEnable();
                        }
                        break;
                }
            }
        }

        private KeyboardLib _keyboardHook = null;
        private bool IsKeyInEdit = false;
        private KC kc = new KC("");
        public int CurrentKeyCode = -1;
        public bool BlockCtrlEnter = false;
        public bool OnCtrlDown = false;
        public bool OnCtrlDown2 = false;
        public MainForm()
        {
            _keyboardHook = new KeyboardLib();
            _keyboardHook.InstallHook(this.OnKeyPress);
            ApplyKeyChange();
            InitializeComponent();
            ShowKCEnable();
            ShowBCEEnable();
            niMain.ShowBalloonTip(1000, "改建器", "已启动", ToolTipIcon.Info);
        }
        private void ApplyKeyChange()
        {
            try
            {
                if (File.Exists("keychange.txt"))
                {
                    string keychangestr = "";
                    using (FileStream fileStream = new FileStream("keychange.txt", FileMode.Open))
                    {
                        using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default))
                        {
                            keychangestr = streamReader.ReadToEnd();
                        }
                    }
                    kc = new KC(keychangestr);
                }
            }
            catch
            { }
        }
        private void Push(int ori, int tar)
        {
            if (ori != tar)
            {
                Console.WriteLine("DOWN: " + ori + " -> " + tar);
            }
            else
            {
                Console.WriteLine("DOWN: " + ori);
            }
        }
        private void Pull(int ori)
        {
            Console.WriteLine("UP: " + ori);
        }
        public void OnKeyPress(KeyboardLib.HookStruct hookStruct, out bool handle)
        {
            handle = false; //预设不拦截任何键 
            int flag = 0;
            if (hookStruct.flags >= 128)
            {
                //up
                flag = 2;
            }
            else
            {
                //down
            }
            if (!IsKeyInEdit)
            {
                if (kc.IsEnable)
                {
                    if (kc.KeyMap.ContainsKey(hookStruct.vkCode))
                    {
                        int v = kc.KeyMap[hookStruct.vkCode];
                        if (flag == 0)
                        {
                            Push(hookStruct.vkCode, v);
                        }
                        else
                        {
                            Pull(hookStruct.vkCode);
                        }
                        handle = true;
                        KeyboardLib.keybd_event(v, KeyboardLib.MapVirtualKey((uint)v, 0), flag, 0);
                    }
                    else
                    {
                        if (flag == 0)
                        {
                            Push(hookStruct.vkCode, hookStruct.vkCode);
                        }
                        else
                        {
                            Pull(hookStruct.vkCode);
                        }
                        if (((Keys)(hookStruct.vkCode)) == Keys.Enter && (OnCtrlDown || OnCtrlDown2) && BlockCtrlEnter)
                        {
                            handle = true;
                        }
                    }
                }
                else
                {
                    if (flag == 0)
                    {
                        Push(hookStruct.vkCode, hookStruct.vkCode);
                    }
                    else
                    {
                        Pull(hookStruct.vkCode);
                    }
                    if (((Keys)(hookStruct.vkCode)) == Keys.Enter && (OnCtrlDown || OnCtrlDown2) && BlockCtrlEnter)
                    {
                        handle = true;
                    }
                }
            }
            else
            {
                CurrentKeyCode = hookStruct.vkCode;
            }
            
            switch ((Keys)(hookStruct.vkCode))
            {
                case Keys.RControlKey:
                    if (hookStruct.flags >= 128)
                    {
                        OnCtrlDown2 = false;
                    }
                    else
                    {
                        OnCtrlDown2 = true;
                    }
                    break;
                case Keys.LControlKey:
                    if (hookStruct.flags >= 128)
                    {
                        OnCtrlDown = false;
                    }
                    else
                    {
                        OnCtrlDown = true;
                    }
                    break;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void btnSetKeys_Click(object sender, EventArgs e)
        {
            OpenSetting();
        }

        private void OpenSetting()
        {
            if (!IsKeyInEdit)
            {
                IsKeyInEdit = true;
                SettingForm kf = new SettingForm(this);
                kf.ShowDialog(this);
                kf.Dispose();
                ApplyKeyChange();
                ShowKCEnable();
                IsKeyInEdit = false;
            }
        }

        private void ShowKCEnable()
        {
            btnEnable.Checked = kc.IsEnable;
        }
        private void ShowBCEEnable()
        {
            btnBlockCE.Checked = BlockCtrlEnter;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.Hide();
            this.Opacity = 1;
        }

        private void Exit()
        {
            niMain.ShowBalloonTip(1000, "改建器", "已退出", ToolTipIcon.Warning);
            niMain.Dispose();
            Environment.Exit(0);
        }

        private void niMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.ShowInTaskbar = true;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                
                this.Hide();
                this.ShowInTaskbar = false;
                return;
            }
        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
            kc.IsEnable = !kc.IsEnable;
            ShowKCEnable();
        }

        private void btnBlockCE_Click(object sender, EventArgs e)
        {
            BlockCtrlEnter = !BlockCtrlEnter;
            ShowBCEEnable();
        }
    }
}
