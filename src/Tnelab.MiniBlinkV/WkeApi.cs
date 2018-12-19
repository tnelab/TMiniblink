using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Tnelab.MiniBlink
{
    using wkeWebView = IntPtr;
    using wkeWebFrameHandle = IntPtr;
    using wkeString = IntPtr;
    using jsExecState = IntPtr;
    using jsValue = System.Int64;
    using HWND = IntPtr;
    using HDC = IntPtr;
    using WPARAM = UInt32;
    using LPARAM = UInt32;
    using LRESULT = UInt32;
    using size_t = UInt32;
    using v8ContextPtr = IntPtr;
    using v8Isolate = IntPtr;
    [SuppressUnmanagedCodeSecurity]
    static class NativeMethods
    {
        static NativeMethods()
        {
            NativeMethods.wkeInitialize();
        }
        //public const string MiniBlinkDll = "node_v8_5_7.dll";
        public const string MiniBlinkDll = "node.dll";
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeInit();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeShutdown();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static UInt16 wkeVersion();
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate string wkeVersionString();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeGC(wkeWebView webView, long delayMs);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static string wkeWebViewName(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeSetWebViewName(wkeWebView webView, string name);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeIsLoaded(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeIsLoadFailed(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeIsLoadComplete(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr wkeGetSource(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static string wkeTitle(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static string wkeTitleW(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static int wkeWidth(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static int wkeHeight(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static int wkeContentsWidth(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static int wkeContentsHeight(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSelectAll(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeCopy(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeCut(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkePaste(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeDelete(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeCookieEnabled(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static float wkeMediaVolume(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeMouseEvent(wkeWebView webView, UInt16 message, int x, int y, UInt16 flags);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeContextMenuEvent(wkeWebView webView, int x, int y, UInt16 flags);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeMouseWheel(wkeWebView webView, int x, int y, int delta, UInt16 flags);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeKeyUp(wkeWebView webView, UInt16 virtualKeyCode, UInt16 flags, bool systemKey);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeKeyDown(wkeWebView webView, UInt16 virtualKeyCode, UInt16 flags, bool systemKey);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeKeyPress(wkeWebView webView, UInt16 charCode, UInt16 flags, bool systemKey);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeFocus(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeUnfocus(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static Rect wkeGetCaret(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeAwaken(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static float wkeZoomFactor(wkeWebView webView);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ON_TITLE_CHANGED(wkeClientHandler clientHandler, string title);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ON_URL_CHANGED(wkeClientHandler clientHandler, string url);
        public class wkeClientHandler
        {
            public ON_TITLE_CHANGED onTitleChanged { get; set; }
            public ON_URL_CHANGED onURLChanged { get; set; }
        }

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetClientHandler(wkeWebView webView, wkeClientHandler handler);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static wkeClientHandler wkeGetClientHandler(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr wkeToString(wkeString str);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr wkeToStringW(wkeString str);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr jsToString(jsExecState es, jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr jsToStringW(jsExecState es, jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeInitialize();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeInitializeEx(Settings settings);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeConfigure(Settings settings);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeIsInitialize();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetViewSettings(wkeWebView webView, ViewSettings settings);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeSetDebugConfig(wkeWebView webView, string debugString, string param);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeFinalize();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeUpdate();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static uint wkeGetVersion();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static string wkeGetVersionString();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static wkeWebView wkeCreateWebView();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static wkeWebView wkeGetWebView(string name);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeDestroyWebView(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetMemoryCacheEnable(wkeWebView webView, bool b);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetTouchEnabled(wkeWebView webView, bool b);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetNavigationToNewWindowEnable(wkeWebView webView, bool b);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetCspCheckEnable(wkeWebView webView, bool b);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetNpapiPluginsEnabled(wkeWebView webView, bool b);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetHeadlessEnabled(wkeWebView webView, bool b);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetDragEnable(wkeWebView webView, bool b);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeSetViewNetInterface(wkeWebView webView, string netInterface);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeSetProxy(Proxy proxy);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeSetViewProxy(wkeWebView webView, Proxy proxy);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static string wkeGetName(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeSetName(wkeWebView webView, string name);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetHandle(wkeWebView webView, HWND wnd);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetHandleOffset(wkeWebView webView, int x, int y);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeIsTransparent(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetTransparent(wkeWebView webView, bool transparent);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeSetUserAgent(wkeWebView webView, string userAgent);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void wkeSetUserAgentW(wkeWebView webView, string userAgent);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void wkeLoadW(wkeWebView webView, string url);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeLoadURL(wkeWebView webView, string url);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void wkeLoadURLW(wkeWebView webView, string url);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkePostURL(wkeWebView wkeView, string url, string postData, int postLen);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void wkePostURLW(wkeWebView wkeView, string url, string postData, int postLen);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeLoadHTML(wkeWebView webView, [MarshalAs(UnmanagedType.LPStr)]string html);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void wkeLoadHTMLW(wkeWebView webView, string html);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeLoadFile(wkeWebView webView, [MarshalAs(UnmanagedType.LPStr)]string filename);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void wkeLoadFileW(wkeWebView webView, string filename);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static string wkeGetURL(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeIsLoading(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeIsLoadingSucceeded(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeIsLoadingFailed(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeIsLoadingCompleted(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeIsDocumentReady(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeStopLoading(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeReload(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static string wkeGetTitle(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static string wkeGetTitleW(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeResize(wkeWebView webView, int w, int h);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static int wkeGetWidth(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static int wkeGetHeight(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static int wkeGetContentWidth(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static int wkeGetContentHeight(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetDirty(wkeWebView webView, [MarshalAs(UnmanagedType.I1)]bool dirty);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeIsDirty(wkeWebView webView);
        //[DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl,EntryPoint = "wkeIsDirty")]
        //public extern static int wkeIsDirty2(wkeWebView webView);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeAddDirtyArea(wkeWebView webView, int x, int y, int w, int h);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeLayoutIfNeeded(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkePaint2(wkeWebView webView, IntPtr bits, int bufWid, int bufHei, int xDst, int yDst, int w, int h, int xSrc, int ySrc, bool bCopyAlpha);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkePaint(wkeWebView webView, IntPtr bits, int pitch);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeRepaintIfNeeded(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static HDC wkeGetViewDC(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static HWND wkeGetHostHWND(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeCanGoBack(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeGoBack(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeCanGoForward(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeGoForward(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeEditorSelectAll(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeEditorUnSelect(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeEditorCopy(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeEditorCut(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeEditorPaste(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeEditorDelete(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeEditorUndo(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeEditorRedo(wkeWebView webView);
        [DllImport(MiniBlinkDll, CharSet = CharSet.Unicode)]
        public extern static string wkeGetCookieW(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static string wkeGetCookie(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeVisitAllCookie(IntPtr pparams, CookieVisitor visitor);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkePerformCookieCommand(CookieCommand command);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetCookieEnabled(wkeWebView webView, [MarshalAs(UnmanagedType.I1)]bool enable);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeIsCookieEnabled(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void wkeSetCookieJarPath(wkeWebView webView, string path);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void wkeSetCookieJarFullPath(wkeWebView webView, string path);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void wkeSetLocalStorageFullPath(wkeWebView webView, string path);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetMediaVolume(wkeWebView webView, float volume);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static float wkeGetMediaVolume(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeFireMouseEvent(wkeWebView webView, uint message, int x, int y, uint flags);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeFireContextMenuEvent(wkeWebView webView, int x, int y, uint flags);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeFireMouseWheelEvent(wkeWebView webView, int x, int y, int delta, int flags);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeFireKeyUpEvent(wkeWebView webView, uint virtualKeyCode, uint flags, [MarshalAs(UnmanagedType.I1)]bool systemKey);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeFireKeyDownEvent(wkeWebView webView, uint virtualKeyCode, uint flags, [MarshalAs(UnmanagedType.I1)]bool systemKey);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeFireKeyPressEvent(wkeWebView webView, uint charCode, uint flags, [MarshalAs(UnmanagedType.I1)]bool systemKey);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeFireWindowsMessage(wkeWebView webView, HWND hWnd, uint message, WPARAM wParam, LPARAM lParam, out LRESULT result);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetFocus(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeKillFocus(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static Rect wkeGetCaretRect(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static jsValue wkeRunJS(wkeWebView webView, [MarshalAs(UnmanagedType.LPStr)]string script);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static jsValue wkeRunJSW(wkeWebView webView, string script);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsExecState wkeGlobalExec(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSleep(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeWake(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeIsAwake(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetZoomFactor(wkeWebView webView, float factor);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static float wkeGetZoomFactor(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetEditable(wkeWebView webView, [MarshalAs(UnmanagedType.I1)]bool editable);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr wkeGetString(wkeString str);
        [DllImport(MiniBlinkDll, CharSet = CharSet.Unicode)]
        public extern static IntPtr wkeGetStringW(wkeString str);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeSetString(wkeString str, [MarshalAs(UnmanagedType.LPStr)]string str2, size_t len);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void wkeSetStringW(wkeString str, string str2, size_t len);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static wkeString wkeCreateStringW(string str, size_t len);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeDeleteString(wkeString str);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static wkeWebView wkeGetWebViewForCurrentContext();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeSetUserKeyValue(wkeWebView webView, string key, IntPtr value);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static IntPtr wkeGetUserKeyValue(wkeWebView webView, string key);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static int wkeGetCursorInfoType(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetDragFiles(wkeWebView webView, POINT clintPos, POINT screenPos, wkeString[] files, int filesCount);
        // blink内部窗口创建回调，例如下拉框
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnBlinkWindowCreate();
        //wke callback-----------------------------------------------------------------------------------
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnTitleChanged(wkeWebView webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkeTitleChangedCallback callback, IntPtr callbackParam);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnMouseOverUrlChanged(wkeWebView webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkeTitleChangedCallback callback, IntPtr callbackParam);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnURLChanged(wkeWebView webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkeURLChangedCallback callback, IntPtr callbackParam);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnURLChanged2(wkeWebView webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkeURLChangedCallback2 callback, IntPtr callbackParam);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnPaintUpdated(wkeWebView webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkePaintUpdatedCallback callback, IntPtr callbackParam);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnAlertBox(wkeWebView webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkeAlertBoxCallback callback, IntPtr callbackParam);


        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnConfirmBox(wkeWebView webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkeConfirmBoxCallback callback, IntPtr callbackParam);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnPromptBox(wkeWebView webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkePromptBoxCallback callback, IntPtr callbackParam);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnNavigation(wkeWebView webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkeNavigationCallback callback, IntPtr param);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnCreateView(wkeWebView webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkeCreateViewCallback callback, IntPtr param);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnDocumentReady(wkeWebView webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkeDocumentReadyCallback callback, IntPtr param);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnDocumentReady2(wkeWebView webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkeDocumentReady2Callback callback, IntPtr param);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnLoadingFinish(wkeWebView webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkeLoadingFinishCallback callback, IntPtr param);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnDownload(wkeWebView webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkeDownloadCallback callback, IntPtr param);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnConsole(wkeWebView webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkeConsoleCallback callback, IntPtr param);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetUIThreadCallback(wkeWebView webView, wkeCallUiThread callback, IntPtr param);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnLoadUrlBegin(wkeWebView webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkeLoadUrlBeginCallback callback, IntPtr callbackParam);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnLoadUrlEnd(wkeWebView webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkeLoadUrlEndCallback callback, IntPtr callbackParam);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnDidCreateScriptContext(wkeWebView webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkeDidCreateScriptContextCallback callback, IntPtr callbackParam);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnWillReleaseScriptContext(wkeWebView webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkeWillReleaseScriptContextCallback callback, IntPtr callbackParam);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeNetSetMIMEType(IntPtr job, string type);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeNetSetHTTPHeaderField(IntPtr job, string key, string value, [MarshalAs(UnmanagedType.I1)]bool response);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeNetSetURL(IntPtr job, string url);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeNetSetData(IntPtr job, IntPtr buf, int len);
        // 调用此函数后,网络层收到数据会存储在一buf内,接收数据完成后响应OnLoadUrlEnd事件.#此调用严重影响性能,慎用
        // 此函数和wkeNetSetData的区别是，wkeNetHookRequest会在接受到真正网络数据后再调用回调，并允许回调修改网络数据。
        // 而wkeNetSetData是在网络数据还没发送的时候修改
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeNetHookRequest(IntPtr job);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeNetOnResponse(wkeWebView webView, wkeNetResponseCallback callback, IntPtr param);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeNetGetMIMEType(IntPtr job, wkeString mime);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeIsMainFrame(wkeWebView webView, wkeWebFrameHandle frameId);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeIsWebRemoteFrame(wkeWebView webView, wkeWebFrameHandle frameId);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static wkeWebFrameHandle wkeWebFrameGetMainFrame(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static jsValue wkeRunJsByFrame(wkeWebView webView, wkeWebFrameHandle frameId, [MarshalAs(UnmanagedType.LPStr)]string script, [MarshalAs(UnmanagedType.I1)]bool isInClosure);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static string wkeGetFrameUrl(wkeWebView webView, wkeWebFrameHandle frameId);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeWebFrameGetMainWorldScriptContext(wkeWebView webView, wkeWebFrameHandle webFrameId, out v8ContextPtr contextOut);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static v8Isolate wkeGetBlinkMainThreadIsolate();

        //wkewindow-----------------------------------------------------------------------------------
        public enum wkeWindowType
        {
            WKE_WINDOW_TYPE_POPUP,
            WKE_WINDOW_TYPE_TRANSPARENT,
            WKE_WINDOW_TYPE_CONTROL

        }


        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static wkeWebView wkeCreateWebWindow(wkeWindowType type, HWND parent, int x, int y, int width, int height);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeDestroyWebWindow(wkeWebView webWindow);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static HWND wkeGetWindowHandle(wkeWebView webWindow);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool wkeWindowClosingCallback(wkeWebView webWindow, IntPtr param);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnWindowClosing(wkeWebView webWindow, wkeWindowClosingCallback callback, IntPtr param);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeWindowDestroyCallback(wkeWebView webWindow, IntPtr param);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnWindowDestroy(wkeWebView webWindow, wkeWindowDestroyCallback callback, IntPtr param);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeShowWindow(wkeWebView webWindow, [MarshalAs(UnmanagedType.I1)]bool show);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeEnableWindow(wkeWebView webWindow, [MarshalAs(UnmanagedType.I1)]bool enable);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeMoveWindow(wkeWebView webWindow, int x, int y, int width, int height);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeMoveToCenter(wkeWebView webWindow);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeResizeWindow(wkeWebView webWindow, int width, int height);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeSetWindowTitle(wkeWebView webWindow, string title);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void wkeSetWindowTitleW(wkeWebView webWindow, string title);
        //JavaScript Bind-----------------------------------------------------------------------------------
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jsValue jsNativeFunction(jsExecState es);


        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void jsBindFunction(string name, jsNativeFunction fn, uint argCount);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void jsBindGetter(string name, jsNativeFunction fn); /*get property*/
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void jsBindSetter(string name, jsNativeFunction fn); /*set property*/

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeJsBindFunction(string name, [MarshalAs(UnmanagedType.FunctionPtr)]wkeJsNativeFunction fn, IntPtr param, uint argCount);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeJsBindGetter(string name, wkeJsNativeFunction fn, IntPtr param); /*get property*/
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeJsBindSetter(string name, wkeJsNativeFunction fn, IntPtr param); /*set property*/

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static int jsArgCount(jsExecState es);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static JsType jsArgType(jsExecState es, int argIdx);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsArg(jsExecState es, int argIdx);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static JsType jsTypeOf(jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool jsIsNumber(jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool jsIsString(jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool jsIsBoolean(jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool jsIsObject(jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool jsIsFunction(jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static bool jsIsUndefined(jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool jsIsNull(jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool jsIsArray(jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool jsIsTrue(jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool jsIsFalse(jsValue v);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static int jsToInt(jsExecState es, jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static float jsToFloat(jsExecState es, jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static double jsToDouble(jsExecState es, jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool jsToBoolean(jsExecState es, jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static string jsToTempString(jsExecState es, jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.LPTStr)]
        public extern static string jsToTempStringW(jsExecState es, jsValue v);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsInt(int n);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsFloat(float f);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsDouble(double d);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsBoolean([MarshalAs(UnmanagedType.I1)]bool b);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsUndefined();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsNull();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsTrue();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsFalse();

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static jsValue jsString(jsExecState es, string str);
        [DllImport(MiniBlinkDll, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsStringW(jsExecState es, string str);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsEmptyObject(jsExecState es);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsEmptyArray(jsExecState es);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsObject(jsExecState es, IntPtr obj);
        //[DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        //public extern static jsValue jsObject(jsExecState es, JsData obj);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static jsValue jsFunction(jsExecState es, JsData obj);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr jsGetData(jsExecState es, jsValue obj);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static jsValue jsGet(jsExecState es, jsValue obj, string prop);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void jsSet(jsExecState es, jsValue obj, string prop, jsValue v);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsGetAt(jsExecState es, jsValue obj, int index);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void jsSetAt(jsExecState es, jsValue obj, int index, jsValue v);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static int jsGetLength(jsExecState es, jsValue obj);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void jsSetLength(jsExecState es, jsValue obj, int length);

        //window object
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsGlobalObject(jsExecState es);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static wkeWebView jsGetWebView(jsExecState es);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static jsValue jsEval(jsExecState es, string str);
        [DllImport(MiniBlinkDll, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsEvalW(jsExecState es, string str);
        [DllImport(MiniBlinkDll, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsEvalExW(jsExecState es, string str, [MarshalAs(UnmanagedType.I1)]bool isInClosure);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsCall(jsExecState es, jsValue func, jsValue thisObject, jsValue[] args, int argCount);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsCallGlobal(jsExecState es, jsValue func, jsValue[] args, int argCount);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static jsValue jsGetGlobal(jsExecState es, string prop);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void jsSetGlobal(jsExecState es, string prop, jsValue v);

        //garbage collect
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void jsGC();


        public static string WkeGetSource(wkeWebView webView)
        {
            return Marshal.PtrToStringAnsi(webView);
        }
        public static string WkeToString(wkeString str)
        {
            return Marshal.PtrToStringAnsi(wkeToString(str));
        }
        public static string WkeToStringW(wkeString str)
        {
            return Marshal.PtrToStringUni(wkeToStringW(str));
        }
        public static string JsToString(IntPtr es, jsValue v)
        {
            return Marshal.PtrToStringAnsi(jsToString(es, v));
        }
        public static string JsToStringW(IntPtr es, jsValue v)
        {
            return Marshal.PtrToStringUni(jsToStringW(es, v));
        }
        public static IntPtr WkeCreateStringW(string mime)
        {
            return wkeCreateStringW(mime, (uint)mime.Length);
        }
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.I1)]
    public delegate bool CookieVisitor(
        IntPtr pparams,
        string name,
        string value,
        string domain,
        string path, // If |path| is non-empty only URLs at or below the path will get the cookie value.
        int secure, // If |secure| is true the cookie will only be sent for HTTPS requests.
        int httpOnly, // If |httponly| is true the cookie will only be sent for HTTP requests.
        ref int expires // The cookie expiration date is only valid if |has_expires| is true.
    );
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int x;
        public int y;
        public int w;
        public int h;
    }
    public enum MouseFlags
    {
        LBUTTON = 0x01,
        RBUTTON = 0x02,
        SHIFT = 0x04,
        CONTROL = 0x08,
        MBUTTON = 0x10,
    }
    public enum KeyFlags
    {
        EXTENDED = 0x0100,
        REPEAT = 0x4000,
    }
    public enum MouseMsg
    {
        MSG_MOUSEMOVE = 0x0200,
        MSG_LBUTTONDOWN = 0x0201,
        MSG_LBUTTONUP = 0x0202,
        MSG_LBUTTONDBLCLK = 0x0203,
        MSG_RBUTTONDOWN = 0x0204,
        MSG_RBUTTONUP = 0x0205,
        MSG_RBUTTONDBLCLK = 0x0206,
        MSG_MBUTTONDOWN = 0x0207,
        MSG_MBUTTONUP = 0x0208,
        MSG_MBUTTONDBLCLK = 0x0209,
        MSG_MOUSEWHEEL = 0x020A,
    }
    public enum ProxyType
    {
        PROXY_NONE,
        PROXY_HTTP,
        PROXY_SOCKS4,
        PROXY_SOCKS4A,
        PROXY_SOCKS5,
        PROXY_SOCKS5HOSTNAME
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class Proxy
    {
        public ProxyType type;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public char[] hostname;
        public UInt16 port;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
        public char[] username;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
        public char[] password;
    }
    public enum SettingMask
    {
        SETTING_PROXY = 1,
        SETTING_PAINTCALLBACK_IN_OTHER_THREAD = 1 << 2,
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Settings
    {
        public Proxy proxy;
        public UInt16 mask;
    }
    [StructLayout(LayoutKind.Sequential)]
    public class ViewSettings
    {
        public int size;
        public UInt16 bgColor;
    }
    [StructLayout(LayoutKind.Sequential)]
    public class POINT
    {
        long x;
        long y;
    }
    public enum CookieCommand
    {
        CookieCommandClearAllCookies,
        CookieCommandClearSessionCookies,
        CookieCommandFlushCookiesToFile,
        CookieCommandReloadCookiesFromFile,
    }
    public enum CursorInfoType
    {
        CursorInfoPointer,
        CursorInfoCross,
        CursorInfoHand,
        CursorInfoIBeam,
        CursorInfoWait,
        CursorInfoHelp,
        CursorInfoEastResize,
        CursorInfoNorthResize,
        CursorInfoNorthEastResize,
        CursorInfoNorthWestResize,
        CursorInfoSouthResize,
        CursorInfoSouthEastResize,
        CursorInfoSouthWestResize,
        CursorInfoWestResize,
        CursorInfoNorthSouthResize,
        CursorInfoEastWestResize,
        CursorInfoNorthEastSouthWestResize,
        CursorInfoNorthWestSouthEastResize,
        CursorInfoColumnResize,
        CursorInfoRowResize,
        CursorInfoMiddlePanning,
        CursorInfoEastPanning,
        CursorInfoNorthPanning,
        CursorInfoNorthEastPanning,
        CursorInfoNorthWestPanning,
        CursorInfoSouthPanning,
        CursorInfoSouthEastPanning,
        CursorInfoSouthWestPanning,
        CursorInfoWestPanning,
        CursorInfoMove,
        CursorInfoVerticalText,
        CursorInfoCell,
        CursorInfoContextMenu,
        CursorInfoAlias,
        CursorInfoProgress,
        CursorInfoNoDrop,
        CursorInfoCopy,
        CursorInfoNone,
        CursorInfoNotAllowed,
        CursorInfoZoomIn,
        CursorInfoZoomOut,
        CursorInfoGrab,
        CursorInfoGrabbing,
        CursorInfoCustom
    }
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void wkeTitleChangedCallback(IntPtr webView, IntPtr param, IntPtr title);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void wkeURLChangedCallback(IntPtr webView, IntPtr param, IntPtr url);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void wkeURLChangedCallback2(IntPtr webView, IntPtr param, IntPtr frameId, IntPtr url);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void wkePaintUpdatedCallback(IntPtr webView, IntPtr param, IntPtr hdc, int x, int y, int cx, int cy);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void wkeAlertBoxCallback(IntPtr webView, IntPtr param, IntPtr msg);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    delegate bool wkeConfirmBoxCallback(IntPtr webView, IntPtr param, IntPtr msg);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    delegate bool wkePromptBoxCallback(IntPtr webView, IntPtr param, IntPtr msg, IntPtr defaultResult, IntPtr result);
    public enum NavigationType
    {
        NAVIGATION_TYPE_LINKCLICK,
        NAVIGATION_TYPE_FORMSUBMITTE,
        NAVIGATION_TYPE_BACKFORWARD,
        NAVIGATION_TYPE_RELOAD,
        NAVIGATION_TYPE_FORMRESUBMITT,
        NAVIGATION_TYPE_OTHER
    }
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    delegate bool wkeNavigationCallback(IntPtr webView, IntPtr param, NavigationType navigationType, IntPtr url);
    [StructLayout(LayoutKind.Sequential)]
    public class WindowFeatures
    {
        public int x;
        public int y;
        public int width;
        public int height;
        [MarshalAs(UnmanagedType.I1)]
        public bool menuBarVisible;
        [MarshalAs(UnmanagedType.I1)]
        public bool statusBarVisible;
        [MarshalAs(UnmanagedType.I1)]
        public bool toolBarVisible;
        [MarshalAs(UnmanagedType.I1)]
        public bool locationBarVisible;
        [MarshalAs(UnmanagedType.I1)]
        public bool scrollbarsVisible;
        [MarshalAs(UnmanagedType.I1)]
        public bool resizable;
        [MarshalAs(UnmanagedType.I1)]
        public bool fullscreen;
    }
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate IntPtr wkeCreateViewCallback(IntPtr webView, IntPtr param, NavigationType navigationType, IntPtr url, WindowFeatures windowFeatures);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void wkeDocumentReadyCallback(IntPtr webView, IntPtr param);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void wkeDocumentReady2Callback(IntPtr webView, IntPtr param, IntPtr frameId);
    public enum LoadingResult
    {
        LOADING_SUCCEEDED,
        LOADING_FAILED,
        LOADING_CANCELED
    }
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void wkeLoadingFinishCallback(IntPtr webView, IntPtr param, IntPtr url, LoadingResult result, IntPtr failedReason);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    delegate bool wkeDownloadCallback(IntPtr webView, IntPtr param, string url);
    public enum ConsoleLevel
    {
        wkeLevelDebug = 4,
        wkeLevelLog = 1,
        wkeLevelInfo = 5,
        wkeLevelWarning = 2,
        wkeLevelError = 3,
        wkeLevelRevokedError = 6,
        wkeLevelLast = wkeLevelInfo
    }
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void wkeConsoleCallback(IntPtr webView, IntPtr param, ConsoleLevel level, IntPtr message, IntPtr sourceName, uint sourceLine, IntPtr stackTrace);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void wkeOnCallUiThread(IntPtr webView, IntPtr paramOnInThread);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void wkeCallUiThread(IntPtr webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkeOnCallUiThread func, IntPtr param);
    //wkeNet--------------------------------------------------------------------------------------
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    delegate bool wkeLoadUrlBeginCallback(IntPtr webView, IntPtr param, string url, IntPtr job);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void wkeLoadUrlEndCallback(IntPtr webView, IntPtr param, string url, IntPtr job, IntPtr buf, int len);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void wkeDidCreateScriptContextCallback(IntPtr webView, IntPtr param, IntPtr frameId, IntPtr context, int extensionGroup, int worldId);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void wkeWillReleaseScriptContextCallback(IntPtr webView, IntPtr param, IntPtr frameId, IntPtr context, int worldId);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    delegate bool wkeNetResponseCallback(IntPtr webView, IntPtr param, string url, IntPtr job);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate long wkeJsNativeFunction(IntPtr es, IntPtr param);
    public enum JsType
    {
        JSTYPE_NUMBER,
        JSTYPE_STRING,
        JSTYPE_BOOLEAN,
        JSTYPE_OBJECT,
        JSTYPE_FUNCTION,
        JSTYPE_UNDEFINED,
        JSTYPE_ARRAY,
    }
    // cexer JS对象、函数绑定支持
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = true, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = true)]
    public delegate long jsGetPropertyCallback(IntPtr es, long obj, string propertyName);
    //public delegate object JsGetPropertyCallback(JsExecState es, object obj, string propertyName);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = true, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = true)]
    [return: MarshalAs(UnmanagedType.I1)]
    public delegate bool jsSetPropertyCallback(IntPtr es, long obj, string propertyName, long value);
    //public delegate bool JsSetPropertyCallback(JsExecState es, object obj, string propertyName, object value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = true, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = true)]
    public delegate long jsCallAsFunctionCallback(IntPtr es, long obj, long[] args, int argCount);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = true, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = true)]
    public delegate void jsFinalizeCallback(JsData data);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class JsData
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string typeName;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public jsGetPropertyCallback propertyGet;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public jsSetPropertyCallback propertySet;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public jsFinalizeCallback finalize;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public jsCallAsFunctionCallback callAsFunction;
    }
}
