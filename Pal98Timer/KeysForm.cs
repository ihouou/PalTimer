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
    public partial class KeysForm : Form
    {
        private KeyChanger kc;
        private GForm np = null;
        public KeysForm(FormEx p)
        {
            if (p is GForm)
            {
                this.np = p as GForm;
            }
            InitializeComponent();
            kc = new KeyChanger("");
            InitFromFile();
            cbEnable.Checked = kc.IsEnable;
            Showi();
        }

        public int CurrentKeyCode
        {
            get
            {
                if (np != null)
                {
                    return np.CurrentKeyCode;
                }
                return -1;
            }
        }

        private void InitFromFile()
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
                kc = new KeyChanger(keychangestr);
            }
        }

        private void Showi()
        {
            lstAll.Items.Clear();
            foreach (KeyValuePair<int, int> kv in kc.KeyMap)
            {
                lstAll.Items.Add(new klstitem(kv.Key, kv.Value));
            }
        }

        private void Save()
        {
            kc.IsEnable = cbEnable.Checked;
            try
            {
                if (File.Exists("keychange.txt"))
                {
                    File.Delete("keychange.txt");
                }
                using (FileStream fileStream = new FileStream("keychange.txt", FileMode.OpenOrCreate))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.Default))
                    {
                        streamWriter.Write(kc.ToString());
                        streamWriter.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "保存失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool IsSet = false;
        public int ori;
        public int n;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            KeyAdd ka = new KeyAdd(this);
            IsSet = false;
            ka.ShowDialog(this);
            if (IsSet)
            {
                kc.Add(ori, n);
                Showi();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Save();
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstAll.SelectedIndex >= 0)
            {
                klstitem li = (klstitem)(lstAll.Items[lstAll.SelectedIndex]);
                kc.Remove(li.ori);
                Showi();
            }
        }
    }

    public class klstitem
    {
        public int ori;
        public int n;

        public klstitem(int ori, int n)
        {
            this.ori = ori;
            this.n = n;
        }

        public override string ToString()
        {
            return KeyChanger.KeyCode2Name(ori) + " -> " + KeyChanger.KeyCode2Name(n);
        }
    }

    public class KeyChanger
    {
        public bool IsEnable;
        public Dictionary<int, int> KeyMap;
        public KeyChanger(string kcstr)
        {
            IsEnable = false;
            KeyMap = new Dictionary<int, int>();
            InitFromString(kcstr);
        }

        private void InitFromString(string kcstr)
        {
            if (kcstr != "")
            {
                kcstr = kcstr.Replace("\r", "");
                string[] spli = kcstr.Split('\n');
                if (spli.Length > 0)
                {
                    IsEnable = (spli[0] == "1");
                }
                if (spli.Length > 1)
                {
                    for (int i = 1; i < spli.Length; ++i)
                    {
                        if (spli[i].Contains(":"))
                        {
                            string[] spli2 = spli[i].Split(':');
                            if (spli2.Length == 2 && spli2[0] != "" && spli2[1] != "")
                            {
                                try
                                {
                                    KeyMap.Add(int.Parse(spli2[0]), int.Parse(spli2[1]));
                                }
                                catch 
                                { 
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Add(int ori, int n)
        {
            if (KeyMap == null)
            {
                KeyMap = new Dictionary<int, int>();
            }
            if (KeyMap.ContainsKey(ori))
            {
                KeyMap[ori] = n;
            }
            else
            {
                KeyMap.Add(ori, n);
            }
        }
        public void Remove(int ori)
        {
            if (KeyMap != null && KeyMap.ContainsKey(ori))
            {
                KeyMap.Remove(ori);
            }
        }


        public static string KeyCode2Name(int KeyCode)
        {
            return ((Keys)(KeyCode)).ToString();
        }
        public override string ToString()
        {
            string res = "";
            if (IsEnable)
            {
                res = "1";
            }
            else
            {
                res = "0";
            }
            foreach (KeyValuePair<int, int> kv in KeyMap)
            {
                res += "\r\n" + kv.Key.ToString() + ":" + kv.Value.ToString();
            }
            return res;
        }
    }
}
