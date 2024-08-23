using System;
using System.Collections.Generic;
using System.Text;

namespace Pal98Timer
{
    public class PalPackVersion
    {
        private static PalPackVersion _ins=null;
        public static PalPackVersion ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new PalPackVersion();
                }
                return _ins;
            }
        }

        private Dictionary<string, string> v;
        private PalPackVersion()
        {
            v = new Dictionary<string, string>();
            // 05D0F8993A610E37462802B79FC962A6
            //pal.dll DATA.MKF SSS.MKF VB40032.dll
            v.Add("678AA2883A4D487B4A04DDA7E05C94B6_D4B3F9B98B9926CCC49A473C86FE2A2C_7A6A09BA26F59F22554670928B85B8F2_05D0F8993A610E37462802B79FC962A6", "黑插件：天命之人");
            v.Add("678AA2883A4D487B4A04DDA7E05C94B6_D4B3F9B98B9926CCC49A473C86FE2A2C_7A6A09BA26F59F22554670928B85B8F2_17DB6A514B5FDC737DD44BA49AD6D76E", "3.0.2014.628");
            v.Add("F749181D643267ACE64E4C62AFC4E783_D4B3F9B98B9926CCC49A473C86FE2A2C_87EE7653337FDDBE865AA3A514DE2806_17DB6A514B5FDC737DD44BA49AD6D76E", "2.0.10.3");
            //pal.dll DATA.MKF SSS.MKF
            v.Add("C1822644B008CCB0EED47FF8FB73D54A_D4B3F9B98B9926CCC49A473C86FE2A2C_87EE7653337FDDBE865AA3A514DE2806", "PAL98Steam");
        }
        public string GetPalPackVersion(string paldllmd5)
        {
            if (v.ContainsKey(paldllmd5))
            {
                return v[paldllmd5];
            }
            else
            {
                return "UNKNOWN";
            }
        }
    }
}
