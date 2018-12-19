using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tnelab.MiniBlink
{
    public class Job
    {
        IntPtr _jobPtr = IntPtr.Zero;
        internal Job(IntPtr job)
        {
            this._jobPtr = job;
        }
        public void NetSetMIMEType(string type)
        {
            NativeMethods.wkeNetSetMIMEType(this._jobPtr, type);
        }

        public void NetSetHTTPHeaderField(string key, string value, bool response)
        {
            NativeMethods.wkeNetSetHTTPHeaderField(this._jobPtr, key, value, response);
        }

        public void NetSetURL(string url)
        {
            NativeMethods.wkeNetSetURL(this._jobPtr, url);
        }
        public void NetSetData(IntPtr buf, int len)
        {
            NativeMethods.wkeNetSetData(this._jobPtr, buf, len);
        }
        // 调用此函数后,网络层收到数据会存储在一buf内,接收数据完成后响应OnLoadUrlEnd事件.#此调用严重影响性能,慎用
        // 此函数和wkeNetSetData的区别是，wkeNetHookRequest会在接受到真正网络数据后再调用回调，并允许回调修改网络数据。
        // 而wkeNetSetData是在网络数据还没发送的时候修改

        public void NetHookRequest()
        {
            NativeMethods.wkeNetHookRequest(this._jobPtr);
        }
        public void NetGetMIMEType(string mime)
        {
            var wkeStr = NativeMethods.WkeCreateStringW(mime);
            NativeMethods.wkeNetGetMIMEType(this._jobPtr, wkeStr);
            NativeMethods.wkeDeleteString(wkeStr);
        }
    }
}
