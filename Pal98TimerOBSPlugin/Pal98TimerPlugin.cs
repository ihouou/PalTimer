using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLROBS;

namespace Pal98TimerOBSPlugin
{
    public class Pal98TimerPlugin : AbstractPlugin
    {
        public Pal98TimerPlugin()
        {
            Name = "Pal98Timer";
            Description = "Pal98 Auto Timer,for fast clear.";
        }

        public override bool LoadPlugin()
        {
            API.Instance.AddImageSourceFactory(new Pal98TimerSourceFactory());
            return true;
        }
    }
}
