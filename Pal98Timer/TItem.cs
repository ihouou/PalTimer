using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pal98Timer
{
    public partial class TItem : UserControl
    {
        public static string TimeSpanToString(TimeSpan ts)
        {
            return ts.Hours.ToString().PadLeft(2, '0') + ":" + ts.Minutes.ToString().PadLeft(2, '0') + ":" + ts.Seconds.ToString().PadLeft(2, '0') + "." + (ts.Milliseconds / 10).ToString().PadLeft(2, '0');
        }

        public static string TimeSpanToStringLite(TimeSpan ts)
        {
            return ts.Hours.ToString().PadLeft(2, '0') + ":" + ts.Minutes.ToString().PadLeft(2, '0') + ":" + ts.Seconds.ToString().PadLeft(2, '0');
        }

        private MainForm parent;
        public TItem()
        {
            InitializeComponent();
        }
        public TItem(MainForm p)
        {
            this.parent = p;
            InitializeComponent();
        }

        private CheckPoint _cp;
        private bool HasStatic = false;
        public void InitShow(CheckPoint cp)
        {
            _cp = cp;
            lblTitle.Text = cp.GetNickName();
            lblBest.Text = TimeSpanToStringLite(cp.Best);
            lblCurrent.Text = "--:--:--.--";
        }

        public void Flush()
        {
            if (!_cp.IsBegin)
            {
                lblCurrent.Text = "--:--:--.--";
            }
            else
            {
                if (_cp.IsEnd)
                {
                    if (!HasStatic)
                    {
                        HasStatic = true;
                        _cp.Current = _cp.Current.Subtract(new TimeSpan(0,0,0,0,_cp.Current.Milliseconds));
                        if (_cp.Current < _cp.Best)
                        {
                            lblCurrent.Text = TimeSpanToStringLite(_cp.Current) + " -" + (_cp.Best - _cp.Current).TotalSeconds.ToString("F0");
                            lblCurrent.ForeColor = parent.FasterColor;
                        }
                        else
                        {
                            lblCurrent.Text = TimeSpanToStringLite(_cp.Current) + " +" + (_cp.Current - _cp.Best).TotalSeconds.ToString("F0");
                            lblCurrent.ForeColor = parent.SlowerColor;
                        }
                    }
                }
                else
                {
                    lblCurrent.Text = TimeSpanToString(_cp.Current);
                    if (_cp.Current < _cp.Best)
                    {
                        lblCurrent.ForeColor = parent.FasterColor;
                    }
                    else
                    {
                        lblCurrent.ForeColor = parent.SlowerColor;
                    }
                }
            }
        }

        private void lblTitle_DoubleClick(object sender, EventArgs e)
        {
            if (parent != null)
            {
                parent.Jump(_cp.Index);
            }
        }
    }
}
