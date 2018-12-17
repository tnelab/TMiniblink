using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Tnelab.MiniBlinkV.NativeMethods;

namespace Tnelab.HtmlView
{
    public enum MapAction { Create = 1, SetAccess = 2, GetAccess = 3, StaticInvoke = 4, InstanceInvoke = 5 }
    public enum MapDataType { NativeObjectId = 1, Value = 2, FunctionId = 3 };
    public class MapDataInfo
    {
        public MapDataType DataType { get; set; }
        public object Value { get; set; }
        public string NativeTypePath { get; set; }
    }
    class NativeObjectInfo
    {
        public long Id;
        public string Path;
        public string GenericInfo;
        public int GcInfo;
        public object RealObject;
        public jsData JsGC;
    }
    public class MapActionInfo
    {
        public MapAction Action { get; set; }
        public string Path { get; set; }
        public List<MapDataInfo> Args { get; set; } = new List<MapDataInfo>();
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class MapResult
    {
        public bool Status;
        public MapDataInfo Data;
    }
    public class RegisterNativeMapInfo
    {
        public string NativeTypeName { get; set; }
        public string JsTypeName { get; set; }
    }
    public class OnCallJsInfo
    {
        public bool Status { get; set; }
        public Object Data { get; set; }
    }
}
