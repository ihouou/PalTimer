using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLROBS;

namespace Pal98TimerOBSPlugin
{
    class Pal98TimerSourceFactory : AbstractImageSourceFactory
    {
        public Pal98TimerSourceFactory()
        {
            ClassName = "Pal98TimerOBSPlugin";
            DisplayName = "Pal98Timer";
        }

        public override ImageSource Create(XElement data)
        {
            return new Pal98TimerSource(data);
        }

        public override bool ShowConfiguration(XElement data)
        {
            return true;//设置为true可以在来源区显示
        }
    }
}
