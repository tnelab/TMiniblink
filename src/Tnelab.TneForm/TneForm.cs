//tne://to_ts
//namespace:TMiniblink
//base:Tnelab.TneFormBase
//import:./TneMap.ts
//import:./TneFormBase.ts
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using static Tnelab.MiniBlinkV.NativeMethods;
using System.Reflection;
using System.Linq;

namespace Tnelab.HtmlView
{
    public enum StartPosition { Manual=1, CenterScreen=2, CenterParent=3 }
    public enum WindowState { Maximized=1, Normal=2 , Minimized=3 }
    public sealed class TneForm
    {
        public IntPtr Handle { get; internal set; }
        public event EventHandler<DragFilesEventArgs> DragFilesEvent;
        string title_ = "TneForm";
        internal bool IsMenuForm { get; set; } = false;
        internal event EventHandler<EventArgs> KillFocus;
        public string Title {
            get {
                return title_;
            }
            private set {
                title_ = value;
                if (this.Handle != IntPtr.Zero)
                {
                    NativeMethods.SetWindowTextW(this.Handle, value);
                }
            }
        }
        public bool SizeAble { get; set; } = true;
        int x_ = 0;
        public int X {
            get
            {
                if (this.Handle != IntPtr.Zero)
                    x_ = GetWindowRect().left;
                return x_;
            }
            set {
                if (this.Handle != IntPtr.Zero)
                {
                    var rect = GetWindowRect();
                    NativeMethods.MoveWindow(this.Handle, value, rect.top, rect.right - rect.left, rect.bottom - rect.top, true);
                }
                x_ = value;
            }
        }
        int y_ = 0;
        public int Y {
            get
            {
                if (this.Handle != IntPtr.Zero)
                    y_ = GetWindowRect().top;
                return y_;
            }
            set
            {
                if (this.Handle != IntPtr.Zero)
                {
                    var rect = GetWindowRect();
                    NativeMethods.MoveWindow(this.Handle, rect.left, value, rect.right - rect.left, rect.bottom - rect.top, true);
                }
                y_ = value;
            }
        }
        int width_ = 1024;
        public int Width {
            get
            {
                if (this.Handle != IntPtr.Zero)
                {
                    var rect = GetWindowRect();
                    width_ = rect.right - rect.left;
                }
                return width_;
            }
            set
            {
                if (this.Handle != IntPtr.Zero)
                {
                    var rect = GetWindowRect();
                    NativeMethods.MoveWindow(this.Handle, rect.left, rect.top, value, rect.bottom - rect.top, true);
                }
                width_ = value;
            }
        }
        int height_ = 768;
        public int Height {
            get
            {
                if (this.Handle != IntPtr.Zero)
                {
                    var rect = GetWindowRect();
                    height_ = rect.bottom - rect.top;
                }
                return height_;
            }
            set
            {
                if (this.Handle != IntPtr.Zero)
                {
                    var rect = GetWindowRect();
                    NativeMethods.MoveWindow(this.Handle, rect.left, rect.top, rect.right - rect.left, value, true);
                }
                height_ = value;
            }
        }
        bool showInTaskBar_ = true;
        public bool ShowInTaskBar {
            get {
                return showInTaskBar_;
            }
            set
            {
                showInTaskBar_ = value;
                if (this.Handle != IntPtr.Zero)
                {
                    var old = NativeMethods.GetWindowLong(this.Handle, NativeMethods.GWL_EXSTYLE);
                    if (value)
                    {
                        NativeMethods.SetWindowLong(this.Handle, NativeMethods.GWL_EXSTYLE, old & ~NativeMethods.WS_EX_TOOLWINDOW);
                    }
                    else
                    {
                        NativeMethods.SetWindowLong(this.Handle, NativeMethods.GWL_EXSTYLE, old | NativeMethods.WS_EX_TOOLWINDOW);
                    }
                }
            }
        }
        bool topMost_ = false;
        public bool TopMost
        {
            get
            {
                return this.topMost_;
            }
            set
            {
                this.topMost_ = value;
                if (this.Handle != IntPtr.Zero)
                {
                    if (this.topMost_)
                    {
                        NativeMethods.SetWindowPos(this.Handle, new IntPtr(-1), 0, 0, 0, 0, 0x0002 | 0x0001);
                    }
                    else
                    {
                        NativeMethods.SetWindowPos(this.Handle, new IntPtr(-2), 0, 0, 0, 0, 0x0002 | 0x0001);
                    }
                }
            }
        }
        public int MinWidth { get; set; } = 0;
        public int MinHeight { get; set; } = 0;
        string url_;
        public string Url {
            get => url_;
            private set
            {
                if (WebBrowser != null)
                {
                    TneApplication.UIInvoke(() => {
                        WebBrowser.Url = value;
                    });
                }
                url_ = value;
            }
        }
        public StartPosition StartPosition { get; set; } = StartPosition.CenterParent;
        WindowState WindowState_ = WindowState.Normal;
        public WindowState WindowState {
            get => WindowState_;
            set {
                if (this.IsDesdroyed_)
                    throw new Exception("窗口已经被释放");
                if (this.Handle != IntPtr.Zero) {
                    switch (value)
                    {
                        case WindowState.Maximized:
                            NativeMethods.ShowWindow(this.Handle, NativeMethods.SW_MAXIMIZE);
                            break;
                        case WindowState.Normal:
                            NativeMethods.ShowWindow(this.Handle, NativeMethods.SW_NORMAL);
                            break;
                        case WindowState.Minimized:
                            NativeMethods.ShowWindow(this.Handle, NativeMethods.SW_MINIMIZE);
                            break;
                    }
                }
                this.WindowState_ = value;
            }
        }
        TneForm parent_;
        public TneForm Parent
        {
            get
            {
                return parent_;
            }
            set
            {
                this.parent_ = value;
            }
        }
        NativeMethods.WinProcDelegate winProcDelegate_;
        public TneForm(string url)
        {
            this.WindowIndex_ = NewWindowIndex();
            this.Url = url;
        }
        public void Close()
        {
            TneApplication.UIInvoke(() => {
                if (this.IsDesdroyed_)
                    throw new Exception("窗口已经被释放");
                if (this.Handle == IntPtr.Zero)
                    throw new Exception("窗口还为创建");
                //NativeMethods.CloseWindow(this.Handle);
                //NativeMethods.DestroyWindow(this.Handle);
                this.WebBrowser.Destroy();
                NativeMethods.PostMessageW(this.Handle, NativeMethods.WM_CLOSE, 0, 0);
            });
        }
        bool isStartDialog_ = false;
        public void ShowDialog() {
            if (this.Handle == IntPtr.Zero)
            {
                CreateWindow();
            }
            this.WindowState = this.WindowState;
            if (this.Parent != null)
            {
                NativeMethods.EnableWindow(Parent.Handle, false);
            }
            if (TneApplication.IsVip)
            {
                this.isStartDialog_ = true;
                while (isStartDialog_)
                {
                    TneApplication.DoEvent();
                }
            }
        }
        public void Show() {
            if (this.Handle == IntPtr.Zero)
            {
                CreateWindow();
            }
            this.WindowState = this.WindowState;
        }
        public void Hide() {
            if (this.IsDesdroyed_)
                throw new Exception("窗口已经被释放");
            if (this.Handle == IntPtr.Zero)
                CreateWindow();
            NativeMethods.ShowWindow(this.Handle, NativeMethods.SW_HIDE);
        }
        public void Move()
        {
            //WindowState_ = WindowState.Normal;
            if (this.Handle == IntPtr.Zero)
                CreateWindow();
            NativeMethods.ReleaseCapture();
            NativeMethods.PostMessageW(this.Handle, NativeMethods.WM_SYSCOMMAND, NativeMethods.SC_MOVE | NativeMethods.HTCAPTION, 0);
        }
        bool allowDrop_ = false;
        public bool AllowDrop
        {
            get
            {
                return allowDrop_;
            }
            set
            {
                allowDrop_ = value;
                if (this.Handle != IntPtr.Zero)
                {
                    NativeMethods.DragAcceptFiles(this.Handle, value);
                }
            }
        }
        public void Active()
        {
            if (this.Handle == IntPtr.Zero)
            {
                CreateWindow();
            }
            this.WindowState = WindowState.Normal;
            NativeMethods.SetActiveWindow(this.Handle);
            NativeMethods.SetForegroundWindow(this.Handle);
        }

        string icon_;
        public string Icon
        {
            get
            {
                return icon_;
            }
            set
            {
                icon_ = value;
                if (this.Handle!=IntPtr.Zero)
                {
                    var assembly = Assembly.GetEntryAssembly();
                    var names = assembly.GetManifestResourceNames();
                    var path = names.Single(it => it.ToLower().EndsWith(icon_.ToLower()));
                    using (var stream = assembly.GetManifestResourceStream(path))
                    {
                        var datas = new byte[stream.Length];
                        stream.Read(datas, 0, datas.Length);
                        var ptr = Marshal.AllocHGlobal((int)stream.Length);
                        Marshal.Copy(datas, 0, ptr, datas.Length);
                        var hIcon = NativeMethods.CreateIconFromResourceEx(ptr, datas.Length, 1, 0x30000, 32, 32, NativeMethods.LR_DEFAULTCOLOR);
                        NativeMethods.SendMessageW(this.Handle, NativeMethods.WM_SETICON, NativeMethods.ICON_BIG, (uint)hIcon);
                        NativeMethods.SendMessageW(this.Handle, NativeMethods.WM_SETICON, NativeMethods.ICON_SMALL, (uint)hIcon);
                        Marshal.FreeHGlobal(ptr);
                    }
                }
            }
        }
        bool isBorderRect_ = false;
        int WinProc(IntPtr hWnd,uint message,uint wParam,uint lParam)
        {
            var result = 0;
            var isHandled = false;
            if (message == NativeMethods.WM_NCHITTEST && SizeAble)
            {
                result = GetBorderRect(lParam);
                isBorderRect_ = result != 0;
                if (isBorderRect_)
                {
                    return result;
                }
            }
            if (message == NativeMethods.WM_SETCURSOR && isBorderRect_)
            {
                result=NativeMethods.DefWindowProcW(hWnd, message, wParam, lParam);
                isBorderRect_ = false;
                return result;
            }
            if (WebBrowser != null)
            {
                (result, isHandled) = WebBrowser.ProcessWindowMessage(hWnd, message, wParam, lParam);
                if (isHandled)
                    return result;
            }

            switch (message)
            {
                //case NativeMethods.WM_SETFOCUS:
                //    NativeMethods.SetFocus(hWnd);
                //    isHandled = false;
                //    break;
                case NotifyIcon.WM_NOTIFYICON:
                    {
                        var notifyicon = NotifyIcon.GetById((int)wParam);
                        switch (lParam)
                        {
                            case NativeMethods.WM_LBUTTONDOWN:
                                notifyicon.OnClick(this, new EventArgs());
                                break;
                            case NativeMethods.WM_RBUTTONDOWN:
                                notifyicon.OnContextMenu(this,new EventArgs());
                                break;
                        }
                    }
                    break;
                case NativeMethods.WM_KILLFOCUS:
                    {
                        this.KillFocus ?.Invoke(this,new EventArgs());
                    }
                    break;
                case NativeMethods.WM_SETTINGCHANGE:
                    if (NativeMethods.IsZoomed(hWnd))
                    {
                        this.Hide();
                        this.WindowState = WindowState.Maximized;
                    }
                    break;
                case NativeMethods.WM_GETMINMAXINFO:
                    WmGetMinMaxInfo(hWnd, lParam);
                    break;
                case NativeMethods.WM_DESTROY:
                    if (isStartDialog_)
                    {
                        this.isStartDialog_ = false;
                    }
                    if (this.Parent != null)
                    {
                        NativeMethods.EnableWindow(this.Parent.Handle, true);
                        //NativeMethods.SetForegroundWindow(this.Parent.Handle);
                    }
                    if (this == TneApplication.MainForm)
                        NativeMethods.PostQuitMessage(0);
                    else
                        result = NativeMethods.DefWindowProcW(hWnd, message, wParam, lParam);
                    this.IsDesdroyed_ = true;
                    JsNativeMaper.This.RemoveBrowser(this.WebBrowser);
                    break;
                case NativeMethods.WM_DROPFILES:
                    if (this.DragFilesEvent != null)
                    {
                        var args = new DragFilesEventArgs();
                        var hDrag = new IntPtr(wParam);
                        var count = NativeMethods.DragQueryFileW(hDrag, 0xFFFFFFFF, IntPtr.Zero, 0);
                        args.Files = new string[count];
                        var tmp = Marshal.AllocHGlobal(255*2);
                        for (var i = 0; i < args.Files.Length; i++)
                        {
                            NativeMethods.DragQueryFileW(hDrag, (uint)i, tmp, 255*2);
                            args.Files[i] = Marshal.PtrToStringUni(tmp);
                        }
                        Marshal.FreeHGlobal(tmp);
                        NativeMethods.POINT point = new NativeMethods.POINT();
                        NativeMethods.DragQueryPoint(hDrag, ref point);
                        args.X = point.x;
                        args.Y = point.y;
                        this.DragFilesEvent(this, args);
                        NativeMethods.DragFinish(hDrag);
                    }
                    break;
                default:
                    result = NativeMethods.DefWindowProcW(hWnd, message, wParam, lParam);
                    break;
            }

            return result;
        }
        static long WindowIndexSeed_ = 0;
        static long NewWindowIndex()=>WindowIndexSeed_++;
        NativeMethods.RECT GetWindowRect()
        {
            var rect = new NativeMethods.RECT();
            NativeMethods.GetWindowRect(this.Handle,out rect);
            return rect;
        }
        long WindowIndex_ { get; set; }
        string ClassName_ { get => $"TneForm{this.WindowIndex_}"; }
        bool IsDesdroyed_ = false;
        internal IWebBrowser WebBrowser { get; private set; }
        void CreateWindow()
        {
            TneApplication.UIInvoke(() =>
            {
                winProcDelegate_ = this.WinProc;
                var className = $"TneForm{WindowIndex_}";
                NativeMethods.WNDCLASS wcex = new NativeMethods.WNDCLASS();
                wcex.style = NativeMethods.CS_HREDRAW | NativeMethods.CS_VREDRAW | NativeMethods.CS_OWNDC;
                wcex.lpfnWndProc = Marshal.GetFunctionPointerForDelegate(winProcDelegate_);
                wcex.lpszClassName = className;
                wcex.hInstance = System.Diagnostics.Process.GetCurrentProcess().Handle;

                if (!string.IsNullOrEmpty(this.Icon))
                {
                    var assembly = Assembly.GetEntryAssembly();
                    var names = assembly.GetManifestResourceNames();
                    var path = names.Single(it => it.ToLower().EndsWith(icon_.ToLower()));
                    using (var stream = assembly.GetManifestResourceStream(path))
                    {
                        var datas = new byte[stream.Length];
                        stream.Read(datas, 0, datas.Length);
                        var ptr = Marshal.AllocHGlobal((int)stream.Length);
                        Marshal.Copy(datas, 0, ptr, datas.Length);
                        var hIcon = NativeMethods.CreateIconFromResourceEx(ptr, datas.Length, 1, 0x30000, 32, 32, NativeMethods.LR_DEFAULTCOLOR);
                        wcex.hIcon = hIcon;
                        Marshal.FreeHGlobal(ptr);
                    }
                }
                wcex.hCursor = NativeMethods.LoadCursorW(IntPtr.Zero, 32512);
                var result = NativeMethods.RegisterClassW(ref wcex);
                if (result == IntPtr.Zero)
                {
                    throw new Exception($"注册窗口类失败,错误代码:{Marshal.GetLastWin32Error()}");
                }


                if (this.StartPosition == StartPosition.CenterScreen || (this.StartPosition == StartPosition.CenterParent && this.Parent == null))
                {
                    var scrWidth = NativeMethods.GetSystemMetrics(NativeMethods.SM_CXSCREEN);
                    var scrHeight = NativeMethods.GetSystemMetrics(NativeMethods.SM_CYSCREEN);

                    this.X = (scrWidth - this.Width) / 2;
                    this.Y = (scrHeight - this.Height) / 2;
                }
                else if (this.StartPosition == StartPosition.CenterParent)
                {
                    this.X = (Parent.Width - this.Width) / 2 + this.Parent.X;
                    this.Y = (Parent.Height - this.Height) / 2 + this.Parent.Y;
                }
                var exStyle = NativeMethods.WS_EX_LAYERED;
                if (!this.showInTaskBar_)
                    exStyle |= NativeMethods.WS_EX_TOOLWINDOW;
                result = NativeMethods.CreateWindowExW((uint)exStyle, this.ClassName_, this.Title, NativeMethods.WS_POPUP, this.X, this.Y, this.Width, this.Height, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                if (result == IntPtr.Zero)
                {
                    throw new Exception($"创建窗口失败,错误代码:{Marshal.GetLastWin32Error()}");
                }
                NativeMethods.DragAcceptFiles(result, this.AllowDrop);
                if (WebBrowser == null)
                {
                    
                    TneApplication.ReadOnlyVipFlag = true;
                    if (TneApplication.IsVip)
                    {
                        WebBrowser = new WebBrowserByVip(result);
                    }
                    else
                    {
                        WebBrowser = new WebBrowserByWke(result);
                    }
                    JsNativeMaper.This.AddBrowser(this.WebBrowser, () => this);
                    WebBrowser.TitleChanged += (sender, args) =>
                    {
                        this.Title = args;
                    };

                    this.Url = this.Url;
                }                
                this.Handle = result;
                this.TopMost = this.TopMost;
            });
        }
        int GetBorderRect(uint lParam)
        {
            if (!NativeMethods.IsZoomed(this.Handle))
            {
                var x = NativeMethods.LOWORD(lParam);
                var y = NativeMethods.HIWORD(lParam);
                NativeMethods.RECT rect = new NativeMethods.RECT();
                NativeMethods.GetWindowRect(this.Handle, out rect);
                var borderWidth = 6;
                //窗口左上角
                if (x > rect.left && x < rect.left + borderWidth && y > rect.top && y < rect.top + borderWidth)
                {
                    return (int)NativeMethods.HitTest.HTTOPLEFT;
                }
                //窗口左下角
                else if (x > rect.left && x < rect.left + borderWidth && y > rect.bottom - borderWidth && y < rect.bottom)
                {
                    return (int)NativeMethods.HitTest.HTBOTTOMLEFT;
                }
                //窗口右上角
                else if (x > rect.right-borderWidth && x < rect.right && y > rect.top && y < rect.top+borderWidth)
                {
                    return (int)NativeMethods.HitTest.HTTOPRIGHT;
                }
                //窗口右下角
                else if (x > rect.right - borderWidth && x < rect.right && y > rect.bottom-borderWidth && y < rect.bottom)
                {
                    return (int)NativeMethods.HitTest.HTBOTTOMRIGHT;
                }
                //窗口左侧
                else if (x > rect.left  && x < rect.left + borderWidth && y > rect.top + borderWidth && y < rect.bottom-borderWidth)
                {
                    return (int)NativeMethods.HitTest.HTLEFT;
                }
                //窗口右侧
                else if (x > rect.right-borderWidth && x < rect.right && y > rect.top + borderWidth && y < rect.bottom - borderWidth)
                {
                    return (int)NativeMethods.HitTest.HTRIGHT;
                }
                //窗口上侧
                else if (x > rect.left+borderWidth && x < rect.right-borderWidth && y > rect.top && y < rect.top + borderWidth)
                {
                    return (int)NativeMethods.HitTest.HTTOP;
                }
                //窗口下侧
                else if (x > rect.left + borderWidth && x < rect.right-borderWidth && y > rect.bottom - borderWidth && y < rect.bottom)
                {
                    return (int)NativeMethods.HitTest.HTBOTTOM;
                }
            }
            return 0;
        }
        void WmGetMinMaxInfo(IntPtr hwnd, uint lParam)
        {
            NativeMethods.MINMAXINFO mmi = (NativeMethods.MINMAXINFO)Marshal.PtrToStructure((IntPtr)lParam, typeof(NativeMethods.MINMAXINFO));

            int MONITOR_DEFAULTTONEAREST = 0x00000002;
            IntPtr monitor = NativeMethods.MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

            if (monitor != IntPtr.Zero)
            {
                NativeMethods.MONITORINFO monitorInfo = new NativeMethods.MONITORINFO();
                NativeMethods.GetMonitorInfo(monitor, monitorInfo);
                NativeMethods.RECT rcWorkArea = monitorInfo.rcWork;
                NativeMethods.RECT rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
                mmi.ptMinTrackSize.x = this.MinWidth;
                mmi.ptMinTrackSize.y = this.MinHeight;
            }

            Marshal.StructureToPtr(mmi, (IntPtr)lParam, true);
            
        }
        ~TneForm()
        {
            if (this.Handle != IntPtr.Zero)
            {
                NativeMethods.CloseWindow(this.Handle);
                NativeMethods.DestroyWindow(this.Handle);
            }
            JsNativeMaper.This.RemoveBrowser(this.WebBrowser);
        }
    }
    public class DragFilesEventArgs : EventArgs
    {
        public string[] Files { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
