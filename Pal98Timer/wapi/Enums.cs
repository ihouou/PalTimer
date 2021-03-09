using System;
using System.Runtime.InteropServices;
using System.Drawing;


namespace HFrame.OS
{
	/// <summary>
	/// Window API enumerations
	/// </summary>
	
	#region Peek Message Flags
	public enum PeekMessageFlags
	{
		PM_NOREMOVE		= 0,
		PM_REMOVE		= 1,
		PM_NOYIELD		= 2
	}
	#endregion
	
	#region Windows Messages
	public enum WinMsg
	{
		WM_NULL                   = 0x0000,
		WM_CREATE                 = 0x0001,
		WM_DESTROY                = 0x0002,
		WM_MOVE                   = 0x0003,
		WM_SIZE                   = 0x0005,
		WM_ACTIVATE               = 0x0006,
		WM_SETFOCUS               = 0x0007,
		WM_KILLFOCUS              = 0x0008,
		WM_ENABLE                 = 0x000A,
		WM_SETREDRAW              = 0x000B,
		WM_SETTEXT                = 0x000C,
		WM_GETTEXT                = 0x000D,
		WM_GETTEXTLENGTH          = 0x000E,
		WM_PAINT                  = 0x000F,
		WM_CLOSE                  = 0x0010,
		WM_QUERYENDSESSION        = 0x0011,
		WM_QUIT                   = 0x0012,
		WM_QUERYOPEN              = 0x0013,
		WM_ERASEBKGND             = 0x0014,
		WM_SYSCOLORCHANGE         = 0x0015,
		WM_ENDSESSION             = 0x0016,
		WM_SHOWWINDOW             = 0x0018,
		WM_CTLCOLOR               = 0x0019,
		WM_WININICHANGE           = 0x001A,
		WM_SETTINGCHANGE          = 0x001A,
		WM_DEVMODECHANGE          = 0x001B,
		WM_ACTIVATEAPP            = 0x001C,
		WM_FONTCHANGE             = 0x001D,
		WM_TIMECHANGE             = 0x001E,
		WM_CANCELMODE             = 0x001F,
		WM_SETCURSOR              = 0x0020,
		WM_MOUSEACTIVATE          = 0x0021,
		WM_CHILDACTIVATE          = 0x0022,
		WM_QUEUESYNC              = 0x0023,
		WM_GETMINMAXINFO          = 0x0024,
		WM_PAINTICON              = 0x0026,
		WM_ICONERASEBKGND         = 0x0027,
		WM_NEXTDLGCTL             = 0x0028,
		WM_SPOOLERSTATUS          = 0x002A,
		WM_DRAWITEM               = 0x002B,
		WM_MEASUREITEM            = 0x002C,
		WM_DELETEITEM             = 0x002D,
		WM_VKEYTOITEM             = 0x002E,
		WM_CHARTOITEM             = 0x002F,
		WM_SETFONT                = 0x0030,
		WM_GETFONT                = 0x0031,
		WM_SETHOTKEY              = 0x0032,
		WM_GETHOTKEY              = 0x0033,
		WM_QUERYDRAGICON          = 0x0037,
		WM_COMPAREITEM            = 0x0039,
		WM_GETOBJECT              = 0x003D,
		WM_COMPACTING             = 0x0041,
		WM_COMMNOTIFY             = 0x0044 ,
		WM_WINDOWPOSCHANGING      = 0x0046,
		WM_WINDOWPOSCHANGED       = 0x0047,
		WM_POWER                  = 0x0048,
		WM_COPYDATA               = 0x004A,
		WM_CANCELJOURNAL          = 0x004B,
		WM_NOTIFY                 = 0x004E,
		WM_INPUTLANGCHANGEREQUEST = 0x0050,
		WM_INPUTLANGCHANGE        = 0x0051,
		WM_TCARD                  = 0x0052,
		WM_HELP                   = 0x0053,
		WM_USERCHANGED            = 0x0054,
		WM_NOTIFYFORMAT           = 0x0055,
		WM_CONTEXTMENU            = 0x007B,
		WM_STYLECHANGING          = 0x007C,
		WM_STYLECHANGED           = 0x007D,
		WM_DISPLAYCHANGE          = 0x007E,
		WM_GETICON                = 0x007F,
		WM_SETICON                = 0x0080,
		WM_NCCREATE               = 0x0081,
		WM_NCDESTROY              = 0x0082,
		WM_NCCALCSIZE             = 0x0083,
		WM_NCHITTEST              = 0x0084,
		WM_NCPAINT                = 0x0085,
		WM_NCACTIVATE             = 0x0086,
		WM_GETDLGCODE             = 0x0087,
		WM_SYNCPAINT              = 0x0088,
		WM_NCMOUSEMOVE            = 0x00A0,
		WM_NCLBUTTONDOWN          = 0x00A1,
		WM_NCLBUTTONUP            = 0x00A2,
		WM_NCLBUTTONDBLCLK        = 0x00A3,
		WM_NCRBUTTONDOWN          = 0x00A4,
		WM_NCRBUTTONUP            = 0x00A5,
		WM_NCRBUTTONDBLCLK        = 0x00A6,
		WM_NCMBUTTONDOWN          = 0x00A7,
		WM_NCMBUTTONUP            = 0x00A8,
		WM_NCMBUTTONDBLCLK        = 0x00A9,
		WM_KEYDOWN                = 0x0100,
		WM_KEYUP                  = 0x0101,
		WM_CHAR                   = 0x0102,
		WM_DEADCHAR               = 0x0103,
		WM_SYSKEYDOWN             = 0x0104,
		WM_SYSKEYUP               = 0x0105,
		WM_SYSCHAR                = 0x0106,
		WM_SYSDEADCHAR            = 0x0107,
		WM_KEYLAST                = 0x0108,
		WM_IME_STARTCOMPOSITION   = 0x010D,
		WM_IME_ENDCOMPOSITION     = 0x010E,
		WM_IME_COMPOSITION        = 0x010F,
		WM_IME_KEYLAST            = 0x010F,
		WM_INITDIALOG             = 0x0110,
		WM_COMMAND                = 0x0111,
		WM_SYSCOMMAND             = 0x0112,
		WM_TIMER                  = 0x0113,
		WM_HSCROLL                = 0x0114,
		WM_VSCROLL                = 0x0115,
		WM_INITMENU               = 0x0116,
		WM_INITMENUPOPUP          = 0x0117,
		WM_MENUSELECT             = 0x011F,
		WM_MENUCHAR               = 0x0120,
		WM_ENTERIDLE              = 0x0121,
		WM_MENURBUTTONUP          = 0x0122,
		WM_MENUDRAG               = 0x0123,
		WM_MENUGETOBJECT          = 0x0124,
		WM_UNINITMENUPOPUP        = 0x0125,
		WM_MENUCOMMAND            = 0x0126,
		WM_CTLCOLORWinMsgBOX      = 0x0132,
		WM_CTLCOLOREDIT           = 0x0133,
		WM_CTLCOLORLISTBOX        = 0x0134,
		WM_CTLCOLORBTN            = 0x0135,
		WM_CTLCOLORDLG            = 0x0136,
		WM_CTLCOLORSCROLLBAR      = 0x0137,
		WM_CTLCOLORSTATIC         = 0x0138,
		WM_MOUSEMOVE              = 0x0200,
		WM_LBUTTONDOWN            = 0x0201,
		WM_LBUTTONUP              = 0x0202,
		WM_LBUTTONDBLCLK          = 0x0203,
		WM_RBUTTONDOWN            = 0x0204,
		WM_RBUTTONUP              = 0x0205,
		WM_RBUTTONDBLCLK          = 0x0206,
		WM_MBUTTONDOWN            = 0x0207,
		WM_MBUTTONUP              = 0x0208,
		WM_MBUTTONDBLCLK          = 0x0209,
		WM_MOUSEWHEEL             = 0x020A,
		WM_PARENTNOTIFY           = 0x0210,
		WM_ENTERMENULOOP          = 0x0211,
		WM_EXITMENULOOP           = 0x0212,
		WM_NEXTMENU               = 0x0213,
		WM_SIZING                 = 0x0214,
		WM_CAPTURECHANGED         = 0x0215,
		WM_MOVING                 = 0x0216,
		WM_DEVICECHANGE           = 0x0219,
		WM_MDICREATE              = 0x0220,
		WM_MDIDESTROY             = 0x0221,
		WM_MDIACTIVATE            = 0x0222,
		WM_MDIRESTORE             = 0x0223,
		WM_MDINEXT                = 0x0224,
		WM_MDIMAXIMIZE            = 0x0225,
		WM_MDITILE                = 0x0226,
		WM_MDICASCADE             = 0x0227,
		WM_MDIICONARRANGE         = 0x0228,
		WM_MDIGETACTIVE           = 0x0229,
		WM_MDISETMENU             = 0x0230,
		WM_ENTERSIZEMOVE          = 0x0231,
		WM_EXITSIZEMOVE           = 0x0232,
		WM_DROPFILES              = 0x0233,
		WM_MDIREFRESHMENU         = 0x0234,
		WM_IME_SETCONTEXT         = 0x0281,
		WM_IME_NOTIFY             = 0x0282,
		WM_IME_CONTROL            = 0x0283,
		WM_IME_COMPOSITIONFULL    = 0x0284,
		WM_IME_SELECT             = 0x0285,
		WM_IME_CHAR               = 0x0286,
		WM_IME_REQUEST            = 0x0288,
		WM_IME_KEYDOWN            = 0x0290,
		WM_IME_KEYUP              = 0x0291,
		WM_MOUSEHOVER             = 0x02A1,
		WM_MOUSELEAVE             = 0x02A3,
		WM_CUT                    = 0x0300,
		WM_COPY                   = 0x0301,
		WM_PASTE                  = 0x0302,
		WM_CLEAR                  = 0x0303,
		WM_UNDO                   = 0x0304,
		WM_RENDERFORMAT           = 0x0305,
		WM_RENDERALLFORMATS       = 0x0306,
		WM_DESTROYCLIPBOARD       = 0x0307,
		WM_DRAWCLIPBOARD          = 0x0308,
		WM_PAINTCLIPBOARD         = 0x0309,
		WM_VSCROLLCLIPBOARD       = 0x030A,
		WM_SIZECLIPBOARD          = 0x030B,
		WM_ASKCBFORMATNAME        = 0x030C,
		WM_CHANGECBCHAIN          = 0x030D,
		WM_HSCROLLCLIPBOARD       = 0x030E,
		WM_QUERYNEWPALETTE        = 0x030F,
		WM_PALETTEISCHANGING      = 0x0310,
		WM_PALETTECHANGED         = 0x0311,
		WM_HOTKEY                 = 0x0312,
		WM_PRINT                  = 0x0317,
		WM_PRINTCLIENT            = 0x0318,
		WM_HANDHELDFIRST          = 0x0358,
		WM_HANDHELDLAST           = 0x035F,
		WM_AFXFIRST               = 0x0360,
		WM_AFXLAST                = 0x037F,
		WM_PENWINFIRST            = 0x0380,
		WM_PENWINLAST             = 0x038F,
		WM_APP                    = 0x8000,
		WM_USER                   = 0x0400,
		WM_REFLECT                = WM_USER + 0x1c00
	}
	#endregion

	#region Window Styles
	public enum WindowStyles : uint
	{
		WS_OVERLAPPED       = 0x00000000,
		WS_POPUP            = 0x80000000,
		WS_CHILD            = 0x40000000,
		WS_MINIMIZE         = 0x20000000,
		WS_VISIBLE          = 0x10000000,
		WS_DISABLED         = 0x08000000,
		WS_CLIPSIBLINGS     = 0x04000000,
		WS_CLIPCHILDREN     = 0x02000000,
		WS_MAXIMIZE         = 0x01000000,
		WS_CAPTION          = 0x00C00000,
		WS_BORDER           = 0x00800000,
		WS_DLGFRAME         = 0x00400000,
		WS_VSCROLL          = 0x00200000,
		WS_HSCROLL          = 0x00100000,
		WS_SYSMENU          = 0x00080000,
		WS_THICKFRAME       = 0x00040000,
		WS_GROUP            = 0x00020000,
		WS_TABSTOP          = 0x00010000,
		WS_MINIMIZEBOX      = 0x00020000,
		WS_MAXIMIZEBOX      = 0x00010000,
		WS_TILED            = 0x00000000,
		WS_ICONIC           = 0x20000000,
		WS_SIZEBOX          = 0x00040000,
		WS_POPUPWINDOW      = 0x80880000,
		WS_OVERLAPPEDWINDOW = 0x00CF0000,
		WS_TILEDWINDOW      = 0x00CF0000,
		WS_CHILDWINDOW      = 0x40000000
	}
	#endregion

	#region Window Extended Styles
	public enum WindowExStyles
	{
		WS_EX_DLGMODALFRAME     = 0x00000001,
		WS_EX_NOPARENTNOTIFY    = 0x00000004,
		WS_EX_TOPMOST           = 0x00000008,
		WS_EX_ACCEPTFILES       = 0x00000010,
		WS_EX_TRANSPARENT       = 0x00000020,
		WS_EX_MDICHILD          = 0x00000040,
		WS_EX_TOOLWINDOW        = 0x00000080,
		WS_EX_WINDOWEDGE        = 0x00000100,
		WS_EX_CLIENTEDGE        = 0x00000200,
		WS_EX_CONTEXTHELP       = 0x00000400,
		WS_EX_RIGHT             = 0x00001000,
		WS_EX_LEFT              = 0x00000000,
		WS_EX_RTLREADING        = 0x00002000,
		WS_EX_LTRREADING        = 0x00000000,
		WS_EX_LEFTSCROLLBAR     = 0x00004000,
		WS_EX_RIGHTSCROLLBAR    = 0x00000000,
		WS_EX_CONTROLPARENT     = 0x00010000,
		WS_EX_STATICEDGE        = 0x00020000,
		WS_EX_APPWINDOW         = 0x00040000,
		WS_EX_OVERLAPPEDWINDOW  = 0x00000300,
		WS_EX_PALETTEWINDOW     = 0x00000188,
		WS_EX_LAYERED			= 0x00080000
	}
	#endregion

	#region ShowWindow Styles
	public enum ShowWindowStyles : short
	{
		SW_HIDE             = 0,
		SW_SHOWNORMAL       = 1,
		SW_NORMAL           = 1,
		SW_SHOWMINIMIZED    = 2,
		SW_SHOWMAXIMIZED    = 3,
		SW_MAXIMIZE         = 3,
		SW_SHOWNOACTIVATE   = 4,
		SW_SHOW             = 5,
		SW_MINIMIZE         = 6,
		SW_SHOWMINNOACTIVE  = 7,
		SW_SHOWNA           = 8,
		SW_RESTORE          = 9,
		SW_SHOWDEFAULT      = 10,
		SW_FORCEMINIMIZE    = 11,
		SW_MAX              = 11
	}

	#endregion

	#region SetWindowPos Z Order
	public enum SetWindowPosZOrder
	{
		HWND_TOP        = 0,
		HWND_BOTTOM     = 1,
		HWND_TOPMOST    = -1,
		HWND_NOTOPMOST  = -2
	}
	#endregion

	#region SetWindowPosFlags
	public enum SetWindowPosFlags : uint
	{
		SWP_NOSIZE          = 0x0001,
		SWP_NOMOVE          = 0x0002,
		SWP_NOZORDER        = 0x0004,
		SWP_NOREDRAW        = 0x0008,
		SWP_NOACTIVATE      = 0x0010,
		SWP_FRAMECHANGED    = 0x0020,
		SWP_SHOWWINDOW      = 0x0040,
		SWP_HIDEWINDOW      = 0x0080,
		SWP_NOCOPYBITS      = 0x0100,
		SWP_NOOWNERZORDER   = 0x0200, 
		SWP_NOSENDCHANGING  = 0x0400,
		SWP_DRAWFRAME       = 0x0020,
		SWP_NOREPOSITION    = 0x0200,
		SWP_DEFERERASE      = 0x2000,
		SWP_ASYNCWINDOWPOS  = 0x4000
	}
	#endregion

	#region Virtual Keys
	public enum VirtualKeys
	{
		VK_LBUTTON		= 0x01,
		VK_CANCEL		= 0x03,
		VK_BACK			= 0x08,
		VK_TAB			= 0x09,
		VK_CLEAR		= 0x0C,
		VK_RETURN		= 0x0D,
		VK_SHIFT		= 0x10,
		VK_CONTROL		= 0x11,
		VK_MENU			= 0x12,
		VK_CAPITAL		= 0x14,
		VK_ESCAPE		= 0x1B,
		VK_SPACE		= 0x20,
		VK_PRIOR		= 0x21,
		VK_NEXT			= 0x22,
		VK_END			= 0x23,
		VK_HOME			= 0x24,
		VK_LEFT			= 0x25,
		VK_UP			= 0x26,
		VK_RIGHT		= 0x27,
		VK_DOWN			= 0x28,
		VK_SELECT		= 0x29,
		VK_EXECUTE		= 0x2B,
		VK_SNAPSHOT		= 0x2C,
		VK_HELP			= 0x2F,
		VK_0			= 0x30,
		VK_1			= 0x31,
		VK_2			= 0x32,
		VK_3			= 0x33,
		VK_4			= 0x34,
		VK_5			= 0x35,
		VK_6			= 0x36,
		VK_7			= 0x37,
		VK_8			= 0x38,
		VK_9			= 0x39,
		VK_A			= 0x41,
		VK_B			= 0x42,
		VK_C			= 0x43,
		VK_D			= 0x44,
		VK_E			= 0x45,
		VK_F			= 0x46,
		VK_G			= 0x47,
		VK_H			= 0x48,
		VK_I			= 0x49,
		VK_J			= 0x4A,
		VK_K			= 0x4B,
		VK_L			= 0x4C,
		VK_M			= 0x4D,
		VK_N			= 0x4E,
		VK_O			= 0x4F,
		VK_P			= 0x50,
		VK_Q			= 0x51,
		VK_R			= 0x52,
		VK_S			= 0x53,
		VK_T			= 0x54,
		VK_U			= 0x55,
		VK_V			= 0x56,
		VK_W			= 0x57,
		VK_X			= 0x58,
		VK_Y			= 0x59,
		VK_Z			= 0x5A,
		VK_NUMPAD0		= 0x60,
		VK_NUMPAD1		= 0x61,
		VK_NUMPAD2		= 0x62,
		VK_NUMPAD3		= 0x63,
		VK_NUMPAD4		= 0x64,
		VK_NUMPAD5		= 0x65,
		VK_NUMPAD6		= 0x66,
		VK_NUMPAD7		= 0x67,
		VK_NUMPAD8		= 0x68,
		VK_NUMPAD9		= 0x69,
		VK_MULTIPLY		= 0x6A,
		VK_ADD			= 0x6B,
		VK_SEPARATOR	= 0x6C,
		VK_SUBTRACT		= 0x6D,
		VK_DECIMAL		= 0x6E,
		VK_DIVIDE		= 0x6F,
		VK_ATTN			= 0xF6,
		VK_CRSEL		= 0xF7,
		VK_EXSEL		= 0xF8,
		VK_EREOF		= 0xF9,
		VK_PLAY			= 0xFA,  
		VK_ZOOM			= 0xFB,
		VK_NONAME		= 0xFC,
		VK_PA1			= 0xFD,
		VK_OEM_CLEAR	= 0xFE,
		VK_LWIN			= 0x5B,
		VK_RWIN			= 0x5C,
		VK_APPS			= 0x5D,   
		VK_LSHIFT		= 0xA0,   
		VK_RSHIFT		= 0xA1,   
		VK_LCONTROL		= 0xA2,   
		VK_RCONTROL		= 0xA3,   
		VK_LMENU		= 0xA4,   
		VK_RMENU		= 0xA5
	}
	#endregion
		
	#region PatBlt Types
	public enum PatBltTypes
	{
		SRCCOPY          =   0x00CC0020,
		SRCPAINT         =   0x00EE0086,
		SRCAND           =   0x008800C6,
		SRCINVERT        =   0x00660046,
		SRCERASE         =   0x00440328,
		NOTSRCCOPY       =   0x00330008,
		NOTSRCERASE      =   0x001100A6,
		MERGECOPY        =   0x00C000CA,
		MERGEPAINT       =   0x00BB0226,
		PATCOPY          =   0x00F00021,
		PATPAINT         =   0x00FB0A09,
		PATINVERT        =   0x005A0049,
		DSTINVERT        =   0x00550009,
		BLACKNESS        =   0x00000042,
		WHITENESS        =   0x00FF0062
	}
	#endregion
	
	#region Clipboard Formats
	public enum ClipboardFormats : uint
	{			
		CF_TEXT             = 1,
		CF_BITMAP           = 2,
		CF_METAFILEPICT     = 3,
		CF_SYLK             = 4,
		CF_DIF              = 5,
		CF_TIFF             = 6,
		CF_OEMTEXT          = 7,
		CF_DIB              = 8,
		CF_PALETTE          = 9,
		CF_PENDATA          = 10,
		CF_RIFF             = 11,
		CF_WAVE             = 12,
		CF_UNICODETEXT      = 13,
		CF_ENHMETAFILE      = 14,
		CF_HDROP            = 15,
		CF_LOCALE           = 16,
		CF_MAX              = 17,
		CF_OWNERDISPLAY     = 0x0080,
		CF_DSPTEXT          = 0x0081,
		CF_DSPBITMAP        = 0x0082,
		CF_DSPMETAFILEPICT  = 0x0083,
		CF_DSPENHMETAFILE   = 0x008E,
		CF_PRIVATEFIRST     = 0x0200,
		CF_PRIVATELAST      = 0x02FF,
		CF_GDIOBJFIRST      = 0x0300,
		CF_GDIOBJLAST       = 0x03FF
	}
	#endregion

	#region Common Controls Initialization flags
	public enum CommonControlInitFlags
	{
		ICC_LISTVIEW_CLASSES   = 0x00000001, 
		ICC_TREEVIEW_CLASSES   = 0x00000002, 
		ICC_BAR_CLASSES        = 0x00000004, 
		ICC_TAB_CLASSES        = 0x00000008, 
		ICC_UPDOWN_CLASS       = 0x00000010, 
		ICC_PROGRESS_CLASS     = 0x00000020, 
		ICC_HOTKEY_CLASS       = 0x00000040, 
		ICC_ANIMATE_CLASS      = 0x00000080, 
		ICC_WIN95_CLASSES      = 0x000000FF,
		ICC_DATE_CLASSES       = 0x00000100, 
		ICC_USEREX_CLASSES     = 0x00000200,
		ICC_COOL_CLASSES       = 0x00000400, 
		ICC_INTERNET_CLASSES   = 0x00000800,
		ICC_PAGESCROLLER_CLASS = 0x00001000, 
		ICC_NATIVEFNTCTL_CLASS = 0x00002000  
	}
	#endregion

	#region Common Controls Styles
	public  enum CommonControlStyles
	{
		CCS_TOP                 = 0x00000001,
		CCS_NOMOVEY             = 0x00000002,
		CCS_BOTTOM              = 0x00000003,
		CCS_NORESIZE            = 0x00000004,
		CCS_NOPARENTALIGN       = 0x00000008,
		CCS_ADJUSTABLE          = 0x00000020,
		CCS_NODIVIDER           = 0x00000040,
		CCS_VERT                = 0x00000080,
		CCS_LEFT                = (CCS_VERT | CCS_TOP),
		CCS_RIGHT               = (CCS_VERT | CCS_BOTTOM),
		CCS_NOMOVEX             = (CCS_VERT | CCS_NOMOVEY)
	}
	#endregion

	#region ToolBar Styles
	public enum ToolBarStyles
	{
		TBSTYLE_BUTTON          = 0x0000,
		TBSTYLE_SEP             = 0x0001,
		TBSTYLE_CHECK           = 0x0002,
		TBSTYLE_GROUP           = 0x0004,
		TBSTYLE_CHECKGROUP      = (TBSTYLE_GROUP | TBSTYLE_CHECK),
		TBSTYLE_DROPDOWN        = 0x0008,
		TBSTYLE_AUTOSIZE        = 0x0010,
		TBSTYLE_NOPREFIX        = 0x0020, 
		TBSTYLE_TOOLTIPS        = 0x0100,
		TBSTYLE_WRAPABLE        = 0x0200,
		TBSTYLE_ALTDRAG         = 0x0400,
		TBSTYLE_FLAT            = 0x0800,
		TBSTYLE_LIST            = 0x1000,
		TBSTYLE_CUSTOMERASE     = 0x2000,
		TBSTYLE_REGISTERDROP    = 0x4000,
		TBSTYLE_TRANSPARENT     = 0x8000,
		TBSTYLE_EX_DRAWDDARROWS = 0x00000001
	}
	#endregion

	#region ToolBar Ex Styles
	public enum ToolBarExStyles
	{
		TBSTYLE_EX_DRAWDDARROWS			= 0x1,
		TBSTYLE_EX_HIDECLIPPEDBUTTONS	= 0x10,
		TBSTYLE_EX_DOUBLEBUFFER			= 0x80
	}
	#endregion

	#region ToolBar Messages
	public enum ToolBarMessages
	{
		WM_USER                 =  0x0400,
		TB_ENABLEBUTTON         = (WM_USER + 1),
		TB_CHECKBUTTON          = (WM_USER + 2),
		TB_PRESSBUTTON          = (WM_USER + 3),
		TB_HIDEBUTTON           = (WM_USER + 4),
		TB_INDETERMINATE        = (WM_USER + 5),
		TB_MARKBUTTON           = (WM_USER + 6),
		TB_ISBUTTONENABLED      = (WM_USER + 9),
		TB_ISBUTTONCHECKED      = (WM_USER + 10),
		TB_ISBUTTONPRESSED      = (WM_USER + 11),
		TB_ISBUTTONHIDDEN       = (WM_USER + 12),
		TB_ISBUTTONINDETERMINATE= (WM_USER + 13),
		TB_ISBUTTONHIGHLIGHTED  = (WM_USER + 14),
		TB_SETSTATE             = (WM_USER + 17),
		TB_GETSTATE             = (WM_USER + 18),
		TB_ADDBITMAP            = (WM_USER + 19),
		TB_ADDBUTTONSA          = (WM_USER + 20),
		TB_INSERTBUTTONA        = (WM_USER + 21),
		TB_ADDBUTTONS           = (WM_USER + 20),
		TB_INSERTBUTTON         = (WM_USER + 21),
		TB_DELETEBUTTON         = (WM_USER + 22),
		TB_GETBUTTON            = (WM_USER + 23),
		TB_BUTTONCOUNT          = (WM_USER + 24),
		TB_COMMANDTOINDEX       = (WM_USER + 25),
		TB_SAVERESTOREA         = (WM_USER + 26),
		TB_CUSTOMIZE            = (WM_USER + 27),
		TB_ADDSTRINGA           = (WM_USER + 28),
		TB_GETITEMRECT          = (WM_USER + 29),
		TB_BUTTONSTRUCTSIZE     = (WM_USER + 30),
		TB_SETBUTTONSIZE        = (WM_USER + 31),
		TB_SETBITMAPSIZE        = (WM_USER + 32),
		TB_AUTOSIZE             = (WM_USER + 33),
		TB_GETTOOLTIPS          = (WM_USER + 35),
		TB_SETTOOLTIPS          = (WM_USER + 36),
		TB_SETPARENT            = (WM_USER + 37),
		TB_SETROWS              = (WM_USER + 39),
		TB_GETROWS              = (WM_USER + 40),
		TB_GETBITMAPFLAGS       = (WM_USER + 41),
		TB_SETCMDID             = (WM_USER + 42),
		TB_CHANGEBITMAP         = (WM_USER + 43),
		TB_GETBITMAP            = (WM_USER + 44),
		TB_GETBUTTONTEXTA       = (WM_USER + 45),
		TB_GETBUTTONTEXTW       = (WM_USER + 75),
		TB_REPLACEBITMAP        = (WM_USER + 46),
		TB_SETINDENT            = (WM_USER + 47),
		TB_SETIMAGELIST         = (WM_USER + 48),
		TB_GETIMAGELIST         = (WM_USER + 49),
		TB_LOADIMAGES           = (WM_USER + 50),
		TB_GETRECT              = (WM_USER + 51),
		TB_SETHOTIMAGELIST      = (WM_USER + 52),
		TB_GETHOTIMAGELIST      = (WM_USER + 53),
		TB_SETDISABLEDIMAGELIST = (WM_USER + 54),
		TB_GETDISABLEDIMAGELIST = (WM_USER + 55),
		TB_SETSTYLE             = (WM_USER + 56),
		TB_GETSTYLE             = (WM_USER + 57),
		TB_GETBUTTONSIZE        = (WM_USER + 58),
		TB_SETBUTTONWIDTH       = (WM_USER + 59),
		TB_SETMAXTEXTROWS       = (WM_USER + 60),
		TB_GETTEXTROWS          = (WM_USER + 61),
		TB_GETOBJECT            = (WM_USER + 62), 
		TB_GETBUTTONINFOW       = (WM_USER + 63),
		TB_SETBUTTONINFOW       = (WM_USER + 64),
		TB_GETBUTTONINFOA       = (WM_USER + 65),
		TB_SETBUTTONINFOA       = (WM_USER + 66),
		TB_INSERTBUTTONW        = (WM_USER + 67),
		TB_ADDBUTTONSW          = (WM_USER + 68),
		TB_HITTEST              = (WM_USER + 69),
		TB_SETDRAWTEXTFLAGS     = (WM_USER + 70),
		TB_GETHOTITEM           = (WM_USER + 71),
		TB_SETHOTITEM           = (WM_USER + 72), 
		TB_SETANCHORHIGHLIGHT   = (WM_USER + 73),  
		TB_GETANCHORHIGHLIGHT   = (WM_USER + 74),
		TB_SAVERESTOREW         = (WM_USER + 76),
		TB_ADDSTRINGW           = (WM_USER + 77),
		TB_MAPACCELERATORA      = (WM_USER + 78), 
		TB_GETINSERTMARK        = (WM_USER + 79), 
		TB_SETINSERTMARK        = (WM_USER + 80), 
		TB_INSERTMARKHITTEST    = (WM_USER + 81),  
		TB_MOVEBUTTON           = (WM_USER + 82),
		TB_GETMAXSIZE           = (WM_USER + 83),  
		TB_SETEXTENDEDSTYLE     = (WM_USER + 84),  
		TB_GETEXTENDEDSTYLE     = (WM_USER + 85),  
		TB_GETPADDING           = (WM_USER + 86),
		TB_SETPADDING           = (WM_USER + 87),
		TB_SETINSERTMARKCOLOR   = (WM_USER + 88),
		TB_GETINSERTMARKCOLOR   = (WM_USER + 89)
	}
	#endregion

	#region ToolBar Notifications
	public enum ToolBarNotifications
	{
		TTN_NEEDTEXTA		= ((0-520)-0),
		TTN_NEEDTEXTW		= ((0-520)-10),
		TBN_QUERYINSERT		= ((0-700)-6),
		TBN_DROPDOWN		= ((0-700)-10),
		TBN_HOTITEMCHANGE	= ((0 - 700) - 13)
	}
	#endregion

	#region Reflected Messages
	public enum ReflectedMessages
	{
		OCM__BASE				= (WinMsg.WM_USER+0x1c00),
		OCM_COMMAND				= (OCM__BASE + WinMsg.WM_COMMAND),
		OCM_CTLCOLORBTN			= (OCM__BASE + WinMsg.WM_CTLCOLORBTN),
		OCM_CTLCOLOREDIT		= (OCM__BASE + WinMsg.WM_CTLCOLOREDIT),
		OCM_CTLCOLORDLG			= (OCM__BASE + WinMsg.WM_CTLCOLORDLG),
		OCM_CTLCOLORLISTBOX		= (OCM__BASE + WinMsg.WM_CTLCOLORLISTBOX),
		OCM_CTLCOLORWinMsgBOX		= (OCM__BASE + WinMsg.WM_CTLCOLORWinMsgBOX),
		OCM_CTLCOLORSCROLLBAR   = (OCM__BASE + WinMsg.WM_CTLCOLORSCROLLBAR),
		OCM_CTLCOLORSTATIC		= (OCM__BASE + WinMsg.WM_CTLCOLORSTATIC),
		OCM_CTLCOLOR			= (OCM__BASE + WinMsg.WM_CTLCOLOR),
		OCM_DRAWITEM			= (OCM__BASE + WinMsg.WM_DRAWITEM),
		OCM_MEASUREITEM			= (OCM__BASE + WinMsg.WM_MEASUREITEM),
		OCM_DELETEITEM          = (OCM__BASE + WinMsg.WM_DELETEITEM),
		OCM_VKEYTOITEM          = (OCM__BASE + WinMsg.WM_VKEYTOITEM),
		OCM_CHARTOITEM          = (OCM__BASE + WinMsg.WM_CHARTOITEM),
		OCM_COMPAREITEM         = (OCM__BASE + WinMsg.WM_COMPAREITEM),
		OCM_HSCROLL             = (OCM__BASE + WinMsg.WM_HSCROLL),
		OCM_VSCROLL             = (OCM__BASE + WinMsg.WM_VSCROLL),
		OCM_PARENTNOTIFY        = (OCM__BASE + WinMsg.WM_PARENTNOTIFY),
		OCM_NOTIFY              = (OCM__BASE + WinMsg.WM_NOTIFY)
	}
	#endregion

	#region Notification Messages
	public enum NotificationMessages
	{
		NM_FIRST      = (0-0),
		NM_CUSTOMDRAW = (NM_FIRST-12),
		NM_NCHITTEST  = (NM_FIRST-14) 
	}
	#endregion

	#region ToolTip Flags
	public enum ToolTipFlags
	{
		TTF_CENTERTIP           = 0x0002,
		TTF_RTLREADING          = 0x0004,
		TTF_SUBCLASS            = 0x0010,
		TTF_TRACK               = 0x0020,
		TTF_ABSOLUTE            = 0x0080,
		TTF_TRANSPARENT         = 0x0100,
		TTF_DI_SETITEM          = 0x8000   
	}
	#endregion

	#region Custom Draw Return Flags
	public enum CustomDrawReturnFlags
	{
		CDRF_DODEFAULT          = 0x00000000,
		CDRF_NEWFONT            = 0x00000002,
		CDRF_SKIPDEFAULT        = 0x00000004,
		CDRF_NOTIFYPOSTPAINT    = 0x00000010,
		CDRF_NOTIFYITEMDRAW     = 0x00000020,
		CDRF_NOTIFYSUBITEMDRAW  = 0x00000020, 
		CDRF_NOTIFYPOSTERASE    = 0x00000040
	}
	#endregion

	#region Custom Draw Item State Flags
	public enum CustomDrawItemStateFlags
	{
		CDIS_SELECTED       = 0x0001,
		CDIS_GRAYED         = 0x0002,
		CDIS_DISABLED       = 0x0004,
		CDIS_CHECKED        = 0x0008,
		CDIS_FOCUS          = 0x0010,
		CDIS_DEFAULT        = 0x0020,
		CDIS_HOT            = 0x0040,
		CDIS_MARKED         = 0x0080,
		CDIS_INDETERMINATE  = 0x0100
	}
	#endregion

	#region Custom Draw Draw State Flags
	public enum CustomDrawDrawStateFlags
	{
		CDDS_PREPAINT           = 0x00000001,
		CDDS_POSTPAINT          = 0x00000002,
		CDDS_PREERASE           = 0x00000003,
		CDDS_POSTERASE          = 0x00000004,
		CDDS_ITEM               = 0x00010000,
		CDDS_ITEMPREPAINT       = (CDDS_ITEM | CDDS_PREPAINT),
		CDDS_ITEMPOSTPAINT      = (CDDS_ITEM | CDDS_POSTPAINT),
		CDDS_ITEMPREERASE       = (CDDS_ITEM | CDDS_PREERASE),
		CDDS_ITEMPOSTERASE      = (CDDS_ITEM | CDDS_POSTERASE),
		CDDS_SUBITEM            = 0x00020000
	}
	#endregion

	#region Toolbar button info flags
	public enum ToolBarButtonInfoFlags
	{
		TBIF_IMAGE             = 0x00000001,
		TBIF_TEXT              = 0x00000002,
		TBIF_STATE             = 0x00000004,
		TBIF_STYLE             = 0x00000008,
		TBIF_LPARAM            = 0x00000010,
		TBIF_COMMAND           = 0x00000020,
		TBIF_SIZE              = 0x00000040,
		I_IMAGECALLBACK        = -1,
		I_IMAGENONE            = -2
	}
	#endregion

	#region Toolbar button styles
	public enum ToolBarButtonStyles
	{
		TBSTYLE_BUTTON          = 0x0000,
		TBSTYLE_SEP             = 0x0001,
		TBSTYLE_CHECK           = 0x0002,
		TBSTYLE_GROUP           = 0x0004,
		TBSTYLE_CHECKGROUP      = (TBSTYLE_GROUP | TBSTYLE_CHECK),
		TBSTYLE_DROPDOWN        = 0x0008,
		TBSTYLE_AUTOSIZE        = 0x0010,
		TBSTYLE_NOPREFIX        = 0x0020, 
		TBSTYLE_TOOLTIPS        = 0x0100,
		TBSTYLE_WRAPABLE        = 0x0200,
		TBSTYLE_ALTDRAG         = 0x0400,
		TBSTYLE_FLAT            = 0x0800,
		TBSTYLE_LIST            = 0x1000,
		TBSTYLE_CUSTOMERASE     = 0x2000,
		TBSTYLE_REGISTERDROP    = 0x4000,
		TBSTYLE_TRANSPARENT     = 0x8000,
		TBSTYLE_EX_DRAWDDARROWS = 0x00000001
	}
	#endregion

	#region Toolbar button state
	public enum ToolBarButtonStates
	{
		TBSTATE_CHECKED         = 0x01,
		TBSTATE_PRESSED         = 0x02,
		TBSTATE_ENABLED         = 0x04,
		TBSTATE_HIDDEN          = 0x08,
		TBSTATE_INDETERMINATE   = 0x10,
		TBSTATE_WRAP            = 0x20,
		TBSTATE_ELLIPSES        = 0x40,
		TBSTATE_MARKED          = 0x80
	}
	#endregion

	#region Windows Hook Codes
	public enum WindowsHookCodes
	{
		WH_MSGFILTER        = (-1),
		WH_JOURNALRECORD    = 0,
		WH_JOURNALPLAYBACK  = 1,
		WH_KEYBOARD         = 2,
		WH_GETMESSAGE       = 3,
		WH_CALLWNDPROC      = 4,
		WH_CBT              = 5,
		WH_SYSMSGFILTER     = 6,
		WH_MOUSE            = 7,
		WH_HARDWARE         = 8,
		WH_DEBUG            = 9,
		WH_SHELL            = 10,
		WH_FOREGROUNDIDLE   = 11,
		WH_CALLWNDPROCRET   = 12,
		WH_KEYBOARD_LL      = 13,
		WH_MOUSE_LL         = 14
	}
		  
	#endregion

	#region Mouse Hook Filters
	public enum MouseHookFilters
	{
		MSGF_DIALOGBOX      = 0,
		MSGF_MESSAGEBOX     = 1,
		MSGF_MENU           = 2,
		MSGF_SCROLLBAR      = 5,
		MSGF_NEXTWINDOW     = 6
	}

	#endregion

	#region Draw Text format flags
	public enum DrawTextFormatFlags
	{
		DT_TOP              = 0x00000000,
		DT_LEFT             = 0x00000000,
		DT_CENTER           = 0x00000001,
		DT_RIGHT            = 0x00000002,
		DT_VCENTER          = 0x00000004,
		DT_BOTTOM           = 0x00000008,
		DT_WORDBREAK        = 0x00000010,
		DT_SINGLELINE       = 0x00000020,
		DT_EXPANDTABS       = 0x00000040,
		DT_TABSTOP          = 0x00000080,
		DT_NOCLIP           = 0x00000100,
		DT_EXTERNALLEADING  = 0x00000200,
		DT_CALCRECT         = 0x00000400,
		DT_NOPREFIX         = 0x00000800,
		DT_INTERNAL         = 0x00001000,
		DT_EDITCONTROL      = 0x00002000,
		DT_PATH_ELLIPSIS    = 0x00004000,
		DT_END_ELLIPSIS     = 0x00008000,
		DT_MODIFYSTRING     = 0x00010000,
		DT_RTLREADING       = 0x00020000,
		DT_WORD_ELLIPSIS    = 0x00040000
	}

	#endregion

	#region Rebar Styles
	public enum RebarStyles
	{
		RBS_TOOLTIPS        = 0x0100,
		RBS_VARHEIGHT       = 0x0200,
		RBS_BANDBORDERS     = 0x0400,
		RBS_FIXEDORDER      = 0x0800,
		RBS_REGISTERDROP    = 0x1000,
		RBS_AUTOSIZE        = 0x2000,
		RBS_VERTICALGRIPPER = 0x4000, 
		RBS_DBLCLKTOGGLE    = 0x8000,
	}
	#endregion

	#region Rebar Notifications
	public enum RebarNotifications 
	{
		RBN_FIRST           = (0-831),
		RBN_HEIGHTCHANGE    = (RBN_FIRST - 0),
		RBN_GETOBJECT       = (RBN_FIRST - 1),
		RBN_LAYOUTCHANGED   = (RBN_FIRST - 2),
		RBN_AUTOSIZE        = (RBN_FIRST - 3),
		RBN_BEGINDRAG       = (RBN_FIRST - 4),
		RBN_ENDDRAG         = (RBN_FIRST - 5),
		RBN_DELETINGBAND    = (RBN_FIRST - 6),   
		RBN_DELETEDBAND     = (RBN_FIRST - 7),    
		RBN_CHILDSIZE       = (RBN_FIRST - 8),
		RBN_CHEVRONPUSHED   = (RBN_FIRST - 10)
	}
	#endregion

	#region Rebar Messages
	public enum RebarMessages
	{
		CCM_FIRST           =    0x2000,
		WM_USER             =    0x0400,
		RB_INSERTBANDA		=	(WM_USER +  1),
		RB_DELETEBAND       =	(WM_USER +  2),
		RB_GETBARINFO		=	(WM_USER +  3),
		RB_SETBARINFO		=	(WM_USER +  4),
		RB_GETBANDINFO		=	(WM_USER +  5),
		RB_SETBANDINFOA		=	(WM_USER +  6),
		RB_SETPARENT		=	(WM_USER +  7),
		RB_HITTEST			=	(WM_USER +  8),
		RB_GETRECT			=	(WM_USER +  9),
		RB_INSERTBANDW		=	(WM_USER +  10),
		RB_SETBANDINFOW		=	(WM_USER +  11),
		RB_GETBANDCOUNT		=	(WM_USER +  12),
		RB_GETROWCOUNT		=	(WM_USER +  13),
		RB_GETROWHEIGHT		=	(WM_USER +  14),
		RB_IDTOINDEX		=	(WM_USER +  16),
		RB_GETTOOLTIPS		=	(WM_USER +  17),
		RB_SETTOOLTIPS		=	(WM_USER +  18),
		RB_SETBKCOLOR		=	(WM_USER +  19),
		RB_GETBKCOLOR		=	(WM_USER +  20), 
		RB_SETTEXTCOLOR		=	(WM_USER +  21),
		RB_GETTEXTCOLOR		=	(WM_USER +  22),
		RB_SIZETORECT		=	(WM_USER +  23), 
		RB_SETCOLORSCHEME	=	(CCM_FIRST + 2),  
		RB_GETCOLORSCHEME	=	(CCM_FIRST + 3), 
		RB_BEGINDRAG		=	(WM_USER + 24),
		RB_ENDDRAG			=	(WM_USER + 25),
		RB_DRAGMOVE			=	(WM_USER + 26),
		RB_GETBARHEIGHT		=	(WM_USER + 27),
		RB_GETBANDINFOW		=	(WM_USER + 28),
		RB_GETBANDINFOA		=	(WM_USER + 29),
		RB_MINIMIZEBAND		=	(WM_USER + 30),
		RB_MAXIMIZEBAND		=	(WM_USER + 31),
		RB_GETDROPTARGET	=	(CCM_FIRST + 4),
		RB_GETBANDBORDERS	=	(WM_USER + 34),  
		RB_SHOWBAND			=	(WM_USER + 35),      
		RB_SETPALETTE		=	(WM_USER + 37),
		RB_GETPALETTE		=	(WM_USER + 38),
		RB_MOVEBAND			=	(WM_USER + 39),
		RB_SETUNICODEFORMAT =   (CCM_FIRST + 5),
		RB_GETUNICODEFORMAT =   (CCM_FIRST + 6)
	}
	#endregion

	#region Rebar Info Mask
	public enum RebarInfoMask
	{
		RBBIM_STYLE         = 0x00000001,
		RBBIM_COLORS        = 0x00000002,
		RBBIM_TEXT          = 0x00000004,
		RBBIM_IMAGE         = 0x00000008,
		RBBIM_CHILD         = 0x00000010,
		RBBIM_CHILDSIZE     = 0x00000020,
		RBBIM_SIZE          = 0x00000040,
		RBBIM_BACKGROUND    = 0x00000080,
		RBBIM_ID            = 0x00000100,
		RBBIM_IDEALSIZE     = 0x00000200,
		RBBIM_LPARAM        = 0x00000400,
		BBIM_HEADERSIZE     = 0x00000800  
	}
	#endregion

	#region Rebar Styles
	public enum RebarStylesEx
	{
		RBBS_BREAK			=	0x1,
		RBBS_CHILDEDGE		=	0x4,
		RBBS_FIXEDBMP		=	0x20,
		RBBS_GRIPPERALWAYS	=	0x80,
		RBBS_USECHEVRON		=	0x200
	}
	#endregion

	#region Object types
	public enum ObjectTypes
	{
		OBJ_PEN             = 1,
		OBJ_BRUSH           = 2,
		OBJ_DC              = 3,
		OBJ_METADC          = 4,
		OBJ_PAL             = 5,
		OBJ_FONT            = 6,
		OBJ_BITMAP          = 7,
		OBJ_REGION          = 8,
		OBJ_METAFILE        = 9,
		OBJ_MEMDC           = 10,
		OBJ_EXTPEN          = 11,
		OBJ_ENHMETADC       = 12,
		OBJ_ENHMETAFILE     = 13
	}
	#endregion

	#region WM_MENUCHAR return values
	public enum MenuCharReturnValues
	{
		MNC_IGNORE  = 0,
		MNC_CLOSE   = 1,
		MNC_EXECUTE = 2,
		MNC_SELECT  = 3
	}
	#endregion

	#region Background Mode
	public enum BackgroundMode
	{
		TRANSPARENT = 1,
		OPAQUE = 2
	}
	#endregion

	#region ListView Messages
	public enum ListViewMessages
	{
		LVM_FIRST           =    0x1000,
		LVM_GETSUBITEMRECT  = (LVM_FIRST + 56),
		LVM_GETITEMSTATE    = (LVM_FIRST + 44),
		LVM_GETITEMTEXTW    = (LVM_FIRST + 115)
	}
	#endregion

	#region Header Control Messages
	public enum HeaderControlMessages : int
	{
		HDM_FIRST        =  0x1200,
		HDM_GETITEMRECT  = (HDM_FIRST + 7),
		HDM_HITTEST      = (HDM_FIRST + 6),
		HDM_SETIMAGELIST = (HDM_FIRST + 8),
		HDM_GETITEMW     = (HDM_FIRST + 11),
		HDM_ORDERTOINDEX = (HDM_FIRST + 15)
	}
	#endregion

	#region Header Control Notifications
	public enum HeaderControlNotifications
	{
		HDN_FIRST       = (0-300),
		HDN_BEGINTRACKW = (HDN_FIRST-26),
		HDN_ENDTRACKW   = (HDN_FIRST-27),
		HDN_ITEMCLICKW  = (HDN_FIRST-22),
	}
	#endregion

	#region Header Control HitTest Flags
	public enum HeaderControlHitTestFlags : uint
	{
		HHT_NOWHERE             = 0x0001,
		HHT_ONHEADER            = 0x0002,
		HHT_ONDIVIDER           = 0x0004,
		HHT_ONDIVOPEN           = 0x0008,
		HHT_ABOVE               = 0x0100,
		HHT_BELOW               = 0x0200,
		HHT_TORIGHT             = 0x0400,
		HHT_TOLEFT              = 0x0800
	}
	#endregion

	#region List View sub item portion
	public enum SubItemPortion
	{
		LVIR_BOUNDS = 0,
		LVIR_ICON   = 1,
		LVIR_LABEL  = 2
	}
	#endregion

	#region Cursor Type
	public enum CursorType : uint
	{
		IDC_ARROW		= 32512U,
		IDC_IBEAM       = 32513U,
		IDC_WAIT        = 32514U,
		IDC_CROSS       = 32515U,
		IDC_UPARROW     = 32516U,
		IDC_SIZE        = 32640U,
		IDC_ICON        = 32641U,
		IDC_SIZENWSE    = 32642U,
		IDC_SIZENESW    = 32643U,
		IDC_SIZEWE      = 32644U,
		IDC_SIZENS      = 32645U,
		IDC_SIZEALL     = 32646U,
		IDC_NO          = 32648U,
		IDC_HAND        = 32649U,
		IDC_APPSTARTING = 32650U,
		IDC_HELP        = 32651U
	}
	#endregion
	
	#region Tracker Event Flags
	public enum TrackerEventFlags : uint
	{
		TME_HOVER	= 0x00000001,
		TME_LEAVE	= 0x00000002,
		TME_QUERY	= 0x40000000,
		TME_CANCEL	= 0x80000000
	}
	#endregion

	#region Mouse Activate Flags
	public enum MouseActivateFlags
	{
		MA_ACTIVATE			= 1,
		MA_ACTIVATEANDEAT   = 2,
		MA_NOACTIVATE       = 3,
		MA_NOACTIVATEANDEAT = 4
	}
	#endregion

	#region Dialog Codes
	public enum DialogCodes
	{
		DLGC_WANTARROWS			= 0x0001,
		DLGC_WANTTAB			= 0x0002,
		DLGC_WANTALLKEYS		= 0x0004,
		DLGC_WANTMESSAGE		= 0x0004,
		DLGC_HASSETSEL			= 0x0008,
		DLGC_DEFPUSHBUTTON		= 0x0010,
		DLGC_UNDEFPUSHBUTTON	= 0x0020,
		DLGC_RADIOBUTTON		= 0x0040,
		DLGC_WANTCHARS			= 0x0080,
		DLGC_STATIC				= 0x0100,
		DLGC_BUTTON				= 0x2000
	}
	#endregion

	#region Update Layered Windows Flags
	public enum UpdateLayeredWindowsFlags
	{
		ULW_COLORKEY = 0x00000001,
		ULW_ALPHA    = 0x00000002,
		ULW_OPAQUE   = 0x00000004
	}
	#endregion

	#region Alpha Flags
	public enum AlphaFlags : byte
	{
		AC_SRC_OVER  = 0x00,
		AC_SRC_ALPHA = 0x01
	}
	#endregion

	#region ComboBox messages
	public enum ComboBoxMessages
	{
		CB_GETDROPPEDSTATE = 0x0157
	}
	#endregion

	#region SetWindowLong indexes
	public enum SetWindowLongOffsets
	{
		GWL_WNDPROC     = (-4),
		GWL_HINSTANCE   = (-6),
		GWL_HWNDPARENT  = (-8),
		GWL_STYLE       = (-16),
		GWL_EXSTYLE     = (-20),
		GWL_USERDATA    = (-21),
		GWL_ID          = (-12)
	}
	#endregion

	#region TreeView Messages
	public enum TreeViewMessages
	{
		TV_FIRST        =  0x1100,
		TVM_GETITEMRECT = (TV_FIRST + 4),
		TVM_GETITEMW    = (TV_FIRST + 62)
	}
	#endregion

	#region TreeViewItem Flags
	public enum TreeViewItemFlags
	{
		TVIF_TEXT               = 0x0001,
		TVIF_IMAGE              = 0x0002,
		TVIF_PARAM              = 0x0004,
		TVIF_STATE              = 0x0008,
		TVIF_HANDLE             = 0x0010,
		TVIF_SELECTEDIMAGE      = 0x0020,
		TVIF_CHILDREN           = 0x0040,
		TVIF_INTEGRAL           = 0x0080
	}
	#endregion

	#region ListViewItem flags
	public enum ListViewItemFlags
	{
		LVIF_TEXT               = 0x0001,
		LVIF_IMAGE              = 0x0002,
		LVIF_PARAM              = 0x0004,
		LVIF_STATE              = 0x0008,
		LVIF_INDENT             = 0x0010,
		LVIF_NORECOMPUTE        = 0x0800
	}
	#endregion

	#region HeaderItem flags
	public enum HeaderItemFlags
	{
		HDI_WIDTH               = 0x0001,
		HDI_HEIGHT              = HDI_WIDTH,
		HDI_TEXT                = 0x0002,
		HDI_FORMAT              = 0x0004,
		HDI_LPARAM              = 0x0008,
		HDI_BITMAP              = 0x0010,
		HDI_IMAGE               = 0x0020,
		HDI_DI_SETITEM          = 0x0040,
		HDI_ORDER               = 0x0080
	}
	#endregion

	#region GetDCExFlags
	public enum GetDCExFlags
	{
		DCX_WINDOW           = 0x00000001,
		DCX_CACHE            = 0x00000002,
		DCX_NORESETATTRS     = 0x00000004,
		DCX_CLIPCHILDREN     = 0x00000008,
		DCX_CLIPSIBLINGS     = 0x00000010,
		DCX_PARENTCLIP       = 0x00000020,
		DCX_EXCLUDERGN       = 0x00000040,
		DCX_INTERSECTRGN     = 0x00000080,
		DCX_EXCLUDEUPDATE    = 0x00000100,
		DCX_INTERSECTUPDATE  = 0x00000200,
		DCX_LOCKWINDOWUPDATE = 0x00000400,
		DCX_VALIDATE         = 0x00200000
	}
	#endregion

	#region HitTest 
	public enum HitTest
	{
		HTERROR             = (-2),
		HTTRANSPARENT       = (-1),
		HTNOWHERE           =   0,
		HTCLIENT            =   1,
		HTCAPTION           =   2,
		HTSYSMENU           =   3,
		HTGROWBOX           =   4,
		HTSIZE              =   HTGROWBOX,
		HTMENU              =   5,
		HTHSCROLL           =   6,
		HTVSCROLL           =   7,
		HTMINBUTTON         =   8,
		HTMAXBUTTON         =   9,
		HTLEFT              =   10,
		HTRIGHT             =   11,
		HTTOP               =   12,
		HTTOPLEFT           =   13,
		HTTOPRIGHT          =   14,
		HTBOTTOM            =   15,
		HTBOTTOMLEFT        =   16,
		HTBOTTOMRIGHT       =   17,
		HTBORDER            =   18,
		HTREDUCE            =   HTMINBUTTON,
		HTZOOM              =   HTMAXBUTTON,
		HTSIZEFIRST         =   HTLEFT,
		HTSIZELAST          =   HTBOTTOMRIGHT,
		HTOBJECT            =   19,
		HTCLOSE             =   20,
		HTHELP              =   21
	}
	#endregion

	#region ActivateFlags
	public enum ActivateState
	{
		WA_INACTIVE     = 0,
		WA_ACTIVE       = 1,
		WA_CLICKACTIVE  = 2
	}
	#endregion

	#region StrechModeFlags
	public enum StrechModeFlags
	{
		BLACKONWHITE		= 1,
		WHITEONBLACK        = 2,
		COLORONCOLOR        = 3,
		HALFTONE            = 4,
		MAXSTRETCHBLTMODE   = 4
	}
	#endregion

	#region ScrollBarFlags
	public enum ScrollBarFlags
	{
		SBS_HORZ                    = 0x0000,
		SBS_VERT                    = 0x0001,
		SBS_TOPALIGN                = 0x0002,
		SBS_LEFTALIGN               = 0x0002,
		SBS_BOTTOMALIGN             = 0x0004,
		SBS_RIGHTALIGN              = 0x0004,
		SBS_SIZEBOXTOPLEFTALIGN     = 0x0002,
		SBS_SIZEBOXBOTTOMRIGHTALIGN = 0x0004,
		SBS_SIZEBOX                 = 0x0008,
		SBS_SIZEGRIP                = 0x0010
	}
	#endregion

	#region System Metrics Codes
	public enum SystemMetricsCodes
	{
		SM_CXSCREEN             = 0,
		SM_CYSCREEN             = 1,
		SM_CXVSCROLL            = 2,
		SM_CYHSCROLL            = 3,
		SM_CYCAPTION            = 4,
		SM_CXBORDER             = 5,
		SM_CYBORDER             = 6,
		SM_CXDLGFRAME           = 7,
		SM_CYDLGFRAME           = 8,
		SM_CYVTHUMB             = 9,
		SM_CXHTHUMB             = 10,
		SM_CXICON               = 11,
		SM_CYICON               = 12,
		SM_CXCURSOR             = 13,
		SM_CYCURSOR             = 14,
		SM_CYMENU               = 15,
		SM_CXFULLSCREEN         = 16,
		SM_CYFULLSCREEN         = 17,
		SM_CYKANJIWINDOW        = 18,
		SM_MOUSEPRESENT         = 19,
		SM_CYVSCROLL            = 20,
		SM_CXHSCROLL            = 21,
		SM_DEBUG                = 22,
		SM_SWAPBUTTON           = 23,
		SM_RESERVED1            = 24,
		SM_RESERVED2            = 25,
		SM_RESERVED3            = 26,
		SM_RESERVED4            = 27,
		SM_CXMIN                = 28,
		SM_CYMIN                = 29,
		SM_CXSIZE               = 30,
		SM_CYSIZE               = 31,
		SM_CXFRAME              = 32,
		SM_CYFRAME              = 33,
		SM_CXMINTRACK           = 34,
		SM_CYMINTRACK           = 35,
		SM_CXDOUBLECLK          = 36,
		SM_CYDOUBLECLK          = 37,
		SM_CXICONSPACING        = 38,
		SM_CYICONSPACING        = 39,
		SM_MENUDROPALIGNMENT    = 40,
		SM_PENWINDOWS           = 41,
		SM_DBCSENABLED          = 42,
		SM_CMOUSEBUTTONS        = 43,
		SM_CXFIXEDFRAME         = SM_CXDLGFRAME, 
		SM_CYFIXEDFRAME         = SM_CYDLGFRAME,  
		SM_CXSIZEFRAME          = SM_CXFRAME,    
		SM_CYSIZEFRAME          = SM_CYFRAME,    
		SM_SECURE               = 44,
		SM_CXEDGE               = 45,
		SM_CYEDGE               = 46,
		SM_CXMINSPACING         = 47,
		SM_CYMINSPACING         = 48,
		SM_CXSMICON             = 49,
		SM_CYSMICON             = 50,
		SM_CYSMCAPTION          = 51,
		SM_CXSMSIZE             = 52,
		SM_CYSMSIZE             = 53,
		SM_CXMENUSIZE           = 54,
		SM_CYMENUSIZE           = 55,
		SM_ARRANGE              = 56,
		SM_CXMINIMIZED          = 57,
		SM_CYMINIMIZED          = 58,
		SM_CXMAXTRACK           = 59,
		SM_CYMAXTRACK           = 60,
		SM_CXMAXIMIZED          = 61,
		SM_CYMAXIMIZED          = 62,
		SM_NETWORK              = 63,
		SM_CLEANBOOT            = 67,
		SM_CXDRAG               = 68,
		SM_CYDRAG               = 69,
		SM_SHOWSOUNDS           = 70,
		SM_CXMENUCHECK          = 71,  
		SM_CYMENUCHECK          = 72,
		SM_SLOWMACHINE          = 73,
		SM_MIDEASTENABLED       = 74,
		SM_MOUSEWHEELPRESENT    = 75,
		SM_XVIRTUALSCREEN       = 76,
		SM_YVIRTUALSCREEN       = 77,
		SM_CXVIRTUALSCREEN      = 78,
		SM_CYVIRTUALSCREEN      = 79,
		SM_CMONITORS            = 80,
		SM_SAMEDISPLAYFORMAT    = 81,
		SM_CMETRICS             = 83
	}
	#endregion

	#region ScrollBarTypes
	public enum ScrollBarTypes
	{
		SB_HORZ  = 0,
		SB_VERT  = 1,
		SB_CTL   = 2,
		SB_BOTH  = 3
	}
	#endregion

	#region SrollBarInfoFlags
	public enum ScrollBarInfoFlags
	{
		SIF_RANGE           = 0x0001,
		SIF_PAGE            = 0x0002,
		SIF_POS             = 0x0004,
		SIF_DISABLENOSCROLL = 0x0008,
		SIF_TRACKPOS        = 0x0010,
		SIF_ALL             = (SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS)
	}
	#endregion

	#region Enable ScrollBar flags
	public enum EnableScrollBarFlags
	{
		ESB_ENABLE_BOTH     = 0x0000,
		ESB_DISABLE_BOTH    = 0x0003,
		ESB_DISABLE_LEFT    = 0x0001,
		ESB_DISABLE_RIGHT   = 0x0002,
		ESB_DISABLE_UP      = 0x0001,
		ESB_DISABLE_DOWN    = 0x0002,
		ESB_DISABLE_LTUP    = ESB_DISABLE_LEFT,
		ESB_DISABLE_RTDN    = ESB_DISABLE_RIGHT
	}
	#endregion

	#region Scroll Requests
	public enum ScrollBarRequests
	{
		SB_LINEUP           = 0,
		SB_LINELEFT         = 0,
		SB_LINEDOWN         = 1,
		SB_LINERIGHT        = 1,
		SB_PAGEUP           = 2,
		SB_PAGELEFT         = 2,
		SB_PAGEDOWN         = 3,
		SB_PAGERIGHT        = 3,
		SB_THUMBPOSITION    = 4,
		SB_THUMBTRACK       = 5,
		SB_TOP              = 6,
		SB_LEFT             = 6,
		SB_BOTTOM           = 7,
		SB_RIGHT            = 7,
		SB_ENDSCROLL        = 8
	}
	#endregion

	#region SrollWindowEx flags
	public enum ScrollWindowExFlags
	{
		SW_SCROLLCHILDREN   = 0x0001,  
		SW_INVALIDATE       = 0x0002,  
		SW_ERASE            = 0x0004,  
		SW_SMOOTHSCROLL     = 0x0010  
	}
	#endregion

	#region ImageListFlags
	public enum  ImageListFlags
	{
		ILC_MASK             = 0x0001,
		ILC_COLOR            = 0x0000,
		ILC_COLORDDB         = 0x00FE,
		ILC_COLOR4           = 0x0004,
		ILC_COLOR8           = 0x0008,
		ILC_COLOR16          = 0x0010,
		ILC_COLOR24          = 0x0018,
		ILC_COLOR32          = 0x0020,
		ILC_PALETTE          = 0x0800      
	}
	#endregion

	#region List View Notifications
	public enum ListViewNotifications
	{
		LVN_FIRST             =  (0-100),
		LVN_GETDISPINFOW      = (LVN_FIRST-77),
		LVN_SETDISPINFOA      = (LVN_FIRST-51)
	}
	#endregion


}
