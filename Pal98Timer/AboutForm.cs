using HFrame.EX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Pal98Timer
{
    public partial class AboutForm : FormEx
    {
        public AboutForm()
        {
            InitializeComponent();
            ShowUI(pnC,InitData(InitCreaterInfo()));
        }
        private Dictionary<string, string[]> InitCreaterInfo()
        {
            Dictionary<string, string[]> r = new Dictionary<string, string[]>();
            r.Add("Houou", new string[3] { "Houou", "https://space.bilibili.com/248838981", "1" });
            r.Add("兰", new string[3] { "兰", "https://space.bilibili.com/1643478", "1" });
            r.Add("狐狸", new string[3] { "呀MissFOX", "https://space.bilibili.com/43133106", "0" });
            r.Add("寒泠", new string[3] { "寒泠幽洌", "https://space.bilibili.com/1179827922", "1" });
            r.Add("发财鼠", new string[3] { "追梦发财鼠", "https://space.bilibili.com/108891098", "1" });
            r.Add("小小绝", new string[3] { "小小绝", "https://space.bilibili.com/386841894", "1" });
            r.Add("云出无心", new string[3] { "zyx云出无心", "https://space.bilibili.com/282885914", "1" });
            r.Add("阿绫", new string[3] { "无敌的阿绫又倒下了", "https://space.bilibili.com/66935559", "1" });
            r.Add("江月", new string[3] { "江月/星辰月", "https://space.bilibili.com/1757879609", "0" });
            r.Add("回忆", new string[3] { "回忆", "https://space.bilibili.com/1917199059", "1" });
            r.Add("齐大", new string[3] { "齐小伙", "https://space.bilibili.com/520126686", "1" });
            r.Add("麻烦", new string[3] { "麻烦（鼻祖）", "https://www.douyu.com/1561684", "1" });
            r.Add("wjjjj12", new string[3] { "wjjjj12", "https://space.bilibili.com/1953354", "1" });
            r.Add("!官网", new string[3] { "www.palspeed.com", "http://www.palspeed.com", "2" });
            r.Add("!各位玩家", new string[3] { "各位玩家", "http://www.palspeed.com/player/", "2" });
            r.Add("!github", new string[3] { "https://github.com/ihouou/PalTimer", "https://github.com/ihouou/PalTimer", "2" });
            r.Add("!gitee", new string[3] { "https://gitee.com/houou/PalTimer", "https://gitee.com/houou/PalTimer", "2" });
            return r;
        }
        private HText InitData(Dictionary<string, string[]> c)
        {
            HText ht = new HText();
            ht.Text("本软件以及所属插件均永久开源免费")
                .Title("主程序")
                .Link(c["Houou"]).Space().Text("QQ群：27735311")
                .Title("视觉")
                .Text("界面：").Link(c["Houou"]).Space().Text("图标：").Link(c["兰"]).Space().Link(c["狐狸"])
                .Title("内核贡献")
                .Text("仙剑98柔情：").Link(c["麻烦"]).Space().Link(c["!各位玩家"])
                .Line()
                .Text("仙剑98 Steam：").Link(c["寒泠"])
                .Line()
                .Text("仙剑二 Steam：").Link(c["齐大"]).Space().Link(c["寒泠"]).Line().Space(14).Link(c["发财鼠"]).Space().Link(c["wjjjj12"])
                .Line()
                .Text("仙剑三：").Link(c["小小绝"])
                .Line()
                .Text("仙剑5前传：").Link(c["云出无心"]).Space().Link(c["阿绫"])
                .Line()
                .Text("古剑2官方：").Link(c["江月"])
                .Line()
                .Text("梦幻2.2：").Link(c["回忆"])
                .Title("更多请访问官网")
                .Link(c["!官网"])
                .Title("开源")
                .Text("GitHub：").Link(c["!github"])
                .Line()
                .Text("Gitee：").Link(c["!gitee"]);

            return ht;
        }
        private void ShowUI(Control c,HText ht)
        {
            c.Controls.Clear();
            int curlineheight = 0;
            int y = 0;
            foreach (List<Control> line in ht.lines)
            {
                c.Height += curlineheight;
                y += curlineheight;

                curlineheight = 0;
                int x = 0;
                foreach (Control cell in line)
                {
                    int h = cell.PreferredSize.Height + cell.Padding.Top + cell.Padding.Bottom;
                    
                    if (h > curlineheight) curlineheight = h+5;
                    if ((x + cell.PreferredSize.Width) > c.Width) c.Width = x + cell.PreferredSize.Width;
                    cell.Left = x;
                    cell.Top = y+ cell.Padding.Top;
                    x += cell.PreferredSize.Width;
                    c.Controls.Add(cell);
                }
            }
        }

        public class HText
        {
            public List<List<Control>> lines;
            public List<Control> curline;
            public HText()
            {
                lines = new List<List<Control>>();
                curline = new List<Control>();
                lines.Add(curline);
            }

            public HText Line()
            {
                curline = new List<Control>();
                lines.Add(curline);
                return this;
            }
            public HText Title(string title,bool newLine=true)
            {
                if (newLine)
                {
                    if (curline.Count > 0)
                    {
                        Line();
                    }
                }
                Label lbl = new Label();
                lbl.AutoSize = true;
                lbl.Font = new Font(lbl.Font.FontFamily, lbl.Font.Size * 1.2F, FontStyle.Bold);
                lbl.TextAlign = ContentAlignment.BottomLeft;
                lbl.Text = title;
                lbl.Padding = new Padding(lbl.Padding.Left, 10, lbl.Padding.Right, 2);
                //lbl.Height += 15;
                curline.Add(lbl);
                if (newLine)
                {
                    Line();
                }
                return this;
            }
            public HText Space(int charCount=2)
            {
                Text("".PadLeft(charCount, ' '));
                return this;
            }
            public HText Text(string txt)
            {
                Label lbl = new Label();
                lbl.AutoSize = true;
                lbl.Text = txt;
                curline.Add(lbl);
                return this;
            }
            public HText Link(string txt, string url,bool isF,bool isWebSite)
            {
                Label lbl = new Label();
                lbl.AutoSize = true;
                lbl.Font = new Font(lbl.Font, FontStyle.Underline);
                lbl.Cursor = Cursors.Hand;
                lbl.Text = txt;
                lbl.Tag = url;
                if (isF)
                {
                    lbl.ForeColor = Color.DeepPink;
                }
                else if (isWebSite)
                {
                    lbl.ForeColor = Color.Blue;
                }
                else
                {
                    lbl.ForeColor = Color.RoyalBlue;
                }
                lbl.Click += delegate(object sender, EventArgs e) {
                    string u = ((Label)sender).Tag.ToString();
                    if (u.StartsWith("http"))
                    {
                        System.Diagnostics.Process.Start(u);
                    }
                };
                curline.Add(lbl);
                return this;
            }
            public HText Link(string[] txtANDurl)
            {
                Link(txtANDurl[0], txtANDurl[1], txtANDurl[2] == "0", txtANDurl[2] == "2");
                return this;
            }
        }
    }
}
