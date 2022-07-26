using System.Collections.Generic;
using System.Windows.Forms;

namespace KeyChanger
{
    public class KC
    {
        public bool IsEnable;
        public Dictionary<int, int> KeyMap;
        public KC(string kcstr)
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
