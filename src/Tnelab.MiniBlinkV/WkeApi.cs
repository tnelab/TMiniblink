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
    using jsValue = Int64;
    using HWND = IntPtr;
    using HDC = IntPtr;
    using WPARAM = UInt32;
    using LPARAM = UInt32;
    using LRESULT = IntPtr;
    using size_t = UInt32;
    using v8ContextPtr = IntPtr;
    using v8Isolate = IntPtr;
    using wkeMediaPlayer = IntPtr;
    using wkeMediaPlayerClient = IntPtr;
    using wkeClientHandlerPtr = IntPtr;
    using wkeMemBufPtr = IntPtr;
    using ItemPtr = IntPtr;
    using wkePostBodyElementPtrPtr = IntPtr;
    using wkeWillSendRequestInfoPtr = IntPtr;
    using wkePostBodyElementsPtr = IntPtr;
    using DWORD = Int32;
    using LPWSTR = String;
    using WORD = Int16;
    using HANDLE = IntPtr;
    using LPBYTE = IntPtr;
    using utf8 = String;
    using wkeWebUrlRequestPtr = IntPtr;
    using wkeWebUrlResponsePtr = IntPtr;
    using wkeNetJob = IntPtr;
    using LONG = Int32;
    using charPtrPtr = IntPtr;
    using wkeTempCallbackInfoPtr = IntPtr;
    using wkePostBodyElementPtr = IntPtr;
    using jsKeysPtr = IntPtr;
    using jsExceptionInfoPtr = IntPtr;
    [SuppressUnmanagedCodeSecurity]
    public static class NativeMethods
    {
        //public const string MiniBlinkDll = "node_v8_5_7.dll";
        public const string MiniBlinkDll = "node.dll";
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public LONG left;
            public LONG top;
            public LONG right;
            public LONG bottom;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public LONG x;
            public LONG y;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct STARTUPINFOW
        {
            public DWORD cb;
            public LPWSTR lpReserved;
            public LPWSTR lpDesktop;
            public LPWSTR lpTitle;
            public DWORD dwX;
            public DWORD dwY;
            public DWORD dwXSize;
            public DWORD dwYSize;
            public DWORD dwXCountChars;
            public DWORD dwYCountChars;
            public DWORD dwFillAttribute;
            public DWORD dwFlags;
            public WORD wShowWindow;
            public WORD cbReserved2;
            public LPBYTE lpReserved2;
            public HANDLE hStdInput;
            public HANDLE hStdOutput;
            public HANDLE hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct wkeRect
        {
            public int x;
            public int y;
            public int w;
            public int h;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct wkePoint
        {
            public int x;
            public int y;
        }
        public enum wkeMouseFlags
        {
            LBUTTON = 0x01,
            RBUTTON = 0x02,
            SHIFT = 0x04,
            CONTROL = 0x08,
            MBUTTON = 0x10,
        }
        public enum wkeKeyFlags
        {
            EXTENDED = 0x0100,
            REPEAT = 0x4000,
        }
        public enum wkeMouseMsg
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
        public enum wkeProxyType
        {
            PROXY_NONE,
            PROXY_HTTP,
            PROXY_SOCKS4,
            PROXY_SOCKS4A,
            PROXY_SOCKS5,
            PROXY_SOCKS5HOSTNAME
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct wkeProxy
        {
            public wkeProxyType type;
            [MarshalAs(UnmanagedType.LPStr, SizeConst = 100)]
            public string hostname;
            public UInt16 port;
            [MarshalAs(UnmanagedType.LPStr, SizeConst = 50)]
            public string username;
            [MarshalAs(UnmanagedType.LPStr, SizeConst = 50)]
            public string password;
        }
        public enum wkeSettingMask
        {
            SETTING_PROXY = 1,
            SETTING_PAINTCALLBACK_IN_OTHER_THREAD = 1 << 2,
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct wkeSettings
        {
            public wkeProxy proxy;
            public uint mask;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct wkeViewSettings
        {
            public int size;
            public uint bgColor;
        }
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr WKE_FILE_OPEN(string path);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void WKE_FILE_CLOSE(IntPtr handle);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate size_t WKE_FILE_SIZE(IntPtr handle);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int WKE_FILE_READ(IntPtr handle, IntPtr buffer, size_t size);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int WKE_FILE_SEEK(IntPtr handle, int offset, int origin);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ON_TITLE_CHANGED(wkeClientHandlerPtr clientHandler, wkeString title);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ON_URL_CHANGED(wkeClientHandler clientHandler, string url);
        [StructLayout(LayoutKind.Sequential)]
        public struct wkeClientHandler
        {
            public ON_TITLE_CHANGED onTitleChanged;
            public ON_URL_CHANGED onURLChanged;
        }
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool wkeCookieVisitor(
            IntPtr pparams,
            string name,
            string value,
            string domain,
            string path, // If |path| is non-empty only URLs at or below the path will get the cookie value.
            int secure, // If |secure| is true the cookie will only be sent for HTTPS requests.
            int httpOnly, // If |httponly| is true the cookie will only be sent for HTTP requests.
            ref int expires // The cookie expiration date is only valid if |has_expires| is true.
        );
        public enum wkeCookieCommand
        {
            CookieCommandClearAllCookies,
            CookieCommandClearSessionCookies,
            CookieCommandFlushCookiesToFile,
            CookieCommandReloadCookiesFromFile,
        }
        public enum wkeNavigationType
        {
            NAVIGATION_TYPE_LINKCLICK,
            NAVIGATION_TYPE_FORMSUBMITTE,
            NAVIGATION_TYPE_BACKFORWARD,
            NAVIGATION_TYPE_RELOAD,
            NAVIGATION_TYPE_FORMRESUBMITT,
            NAVIGATION_TYPE_OTHER
        }
        public enum WkeCursorInfoType
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
        [StructLayout(LayoutKind.Sequential)]
        public struct wkeWindowFeatures
        {
            public int x;
            public int y;
            public int width;
            public int height;
            [MarshalAs(UnmanagedType.Bool)]
            public bool menuBarVisible;
            [MarshalAs(UnmanagedType.Bool)]
            public bool statusBarVisible;
            [MarshalAs(UnmanagedType.Bool)]
            public bool toolBarVisible;
            [MarshalAs(UnmanagedType.Bool)]
            public bool locationBarVisible;
            [MarshalAs(UnmanagedType.Bool)]
            public bool scrollbarsVisible;
            [MarshalAs(UnmanagedType.Bool)]
            public bool resizable;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fullscreen;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct wkeMemBuf
        {
            public int size;
            public IntPtr data;
            public size_t length;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct wkeWebDragData
        {
            [StructLayout(LayoutKind.Sequential)]
            struct Item
            {
                enum wkeStorageType
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
                // Only valid when storageType == StorageTypeString.
                public wkeMemBufPtr stringType;
                public wkeMemBufPtr stringData;

                // Only valid when storageType == StorageTypeFilename.
                public wkeMemBufPtr filenameData;
                public wkeMemBufPtr displayNameData;

                // Only valid when storageType == StorageTypeBinaryData.
                public wkeMemBufPtr binaryData;

                // Title associated with a link when stringType == "text/uri-list".
                // Filename when storageType == StorageTypeBinaryData.
                public wkeMemBufPtr title;

                // Only valid when storageType == StorageTypeFileSystemFile.
                public wkeMemBufPtr fileSystemURL;
                public long fileSystemFileSize;

                // Only valid when stringType == "text/html".
                public wkeMemBufPtr baseURL;
            }

            public ItemPtr m_itemList;
            public int m_itemListLength;

            public int m_modifierKeyState; // State of Shift/Ctrl/Alt/Meta keys.
            public wkeMemBufPtr m_filesystemId;
        }

        public enum wkeWebDragOperation
        {
            wkeWebDragOperationNone = 0,
            wkeWebDragOperationCopy = 1,
            wkeWebDragOperationLink = 2,
            wkeWebDragOperationGeneric = 4,
            wkeWebDragOperationPrivate = 8,
            wkeWebDragOperationMove = 16,
            wkeWebDragOperationDelete = 32,
            wkeWebDragOperationEvery = unchecked((int)0xffffffff)
        }
        public enum wkeResourceType
        {
            WKE_RESOURCE_TYPE_MAIN_FRAME = 0,       // top level page
            WKE_RESOURCE_TYPE_SUB_FRAME = 1,        // frame or iframe
            WKE_RESOURCE_TYPE_STYLESHEET = 2,       // a CSS stylesheet
            WKE_RESOURCE_TYPE_SCRIPT = 3,           // an external script
            WKE_RESOURCE_TYPE_IMAGE = 4,            // an image (jpg/gif/png/etc)
            WKE_RESOURCE_TYPE_FONT_RESOURCE = 5,    // a font
            WKE_RESOURCE_TYPE_SUB_RESOURCE = 6,     // an "other" subresource.
            WKE_RESOURCE_TYPE_OBJECT = 7,           // an object (or embed) tag for a plugin,
                                                    // or a resource that a plugin requested.
            WKE_RESOURCE_TYPE_MEDIA = 8,            // a media resource.
            WKE_RESOURCE_TYPE_WORKER = 9,           // the main resource of a dedicated
                                                    // worker.
            WKE_RESOURCE_TYPE_SHARED_WORKER = 10,   // the main resource of a shared worker.
            WKE_RESOURCE_TYPE_PREFETCH = 11,        // an explicitly requested prefetch
            WKE_RESOURCE_TYPE_FAVICON = 12,         // a favicon
            WKE_RESOURCE_TYPE_XHR = 13,             // a XMLHttpRequest
            WKE_RESOURCE_TYPE_PING = 14,            // a ping request for <a ping>
            WKE_RESOURCE_TYPE_SERVICE_WORKER = 15,  // the main resource of a service worker.
            WKE_RESOURCE_TYPE_LAST_TYPE
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct wkeWillSendRequestInfo
        {
            public wkeString url;
            public wkeString newUrl;
            public wkeResourceType resourceType;
            public int httpResponseCode;
            public wkeString method;
            public wkeString referrer;
            public IntPtr headers;
        }
        public enum wkeHttBodyElementType
        {
            wkeHttBodyElementTypeData,
            wkeHttBodyElementTypeFile,
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct wkePostBodyElement
        {
            public int size;
            public wkeHttBodyElementType type;
            public wkeMemBufPtr data;
            public wkeString filePath;
            public long fileStart;
            public long fileLength; // -1 means to the end of the file.
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct wkePostBodyElements
        {
            public int size;
            public wkePostBodyElementPtrPtr element; //wkePostBodyElement** element;
            public size_t elementSize;
            public bool isDirty;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct wkeTempCallbackInfo
        {
            public int size;
            public wkeWebFrameHandle frame;
            public wkeWillSendRequestInfoPtr willSendRequestInfo;
            public string url;
            public wkePostBodyElementsPtr postBody;
        }
        public enum wkeRequestType
        {
            kWkeRequestTypeInvalidation,
            kWkeRequestTypeGet,
            kWkeRequestTypePost,
            kWkeRequestTypePut,
        }
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeTitleChangedCallback(IntPtr webView, IntPtr param, wkeString title);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeURLChangedCallback(IntPtr webView, IntPtr param, wkeString url);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeURLChangedCallback2(IntPtr webView, IntPtr param, IntPtr frameId, wkeString url);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkePaintUpdatedCallback(IntPtr webView, IntPtr param, IntPtr hdc, int x, int y, int cx, int cy);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkePaintBitUpdatedCallback(wkeWebView webView, IntPtr param, IntPtr buffer, ref wkeRect r, int width, int height);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeAlertBoxCallback(IntPtr webView, IntPtr param, wkeString msg);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool wkeConfirmBoxCallback(IntPtr webView, IntPtr param, wkeString msg);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool wkePromptBoxCallback(IntPtr webView, IntPtr param, wkeString msg, wkeString defaultResult, wkeString result);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool wkeNavigationCallback(IntPtr webView, IntPtr param, wkeNavigationType navigationType, wkeString url);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr wkeCreateViewCallback(IntPtr webView, IntPtr param, wkeNavigationType navigationType, wkeString url, wkeWindowFeatures windowFeatures);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeDocumentReadyCallback(IntPtr webView, IntPtr param);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeDocumentReady2Callback(IntPtr webView, IntPtr param, IntPtr frameId);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeOnShowDevtoolsCallback(wkeWebView webView, IntPtr param);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl,CharSet =CharSet.Unicode)]
        public delegate void wkeNodeOnCreateProcessCallback(wkeWebView webView, IntPtr param, string applicationPath, string arguments, ref STARTUPINFOW startup);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeOnPluginFindCallback(wkeWebView webView, IntPtr param, [MarshalAs(UnmanagedType.CustomMarshaler,MarshalTypeRef =typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]utf8 mime, IntPtr initializeFunc, IntPtr getEntryPointsFunc, IntPtr shutdownFunc);
        [StructLayout(LayoutKind.Sequential)]
        public struct wkeMediaLoadInfo
        {
            public int size;
            public int width;
            public int height;
            public double duration;
        }
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeWillMediaLoadCallback(wkeWebView webView, IntPtr param, string url, ref wkeMediaLoadInfo info);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeStartDraggingCallback(
            wkeWebView webView,
            IntPtr param,
            wkeWebFrameHandle frame,
            ref wkeWebDragData data,
            wkeWebDragOperation mask,
            IntPtr image,
            ref wkePoint dragImageOffset
        );
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeUiThreadRunCallback(HWND hWnd, IntPtr param);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int wkeUiThreadPostTaskCallback(HWND hWnd, wkeUiThreadRunCallback callback, IntPtr param);
        public enum wkeOtherLoadType
        {
            WKE_DID_START_LOADING,
            WKE_DID_STOP_LOADING,
            WKE_DID_NAVIGATE,
            WKE_DID_NAVIGATE_IN_PAGE,
            WKE_DID_GET_RESPONSE_DETAILS,
            WKE_DID_GET_REDIRECT_REQUEST,
            WKE_DID_POST_REQUEST,
        }
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeOnOtherLoadCallback(wkeWebView webView, IntPtr param, wkeOtherLoadType type, ref wkeTempCallbackInfo info);
        public enum wkeLoadingResult
        {
            LOADING_SUCCEEDED,
            LOADING_FAILED,
            LOADING_CANCELED
        }
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeLoadingFinishCallback(IntPtr webView, IntPtr param, IntPtr url, wkeLoadingResult result, IntPtr failedReason);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool wkeDownloadCallback(IntPtr webView, IntPtr param, string url);
        public enum wkeConsoleLevel
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
        public delegate void wkeConsoleCallback(IntPtr webView, IntPtr param, wkeConsoleLevel level, wkeString message, wkeString sourceName, uint sourceLine, wkeString stackTrace);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeOnCallUiThread(IntPtr webView, IntPtr paramOnInThread);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeCallUiThread(IntPtr webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkeOnCallUiThread func, IntPtr param);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate wkeMediaPlayer wkeMediaPlayerFactory(wkeWebView webView, wkeMediaPlayerClient client, IntPtr npBrowserFuncs, IntPtr npPluginFuncs);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool wkeOnIsMediaPlayerSupportsMIMEType([MarshalAs(UnmanagedType.CustomMarshaler,MarshalTypeRef =typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]utf8 mime);
        //wkeNet--------------------------------------------------------------------------------------
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeOnUrlRequestWillRedirectCallback(wkeWebView webView, IntPtr param, wkeWebUrlRequestPtr oldRequest, wkeWebUrlRequestPtr request, wkeWebUrlResponsePtr redirectResponse);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeOnUrlRequestDidReceiveResponseCallback(wkeWebView webView, IntPtr param, wkeWebUrlRequestPtr request, wkeWebUrlResponsePtr response);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeOnUrlRequestDidReceiveDataCallback(wkeWebView webView, IntPtr param, wkeWebUrlRequestPtr request, string data, int dataLength);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeOnUrlRequestDidFailCallback(wkeWebView webView, IntPtr param, wkeWebUrlRequestPtr request, [MarshalAs(UnmanagedType.CustomMarshaler,MarshalTypeRef =typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]utf8 error);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeOnUrlRequestDidFinishLoadingCallback(wkeWebView webView, IntPtr param, wkeWebUrlRequestPtr request, double finishTime);
        public struct wkeUrlRequestCallbacks
        {
            public wkeOnUrlRequestWillRedirectCallback willRedirectCallback;
            public wkeOnUrlRequestDidReceiveResponseCallback didReceiveResponseCallback;
            public wkeOnUrlRequestDidReceiveDataCallback didReceiveDataCallback;
            public wkeOnUrlRequestDidFailCallback didFailCallback;
            public wkeOnUrlRequestDidFinishLoadingCallback didFinishLoadingCallback;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool wkeLoadUrlBeginCallback(IntPtr webView, IntPtr param, string url, IntPtr job);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeLoadUrlEndCallback(IntPtr webView, IntPtr param, string url, IntPtr job, IntPtr buf, int len);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeDidCreateScriptContextCallback(IntPtr webView, IntPtr param, IntPtr frameId, IntPtr context, int extensionGroup, int worldId);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeWillReleaseScriptContextCallback(IntPtr webView, IntPtr param, IntPtr frameId, IntPtr context, int worldId);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool wkeNetResponseCallback(IntPtr webView, IntPtr param, string url, IntPtr job);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeOnNetGetFaviconCallback(wkeWebView webView, IntPtr param, [MarshalAs(UnmanagedType.CustomMarshaler,MarshalTypeRef =typeof(MiniBlinkV.Utf8Marshaler))]utf8 url, wkeMemBufPtr buf);
        //wke window-----------------------------------------------------------------------------------
        public enum wkeWindowType
        {
            WKE_WINDOW_TYPE_POPUP,
            WKE_WINDOW_TYPE_TRANSPARENT,
            WKE_WINDOW_TYPE_CONTROL
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool wkeWindowClosingCallback(wkeWebView webWindow, IntPtr param);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeWindowDestroyCallback(wkeWebView webWindow, IntPtr param);
        [StructLayout(LayoutKind.Sequential)]
        public struct wkeDraggableRegion
        {
            public RECT bounds;
            public bool draggable;
        }
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkeDraggableRegionsChangedCallback(wkeWebView webView, IntPtr param, ref wkeDraggableRegion rects, int rectCount);
        //JavaScript Bind-----------------------------------------------------------------------------------
        [UnmanagedFunctionPointer(CallingConvention.FastCall)]
        public delegate jsValue jsNativeFunction(jsExecState es);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jsValue wkeJsNativeFunction(IntPtr es, IntPtr param);
        public enum jsType
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
        public delegate jsValue jsGetPropertyCallback(IntPtr es, jsValue obj, string propertyName);
        //public delegate object JsGetPropertyCallback(JsExecState es, object obj, string propertyName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = true, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool jsSetPropertyCallback(IntPtr es, jsValue obj, string propertyName, jsValue value);
        //public delegate bool JsSetPropertyCallback(JsExecState es, object obj, string propertyName, object value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = true, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = true)]
        public delegate long jsCallAsFunctionCallback(IntPtr es, jsValue obj, jsValue[] args, int argCount);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = true, CharSet = CharSet.Ansi, SetLastError = true, ThrowOnUnmappableChar = true)]
        public delegate void jsFinalizeCallback(ref jsData data);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct jsData
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
        [StructLayout(LayoutKind.Sequential)]
        public struct jsExceptionInfo
        {
            [MarshalAs(UnmanagedType.CustomMarshaler,MarshalTypeRef =typeof(MiniBlinkV.Utf8Marshaler))]
            public utf8 message; // Returns the exception message.
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MiniBlinkV.Utf8Marshaler))]
            public utf8 sourceLine; // Returns the line of source code that the exception occurred within.
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MiniBlinkV.Utf8Marshaler))]
            public utf8 scriptResourceName; // Returns the resource name for the script from where the function causing the error originates.
            public int lineNumber; // Returns the 1-based number of the line where the error occurred or 0 if the line number is unknown.
            public int startPosition; // Returns the index within the script of the first character where the error occurred.
            public int endPosition; // Returns the index within the script of the last character where the error occurred.
            public int startColumn; // Returns the index within the line of the first character where the error occurred.
            public int endColumn; // Returns the index within the line of the last character where the error occurred.
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MiniBlinkV.Utf8Marshaler))]
            public utf8 callstackString;
        }
        public struct jsKeys
        {
            public uint length;
            public charPtrPtr keys;
        }
        // 以下是wke的导出函数。格式按照【返回类型】【函数名】【参数】来排列
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeInit();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeInitialize();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeInitializeEx(ref wkeSettings settings);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeShutdown();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static UInt16 wkeVersion();
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate string wkeVersionString();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeGC(wkeWebView webView, long delayMs);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetResourceGc(wkeWebView webView, long intervalSec);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetFileSystem(WKE_FILE_OPEN pfnOpen, WKE_FILE_CLOSE pfnClose, WKE_FILE_SIZE pfnSize, WKE_FILE_READ pfnRead, WKE_FILE_SEEK pfnSeek);
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
        public extern static wkeRect wkeGetCaret(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeAwaken(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static float wkeZoomFactor(wkeWebView webView);       

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetClientHandler(wkeWebView webView, wkeClientHandler handler);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static wkeClientHandler wkeGetClientHandler(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.CustomMarshaler,MarshalTypeRef =typeof(MiniBlinkV.Utf8Marshaler))]
        public extern static utf8 wkeToString(wkeString str);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr wkeToStringW(wkeString str);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MiniBlinkV.Utf8Marshaler))]
        public extern static utf8 jsToString(jsExecState es, jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl,CharSet =CharSet.Unicode)]
        public extern static IntPtr jsToStringW(jsExecState es, jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeConfigure(ref wkeSettings settings);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeIsInitialize();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetViewSettings(wkeWebView webView, ref wkeViewSettings settings);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeSetDebugConfig(wkeWebView webView, string debugString, string param);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeFinalize();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeUpdate();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static uint wkeGetVersion();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MiniBlinkV.Utf8Marshaler))]
        public extern static string wkeGetVersionString();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static wkeWebView wkeCreateWebView();
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeDestroyWebView(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetMemoryCacheEnable(wkeWebView webView, bool b);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetMouseEnabled(wkeWebView webView, bool b);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetTouchEnabled(wkeWebView webView, bool b);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetNavigationToNewWindowEnable(wkeWebView webView, bool b);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetCspCheckEnable(wkeWebView webView, bool b);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetNpapiPluginsEnabled(wkeWebView webView, bool b);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetHeadlessEnabled(wkeWebView webView, bool b);//可以关闭渲染
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetDragEnable(wkeWebView webView, bool b);//可关闭拖拽文件加载网页
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetDragDropEnable(wkeWebView webView, bool b);//可关闭拖拽到其他进程
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetLanguage(wkeWebView webView, string language);//可关闭拖拽到其他进程
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeSetViewNetInterface(wkeWebView webView, string netInterface);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeSetProxy(ref wkeProxy proxy);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeSetViewProxy(wkeWebView webView, ref wkeProxy proxy);
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
        public extern static void wkeShowDevtools(wkeWebView webView, string path, wkeOnShowDevtoolsCallback callback, IntPtr param);
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
        public extern static void wkeLoadHTML(wkeWebView webView, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]utf8 html);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeLoadHtmlWithBaseUrl(wkeWebView webView, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]utf8 html, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]utf8 baseUrl);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void wkeLoadHTMLW(wkeWebView webView, string html);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeLoadFile(wkeWebView webView, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]utf8 filename);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void wkeLoadFileW(wkeWebView webView, string filename);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]
        public extern static utf8 wkeGetURL(wkeWebView webView);        
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return:MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]
        public extern static utf8 wkeGetFrameUrl(wkeWebView webView, wkeWebFrameHandle frameId);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool wkeIsLoading(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool wkeIsLoadingSucceeded(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool wkeIsLoadingFailed(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool wkeIsLoadingCompleted(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool wkeIsDocumentReady(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeStopLoading(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeReload(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeGoToOffset(wkeWebView webView, int offset);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeGoToIndex(wkeWebView webView, int index);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static int wkeGetWebviewId(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static bool wkeIsWebviewAlive(int id);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]
        public extern static utf8 wkeGetDocumentCompleteURL(wkeWebView webView, wkeWebFrameHandle frameId, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]utf8 partialURL);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static wkeMemBufPtr wkeCreateMemBuf(wkeWebView webView, IntPtr buf, size_t length);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeFreeMemBuf(wkeMemBufPtr buf);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]
        public extern static utf8 wkeGetTitle(wkeWebView webView);
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
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool wkeCanGoBack(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool wkeGoBack(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
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
        [return:MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]
        public extern static utf8 wkeGetCookie(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeVisitAllCookie(wkeWebView webView, IntPtr pparams, wkeCookieVisitor visitor);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkePerformCookieCommand(wkeWebView webView, wkeCookieCommand command);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetCookieEnabled(wkeWebView webView, [MarshalAs(UnmanagedType.Bool)]bool enable);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeIsCookieEnabled(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void wkeSetCookieJarPath(wkeWebView webView, string path);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void wkeSetCookieJarFullPath(wkeWebView webView, string path);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void wkeSetLocalStorageFullPath(wkeWebView webView, string path);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void wkeAddPluginDirectory(wkeWebView webView, string path);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void wkeSetMediaVolume(wkeWebView webView, float volume);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static float wkeGetMediaVolume(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool wkeFireMouseEvent(wkeWebView webView, uint message, int x, int y, uint flags);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeFireContextMenuEvent(wkeWebView webView, int x, int y, uint flags);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeFireMouseWheelEvent(wkeWebView webView, int x, int y, int delta, uint flags);
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
        public extern static wkeRect wkeGetCaretRect(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static jsValue wkeRunJS(wkeWebView webView, [MarshalAs(UnmanagedType.LPStr)]string script);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static jsValue wkeRunJSW(wkeWebView webView, string script);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsExecState wkeGlobalExec(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsExecState wkeGetGlobalExecByFrame(wkeWebView webView, wkeWebFrameHandle frameId);
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
        [return:MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]
        public extern static utf8 wkeGetString(wkeString str);
        [DllImport(MiniBlinkDll, CharSet = CharSet.Unicode)]
        public extern static string wkeGetStringW(wkeString str);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeSetString(wkeString str, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]string str2, size_t len);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void wkeSetStringW(wkeString str, string str2, size_t len);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static wkeString wkeCreateString([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]utf8 str, size_t len);
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
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeSetDeviceParameter(wkeWebView webView, string device, string paramStr, int paramInt, float paramFloat);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static wkeTempCallbackInfoPtr wkeGetTempCallbackInfo(wkeWebView webView);

        //wke callback-----------------------------------------------------------------------------------
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnMouseOverUrlChanged(wkeWebView webView, wkeTitleChangedCallback callback, IntPtr callbackParam);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnTitleChanged(wkeWebView webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkeTitleChangedCallback callback, IntPtr callbackParam);        
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnURLChanged(wkeWebView webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkeURLChangedCallback callback, IntPtr callbackParam);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnURLChanged2(wkeWebView webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkeURLChangedCallback2 callback, IntPtr callbackParam);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnPaintUpdated(wkeWebView webView, [MarshalAs(UnmanagedType.FunctionPtr)]wkePaintUpdatedCallback callback, IntPtr callbackParam);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnPaintBitUpdated(wkeWebView webView, wkePaintBitUpdatedCallback callback, IntPtr callbackParam);

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

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnWindowClosing(wkeWebView webWindow, wkeWindowClosingCallback callback, IntPtr param);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnWindowDestroy(wkeWebView webWindow, wkeWindowDestroyCallback callback, IntPtr param);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnDraggableRegionsChanged(wkeWebView webView, wkeDraggableRegionsChangedCallback callback, IntPtr param);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnWillMediaLoad(wkeWebView webView, wkeWillMediaLoadCallback callback, IntPtr param);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnStartDragging(wkeWebView webView, wkeStartDraggingCallback callback, IntPtr param);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeOnOtherLoad(wkeWebView webView, wkeOnOtherLoadCallback callback, IntPtr param);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.Bool)]
        public extern static bool wkeIsProcessingUserGesture(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeNetSetMIMEType(IntPtr job, string type);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static string wkeNetGetMIMEType(wkeNetJob jobPtr, wkeString mime);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeNetSetHTTPHeaderField(IntPtr job, string key, string value, [MarshalAs(UnmanagedType.I1)]bool response);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static string wkeNetGetHTTPHeaderField(wkeNetJob jobPtr, string key);

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
        public extern static wkeRequestType wkeNetGetRequestMethod(wkeNetJob jobPtr);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static int wkeNetGetFavicon(wkeWebView webView, wkeOnNetGetFaviconCallback callback, IntPtr param);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeNetContinueJob(wkeNetJob jobPtr);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static string wkeNetGetUrlByJob(wkeNetJob jobPtr);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeNetCancelRequest(wkeNetJob jobPtr);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeNetChangeRequestUrl(wkeNetJob jobPtr, string url);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static bool wkeNetHoldJobToAsynCommit(wkeNetJob jobPtr);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static wkeWebUrlRequestPtr wkeNetCreateWebUrlRequest([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))] utf8 url, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]utf8 method, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]utf8 mime);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeNetAddHTTPHeaderFieldToUrlRequest(wkeWebUrlRequestPtr request, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]utf8 name, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]utf8 value);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static int wkeNetStartUrlRequest(wkeWebView webView, wkeWebUrlRequestPtr request, IntPtr param, ref wkeUrlRequestCallbacks callbacks);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static int wkeNetGetHttpStatusCode(wkeWebUrlResponsePtr response);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static long wkeNetGetExpectedContentLength(wkeWebUrlResponsePtr response);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]
        public extern static utf8 wkeNetGetResponseUrl(wkeWebUrlResponsePtr response);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeNetCancelWebUrlRequest(int requestId);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static wkePostBodyElementsPtr wkeNetGetPostBody(wkeNetJob jobPtr);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static wkePostBodyElementsPtr wkeNetCreatePostBodyElements(wkeWebView webView, size_t length);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeNetFreePostBodyElements(ref wkePostBodyElements elements);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static wkePostBodyElementPtr wkeNetCreatePostBodyElement(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeNetFreePostBodyElement(ref wkePostBodyElement element);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool wkeIsMainFrame(wkeWebView webView, wkeWebFrameHandle frameId);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool wkeIsWebRemoteFrame(wkeWebView webView, wkeWebFrameHandle frameId);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static wkeWebFrameHandle wkeWebFrameGetMainFrame(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static jsValue wkeRunJsByFrame(wkeWebView webView, wkeWebFrameHandle frameId, [MarshalAs(UnmanagedType.LPStr)]string script, [MarshalAs(UnmanagedType.I1)]bool isInClosure);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeInsertCSSByFrame(wkeWebView webView, wkeWebFrameHandle frameId, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))] utf8 cssText);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeWebFrameGetMainWorldScriptContext(wkeWebView webView, wkeWebFrameHandle webFrameId, out v8ContextPtr contextOut);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static v8Isolate wkeGetBlinkMainThreadIsolate();

        //wkewindow-----------------------------------------------------------------------------------

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static wkeWebView wkeCreateWebWindow(wkeWindowType type, HWND parent, int x, int y, int width, int height);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeDestroyWebWindow(wkeWebView webWindow);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static HWND wkeGetWindowHandle(wkeWebView webWindow);
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
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static wkeWebDragOperation wkeDragTargetDragEnter(wkeWebView webView, ref wkeWebDragData webDragData, ref POINT clientPoint, ref POINT screenPoint, wkeWebDragOperation operationsAllowed, int modifiers);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static wkeWebDragOperation wkeDragTargetDragOver(wkeWebView webView, ref POINT clientPoint, ref POINT screenPoint, wkeWebDragOperation operationsAllowed, int modifiers);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeDragTargetDragLeave(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeDragTargetDrop(wkeWebView webView, ref POINT clientPoint, ref POINT screenPoint, int modifiers);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeDragTargetEnd(wkeWebView webView, ref POINT clientPoint, ref POINT screenPoint, wkeWebDragOperation operation);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void wkeUtilSetUiCallback(wkeUiThreadPostTaskCallback callback);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]
        public extern static utf8 wkeUtilSerializeToMHTML(wkeWebView webView);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeSetWindowTitle(wkeWebView webWindow, string title);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void wkeSetWindowTitleW(wkeWebView webWindow, string title);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void wkeNodeOnCreateProcess(wkeWebView webView, wkeNodeOnCreateProcessCallback callback, IntPtr param);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeOnPluginFind(wkeWebView webView, string mime, wkeOnPluginFindCallback callback, IntPtr param);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeAddNpapiPlugin(wkeWebView webView, IntPtr initializeFunc, IntPtr getEntryPointsFunc, IntPtr shutdownFunc);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static wkeWebView wkeGetWebViewByNData(IntPtr ndata);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static bool wkeRegisterEmbedderCustomElement(wkeWebView webView, wkeWebFrameHandle frameId, string name, IntPtr options, IntPtr outResult);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static void wkeSetMediaPlayerFactory(wkeWebView webView, wkeMediaPlayerFactory factory, wkeOnIsMediaPlayerSupportsMIMEType callback);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return:MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]
        public extern static utf8 wkeUtilDecodeURLEscape([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]utf8 url);
        //JavaScript Bind-----------------------------------------------------------------------------------
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
        public extern static jsType jsArgType(jsExecState es, int argIdx);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsArg(jsExecState es, int argIdx);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsType jsTypeOf(jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool jsIsNumber(jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool jsIsString(jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool jsIsBoolean(jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool jsIsObject(jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool jsIsFunction(jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static bool jsIsUndefined(jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool jsIsNull(jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool jsIsArray(jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool jsIsTrue(jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool jsIsFalse(jsValue v);

        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static int jsToInt(jsExecState es, jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static float jsToFloat(jsExecState es, jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static double jsToDouble(jsExecState es, jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool jsToBoolean(jsExecState es, jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static wkeMemBufPtr jsGetArrayBuffer(jsExecState es, jsValue value);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public extern static string jsToTempString(jsExecState es, jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.LPTStr)]
        public extern static string jsToTempStringW(jsExecState es, jsValue v);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void jsToV8Value(jsExecState es, jsValue v);//return v8::Persistent<v8::Value>*

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
        public extern static jsValue jsString(jsExecState es, [MarshalAs(UnmanagedType.CustomMarshaler,MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]string str);
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
        public extern static jsValue jsFunction(jsExecState es, jsData obj);
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
        public extern static jsKeysPtr jsGetKeys(jsExecState es, jsValue obj);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static bool jsIsJsValueValid(jsExecState es, jsValue obj);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static bool jsIsValidExecState(jsExecState es);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static void jsDeleteObjectProp(jsExecState es, jsValue obj, string prop);
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
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static bool jsAddRef(jsExecState es, jsValue val);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static bool jsReleaseRef(jsExecState es, jsValue val);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsExceptionInfoPtr jsGetLastErrorIfException(jsExecState es);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        public extern static jsValue jsThrowException(jsExecState es, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]utf8 exception);
        [DllImport(MiniBlinkDll, CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Tnelab.MiniBlinkV.Utf8Marshaler))]
        public extern static utf8 jsGetCallstack(jsExecState es);

        //public static string WkeGetSource(wkeWebView webView)
        //{
        //    return Marshal.PtrToStringAnsi(webView);
        //}
        //public static string WkeToString(wkeString str)
        //{
        //    return Marshal.PtrToStringAnsi(wkeToString(str));
        //}
        //public static string WkeToStringW(wkeString str)
        //{
        //    return Marshal.PtrToStringUni(wkeToStringW(str));
        //}
        //public static string JsToString(IntPtr es, jsValue v)
        //{
        //    return Marshal.PtrToStringAnsi(jsToString(es, v));
        //}
        //public static string JsToStringW(IntPtr es, jsValue v)
        //{
        //    return Marshal.PtrToStringUni(jsToStringW(es, v));
        //}
        //public static IntPtr WkeCreateStringW(string mime)
        //{
        //    return wkeCreateStringW(mime, (uint)mime.Length);
        //}
    }
    
}
