using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
namespace Tnelab.MiniBlink
{
    /// <summary>
    /// MiniBlinkApi封装
    /// </summary>
    public sealed class WebView : IDisposable
    {
        static WebView()
        {
        }
        /// <summary>
        /// 构造MiniBLink WebView
        /// </summary>
        public WebView()
        {
            this._webViewPtr = MiniBlink.NativeMethods.wkeCreateWebView();
            if (_webViewDic.ContainsKey(this._webViewPtr))
            {
                _webViewDic.Remove(this._webViewPtr);
            }
            _webViewDic.Add(this._webViewPtr, this);

            BuildWebViewEvent();
        }
        internal IntPtr WebViewPtr
        {
            get { return this._webViewPtr; }
        }
        /// <summary>
        /// WebView内部名称
        /// </summary>
        public string Name
        {
            get
            {
                return MiniBlink.NativeMethods.wkeGetName(this._webViewPtr);
            }
            set
            {
                MiniBlink.NativeMethods.wkeSetName(this._webViewPtr, value);
            }
        }
        /// <summary>
        /// 获取页面标题
        /// </summary>
        public string Title
        {
            get
            {
                return MiniBlink.NativeMethods.wkeGetTitleW(this._webViewPtr);
            }
        }
        /// <summary>
        /// webview宽度
        /// </summary>
        public int Width
        {
            get
            {
                return MiniBlink.NativeMethods.wkeWidth(this._webViewPtr);
            }
        }
        /// <summary>
        /// webview高度
        /// </summary>
        public int Height
        {
            get
            {
                return MiniBlink.NativeMethods.wkeHeight(this._webViewPtr);
            }
        }
        /// <summary>
        /// 页面内容宽度
        /// </summary>
        public int ContentsWidth
        {
            get
            {
                return MiniBlink.NativeMethods.wkeContentsWidth(this._webViewPtr);
            }
        }
        /// <summary>
        /// 页面内容高度
        /// </summary>
        public int ContentsHeight
        {
            get
            {
                return MiniBlink.NativeMethods.wkeContentsHeight(this._webViewPtr);
            }
        }
        /// <summary>
        /// 是否启用cookie
        /// </summary>
        public bool CookieEnabled
        {
            get
            {
                return MiniBlink.NativeMethods.wkeIsCookieEnabled(this._webViewPtr);
            }
            set
            {
                MiniBlink.NativeMethods.wkeSetCookieEnabled(this._webViewPtr, value);
            }
        }
        /// <summary>
        /// 页面多媒体的音量
        /// </summary>
        /// <returns></returns>
        public float MediaVolume
        {
            get
            {
                return MiniBlink.NativeMethods.wkeGetMediaVolume(this._webViewPtr);
            }
            set
            {
                MiniBlink.NativeMethods.wkeSetMediaVolume(this._webViewPtr, value);
            }
        }
        /// <summary>
        /// 标题发生改变时回调
        /// </summary>
        public TitleChangedCallback TitleChangedCallback { get; private set; }
        public TitleChangedCallback MouseOverUrlChangedCallback { get; private set; }
        public URLChangedCallback URLChangedCallback { get; private set; }
        public URLChangedCallback2 URLChangedCallback2 { get; private set; }
        public PaintUpdatedCallback PaintUpdatedCallback { get; private set; }
        public AlertBoxCallback AlertBoxCallback { get; private set; }
        public ConfirmBoxCallback ConfirmBoxCallback { get; private set; }
        public PromptBoxCallback PromptBoxCallback { get; private set; }
        public NavigationCallback NavigationCallback { get; private set; }
        public CreateViewCallback CreateViewCallback { get; private set; }
        public DocumentReadyCallback DocumentReadyCallback { get; private set; }
        public DocumentReady2Callback DocumentReady2Callback { get; private set; }
        public LoadingFinishCallback LoadingFinishCallback { get; private set; }
        public DownloadCallback DownloadCallback { get; private set; }
        public ConsoleCallback ConsoleCallback { get; private set; }
        public LoadUrlBeginCallback LoadUrlBeginCallback { get; private set; }
        public LoadUrlEndCallback LoadUrlEndCallback { get; private set; }
        public DidCreateScriptContextCallback DidCreateScriptContextCallback { get; private set; }
        public WillReleaseScriptContextCallback WillReleaseScriptContextCallback { get; private set; }
        public NetResponseCallback NetResponseCallback { get; private set; }

        /// <summary>
        /// 初始化miniblink执行环境
        /// </summary>
        public static void Initialize()
        {
            MiniBlink.NativeMethods.wkeInitialize();
        }
        /// <summary>
        /// 用指定设置项初始化miniblink执行化境
        /// </summary>
        /// <param name="settings"></param>
        public static void InitializeEx(Settings settings)
        {
            MiniBlink.NativeMethods.wkeInitializeEx(settings);
        }
        /// <summary>
        /// 配置minblink执行环境
        /// </summary>
        /// <param name="settings"></param>
        public static void Configure(Settings settings)
        {
            MiniBlink.NativeMethods.wkeConfigure(settings);
        }
        /// <summary>
        /// miniblink环境是否已经初始化
        /// </summary>
        /// <returns></returns>
        public static bool IsInitialize()
        {
            return MiniBlink.NativeMethods.wkeIsInitialize();
        }
        /// <summary>
        /// 关闭MiniBlink环境
        /// </summary>
        public static void Shutdown()
        {
            MiniBlink.NativeMethods.wkeShutdown();
        }
        /// <summary>
        /// 关闭miniblink执行环境
        /// </summary>
        public static void MiniBlinkFinalize()
        {
            MiniBlink.NativeMethods.wkeFinalize();
        }
        /// <summary>
        /// 获取miniblink版本号
        /// </summary>
        public static uint GetVersion()
        {
            return MiniBlink.NativeMethods.wkeGetVersion();
        }
        /// <summary>
        /// 获取miniblink版本号
        /// </summary>
        public static string GetVersionString()
        {
            return MiniBlink.NativeMethods.wkeGetVersionString();
        }
        /// <summary>
        /// 根据名称获取webview
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static WebView GetWebView(string name)
        {
            return _webViewDic[MiniBlink.NativeMethods.wkeGetWebView(name)];
        }
        public static WebView GetWebViewForCurrentContext()
        {
            return _webViewDic[MiniBlink.NativeMethods.wkeGetWebViewForCurrentContext()];
        }

        /// <summary>
        /// 设置miniblink代理服务器信息
        /// </summary>
        /// <param name="proxy"></param>
        public static void SetProxy(Proxy proxy)
        {
            MiniBlink.NativeMethods.wkeSetProxy(proxy);
        }
        /// <summary>
        /// 访问所有cookie
        /// </summary>
        /// <param name="param_s"></param>
        /// <param name="visitor"></param>
        public static void VisitAllCookie(IntPtr param_s, CookieVisitor visitor)
        {
            MiniBlink.NativeMethods.wkeVisitAllCookie(param_s, visitor);
        }
        public static void PerformCookieCommand(CookieCommand command)
        {
            MiniBlink.NativeMethods.wkePerformCookieCommand(command);
        }
        public static IntPtr GetBlinkMainThreadIsolate()
        {
            return MiniBlink.NativeMethods.wkeGetBlinkMainThreadIsolate();
        }
        public static void JsBindFunction(string name, JsNativeFunction fn, uint argCount)
        {
            long lfun(IntPtr es, IntPtr param)
            {
                var jes = new JsExecState(es);
                return fn(jes).Value;
            }
            wkeJsNativeFunction func = lfun;
            MiniBlink.NativeMethods.wkeJsBindFunction(name, func, IntPtr.Zero, argCount);
            wkeJsNativeFunctionList.Add(func);
        }
        /// <summary>
        /// 启动JS垃圾回收
        /// </summary>
        /// <param name="delayMs">延迟毫秒数</param>
        public void GC(long delayMs)
        {
            MiniBlink.NativeMethods.wkeGC(this._webViewPtr, delayMs);
        }
        /// <summary>
        /// 获取当前页面源代码
        /// </summary>
        /// <returns></returns>
        public string GetSource()
        {
            return MiniBlink.NativeMethods.WkeGetSource(this._webViewPtr);
        }
        /// <summary>
        /// 选择页面所有内容
        /// </summary>
        //public void SelectAll()
        //{
        //    MiniBlinkApi.wkeSelectAll(this._webViewPtr);
        //}
        ///// <summary>
        ///// 复制选择内容
        ///// </summary>
        //public void Copy()
        //{
        //    MiniBlinkApi.wkeCopy(this._webViewPtr);
        //}
        ///// <summary>
        ///// 剪切选择内容
        ///// </summary>
        //public void Cut()
        //{
        //    MiniBlinkApi.wkeCopy(this._webViewPtr);
        //}
        ///// <summary>
        ///// 黏贴剪切板内容
        ///// </summary>
        //public void Paste()
        //{
        //    MiniBlinkApi.wkePaste(this._webViewPtr);
        //}
        ///// <summary>
        ///// 删除选中内容
        ///// </summary>
        //public void Delete()
        //{
        //    MiniBlinkApi.wkeDelete(this._webViewPtr);
        //}        
        public float ZoomFactor
        {
            get
            {
                return MiniBlink.NativeMethods.wkeGetZoomFactor(this._webViewPtr);
            }
            set
            {
                MiniBlink.NativeMethods.wkeSetZoomFactor(this._webViewPtr, value);
            }
        }

        public void SetViewSettings(ViewSettings settings)
        {
            MiniBlink.NativeMethods.wkeSetViewSettings(this._webViewPtr, settings);
        }
        /// <summary>
        /// 显示浏览器调试器
        /// 开启方式是：SetDebugConfig(m_wkeView, "showDevTools", "E:/mycode/miniblink49/trunk/third_party/WebKit/Source/devtools/front_end/inspector.html(utf8编码)");
        /// </summary>
        /// <param name="debugString"></param>
        /// <param name="param"></param>
        public void SetDebugConfig(string debugString, string param)
        {
            MiniBlink.NativeMethods.wkeSetDebugConfig(this._webViewPtr, debugString, param);
        }
        /// <summary>
        /// 刷新webview
        /// </summary>
        public void Update()
        {
            MiniBlink.NativeMethods.wkeUpdate();
        }
        //public void DestroyWebView()
        //{
        //    MiniBlink.NativeMethods.wkeDestroyWebView(this._webViewPtr);
        //}
        /// <summary>
        /// 启用内存缓冲器
        /// </summary>
        /// <param name="b"></param>
        public void SetMemoryCacheEnable(bool b)
        {
            MiniBlink.NativeMethods.wkeSetMemoryCacheEnable(this._webViewPtr, b);
        }
        /// <summary>
        /// 启用触屏
        /// </summary>
        /// <param name="b"></param>
        public void SetTouchEnabled(bool b)
        {
            MiniBlink.NativeMethods.wkeSetTouchEnabled(this._webViewPtr, b);
        }
        /// <summary>
        /// 启用导航到新窗口
        /// </summary>
        /// <param name="b"></param>
        public void SetNavigationToNewWindowEnable(bool b)
        {
            MiniBlink.NativeMethods.wkeSetNavigationToNewWindowEnable(this._webViewPtr, b);
        }
        /// <summary>
        /// 启用CspCheck
        /// </summary>
        /// <param name="b"></param>
        public void SetCspCheckEnable(bool b)
        {
            MiniBlink.NativeMethods.wkeSetCspCheckEnable(this._webViewPtr, b);
        }
        /// <summary>
        /// 启用npapi插件
        /// </summary>
        /// <param name="b"></param>
        public void SetNpapiPluginsEnabled(bool b)
        {
            MiniBlink.NativeMethods.wkeSetNpapiPluginsEnabled(this._webViewPtr, b);
        }
        public void SetHeadlessEnabled(bool b)
        {
            MiniBlink.NativeMethods.wkeSetHeadlessEnabled(this._webViewPtr, b);
        }
        /// <summary>
        /// 启用拖放
        /// </summary>
        /// <param name="b"></param>
        public void SetDragEnable(bool b)
        {
            MiniBlink.NativeMethods.wkeSetDragEnable(this._webViewPtr, b);
        }
        public void SetViewNetInterface(string netInterface)
        {
            MiniBlink.NativeMethods.wkeSetViewNetInterface(this._webViewPtr, netInterface);
        }
        /// <summary>
        /// 设置webview代理服务器信息
        /// </summary>
        /// <param name="proxy"></param>
        public void SetViewProxy(Proxy proxy)
        {
            MiniBlink.NativeMethods.wkeSetViewProxy(this._webViewPtr, proxy);
        }
        /// <summary>
        /// 设置webview所在窗口句柄
        /// </summary>
        /// <param name="wnd"></param>
        public void SetHandle(IntPtr hwnd)
        {
            MiniBlink.NativeMethods.wkeSetHandle(this._webViewPtr, hwnd);
        }
        public void SetHandleOffset(int x, int y)
        {
            MiniBlink.NativeMethods.wkeSetHandleOffset(this._webViewPtr, x, y);
        }
        /// <summary>
        /// 是否透明显示
        /// </summary>
        /// <returns></returns>
        public bool IsTransparent()
        {
            return MiniBlink.NativeMethods.wkeIsTransparent(this._webViewPtr);
        }
        /// <summary>
        /// 设置透明显示，注意设置透明为true需要设置所在窗体的属性为
        /// var exStyle = Win32.GetWindowLong(ParentControl.Handle, Win32.GWL_STYLE);
        ///Win32.SetWindowLong(ParentControl.Handle, Win32.GWL_STYLE, exStyle & ~Win32.WS_CAPTION & ~Win32.WS_SYSMENU & ~Win32.WS_SIZEBOX);
        ///exStyle = Win32.GetWindowLong(ParentControl.Handle, Win32.GWL_EXSTYLE);
        ///Win32.SetWindowLong(ParentControl.Handle, Win32.GWL_EXSTYLE, exStyle | Win32.WS_EX_LAYERED);
        /// </summary>
        /// <param name="transparent"></param>
        public void SetTransparent(bool transparent)
        {
            MiniBlink.NativeMethods.wkeSetTransparent(this._webViewPtr, transparent);
        }
        public void SetUserAgent(string userAgent)
        {
            MiniBlink.NativeMethods.wkeSetUserAgentW(this._webViewPtr, userAgent);
        }
        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        public void LoadURLW(string url)
        {
            MiniBlink.NativeMethods.wkeLoadURLW(this._webViewPtr, url);
        }
        /// <summary>
        /// url为unicode编码
        /// </summary>
        /// <param name="View"></param>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="postLen"></param>
        public void PostURLW(WebView View, string url, string postData, int postLen)
        {
            MiniBlink.NativeMethods.wkePostURLW(this._webViewPtr, url, postData, postLen);
        }
        /// <summary>
        /// </summary>
        /// <param name="html"></param>
        public void LoadHTMLW(string html)
        {
            MiniBlink.NativeMethods.wkeLoadHTMLW(this._webViewPtr, html);
        }
        /// <summary>
        /// </summary>
        /// <param name="filename"></param>
        public void LoadFileW(string filename)
        {
            MiniBlink.NativeMethods.wkeLoadFileW(this._webViewPtr, filename);
        }
        /// <summary>
        /// 返回Url
        /// </summary>
        /// <returns></returns>
        public string GetURL()
        {
            return MiniBlink.NativeMethods.wkeGetURL(this._webViewPtr);
        }
        /// <summary>
        /// 是否处于加载状态
        /// </summary>
        /// <returns></returns>
        public bool IsLoading()
        {
            return MiniBlink.NativeMethods.wkeIsLoading(this._webViewPtr);
        }
        /// <summary>
        /// 加载是否成功
        /// </summary>
        /// <returns></returns>
        public bool IsLoadingSucceeded()
        {
            return MiniBlink.NativeMethods.wkeIsLoadingSucceeded(this._webViewPtr);
        }
        /// <summary>
        /// 加载是否失败
        /// </summary>
        /// <returns></returns>
        public bool IsLoadingFailed()
        {
            return MiniBlink.NativeMethods.wkeIsLoadingFailed(this._webViewPtr);
        }
        /// <summary>
        /// 加载是否完成
        /// </summary>
        /// <returns></returns>
        public bool IsLoadingCompleted()
        {
            return MiniBlink.NativeMethods.wkeIsLoadingCompleted(this._webViewPtr);
        }
        /// <summary>
        /// 文档是否解析完成
        /// </summary>
        /// <returns></returns>
        public bool IsDocumentReady()
        {
            return MiniBlink.NativeMethods.wkeIsDocumentReady(this._webViewPtr);
        }
        /// <summary>
        /// 停止加载
        /// </summary>
        public void StopLoading()
        {
            MiniBlink.NativeMethods.wkeStopLoading(this._webViewPtr);
        }
        /// <summary>
        /// 重新加载
        /// </summary>
        public void Reload()
        {
            MiniBlink.NativeMethods.wkeReload(this._webViewPtr);
        }
        /// <summary>
        /// 发送resize消息
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void Resize(int w, int h)
        {
            MiniBlink.NativeMethods.wkeResize(this._webViewPtr, w, h);
        }
        /// <summary>
        /// 发送页面渲染已经过期
        /// </summary>
        /// <param name="dirty"></param>
        public void SetDirty(bool dirty)
        {
            MiniBlink.NativeMethods.wkeSetDirty(this._webViewPtr, dirty);
        }
        /// <summary>
        /// 页面渲染是否过期
        /// </summary>
        /// <returns></returns>
        public bool IsDirty()
        {
            return MiniBlink.NativeMethods.wkeIsDirty(this._webViewPtr);
        }
        /// <summary>
        /// 增加渲染过期区域
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void AddDirtyArea(int x, int y, int w, int h)
        {
            MiniBlink.NativeMethods.wkeAddDirtyArea(this._webViewPtr, x, y, w, h);
        }
        /// <summary>
        /// 如果需要重整页面布局，则重整页面布局
        /// </summary>
        public void LayoutIfNeeded()
        {
            MiniBlink.NativeMethods.wkeLayoutIfNeeded(this._webViewPtr);
        }
        /// <summary>
        /// 渲染到内存，可以把页面渲染结果保存到文件
        /// </summary>
        /// <param name="bits"></param>
        /// <param name="bufWid"></param>
        /// <param name="bufHei"></param>
        /// <param name="xDst"></param>
        /// <param name="yDst"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="xSrc"></param>
        /// <param name="ySrc"></param>
        /// <param name="bCopyAlpha"></param>
        public byte[] Paint2(int bufWid, int bufHei, int xDst, int yDst, int w, int h, int xSrc, int ySrc, bool bCopyAlpha)
        {
            byte[] bits = new byte[bufHei * bufHei * 4];
            var bitsPtr = Marshal.AllocHGlobal(bits.Length);
            MiniBlink.NativeMethods.wkePaint2(this._webViewPtr, bitsPtr, bufWid, bufHei, xDst, yDst, w, h, xSrc, ySrc, bCopyAlpha);
            Marshal.Copy(bitsPtr, bits, 0, bits.Length);
            Marshal.FreeHGlobal(bitsPtr);
            return bits;
        }
        public void Paint2(IntPtr bitsPtr, int bufWid, int bufHei, int xDst, int yDst, int w, int h, int xSrc, int ySrc, bool bCopyAlpha)
        {
            MiniBlink.NativeMethods.wkePaint2(this._webViewPtr, bitsPtr, bufWid, bufHei, xDst, yDst, w, h, xSrc, ySrc, bCopyAlpha);
        }
        /// <summary>
        /// 渲染到内存，可以把页面渲染结果保存到文件
        /// </summary>
        /// <param name="bits"></param>
        /// <param name="pitch"></param>
        public void Paint(IntPtr bits, int pitch)
        {
            MiniBlink.NativeMethods.wkePaint(this._webViewPtr, bits, pitch);
        }
        /// <summary>
        /// 重新渲染页面
        /// </summary>
        public void RepaintIfNeeded()
        {
            MiniBlink.NativeMethods.wkeRepaintIfNeeded(this._webViewPtr);
        }
        /// <summary>
        /// 获取webview的hdc
        /// </summary>
        /// <returns></returns>
        public IntPtr GetViewDC()
        {
            return MiniBlink.NativeMethods.wkeGetViewDC(this._webViewPtr);
        }
        /// <summary>
        /// 获取webview所在窗体的句柄
        /// </summary>
        /// <returns></returns>
        public IntPtr GetHostHWND()
        {
            return MiniBlink.NativeMethods.wkeGetHostHWND(this._webViewPtr);
        }

        public bool CanGoBack()
        {
            return MiniBlink.NativeMethods.wkeCanGoBack(this._webViewPtr);
        }

        public bool GoBack()
        {
            return MiniBlink.NativeMethods.wkeGoBack(this._webViewPtr);
        }

        public bool CanGoForward()
        {
            return MiniBlink.NativeMethods.wkeCanGoForward(this._webViewPtr);
        }

        public bool GoForward()
        {
            return MiniBlink.NativeMethods.wkeGoForward(this._webViewPtr);
        }
        public void EditorSelectAll()
        {
            MiniBlink.NativeMethods.wkeEditorSelectAll(this._webViewPtr);
        }
        public void EditorUnSelect()
        {
            MiniBlink.NativeMethods.wkeEditorUnSelect(this._webViewPtr);
        }
        public void EditorCopy()
        {
            MiniBlink.NativeMethods.wkeEditorCopy(this._webViewPtr);
        }
        public void EditorCut()
        {
            MiniBlink.NativeMethods.wkeEditorCut(this._webViewPtr);
        }
        public void EditorPaste()
        {
            MiniBlink.NativeMethods.wkeEditorPaste(this._webViewPtr);
        }
        public void EditorDelete()
        {
            MiniBlink.NativeMethods.wkeEditorDelete(this._webViewPtr);
        }
        public void EditorUndo()
        {
            MiniBlink.NativeMethods.wkeEditorUndo(this._webViewPtr);
        }
        public void EditorRedo()
        {
            MiniBlink.NativeMethods.wkeEditorRedo(this._webViewPtr);
        }
        /// <summary>
        /// 获取cookie
        /// </summary>
        /// <returns></returns>
        public string GetCookieW()
        {
            return MiniBlink.NativeMethods.wkeGetCookieW(this._webViewPtr);
        }
        public void SetCookieJarPath(string path)
        {
            MiniBlink.NativeMethods.wkeSetCookieJarPath(this._webViewPtr, path);
        }

        public void SetCookieJarFullPath(string path)
        {
            MiniBlink.NativeMethods.wkeSetCookieJarFullPath(this._webViewPtr, path);
        }
        /// <summary>
        /// 设置本地存储路径
        /// </summary>
        /// <param name="path"></param>
        public void SetLocalStorageFullPath(string path)
        {
            MiniBlink.NativeMethods.wkeSetLocalStorageFullPath(this._webViewPtr, path);
        }
        /// <summary>
        /// 发送鼠标消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool FireMouseEvent(uint message, int x, int y, uint flags)
        {
            return MiniBlink.NativeMethods.wkeFireMouseEvent(this._webViewPtr, message, x, y, flags);
        }
        /// <summary>
        /// 发送上下文菜单消息
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool FireContextMenuEvent(int x, int y, uint flags)
        {
            return MiniBlink.NativeMethods.wkeFireContextMenuEvent(this._webViewPtr, x, y, flags);
        }
        /// <summary>
        /// 发送鼠标滚轮消息
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="delta"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool FireMouseWheelEvent(int x, int y, int delta, int flags)
        {
            return MiniBlink.NativeMethods.wkeFireMouseWheelEvent(this._webViewPtr, x, y, delta, flags);
        }
        /// <summary>
        /// 发送松开键盘消息
        /// </summary>
        /// <param name="virtualKeyCode"></param>
        /// <param name="flags"></param>
        /// <param name="systemKey"></param>
        /// <returns></returns>
        public bool FireKeyUpEvent(uint virtualKeyCode, uint flags, bool systemKey)
        {
            return MiniBlink.NativeMethods.wkeFireKeyUpEvent(this._webViewPtr, virtualKeyCode, flags, systemKey);
        }
        /// <summary>
        /// 发送按下键盘消息
        /// </summary>
        /// <param name="virtualKeyCode"></param>
        /// <param name="flags"></param>
        /// <param name="systemKey"></param>
        /// <returns></returns>
        public bool FireKeyDownEvent(uint virtualKeyCode, uint flags, bool systemKey)
        {
            return MiniBlink.NativeMethods.wkeFireKeyDownEvent(this._webViewPtr, virtualKeyCode, flags, systemKey);
        }
        /// <summary>
        /// 发送键盘消息
        /// </summary>
        /// <param name="charCode"></param>
        /// <param name="flags"></param>
        /// <param name="systemKey"></param>
        /// <returns></returns>
        public bool FireKeyPressEvent(uint charCode, uint flags, bool systemKey)
        {
            return MiniBlink.NativeMethods.wkeFireKeyPressEvent(this._webViewPtr, charCode, flags, systemKey);
        }
        /// <summary>
        /// 发送windows消息
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="message"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool FireWindowsMessage(IntPtr hWnd, uint message, uint wParam, uint lParam, out uint result)
        {
            return MiniBlink.NativeMethods.wkeFireWindowsMessage(this._webViewPtr, hWnd, message, wParam, lParam, out result);
        }
        /// <summary>
        /// 设置焦点
        /// </summary>
        public void SetFocus()
        {
            MiniBlink.NativeMethods.wkeSetFocus(this._webViewPtr);
        }
        /// <summary>
        /// 取消焦点
        /// </summary>
        public void KillFocus()
        {
            MiniBlink.NativeMethods.wkeKillFocus(this._webViewPtr);
        }

        public Rect GetCaretRect()
        {
            var rect = MiniBlink.NativeMethods.wkeGetCaretRect(this._webViewPtr);
            return rect;
        }
        public JsValue RunJSW(string script)
        {
            return new JsValue(MiniBlink.NativeMethods.wkeRunJSW(this._webViewPtr, script));
        }
        public JsExecState GlobalExec()
        {
            return new JsExecState(MiniBlink.NativeMethods.wkeGlobalExec(this._webViewPtr));
        }
        public void Sleep()
        {
            MiniBlink.NativeMethods.wkeSleep(this._webViewPtr);
        }

        public void Wake()
        {
            MiniBlink.NativeMethods.wkeWake(this._webViewPtr);
        }


        public bool IsAwake()
        {
            return MiniBlink.NativeMethods.wkeIsAwake(this._webViewPtr);
        }
        public void SetEditable(bool editable)
        {
            MiniBlink.NativeMethods.wkeSetEditable(this._webViewPtr, editable);
        }
        public void SetUserKeyValue(string key, IntPtr value)
        {
            MiniBlink.NativeMethods.wkeSetUserKeyValue(this._webViewPtr, key, value);
        }
        public IntPtr GetUserKeyValue(string key)
        {
            return MiniBlink.NativeMethods.wkeGetUserKeyValue(this._webViewPtr, key);
        }
        public CursorInfoType GetCursorInfoType()
        {
            return (CursorInfoType)MiniBlink.NativeMethods.wkeGetCursorInfoType(this._webViewPtr);
        }

        public void SetDragFiles(POINT clintPos, POINT screenPos, String[] files, int filesCount)
        {
            IntPtr[] filePtrs = new IntPtr[files.Length];
            for (var i = 0; i < files.Length; i++)
            {
                filePtrs[i] = MiniBlink.NativeMethods.wkeCreateStringW(files[i], (uint)files[i].Length);
            }
            MiniBlink.NativeMethods.wkeSetDragFiles(this._webViewPtr, clintPos, screenPos, filePtrs, filesCount);
            foreach (var fp in filePtrs)
            {
                MiniBlink.NativeMethods.wkeDeleteString(fp);
            }
        }
        public void SetTitleChangedCallback(TitleChangedCallback callbak)
        {
            this.TitleChangedCallback = callbak;
            if (this.TitleChangedCallback == null)
                return;
            MiniBlink.NativeMethods.wkeOnTitleChanged(this._webViewPtr, this._titleChangeCallback, IntPtr.Zero);
        }
        public void SetMouseOverUrlChangedCallback(TitleChangedCallback callbak)
        {
            this.MouseOverUrlChangedCallback = callbak;
            if (this.MouseOverUrlChangedCallback == null)
                return;
            MiniBlink.NativeMethods.wkeOnMouseOverUrlChanged(this._webViewPtr, this._mouseOverUrlChanged, IntPtr.Zero);
        }
        public void SetURLChangedCallback(URLChangedCallback callbak)
        {
            this.URLChangedCallback = callbak;
            if (this.URLChangedCallback == null)
                return;
            MiniBlink.NativeMethods.wkeOnURLChanged(this._webViewPtr, this._uRLChangedCallback, IntPtr.Zero);
        }
        public void SetURLChangedCallback2(URLChangedCallback2 callbak)
        {
            this.URLChangedCallback2 = callbak;
            if (this.URLChangedCallback2 == null)
                return;
            MiniBlink.NativeMethods.wkeOnURLChanged2(this._webViewPtr, this._uRLChangedCallback2, IntPtr.Zero);
        }
        public void SetPaintUpdatedCallback(PaintUpdatedCallback callbak)
        {
            this.PaintUpdatedCallback = callbak;
            if (this.PaintUpdatedCallback == null)
                return;
            MiniBlink.NativeMethods.wkeOnPaintUpdated(this._webViewPtr, this._paintUpdatedCallback, IntPtr.Zero);
        }
        public void SetAlertBoxCallback(AlertBoxCallback callbak)
        {
            this.AlertBoxCallback = callbak;
            if (this.AlertBoxCallback == null)
                return;
            MiniBlink.NativeMethods.wkeOnAlertBox(this._webViewPtr, this._alertBoxCallback, IntPtr.Zero);
        }
        public void SetConfirmBoxCallback(ConfirmBoxCallback callbak)
        {
            this.ConfirmBoxCallback = callbak;
            if (this.ConfirmBoxCallback == null)
                return;
            MiniBlink.NativeMethods.wkeOnConfirmBox(this._webViewPtr, this._confirmBoxCallback, IntPtr.Zero);
        }
        public void SetPromptBoxCallback(PromptBoxCallback callbak)
        {
            this.PromptBoxCallback = callbak;
            if (this.PromptBoxCallback == null)
                return;
            MiniBlink.NativeMethods.wkeOnPromptBox(this._webViewPtr, this._promptBoxCallback, IntPtr.Zero);
        }
        public void SetNavigationCallback(NavigationCallback callbak)
        {
            this.NavigationCallback = callbak;
            if (this.NavigationCallback == null)
                return;
            MiniBlink.NativeMethods.wkeOnNavigation(this._webViewPtr, this._NavigationCallback, IntPtr.Zero);
        }
        public void SetCreateViewCallback(CreateViewCallback callbak)
        {
            this.CreateViewCallback = callbak;
            if (this.CreateViewCallback == null)
                return;
            MiniBlink.NativeMethods.wkeOnCreateView(this._webViewPtr, this._CreateViewCallback, IntPtr.Zero);
        }
        public void SetDocumentReadyCallback(DocumentReadyCallback callbak)
        {
            this.DocumentReadyCallback = callbak;
            if (this.DocumentReadyCallback == null)
                return;
            MiniBlink.NativeMethods.wkeOnDocumentReady(this._webViewPtr, this._DocumentReadyCallback, IntPtr.Zero);
        }
        public void SetDocumentReady2Callback(DocumentReady2Callback callbak)
        {
            this.DocumentReady2Callback = callbak;
            if (this.DocumentReady2Callback == null)
                return;
            MiniBlink.NativeMethods.wkeOnDocumentReady2(this._webViewPtr, this._DocumentReady2Callback, IntPtr.Zero);
        }
        public void SetLoadingFinishCallback(LoadingFinishCallback callbak)
        {
            this.LoadingFinishCallback = callbak;
            if (this.LoadingFinishCallback == null)
                return;
            MiniBlink.NativeMethods.wkeOnLoadingFinish(this._webViewPtr, this._LoadingFinishCallback, IntPtr.Zero);
        }
        public void SetDownloadCallback(DownloadCallback callbak)
        {
            this.DownloadCallback = callbak;
            if (this.DownloadCallback == null)
                return;
            MiniBlink.NativeMethods.wkeOnDownload(this._webViewPtr, this._DownloadCallback, IntPtr.Zero);
        }
        public void SetConsoleCallback(ConsoleCallback callbak)
        {
            this.ConsoleCallback = callbak;
            if (this.ConsoleCallback == null)
                return;
            MiniBlink.NativeMethods.wkeOnConsole(this._webViewPtr, this._ConsoleCallback, IntPtr.Zero);
        }
        public void SetLoadUrlBeginCallback(LoadUrlBeginCallback callbak)
        {
            this.LoadUrlBeginCallback = callbak;
            if (this.LoadUrlBeginCallback == null)
                return;
            MiniBlink.NativeMethods.wkeOnLoadUrlBegin(this._webViewPtr, this._LoadUrlBeginCallback, IntPtr.Zero);
        }
        public void SetLoadUrlEndCallback(LoadUrlEndCallback callbak)
        {
            this.LoadUrlEndCallback = callbak;
            if (this.LoadUrlEndCallback == null)
                return;
            MiniBlink.NativeMethods.wkeOnLoadUrlEnd(this._webViewPtr, this._LoadUrlEndCallback, IntPtr.Zero);
        }
        public void SetDidCreateScriptContextCallback(DidCreateScriptContextCallback callbak)
        {
            this.DidCreateScriptContextCallback = callbak;
            if (this.DidCreateScriptContextCallback == null)
                return;
            MiniBlink.NativeMethods.wkeOnDidCreateScriptContext(this._webViewPtr, this._DidCreateScriptContextCallback, IntPtr.Zero);
        }
        public void SetWillReleaseScriptContextCallback(WillReleaseScriptContextCallback callbak)
        {
            this.WillReleaseScriptContextCallback = callbak;
            if (this.WillReleaseScriptContextCallback == null)
                return;
            MiniBlink.NativeMethods.wkeOnWillReleaseScriptContext(this._webViewPtr, this._WillReleaseScriptContextCallback, IntPtr.Zero);
        }
        public void SetNetResponseCallback(NetResponseCallback callbak)
        {
            this.NetResponseCallback = callbak;
            if (this.NetResponseCallback == null)
                return;
            MiniBlink.NativeMethods.wkeNetOnResponse(this._webViewPtr, this._NetResponseCallback, IntPtr.Zero);
        }
        public bool IsMainFrame(IntPtr frameId)
        {
            return MiniBlink.NativeMethods.wkeIsMainFrame(this._webViewPtr, frameId);
        }


        public bool IsWebRemoteFrame(IntPtr frameId)
        {
            return MiniBlink.NativeMethods.wkeIsWebRemoteFrame(this._webViewPtr, frameId);
        }
        public IntPtr WebFrameGetMainFrame()
        {
            return MiniBlink.NativeMethods.wkeWebFrameGetMainFrame(this._webViewPtr);
        }

        public long RunJsByFrame(IntPtr frameId, string script, bool isInClosure)
        {
            return MiniBlink.NativeMethods.wkeRunJsByFrame(this._webViewPtr, frameId, script, isInClosure);
        }

        public string GetFrameUrl(IntPtr frameId)
        {
            return MiniBlink.NativeMethods.wkeGetFrameUrl(this._webViewPtr, frameId);
        }


        public void WebFrameGetMainWorldScriptContext(IntPtr webFrameId, out IntPtr contextOut)
        {
            MiniBlink.NativeMethods.wkeWebFrameGetMainWorldScriptContext(this._webViewPtr, webFrameId, out contextOut);
        }
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _webViewDic.Remove(this._webViewPtr);
                System.GC.SuppressFinalize(this);
            }
            MiniBlink.NativeMethods.wkeDestroyWebView(this._webViewPtr);
        }
        public void Dispose()
        {
            this.Dispose(true);
        }
        ~WebView()
        {
            Dispose(false);
        }
        // blink内部窗口创建回调，例如下拉框

        //public void OnBlinkWindowCreate()
        //{
        //    MiniBlinkApi.wkeOnBlinkWindowCreate();
        //}        
        internal static WebView GetWebView(IntPtr webViewPtr)
        {
            return _webViewDic[webViewPtr];
        }
        static readonly Dictionary<IntPtr, WebView> _webViewDic = new Dictionary<IntPtr, WebView>();
        static readonly List<wkeJsNativeFunction> wkeJsNativeFunctionList = new List<wkeJsNativeFunction>();
        private IntPtr _webViewPtr = IntPtr.Zero;
        private wkeTitleChangedCallback _titleChangeCallback;
        private wkeTitleChangedCallback _mouseOverUrlChanged;
        private wkeURLChangedCallback _uRLChangedCallback;
        private wkeURLChangedCallback2 _uRLChangedCallback2;
        private wkePaintUpdatedCallback _paintUpdatedCallback;
        private wkeAlertBoxCallback _alertBoxCallback;
        private wkeConfirmBoxCallback _confirmBoxCallback;
        private wkePromptBoxCallback _promptBoxCallback;
        private wkeNavigationCallback _NavigationCallback;
        private wkeCreateViewCallback _CreateViewCallback;
        private wkeDocumentReadyCallback _DocumentReadyCallback;
        private wkeDocumentReady2Callback _DocumentReady2Callback;
        private wkeLoadingFinishCallback _LoadingFinishCallback;
        private wkeDownloadCallback _DownloadCallback;
        private wkeConsoleCallback _ConsoleCallback;
        private wkeLoadUrlBeginCallback _LoadUrlBeginCallback;
        private wkeLoadUrlEndCallback _LoadUrlEndCallback;
        private wkeDidCreateScriptContextCallback _DidCreateScriptContextCallback;
        private wkeWillReleaseScriptContextCallback _WillReleaseScriptContextCallback;
        private wkeNetResponseCallback _NetResponseCallback;
        private void BuildWebViewEvent()
        {
            this._titleChangeCallback = (webViewPtr, param, title) =>
            {
                if (this.TitleChangedCallback == null)
                    return;
                this.TitleChangedCallback(this, MiniBlink.NativeMethods.WkeToStringW(title));
            };


            this._mouseOverUrlChanged = (webViewPtr, param, title) =>
            {
                if (this.TitleChangedCallback == null)
                    return;
                this.TitleChangedCallback(this, MiniBlink.NativeMethods.WkeToStringW(title));
            };


            this._uRLChangedCallback = (webViewPtr, param, url) =>
            {
                if (this.URLChangedCallback == null)
                    return;
                this.URLChangedCallback(this, MiniBlink.NativeMethods.WkeToStringW(url));
            };


            this._uRLChangedCallback2 = (webViewPtr, param, frameId, url) =>
            {
                if (this.URLChangedCallback2 == null)
                    return;
                this.URLChangedCallback2(this, frameId, MiniBlink.NativeMethods.WkeToStringW(url));
            };


            this._paintUpdatedCallback = (IntPtr webView, IntPtr param, IntPtr hdc, int x, int y, int cx, int cy) =>
            {
                if (this.PaintUpdatedCallback == null)
                    return;
                this.PaintUpdatedCallback(this, hdc, x, y, cx, cy);
            };


            this._alertBoxCallback = (IntPtr webView, IntPtr param, IntPtr msg) => {
                if (this.AlertBoxCallback == null)
                    return;
                this.AlertBoxCallback(this, MiniBlink.NativeMethods.WkeToStringW(msg));
            };


            this._confirmBoxCallback = (IntPtr webView, IntPtr param, IntPtr msg) => {
                if (this.ConfirmBoxCallback == null)
                    throw new Exception("WebView.ConfirmBoxCallback为空");
                return this.ConfirmBoxCallback(this, MiniBlink.NativeMethods.WkeToStringW(msg));
            };


            this._promptBoxCallback = (IntPtr webView, IntPtr param, IntPtr msg, IntPtr defaultResult, IntPtr result) => {
                if (this.PromptBoxCallback == null)
                    throw new Exception("WebView.PromptBoxCallback 为空");
                return this.PromptBoxCallback(this, MiniBlink.NativeMethods.WkeToStringW(msg), MiniBlink.NativeMethods.WkeToStringW(defaultResult), MiniBlink.NativeMethods.WkeToStringW(result));
            };

            this._NavigationCallback = (IntPtr webView, IntPtr param, NavigationType navigationType, IntPtr url) => {
                if (this.NavigationCallback == null)
                    throw new Exception("WebView.NavigationCallback 为空");
                return this.NavigationCallback(this, navigationType, MiniBlink.NativeMethods.WkeToStringW(url));
            };
            this._CreateViewCallback = (IntPtr webView, IntPtr param, NavigationType navigationType, IntPtr url, WindowFeatures windowFeatures) =>
            {
                if (this.CreateViewCallback == null)
                    throw new Exception("WebView.CreateViewCallback 为空");
                return this.CreateViewCallback(this, navigationType, MiniBlink.NativeMethods.WkeToStringW(url), windowFeatures).WebViewPtr;

            };
            this._DocumentReadyCallback = (IntPtr webView, IntPtr param) =>
            {
                if (this.DocumentReadyCallback == null)
                    throw new Exception("WebView.DocumentReadyCallback 为空");
                this.DocumentReadyCallback(this);

            };
            this._DocumentReady2Callback = (IntPtr webView, IntPtr param, IntPtr frameId) =>
            {
                if (this.DocumentReady2Callback == null)
                    throw new Exception("WebView.DocumentReady2Callback 为空");
                this.DocumentReady2Callback(this, frameId);

            };
            this._LoadingFinishCallback = (IntPtr webView, IntPtr param, IntPtr url, LoadingResult result, IntPtr failedReason) =>
            {
                if (this.LoadingFinishCallback == null)
                    throw new Exception("WebView.LoadingFinishCallback 为空");
                this.LoadingFinishCallback(this, MiniBlink.NativeMethods.WkeToStringW(url), result, MiniBlink.NativeMethods.WkeToStringW(failedReason));
            };
            this._DownloadCallback = (IntPtr webView, IntPtr param, string url) =>
            {
                if (this.DownloadCallback == null)
                    throw new Exception("WebView.DownloadCallback 为空");
                return this.DownloadCallback(this, url);

            };
            this._ConsoleCallback = (IntPtr webView, IntPtr param, ConsoleLevel level, IntPtr message, IntPtr sourceName, uint sourceLine, IntPtr stackTrace) =>
            {
                if (this.ConsoleCallback == null)
                    throw new Exception("WebView.ConsoleCallback 为空");
                this.ConsoleCallback(this, level, MiniBlink.NativeMethods.WkeToStringW(message), MiniBlink.NativeMethods.WkeToStringW(sourceName), sourceLine, MiniBlink.NativeMethods.WkeToStringW(stackTrace));

            };
            this._LoadUrlBeginCallback = (IntPtr webView, IntPtr param, string url, IntPtr job) =>
            {
                if (this.LoadUrlBeginCallback == null)
                    throw new Exception("WebView.LoadUrlBeginCallback 为空");
                return this.LoadUrlBeginCallback(this, url, new Job(job));
            };
            this._LoadUrlEndCallback = (IntPtr webView, IntPtr param, string url, IntPtr job, IntPtr buf, int len) =>
            {
                if (this.LoadUrlEndCallback == null)
                    throw new Exception("WebView.LoadUrlEndCallback 为空");
                this.LoadUrlEndCallback(this, url, new Job(job), buf, len);

            };
            this._DidCreateScriptContextCallback = (IntPtr webView, IntPtr param, IntPtr frameId, IntPtr context, int extensionGroup, int worldId) =>
            {
                if (this.DidCreateScriptContextCallback == null)
                    throw new Exception("WebView.DidCreateScriptContextCallback 为空");
                this.DidCreateScriptContextCallback(this, frameId, context, extensionGroup, worldId);

            };
            this._WillReleaseScriptContextCallback = (IntPtr webView, IntPtr param, IntPtr frameId, IntPtr context, int worldId) =>
            {
                if (this.WillReleaseScriptContextCallback == null)
                    throw new Exception("WebView.WillReleaseScriptContextCallback 为空");
                this.WillReleaseScriptContextCallback(this, frameId, context, worldId);

            };
            this._NetResponseCallback = (IntPtr webView, IntPtr param, string url, IntPtr job) =>
            {
                if (this.NetResponseCallback == null)
                    throw new Exception("WebView.NetResponseCallback 为空");
                return this.NetResponseCallback(this, url, new Job(job));
            };
        }
    }
    public delegate void TitleChangedCallback(WebView webView, string title);

    public delegate void URLChangedCallback(WebView webView, string url);


    public delegate void URLChangedCallback2(WebView webView, IntPtr frameId, string url);

    public delegate void PaintUpdatedCallback(WebView webView, IntPtr hdc, int x, int y, int cx, int cy);

    public delegate void AlertBoxCallback(WebView webView, string msg);

    public delegate bool ConfirmBoxCallback(WebView webView, string msg);

    public delegate bool PromptBoxCallback(WebView webView, string msg, string defaultResult, string result);

    public delegate bool NavigationCallback(WebView webView, NavigationType navigationType, string url);


    public delegate WebView CreateViewCallback(WebView webView, NavigationType navigationType, string url, WindowFeatures windowFeatures);

    public delegate void DocumentReadyCallback(WebView webView);

    public delegate void DocumentReady2Callback(WebView webView, IntPtr frameId);

    public delegate void LoadingFinishCallback(WebView webView, String url, LoadingResult result, string failedReason);

    public delegate bool DownloadCallback(WebView webView, string url);
    public delegate void ConsoleCallback(WebView webView, ConsoleLevel level, string message, string sourceName, uint sourceLine, string stackTrace);

    //public delegate void OnCallUiThread(WebView webView, IntPtr paramOnInThread);

    //delegate void CallUiThread(WebView webView, OnCallUiThread func, IntPtr param);

    public delegate bool LoadUrlBeginCallback(WebView webView, string url, Job job);

    public delegate void LoadUrlEndCallback(WebView webView, string url, Job job, IntPtr buf, int len);

    public delegate void DidCreateScriptContextCallback(WebView webView, IntPtr frameId, IntPtr context, int extensionGroup, int worldId);

    public delegate void WillReleaseScriptContextCallback(WebView webView, IntPtr frameId, IntPtr context, int worldId);

    public delegate bool NetResponseCallback(WebView webView, string url, Job job);

    public delegate JsValue JsNativeFunction(JsExecState es);


}
