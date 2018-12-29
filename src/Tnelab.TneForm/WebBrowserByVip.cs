using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using static Tnelab.MiniBlinkV.NativeMethods;
using System.Linq;
using System.Threading.Tasks;

namespace Tnelab.HtmlView
{
    class WebBrowserByVip:IWebBrowser
    {
        string url_;
        public string Url
        {
            get
            {
                return url_;
            }
            set
            {
                url_ = value;
                if (webView_ != IntPtr.Zero && !string.IsNullOrEmpty(url_)) {
                    //var uri = new Uri(url_);
                    //var url = $"{uri.Scheme}://{System.Web.HttpUtility.UrlEncode(uri.Host)}{uri.PathAndQuery}";
                    mbLoadURL(webView_,url_);
                }
            }
        }
        public event EventHandler<string> TitleChanged ;
        public event EventHandler<JsQueryEventArgs> JsQuery;
        public IntPtr WebView { get => webView_; }
        static bool isInited_=false;
        public WebBrowserByVip(IntPtr parentHandle)
        {
            if (!isInited_)
            {
                var settings = new mbSettings();
                settings.mask = (uint)mbSettingMask.MB_SETTING_PAINTCALLBACK_IN_OTHER_THREAD;
                mbInit(ref settings);
                isInited_ = true;
            }

            this.parentHandle_ = parentHandle;
            var rect = new NativeMethods.RECT();
            NativeMethods.GetWindowRect(parentHandle, out rect);
            webView_ = mbCreateWebWindow(mbWindowType.MB_WINDOW_TYPE_TRANSPARENT, parentHandle, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
            this.paintUpdatedCallback_ = this.OnPaintCallback;
            mbOnPaintUpdated(webView_, this.paintUpdatedCallback_, IntPtr.Zero);
            this.titleChangedCallback_ = this.OnTitleChanged;
            mbOnTitleChanged(webView_,  this.titleChangedCallback_, IntPtr.Zero);
            this.loadUrlBeginCallback_ = this.OnLoadUrlBegin;
            mbOnLoadUrlBegin(webView_, this.loadUrlBeginCallback_, IntPtr.Zero);
            this.jsQueryCallback_ = this.OnJsQuery;
            mbOnJsQuery(webView_, this.jsQueryCallback_, IntPtr.Zero);
            this.consoleCallback_ = this.OnConsole;
            mbOnConsole(webView_, this.consoleCallback_, IntPtr.Zero);
        }
        List<Action> uiInvokeList = new List<Action>();
        object uiInvokeListLock_ = new object();
        public void UIInvoke(Action action)
        {
            lock (uiInvokeListLock_)
            {
                uiInvokeList.Add(action);
            }
            NativeMethods.PostMessageW(this.parentHandle_,NativeMethods.WM_UI_INVOKE,0,0);
        }
        public (int result,bool isHandle) ProcessWindowMessage(IntPtr hwnd, uint msg, uint wParam, uint lParam)
        {
            var isHandled = false;
            var result = 0;
            if (webView_ != IntPtr.Zero)
            {
                switch (msg)
                {
                    //case NativeMethods.WM_PAINT:
                    //    break;
                    case NativeMethods.WM_UI_INVOKE:
                        lock (uiInvokeListLock_)
                        {
                            foreach(var action in uiInvokeList)
                            {
                                action();
                            }
                            uiInvokeList.Clear();
                        }
                        break;
                    case NativeMethods.WM_SIZE:
                        {
                            var newWidth = NativeMethods.LOWORD(lParam);
                            var newHeight = NativeMethods.HIWORD(lParam);

                            mbResize(webView_,newWidth, newHeight);
                            isHandled = true;
                        }
                        break;
                    case NativeMethods.WM_MOUSEWHEEL:
                        {
                            OnMouseWheel(lParam, wParam);
                            isHandled = true;
                        }
                        break;
                    case NativeMethods.WM_LBUTTONDOWN:
                    case NativeMethods.WM_LBUTTONUP:
                    case NativeMethods.WM_MOUSEMOVE:
                    case NativeMethods.WM_RBUTTONDOWN:
                    case NativeMethods.WM_RBUTTONUP:
                        {
                            (result,isHandled) = OnMouseEvent(parentHandle_, msg, lParam, wParam);
                        }
                        break;
                    case NativeMethods.WM_KEYDOWN:
                        {
                            uint virtualKeyCode = wParam;
                            uint flags = 0;
                            if ((NativeMethods.HIWORD(lParam) & NativeMethods.KF_REPEAT) == NativeMethods.KF_REPEAT)
                                flags |= (uint)mbKeyFlags.MB_REPEAT;
                            if ((NativeMethods.HIWORD(lParam) & NativeMethods.KF_EXTENDED) == NativeMethods.KF_EXTENDED)
                                flags |= (uint)mbKeyFlags.MB_EXTENDED;
                            mbFireKeyDownEvent(webView_, virtualKeyCode, flags, false);
                            isHandled = true;
                        }
                        break;
                    case NativeMethods.WM_KEYUP:
                        {
                            uint virtualKeyCode = wParam;
                            uint flags = 0;
                            if ((NativeMethods.HIWORD(lParam) & NativeMethods.KF_REPEAT) == NativeMethods.KF_REPEAT)
                                flags |= (uint)mbKeyFlags.MB_REPEAT;
                            if ((NativeMethods.HIWORD(lParam) & NativeMethods.KF_EXTENDED) == NativeMethods.KF_EXTENDED)
                                flags |= (uint)mbKeyFlags.MB_EXTENDED;
                            isHandled = mbFireKeyUpEvent(webView_, virtualKeyCode, flags, false);
                        }
                        break;
                    case NativeMethods.WM_CHAR:
                        {
                            uint charCode = wParam;
                            uint flags = 0;
                            if ((NativeMethods.HIWORD(lParam) & NativeMethods.KF_REPEAT)==NativeMethods.KF_REPEAT)
                                flags |= (uint)mbKeyFlags.MB_REPEAT;
                            if ((NativeMethods.HIWORD(lParam) & NativeMethods.KF_EXTENDED) == NativeMethods.KF_EXTENDED)
                                flags |= (uint)mbKeyFlags.MB_EXTENDED;
                            isHandled=mbFireKeyPressEvent(webView_, charCode, flags, false);                            
                        }
                        break;                    
                    case NativeMethods.WM_SETCURSOR:
                        isHandled = mbFireWindowsMessage(webView_,hwnd, NativeMethods.WM_SETCURSOR, 0, 0, out var r);
                        break;
                    case NativeMethods.WM_SETFOCUS:
                        mbSetFocus(webView_);
                        isHandled = true;
                        break;
                    case NativeMethods.WM_KILLFOCUS:
                        mbKillFocus(webView_);
                        isHandled = true;
                        break;
                    case NativeMethods.WM_IME_STARTCOMPOSITION:
                        {
                            IntPtr rx;
                            mbFireWindowsMessage(webView_, this.parentHandle_, msg, wParam, lParam, out rx);
                            result = rx.ToInt32();
                            isHandled = true;
                        }
                        break;
                }
            }
            return (result,isHandled);
        }
        public void ResponseJsQuery(IntPtr webView,Int64 queryId,int customMsg,string response)
        {
            mbResponseQuery(webView, queryId, customMsg, response);
        }
        public string RunJs(string script)
        {
            var jv=MiniBlink.NativeMethods.wkeRunJS(WebView, script);
            var es = MiniBlink.NativeMethods.wkeGlobalExec(WebView);
            return MiniBlink.NativeMethods.jsToStringW(es, jv);
        }
        IntPtr webView_=IntPtr.Zero;
        IntPtr parentHandle_ = IntPtr.Zero;
        mbPaintUpdatedCallback paintUpdatedCallback_;
        mbTitleChangedCallback titleChangedCallback_;
        mbLoadUrlBeginCallback loadUrlBeginCallback_;
        mbJsQueryCallback jsQueryCallback_;
        mbConsoleCallback consoleCallback_;
        void OnMouseWheel(uint lParam, uint wParam)
        {
            var (x, y, delta, flags) = GetMouseMsgInfo(lParam, wParam);
            mbFireMouseWheelEvent(webView_,x, y, delta, flags);
        }
        (int result,bool isHandled) OnMouseEvent(IntPtr handle, uint msg, uint lParam, uint wParam)
        {
            var (x, y, delta, flags) = GetMouseMsgInfo(lParam, wParam);
            mbFireMouseEvent(webView_,msg, x, y, flags);
            return (0, true);
        }
        (int, int, int, uint) GetMouseMsgInfo(uint lParam, uint wParam)
        {
            var x = NativeMethods.LOWORD(lParam);
            var y = NativeMethods.HIWORD(lParam);
            var delta = NativeMethods.HIWORD(wParam);
            var flags = NativeMethods.LOWORD(wParam);
            return (x, y, delta, flags);
        }
        void OnPaintCallback(IntPtr webView, IntPtr param, IntPtr hdc, int x, int y, int cx, int cy)
        {
            var rect = new NativeMethods.RECT();
            NativeMethods.GetWindowRect(parentHandle_, out rect);
            var winHdc = NativeMethods.GetDC(parentHandle_);
            NativeMethods.SIZE size = new NativeMethods.SIZE();
            size.cx = rect.right - rect.left;
            size.cy = rect.bottom - rect.top;
            NativeMethods.POINT point = new NativeMethods.POINT();
            point.x = rect.left;
            point.y = rect.top;
            NativeMethods.POINT point2 = new NativeMethods.POINT();
            point2.x = 0;
            point2.x = 0;
            NativeMethods.BLENDFUNCTION blInfo = new NativeMethods.BLENDFUNCTION();
            blInfo.BlendOp = NativeMethods.AC_SRC_OVER;
            blInfo.BlendFlags = 0;
            blInfo.AlphaFormat = NativeMethods.AC_SRC_ALPHA;
            blInfo.SourceConstantAlpha = 0xFF;

            NativeMethods.RECT rectDirty = new NativeMethods.RECT();
            rectDirty.left = x;
            rectDirty.right = x + cx;
            rectDirty.top = y;
            rectDirty.bottom = y + cy;
            NativeMethods.UPDATELAYEREDWINDOWINFO ulwInfo = new NativeMethods.UPDATELAYEREDWINDOWINFO();
            unsafe
            {                
                ulwInfo.cbSize = Marshal.SizeOf<NativeMethods.UPDATELAYEREDWINDOWINFO>();
                ulwInfo.hdcDst = winHdc;
                ulwInfo.pptDst = &point;
                ulwInfo.psize = &size;
                ulwInfo.pptSrc = &point2;
                ulwInfo.hdcSrc = hdc;
                ulwInfo.crKey = 0;
                ulwInfo.dwFlags = NativeMethods.ULW_ALPHA;
                ulwInfo.pblend = &blInfo;
                ulwInfo.prcDirty = &rectDirty;
            }
            //NativeMethods.UpdateLayeredWindow(parentHandle_, winHdc, point, size, hdc, point2, 0, blInfo, NativeMethods.ULW_ALPHA);
            NativeMethods.UpdateLayeredWindowIndirect(parentHandle_, ulwInfo);
            NativeMethods.ReleaseDC(this.parentHandle_, winHdc);
        }
        void OnTitleChanged(IntPtr webView, IntPtr param, string title)
        {            
            if (this.TitleChanged != null)
            {
                this.TitleChanged(this, title);
            }
        }
        bool OnLoadUrlBegin(IntPtr webView, IntPtr param, string url, IntPtr job)
        {
            url = System.Web.HttpUtility.UrlDecode(url);
            var uri = new Uri(url);
            if (uri.Scheme.ToLower() == "tne")
            {
                var path = $"{uri.AbsolutePath}".Replace("/",".").ToLower();
                Assembly currentAssembly = Assembly.Load(uri.Host);
                byte[] bytes;
                var names = currentAssembly.GetManifestResourceNames();
                path = names.Single(it => it.ToLower().EndsWith(path));
                using (var stream = currentAssembly.GetManifestResourceStream(path))
                {
                    bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, bytes.Length);
                }
                var buff = Marshal.AllocHGlobal(bytes.Length);
                Marshal.Copy(bytes, 0, buff, bytes.Length);
                mbNetSetData(job, buff, bytes.Length);
                Marshal.FreeHGlobal(buff);
                return true;
            }
            else
            {
                return false;
            }
        }
        object jsqueryLock_ = new object();
        void OnJsQuery(IntPtr webView, IntPtr param, IntPtr es, Int64 queryId, int customMsg, string request)
        {
            if (this.JsQuery != null)
            {
                var args = new JsQueryEventArgs();
                args.WebView = webView;
                args.Param = param;
                args.ES = es;
                args.QueryId = queryId;
                args.CustomMsg = customMsg;
                args.Request = request;
                Task.Factory.StartNew(() =>
                {
                    lock (jsqueryLock_)
                    {
                        JsQuery(this, args);
                    }
                });                
            }            
        }
        void OnConsole(IntPtr webView, IntPtr param, mbConsoleLevel level, string message, string sourceName, uint sourceLine, string stackTrace)
        {
        }
    }
}
