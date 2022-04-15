using System;
using System.Collections.Generic;
using System.Text;
using TimerPluginBase;
namespace PAL98.BestResShow
{
    public class Main : TimerPlugin
    {
        public override EPluginPosition GetPosition()
        {
            return EPluginPosition.BR;
        }

        public override void OnEvent(string name, object data)
        {
            if (name == "LoadBest" && data!=null)
            {
                read(data.ToString());
            }
        }
        string ret = "无";
        private void read(string json)
        {
            string[] sp = json.Replace("\"", "").Replace("'", "").Replace("}", ",").Replace("]", ",").Split(',');
            ret = "蜂:" + readone(sp, "BeeHouse") + " 蜜:" + readone(sp, "BeeSheet") + " 火:" + readone(sp, "FireWorm") + " 血:" + readone(sp, "BloodLink") + " 夜:" + readone(sp, "NightCloth") + " 剑:" + readone(sp, "DragonSword");
        }
        private int readone(string[] sp, string find)
        {
            int r = 0;
            try
            {
                foreach (string s in sp)
                {
                    if (s.IndexOf(find) >= 0)
                    {
                        string[] spli = s.Split(':');
                        r = int.Parse(spli[1]);
                        break;
                    }
                }
            }
            catch { }
            return r;
        }

        public override string GetResult()
        {
            return ret;
        }

        public override void OnLoad()
        {
        }

        public override void OnUnload()
        {
        }
    }
}
