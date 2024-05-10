using HFrame.ENT;
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
    public partial class BestEditForm : Form , iBestEditForm
    {
        private string tarfile;
        private string beststr = "";
        public BestEditForm(string bestFile)
        {
            if (string.IsNullOrEmpty(bestFile)) throw new Exception();
            InitializeComponent();
            if (bestFile.IndexOf("\\") >= 0)
            {
                tarfile = bestFile;
            }
            else
            {
                if (!bestFile.StartsWith("best"))
                {
                    bestFile = "best" + bestFile;
                }
                if (!bestFile.EndsWith(".txt"))
                {
                    bestFile += ".txt";
                }
                tarfile = bestFile;
            }

            if (File.Exists(tarfile))
            {
                initBest();
            }
            
        }

        private int _idx = 0;
        private int IDX
        {
            get
            {
                _idx++;
                return _idx;
            }
        }

        private void initBest()
        {
            Encoding charset = TimerCore.GetFileEncodeType(tarfile);
            using (FileStream fileStream = new FileStream(tarfile, FileMode.Open))
            {
                using (StreamReader streamReader = new StreamReader(fileStream, charset))
                {
                    beststr = streamReader.ReadToEnd();
                }
            }
            List<object> tmp = (new HObj(beststr)).GetValue<HObj>("CheckPoints").ToList();
            foreach (object o in tmp)
            {
                HObj co = o as HObj;
                BestEditItem bi = new BestEditItem(this, co.GetValue<string>("name"), co.GetValue<string>("des"), TimerCore.ConvertTimeSpan(co.GetValue<string>("time")), IDX);
                pnMain.Controls.Add(bi);
            }
        }

        public void CallRemoveItem(BestEditItem c)
        {
            pnMain.Controls.Remove(c);
        }

        private string GetResult()
        {
            HObj b;
            if (HObj.IsJson(beststr))
            {
                b = new HObj();
            }
            else
            {
                b = new HObj(beststr);
                b.Remove("CheckPoints");
            }

            HObj cps = new HObj("[]");
            foreach(BestEditItem i in pnMain.Controls)
            {
                HObj cur = new HObj();
                CheckPointNewer cp = i.GetValue();
                cur["name"] = cp.Name;
                cur["des"]=cp.NickName.Replace("\"", "").Replace("'", "");
                cur["time"] = TItem.TimeSpanToFullString(cp.BestTS);
                cps.Add(cur);
            }

            b["CheckPoints"] = cps;
            return b.ToJson();
        }

        private void btnOK_Click(object sender, EventArgs e)
        { 
            StringBuilder sb=new StringBuilder();
            sb.AppendLine("即将覆盖您的最佳线配置文件").AppendLine(tarfile).AppendLine("确定继续么？");
            sb.AppendLine("是：覆盖并使用新编辑的最佳线").AppendLine("否：将放弃本次更改并使用老的最佳线").AppendLine("取消：将返回继续编辑");
            switch (MessageBox.Show(this, sb.ToString(), "确认修改", MessageBoxButtons.YesNoCancel))
            {
                case DialogResult.Yes:
                    SaveToTarFile();
                    this.Dispose();
                    break;
                case DialogResult.No:
                    this.Dispose();
                    break;
            }
        }

        private void SaveToTarFile()
        {
            if (File.Exists(tarfile))
            {
                File.Delete(tarfile);
            }
            using (FileStream fs = new FileStream(tarfile, FileMode.Create, FileAccess.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(GetResult());
                    sw.Flush();
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            BestEditItem bi = new BestEditItem(this, "", "", TimeSpan.FromTicks(0), IDX);
            pnMain.Controls.Add(bi);
        }
    }
}
