using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using static Tnelab.MiniBlink.NativeMethods;
using System.Linq;
using System.Threading.Tasks;

namespace Tnelab.HtmlView
{
    class WebBrowserByWke:IWebBrowser
    {
        static wkeJsNativeFunction jsOnQueryFunction_;
        static long idSeed_ = 0;
        static long jsOnQueryFunction(IntPtr es,IntPtr param)
        {
            if (jsQuery != null)
            {
                JsQueryEventArgs args = new JsQueryEventArgs();
                args.CustomMsg = jsToInt(es,jsArg(es, 0));
                args.Request = jsToString(es, jsArg(es, 1));
                args.QueryId = idSeed_;
                args.WebView = jsGetWebView(es);                
                args.Func=jsArg(es, 2);
                args.ES = wkeGlobalExec(args.WebView);
                idSeed_++;                
                jsQuery(jsGetWebView(es), args);
            }
            return jsUndefined();
        }
        static event EventHandler<JsQueryEventArgs> jsQuery;
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
                    wkeLoadURL(webView_,url_);
                }
            }
        }
        public event EventHandler<string> TitleChanged ;
        public event EventHandler<JsQueryEventArgs> JsQuery;
        public IntPtr WebView { get => webView_; }
        public WebBrowserByWke(IntPtr parentHandle)
        {
            if (!wkeIsInitialize())
            {
                wkeInit();               
                jsOnQueryFunction_ = jsOnQueryFunction;
                wkeJsBindFunction("mbQuery", jsOnQueryFunction_, IntPtr.Zero, 3);
            }            
            this.parentHandle_ = parentHandle;
            var rect = new NativeMethods.RECT();
            NativeMethods.GetWindowRect(parentHandle, out rect);
            //zmg
            webView_ = wkeCreateWebView();// (wkeWindowType.WKE_WINDOW_TYPE_TRANSPARENT, parentHandle, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);            
            wkeSetHandle(webView_,parentHandle);
            wkeSetTransparent(webView_, true);
            wkeResize(webView_,rect.right-rect.left,rect.bottom-rect.top);

            this.paintUpdatedCallback_ = this.OnPaintCallback;
            wkeOnPaintUpdated(webView_, this.paintUpdatedCallback_, IntPtr.Zero);
            this.titleChangedCallback_ = this.OnTitleChanged;
            wkeOnTitleChanged(webView_,  this.titleChangedCallback_, IntPtr.Zero);
            this.loadUrlBeginCallback_ = this.OnLoadUrlBegin;
            wkeOnLoadUrlBegin(webView_, this.loadUrlBeginCallback_, IntPtr.Zero);
            this.jsQueryCallback_ = this.OnJsQuery;
            jsQuery += this.jsQueryCallback_;
            //wkeOnJsQuery(webView_, this.jsQueryCallback_, IntPtr.Zero);
            this.consoleCallback_ = this.OnConsole;
            wkeOnConsole(webView_, this.consoleCallback_, IntPtr.Zero);
        }
        public void UIInvoke(Action action)
        {
            TneApplication.UIInvoke(action);
        }
        public (int result,bool isHandle) ProcessWindowMessage(IntPtr hwnd, uint msg, uint wParam, uint lParam)
        {
            if (this.webView_ == IntPtr.Zero)
                return (0, false);
            var isHandled = false;
            var result = 0;
            if (webView_ == IntPtr.Zero)
            {
                return (result, isHandled);
            }
            switch (msg)
            {
                case NativeMethods.WM_SIZE:
                    {
                        var newWidth = NativeMethods.LOWORD(lParam);
                        var newHeight = NativeMethods.HIWORD(lParam);

                        wkeResize(webView_, newWidth, newHeight);
                        isHandled = true;
                    }
                    break;
                //case NativeMethods.WM_CONTEXTMENU:
                //    {
                //        var (x, y, delta, flags) = GetMouseMsgInfo(lParam, wParam);
                //        //wkeFireContextMenuEvent(this.webView_, x, y, flags);
                //        isHandled = true;
                //    }
                //    break;
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
                case NativeMethods.WM_MBUTTONDOWN:
                case NativeMethods.WM_MBUTTONUP:
                    {
                        onCursorChange();

                        //if (msg == NativeMethods.WM_LBUTTONDOWN || msg == NativeMethods.WM_MBUTTONDOWN || msg == NativeMethods.WM_RBUTTONDOWN)
                        //{
                        //    NativeMethods.SetFocus(hwnd);
                        //    NativeMethods.SetCapture(hwnd);
                        //}
                        //else if (msg == NativeMethods.WM_LBUTTONUP || msg == NativeMethods.WM_MBUTTONUP || msg == NativeMethods.WM_RBUTTONUP)
                        //{
                        //    NativeMethods.ReleaseCapture();
                        //}
                        var (x, y, delta, flags) = GetMouseMsgInfo(lParam, wParam);
                        wkeFireMouseEvent(webView_, msg, x, y, flags);
                        if (msg == NativeMethods.WM_RBUTTONUP)
                        {
                            isHandled = false;
                        }
                        else
                        {
                            isHandled = true;
                        }
                    }
                    break;
                case NativeMethods.WM_KEYDOWN:
                    {
                        uint virtualKeyCode = wParam;
                        uint flags = 0;
                        if ((NativeMethods.HIWORD(lParam) & NativeMethods.KF_REPEAT) == NativeMethods.KF_REPEAT)
                            flags |= (uint)wkeKeyFlags.REPEAT;
                        if ((NativeMethods.HIWORD(lParam) & NativeMethods.KF_EXTENDED) == NativeMethods.KF_EXTENDED)
                            flags |= (uint)wkeKeyFlags.EXTENDED;
                        wkeFireKeyDownEvent(webView_, virtualKeyCode, flags, false);
                        isHandled = true;
                    }
                    break;
                case NativeMethods.WM_KEYUP:
                    {
                        uint virtualKeyCode = wParam;
                        uint flags = 0;
                        if ((NativeMethods.HIWORD(lParam) & NativeMethods.KF_REPEAT) == NativeMethods.KF_REPEAT)
                            flags |= (uint)wkeKeyFlags.REPEAT;
                        if ((NativeMethods.HIWORD(lParam) & NativeMethods.KF_EXTENDED) == NativeMethods.KF_EXTENDED)
                            flags |= (uint)wkeKeyFlags.EXTENDED;
                        wkeFireKeyUpEvent(webView_, virtualKeyCode, flags, false);
                        isHandled = true;
                    }
                    break;
                case NativeMethods.WM_CHAR:
                    {
                        uint charCode = wParam;
                        uint flags = 0;
                        if ((NativeMethods.HIWORD(lParam) & NativeMethods.KF_REPEAT) == NativeMethods.KF_REPEAT)
                            flags |= (uint)wkeKeyFlags.REPEAT;
                        if ((NativeMethods.HIWORD(lParam) & NativeMethods.KF_EXTENDED) == NativeMethods.KF_EXTENDED)
                            flags |= (uint)wkeKeyFlags.EXTENDED;
                        wkeFireKeyPressEvent(webView_, charCode, flags, false);
                        isHandled = true;
                    }
                    break;
                case NativeMethods.WM_SETCURSOR:
                    //isHandled = wkeFireWindowsMessage(webView_, hwnd, NativeMethods.WM_SETCURSOR, 0, 0, out var r);
                    //isHandled = true;
                    setCursorInfoTypeByCache(hwnd);
                    isHandled = true;
                    break;
                case NativeMethods.WM_SETFOCUS:
                    wkeSetFocus(WebView);
                    isHandled = true;
                    break;
                case NativeMethods.WM_KILLFOCUS:
                    wkeKillFocus(WebView);
                    isHandled = false;
                    break;
                case NativeMethods.WM_IME_STARTCOMPOSITION:
                    {
                        IntPtr rx;
                        wkeFireWindowsMessage(webView_, this.parentHandle_, msg, wParam, lParam, out rx);
                        result = rx.ToInt32();
                        isHandled = true;
                    }
                    break;
            }
            return (result,isHandled);
        }
        public void ResponseJsQuery(IntPtr webView,Int64 queryId,int customMsg,string response)
        {
            response = TneEncoder.Escape(response);
            var args = queryMap_[queryId];
            queryMap_.Remove(queryId);
            var tnelab = jsGetGlobal(args.ES, "Tnelab");
            var func = args.Func;
            jsCall(args.ES, func, tnelab, new[] { jsInt(customMsg), jsStringW(args.ES, response) }, 2);
        }
        public string RunJs(string script)
        {
            var jv = MiniBlink.NativeMethods.wkeRunJS(WebView, script);
            var es = MiniBlink.NativeMethods.wkeGlobalExec(WebView);
            var val = MiniBlink.NativeMethods.jsToString(es, jv);
            return val;
        }
        public void JsExecStateInvoke(Action<IntPtr> action)
        {
            var es = wkeGlobalExec(this.WebView);
            action(es);
        }
        public void Destroy()
        {
            this.webView_ = IntPtr.Zero;            
        }
        IntPtr webView_=IntPtr.Zero;
        IntPtr parentHandle_ = IntPtr.Zero;
        wkePaintUpdatedCallback paintUpdatedCallback_;
        wkeTitleChangedCallback titleChangedCallback_;
        wkeLoadUrlBeginCallback loadUrlBeginCallback_;
        EventHandler<JsQueryEventArgs> jsQueryCallback_;
        wkeConsoleCallback consoleCallback_;       
        void OnMouseWheel(uint lParam, uint wParam)
        {
            var (x, y, delta, flags) = GetMouseMsgInfo(lParam, wParam);
            wkeFireMouseWheelEvent(webView_,x, y, delta, flags);
        }
        (int, int, int, uint) GetMouseMsgInfo(uint lParam, uint wParam)
        {
            var x = NativeMethods.LOWORD(lParam);
            var y = NativeMethods.HIWORD(lParam);
            var delta = NativeMethods.HIWORD(wParam);
            var flags_ = NativeMethods.LOWORD(wParam);
            uint flags = 0;
            if ((wParam & NativeMethods.MK_CONTROL) > 0)
                flags |= (int)wkeMouseFlags.CONTROL;
            if ((wParam & NativeMethods.MK_SHIFT) > 0)
                flags |= (int)wkeMouseFlags.SHIFT;
            if ((wParam & NativeMethods.MK_LBUTTON) > 0)
                flags |= (int)wkeMouseFlags.LBUTTON;
            if ((wParam & NativeMethods.MK_MBUTTON) > 0)
                flags |= (int)wkeMouseFlags.MBUTTON;
            if ((wParam & NativeMethods.MK_RBUTTON) > 0)
                flags |= (int)wkeMouseFlags.RBUTTON;
            return (x, y, delta, flags);
        }
        WkeCursorInfoType cursorInfoType_;
        void onCursorChange()
        {
            cursorInfoType_ = (WkeCursorInfoType)wkeGetCursorInfoType(WebView);
        }

        bool setCursorInfoTypeByCache(IntPtr hWnd)
        {
            NativeMethods.RECT rc;
            NativeMethods.GetClientRect(hWnd, out rc);
            NativeMethods.POINT pt=new NativeMethods.POINT();
            NativeMethods.GetCursorPos(ref pt);
            NativeMethods.ScreenToClient(hWnd, ref pt);
            if (!NativeMethods.PtInRect(ref rc, pt))
                return false;

            IntPtr hCur = IntPtr.Zero;
            switch (cursorInfoType_)
            {
                case WkeCursorInfoType.CursorInfoPointer:
                    hCur = NativeMethods.LoadCursorW(IntPtr.Zero, 32512);//
                    break;
                case WkeCursorInfoType.CursorInfoIBeam:
                    hCur = NativeMethods.LoadCursorW(IntPtr.Zero, 32513);
                    break;
                case WkeCursorInfoType.CursorInfoHand:
                    hCur = NativeMethods.LoadCursorW(IntPtr.Zero, 32649);
                    break;
                case WkeCursorInfoType.CursorInfoWait:
                    hCur = NativeMethods.LoadCursorW(IntPtr.Zero, 32514);
                    break;
                case WkeCursorInfoType.CursorInfoHelp:
                    hCur = NativeMethods.LoadCursorW(IntPtr.Zero, 32651);
                    break;
                case WkeCursorInfoType.CursorInfoEastResize:
                    hCur = NativeMethods.LoadCursorW(IntPtr.Zero, 32644);
                    break;
                case WkeCursorInfoType.CursorInfoNorthResize:
                    hCur = NativeMethods.LoadCursorW(IntPtr.Zero, 32645);
                    break;
                case WkeCursorInfoType.CursorInfoSouthWestResize:
                case WkeCursorInfoType.CursorInfoNorthEastResize:
                    hCur = NativeMethods.LoadCursorW(IntPtr.Zero, 32643);
                    break;
                case WkeCursorInfoType.CursorInfoSouthResize:
                case WkeCursorInfoType.CursorInfoNorthSouthResize:
                    hCur = NativeMethods.LoadCursorW(IntPtr.Zero, 32645);
                    break;
                case WkeCursorInfoType.CursorInfoNorthWestResize:
                case WkeCursorInfoType.CursorInfoSouthEastResize:
                    hCur = NativeMethods.LoadCursorW(IntPtr.Zero, 32642);
                    break;
                case WkeCursorInfoType.CursorInfoWestResize:
                case WkeCursorInfoType.CursorInfoEastWestResize:
                    hCur = NativeMethods.LoadCursorW(IntPtr.Zero, 32644);
                    break;
                case WkeCursorInfoType.CursorInfoNorthEastSouthWestResize:
                case WkeCursorInfoType.CursorInfoNorthWestSouthEastResize:
                    hCur = NativeMethods.LoadCursorW(IntPtr.Zero, 32646);
                    break;
                default:
                    hCur = NativeMethods.LoadCursorW(IntPtr.Zero, 32512);
                    break;
            }

            if (hCur!=IntPtr.Zero)
            {
                NativeMethods.SetCursor(hCur);
                return true;
            }

            return false;
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
        void OnTitleChanged(IntPtr webView, IntPtr param, IntPtr title)
        {            
            if (this.TitleChanged != null)
            {
                this.TitleChanged(this, Marshal.PtrToStringUni(wkeToStringW(title)));
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
                wkeNetSetData(job, buff, bytes.Length);
                Marshal.FreeHGlobal(buff);
                return true;
            }
            else
            {
                return false;
            }
        }
        object jsqueryLock_ = new object();
        Dictionary<long, JsQueryEventArgs> queryMap_ = new Dictionary<long, JsQueryEventArgs>();
        void OnJsQuery(object sender,JsQueryEventArgs args)
        {
            if ((IntPtr)sender != this.webView_)
                return;
            args.Request = TneEncoder.UnEscape(args.Request);
            if (this.JsQuery != null)
            {
                //Task.Factory.StartNew(() =>
                //{
                //    lock (jsqueryLock_)
                //    {
                //        queryMap_.Add(args.QueryId, args);
                //        JsQuery(this, args);
                //    }
                //});           
                queryMap_.Add(args.QueryId, args);
                JsQuery(this, args);
            }            
        }
        void OnConsole(IntPtr webView, IntPtr param, wkeConsoleLevel level, IntPtr message, IntPtr sourceName, uint sourceLine, IntPtr stackTrace)
        {
            var msg = wkeToString(message);
            var srName = wkeToString(sourceName);
            var stckTrace = wkeToString(stackTrace);
        }
    }
}
