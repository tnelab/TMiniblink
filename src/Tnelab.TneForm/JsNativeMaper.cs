using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Tnelab.MiniBlinkV.NativeMethods;
using System.Runtime.InteropServices;
namespace Tnelab.HtmlView
{
    enum TneQueryId { NativeMap = 1, RegisterNativeMap = 2, DeleteNativeObject = 3 ,GetThisFormHashCode=4}
    interface IJsNativeMaper
    {
        long AddBrowser(IWebBrowser browser, Func<object> GetParentControl);
        void RemoveBrowser(IWebBrowser browser);
    }
    sealed class JsNativeMaper: IJsNativeMaper
    {
        public static JsNativeMaper This { get; } = new JsNativeMaper();
        static readonly Dictionary<string, string> NativeTypeDic = new Dictionary<string, string>();
        static void OnJavaScriptNativeMap(IWebBrowser browser,JsQueryEventArgs args)
        {
            MapResult result = null;
            var info = This.GetBrowserInfo(browser);
            try
            {
                var json = args.Request;
                var mapInfo = JsonConvert.DeserializeObject<MapActionInfo>(json);
                var context = new JsNativeInvokeContext(info, mapInfo, (typeName) => NativeTypeDic.ContainsKey(typeName) ? NativeTypeDic[typeName] : null);
                result=JsNativeInvokeHandleFactory.This.CreateHandle(context).Handle();
                if (result == null)
                {
                    browser.ResponseJsQuery(args.WebView, args.QueryId, args.CustomMsg, "OK");
                    return;
                }
                var resultJson = JsonConvert.SerializeObject(result);
                browser.ResponseJsQuery(args.WebView, args.QueryId, args.CustomMsg, resultJson);
            }
            catch (Exception ex)
            {
                if (result != null && result.Data.DataType == MapDataType.NativeObjectId)
                {
                    info.DestroyNativeObject(((NativeObjectInfo)result.Data.Value).Id,false);
                }
                result = new MapResult { Status = false, Data = new MapDataInfo { DataType = MapDataType.Value, Value = ex.Message } };
                var resultJson = JsonConvert.SerializeObject(result).Replace("\\r","").Replace("\\n","");
                browser.ResponseJsQuery(args.WebView, args.QueryId, args.CustomMsg, resultJson);
            }
        }
        void OnRegisterNativeMap(IWebBrowser browser,JsQueryEventArgs args)
        {
            var regInfo = JsonConvert.DeserializeObject<RegisterNativeMapInfo>(args.Request);
            var nativeTypeName = regInfo.NativeTypeName;
            var b = nativeTypeName.IndexOf("<");
            var isGeneric =b !=-1;
            if (isGeneric)
            {
                var tmps = nativeTypeName.Split(',');
                nativeTypeName=$"{nativeTypeName.Substring(0,b)}`{tmps.Length}";
            }
            var jsTypeName = regInfo.JsTypeName;
            if (!NativeTypeDic.ContainsKey(nativeTypeName))
            {
                NativeTypeDic.Add(nativeTypeName, jsTypeName);
            }
            var info = GetBrowserInfo(browser);
            browser.ResponseJsQuery(args.WebView, args.QueryId, args.CustomMsg, "OK");
        }
        static void OnDeleteNativeObject(IWebBrowser browser, JsQueryEventArgs args)
        {
            var info = This.GetBrowserInfo(browser);
            var hashCode = long.Parse(args.Request);
            info.DestroyNativeObject(hashCode,false);
            browser.ResponseJsQuery(args.WebView, args.QueryId, args.CustomMsg, "OK");
        }
        //JsNativeMaper() { }
        public long AddBrowser(IWebBrowser browser, Func<object> getParentControl)
        {
            if (browser == null)
                throw new ArgumentException("浏览器对象不能为空", "browser");
            browser.JsQuery += (webBrowser, args) =>
            {
                switch ((TneQueryId)args.CustomMsg)
                {
                    case TneQueryId.RegisterNativeMap:
                        OnRegisterNativeMap(webBrowser as IWebBrowser, args);
                        break;
                    case TneQueryId.NativeMap:
                        OnJavaScriptNativeMap(webBrowser as IWebBrowser, args);
                        break;
                    case TneQueryId.DeleteNativeObject:
                        OnDeleteNativeObject(webBrowser as IWebBrowser, args);
                        break;
                    case TneQueryId.GetThisFormHashCode:
                        {
                            var wb = webBrowser as IWebBrowser;
                            var wbinfo = GetBrowserInfo(wb);
                            wb.ResponseJsQuery(args.WebView, args.QueryId, args.CustomMsg, wbinfo.ParentControlId.ToString());
                        }
                        break;
                    default:
                        throw new Exception("未知JS消息");
                }
            };
            var info = new WebBrowserInfo(browser, getParentControl);            
            WebBrowserInfoDic.Add(browser, info);            
            return info.ParentControlId;
        }
        public void RemoveBrowser(IWebBrowser browser)
        {
            if (WebBrowserInfoDic.ContainsKey(browser))
            {
                //var info = WebBrowserInfoDic[browser];
                WebBrowserInfoDic.Remove(browser);
            }
        }
        readonly Dictionary<IWebBrowser,WebBrowserInfo> WebBrowserInfoDic = new Dictionary<IWebBrowser, WebBrowserInfo>();
        WebBrowserInfo GetBrowserInfo(IWebBrowser browser)
        {
            return WebBrowserInfoDic.Values.SingleOrDefault(item => item.WebBrowser == browser);
        }
    }
}
