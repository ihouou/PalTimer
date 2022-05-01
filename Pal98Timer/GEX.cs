using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Pal98Timer
{
    public class GEX
    {
        public static int GDIMulti(int ori, float multi)
        {
            return (int)(multi * ori);
        }
        public static void ClearRect(Graphics g, Rectangle rect, Image bg = null, int bgW = 0, int bgH = 0)
        {
            if (bg == null || bgW <= 0 || bgH <= 0)
            {
                var tmp = g.CompositingMode;
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                SolidBrush b = new SolidBrush(Color.FromArgb(0, 0, 0, 0));
                //g.FillRectangle(b, x, y, w, h);
                g.FillRectangle(b, rect);
                g.CompositingMode = tmp;
            }
            else
            {
                float iw = (float)bg.Width / bgW;
                float ih = (float)bg.Height / bgH;
                g.DrawImage(bg, rect,
                new Rectangle(
                    GDIMulti(rect.X, iw),
                    GDIMulti(rect.Y, ih),
                    GDIMulti(rect.Width, iw),
                    GDIMulti(rect.Height, ih)
                    ),
                GraphicsUnit.Pixel);
            }
        }
        public static void DrawText(Graphics g, string text, Font font, Brush fillBrush, Pen strokePen, Rectangle rect, StringFormat sf)
        {
            if (text == null) text = "";
            using (GraphicsPath p = new GraphicsPath())
            {
                p.AddString(text, font.FontFamily, (font.Bold ? ((int)FontStyle.Bold) : 0) | (font.Italic ? ((int)FontStyle.Italic) : 0) | (font.Underline ? ((int)FontStyle.Underline) : 0), font.Size, rect, sf);
                SmoothingMode tmp = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                if (strokePen != null)
                {
                    g.DrawPath(strokePen, p);
                }
                g.FillPath(fillBrush, p);
                g.SmoothingMode = tmp;
            }
        }
        public static GraphicsPath RadiusRectPath(int x, int y, int w, int h, int r)
        {
            GraphicsPath GraphicsPath1 = new GraphicsPath();

            //x 坐标
            float[] itXs = new float[3]
            {
        x + r,
        x + w - 1 - r,
        x + w - 1
            };
            //y 坐标
            float[] itYs = new float[3]
            {
        y + r,
        y + h - 1 - r,
        y + h - 1
            };

            //左边
            GraphicsPath1.AddLine(x, itYs[1], x, itYs[0]);
            //左上角
            GraphicsPath1.AddArc(x, y, 2 * r, 2 * r, 180, 90);

            //上边
            GraphicsPath1.AddLine(itXs[0], y, itXs[1], y);
            //右上角
            GraphicsPath1.AddArc(itXs[1] - r, y, 2 * r, 2 * r, -90, 90);

            //右边
            GraphicsPath1.AddLine(itXs[2], itYs[0], itXs[2], itYs[1]);
            //右下角
            GraphicsPath1.AddArc(itXs[1] - r, itYs[1] - r, 2 * r, 2 * r, 0, 90);

            //下边
            GraphicsPath1.AddLine(itXs[1], itYs[2], itXs[0], itYs[2]);
            //左下角
            GraphicsPath1.AddArc(x, itYs[1] - r, 2 * r, 2 * r, 90, 90);
            return GraphicsPath1;
        }
    }

    public class GRender
    {
        public bool IsForceRefreshAll = false;
        public bool IsForceRefreshAllMode = false;

        private string Title;
        private bool isTitleChanged = false;
        public void SetTitle(string val)
        {
            if (Title != val)
            {
                Title = val;
                isTitleChanged = true;
            }
        }

        private string GameVersion;
        private bool isGameVersionChanged = false;
        public void SetGameVersion(string val)
        {
            if (GameVersion != val)
            {
                GameVersion = val;
                isGameVersionChanged = true;
            }
        }

        private string Version;
        private bool isVersionChanged = false;
        public void SetVersion(string val)
        {
            if (Version != val)
            {
                Version = val;
                isVersionChanged = true;
            }
        }

        private string BL;
        private bool isBLChanged = false;
        public void SetBL(string val)
        {
            if (BL != val)
            {
                BL = val;
                isBLChanged = true;
            }
        }

        private string BR;
        private bool isBRChanged = false;
        public void SetBR(string val)
        {
            if (BR != val)
            {
                BR = val;
                isBRChanged = true;
            }
        }

        private string MoreInfo;
        private bool isMoreInfoChanged = false;
        public void SetMoreInfo(string val)
        {
            if (MoreInfo != val)
            {
                MoreInfo = val;
                isMoreInfoChanged = true;
            }
        }

        private string SubTimer;
        private bool isSubTimerChanged = false;
        public void SetSubTimer(string val)
        {
            if (SubTimer != val)
            {
                SubTimer = val;
                isSubTimerChanged = true;
            }
        }

        private string OutTimer;
        private bool isOutTimerChanged = false;
        public void SetOutTimer(string val)
        {
            if (OutTimer != val)
            {
                OutTimer = val;
                isOutTimerChanged = true;
            }
        }

        private string WillClear;
        private bool isWillClearChanged = false;
        public void SetWillClear(string val)
        {
            if (WillClear != val)
            {
                WillClear = val;
                isWillClearChanged = true;
            }
        }

        private TimeSpan MainTimer = new TimeSpan(0);
        private bool isMainTimerChanged = false;
        private bool isMainTimerMSChanged = false;
        public void SetMainTimer(TimeSpan val)
        {
            if (MainTimer.Ticks != val.Ticks)
            {
                if (MainTimer.Milliseconds != val.Milliseconds)
                {
                    isMainTimerMSChanged = true;
                }
                if (((long)(MainTimer.TotalSeconds)) != ((long)(val.TotalSeconds)))
                {
                    isMainTimerChanged = true;
                }
                MainTimer = val;
                if (ItemIdx >= 0 && ItemIdx < itemList.Count)
                {
                    itemList[ItemIdx].Cur = val;
                }
            }
        }
        private bool isInCheck = false;
        public bool IsInCheck
        {
            get
            {
                return isInCheck;
            }
            set
            {
                if (isInCheck != value)
                {
                    isInCheck = value;
                    isMainTimerChanged = true;
                }
            }
        }

        private bool isC = false;
        private bool isCChanged = false;
        public bool IsC
        {
            get
            {
                return isC;
            }
            set
            {
                if (isC != value)
                {
                    isC = value;
                    isCChanged = true;
                }
            }
        }

        private Control BaseCtl = null;
        private Image CI;
        private Graphics CG;
        private Image configIcon;
        private bool IsEdit = false;
        public GRender(Control baseCtl, bool isEdit = false)
        {
            ClearAllDots();
            createConfigIconFromBase64();

            if (baseCtl != null)
            {
                BaseCtl = baseCtl;
                foreach (Control c in BaseCtl.Controls)
                {
                    if (c.Tag == null) continue;
                    switch (c.Tag.ToString())
                    {
                        case ":close":
                            close_ctl = c;
                            break;
                        case ":config":
                            config_ctl = c;
                            break;
                        case ":function":
                            btn_ctl = c;
                            break;
                    }
                }
                if (!isEdit)
                {
                    this.bindCtlEvents();
                }
                else
                {
                    IsEdit = true;
                    BaseCtl.MouseMove += OnEditMouseMove;
                    BaseCtl.MouseLeave += delegate (object sender, EventArgs e) {
                        CurrentEdit = null;
                    };
                }
                CI = new Bitmap(BaseCtl.Width, BaseCtl.Height);
                CG = Graphics.FromImage(CI);
                BaseCtl.BackgroundImage = CI;
            }
        }

        private void bindCtlEvents()
        {
            if (btn_ctl != null)
            {
                btn_ctl.MouseClick += onFuncAreaClicked;
            }
            if (BaseCtl != null)
            {
                BaseCtl.MouseDoubleClick += onBaseMouseDBClick;
                BaseCtl.MouseWheel += onBaseMouseWheel;
                BaseCtl.MouseEnter += delegate (object sender, EventArgs e) {
                    BaseCtl.Focus();
                };
            }
        }
        private void createConfigIconFromBase64()
        {
            string base64 = "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";
            try
            {
                MemoryStream ms = new MemoryStream(Convert.FromBase64String(base64));
                configIcon = Image.FromStream(ms);
            }
            catch { }
        }
        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            int tar = 0;
            if (e.Delta > 0) tar = -1;
            else if (e.Delta < 0) tar = 1;
            if (rcDots.Contains(e.Location))
            {
                DotScroll += tar * 10;
                isDotScroll = true;
            }
            else if (rcItems.Contains(e.Location))
            {
                ItemScroll += tar;
                isItemScroll = true;
            }
        }
        public delegate void delOnDBClickItem(GItem item);
        public delegate void delOnMainTimerDBClicked();
        public delOnDBClickItem OnDBClickItem = null;
        public delOnMainTimerDBClicked OnMainTimerDBClicked = null;
        private void OnDBClicked(Point pos)
        {
            if (OnDBClickItem != null && itemList.Count > 0 && rcItems.Contains(pos))
            {
                int idx = (int)Math.Floor(((double)(pos.Y - rcItems.Y)) / GItem.Height);
                idx += ItemScroll;
                if (idx >= 0 && idx < itemList.Count)
                {
                    OnDBClickItem(itemList[idx]);
                }
            }
            if (OnMainTimerDBClicked != null && (rcMainTimer.Contains(pos) || rcMainTimerMS.Contains(pos) || rcIsC.Contains(pos)))
            {
                OnMainTimerDBClicked();
            }
        }
        private void OnFunctionAreaClicked(Point pos)
        {
            int x = pos.X + btn_rc.X;
            foreach (GBtn b in btnList)
            {
                if (b.IsXIn(x))
                {
                    if (b.Enabled)
                    {
                        b.OnClicked?.Invoke(x, btn_rc.Y, b);
                    }
                    return;
                }
            }
        }
        private Image bg = null;
        private bool isBGChanged = false;
        public void SetBG(string path)
        {
            Image tmp = null;
            if (File.Exists(path))
            {
                tmp = Image.FromFile(path);
            }
            if (bg == null && tmp != null)
            {
                bg = new Bitmap(tmp);
                tmp.Dispose();
                isBGChanged = true;
            }
            else if (bg != null && tmp == null)
            {
                tmp = bg;
                bg = null;
                tmp.Dispose();
                isBGChanged = true;
            }
            else if (bg != null && tmp != null)
            {
                Image t = bg;
                bg = new Bitmap(tmp);
                tmp.Dispose();
                t.Dispose();
                isBGChanged = true;
            }
        }

        public const int DotHeight = 30;
        private Bitmap LastDots;
        private bool isDotsChanged = true;
        private int curDotFillIdx = 0;
        private List<string> cacheDots = new List<string>();
        public void AddDot(string str, bool isFromCache = false)
        {
            if (!isFromCache)
            {
                cacheDots.Add(str);
            }
            Graphics g = Graphics.FromImage(LastDots);
            int addw = (int)(g.MeasureString(str, bb.DotTextFont).Width);
            g.Dispose();
            addw += 20;
            Bitmap tmp = new Bitmap(LastDots.Width + 5 + addw, LastDots.Height);
            g = Graphics.FromImage(tmp);
            Rectangle rect = new Rectangle(3, 0, addw, LastDots.Height);
            using (GraphicsPath p = GEX.RadiusRectPath(rect.X, rect.Y, rect.Width, rect.Height, GEX.GDIMulti(rect.Height, 0.5F)))
            {
                var mode = g.SmoothingMode;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.FillPath(bb.DotFills[curDotFillIdx], p);
                g.DrawPath(bb.DotBorder, p);
                g.SmoothingMode = mode;
                curDotFillIdx++;
                if (curDotFillIdx >= bb.DotFills.Length) curDotFillIdx = 0;
            }
            GEX.DrawText(g, str, bb.DotTextFont, bb.DotTextFill, bb.DotTextBorder, rect, GLayout.sfCC);
            g.DrawImage(LastDots, addw + 5, 0);
            g.Dispose();
            Bitmap willdis = LastDots;
            LastDots = tmp;
            willdis.Dispose();
            if (!isFromCache)
            {
                if (DotScroll != 0)
                {
                    DotScroll = 0;
                    isDotScroll = true;
                }
                isDotsChanged = true;
            }
        }
        private bool isDotScroll = false;
        private int DotScroll = 0;
        public void ClearAllDots()
        {
            if (LastDots == null)
            {
                LastDots = new Bitmap(1, DotHeight);
            }
            else
            {
                Bitmap tmp = LastDots;
                LastDots = new Bitmap(1, DotHeight);
                tmp.Dispose();
            }
            if (DotScroll != 0)
            {
                DotScroll = 0;
                isDotScroll = true;
            }
            cacheDots.Clear();
            isDotsChanged = true;
        }
        public void RefreshDotsStyle()
        {
            if (LastDots == null)
            {
                LastDots = new Bitmap(1, DotHeight);
            }
            else
            {
                Bitmap tmp = LastDots;
                LastDots = new Bitmap(1, DotHeight);
                tmp.Dispose();
            }
            foreach (string d in cacheDots)
            {
                AddDot(d, true);
            }
            isDotScroll = true;
            isDotsChanged = true;
        }

        private GBoard bb = null;
        private bool isBBChanged = false;
        public void SetGBoard(GBoard gb)
        {
            if (bb == null)
            {
                bb = gb;
            }
            else
            {
                bb = gb;
                GC.Collect();
            }
            isBBChanged = true;
        }
        public GBoard GetGBoard()
        {
            return bb;
        }
        public void SetGBoardChanged()
        {
            isBBChanged = true;
        }

        public class GBtn
        {
            private GRender _parent = null;
            public GBtn(GRender p)
            {
                _parent = p;
            }
            public GBtn(int idx, GRender p)
            {
                _idx = idx;
                _parent = p;
            }
            public void Remove()
            {
                _parent?.RemoveBtn(this);
            }
            private int _idx;
            public int Index
            {
                get { return _idx; }
            }
            public delegate void delOnClicked(int x, int y, GBtn ctl);
            public const int Width = 60;
            public const int Margin = 5;
            private string _text;
            public string Text
            {
                get
                {
                    return _text;
                }
                set
                {
                    if (_text != value)
                    {
                        _text = value;
                        isDirty = true;
                    }
                }
            }
            private int _style = 0;//0:白 1:红 2:橙
            public bool IsWhite()
            {
                return _style == 0;
            }
            public bool IsRed()
            {
                return _style == 1;
            }
            public bool IsOrange()
            {
                return _style == 2;
            }
            public GBtn White()
            {
                if (_style != 0)
                {
                    _style = 0;
                    isDirty = true;
                }
                return this;
            }
            public GBtn Red()
            {
                if (_style != 1)
                {
                    _style = 1;
                    isDirty = true;
                }
                return this;
            }
            public GBtn Orange()
            {
                if (_style != 2)
                {
                    _style = 2;
                    isDirty = true;
                }
                return this;
            }
            private bool isDirty = true;
            public delOnClicked OnClicked = null;
            private bool _enable = true;
            public bool Enabled
            {
                get
                {
                    return _enable;
                }
                set
                {
                    if (_enable != value)
                    {
                        _enable = value;
                        isDirty = true;
                    }
                }
            }
            public bool IsDirty
            {
                get { return isDirty; }
            }
            private Rectangle rectCache = new Rectangle();
            public bool IsXIn(int x)
            {
                if (!IsVisible) return false;
                return x >= rectCache.X && x <= (rectCache.X + rectCache.Width);
            }
            public bool IsVisible = true;
            public void Draw(Graphics g, Rectangle rect, GBoard bb)
            {
                rectCache.X = rect.X;
                rectCache.Width = rect.Width;
                using (GraphicsPath p = GEX.RadiusRectPath(rect.X, rect.Y, rect.Width, rect.Height, GEX.GDIMulti(rect.Height, 0.5F)))
                {
                    var mode = g.SmoothingMode;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.FillPath(bb.ButtonFill, p);
                    g.DrawPath(bb.ButtonBorder, p);
                    g.SmoothingMode = mode;
                }
                switch (_style)
                {
                    case 1:
                        //red
                        GEX.DrawText(g, Text, bb.ButtonTextFont, bb.ButtonTextFillRed, bb.ButtonTextBorder, rect, GLayout.sfCC);
                        break;
                    case 2:
                        //orange
                        GEX.DrawText(g, Text, bb.ButtonTextFont, bb.ButtonTextFillOrange, bb.ButtonTextBorder, rect, GLayout.sfCC);
                        break;
                    default:
                        //white
                        GEX.DrawText(g, Text, bb.ButtonTextFont, bb.ButtonTextFill, bb.ButtonTextBorder, rect, GLayout.sfCC);
                        break;
                }
                isDirty = false;
            }
        }
        private List<GBtn> btnList = new List<GBtn>();
        private bool isBtnsChanged = false;
        public GBtn AddBtn(string text = null, GBtn.delOnClicked onClicked = null,int index = 0)
        {
            GBtn b = new GBtn(index, this);
            btnList.Add(b);
            btnList.Sort(delegate (GBtn A, GBtn B)
            {
                return A.Index - B.Index;
            });
            if (!string.IsNullOrEmpty(text))
            {
                b.Text = text;
            }
            if (onClicked != null)
            {
                b.OnClicked = onClicked;
            }
            isBtnsChanged = true;
            return b;
        }
        public void RemoveBtn(GBtn b)
        {
            int willdel = -1;
            for (int i = 0; i < btnList.Count; ++i)
            {
                if (btnList[i] == b)
                {
                    willdel = i;
                    break;
                }
            }
            if (willdel != -1)
            {
                btnList.RemoveAt(willdel);
                isBtnsChanged = true;
            }
        }
        public void ClearAllBtn()
        {
            if (btnList.Count > 0)
            {
                btnList.Clear();
                isBtnsChanged = true;
            }
        }
        public void VisibleBtn(int idx, bool isVisible)
        {
            if (btnList == null || btnList.Count <= 0) return;
            if (idx < 0 || idx >= btnList.Count) return;
            GBtn cur = btnList[idx];
            if (isVisible != cur.IsVisible)
            {
                cur.IsVisible = isVisible;
                isBtnsChanged = true;
            }
        }

        public class GItem
        {
            public const int EditMetaFast = -10;
            public const int EditMetaSlow = -20;
            public const int EditMetaEqual = -30;
            public const int EditMetaActive = -40;
            public const int Height = 28;
            public const int HalfHeight = 14;
            private bool isDirty = true;
            public bool IsDirty
            {
                get { return isDirty; }
            }
            private bool isTimeDirty = false;
            public bool IsTimeDirty
            {
                get { return isTimeDirty; }
            }
            private int _logicidx = 0;
            public int Index
            {
                get { return _logicidx; }
            }
            private string _name;
            public string Name
            {
                get { return _name; }
            }
            private TimeSpan _best;
            public TimeSpan Best
            {
                get { return _best; }
            }
            private int _metaNum = -1;
            public int MetaNum
            {
                get { return _metaNum; }
            }
            public GItem(int logicIdx, string name, TimeSpan best, int metaNum = 0)
            {
                _logicidx = logicIdx;
                _name = name;
                _best = best;
                if (metaNum != 0)
                {
                    _metaNum = metaNum;
                }
                else
                {
                    _metaNum = 2000 - logicIdx;
                }
                isDirty = true;
            }
            private TimeSpan _cha = new TimeSpan(0);
            public TimeSpan Cha
            {
                get { return _cha; }
            }
            private TimeSpan _cur = new TimeSpan(0);
            public TimeSpan Cur
            {
                get
                {
                    return _cur;
                }
                set
                {
                    if (value.Ticks != _cur.Ticks)
                    {
                        _cur = value;
                        _cha = _cur - _best;
                        isTimeDirty = true;
                    }
                }
            }
            private bool _ispassed = false;
            public bool IsPassed
            {
                get { return _ispassed; }
                set
                {
                    if (value != _ispassed)
                    {
                        _ispassed = value;
                        isDirty = true;
                    }
                }
            }
            private bool _isact = false;
            public bool IsActive
            {
                get { return _isact; }
                set
                {
                    if (value != _isact)
                    {
                        _isact = value;
                        isDirty = true;
                    }
                }
            }
            public bool Draw(Graphics g, bool isForceDrawAll, GBoard bb, Rectangle rcName, Rectangle rcBest, Rectangle rcCha, Rectangle rcCur, Image bg, int bgWidth, int bgHeight, delUpdateRect ur = null)
            {
                bool ret = false;
                if (isForceDrawAll) isDirty = true;
                if (isDirty) isTimeDirty = true;

                ret = (IsDirty || IsTimeDirty);

                bool needFillBG = (_logicidx % 2 == 0);
                if (isDirty)
                {
                    GEX.ClearRect(g, rcName, bg, bgWidth, bgHeight);
                    GEX.ClearRect(g, rcBest, bg, bgWidth, bgHeight);
                    if (IsActive)
                    {
                        g.FillRectangles(bb.CPItemActBG, new Rectangle[] { rcName, rcBest });
                    }
                    else if (needFillBG)
                    {
                        g.FillRectangles(bb.CPItemBG, new Rectangle[] { rcName, rcBest });
                    }

                    GEX.DrawText(g, _name, bb.CPNameFont, bb.CPNameFill, bb.CPNameBorder, rcName, GLayout.sfCC);
                    GEX.DrawText(g, TS2HHMMSS(_best), bb.CPBestFont, bb.CPBestFill, bb.CPBestBorder, rcBest, GLayout.sfFC);
                    if (!isForceDrawAll)
                    {
                        ur?.Invoke(rcName);
                        ur?.Invoke(rcBest);
                    }
                }

                if (IsTimeDirty)
                {
                    GEX.ClearRect(g, rcCha, bg, bgWidth, bgHeight);
                    GEX.ClearRect(g, rcCur, bg, bgWidth, bgHeight);
                    if (IsActive)
                    {
                        g.FillRectangles(bb.CPItemActBG, new Rectangle[] { rcCha, rcCur });
                    }
                    else if (needFillBG)
                    {
                        g.FillRectangles(bb.CPItemBG, new Rectangle[] { rcCha, rcCur });
                    }

                    if (_ispassed || _isact)
                    {
                        Brush fill = bb.CPSameFill;
                        Pen border = bb.CPSameBorder;
                        if (_cha.Ticks < 0)
                        {
                            //快
                            fill = bb.CPGoodFill;
                            border = bb.CPGoodBorder;
                            GEX.DrawText(g, _cha.Hours == 0 ? ("-" + TS2MSS(_cha)) : ("-" + TS2HMMSS(_cha)), bb.CPChaFont, fill, border, rcCha, GLayout.sfFC);
                        }
                        else if (_cha.Ticks > 10000000)
                        {
                            //慢
                            fill = bb.CPBadFill;
                            border = bb.CPBadBorder;
                            GEX.DrawText(g, _cha.Hours == 0 ? ("+" + TS2MSS(_cha)) : ("+" + TS2HMMSS(_cha)), bb.CPChaFont, fill, border, rcCha, GLayout.sfFC);
                        }
                        else
                        {
                            //同
                            fill = bb.CPSameFill;
                            border = bb.CPSameBorder;
                            GEX.DrawText(g, "0:00", bb.CPChaFont, fill, border, rcCha, GLayout.sfFC);
                        }

                        GEX.DrawText(g, TS2HHMMSSFF(_cur), bb.CPCurFont, fill, border, rcCur, GLayout.sfFC);
                    }

                    if (!isForceDrawAll)
                    {
                        ur?.Invoke(rcCha);
                        ur?.Invoke(rcCur);
                    }
                }

                isTimeDirty = false;
                isDirty = false;
                return ret;
            }
        }
        public static string TS2HHMMSS(TimeSpan ts)
        {
            return ts.Hours.ToString().PadLeft(2, '0') + ":" + ts.Minutes.ToString().PadLeft(2, '0') + ":" + ts.Seconds.ToString().PadLeft(2, '0');
        }
        public static string TS2HHMMSSFF(TimeSpan ts)
        {
            return ts.Hours.ToString().PadLeft(2, '0') + ":" + ts.Minutes.ToString().PadLeft(2, '0') + ":" + ts.Seconds.ToString().PadLeft(2, '0') + "." + ts.Milliseconds.ToString().PadLeft(3, '0').Substring(0, 2);
        }
        public static string TS2MSS(TimeSpan ts)
        {
            return Math.Abs(ts.Minutes).ToString() + ":" + Math.Abs(ts.Seconds).ToString().PadLeft(2, '0');
        }
        public static string TS2HMMSS(TimeSpan ts)
        {
            return Math.Abs(ts.Hours).ToString() + ":" + Math.Abs(ts.Minutes).ToString().PadLeft(2, '0') + ":" + Math.Abs(ts.Seconds).ToString().PadLeft(2, '0');
        }
        private List<GItem> itemList = new List<GItem>();
        private bool isItemsChanged = false;
        public GItem AddItem(string Name, TimeSpan Best, int MetaNum = -1)
        {
            GItem i = new GItem(itemList.Count, Name, Best, MetaNum);
            itemList.Add(i);
            isItemsChanged = true;
            return i;
        }
        public void ClearAllItem()
        {
            itemList.Clear();
            isItemsChanged = true;
        }
        public GItem GetItem(int idx)
        {
            if (idx >= 0 && idx < itemList.Count)
            {
                return itemList[idx];
            }
            return null;
        }
        private int _cidx = -1;
        public int ItemIdx
        {
            get { return _cidx; }
            set
            {
                if (_cidx != value)
                {
                    _cidx = value;
                    int CanShowCount = (int)Math.Floor(((double)(rcItems.Height)) / GItem.Height);
                    if (CanShowCount < itemList.Count)
                    {
                        while ((CanShowCount + ItemScroll) < (value+2))
                        {
                            ItemScroll++;
                        }
                        while ((value-1) < ItemScroll)
                        {
                            ItemScroll--;
                        }
                        if (ItemScroll < 0) ItemScroll = 0;
                        isItemScroll = true;
                    }
                    for (int i = 0; i < itemList.Count; ++i)
                    {
                        GItem cur = itemList[i];
                        if (i == _cidx)
                        {
                            cur.IsActive = true;
                            cur.IsPassed = false;
                        }
                        else if (i < _cidx)
                        {
                            cur.IsActive = false;
                            cur.IsPassed = true;
                        }
                        else
                        {
                            cur.IsActive = false;
                            cur.IsPassed = false;
                        }
                    }
                }
            }
        }
        private int ItemScroll = 0;
        private bool isItemScroll = false;

        private Rectangle rcTitle = new Rectangle();
        private Rectangle rcGameVersion = new Rectangle();
        private Rectangle rcVersion = new Rectangle();
        private Rectangle rcBL = new Rectangle();
        private Rectangle rcBR = new Rectangle();
        private Rectangle rcMoreInfo = new Rectangle();
        private Rectangle rcMainTimer = new Rectangle();
        private Rectangle rcMainTimerMS = new Rectangle();
        private Rectangle rcIsC = new Rectangle();
        private Rectangle rcSubTimer = new Rectangle();
        private Rectangle rcOutTimer = new Rectangle();
        private Rectangle rcWillClear = new Rectangle();
        private Rectangle rcDots = new Rectangle();
        private Rectangle rcItems = new Rectangle();
        private Rectangle rcItemScroll = new Rectangle();
        private Rectangle rcItemScBlock = new Rectangle();
        private Rectangle rcIName = new Rectangle();
        private Rectangle rcIBest = new Rectangle();
        private Rectangle rcICha = new Rectangle();
        private Rectangle rcICur = new Rectangle();
        private void ModifyRect(ref Rectangle rect, int x, int y, int w, int h)
        {
            rect.X = x;
            rect.Y = y;
            rect.Width = w;
            rect.Height = h;
        }
        private void BuildRects()
        {
            ModifyRect(ref rcTitle, 5, 5, Width - 100, 26);
            ModifyRect(ref rcGameVersion, 5, 31, GEX.GDIMulti(Width, 0.7F), 26);
            ModifyRect(ref rcVersion, rcGameVersion.X + rcGameVersion.Width, rcGameVersion.Y, Width - 2 * rcGameVersion.X - rcGameVersion.Width, rcGameVersion.Height);
            ModifyRect(ref rcBL, 5, Height - 26, GEX.GDIMulti(Width, 0.4F), 26);
            ModifyRect(ref rcBR, rcBL.X + rcBL.Width, rcBL.Y, Width - 2 * rcBL.X - rcBL.Width, rcBL.Height);

            ModifyRect(ref rcMoreInfo, 5, Height - 100, Width - 10, 26);
            ModifyRect(ref rcMainTimer, 20, Height - 150, Width - 65, 50);
            ModifyRect(ref rcMainTimerMS, rcMainTimer.X + rcMainTimer.Width, rcMainTimer.Y, Width - 25 - rcMainTimer.Width, rcMainTimer.Height);
            ModifyRect(ref rcIsC, -1, rcMainTimer.Y + 5, 20, rcMainTimer.Height - 10);

            ModifyRect(ref rcSubTimer, 10, Height - 170, GEX.GDIMulti(Width, 0.35F), 26);
            ModifyRect(ref rcOutTimer, rcSubTimer.X + rcSubTimer.Width, rcSubTimer.Y, Width - 2 * rcSubTimer.X - rcSubTimer.Width, rcSubTimer.Height);
            ModifyRect(ref rcWillClear, 10, Height - 200, Width - 20, 26);

            ModifyRect(ref rcDots, 0, 60, Width, 30);

            /*ModifyRect(ref rcItems, 5, 95, Width - 10, Height - 200 - 95);
            ModifyRect(ref rcIName, rcItems.X, 0, rcItems.Width - 170, GItem.Height);
            ModifyRect(ref rcIBest, rcItems.X + rcItems.Width - 170, 0, 60, GItem.HalfHeight);
            ModifyRect(ref rcICha, rcIBest.X, 0, rcIBest.Width, GItem.Height - GItem.HalfHeight);
            ModifyRect(ref rcICur, rcItems.X + rcItems.Width - 110, 0, 110, GItem.Height);*/
            BuildRects_Item(rcItemScroll.Width > 0);
        }
        private void BuildRects_Item(bool showScroll)
        {
            if (showScroll)
            {
                ModifyRect(ref rcItems, 0, 95, Width - 10, Height - 200 - 95);
                ModifyRect(ref rcItemScroll, Width - 6, rcItems.Y, 3, rcItems.Height);
            }
            else
            {
                ModifyRect(ref rcItems, 0, 95, Width, Height - 200 - 95);
                ModifyRect(ref rcItemScroll, 0, 0, 0, 0);
            }
            ModifyRect(ref rcIName, rcItems.X, 0, rcItems.Width - 170, GItem.Height);
            ModifyRect(ref rcIBest, rcItems.X + rcItems.Width - 170, 0, 60, GItem.HalfHeight);
            ModifyRect(ref rcICha, rcIBest.X, 0, rcIBest.Width, GItem.Height - GItem.HalfHeight);
            ModifyRect(ref rcICur, rcItems.X + rcItems.Width - 110, 0, 110, GItem.Height);
        }
        public GBoardChanger CurrentEdit = null;
        public delegate void delOnEditCurrentChanged(GBoardChanger gbc);
        public delOnEditCurrentChanged OnEditCurrentChanged = null;
        private void addFontSize(ref Font ori, int delta)
        {
            Font old = ori;
            float s = old.Size + delta;
            if (s < 7) s = 7;
            ori = new Font(old.FontFamily, s, old.Style);
            old.Dispose();
        }
        private void resetFont(ref Font ori, Font newf)
        {
            Font old = ori;
            ori = newf.Clone() as Font;
            old.Dispose();
            newf.Dispose();
        }
        private void OnEditMouseMove(object sender, MouseEventArgs e)
        {
            if (rcTitle.Contains(e.Location))
            {
                if (CurrentEdit == null || CurrentEdit.type != 1)
                {
                    CurrentEdit = new GBoardChanger() { type = 1, Name = "标题", Description = "标题区域" };
                    CurrentEdit.OnLeftClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cColor != null)
                        {
                            bb.TitleFill.Color = cColor(bb.TitleFill.Color);
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnRightClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cColor != null)
                        {
                            bb.TitleBorder.Color = cColor(bb.TitleBorder.Color);
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnMiddleClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cFont != null)
                        {
                            resetFont(ref bb.TitleFont, cFont(bb.TitleFont));
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnWheel = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        addFontSize(ref bb.TitleFont, delta);
                        IsForceRefreshAll = true;
                    };
                    OnEditCurrentChanged?.Invoke(CurrentEdit);
                }
            }
            else if (rcGameVersion.Contains(e.Location))
            {
                if (CurrentEdit == null || CurrentEdit.type != 2)
                {
                    CurrentEdit = new GBoardChanger() { type = 2, Name = "游戏版本", Description = "游戏版本区域" };
                    CurrentEdit.OnLeftClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cColor != null)
                        {
                            bb.GVersionFill.Color = cColor(bb.GVersionFill.Color);
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnRightClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cColor != null)
                        {
                            bb.GVersionBorder.Color = cColor(bb.GVersionBorder.Color);
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnMiddleClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cFont != null)
                        {
                            resetFont(ref bb.GVersionFont, cFont(bb.GVersionFont));
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnWheel = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        addFontSize(ref bb.GVersionFont, delta);
                        IsForceRefreshAll = true;
                    };
                    OnEditCurrentChanged?.Invoke(CurrentEdit);
                }
            }
            else if (rcVersion.Contains(e.Location))
            {
                if (CurrentEdit == null || CurrentEdit.type != 3)
                {
                    CurrentEdit = new GBoardChanger() { type = 3, Name = "计时器版本", Description = "计时器版本区域" };
                    CurrentEdit.OnLeftClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cColor != null)
                        {
                            bb.VersionFill.Color = cColor(bb.VersionFill.Color);
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnRightClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cColor != null)
                        {
                            bb.VersionBorder.Color = cColor(bb.VersionBorder.Color);
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnMiddleClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cFont != null)
                        {
                            resetFont(ref bb.VersionFont, cFont(bb.VersionFont));
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnWheel = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        addFontSize(ref bb.VersionFont, delta);
                        IsForceRefreshAll = true;
                    };
                    OnEditCurrentChanged?.Invoke(CurrentEdit);
                }
            }
            else if (rcBL.Contains(e.Location))
            {
                if (CurrentEdit == null || CurrentEdit.type != 4)
                {
                    CurrentEdit = new GBoardChanger() { type = 4, Name = "好运签", Description = "好运签区域" };
                    CurrentEdit.OnLeftClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cColor != null)
                        {
                            bb.BLFill.Color = cColor(bb.BLFill.Color);
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnRightClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cColor != null)
                        {
                            bb.BLBorder.Color = cColor(bb.BLBorder.Color);
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnMiddleClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cFont != null)
                        {
                            resetFont(ref bb.BLFont, cFont(bb.BLFont));
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnWheel = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        addFontSize(ref bb.BLFont, delta);
                        IsForceRefreshAll = true;
                    };
                    OnEditCurrentChanged?.Invoke(CurrentEdit);
                }
            }
            else if (rcBR.Contains(e.Location))
            {
                if (CurrentEdit == null || CurrentEdit.type != 5)
                {
                    CurrentEdit = new GBoardChanger() { type = 5, Name = "皮言皮语", Description = "皮言皮语区域" };
                    CurrentEdit.OnLeftClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cColor != null)
                        {
                            bb.BRFill.Color = cColor(bb.BRFill.Color);
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnRightClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cColor != null)
                        {
                            bb.BRBorder.Color = cColor(bb.BRBorder.Color);
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnMiddleClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cFont != null)
                        {
                            resetFont(ref bb.BRFont, cFont(bb.BRFont));
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnWheel = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        addFontSize(ref bb.BRFont, delta);
                        IsForceRefreshAll = true;
                    };
                    OnEditCurrentChanged?.Invoke(CurrentEdit);
                }
            }
            else if (rcMoreInfo.Contains(e.Location))
            {
                if (CurrentEdit == null || CurrentEdit.type != 6)
                {
                    CurrentEdit = new GBoardChanger() { type = 6, Name = "特殊信息", Description = "特殊信息区域" };
                    CurrentEdit.OnLeftClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cColor != null)
                        {
                            bb.MoreInfoFill.Color = cColor(bb.MoreInfoFill.Color);
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnRightClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cColor != null)
                        {
                            bb.MoreInfoBorder.Color = cColor(bb.MoreInfoBorder.Color);
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnMiddleClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cFont != null)
                        {
                            resetFont(ref bb.MoreInfoFont, cFont(bb.MoreInfoFont));
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnWheel = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        addFontSize(ref bb.MoreInfoFont, delta);
                        IsForceRefreshAll = true;
                    };
                    OnEditCurrentChanged?.Invoke(CurrentEdit);
                }
            }
            else if (rcMainTimer.Contains(e.Location) || rcMainTimerMS.Contains(e.Location))
            {
                if (CurrentEdit == null || CurrentEdit.type != 7)
                {
                    CurrentEdit = new GBoardChanger() { type = 7, Name = "主计时", Description = "主计时区域" };
                    CurrentEdit.OnLeftClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cColor != null)
                        {
                            bb.MainTimerFill.Color = cColor(bb.MainTimerFill.Color);
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnRightClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cColor != null)
                        {
                            bb.MainTimerBorder.Color = cColor(bb.MainTimerBorder.Color);
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnMiddleClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cFont != null)
                        {
                            resetFont(ref bb.MainTimerFont, cFont(bb.MainTimerFont));
                            Font old = bb.MainTimerMSFont;
                            float mss = bb.MainTimerFont.Size - 14;
                            if (mss < 6) mss = 6;
                            bb.MainTimerMSFont = new Font(bb.MainTimerFont.FontFamily, mss, bb.MainTimerFont.Style);
                            old.Dispose();
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnWheel = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        addFontSize(ref bb.MainTimerFont, delta);
                        addFontSize(ref bb.MainTimerMSFont, delta);
                        IsForceRefreshAll = true;
                    };
                    OnEditCurrentChanged?.Invoke(CurrentEdit);
                }
            }
            else if (rcSubTimer.Contains(e.Location))
            {
                if (CurrentEdit == null || CurrentEdit.type != 9)
                {
                    CurrentEdit = new GBoardChanger() { type = 9, Name = "副计时", Description = "一般用于显示战斗时长" };
                    CurrentEdit.OnLeftClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cColor != null)
                        {
                            bb.SubTimerFill.Color = cColor(bb.SubTimerFill.Color);
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnRightClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cColor != null)
                        {
                            bb.SubTimerBorder.Color = cColor(bb.SubTimerBorder.Color);
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnMiddleClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cFont != null)
                        {
                            resetFont(ref bb.SubTimerFont, cFont(bb.SubTimerFont));
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnWheel = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        addFontSize(ref bb.SubTimerFont, delta);
                        IsForceRefreshAll = true;
                    };
                    OnEditCurrentChanged?.Invoke(CurrentEdit);
                }
            }
            else if (rcOutTimer.Contains(e.Location))
            {
                if (CurrentEdit == null || CurrentEdit.type != 10)
                {
                    CurrentEdit = new GBoardChanger() { type = 10, Name = "额外计时", Description = "一般用于显示关游戏时长" };
                    CurrentEdit.OnLeftClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cColor != null)
                        {
                            bb.OutTimerFill.Color = cColor(bb.OutTimerFill.Color);
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnRightClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cColor != null)
                        {
                            bb.OutTimerBorder.Color = cColor(bb.OutTimerBorder.Color);
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnMiddleClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cFont != null)
                        {
                            resetFont(ref bb.OutTimerFont, cFont(bb.OutTimerFont));
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnWheel = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        addFontSize(ref bb.OutTimerFont, delta);
                        IsForceRefreshAll = true;
                    };
                    OnEditCurrentChanged?.Invoke(CurrentEdit);
                }
            }
            else if (rcWillClear.Contains(e.Location))
            {
                if (CurrentEdit == null || CurrentEdit.type != 11)
                {
                    CurrentEdit = new GBoardChanger() { type = 11, Name = "预测显示", Description = "用于显示大致结束的时长" };
                    CurrentEdit.OnLeftClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cColor != null)
                        {
                            bb.WillClearFill.Color = cColor(bb.WillClearFill.Color);
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnRightClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cColor != null)
                        {
                            bb.WillClearBorder.Color = cColor(bb.WillClearBorder.Color);
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnMiddleClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cFont != null)
                        {
                            resetFont(ref bb.WillClearFont, cFont(bb.WillClearFont));
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnWheel = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        addFontSize(ref bb.WillClearFont, delta);
                        IsForceRefreshAll = true;
                    };
                    OnEditCurrentChanged?.Invoke(CurrentEdit);
                }
            }
            else if (rcDots.Contains(e.Location))
            {
                if (CurrentEdit == null || CurrentEdit.type != 12)
                {
                    CurrentEdit = new GBoardChanger() { type = 12, Name = "小统计", Description = "一般用于显示多个可追加的特殊统计信息" };
                    CurrentEdit.OnLeftClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cColor != null)
                        {
                            bb.DotTextFill.Color = cColor(bb.DotTextFill.Color);
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnMiddleClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        if (cFont != null)
                        {
                            resetFont(ref bb.DotTextFont, cFont(bb.DotTextFont));
                            IsForceRefreshAll = true;
                        }
                    };
                    CurrentEdit.OnWheel = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                    {
                        addFontSize(ref bb.DotTextFont, delta);
                        IsForceRefreshAll = true;
                    };
                    OnEditCurrentChanged?.Invoke(CurrentEdit);
                }
            }
            else if (rcItems.Contains(e.Location))
            {
                if (e.Y < (rcItems.Y + itemList.Count * GItem.Height))
                {
                    int yoff = e.Y - rcItems.Y;
                    GItem itm = null;
                    int idx = (int)Math.Floor(((double)yoff) / GItem.Height);
                    idx += ItemScroll;
                    if (idx >= 0 && idx < itemList.Count)
                    {
                        itm = itemList[idx];
                    }

                    if (e.X <= (rcIName.X + rcIName.Width))
                    {
                        //name
                        if (CurrentEdit == null || CurrentEdit.type != 13)
                        {
                            CurrentEdit = new GBoardChanger() { type = 13, Name = "节点名称", Description = "每个节点的名称" };
                            CurrentEdit.OnLeftClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                            {
                                if (cColor != null)
                                {
                                    bb.CPNameFill.Color = cColor(bb.CPNameFill.Color);
                                    IsForceRefreshAll = true;
                                }
                            };
                            CurrentEdit.OnRightClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                            {
                                if (cColor != null)
                                {
                                    bb.CPNameBorder.Color = cColor(bb.CPNameBorder.Color);
                                    IsForceRefreshAll = true;
                                }
                            };
                            CurrentEdit.OnMiddleClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                            {
                                if (cFont != null)
                                {
                                    resetFont(ref bb.CPNameFont, cFont(bb.CPNameFont));
                                    IsForceRefreshAll = true;
                                }
                            };
                            CurrentEdit.OnWheel = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                            {
                                addFontSize(ref bb.CPNameFont, delta);
                                IsForceRefreshAll = true;
                            };
                            OnEditCurrentChanged?.Invoke(CurrentEdit);
                        }
                    }
                    else if (e.X >= rcICur.X)
                    {
                        //cur
                        int type = 2000 + itm.MetaNum;
                        switch (itm.MetaNum)
                        {
                            case GItem.EditMetaSlow:
                                if (CurrentEdit == null || CurrentEdit.type != type)
                                {
                                    CurrentEdit = new GBoardChanger() { type = type, Name = "节点当前时间（慢）", Description = "慢的节点当前时间" };
                                    CurrentEdit.OnLeftClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                    {
                                        if (cColor != null)
                                        {
                                            bb.CPBadFill.Color = cColor(bb.CPBadFill.Color);
                                            IsForceRefreshAll = true;
                                        }
                                    };
                                    CurrentEdit.OnRightClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                    {
                                        if (cColor != null)
                                        {
                                            bb.CPBadBorder.Color = cColor(bb.CPBadBorder.Color);
                                            IsForceRefreshAll = true;
                                        }
                                    };
                                    CurrentEdit.OnMiddleClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                    {
                                        if (cFont != null)
                                        {
                                            resetFont(ref bb.CPCurFont, cFont(bb.CPCurFont));
                                            IsForceRefreshAll = true;
                                        }
                                    };
                                    CurrentEdit.OnWheel = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                    {
                                        addFontSize(ref bb.CPCurFont, delta);
                                        IsForceRefreshAll = true;
                                    };
                                    OnEditCurrentChanged?.Invoke(CurrentEdit);
                                }
                                break;
                            case GItem.EditMetaFast:
                                if (CurrentEdit == null || CurrentEdit.type != type)
                                {
                                    CurrentEdit = new GBoardChanger() { type = type, Name = "节点当前时间（快）", Description = "快的节点当前时间" };
                                    CurrentEdit.OnLeftClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                    {
                                        if (cColor != null)
                                        {
                                            bb.CPGoodFill.Color = cColor(bb.CPGoodFill.Color);
                                            IsForceRefreshAll = true;
                                        }
                                    };
                                    CurrentEdit.OnRightClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                    {
                                        if (cColor != null)
                                        {
                                            bb.CPGoodBorder.Color = cColor(bb.CPGoodBorder.Color);
                                            IsForceRefreshAll = true;
                                        }
                                    };
                                    CurrentEdit.OnMiddleClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                    {
                                        if (cFont != null)
                                        {
                                            resetFont(ref bb.CPCurFont, cFont(bb.CPCurFont));
                                            IsForceRefreshAll = true;
                                        }
                                    };
                                    CurrentEdit.OnWheel = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                    {
                                        addFontSize(ref bb.CPCurFont, delta);
                                        IsForceRefreshAll = true;
                                    };
                                    OnEditCurrentChanged?.Invoke(CurrentEdit);
                                }
                                break;
                            case GItem.EditMetaEqual:
                                if (CurrentEdit == null || CurrentEdit.type != type)
                                {
                                    CurrentEdit = new GBoardChanger() { type = type, Name = "节点当前时间（同）", Description = "同等的节点当前时间" };
                                    CurrentEdit.OnLeftClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                    {
                                        if (cColor != null)
                                        {
                                            bb.CPSameFill.Color = cColor(bb.CPSameFill.Color);
                                            IsForceRefreshAll = true;
                                        }
                                    };
                                    CurrentEdit.OnRightClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                    {
                                        if (cColor != null)
                                        {
                                            bb.CPSameBorder.Color = cColor(bb.CPSameBorder.Color);
                                            IsForceRefreshAll = true;
                                        }
                                    };
                                    CurrentEdit.OnMiddleClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                    {
                                        if (cFont != null)
                                        {
                                            resetFont(ref bb.CPCurFont, cFont(bb.CPCurFont));
                                            IsForceRefreshAll = true;
                                        }
                                    };
                                    CurrentEdit.OnWheel = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                    {
                                        addFontSize(ref bb.CPCurFont, delta);
                                        IsForceRefreshAll = true;
                                    };
                                    OnEditCurrentChanged?.Invoke(CurrentEdit);
                                }
                                break;
                            case GItem.EditMetaActive:
                                if (CurrentEdit == null || CurrentEdit.type != type)
                                {
                                    CurrentEdit = new GBoardChanger() { type = type, Name = "节点当前时间", Description = "当前时间" };
                                    CurrentEdit.OnMiddleClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                    {
                                        if (cFont != null)
                                        {
                                            resetFont(ref bb.CPCurFont, cFont(bb.CPCurFont));
                                            IsForceRefreshAll = true;
                                        }
                                    };
                                    CurrentEdit.OnWheel = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                    {
                                        addFontSize(ref bb.CPCurFont, delta);
                                        IsForceRefreshAll = true;
                                    };
                                    OnEditCurrentChanged?.Invoke(CurrentEdit);
                                }
                                break;
                        }
                    }
                    else
                    {
                        //best || cha
                        while (yoff >= GItem.Height)
                        {
                            yoff -= GItem.Height;
                        }
                        if (yoff < GItem.HalfHeight)
                        {
                            //best
                            if (CurrentEdit == null || CurrentEdit.type != 14)
                            {
                                CurrentEdit = new GBoardChanger() { type = 14, Name = "节点最佳", Description = "每个节点的最佳" };
                                CurrentEdit.OnLeftClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                {
                                    if (cColor != null)
                                    {
                                        bb.CPBestFill.Color = cColor(bb.CPBestFill.Color);
                                        IsForceRefreshAll = true;
                                    }
                                };
                                CurrentEdit.OnRightClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                {
                                    if (cColor != null)
                                    {
                                        bb.CPBestBorder.Color = cColor(bb.CPBestBorder.Color);
                                        IsForceRefreshAll = true;
                                    }
                                };
                                CurrentEdit.OnMiddleClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                {
                                    if (cFont != null)
                                    {
                                        resetFont(ref bb.CPBestFont, cFont(bb.CPBestFont));
                                        IsForceRefreshAll = true;
                                    }
                                };
                                CurrentEdit.OnWheel = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                {
                                    addFontSize(ref bb.CPBestFont, delta);
                                    IsForceRefreshAll = true;
                                };
                                OnEditCurrentChanged?.Invoke(CurrentEdit);
                            }
                        }
                        else
                        {
                            //cha
                            int type = 3000 + itm.MetaNum;
                            switch (itm.MetaNum)
                            {
                                case GItem.EditMetaSlow:
                                    if (CurrentEdit == null || CurrentEdit.type != type)
                                    {
                                        CurrentEdit = new GBoardChanger() { type = type, Name = "节点时间差值（慢）", Description = "慢的节点时间差值" };
                                        CurrentEdit.OnLeftClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                        {
                                            if (cColor != null)
                                            {
                                                bb.CPBadFill.Color = cColor(bb.CPBadFill.Color);
                                                IsForceRefreshAll = true;
                                            }
                                        };
                                        CurrentEdit.OnRightClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                        {
                                            if (cColor != null)
                                            {
                                                bb.CPBadBorder.Color = cColor(bb.CPBadBorder.Color);
                                                IsForceRefreshAll = true;
                                            }
                                        };
                                        CurrentEdit.OnMiddleClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                        {
                                            if (cFont != null)
                                            {
                                                resetFont(ref bb.CPChaFont, cFont(bb.CPChaFont));
                                                IsForceRefreshAll = true;
                                            }
                                        };
                                        CurrentEdit.OnWheel = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                        {
                                            addFontSize(ref bb.CPChaFont, delta);
                                            IsForceRefreshAll = true;
                                        };
                                        OnEditCurrentChanged?.Invoke(CurrentEdit);
                                    }
                                    break;
                                case GItem.EditMetaFast:
                                    if (CurrentEdit == null || CurrentEdit.type != type)
                                    {
                                        CurrentEdit = new GBoardChanger() { type = type, Name = "节点时间差值（快）", Description = "快的节点时间差值" };
                                        CurrentEdit.OnLeftClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                        {
                                            if (cColor != null)
                                            {
                                                bb.CPGoodFill.Color = cColor(bb.CPGoodFill.Color);
                                                IsForceRefreshAll = true;
                                            }
                                        };
                                        CurrentEdit.OnRightClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                        {
                                            if (cColor != null)
                                            {
                                                bb.CPGoodBorder.Color = cColor(bb.CPGoodBorder.Color);
                                                IsForceRefreshAll = true;
                                            }
                                        };
                                        CurrentEdit.OnMiddleClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                        {
                                            if (cFont != null)
                                            {
                                                resetFont(ref bb.CPChaFont, cFont(bb.CPChaFont));
                                                IsForceRefreshAll = true;
                                            }
                                        };
                                        CurrentEdit.OnWheel = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                        {
                                            addFontSize(ref bb.CPChaFont, delta);
                                            IsForceRefreshAll = true;
                                        };
                                        OnEditCurrentChanged?.Invoke(CurrentEdit);
                                    }
                                    break;
                                case GItem.EditMetaEqual:
                                    if (CurrentEdit == null || CurrentEdit.type != type)
                                    {
                                        CurrentEdit = new GBoardChanger() { type = type, Name = "节点时间差值（同）", Description = "同等的节点时间差值" };
                                        CurrentEdit.OnLeftClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                        {
                                            if (cColor != null)
                                            {
                                                bb.CPSameFill.Color = cColor(bb.CPSameFill.Color);
                                                IsForceRefreshAll = true;
                                            }
                                        };
                                        CurrentEdit.OnRightClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                        {
                                            if (cColor != null)
                                            {
                                                bb.CPSameBorder.Color = cColor(bb.CPSameBorder.Color);
                                                IsForceRefreshAll = true;
                                            }
                                        };
                                        CurrentEdit.OnMiddleClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                        {
                                            if (cFont != null)
                                            {
                                                resetFont(ref bb.CPChaFont, cFont(bb.CPChaFont));
                                                IsForceRefreshAll = true;
                                            }
                                        };
                                        CurrentEdit.OnWheel = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                        {
                                            addFontSize(ref bb.CPChaFont, delta);
                                            IsForceRefreshAll = true;
                                        };
                                        OnEditCurrentChanged?.Invoke(CurrentEdit);
                                    }
                                    break;
                                case GItem.EditMetaActive:
                                    if (CurrentEdit == null || CurrentEdit.type != type)
                                    {
                                        CurrentEdit = new GBoardChanger() { type = type, Name = "节点时间差值", Description = "时间差值" };
                                        CurrentEdit.OnMiddleClick = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                        {
                                            if (cFont != null)
                                            {
                                                resetFont(ref bb.CPChaFont, cFont(bb.CPChaFont));
                                                IsForceRefreshAll = true;
                                            }
                                        };
                                        CurrentEdit.OnWheel = delegate (int delta, GBoardChanger.delGBoardChangerGetColor cColor, GBoardChanger.delGBoardChangerGetFont cFont)
                                        {
                                            addFontSize(ref bb.CPChaFont, delta);
                                            IsForceRefreshAll = true;
                                        };
                                        OnEditCurrentChanged?.Invoke(CurrentEdit);
                                    }
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    CurrentEdit = null;
                    OnEditCurrentChanged?.Invoke(CurrentEdit);
                }
            }
            else
            {
                CurrentEdit = null;
                OnEditCurrentChanged?.Invoke(CurrentEdit);
            }
        }

        private int Width = 0;
        private int Height = 0;
        private bool isSizeChanged = true;
        private Rectangle close_rc = new Rectangle();
        private Rectangle btn_rc = new Rectangle();
        private Rectangle config_rc = new Rectangle();
        private bool isCloseCtlChanged = false;
        private bool isBtnCtlChanged = false;
        private bool isConfigCtlChanged = false;
        private Control close_ctl = null;
        private Control btn_ctl = null;
        private Control config_ctl = null;
        private void onFuncAreaClicked(object sender, MouseEventArgs e)
        {
            OnFunctionAreaClicked(e.Location);
        }
        private void onBaseMouseWheel(object sender, MouseEventArgs e)
        {
            OnMouseWheel(sender, e);
        }
        private void onBaseMouseDBClick(object sender, MouseEventArgs e)
        {
            OnDBClicked(e.Location);
        }
        public delegate void delUpdateRect(Rectangle? rect);
        public bool Draw(delUpdateRect ur = null)
        {
            if (BaseCtl == null) return false;
            if (IsForceRefreshAll || IsForceRefreshAllMode)
            {
                isBGChanged = true;
                RefreshDotsStyle();
                IsForceRefreshAll = false;
            }
            if (BaseCtl.Width != Width)
            {
                Width = BaseCtl.Width;
                isSizeChanged = true;
            }
            if (BaseCtl.Height != Height)
            {
                Height = BaseCtl.Height;
                isSizeChanged = true;
            }
            if (close_ctl != null)
            {
                if (close_ctl.Left != close_rc.X || close_ctl.Top != close_rc.Y || close_ctl.Width != close_rc.Width || close_ctl.Height != close_rc.Height)
                {
                    close_rc.X = close_ctl.Left;
                    close_rc.Y = close_ctl.Top;
                    close_rc.Width = close_ctl.Width;
                    close_rc.Height = close_ctl.Height;
                    isCloseCtlChanged = true;
                }
            }

            if (btn_ctl != null)
            {
                if (btn_ctl.Left != btn_rc.X || btn_ctl.Top != btn_rc.Y || btn_ctl.Width != btn_rc.Width || btn_ctl.Height != btn_rc.Height)
                {
                    btn_rc.X = btn_ctl.Left;
                    btn_rc.Y = btn_ctl.Top;
                    btn_rc.Width = btn_ctl.Width;
                    btn_rc.Height = btn_ctl.Height;
                    isBtnCtlChanged = true;
                }
            }

            if (config_ctl != null)
            {
                if (config_ctl.Left != config_rc.X || config_ctl.Top != config_rc.Y || config_ctl.Width != config_rc.Width || config_ctl.Height != config_rc.Height)
                {
                    config_rc.X = config_ctl.Left;
                    config_rc.Y = config_ctl.Top;
                    config_rc.Width = config_ctl.Width;
                    config_rc.Height = config_ctl.Height;
                    isConfigCtlChanged = true;
                }
            }

            if (isSizeChanged)
            {
                Image tmpi = CI;
                Graphics tmpg = CG;
                CI = new Bitmap(BaseCtl.Width, BaseCtl.Height);
                CG = Graphics.FromImage(CI);
                BaseCtl.BackgroundImage = CI;
                tmpg?.Dispose();
                tmpi?.Dispose();
                BuildRects();
            }
            if (CG == null) return false;
            if (isSizeChanged || isBGChanged)
            {
                if (bg != null)
                {
                    CG.DrawImage(bg, 0, 0, Width, Height);
                }
                else
                {
                    CG.Clear(Color.Transparent);
                }
            }
            DrawClose(CG, ur);
            DrawConfig(CG, ur);
            bool btnc = DrawButtons(CG, ur);
            DrawTitle(CG, ur);
            DrawGameVersion(CG, ur);
            DrawVersion(CG, ur);
            DrawBL(CG, ur);
            DrawBR(CG, ur);
            DrawMoreInfo(CG, ur);
            DrawIsC(CG, ur);
            DrawSubTimer(CG, ur);
            DrawOutTimer(CG, ur);
            DrawWillClear(CG, ur);
            DrawDots(CG, ur);
            bool itemc = DrawCheckPoints(CG, ur);
            DrawMainTimer(CG, ur);
            DrawMainTimerMS(CG, ur);
            if (isSizeChanged || isBGChanged || isBBChanged)
            {
                ur?.Invoke(null);
            }
            bool afterc = AfterDraw();
            return (btnc || itemc || afterc);
        }
        private bool AfterDraw()
        {
            bool ret = ((isBBChanged) ||
                (isBGChanged) ||
                (isBLChanged) ||
                (isBRChanged) ||
                (isBtnCtlChanged) ||
                (isBtnsChanged) ||
                (isCloseCtlChanged) ||
                (isDotsChanged) ||
                (isDotScroll) ||
                (isGameVersionChanged) ||
                (isItemsChanged) ||
                (isItemScroll) ||
                (isMainTimerChanged) ||
                (isMainTimerMSChanged) ||
                (isMoreInfoChanged) ||
                (isOutTimerChanged) ||
                (isSizeChanged) ||
                (isSubTimerChanged) ||
                (isTitleChanged) ||
                (isVersionChanged) ||
                (isWillClearChanged) ||
                (isConfigCtlChanged) ||
                (isCChanged)
                );

            isBBChanged = false;
            isBGChanged = false;
            isBLChanged = false;
            isBRChanged = false;
            isBtnCtlChanged = false;
            isBtnsChanged = false;
            isCloseCtlChanged = false;
            isDotsChanged = false;
            isDotScroll = false;
            isGameVersionChanged = false;
            isItemsChanged = false;
            isItemScroll = false;
            isMainTimerChanged = false;
            isMainTimerMSChanged = false;
            isMoreInfoChanged = false;
            isOutTimerChanged = false;
            isSizeChanged = false;
            isSubTimerChanged = false;
            isTitleChanged = false;
            isVersionChanged = false;
            isWillClearChanged = false;
            isConfigCtlChanged = false;
            isCChanged = false;

            return ret;
        }
        private void DrawClose(Graphics g, delUpdateRect ur = null)
        {
            if (close_ctl == null) return;
            if (isSizeChanged || isBGChanged || isBBChanged || isCloseCtlChanged)
            {
                if (!isSizeChanged && !isBGChanged)
                {
                    GEX.ClearRect(g, close_rc, bg, Width, Height);
                }
                var mode = g.SmoothingMode;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.FillEllipse(bb.CloseFill, close_rc);
                g.DrawLine(bb.CloseLine, close_rc.X + 10, close_rc.Y + 10, close_rc.X + close_rc.Width - 10, close_rc.Y + close_rc.Height - 10);
                g.DrawLine(bb.CloseLine, close_rc.X + close_rc.Width - 10, close_rc.Y + 10, close_rc.X + 10, close_rc.Y + close_rc.Height - 10);
                g.SmoothingMode = mode;
                if (!isSizeChanged && !isBGChanged && !isBBChanged)
                {
                    ur?.Invoke(close_rc);
                }
            }
        }
        private void DrawConfig(Graphics g, delUpdateRect ur = null)
        {
            if (config_ctl == null) return;
            if (isSizeChanged || isBGChanged || isBBChanged || isConfigCtlChanged)
            {
                if (!isSizeChanged && !isBGChanged)
                {
                    GEX.ClearRect(g, config_rc, bg, Width, Height);
                }
                if (configIcon != null)
                {
                    g.DrawImage(configIcon, config_rc);
                }
                if (!isSizeChanged && !isBGChanged && !isBBChanged)
                {
                    ur?.Invoke(config_rc);
                }
            }
        }
        private void DrawTitle(Graphics g, delUpdateRect ur = null)
        {
            //自动计时器
            if (isSizeChanged || isBGChanged || isBBChanged || isTitleChanged)
            {
                if (!isSizeChanged && !isBGChanged)
                {
                    GEX.ClearRect(g, rcTitle, bg, Width, Height);
                }
                GEX.DrawText(g, Title, bb.TitleFont, bb.TitleFill, bb.TitleBorder, rcTitle, GLayout.sfNC);
                if (!isSizeChanged && !isBGChanged && !isBBChanged)
                {
                    ur?.Invoke(rcTitle);
                }
            }
        }
        private void DrawGameVersion(Graphics g, delUpdateRect ur = null)
        {
            //等待游戏运行
            if (isSizeChanged || isBGChanged || isBBChanged || isGameVersionChanged)
            {
                if (!isSizeChanged && !isBGChanged)
                {
                    GEX.ClearRect(g, rcGameVersion, bg, Width, Height);
                }
                GEX.DrawText(g, GameVersion, bb.GVersionFont, bb.GVersionFill, bb.GVersionBorder, rcGameVersion, GLayout.sfNC);
                if (!isSizeChanged && !isBGChanged && !isBBChanged)
                {
                    ur?.Invoke(rcGameVersion);
                }
            }
        }
        private void DrawVersion(Graphics g, delUpdateRect ur = null)
        {
            //v2.34
            if (isSizeChanged || isBGChanged || isBBChanged || isVersionChanged)
            {
                if (!isSizeChanged && !isBGChanged)
                {
                    GEX.ClearRect(g, rcVersion, bg, Width, Height);
                }
                GEX.DrawText(g, Version, bb.VersionFont, bb.VersionFill, bb.VersionBorder, rcVersion, GLayout.sfFC);
                if (!isSizeChanged && !isBGChanged && !isBBChanged)
                {
                    ur?.Invoke(rcVersion);
                }
            }
        }
        private void DrawBL(Graphics g, delUpdateRect ur = null)
        {
            //脚底抹油
            if (isSizeChanged || isBGChanged || isBBChanged || isBLChanged)
            {
                if (!isSizeChanged && !isBGChanged)
                {
                    GEX.ClearRect(g, rcBL, bg, Width, Height);
                }
                GEX.DrawText(g, BL, bb.BLFont, bb.BLFill, bb.BLBorder, rcBL, GLayout.sfNC);
                if (!isSizeChanged && !isBGChanged && !isBBChanged)
                {
                    ur?.Invoke(rcBL);
                }
            }
        }
        private void DrawBR(Graphics g, delUpdateRect ur = null)
        {
            //毒门和爆炸门都惹不起啊
            if (isSizeChanged || isBGChanged || isBBChanged || isBRChanged)
            {
                if (!isSizeChanged && !isBGChanged)
                {
                    GEX.ClearRect(g, rcBR, bg, Width, Height);
                }
                GEX.DrawText(g, BR, bb.BRFont, bb.BRFill, bb.BRBorder, rcBR, GLayout.sfFC);
                if (!isSizeChanged && !isBGChanged && !isBBChanged)
                {
                    ur?.Invoke(rcBR);
                }
            }
        }
        private void DrawMoreInfo(Graphics g, delUpdateRect ur = null)
        {
            //蜂0 蜜0 火0 血0 夜0 剑0
            if (isSizeChanged || isBGChanged || isBBChanged || isMoreInfoChanged)
            {
                if (!isSizeChanged && !isBGChanged)
                {
                    GEX.ClearRect(g, rcMoreInfo, bg, Width, Height);
                }
                GEX.DrawText(g, MoreInfo, bb.MoreInfoFont, bb.MoreInfoFill, bb.MoreInfoBorder, rcMoreInfo, GLayout.sfCC);
                if (!isSizeChanged && !isBGChanged && !isBBChanged)
                {
                    ur?.Invoke(rcMoreInfo);
                }
            }
        }
        private void DrawMainTimer(Graphics g, delUpdateRect ur = null)
        {
            //Anti Cheat
            if (isSizeChanged || isBGChanged || isBBChanged || isMainTimerChanged)
            {
                if (!isSizeChanged && !isBGChanged)
                {
                    GEX.ClearRect(g, rcMainTimer, bg, Width, Height);
                }
                if (isInCheck)
                {
                    GEX.DrawText(g, "*" + TS2HHMMSS(MainTimer), bb.MainTimerFont, bb.MainTimerFill, bb.MainTimerBorder, rcMainTimer, GLayout.sfFC);
                }
                else
                {
                    GEX.DrawText(g, TS2HHMMSS(MainTimer), bb.MainTimerFont, bb.MainTimerFill, bb.MainTimerBorder, rcMainTimer, GLayout.sfFC);
                }
                if (!isSizeChanged && !isBGChanged && !isBBChanged)
                {
                    ur?.Invoke(rcMainTimer);
                }
            }
        }
        private void DrawMainTimerMS(Graphics g, delUpdateRect ur = null)
        {
            if (isSizeChanged || isBGChanged || isBBChanged || isMainTimerMSChanged)
            {
                if (!isSizeChanged && !isBGChanged)
                {
                    GEX.ClearRect(g, rcMainTimerMS, bg, Width, Height);
                }
                GEX.DrawText(g, MainTimer.Milliseconds.ToString().PadLeft(3, '0').Substring(0, 2), bb.MainTimerMSFont, bb.MainTimerFill, bb.MainTimerBorder, rcMainTimerMS, GLayout.sfFN);
                if (!isSizeChanged && !isBGChanged && !isBBChanged)
                {
                    ur?.Invoke(rcMainTimerMS);
                }
            }
        }
        private void DrawSubTimer(Graphics g, delUpdateRect ur = null)
        {
            //0.00s
            if (isSizeChanged || isBGChanged || isBBChanged || isSubTimerChanged)
            {
                if (!isSizeChanged && !isBGChanged)
                {
                    GEX.ClearRect(g, rcSubTimer, bg, Width, Height);
                }
                GEX.DrawText(g, SubTimer, bb.SubTimerFont, bb.SubTimerFill, bb.SubTimerBorder, rcSubTimer, GLayout.sfNC);
                if (!isSizeChanged && !isBGChanged && !isBBChanged)
                {
                    ur?.Invoke(rcSubTimer);
                }
            }
        }
        private void DrawOutTimer(Graphics g, delUpdateRect ur = null)
        {
            //+ 0:00:00.00
            if (isSizeChanged || isBGChanged || isBBChanged || isOutTimerChanged)
            {
                if (!isSizeChanged && !isBGChanged)
                {
                    GEX.ClearRect(g, rcOutTimer, bg, Width, Height);
                }
                GEX.DrawText(g, OutTimer, bb.OutTimerFont, bb.OutTimerFill, bb.OutTimerBorder, rcOutTimer, GLayout.sfFC);
                if (!isSizeChanged && !isBGChanged && !isBBChanged)
                {
                    ur?.Invoke(rcOutTimer);
                }
            }
        }
        private void DrawWillClear(Graphics g, delUpdateRect ur = null)
        {
            //预计通关  03:20:01
            if (isSizeChanged || isBGChanged || isBBChanged || isWillClearChanged)
            {
                if (!isSizeChanged && !isBGChanged)
                {
                    GEX.ClearRect(g, rcWillClear, bg, Width, Height);
                }
                GEX.DrawText(g, WillClear, bb.WillClearFont, bb.WillClearFill, bb.WillClearBorder, rcWillClear, GLayout.sfCC);
                if (!isSizeChanged && !isBGChanged && !isBBChanged)
                {
                    ur?.Invoke(rcWillClear);
                }
            }
        }
        private bool DrawButtons(Graphics g, delUpdateRect ur = null)
        {
            if (btn_ctl == null) return false;
            bool isForceDrawAll = isBtnsChanged;
            if (isSizeChanged || isBGChanged || isBBChanged || isBtnCtlChanged)
            {
                if (!isSizeChanged && !isBGChanged)
                {
                    GEX.ClearRect(g, btn_rc, bg, Width, Height);
                }
                isForceDrawAll = true;
            }
            bool ret = false;
            Rectangle rc = new Rectangle();
            int p = 0;
            for (int i = 0; i < btnList.Count; ++i)
            {
                GBtn cur = btnList[i];
                if (!cur.IsVisible) continue;
                if (isForceDrawAll || cur.IsDirty)
                {
                    ret = true;
                    ModifyRect(ref rc, btn_rc.X + p * (GBtn.Margin + GBtn.Width), btn_rc.Y, GBtn.Width, btn_rc.Height);
                    GEX.ClearRect(g, rc, bg, Width, Height);
                    cur.Draw(g, rc, bb);
                    if (!isForceDrawAll)
                    {
                        ur?.Invoke(rc);
                    }
                }
                p++;
            }
            if (isForceDrawAll)
            {
                ur?.Invoke(btn_rc);
            }
            return ret;
        }
        private void DrawDots(Graphics g, delUpdateRect ur = null)
        {
            if (rcDots.Height <= 0) return;
            if (isSizeChanged || isBGChanged || isBBChanged || isDotsChanged || isDotScroll)
            {
                if (!isSizeChanged && !isBGChanged)
                {
                    GEX.ClearRect(g, rcDots, bg, Width, Height);
                }
                if (LastDots != null)
                {
                    if (LastDots.Width <= rcDots.Width)
                    {
                        DotScroll = 0;
                        g.DrawImage(LastDots, rcDots.X, rcDots.Y);
                    }
                    else
                    {
                        if (DotScroll + rcDots.Width > LastDots.Width)
                        {
                            DotScroll = LastDots.Width - rcDots.Width;
                        }
                        if (DotScroll < 0) DotScroll = 0;
                        g.DrawImage(LastDots, rcDots, new Rectangle(DotScroll, 0, rcDots.Width, rcDots.Height), GraphicsUnit.Pixel);
                    }
                }
                if (!isSizeChanged && !isBGChanged && !isBBChanged)
                {
                    ur?.Invoke(rcDots);
                }
            }
        }
        private bool DrawCheckPoints(Graphics g, delUpdateRect ur = null)
        {
            if (rcItems.Height <= 0) return false;
            int CanShowCount = (int)Math.Floor(((double)(rcItems.Height)) / GItem.Height);
            if (CanShowCount < 1) CanShowCount = 1;
            if (CanShowCount >= itemList.Count)
            {
                ItemScroll = 0;
                //hide scroll
                if (rcItemScroll.Width > 0)
                {
                    BuildRects_Item(false);
                }
            }
            else
            {
                if ((itemList.Count - ItemScroll) < CanShowCount)
                {
                    ItemScroll = itemList.Count - CanShowCount;
                }
                if (ItemScroll < 0) ItemScroll = 0;
                //draw scroll
                BuildRects_Item(true);
                if (isItemScroll || isSizeChanged)
                {
                    int barstep = (int)(((double)1 / itemList.Count) * rcItemScroll.Height);
                    ModifyRect(ref rcItemScBlock, rcItemScroll.X, rcItemScroll.Y + ItemScroll * barstep, rcItemScroll.Width, barstep * CanShowCount);
                }
            }

            bool isForceDrawAll = false;
            if (isSizeChanged || isBGChanged || isBBChanged || isItemsChanged || isItemScroll)
            {
                if (!isSizeChanged && !isBGChanged)
                {
                    GEX.ClearRect(g, rcItems, bg, Width, Height);
                }
                if (!isSizeChanged && !isBGChanged && isItemScroll && rcItemScroll.Width > 0)
                {
                    GEX.ClearRect(g, rcItemScroll, bg, Width, Height);
                }
                isForceDrawAll = true;
            }
            if (isForceDrawAll)
            {
                g.FillRectangle(bb.CPItemBG, rcItemScroll);
                g.FillRectangle(bb.CPItemActBG, rcItemScBlock);
            }
            bool ret = false;
            for (int i = ItemScroll, j = 0; i < itemList.Count && j < CanShowCount; ++i, ++j)
            {
                int y = rcItems.Y + j * GItem.Height;
                rcIName.Y = y;
                rcIBest.Y = y;
                rcICha.Y = y + GItem.HalfHeight;
                rcICur.Y = y;
                if (itemList[i].Draw(g, isForceDrawAll, bb, rcIName, rcIBest, rcICha, rcICur, bg, Width, Height, ur))
                {
                    ret = true;
                }
            }
            if (isForceDrawAll)
            {
                ur?.Invoke(rcItems);
                if (rcItemScroll.Width > 0)
                {
                    ur?.Invoke(rcItemScroll);
                }
            }
            return ret;
        }
        private void DrawIsC(Graphics g, delUpdateRect ur = null)
        {
            if (isSizeChanged || isBGChanged || isBBChanged || isCChanged)
            {
                if (!isSizeChanged && !isBGChanged)
                {
                    GEX.ClearRect(g, rcIsC, bg, Width, Height);
                }
                if (isC)
                {
                    g.FillRectangle(bb.IsCFill, rcIsC);
                    g.DrawRectangle(bb.IsCBorder, rcIsC);
                    GEX.DrawText(g, "C", bb.IsCFont, bb.IsCTextFill, null, rcIsC, GLayout.sfCC);
                }
                if (!isSizeChanged && !isBGChanged && !isBBChanged)
                {
                    ur?.Invoke(rcIsC);
                }
            }
        }
    }

    public static class GLayout
    {
        public static StringFormat sfNN = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
        public static StringFormat sfCN = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near };
        public static StringFormat sfFN = new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near };
        public static StringFormat sfNC = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
        public static StringFormat sfCC = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        public static StringFormat sfFC = new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };
        public static StringFormat sfNF = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Far };
        public static StringFormat sfCF = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far };
        public static StringFormat sfFF = new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far };
    }

    public class GBoardChanger
    {
        public delegate Color delGBoardChangerGetColor(Color ori);
        public delegate Font delGBoardChangerGetFont(Font ori);
        public delegate void delGBoardChangerCall(int delta = 0, delGBoardChangerGetColor cbGetColor = null, delGBoardChangerGetFont cbGetFont = null);
        public int type;
        public string Name;
        public string Description;
        public delGBoardChangerCall OnLeftClick = null;
        public delGBoardChangerCall OnRightClick = null;
        public delGBoardChangerCall OnMiddleClick = null;
        public delGBoardChangerCall OnWheel = null;
    }

    public class GBoard : IDisposable
    {
        public Font DotTextFont;
        public SolidBrush DotTextFill;
        public Pen DotTextBorder;
        public Pen DotBorder;
        public SolidBrush[] DotFills;
        public SolidBrush CloseFill;
        public Pen CloseLine;
        public Font TitleFont;
        public SolidBrush TitleFill;
        public Pen TitleBorder;
        public Font GVersionFont;
        public SolidBrush GVersionFill;
        public Pen GVersionBorder;
        public Font VersionFont;
        public SolidBrush VersionFill;
        public Pen VersionBorder;
        public Font BLFont;
        public SolidBrush BLFill;
        public Pen BLBorder;
        public Font BRFont;
        public SolidBrush BRFill;
        public Pen BRBorder;
        public Font MoreInfoFont;
        public SolidBrush MoreInfoFill;
        public Pen MoreInfoBorder;
        public Font MainTimerFont;
        public SolidBrush MainTimerFill;
        public Pen MainTimerBorder;
        public Font MainTimerMSFont;
        public Font SubTimerFont;
        public SolidBrush SubTimerFill;
        public Pen SubTimerBorder;
        public Font OutTimerFont;
        public SolidBrush OutTimerFill;
        public Pen OutTimerBorder;
        public Font WillClearFont;
        public SolidBrush WillClearFill;
        public Pen WillClearBorder;
        public SolidBrush ButtonFill;
        public Pen ButtonBorder;
        public Font ButtonTextFont;
        public SolidBrush ButtonTextFill;
        public SolidBrush ButtonTextFillRed;
        public SolidBrush ButtonTextFillOrange;
        public Pen ButtonTextBorder;
        public Font CPNameFont;
        public SolidBrush CPNameFill;
        public Pen CPNameBorder;
        public Font CPBestFont;
        public SolidBrush CPBestFill;
        public Pen CPBestBorder;
        public Font CPChaFont;
        public Font CPCurFont;
        public SolidBrush CPGoodFill;
        public Pen CPGoodBorder;
        public SolidBrush CPBadFill;
        public Pen CPBadBorder;
        public SolidBrush CPSameFill;
        public Pen CPSameBorder;
        public SolidBrush CPItemBG;
        public SolidBrush CPItemActBG;
        public SolidBrush IsCFill;
        public Pen IsCBorder;
        public SolidBrush IsCTextFill;
        public Font IsCFont;

        public GBoard()
        {
            BuildDefault();
        }

        public GBoard(GBoard b)
        {
            CloneFrom(b);
        }
        private void CloneFrom(GBoard b)
        {
            foreach (System.Reflection.FieldInfo p in this.GetType().GetFields())
            {
                object val = p.GetValue(b);
                if (val == null) continue;
                if (p.IsPublic)
                {
                    if (p.FieldType == typeof(Font))
                    {
                        p.SetValue(this, (val as Font).Clone());
                    }
                    else if (p.FieldType == typeof(Pen))
                    {
                        p.SetValue(this, (val as Pen).Clone());
                    }
                    else if (p.FieldType == typeof(SolidBrush))
                    {
                        p.SetValue(this, (val as SolidBrush).Clone());
                    }
                    else if (p.FieldType == typeof(SolidBrush[]))
                    {
                        p.SetValue(this, (val as SolidBrush[]).Clone());
                    }
                }
            }
        }
        private void LoadFromString(string str)
        {
            if (string.IsNullOrEmpty(str)) return;
            string[] split1 = str.Trim().Replace("\r\n", "\n").Split('\n');
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (string line in split1)
            {
                string l = line.Trim();
                if (string.IsNullOrEmpty(l) || !l.Contains(":")) continue;
                string[] split2 = l.Split(':');
                if (!dic.ContainsKey(split2[0]))
                {
                    dic.Add(split2[0], split2[1]);
                }
            }
            foreach (System.Reflection.FieldInfo i in this.GetType().GetFields())
            {
                if (i.IsPublic)
                {
                    if (!dic.ContainsKey(i.Name)) continue;
                    if (i.FieldType == typeof(Font))
                    {
                        Font f = StringToFont(dic[i.Name]);
                        if (f != null)
                        {
                            i.SetValue(this, f);
                        }
                    }
                    else if (i.FieldType == typeof(Pen))
                    {
                        Pen p = StringToPen(dic[i.Name]);
                        if (p != null)
                        {
                            i.SetValue(this, p);
                        }
                    }
                    else if (i.FieldType == typeof(SolidBrush))
                    {
                        SolidBrush b = StringToSolidBrush(dic[i.Name]);
                        if (b != null)
                        {
                            i.SetValue(this, b);
                        }
                    }
                    else if (i.FieldType == typeof(SolidBrush[]))
                    {
                        SolidBrush[] ba = StringToSolidBrushArr(dic[i.Name]);
                        if (ba != null)
                        {
                            i.SetValue(this, ba);
                        }
                    }
                }
            }
            GC.Collect();
        }
        public const string Path = "GDisplay.cnf";
        public void Save()
        {
            if (File.Exists(Path))
            {
                File.Delete(Path);
            }
            using (FileStream fs = new FileStream(Path, FileMode.Create, FileAccess.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(ToString());
                }
            }
        }
        public void Load()
        {
            if (File.Exists(Path))
            {
                using (FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                    {
                        try
                        {
                            LoadFromString(sr.ReadToEnd());
                        }
                        catch { }
                    }
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (System.Reflection.FieldInfo p in this.GetType().GetFields())
            {
                if (p.IsPublic)
                {
                    string name = p.Name + ":";
                    if (p.FieldType == typeof(Font))
                    {
                        name += FontToString(p.GetValue(this) as Font);
                    }
                    else if (p.FieldType == typeof(Pen))
                    {
                        name += PenToString(p.GetValue(this) as Pen);
                    }
                    else if (p.FieldType == typeof(SolidBrush))
                    {
                        name += SolidBrushToString(p.GetValue(this) as SolidBrush);
                    }
                    else if (p.FieldType == typeof(SolidBrush[]))
                    {
                        name += SolidBrushArrToString(p.GetValue(this) as SolidBrush[]);
                    }
                    sb.AppendLine(name);
                }
            }
            return sb.ToString();
        }

        public static string PenToString(Pen p)
        {
            if (p == null) return "NULL";
            return p.Width.ToString() + " " + p.Color.ToArgb().ToString();
        }
        public static string SolidBrushToString(SolidBrush b)
        {
            if (b == null) return "NULL";
            return b.Color.ToArgb().ToString();
        }
        public static string SolidBrushArrToString(SolidBrush[] arr)
        {
            if (arr == null) return "NULL";
            string a = "";
            foreach (SolidBrush b in arr)
            {
                a += SolidBrushToString(b) + ",";
            }
            if (a != "") a = a.Substring(0, a.Length - 1);
            return a;
        }
        public static string FontToString(Font f)
        {
            if (f == null) return "NULL";
            return f.FontFamily.Name + " " + f.Size.ToString() + " " + ((int)f.Style).ToString();
        }
        public static Pen StringToPen(string str)
        {
            if (string.IsNullOrEmpty(str) || str.Trim().ToUpper() == "NULL")
            {
                return null;
            }
            try
            {
                string[] spli = str.Trim().Split(' ');
                return new Pen(Color.FromArgb(int.Parse(spli[1])), float.Parse(spli[0]));
            }
            catch { }
            return null;
        }
        public static SolidBrush StringToSolidBrush(string str)
        {
            if (string.IsNullOrEmpty(str) || str.Trim().ToUpper() == "NULL")
            {
                return null;
            }
            try
            {
                return new SolidBrush(Color.FromArgb(int.Parse(str.Trim())));
            }
            catch { }
            return null;
        }
        public static SolidBrush[] StringToSolidBrushArr(string str)
        {
            if (string.IsNullOrEmpty(str) || str.Trim().ToUpper() == "NULL")
            {
                return null;
            }
            try
            {
                string[] spli = str.Trim().Split(',');
                List<SolidBrush> lst = new List<SolidBrush>();
                foreach (string s in spli)
                {
                    SolidBrush r = StringToSolidBrush(s);
                    if (r != null)
                    {
                        lst.Add(r);
                    }
                }
                return lst.ToArray();
            }
            catch { }
            return null;
        }
        public static Font StringToFont(string str)
        {
            if (string.IsNullOrEmpty(str) || str.Trim().ToUpper() == "NULL")
            {
                return null;
            }
            try
            {
                string[] spli = str.Trim().Split(' ');
                if (spli.Length == 3)
                {
                    return new Font(spli[0], float.Parse(spli[1]), (FontStyle)(int.Parse(spli[2])));
                }
                else if (spli.Length > 3)
                {
                    int ni = spli.Length - 2;
                    string fn = spli[0];
                    for (int i = 1; i < ni; ++i)
                    {
                        fn += " " + spli[i];
                    }
                    return new Font(fn, float.Parse(spli[spli.Length - 2]), (FontStyle)(int.Parse(spli[spli.Length - 1])));
                }
            }
            catch { }
            return null;
        }

        private void BuildDefault()
        {
            IsCFont = new Font("Consolas", 12F, FontStyle.Bold);
            IsCTextFill = new SolidBrush(Color.White);
            IsCFill = new SolidBrush(Color.Red);
            IsCBorder = new Pen(Color.White, 1F);

            DotTextFont = new Font("微软雅黑", 12F, FontStyle.Bold);
            DotTextFill = new SolidBrush(Color.White);
            DotTextBorder = null;
            DotBorder = new Pen(Color.FromArgb(150, 0, 0, 0), 1F);
            DotFills = new SolidBrush[] {
                new SolidBrush(Color.FromArgb(192, 255, 0, 0)),
                new SolidBrush(Color.FromArgb(192, 0, 190, 0)),
                new SolidBrush(Color.FromArgb(192, 0, 170, 220)),
                new SolidBrush(Color.FromArgb(192, 230, 100, 0))
            };
            CloseFill = new SolidBrush(Color.FromArgb(192, 200, 0, 0));
            CloseLine = new Pen(Color.White, 3);
            TitleFont = new Font("微软雅黑", 12F, FontStyle.Bold);
            TitleFill = new SolidBrush(Color.Black);
            TitleBorder = new Pen(Color.White, 3F);
            GVersionFont = new Font("微软雅黑", 10F, FontStyle.Bold);
            GVersionFill = new SolidBrush(Color.Green);
            GVersionBorder = new Pen(Color.White, 3F);
            VersionFont = new Font("微软雅黑", 10F, FontStyle.Bold);
            VersionFill = new SolidBrush(Color.Green);
            VersionBorder = new Pen(Color.White, 3F);
            BLFont = new Font("微软雅黑", 12F, FontStyle.Bold);
            BLFill = new SolidBrush(Color.OrangeRed);
            BLBorder = new Pen(Color.White, 3F);
            BRFont = new Font("微软雅黑", 12F, FontStyle.Bold);
            BRFill = new SolidBrush(Color.Purple);
            BRBorder = new Pen(Color.White, 3F);
            MoreInfoFont = new Font("微软雅黑", 14F, FontStyle.Regular);
            MoreInfoFill = new SolidBrush(Color.DarkOrange);
            MoreInfoBorder = new Pen(Color.Black, 3F);
            MainTimerFont = new Font("Consolas", 40F, FontStyle.Bold);
            MainTimerFill = new SolidBrush(Color.LawnGreen);
            MainTimerBorder = new Pen(Color.Black, 3F);
            MainTimerMSFont = new Font("Consolas", 26F, FontStyle.Bold);
            SubTimerFont = new Font("Consolas", 14F, FontStyle.Bold);
            SubTimerFill = new SolidBrush(Color.Yellow);
            SubTimerBorder = new Pen(Color.Black, 3F);
            OutTimerFont = new Font("Consolas", 14F, FontStyle.Bold);
            OutTimerFill = new SolidBrush(Color.Yellow);
            OutTimerBorder = new Pen(Color.Black, 3F);
            WillClearFont = new Font("微软雅黑", 14F, FontStyle.Regular);
            WillClearFill = new SolidBrush(Color.Orange);
            WillClearBorder = new Pen(Color.Black, 3F);
            ButtonFill = new SolidBrush(Color.FromArgb(192, 255, 255, 255));
            ButtonBorder = new Pen(Color.FromArgb(150, 0, 0, 0), 1F);
            ButtonTextFont = new Font("微软雅黑", 12F, FontStyle.Regular);
            ButtonTextFill = new SolidBrush(Color.Black);
            ButtonTextFillRed = new SolidBrush(Color.Red);
            ButtonTextFillOrange = new SolidBrush(Color.OrangeRed);
            ButtonTextBorder = null;
            CPNameFont = new Font("微软雅黑", 14F, FontStyle.Bold);
            CPNameFill = new SolidBrush(Color.White);
            CPNameBorder = new Pen(Color.Black, 3F);
            CPBestFont = new Font("Consolas", 12F, FontStyle.Bold);
            CPBestFill = new SolidBrush(Color.LightGray);
            CPBestBorder = new Pen(Color.Black, 3F);
            CPChaFont = new Font("Consolas", 12F, FontStyle.Bold);
            CPCurFont = new Font("Consolas", 16F, FontStyle.Bold);
            CPGoodFill = new SolidBrush(Color.LightGreen);
            CPGoodBorder = new Pen(Color.Black, 3F);
            CPBadFill = new SolidBrush(Color.Red);
            CPBadBorder = new Pen(Color.Black, 3F);
            CPSameFill = new SolidBrush(Color.White);
            CPSameBorder = new Pen(Color.Black, 3F);
            CPItemActBG = new SolidBrush(Color.FromArgb(90, 255, 255, 0));
            CPItemBG = new SolidBrush(Color.FromArgb(50, 255, 255, 255));
        }

        public void Dispose()
        {
            IsCFont?.Dispose();
            IsCFill?.Dispose();
            IsCBorder?.Dispose();
            IsCTextFill?.Dispose();
            CPItemActBG?.Dispose();
            CPItemBG?.Dispose();
            DotTextBorder?.Dispose();
            DotTextFill?.Dispose();
            CPSameBorder?.Dispose();
            CPSameFill?.Dispose();
            CPBadBorder?.Dispose();
            CPBadFill?.Dispose();
            CPGoodBorder?.Dispose();
            CPGoodFill?.Dispose();
            CPCurFont?.Dispose();
            CPChaFont?.Dispose();
            CPBestBorder?.Dispose();
            CPBestFill?.Dispose();
            CPBestFont?.Dispose();
            CPNameBorder?.Dispose();
            CPNameFill?.Dispose();
            CPNameFont?.Dispose();
            ButtonTextBorder?.Dispose();
            ButtonTextFillOrange?.Dispose();
            ButtonTextFillRed?.Dispose();
            ButtonTextFill?.Dispose();
            ButtonTextFont?.Dispose();
            ButtonBorder?.Dispose();
            ButtonFill?.Dispose();
            WillClearBorder?.Dispose();
            WillClearFill?.Dispose();
            WillClearFont?.Dispose();
            OutTimerBorder?.Dispose();
            OutTimerFill?.Dispose();
            OutTimerFont?.Dispose();
            SubTimerBorder?.Dispose();
            SubTimerFill?.Dispose();
            SubTimerFont?.Dispose();
            MainTimerMSFont?.Dispose();
            MainTimerBorder?.Dispose();
            MainTimerFill?.Dispose();
            MainTimerFont?.Dispose();
            MoreInfoBorder?.Dispose();
            MoreInfoFill?.Dispose();
            MoreInfoFont?.Dispose();


            DotTextFont?.Dispose();
            DotBorder?.Dispose();
            if (DotFills != null && DotFills.Length > 0)
            {
                foreach (Brush b in DotFills)
                {
                    b?.Dispose();
                }
            }
            CloseFill?.Dispose();
            CloseLine?.Dispose();
            TitleFont?.Dispose();
            TitleFill?.Dispose();
            TitleBorder?.Dispose();
            GVersionFont?.Dispose();
            GVersionFill?.Dispose();
            GVersionBorder?.Dispose();
            VersionFont?.Dispose();
            VersionFill?.Dispose();
            VersionBorder?.Dispose();
            BLFont?.Dispose();
            BLFill?.Dispose();
            BLBorder?.Dispose();
            BRFont?.Dispose();
            BRFill?.Dispose();
            BRBorder?.Dispose();
        }
    }
}
