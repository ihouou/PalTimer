using HFrame.EX;
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
    public partial class PluginMgrForm : FormEx
    {
        private bool HasChanged = false;
        public PluginMgrForm()
        {
            InitializeComponent();
            this.FormClosing += PluginMgrForm_FormClosing;
            RefreshPlugins();
        }

        private void PluginMgrForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (HasChanged)
            {
                Info("启用/禁用插件后需要重置计时器才能应用！", "别忘了~");
            }
        }

        public void RefreshPlugins()
        {
            dvPlugins.Rows.Clear();
            string pluginPath = TimerPluginPackageInfo.GetPluginDir();
            if (!Directory.Exists(pluginPath)) return;
            DirectoryInfo root = new DirectoryInfo(pluginPath);
            FileInfo[] files = root.GetFiles();
            foreach (var f in files)
            {
                if (f.Name.EndsWith(".tpg"))
                {
                    string tpsfile = f.FullName;
                    if (File.Exists(tpsfile))
                    {
                        ShowOnePlugin(tpsfile);
                        try
                        {
                        }
                        catch
                        { }
                    }
                }
            }

        }
        private void ShowOnePlugin(string tpgPath)
        {
            TimerPluginPackageInfo ti = new TimerPluginPackageInfo(tpgPath);
            DataGridViewRow dr = new DataGridViewRow();
            dr.CreateCells(dvPlugins);
            dr.Cells[0].Value = ti.Enable;
            dr.Cells[0].Tag = ti;
            dr.Cells[1].Value = ti.Des;
            dr.Cells[2].Value = ti.Core;
            dr.Cells[3].Value = ti.Version;
            dr.Cells[4].Value = (ti.IsOK ? "是" : "否");

            SetRowColor(dr, ti);

            dvPlugins.Rows.Add(dr);
        }

        private void dvPlugins_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dvPlugins.RowCount && e.ColumnIndex == 0)
            {
                var row = dvPlugins.Rows[e.RowIndex];
                var cell = row.Cells[e.ColumnIndex];
                bool enable = (bool)cell.EditedFormattedValue;
                TimerPluginPackageInfo ti = cell.Tag as TimerPluginPackageInfo;
                if (enable != ti.Enable)
                {
                    ti.Enable = enable;
                    cell.Value = ti.Enable;
                    SetRowColor(row, ti);
                    HasChanged = true;
                }
            }
        }

        private void SetRowColor(DataGridViewRow dr, TimerPluginPackageInfo ti)
        {
            Color fc = Color.Black;
            if (ti.Version != TimerPluginBase.TimerPlugin.Version.ToString())
            {
                fc = Color.Gray;
            }
            else if (!ti.IsOK)
            {
                fc = Color.Red;
            }
            else if (ti.Enable)
            {
                fc = Color.Green;
            }
            dr.DefaultCellStyle.ForeColor = fc;
        }
    }
}
