using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Tnelab.MiniBlinkV
{
    using int64_t = Int64;
    using mbJsValue= Int64;
    using mbJsExecState=IntPtr;
    using utf8 =String;
    using utf8_ = IntPtr;
    using mbWebFrameHandle = IntPtr;
    using mbNetJob = IntPtr;
    using mbWebView = IntPtr;
    using HDC = IntPtr;
    using v8ContextPtr=IntPtr;
    using v8Isolate=IntPtr;
    using mbWebUrlRequestPtr=IntPtr;
    using mbWebUrlResponsePtr=IntPtr;
    using LONG = Int32;
    using HWND = IntPtr;

    using jsValue = Int64;
    using jsExecState = IntPtr;
    using wkeWebView = IntPtr;

    [SuppressUnmanagedCodeSecurity]
    public static class NativeMethods
    {
        const string MiniBlinkVDll = "mb.dll";
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public LONG left;
            public LONG top;
            public LONG right;
            public LONG bottom;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct mbRect
        {
            public int x;
            public int y;
            public int w;
            public int h;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct mbPoint
        {
            public int x;
            public int y;
        }
        public enum mbMouseFlags
        {
            MB_LBUTTON = 0x01,
            MB_RBUTTON = 0x02,
            MB_SHIFT = 0x04,
            MB_CONTROL = 0x08,
            MB_MBUTTON = 0x10,
        }
        public enum mbKeyFlags
        {
            MB_EXTENDED = 0x0100,
            MB_REPEAT = 0x4000,
        }
        public enum mbMouseMsg
        {
            MB_MSG_MOUSEMOVE = 0x0200,
            MB_MSG_LBUTTONDOWN = 0x0201,
            MB_MSG_LBUTTONUP = 0x0202,
            MB_MSG_LBUTTONDBLCLK = 0x0203,
            MB_MSG_RBUTTONDOWN = 0x0204,
            MB_MSG_RBUTTONUP = 0x0205,
            MB_MSG_RBUTTONDBLCLK = 0x0206,
            MB_MSG_MBUTTONDOWN = 0x0207,
            MB_MSG_MBUTTONUP = 0x0208,
            MB_MSG_MBUTTONDBLCLK = 0x0209,
            MB_MSG_MOUSEWHEEL = 0x020A,
        }
        public enum mbProxyType
        {
            MB_PROXY_NONE,
            MB_PROXY_HTTP,
            MB_PROXY_SOCKS4,
            MB_PROXY_SOCKS4A,
            MB_PROXY_SOCKS5,
            MB_PROXY_SOCKS5HOSTNAME
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct mbProxy
        {
            public mbProxyType type;
            public fixed char hostname[100];
            public UInt16 port;
            public fixed char username[50];
            public fixed char password[50];
        }

        public enum mbSettingMask
        {
            MB_SETTING_PROXY = 1,
            MB_SETTING_PAINTCALLBACK_IN_OTHER_THREAD = 1 << 2,
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct mbSettings
        {
            public mbProxy proxy;
            public uint mask;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct mbViewSettings
        {
            public int size;
            public uint bgColor;
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall,CharSet =CharSet.Ansi)]
        public delegate bool mbCookieVisitor(
            IntPtr _params,
            string name,
            string value,
            string domain,
            string  path, // If |path| is non-empty only URLs at or below the path will get the cookie value.
            int secure, // If |secure| is true the cookie will only be sent for HTTPS requests.
            int httpOnly, // If |httponly| is true the cookie will only be sent for HTTP requests.
            ref int expires // The cookie expiration date is only valid if |has_expires| is true.
        );

        public enum mbCookieCommand
        {
            mbCookieCommandClearAllCookies,
            mbCookieCommandClearSessionCookies,
            mbCookieCommandFlushCookiesToFile,
            mbCookieCommandReloadCookiesFromFile,
        } 

        public enum mbNavigationType
        {
            MB_NAVIGATION_TYPE_LINKCLICK,
            MB_NAVIGATION_TYPE_FORMSUBMITTE,
            MB_NAVIGATION_TYPE_BACKFORWARD,
            MB_NAVIGATION_TYPE_RELOAD,
            MB_NAVIGATION_TYPE_FORMRESUBMITT,
            MB_NAVIGATION_TYPE_OTHER
        }

        public enum mbCursorInfoType
        {
            kMbCursorInfoPointer,
            kMbCursorInfoCross,
            kMbCursorInfoHand,
            kMbCursorInfoIBeam,
            kMbCursorInfoWait,
            kMbCursorInfoHelp,
            kMbCursorInfoEastResize,
            kMbCursorInfoNorthResize,
            kMbCursorInfoNorthEastResize,
            kMbCursorInfoNorthWestResize,
            kMbCursorInfoSouthResize,
            kMbCursorInfoSouthEastResize,
            kMbCursorInfoSouthWestResize,
            kMbCursorInfoWestResize,
            kMbCursorInfoNorthSouthResize,
            kMbCursorInfoEastWestResize,
            kMbCursorInfoNorthEastSouthWestResize,
            kMbCursorInfoNorthWestSouthEastResize,
            kMbCursorInfoColumnResize,
            kMbCursorInfoRowResize,
            kMbCursorInfoMiddlePanning,
            kMbCursorInfoEastPanning,
            kMbCursorInfoNorthPanning,
            kMbCursorInfoNorthEastPanning,
            kMbCursorInfoNorthWestPanning,
            kMbCursorInfoSouthPanning,
            kMbCursorInfoSouthEastPanning,
            kMbCursorInfoSouthWestPanning,
            kMbCursorInfoWestPanning,
            kMbCursorInfoMove,
            kMbCursorInfoVerticalText,
            kMbCursorInfoCell,
            kMbCursorInfoContextMenu,
            kMbCursorInfoAlias,
            kMbCursorInfoProgress,
            kMbCursorInfoNoDrop,
            kMbCursorInfoCopy,
            kMbCursorInfoNone,
            kMbCursorInfoNotAllowed,
            kMbCursorInfoZoomIn,
            kMbCursorInfoZoomOut,
            kMbCursorInfoGrab,
            kMbCursorInfoGrabbing,
            kMbCursorInfoCustom
        }
        [StructLayout(LayoutKind.Sequential)]
        public  struct mbWindowFeatures
        {
            public int x;
            public int y;
            public int width;
            public int height;

            public bool menuBarVisible;
            public bool statusBarVisible;
            public bool toolBarVisible;
            public bool locationBarVisible;
            public bool scrollbarsVisible;
            public bool resizable;
            public bool fullscreen;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct mbMemBuf
        {
            public int size;
            public IntPtr data;
            public IntPtr length;
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct mbWebDragData
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct Item {
                public enum mbStorageType
                {
                    // String data with an associated MIME type. Depending on the MIME type, there may be
                    // optional metadata attributes as well.
                    StorageTypeString,
                    // Stores the name of one file being dragged into the renderer.
                    StorageTypeFilename,
                    // An image being dragged out of the renderer. Contains a buffer holding the image data
                    // as well as the suggested name for saving the image to.
                    StorageTypeBinaryData,
                    // Stores the filesystem URL of one file being dragged into the renderer.
                    StorageTypeFileSystemFile,
                }
                public mbStorageType storageType;

                // Only valid when storageType == StorageTypeString.
                public mbMemBuf* stringType;
                public mbMemBuf* stringData;

                // Only valid when storageType == StorageTypeFilename.
                public mbMemBuf* filenameData;
                public mbMemBuf* displayNameData;

                // Only valid when storageType == StorageTypeBinaryData.
                public mbMemBuf* binaryData;

                // Title associated with a link when stringType == "text/uri-list".
                // Filename when storageType == StorageTypeBinaryData.
                public mbMemBuf* title;

                // Only valid when storageType == StorageTypeFileSystemFile.
                public mbMemBuf* fileSystemURL;
                public Int64 fileSystemFileSize;

                // Only valid when stringType == "text/html".
                public mbMemBuf* baseURL;
            }

            public IntPtr m_itemList;
            public int m_itemListLength;

            public int m_modifierKeyState; // State of Shift/Ctrl/Alt/Meta keys.
            public IntPtr m_filesystemId;
        }

        public enum mbWebDragOperation
        {
            mbWebDragOperationNone = 0,
            mbWebDragOperationCopy = 1,
            mbWebDragOperationLink = 2,
            mbWebDragOperationGeneric = 4,
            mbWebDragOperationPrivate = 8,
            mbWebDragOperationMove = 16,
            mbWebDragOperationDelete = 32,
            mbWebDragOperationEvery = unchecked((int)0xffffffff)
        }

        //public static mbWebDragOperation mbWebDragOperationsMask;

        public enum mbResourceType
        {
            MB_RESOURCE_TYPE_MAIN_FRAME = 0,       // top level page
            MB_RESOURCE_TYPE_SUB_FRAME = 1,        // frame or iframe
            MB_RESOURCE_TYPE_STYLESHEET = 2,       // a CSS stylesheet
            MB_RESOURCE_TYPE_SCRIPT = 3,           // an external script
            MB_RESOURCE_TYPE_IMAGE = 4,            // an image (jpg/gif/png/etc)
            MB_RESOURCE_TYPE_FONT_RESOURCE = 5,    // a font
            MB_RESOURCE_TYPE_SUB_RESOURCE = 6,     // an "other" subresource.
            MB_RESOURCE_TYPE_OBJECT = 7,           // an object (or embed) tag for a plugin,
                                            // or a resource that a plugin requested.
            MB_RESOURCE_TYPE_MEDIA = 8,            // a media resource.
            MB_RESOURCE_TYPE_WORKER = 9,           // the main resource of a dedicated
                                            // worker.
            MB_RESOURCE_TYPE_SHARED_WORKER = 10,   // the main resource of a shared worker.
            MB_RESOURCE_TYPE_PREFETCH = 11,        // an explicitly requested prefetch
            MB_RESOURCE_TYPE_FAVICON = 12,         // a favicon
            MB_RESOURCE_TYPE_XHR = 13,             // a XMLHttpRequest
            MB_RESOURCE_TYPE_PING = 14,            // a ping request for <a ping>
            MB_RESOURCE_TYPE_SERVICE_WORKER = 15,  // the main resource of a service worker.
            MB_RESOURCE_TYPE_LAST_TYPE
        }

        public enum _mbRequestType {
            kMbRequestTypeInvalidation,
            kMbRequestTypeGet,
            kMbRequestTypePost,
            kMbRequestTypePut,
        }
        //public static _mbRequestType mbRequestType;

        //////////////////////////////////////////////////////////////////////////

        public enum mbJsType
        {
            kMbJsTypeNumber = 0,
            kMbJsTypeString = 1,
            kMbJsTypebool = 2,
            //kMbJsTypeObject = 3,
            //kMbJsTypeFunction = 4,
            kMbJsTypeUndefined  = 5,
            //kMbJsTypeArray = 6,
            kMbJsTypeNull = 7,
        }
        public static string Ptr2Utf8(IntPtr ptr)
        {
            List<byte> bytes = new List<byte>();
            var b = Marshal.ReadByte(ptr);
            while (b != 0)
            {
                bytes.Add(b);
                ptr += 1;
                b = Marshal.ReadByte(ptr);                
            }
            var result = Encoding.UTF8.GetString(bytes.ToArray());
            return result;
        }
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbRunJsCallback(mbWebView webView, IntPtr param, mbJsExecState es, mbJsValue v);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbJsQueryCallback(mbWebView webView, IntPtr param, mbJsExecState es, int64_t queryId, int customMsg,utf8_ request);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void mbTitleChangedCallback(mbWebView webView, IntPtr param,  utf8_ title);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbURLChangedCallback(mbWebView webView, IntPtr param, utf8 url, bool canGoBack, bool canGoForward);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbURLChangedCallback2(mbWebView webView, IntPtr param, mbWebFrameHandle frameId, utf8 url);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbPaintUpdatedCallback(mbWebView webView, IntPtr param, HDC hdc, int x, int y, int cx, int cy);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbPaintBitUpdatedCallback(mbWebView webView, IntPtr param, IntPtr buffer, ref mbRect r, int width, int height);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbAlertBoxCallback(mbWebView webView, IntPtr param, utf8 msg);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate bool mbConfirmBoxCallback(mbWebView webView, IntPtr param, utf8 msg);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate bool mbPromptBoxCallback(mbWebView webView, IntPtr param, utf8 msg, utf8 defaultResult, utf8 result);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate bool mbNavigationCallback(mbWebView webView, IntPtr param, mbNavigationType navigationType, utf8 url);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate mbWebView mbCreateViewCallback(mbWebView webView, IntPtr param, mbNavigationType navigationType, utf8 url, ref mbWindowFeatures windowFeatures);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbDocumentReadyCallback(mbWebView webView, IntPtr param, mbWebFrameHandle frameId);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbOnShowDevtoolsCallback(mbWebView webView, IntPtr param);

        public enum mbLoadingResult
        {
            MB_LOADING_SUCCEEDED,
            MB_LOADING_FAILED,
            MB_LOADING_CANCELED
        }
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbLoadingFinishCallback(mbWebView webView, IntPtr param, mbWebFrameHandle frameId, utf8 url, mbLoadingResult result, utf8 failedReason);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate bool mbDownloadCallback(mbWebView webView, IntPtr param, mbWebFrameHandle frameId, string url);

        public enum mbConsoleLevel
        {
            mbLevelDebug = 4,   
            mbLevelLog = 1,
            mbLevelInfo = 5,
            mbLevelWarning = 2,
            mbLevelError = 3,
            mbLevelRevokedError = 6,
            mbLevelLast = mbLevelInfo
        }
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbConsoleCallback(mbWebView webView, IntPtr param, mbConsoleLevel level, utf8_ message, utf8_ sourceName, uint sourceLine, utf8_ stackTrace);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbOnCallUiThread(mbWebView webView, IntPtr paramOnInThread);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbCallUiThread(mbWebView webView, mbOnCallUiThread func, IntPtr param);

        //mbNet--------------------------------------------------------------------------------------
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        [return:MarshalAs(UnmanagedType.Bool)]
        public delegate bool mbLoadUrlBeginCallback(mbWebView webView, IntPtr param, string url, IntPtr job);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbLoadUrlEndCallback(mbWebView webView, IntPtr param, string url, IntPtr job, IntPtr buf, int len);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbDidCreateScriptContextCallback(mbWebView webView, IntPtr param, mbWebFrameHandle frameId, IntPtr context, int extensionGroup, int worldId);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbWillReleaseScriptContextCallback(mbWebView webView, IntPtr param, mbWebFrameHandle frameId, IntPtr context, int worldId);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool mbNetResponseCallback(mbWebView webView, IntPtr param, string url, IntPtr job);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbOnNetGetFaviconCallback(mbWebView webView, IntPtr param, utf8 url, ref mbMemBuf buf);
        

        public enum MbAsynRequestState {
            kMbAsynRequestStateOk = 0,
            kMbAsynRequestStateFail = 1,
        };
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbCanGoBackForwardCallback(mbWebView webView, IntPtr param, MbAsynRequestState state, bool b);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbGetCookieCallback(mbWebView webView, IntPtr param, MbAsynRequestState state, utf8 cookie);


        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbGetMHTMLCallback(mbWebView webView, IntPtr param, utf8 mhtml);


        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbOnUrlRequestWillRedirectCallback(mbWebView webView, IntPtr param, mbWebUrlRequestPtr oldRequest, mbWebUrlRequestPtr request, mbWebUrlResponsePtr redirectResponse);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbOnUrlRequestDidReceiveResponseCallback(mbWebView webView, IntPtr param, mbWebUrlRequestPtr request, mbWebUrlResponsePtr response);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbOnUrlRequestDidReceiveDataCallback(mbWebView webView, IntPtr param, mbWebUrlRequestPtr request, string data, int dataLength);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbOnUrlRequestDidFailCallback(mbWebView webView, IntPtr param, mbWebUrlRequestPtr request, utf8 error);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbOnUrlRequestDidFinishLoadingCallback(mbWebView webView, IntPtr param, mbWebUrlRequestPtr request, double finishTime);
        [StructLayout(LayoutKind.Sequential)]
        public struct mbUrlRequestCallbacks
        {
            public mbOnUrlRequestWillRedirectCallback willRedirectCallback;
            public mbOnUrlRequestDidReceiveResponseCallback didReceiveResponseCallback;
            public mbOnUrlRequestDidReceiveDataCallback didReceiveDataCallback;
            public mbOnUrlRequestDidFailCallback didFailCallback;
            public mbOnUrlRequestDidFinishLoadingCallback didFinishLoadingCallback;
        }
        //public static _mbUrlRequestCallbacks mbUrlRequestCallbacks;
        //mbwindow-----------------------------------------------------------------------------------
        public enum mbWindowType
        {
            MB_WINDOW_TYPE_POPUP,
            MB_WINDOW_TYPE_TRANSPARENT,
            MB_WINDOW_TYPE_CONTROL
        }
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool mbWindowClosingCallback(mbWebView webWindow, IntPtr param);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbWindowDestroyCallback(mbWebView webWindow, IntPtr param);
        [StructLayout(LayoutKind.Sequential)]
        public struct mbDraggableRegion
        {
            public RECT bounds;
            public bool draggable;
        }
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void mbDraggableRegionsChangedCallback(mbWebView webWindow, IntPtr param, ref mbDraggableRegion rects, int rectCount);

        // 以下是mb的导出函数
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbUninit();
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static mbWebView mbCreateWebView();
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbDestroyWebView(mbWebView webView);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static mbWebView mbCreateWebWindow(mbWindowType type, HWND parent, int x, int y, int width, int height);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbMoveToCenter(mbWebView webWindow);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbSetDebugConfig(mbWebView webView, string debugString, string param);
        // 调用此函数后,网络层收到数据会存储在一buf内,接收数据完成后响应OnLoadUrlEnd事件.#此调用严重影响性能,慎用
        //此函数和mbNetSetData的区别是，mbNetHookRequest会在接受到真正网络数据后再调用回调，并允许回调修改网络数据。
        //而mbNetSetData是在网络数据还没发送的时候修改
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbNetSetData(mbNetJob job, IntPtr buf, int len);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbNetHookRequest(mbNetJob job);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbNetChangeRequestUrl(mbNetJob jobPtr, string url);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static mbWebUrlRequestPtr mbNetCreateWebUrlRequest(utf8 url, utf8 method, utf8 mime);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbNetAddHTTPHeaderFieldToUrlRequest(mbWebUrlRequestPtr request, utf8 name, utf8 value);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static int mbNetStartUrlRequest(mbWebView webView, mbWebUrlRequestPtr request, IntPtr param, ref mbUrlRequestCallbacks callbacks);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static int mbNetGetHttpStatusCode(mbWebUrlResponsePtr response);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static Int64 mbNetGetExpectedContentLength(mbWebUrlResponsePtr response);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static string mbNetGetResponseUrl(mbWebUrlResponsePtr response);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbNetCancelWebUrlRequest(int requestId);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbSetNavigationToNewWindowEnable(mbWebView webView, bool b);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbSetHeadlessEnabled(mbWebView webView, bool b); //可以关闭渲染
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbSetHandle(mbWebView webView, HWND wnd);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbSetHandleOffset(mbWebView webView, int x, int y);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]        
        public extern static HWND mbGetHostHWND(mbWebView webView);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbSetCspCheckEnable(mbWebView webView, bool b);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbSetMemoryCacheEnable(mbWebView webView, bool b);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbSetDragDropEnable(mbWebView webView, bool b);//可关闭拖拽到其他进程
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbSetCookie(mbWebView webView, utf8 url, utf8 cookie);//cookie格式必须是:Set-cookie: PRODUCTINFO=webxpress; domain=.fidelity.com; path=/; secure
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbSetCookieEnabled(mbWebView webView, bool enable);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall,CharSet =CharSet.Unicode)]
        public extern static void mbSetCookieJarPath(mbWebView webView, string path);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbSetUserAgent(mbWebView webView, utf8 userAgent);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbSetZoomFactor(mbWebView webView, float factor);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbSetResourceGc(mbWebView webView, int intervalSec);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]

        public extern static void mbCanGoForward(mbWebView webView, mbCanGoBackForwardCallback callback, IntPtr param);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbCanGoBack(mbWebView webView, mbCanGoBackForwardCallback callback, IntPtr param);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbGetCookie(mbWebView webView, mbGetCookieCallback callback, IntPtr param);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbResize(mbWebView webView, int w, int h);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbOnNavigation(mbWebView webView, mbNavigationCallback callback, IntPtr param);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbOnCreateView(mbWebView webView, mbCreateViewCallback callback, IntPtr param);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbOnDocumentReady(mbWebView webView, mbDocumentReadyCallback callback, IntPtr param);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbOnPaintUpdated(mbWebView webView, mbPaintUpdatedCallback callback, IntPtr callbackParam);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbOnLoadUrlBegin(mbWebView webView, mbLoadUrlBeginCallback callback, IntPtr callbackParam);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbOnLoadUrlEnd(mbWebView webView, mbLoadUrlEndCallback callback, IntPtr callbackParam);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbOnTitleChanged(mbWebView webView, mbTitleChangedCallback callback, IntPtr callbackParam);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbOnURLChanged(mbWebView webView, mbURLChangedCallback callback, IntPtr callbackParam);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbOnLoadingFinish(mbWebView webView, mbLoadingFinishCallback callback, IntPtr param);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbOnDownload(mbWebView webView, mbDownloadCallback callback, IntPtr param);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbGoBack(mbWebView webView);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbGoForward(mbWebView webView);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbStopLoading(mbWebView webView);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbReload(mbWebView webView);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbPerformCookieCommand(mbWebView webView, mbCookieCommand command);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbEditorSelectAll(mbWebView webView);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbEditorCopy(mbWebView webView);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbEditorCut(mbWebView webView);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbEditorPaste(mbWebView webView);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbEditorDelete(mbWebView webView);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static bool mbFireMouseEvent(mbWebView webView, uint message, int x, int y, uint flags);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static bool mbFireContextMenuEvent(mbWebView webView, int x, int y, uint flags);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static bool mbFireMouseWheelEvent(mbWebView webView, int x, int y, int delta, uint flags);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static bool mbFireKeyUpEvent(mbWebView webView, uint virtualKeyCode, uint flags, bool systemKey);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static bool mbFireKeyDownEvent(mbWebView webView, uint virtualKeyCode, uint flags, bool systemKey);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static bool mbFireKeyPressEvent(mbWebView webView, uint charCode, uint flags, bool systemKey);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall,CharSet =CharSet.Unicode)]
        public extern static bool mbFireWindowsMessage(mbWebView webView, HWND hWnd, uint message, uint wParam, uint lParam,out IntPtr result);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbSetFocus(mbWebView webView);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbKillFocus(mbWebView webView);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbShowWindow(mbWebView webWindow, bool show);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbLoadURL(mbWebView webView, utf8 url);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbLoadHtmlWithBaseUrl(mbWebView webView, utf8 html, utf8 baseUrl);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static HDC mbGetLockedViewDC(mbWebView webView);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbUnlockViewDC(mbWebView webView);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbWake(mbWebView webView);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static double mbJsToDouble(mbJsExecState es, mbJsValue v);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static bool mbJsToBoolean(mbJsExecState es, mbJsValue v);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static utf8_ mbJsToString(mbJsExecState es, mbJsValue v);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbOnJsQuery(mbWebView webView, mbJsQueryCallback callback, IntPtr param);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbResponseQuery(mbWebView webView, int64_t queryId, int customMsg, utf8_ response);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbRunJs(mbWebView webView, mbWebFrameHandle frameId, utf8_ script, bool isInClosure, mbRunJsCallback callback, IntPtr param, IntPtr unuse);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static mbJsValue mbRunJsSync(mbWebView webView, mbWebFrameHandle frameId, utf8_ script, bool isInClosure);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static mbWebFrameHandle mbWebFrameGetMainFrame(mbWebView webView);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static bool mbIsMainFrame(mbWebView webView, mbWebFrameHandle frameId);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbUtilSerializeToMHTML(mbWebView webView, mbGetMHTMLCallback calback, IntPtr param);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static string mbUtilCreateRequestCode(string registerInfo);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall,CharSet =CharSet.Unicode)]
        public extern static bool mbUtilIsRegistered(string defaultPath);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbInit(ref mbSettings settings);
        [DllImport(MiniBlinkVDll, CallingConvention = CallingConvention.StdCall)]
        public extern static void mbOnConsole(mbWebView webView, mbConsoleCallback callback, IntPtr param);



        const string MiniBlinkDll = "node.dll";
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate jsValue jsGetPropertyCallback(jsExecState es, jsValue obj, string propertyName);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate bool jsSetPropertyCallback(jsExecState es, jsValue obj, string propertyName, jsValue value);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate jsValue jsCallAsFunctionCallback(jsExecState es, jsValue obj, jsValue[] args, int argCount);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate void jsFinalizeCallback(jsData data);
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public class jsData
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
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsObject(jsExecState es, IntPtr obj);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsExecState wkeGlobalExec(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsGetGlobal(jsExecState es, string prop);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsCall(jsExecState es, jsValue func, jsValue thisObject, jsValue[] args, int argCount);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsGet(jsExecState es, jsValue obj, string prop);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsInt(int n);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsUndefined();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void jsGC();
    }
}
