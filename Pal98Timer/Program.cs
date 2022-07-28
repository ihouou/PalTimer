using System;
using System.IO;
using System.Collections.Generic;
//using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;

namespace Pal98Timer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            ClearTmpBG();
            UpdateBestFiles();
            KeyChangerDel.Open();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GForm());
            //Application.Run(new BestEditForm("CUSTOM"));
        }
        static void ClearTmpBG()
        {
            string p = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string[] fs = Directory.GetFiles(p);
            foreach (string fn in fs)
            {
                string sfn=Path.GetFileName(fn);
                if (sfn.StartsWith("tmpbg_") && sfn.EndsWith(".png"))
                {
                    File.Delete(fn);
                }
            }
        }
        /*
         老的best文件：

            bestGuJian2.txt
            Dream22_best.txt
            best5q.txt
            Pal98Steam_best.txt
            best.txt
            best2.txt
            best3.txt
             */
        static void UpdateBestFiles()
        {
            string p = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\";
            Dictionary<string, string> NeedUpdateFiles = new Dictionary<string, string>();
            NeedUpdateFiles.Add("bestGuJian2.txt", "bestGUJIAN2.txt");
            NeedUpdateFiles.Add("Dream22_best.txt", "bestDREAM22.txt");
            NeedUpdateFiles.Add("best5q.txt", "bestPAL5QSTM.txt");
            NeedUpdateFiles.Add("Pal98Steam_best.txt", "bestPAL98STM.txt");
            NeedUpdateFiles.Add("best.txt", "bestPAL98.txt");
            NeedUpdateFiles.Add("best2.txt", "bestPAL2STM.txt");
            NeedUpdateFiles.Add("best3.txt", "bestPAL3.txt");
            string[] fns = Directory.GetFiles(p);
            foreach (string fn in fns)
            {
                string sfn = Path.GetFileName(fn);
                if (NeedUpdateFiles.ContainsKey(sfn))
                {
                    string nfn = NeedUpdateFiles[sfn];
                    string content = "";
                    Encoding ed = TimerCore.GetFileEncodeType(fn);
                    using (FileStream fs = new FileStream(fn, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs, ed))
                        { 
                            content = sr.ReadToEnd();
                        }
                    }
                    File.Delete(fn);
                    using (FileStream fs = new FileStream(p + nfn, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                        {
                            sw.Write(content);
                            sw.Flush();
                        }
                    }
                }
            }
        }
    }
}
