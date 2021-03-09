using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Pal98Timer
{
    public class PngDraw
    {
        private static int Col1Off = 220;
        private static int Col2Off = 140;
        //private int Col3Off = 100;
        private static int Col4Off = 70;
        private static int RowHeightOff = 18;
        private static int PointFontSize = 12;
        private static Bitmap bp = null;

        public static Image Draw(TimerData td, int width = 960, int height = 600)
        {
            bp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bp))
            {
                try
                {
                    g.DrawString(td.GameVersion, new Font("黑体", 10), new SolidBrush(Color.Green), 10, 10);
                    g.DrawString(td.Version + "  " + td.CloudID, new Font("黑体", 10), new SolidBrush(Color.White), 200, 10);
                    g.DrawString("蜂:" + td.FC + " " + "蜜:" + td.FM + " " + "火:" + td.HCG + " " + "血:" + td.XLL + " " + "夜:" + td.YXY + " " + "剑:" + td.LQJ, new Font("黑体", 12, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 128, 0)), 10, 30);
                    g.DrawString(td.Luck, new Font("黑体", 12, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 0, 0)), 10, 80);
                    g.DrawString("战斗时长：" + td.BattleLong, new Font("黑体", 12, FontStyle.Bold), new SolidBrush(Color.FromArgb(0, 255, 255)), 10, 50);
                    g.DrawString(td.MT, new Font("黑体", 18, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 255, 128)), 10, height - 50);
                    g.DrawString(td.ST, new Font("黑体", 10, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 255, 128)), 10, height - 65);
                    g.DrawString(td.ColorEgg, new Font("黑体", 12), new SolidBrush(Color.DeepPink), 10, height - 20);


                    //时间线
                    int RowYPos = height - RowHeightOff;
                    for (int i = 0; i < td.cpls.Count; ++i)
                    {
                        RowYPos -= RowHeightOff;
                        CheckPointLite cp = td.cpls[i];

                        if (cp.IsCurrent)
                        {
                            g.DrawString(">", new Font("黑体", PointFontSize, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 255, 128)), width - Col1Off - 20, RowYPos);
                        }
                        g.DrawString(cp.Name, new Font("黑体", PointFontSize), new SolidBrush(Color.FromArgb(255, 255, 128)), width - Col1Off, RowYPos);
                        g.DrawString(cp.Best, new Font("黑体", PointFontSize - 0), new SolidBrush(Color.FromArgb(255, 255, 180)), width - Col2Off, RowYPos);
                        if (cp.CHA.IndexOf("-") >= 0)
                        {
                            g.DrawString(cp.CHA, new Font("黑体", PointFontSize, FontStyle.Bold), new SolidBrush(Color.FromArgb(0, 255, 0)), width - Col4Off, RowYPos);
                        }
                        else if (cp.CHA.IndexOf("+") >= 0)
                        {
                            g.DrawString(cp.CHA, new Font("黑体", PointFontSize, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 0, 0)), width - Col4Off, RowYPos);
                        }
                    }
                    g.DrawString("预计通关  " + td.WillClear, new Font("黑体", PointFontSize), new SolidBrush(Color.Orange), width - Col1Off + 20, height - RowHeightOff);
                }
                catch { }
            }
            return bp;
            
        }
    }
}
