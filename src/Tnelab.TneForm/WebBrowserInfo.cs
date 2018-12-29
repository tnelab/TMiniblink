using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Tnelab.MiniBlinkV.NativeMethods;

namespace Tnelab.HtmlView
{
    class WebBrowserInfo
    {
        public WebBrowserInfo(IWebBrowser browser, Func<object> getParentControl)
        {
            ParentControlId = this.CreateId();
            this.WebBrowser = browser;
            this.GetParentControl = getParentControl;
        }
        public long ParentControlId { get; private set; }
        public IWebBrowser WebBrowser { get; private set; }
        public Func<object> GetParentControl { get; private set; }
        public long AddNativeObject(object obj,string genericInfo)
        {
            if (obj == null)
                throw new InvalidOperationException("不能把空对象添加到浏览器的本机对象列表中");
            if (NativeObjectInfoDic.Values.SingleOrDefault(item => item.RealObject == obj) != null)
                throw new InvalidOperationException("不能重复添加对象到相同浏览器的本机对象列表中");
            var id = CreateId();
            var info = new NativeObjectInfo();
            info.Id = id;
            info.RealObject = obj;
            info.GenericInfo = genericInfo;
            info.GcInfo = 1;
            var jsGc= new Tnelab.MiniBlink.NativeMethods.jsData();
            jsGc.typeName = "NativeObjectGC";
            jsGc.propertyGet = (es, jobj, name) =>
            {
                return Tnelab.MiniBlink.NativeMethods.jsUndefined();
            };
            jsGc.propertySet = (es, jobj, name, jv) =>
            {
                return true;
            };
            jsGc.callAsFunction = (es,jobj,args,argsCount) =>
            {
                return Tnelab.MiniBlink.NativeMethods.jsUndefined();
            };
            var gcPtr = Marshal.AllocHGlobal(Marshal.SizeOf<MiniBlink.NativeMethods.jsData>());
            Marshal.StructureToPtr(jsGc, gcPtr, true);

            jsGc.finalize = (ref Tnelab.MiniBlink.NativeMethods.jsData data) =>
            {
                this.DestroyNativeObject(id,true);
                Marshal.FreeHGlobal(gcPtr);
            };
            var tes = MiniBlink.NativeMethods.wkeGlobalExec(this.WebBrowser.WebView);
            var gcValue = MiniBlink.NativeMethods.jsObject(tes, gcPtr);
            var idValue = MiniBlink.NativeMethods.jsInt((int)id);
            var tnelabValue = MiniBlink.NativeMethods.jsGetGlobal(tes, "Tnelab");
            var onSetGCValue = MiniBlink.NativeMethods.jsGet(tes, tnelabValue, "OnSetGC");
            MiniBlink.NativeMethods.jsCall(tes, onSetGCValue, tnelabValue, new long[] { idValue, gcValue }, 2);
            info.JsGC = jsGc;
            NativeObjectInfoDic.Add(id, info);
            return id;
        }
        public object GetNativeObject(long id, bool addGC)
        {
            if (this.ParentControlId == id)
            {

                return this.GetParentControl();
            }

            if (NativeObjectInfoDic.ContainsKey(id))
            {
                if (addGC)
                    NativeObjectInfoDic[id].GcInfo++;
                return NativeObjectInfoDic[id].RealObject;
            }
            return null;
        }
        public long GetNativeObjectId(object obj)
        {
            if(this.GetParentControl()==obj)
            {
                return this.ParentControlId;
            }
            var iobj = NativeObjectInfoDic.SingleOrDefault(it => it.Value.RealObject == obj);
            if (iobj.Value != null)
                return iobj.Key;
            return -1;
        }
        public void DestroyNativeObject(long id,bool isGC)
        {
            if (NativeObjectInfoDic.ContainsKey(id))
            {
                var obj = NativeObjectInfoDic[id];
                if (isGC) {
                    obj.GcInfo--;                       
                }
                else
                {
                    obj.GcInfo = 0;
                }
                if (obj.GcInfo == 0)
                {
                    this.NativeObjectInfoDic.Remove(id);
                    if (obj.RealObject is IDisposable)
                    {
                        var disposableObj = obj.RealObject as IDisposable;
                        disposableObj.Dispose();
                    }
                }
            }
        }
        static long IdSeed = 0;
        long CreateId()
        {
            IdSeed += 1;
            return IdSeed;
        }
        Dictionary<long, NativeObjectInfo> NativeObjectInfoDic { get; } = new Dictionary<long, NativeObjectInfo>();
        void ClearNativeObject()
        {
            var keys = NativeObjectInfoDic.Keys.ToList();
            foreach (var id in keys)
            {
                this.DestroyNativeObject(id,false);
            }
        }
        ~WebBrowserInfo()
        {
            ClearNativeObject();
        }
    }
}
