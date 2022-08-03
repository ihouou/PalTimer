using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
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
            try
            {
                initForPaint();
            }
            catch
            { }
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
        private Image bg = null;
        private Image act = null;
        private Image si = null;
        private Image arr = null;
        private Graphics g = null;
        private Dictionary<int, Rectangle> lay = null;
        private Dictionary<int, Point> keymid = null;
        private bool imgok = false;
        private void initForPaint()
        {
            if (File.Exists("keyboard\\normal.png") && File.Exists("keyboard\\act.png") && File.Exists("keyboard\\keys.meta"))
            {
                bg = Image.FromFile("keyboard\\normal.png");
                act= Image.FromFile("keyboard\\act.png");
                if (File.Exists("keyboard\\arr.png"))
                {
                    arr = Image.FromFile("keyboard\\arr.png");
                }
                si = (Image)bg.Clone();
                g = Graphics.FromImage(si);
                OBJ o = null;
                using (FileStream fs = new FileStream("keyboard\\keys.meta", FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                    {
                        o = new OBJ(sr.ReadToEnd());
                    }
                }
                if (o != null)
                {
                    lay = new Dictionary<int, Rectangle>();
                    keymid = new Dictionary<int, Point>();
                    int xoff = o.GetValue<int>("Xoff");
                    int yoff = o.GetValue<int>("Yoff");
                    OBJ ls = o.GetValue<OBJ>("Layout");
                    for (int i = 0; i < ls.Count; ++i)
                    {
                        OBJ cur = ls.GetValue<OBJ>(i);
                        int key = cur.GetValue<int>("key");
                        if (lay.ContainsKey(key)) continue;
                        lay.Add(key, new Rectangle(cur.GetValue<int>("X")+xoff, cur.GetValue<int>("Y")+yoff, cur.GetValue<int>("W"), cur.GetValue<int>("H")));
                        keymid.Add(key, new Point(xoff+ cur.GetValue<int>("X")+ (int)(cur.GetValue<double>("W")*0.5), yoff + cur.GetValue<int>("Y") + (int)(cur.GetValue<double>("H") * 0.5)));
                    }
                }
                pbMain.Image = si;
                imgok = true;
            }
        }
        private void draw()
        {
            if (!imgok) return;
            if (!this.ShowInTaskbar) return;
            if (_hasmodify<=0) return;
            _hasmodify -= 10;
            _draw();
        }
        private void _draw()
        {
            g.DrawImage(bg, 0, 0);
            foreach (KeyValuePair<int, int> kv in ActKeys)
            {
                _drawOne(kv.Key);
            }
            foreach (KeyValuePair<int, int> kv in ActKeys)
            {
                if (kv.Key != kv.Value)
                {
                    _drawOneLink(kv.Key, kv.Value);
                }
            }

            pbMain.Refresh();
        }
        private void _drawOne(int ori)
        {
            if (lay.ContainsKey(ori))
            {
                Rectangle rc = lay[ori];
                g.DrawImage(act, rc, rc, GraphicsUnit.Pixel);
                if (ori == 13 && lay.ContainsKey(2013))
                {
                    Rectangle rc13 = lay[2013];
                    g.DrawImage(act, rc13, rc13, GraphicsUnit.Pixel);
                }
            }
        }
        private Pen linkpen = new Pen(Color.Purple, 5);
        private void _drawOneLink(int ori, int tar)
        {
            if (arr != null && keymid.ContainsKey(tar) && keymid.ContainsKey(ori))
            {
                //g.DrawLine(linkpen, keymid[ori], keymid[tar]);
                Point from = keymid[ori];
                Point to = keymid[tar];
                var sg = g.Save();
                g.TranslateTransform(from.X, from.Y);
                g.RotateTransform((float)(Math.Atan2(to.Y - from.Y, to.X - from.X) * 180 / Math.PI));
                g.DrawImage(arr, 0, (int)(0.0D - 0.5D * arr.Height), (float)Math.Sqrt(Math.Pow(to.Y - from.Y, 2) + Math.Pow(to.X - from.X, 2)), arr.Height);
                g.Restore(sg);
            }
        }

        private int _hasmodify = 100;
        private Dictionary<int, int> ActKeys = new Dictionary<int, int>();
        private void Push(int ori, int tar)
        {
            /*if (ori != tar)
            {
                Console.WriteLine("DOWN: " + ori + " -> " + tar);
            }
            else
            {
                Console.WriteLine("DOWN: " + ori);
            }*/
            if (ActKeys.ContainsKey(ori))
            {
                ActKeys[ori] = tar;
            }
            else
            {
                ActKeys.Add(ori, tar);
            }
            _hasmodify = 100;
        }
        private void Pull(int ori, int tar)
        {
            /*if (ori != tar)
            {
                Console.WriteLine("UP: " + ori + " -> " + tar);
            }
            else
            {
                Console.WriteLine("UP: " + ori);
            }*/
            if (ActKeys.ContainsKey(ori))
            {
                ActKeys.Remove(ori);
            }
            _hasmodify = 100;
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
            int c = hookStruct.vkCode;
            if (c == 13 && (hookStruct.flags == 1 || hookStruct.flags == 129))
            {
                c = 1013;
            }
            if (!IsKeyInEdit)
            {
                if (kc.IsEnable)
                {
                    if (kc.KeyMap.ContainsKey(c))
                    {
                        int v = kc.KeyMap[c];
                        if (flag == 0)
                        {
                            Push(c, v);
                        }
                        else
                        {
                            Pull(c, v);
                        }
                        handle = true;
                        KeyboardLib.keybd_event(v, KeyboardLib.MapVirtualKey((uint)v, 0), flag, 0);
                    }
                    else
                    {
                        if (flag == 0)
                        {
                            Push(c, c);
                        }
                        else
                        {
                            Pull(c, c);
                        }
                        if (((Keys)c) == Keys.Enter && (OnCtrlDown || OnCtrlDown2) && BlockCtrlEnter)
                        {
                            handle = true;
                        }
                    }
                }
                else
                {
                    if (flag == 0)
                    {
                        Push(c, c);
                    }
                    else
                    {
                        Pull(c, c);
                    }
                    if (((Keys)c) == Keys.Enter && (OnCtrlDown || OnCtrlDown2) && BlockCtrlEnter)
                    {
                        handle = true;
                    }
                }
            }
            else
            {
                CurrentKeyCode = c;
            }
            
            switch ((Keys)c)
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
            _keyboardHook.UninstallHook();
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

        private void tmMain_Tick(object sender, EventArgs e)
        {
            draw();
        }
    }
}
