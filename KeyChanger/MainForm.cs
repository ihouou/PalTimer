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
        private KeyboardLib _keyboardHook = null;
        private bool IsKeyInEdit = false;
        private KC kc = new KC("");
        public int CurrentKeyCode = -1;
        public MainForm()
        {
            _keyboardHook = new KeyboardLib();
            _keyboardHook.InstallHook(this.OnKeyPress);
            ApplyKeyChange();
            InitializeComponent();
            ShowKCEnable();
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
        public void OnKeyPress(KeyboardLib.HookStruct hookStruct, out bool handle)
        {
            handle = false; //预设不拦截任何键 
            if (!IsKeyInEdit)
            {
                if (kc.IsEnable)
                {
                    if (kc.KeyMap.ContainsKey(hookStruct.vkCode))
                    {
                        int flag = 0;
                        if (hookStruct.flags >= 128)
                        {
                            flag = 2;
                        }
                        int v = kc.KeyMap[hookStruct.vkCode];
                        handle = true;
                        KeyboardLib.keybd_event(v, KeyboardLib.MapVirtualKey((uint)v, 0), flag, 0);
                    }
                    /*else
                    {
                        if (((Keys)(hookStruct.vkCode)) == Keys.Enter && (OnCtrlDown || OnCtrlDown2) && this.core != null && this.core.NeedBlockCtrlEnter())
                        {
                            handle = true;
                        }
                    }*/
                }
                else
                {
                    /*if (((Keys)(hookStruct.vkCode)) == Keys.Enter && (OnCtrlDown || OnCtrlDown2) && this.core != null && this.core.NeedBlockCtrlEnter())
                    {
                        handle = true;
                    }*/
                }
            }
            else
            {
                CurrentKeyCode = hookStruct.vkCode;
            }
            /*
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
            }*/
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void btnSetKeys_Click(object sender, EventArgs e)
        {
            IsKeyInEdit = true;
            SettingForm kf = new SettingForm(this);
            kf.ShowDialog(this);
            ApplyKeyChange();
            ShowKCEnable();
            IsKeyInEdit = false;
        }

        private void ShowKCEnable()
        {
            btnEnable.Checked = kc.IsEnable;
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
            //this.niMain.Visible = false;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                
                //this.niMain.Visible = true;
                this.Hide();
                return;
            }
        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
            kc.IsEnable = !kc.IsEnable;
            ShowKCEnable();
        }
    }
}
