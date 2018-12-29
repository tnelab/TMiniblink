using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using static Tnelab.MiniBlinkV.NativeMethods;

namespace Tnelab.HtmlView
{
    public enum StartPosition { Manual, CenterScreen, CenterParent }
    public enum WindowState { Maximized, Normal , Minimized }
    public sealed class TneForm
    {        
        public IntPtr Handle { get; internal set; }
        string title_ = "TneForm";
        public string Title {
            get {
                return title_;
            }
            private set {
                title_ = value;
                if (this.Handle != IntPtr.Zero)
                {
                    NativeMethods.SetWindowTextW(this.Handle,value);
                }
            }
        }
        public bool SizeAble { get; set; } = true;
        int x_ = 0;
        public int X {
            get
            {
                if (this.Handle != IntPtr.Zero)
                    x_= GetWindowRect().left;
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
                if(this.Handle!=IntPtr.Zero)
                    y_= GetWindowRect().top;
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
                    width_= rect.right - rect.left;
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
                    height_= rect.bottom - rect.top;
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
        public int MinWidth { get; set; } = 0;
        public int MinHeight { get; set; } = 0;
        string url_;
        public string Url {
            get =>url_;
            set
            {
                if (webBrowser_ != null)
                {
                    webBrowser_.Url = value;
                }
                url_ = value;
            }
        }
        public StartPosition StartPosition { get; set; } = StartPosition.CenterParent;
        WindowState WindowState_= WindowState.Normal;
        public WindowState WindowState {
            get =>WindowState_;
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
        public TneForm()
        {
            this.WindowIndex_ = NewWindowIndex();
            winProcDelegate_ = this.WinProc;
            var className = $"TneForm{WindowIndex_}";
            NativeMethods.WNDCLASS wcex = new NativeMethods.WNDCLASS();
            wcex.style = NativeMethods.CS_HREDRAW | NativeMethods.CS_VREDRAW | NativeMethods.CS_OWNDC;
            wcex.lpfnWndProc = Marshal.GetFunctionPointerForDelegate(winProcDelegate_);
            wcex.lpszClassName = className;
            wcex.hInstance = System.Diagnostics.Process.GetCurrentProcess().Handle;
            var result = NativeMethods.RegisterClassW(ref wcex);
            if (result == IntPtr.Zero)
            {
                throw new Exception($"注册窗口类失败,错误代码:{Marshal.GetLastWin32Error()}");
            }
        }
        public void Close()
        {
            if (this.IsDesdroyed_)
                throw new Exception("窗口已经被释放");
            if (this.Handle == IntPtr.Zero)
                CreateWindow();
            if (this.Parent != null)
            {                
                NativeMethods.EnableWindow(this.Parent.Handle, true);
                NativeMethods.SetActiveWindow(this.Parent.Handle);
            }
            NativeMethods.CloseWindow(this.Handle);
            NativeMethods.DestroyWindow(this.Handle);
        }
        public void ShowDialog() { }
        public void Show() {
            if(this.Handle == IntPtr.Zero)
                CreateWindow();

            if (this.Parent != null)
            {
                NativeMethods.EnableWindow(this.Parent.Handle, false);
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
            if (this.Handle == IntPtr.Zero)
                CreateWindow();
            NativeMethods.ReleaseCapture();
            NativeMethods.SendMessage(this.Handle, NativeMethods.WM_SYSCOMMAND, NativeMethods.SC_MOVE | NativeMethods.HTCAPTION, 0);
        }
        bool isBorderRect_ = false;
        int WinProc(IntPtr hWnd,uint message,uint wParam,uint lParam)
        {
            if (webBrowser_ == null)
            {
                TneApplication.ReadOnlyVipFlag = true;
                if (TneApplication.IsVip) {
                    webBrowser_ = new WebBrowserByVip(hWnd);
                }
                else
                {
                    webBrowser_ = new WebBrowserByWke(hWnd);
                }
                JsNativeMaper.This.AddBrowser(this.webBrowser_, () => this);
                webBrowser_.TitleChanged += (sender, args) =>
                {
                    this.Title = args;
                };
                
                this.Url = this.Url;
            }
            var result = 0;
            var isHandled = false;
            if (message == NativeMethods.WM_SETCURSOR&&isBorderRect_)
            {
                result = NativeMethods.DefWindowProcW(hWnd, message, wParam, lParam);
                isHandled = true;
            }
            
            if(webBrowser_!=null&&!isHandled)
                (result, isHandled) = webBrowser_.ProcessWindowMessage(hWnd, message, wParam, lParam);
            if (webBrowser_==null||!isHandled)
            {
                switch (message)
                {
                    case NativeMethods.WM_NCHITTEST:
                        if (SizeAble)
                        {
                            result = GetBorderRect(lParam);
                            isBorderRect_ = result != 0;
                        }
                        if(!isBorderRect_)
                        {
                            result = NativeMethods.DefWindowProcW(hWnd, message, wParam, lParam);
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
                        if (this == TneApplication.MainForm)
                            NativeMethods.PostQuitMessage(0);
                        else
                            result = NativeMethods.DefWindowProcW(hWnd, message, wParam, lParam);
                        this.IsDesdroyed_ = true;
                        break;
                    case NativeMethods.WM_MOVE:
                        //break;
                    default:
                        result = NativeMethods.DefWindowProcW(hWnd, message, wParam, lParam);
                        break;
                }
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
        IWebBrowser webBrowser_;
        void CreateWindow()
        {
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
            var result = NativeMethods.CreateWindowExW(NativeMethods.WS_EX_LAYERED, this.ClassName_, this.Title, NativeMethods.WS_POPUP, this.X, this.Y, this.Width, this.Height, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            if (result == IntPtr.Zero)
            {
                throw new Exception($"创建窗口失败,错误代码:{Marshal.GetLastWin32Error()}");
            }
            this.Handle = result;
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
            JsNativeMaper.This.RemoveBrowser(this.webBrowser_);
        }
    }
}
