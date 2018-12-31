using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tnelab.HtmlView
{
    interface IWebBrowser
    {
        IntPtr WebView { get; }
        event EventHandler<string> TitleChanged;
        event EventHandler<JsQueryEventArgs> JsQuery;
        string Url { get; set; }
        (int result,bool isHandle) ProcessWindowMessage(IntPtr hwnd, uint msg, uint wParam, uint lParam);
        void ResponseJsQuery(IntPtr webView, Int64 queryId, int customMsg, string response);
        Task<(IntPtr view,IntPtr es,string value)> RunJs(string script,Action<IntPtr,IntPtr ,long> hook=null);
        void UIInvoke(Action action);
    }
}
