using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Tnelab.HtmlView
{
    using BOOL = Boolean;
    using HWND = IntPtr;
    using LPMSG = IntPtr;
    using UINT = UInt32;
    using WPARAM = UInt32;
    using LPARAM = UInt32;
    using DWORD = Int32;
    using LONG = Int32;
    using LRESULT = Int32;
    using LPCTSTR = String;
    using HMENU = IntPtr;
    using HANDLE = IntPtr;
    using HDC = IntPtr;
    using COLORREF = Int32;
    using BYTE = Byte;
    using HCURSOR = IntPtr;
    using HINSTANCE = IntPtr;
    using LPCSTR = String;
    using HMODULE = IntPtr;
    using ATOM = IntPtr;
    using LPCWSTR = String;
    using HICON = IntPtr;
    using HBRUSH = IntPtr;
    using PBYTE = IntPtr;
    using HDROP = IntPtr;
    using LPWSTR = IntPtr;
    using PVOID = IntPtr;

    [SuppressUnmanagedCodeSecurity]
    static partial class NativeMethods
    {
        [UnmanagedFunctionPointer(CallingConvention.Winapi,SetLastError =true)]
        public delegate int WinProcDelegate(IntPtr hWnd, uint message, uint wParam, uint lParam);
        public enum HitTest : int                   //测试句柄
        {
            #region 测试句柄

            HTERROR = -2,
            HTTRANSPARENT = -1,
            HTNOWHERE = 0,
            HTCLIENT = 1,
            HTCAPTION = 2,
            HTSYSMENU = 3,
            HTGROWBOX = 4,
            HTSIZE = HTGROWBOX,
            HTMENU = 5,
            HTHSCROLL = 6,
            HTVSCROLL = 7,
            HTMINBUTTON = 8,
            HTMAXBUTTON = 9,
            HTLEFT = 10,
            HTRIGHT = 11,
            HTTOP = 12,
            HTTOPLEFT = 13,
            HTTOPRIGHT = 14,
            HTBOTTOM = 15,
            HTBOTTOMLEFT = 16,
            HTBOTTOMRIGHT = 17,
            HTBORDER = 18,
            HTREDUCE = HTMINBUTTON,
            HTZOOM = HTMAXBUTTON,
            HTSIZEFIRST = HTLEFT,
            HTSIZELAST = HTBOTTOMRIGHT,
            HTOBJECT = 19,
            HTCLOSE = 20,
            HTHELP = 21

            #endregion
        }
        [StructLayout(LayoutKind.Sequential,CharSet = CharSet.Unicode)]
        public struct WNDCLASS
        {
            public uint style;
            public IntPtr lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            public IntPtr lpszMenuName;
            public string lpszClassName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe class UPDATELAYEREDWINDOWINFO
        {
            public DWORD cbSize;
            public HDC hdcDst;
            public POINT* pptDst;
            public SIZE* psize;
            public HDC hdcSrc;
            public POINT* pptSrc;
            public COLORREF crKey;
            public BLENDFUNCTION* pblend;
            public DWORD dwFlags;
            public RECT* prcDirty;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct POINT
        {
            public LONG x;
            public LONG y;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct MSG
        {
            public HWND hwnd;
            public UINT message;
            public WPARAM wParam;
            public LPARAM lParam;
            public DWORD time;
            public POINT pt;
            public DWORD lPrivate;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct RECT
        {
            public LONG left;
            public LONG top;
            public LONG right;
            public LONG bottom;
        }

        #region Nested type: MINMAXINFO
        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }
        #endregion

        #region Nested type: MONITORINFO
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            public RECT rcMonitor;
            public RECT rcWork;
            public int dwFlags;
        }
        #endregion
        [StructLayout(LayoutKind.Sequential)]
        public struct SIZE
        {
            public Int32 cx;
            public Int32 cy;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct BLENDFUNCTION
        {
            public BYTE BlendOp;
            public BYTE BlendFlags;
            public BYTE SourceConstantAlpha;
            public BYTE AlphaFormat;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct COMPOSITIONFORM
        {
            public int dwStyle;
            public POINT ptCurrentPos;
            public RECT rcArea;
        }
        public const Int32 ULW_COLORKEY = 0x00000001;
        public const Int32 ULW_ALPHA = 0x00000002;
        public const Int32 ULW_OPAQUE = 0x00000004;

        public const byte AC_SRC_OVER = 0x00;
        public const byte AC_SRC_ALPHA = 0x01;

        public const byte CFS_DEFAULT = 0x0;
        public const byte CFS_RECT = 0x1;
        public const byte CFS_POINT = 0x2;
        public const byte CFS_SCREEN = 0x4;
        public const byte CFS_FORCE_POSITION = 0x20;
        public const byte CFS_CANDIDATEPOS = 0x40;
        public const byte CFS_EXCLUDE = 0x80;

        public const int GWL_STYLE = (-16);
        public const int GWL_EXSTYLE = (-20);
        public const int LWA_ALPHA = 0;

        public const int CS_VREDRAW=          0x0001;
        public const int CS_HREDRAW =          0x0002;
        public const int CS_DBLCLKS =          0x0008;
        public const int CS_OWNDC =            0x0020;
        public const int CS_CLASSDC =          0x0040;
        public const int CS_PARENTDC =         0x0080;
        public const int CS_NOCLOSE =          0x0200;
        public const int CS_SAVEBITS =         0x0800;
        public const int CS_BYTEALIGNCLIENT =  0x1000;
        public const int CS_BYTEALIGNWINDOW =  0x2000;
        public const int CS_GLOBALCLASS =      0x4000;

        public const int WS_OVERLAPPED=       0x000000;
        public const uint WS_POPUP=           0x80000000;
        public const int WS_CHILD=            0x400000;
        public const int WS_MINIMIZE=         0x200000;
        public const int WS_VISIBLE=          0x100000;
        public const int WS_DISABLED=         0x080000;
        public const int WS_CLIPSIBLINGS=     0x040000;
        public const int WS_CLIPCHILDREN=     0x020000;
        public const int WS_MAXIMIZE=         0x010000;
        public const int WS_CAPTION=          0x00C000;     /* WS_BORDER | WS_DLGFRAME  */
        public const int WS_BORDER=           0x008000;
        public const int WS_DLGFRAME=         0x004000;
        public const int WS_VSCROLL=          0x002000;
        public const int WS_HSCROLL=          0x001000;
        public const int WS_SYSMENU=          0x000800;
        public const int WS_THICKFRAME=       0x000400;
        public const int WS_GROUP=            0x000200;
        public const int WS_TABSTOP=          0x000100;

        public const int WS_MINIMIZEBOX=      0x000200;
        public const int WS_MAXIMIZEBOX=      0x000100;
        public const int WS_OVERLAPPEDWINDOW = (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX);

        public const int WS_EX_LAYERED=           0x00080000;
        public const int WS_EX_TRANSPARENT = 0x20;
        public const int WS_EX_APPWINDOW = 0x00040000;
        public const int WS_EX_TOOLWINDOW = 0x00000080;


        /*
         * ShowWindow() Commands
        */
        public const int SW_HIDE=             0;
        public const int SW_SHOWNORMAL=       1;
        public const int SW_NORMAL=           1;
        public const int SW_SHOWMINIMIZED=    2;
        public const int SW_SHOWMAXIMIZED=    3;
        public const int SW_MAXIMIZE=         3;
        public const int SW_SHOWNOACTIVATE=   4;
        public const int SW_SHOW=             5;
        public const int SW_MINIMIZE=         6;
        public const int SW_SHOWMINNOACTIVE=  7;
        public const int SW_SHOWNA=           8;
        public const int SW_RESTORE=          9;
        public const int SW_SHOWDEFAULT=      10;
        public const int SW_FORCEMINIMIZE=    11;
        public const int SW_MAX=              11;

        public const int SM_CXSCREEN = 0;
        public const int SM_CYSCREEN = 1;

        /*
        * Window Messages
        */
        public const uint WM_USER = 0x0400;
        public const uint WM_UI_INVOKE = WM_USER + 1;
        public const uint WM_NULL=                         0x0000;
        public const uint WM_CREATE=                       0x0001;
        public const uint WM_DESTROY=                      0x0002;
        public const uint WM_MOVE=                         0x0003;
        public const uint WM_SIZE=                         0x0005;

        public const uint WM_ACTIVATE=                     0x0006;
        public const uint WM_SHOWWINDOW=                   0x0018;
        public const uint WM_CLOSE=                        0x0010;
        public const uint WM_NCDESTROY = 0x0082;
        public const uint WM_GETMINMAXINFO = 0x0024;
        public const uint WM_IME_STARTCOMPOSITION = 0x010d;
        public const uint WM_MOUSEMOVE = 0x0200;
        public const uint WM_RBUTTONDOWN = 0x0204;
        public const uint WM_RBUTTONUP = 0x0205;
        public const uint WM_KEYDOWN = 0x0100;
        public const uint WM_KEYUP = 0x0101;
        public const uint WM_CHAR = 0x0102;
        public const uint WM_IME_SETCONTEXT = 0x0281;
        public const uint WM_IME_NOTIFY = 0x0282;

        public const uint WM_SETCURSOR = 0x0020;
        public const uint WM_SETFOCUS = 0x0007;
        public const uint WM_KILLFOCUS = 0x0008;

        public const uint WM_MOUSEWHEEL = 0x020a;
        public const uint WM_LBUTTONDOWN = 0x0201;
        public const uint WM_LBUTTONUP = 0x0202;
        public const uint WM_SYSCOMMAND = 0x0112;
        public const uint WM_SIZING = 0x0214;
        public const uint WM_SETTINGCHANGE = 0x001A;
        public const int WM_NCHITTEST = 0x0084;    //测试消息
        public const int WM_PAINT = 0x000F;

        public const int WM_MBUTTONDOWN = 0x0207;
        public const int WM_MBUTTONUP = 0x0208;
        public const int WM_DROPFILES =                   0x0233;
        public const int WM_CONTEXTMENU=                  0x007B;

        public const int CW_USEDEFAULT = unchecked((int)0x80000000);

        public const int SC_MOVE = 0xF012;

        public const int SC_MAXIMIZE = 61488;
        public const int SC_MINIMIZE = 61472;

        public const int SRCCOPY = 0xCC0020;
        public const int HTCAPTION = 2;   //标题栏
        public const DWORD CAPTUREBLT = 0x40000000;

        public const int KF_EXTENDED = 0x0100;
        public const int KF_DLGMODE = 0x0800;
        public const int KF_MENUMODE = 0x1000;
        public const int KF_ALTDOWN = 0x2000;
        public const int KF_REPEAT = 0x4000;
        public const int KF_UP = 0x8000;

        public const int WM_GETICON = 0x007F;
        public const int WM_SETICON = 0x0080;
        public const int ICON_SMALL=          0;
        public const int ICON_BIG=            1;


        public const int LR_DEFAULTCOLOR=     0x00000000;
        public const int LR_MONOCHROME=       0x00000001;
        public const int LR_COLOR=            0x00000002;
        public const int LR_COPYRETURNORG=    0x00000004;
        public const int LR_COPYDELETEORG=    0x00000008;
        public const int LR_LOADFROMFILE=     0x00000010;
        public const int LR_LOADTRANSPARENT=  0x00000020;
        public const int LR_DEFAULTSIZE=      0x00000040;
        public const int LR_VGACOLOR=         0x00000080;
        public const int LR_LOADMAP3DCOLORS=  0x00001000;
        public const int LR_CREATEDIBSECTION= 0x00002000;
        public const int LR_COPYFROMRESOURCE= 0x00004000;
        public const int LR_SHARED=           0x00008000;

        public const int MK_LBUTTON = 0x0001;
        public const int MK_RBUTTON = 0x0002;
        public const int MK_SHIFT = 0x0004;
        public const int MK_CONTROL = 0x0008;
        public const int MK_MBUTTON = 0x0010;
        public const int SPI_GETWORKAREA = 0x0030;



        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        public static extern DWORD GetLastError();
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi,SetLastError =true,CharSet =CharSet.Unicode, ExactSpelling=true)]
        public static extern ATOM RegisterClassW(ref WNDCLASS Arg1);
        [DllImport("user32.dll", SetLastError = true,CallingConvention =CallingConvention.Winapi,CharSet =CharSet.Unicode)]
        public static extern IntPtr CreateWindowExW(
            UInt32 dwExStyle,
            string lpClassName,
            string lpWindowName,
            UInt32 dwStyle,
            Int32 x,
            Int32 y,
            Int32 nWidth,
            Int32 nHeight,
            IntPtr hWndParent,
            IntPtr hMenu,
            IntPtr hInstance,
            IntPtr lpParam
        );
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern BOOL ShowWindow(HWND hWnd, int nCmdShow);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern BOOL GetMessage(ref MSG lpMsg,HWND hWnd,UINT wMsgFilterMin,UINT wMsgFilterMax);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern BOOL TranslateMessage(ref MSG lpMsg);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern LRESULT DispatchMessage(ref MSG lpMsg);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern LRESULT DefWindowProcW(HWND hWnd,UINT Msg,WPARAM wParam,LPARAM lParam);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern void PostQuitMessage(int nExitCode);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern int GetSystemMetrics(int nIndex);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern BOOL GetWindowRect(HWND hWnd,out RECT lpRect);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern BOOL GetClientRect(HWND hWnd,out RECT lpRect);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern BOOL MoveWindow(HWND hWnd,int X,int Y,int nWidth,int nHeight,BOOL bRepaint);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern BOOL SetWindowPos(HWND hWnd,HWND hWndInsertAfter,int X,int Y,int cx,int cy,UINT uFlags);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern HWND GetParent(HWND hWnd);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern HWND SetParent(HWND hWndChild, HWND hWndNewParent);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern BOOL CloseWindow(HWND hWnd);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern BOOL EnableWindow(HWND hWnd,BOOL bEnable);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern HWND SetActiveWindow(HWND hWnd);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern BOOL DestroyWindow(HWND hWnd);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern int SetWindowLong(HWND hWnd,int nIndex,int dwNewLong);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern int GetWindowLong(HWND hWnd,int nIndex);
        [DllImport("user32", CallingConvention = CallingConvention.Winapi)]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);
        [DllImport("User32", CallingConvention = CallingConvention.Winapi)]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern int ReleaseCapture();
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern int SendMessageW(IntPtr hWnd, uint Msg, uint wParam, uint lParam);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern BOOL UpdateLayeredWindow(HWND hWnd, HDC hdcDst, POINT pptDst, SIZE psize, HDC hdcSrc, POINT pptSrc, COLORREF crKey, BLENDFUNCTION pblend, DWORD dwFlags);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern BOOL UpdateLayeredWindowIndirect(HWND hwnd, UPDATELAYEREDWINDOWINFO pULWInfo);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern HDC GetDC(HWND hWnd);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern BOOL UpdateWindow(HWND hWnd);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern BOOL IsZoomed(HWND hWnd);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern HCURSOR SetCursor(HCURSOR hCursor);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern HCURSOR LoadCursorA(HINSTANCE hInstance,int lpCursorName);
        [DllImport("Gdi32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern BOOL BitBlt(HDC hdc,  int x,  int y,  int cx,  int cy,  HDC hdcSrc,  int x1,  int y1,  DWORD rop);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern int ReleaseDC(HWND hWnd,HDC hDC);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi,CharSet =CharSet.Unicode)]
        [return:MarshalAs(UnmanagedType.Bool)]
        public static extern BOOL SetWindowTextW(HWND hWnd,[MarshalAs(UnmanagedType.LPWStr)]LPCWSTR lpString);
        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        public static extern HMODULE LoadLibraryA(LPCSTR lpLibFileName);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        public static extern BOOL PostMessageW(HWND hWnd,UINT Msg,WPARAM wParam,LPARAM lParam);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi,SetLastError =true)]
        public static extern HICON CreateIconFromResourceEx(
          PBYTE presbits,
          DWORD dwResSize,
          int fIcon,
          DWORD dwVer,
          int cxDesired,
          int cyDesired,
          UINT Flags
        );
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern HWND SetFocus(HWND hWnd);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern HWND SetCapture(HWND hwnd);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern BOOL GetCursorPos(ref POINT lpPoint);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern BOOL ScreenToClient(HWND hWnd, ref POINT lpPoint);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern BOOL PtInRect(ref RECT lprc,POINT pt);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true,CharSet =CharSet.Unicode)]
        public static extern HCURSOR LoadCursorW(HINSTANCE hInstance,uint lpCursorName);
        [DllImport("user32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern BOOL SetForegroundWindow(HWND hWnd);
        [DllImport("Shell32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern void DragAcceptFiles(HWND hWnd,BOOL fAccept);
        [DllImport("Shell32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern void DragFinish(HDROP hDrop);
        [DllImport("Shell32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern UINT DragQueryFileW(HDROP hDrop,UINT iFile,IntPtr lpszFile,UINT cch);
        [DllImport("Shell32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern BOOL DragQueryPoint(HDROP hDrop,ref POINT ppt);
        [DllImport("User32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern BOOL SystemParametersInfoW(UINT uiAction,UINT uiParam,PVOID pvParam,UINT fWinIni);
        [DllImport("User32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern HWND GetForegroundWindow();
        public static ushort LOWORD(uint value)
        {
            return (ushort)(value & 0xFFFF);
        }
        public static ushort HIWORD(uint value)
        {
            return (ushort)(value >> 16);
        }
        public static byte LOWBYTE(ushort value)
        {
            return (byte)(value & 0xFF);
        }
        public static byte HIGHBYTE(ushort value)
        {
            return (byte)(value >> 8);
        }
        //static NativeMethods()
        //{
        //    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        //}
    }
}
