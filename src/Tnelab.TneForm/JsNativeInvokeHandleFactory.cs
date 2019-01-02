using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Tnelab.MiniBlinkV.NativeMethods;

namespace Tnelab.HtmlView
{
    interface IJsNavateInvokeHandle
    {
        MapResult Handle();
    }
    class JsNativeInvokeHandleFactory
    {        
        static JsNativeInvokeHandleFactory()
        {
            JsNativeInvokeHandleFactory.This.RegisterHandleCreater(MapAction.Create, context => new CreateInvokeHandle(context));
            JsNativeInvokeHandleFactory.This.RegisterHandleCreater(MapAction.InstanceInvoke, context => new InstanceInvokeHandle(context));
            JsNativeInvokeHandleFactory.This.RegisterHandleCreater(MapAction.StaticInvoke, context => new StaticInvokeHandle(context));
            JsNativeInvokeHandleFactory.This.RegisterHandleCreater(MapAction.SetAccess, context => new SetAccessHandle(context));
            JsNativeInvokeHandleFactory.This.RegisterHandleCreater(MapAction.GetAccess, context => new GetAccessHandle(context));
        }
        public static JsNativeInvokeHandleFactory This { get; } = new JsNativeInvokeHandleFactory();
        public IJsNavateInvokeHandle CreateHandle(JsNativeInvokeContext context)
        {
            if (HandleCreaterMap.ContainsKey(context.MapInfo.Action))
            {
                return HandleCreaterMap[context.MapInfo.Action](context);
            }
            throw new InvalidOperationException($"处理器{context.MapInfo.Action}无法创建，该处理器构建器未在工厂注册");
        }
        public void RegisterHandleCreater(MapAction action,Func<JsNativeInvokeContext, IJsNavateInvokeHandle> creater)
        {
            if (HandleCreaterMap.ContainsKey(action))
            {
                throw new InvalidOperationException($"处理器{action}构建器无法重复注册，工厂已经存在该处理器构建器注册");
            }
            HandleCreaterMap.Add(action, creater);

        }
        private readonly Dictionary<MapAction, Func<JsNativeInvokeContext, IJsNavateInvokeHandle>> HandleCreaterMap = new Dictionary<MapAction, Func<JsNativeInvokeContext, IJsNavateInvokeHandle>>();
        private JsNativeInvokeHandleFactory() {
        }
    }
    class JsNativeInvokeContext
    {
        public WebBrowserInfo WebBrowserInfo { get; private set; }
        public MapActionInfo MapInfo { get; private set; }
        public Func<string,string> GetJsTypeName { get; private set; }
        public JsNativeInvokeContext(WebBrowserInfo info,MapActionInfo mapInfo,Func<string,string> getJsTypeName)
        {
            this.WebBrowserInfo = info;
            this.MapInfo = mapInfo;
            this.GetJsTypeName = getJsTypeName;
        }
        public MethodInfo GetMethod(string name,Type type,Type[] argTypes,Type[] genericTypes)
        {
            if (genericTypes == null || genericTypes.Length == 0)
            {
                return type.GetMethod(name,argTypes);
            }
            var methods = type.GetMethods().Where(it => it.Name == name && it.IsGenericMethod);
            foreach(var method in methods)
            {
                var genericArgs = method.GetGenericArguments();
                if (genericArgs.Length != genericArgs.Length)
                    continue;
                var realMethod = method.MakeGenericMethod(genericTypes);
                var parameters = realMethod.GetParameters();
                if (parameters.Length != argTypes.Length)
                    continue;
                bool isOK = true;
                for(var i=0;i<parameters.Length;i++)
                {
                    if (parameters[i].ParameterType.FullName != argTypes[i].FullName)
                    {
                        isOK = false;
                        break;
                    }
                }
                if (!isOK)
                    continue;
                return realMethod;
            }
            return null;
        }
        public List<string> GetTypeNames(string genericInfo)
        {
            var typeNams = new List<string>();
            var lc = 0;
            var b = 0;
            for (var i = 0; i < genericInfo.Length; i++)
            {
                if (genericInfo[i] == '<')
                {
                    lc++;
                }
                else if (genericInfo[i] == '>')
                {
                    lc--;
                }
                else if (genericInfo[i] == ',')
                {
                    if (lc == 0)
                    {
                        typeNams.Add(genericInfo.Substring(b, i - b));
                        b = i + 1;
                    }
                }
            }
            if (b < genericInfo.Length)
            {
                typeNams.Add(genericInfo.Substring(b, genericInfo.Length - b));
            }
            return typeNams;
        }
        public Type GetTypeByString(string typeName)
        {
            Type result = null;
            var genericFlag = typeName.IndexOf('<');
            if (genericFlag == -1)
            {
                switch (typeName.ToLower())
                {
                    case "bool":
                        return Type.GetType("System.Boolean");
                    case "byte":
                        return Type.GetType("System.Byte");
                    case "sbyte":
                        return Type.GetType("System.SByte");
                    case "char":
                        return Type.GetType("System.Char");
                    case "decimal":
                        return Type.GetType("System.Decimal");
                    case "double":
                        return Type.GetType("System.Double");
                    case "float":
                        return Type.GetType("System.Single");
                    case "int":
                        return Type.GetType("System.Int32");
                    case "uint":
                        return Type.GetType("System.UInt32");
                    case "long":
                        return Type.GetType("System.Int64");
                    case "ulong":
                        return Type.GetType("System.UInt64");
                    case "object":
                        return Type.GetType("System.Object");
                    case "short":
                        return Type.GetType("System.Int16");
                    case "ushort":
                        return Type.GetType("System.UInt16");
                    case "string":
                        return Type.GetType("System.String");
                    default:
                        result = Type.GetType(typeName);
                        break;
                }
                if (result != null)
                    return result;
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assemblie in assemblies)
                {
                    result = assemblie.GetType(typeName);
                    if (result != null)
                    {
                        return result;
                    }
                }
                if (result == null)
                    throw new Exception($"找不到本机类型{typeName}");
            }
            //处理泛型            
            var genericInfo = typeName.Substring(genericFlag + 1, typeName.Length - genericFlag - 2);
            var genericArgsTypeNams = GetTypeNames(genericInfo);
            var genericTypeName = $"{typeName.Substring(0, genericFlag)}`{genericArgsTypeNams.Count}";
            result = GetTypeByString(genericTypeName);
            Type[] typeArgs = new Type[genericArgsTypeNams.Count];
            for (var i = 0; i < typeArgs.Length; i++)
            {
                typeArgs[i] = GetTypeByString(genericArgsTypeNams[i]);
            }
            result = result.MakeGenericType(typeArgs);
            return result;
        }
        public string GetGenericInfo(Type type)
        {
            if (!type.IsGenericType)
            {
                return "";
            }
            string GetFriendName(Type t)
            {
                if (!t.IsGenericType)
                {
                    return $"{t.Namespace}.{t.Name}";
                }
                var gtypeNameList = new List<string>();
                foreach (var gtype in t.GenericTypeArguments)
                {
                    gtypeNameList.Add(GetFriendName(gtype));
                }
                var b = t.Name.IndexOf("`");
                var name = t.Name.Substring(0, b);
                var result = $"{t.Namespace}.{name}<{string.Join(",", gtypeNameList.ToArray())}>";
                return result;
            }
            var friendName = GetFriendName(type);
            var sb = friendName.IndexOf("<");
            var tmps = GetTypeNames(friendName.Substring(sb + 1, friendName.Length - sb - 2));
            return String.Join("|", tmps);
        }
        public string NativeToJsValue(object nVal)
        {
            string getJvByTypeName(string tName,object val)
            {
                string gv = null;
                switch (tName)
                {
                    case "System.Char":
                    case "System.String":
                        gv = $"\"{val.ToString()}\"";
                        break;
                    case "System.Int16":
                    case "System.UInt16":
                    case "System.Int32":
                    case "System.UInt32":
                    case "System.Int64":
                    case "System.UInt64":
                    case "System.Double":
                    case "System.Decimal":
                    case "System.Single":
                    case "System.Boolean":
                    case "System.Byte":
                    case "System.SByte":
                        gv = val.ToString();
                        break;
                }
                return gv;
            }
            string getJvByTuple(long id, string type_name,string genericInfo)
            {
                var jsTypeName = GetJsTypeName(type_name);
                var b = jsTypeName.IndexOf("<");
                if(b!=-1)
                {
                    jsTypeName = jsTypeName.Substring(0, b);
                }
                var strBuilder = new StringBuilder();
                strBuilder.Append("return (function(){")
                    .Append("let no=new Tnelab.JsNativeMap.NativeObjectInfo();")
                    .Append($"no.Id = {id};")
                    .Append($"no.GenericInfo = \"{genericInfo}\";")
                    .Append($"return new {jsTypeName}(no);")
                    .Append("})()");
                return strBuilder.ToString();
            }
            string getJvByMaped(string type_name,object val,string genericInfo)
            {
                var id = WebBrowserInfo.AddNativeObject(val,genericInfo);
                var jsTypeName = GetJsTypeName(type_name);
                var b = jsTypeName.IndexOf("<");
                if (b != -1)
                {
                    jsTypeName = jsTypeName.Substring(0, b);
                }
                var strBuilder = new StringBuilder();
                strBuilder.Append("return (function(){")
                    .Append("let no=new Tnelab.JsNativeMap.NativeObjectInfo();")
                    .Append($"no.Id = {id};")
                    .Append($"no.GenericInfo = \"{genericInfo}\";")
                    .Append($"return new {jsTypeName}(no);")
                    .Append("})()");
                return strBuilder.ToString();
            }
            string getJv(string type_name, object val,string genericInfo)
            {
                string gv;
                gv = getJvByTypeName(type_name, val);
                if (gv != null)
                    return gv;
                var tVal = val as Tuple<long, object>;//是否一个已存在的js同步对象
                if (val == null)
                {
                    gv = "undefined";
                }
                else if (tVal != null)//是否一个已存在的js同步对象
                {
                    gv = getJvByTuple(tVal.Item1, type_name, genericInfo);
                }
                else if (GetJsTypeName(type_name) != null)//一个已经映射的js类型,但是未存在的js对象
                {
                    gv = getJvByMaped(type_name,val, genericInfo);
                }
                else//其他，直接序列
                {
                    var json = JsonConvert.SerializeObject(val);
                    gv = $"JSON.parse({json})";
                }
                return gv;
            }

            var browser = this.WebBrowserInfo.WebBrowser;
            var type = nVal.GetType();
            var typeName = $"{type.Namespace}.{type.Name}";
            if (type.IsGenericType)
            {
                typeName = $"{typeName}`{type.GenericTypeArguments.Length}";
            }
            var rv = getJv(typeName, nVal,GetGenericInfo(type));
            return rv;
        }
        public (Type,object) JsValueInfoToNative(MapDataInfo dataInfo)
        {
            //处理范型Action和func
            //(Type, bool) IsAction(string nativeTypePath)
            //{
            //    Type r_nativeType = GetTypeByString(nativeTypePath);
            //    bool lisAction = false;
            //    if (nativeTypePath.IndexOf("System.Action<") >= 0)
            //    {
            //        lisAction = true;
            //        //isFunc = false;
            //    }
            //    else if (nativeTypePath.IndexOf("Func<") >= 0)
            //    {
            //        lisAction = false;
            //        //isFunc = true;
            //    }
            //    return (lnativeType, lisAction);
            //}
            //(Type, bool) NativeTypePathNotNull()
            //{
            //    Type lnativeType = null;
            //    bool lisAction = false;
            //    var ntPath = dataInfo.NativeTypePath;
            //    var sIdx = ntPath.IndexOf("<");
            //    if (sIdx > 0)
            //    {
            //        var (nt, ia) = IsFx(ntPath, sIdx);
            //        lnativeType = nt;
            //        lisAction = ia;
            //    }
            //    else
            //    {
            //        lnativeType = Type.GetType(dataInfo.NativeTypePath);
            //        lisAction = lnativeType == typeof(Action);
            //    }
            //    return (lnativeType, lisAction);
            //}
            object IsFunctionId(Type theNativeType, bool theIsAction, IWebBrowser theBrowser,string script)
            {
                var paramTypes = theNativeType.GetGenericArguments();
                List<ParameterExpression> paramExps = new List<ParameterExpression>();
                List<Expression> vParamExps = new List<Expression>() {
                    Expression.Convert(Expression.Constant(theIsAction), typeof(object)),
                    Expression.Convert(Expression.Constant(theBrowser), typeof(object)),
                    Expression.Convert(Expression.Constant(script), typeof(object))
                };

                var len = paramTypes.Length;
                if (!theIsAction)
                {
                    len -= 1;
                }
                for (var i = 0; i < len; i++)
                {
                    var pt = paramTypes[i];
                    var pEx = Expression.Parameter(pt);
                    paramExps.Add(pEx);
                    vParamExps.Add(Expression.Convert(pEx, typeof(object)));
                }
                var arrayExp = Expression.NewArrayInit(typeof(object), vParamExps);
                LambdaExpression lambdaExpr;
                if (theIsAction)
                {
                    lambdaExpr = Expression.Lambda(Expression.Call(Expression.Constant(this), typeof(JsNativeInvokeContext).GetMethod("ActionCallJs", BindingFlags.NonPublic | BindingFlags.Instance), arrayExp), paramExps);
                }
                else
                {
                    lambdaExpr = Expression.Lambda(Expression.Convert(Expression.Call(Expression.Constant(this), typeof(JsNativeInvokeContext).GetMethod("FuncCallJs", BindingFlags.NonPublic | BindingFlags.Instance), arrayExp), paramTypes[paramTypes.Length - 1]), paramExps);
                }
                return lambdaExpr.Compile();
            }
            var browser =WebBrowserInfo.WebBrowser;
            Type nativeType = null;
            object obj = null;
            //bool isFunc = false;
            //if (dataInfo.NativeTypePath != null)
            //{
            //    var (nt, ia) = NativeTypePathNotNull();
            //    nativeType = nt;
            //    isAction = ia;
            //}
            if (dataInfo.DataType == MapDataType.NativeObjectId)
            {
                dynamic dobj = dataInfo.Value;
                long hcode = dobj.Id;
                var nobj = WebBrowserInfo.GetNativeObject(hcode,false);
                if (nobj == null)
                    throw new Exception($"本机对象不存在{hcode}");
                nativeType = nobj.GetType();
                obj = new Tuple<long, object>(hcode, nobj);
            }
            else if (dataInfo.DataType == MapDataType.FunctionId)
            {
                nativeType = GetTypeByString(dataInfo.NativeTypePath);
                bool isAction = nativeType.FullName.StartsWith("System.Action");
                obj = IsFunctionId(nativeType, isAction, browser,dataInfo.Value.ToString());
            }
            else
            {
                nativeType = GetTypeByString(dataInfo.NativeTypePath);
                obj = dataInfo.Value;
                if (nativeType != null)
                {
                    if (typeof(Enum).IsAssignableFrom(nativeType))
                    {
                        obj = Enum.Parse(nativeType, dataInfo.Value.ToString());
                    }
                    else
                    {
                        obj = Convert.ChangeType(dataInfo.Value, nativeType);
                    }
                }
            }
            if (nativeType == null)
            {
                nativeType = obj.GetType();
            }
            return (nativeType, obj);
        }
        object FuncCallJs(params object[] objs)
        {
            object toNativeObject(object obj)
            {
                if (obj == null)
                    return obj;
                var nobj = obj as NativeObjectInfo;
                if (nobj == null)
                    return obj;
                var r= WebBrowserInfo.GetNativeObject(nobj.Id,false);
                if(r==null)
                    throw new Exception($"本机对象不存在{nobj.Id}");
                return r;
            }
            var isAction = (bool)objs[0];
            var browser = objs[1] as IWebBrowser;
            var script = objs[2].ToString();
            var args = new string[objs.Length - 3];
            for (var i = 0; i < args.Length; i++)
            {
                args[i] = NativeToJsValue(objs[i + 3]);
            }
            var toRun = $"return Tnelab.OnCallJs(({script})({string.Join(",", args)}))";
            var result = browser.RunJs(toRun);
            var robj = JsonConvert.DeserializeObject<OnCallJsInfo>(result);
            if (!robj.Status)
                throw new Exception(robj.Data.ToString());
            return toNativeObject(robj.Data);
        }
        void ActionCallJs(params object[] objs)
        {
            var isAction = (bool)objs[0];
            var browser = objs[1] as IWebBrowser;
            var script = objs[2].ToString();
            var args = new string[objs.Length - 3];
            for (var i = 0; i < args.Length; i++)
            {
                args[i] = NativeToJsValue(objs[i + 3]);
            }
            var toRun = $"return Tnelab.OnCallJs(({script})({string.Join(",", args)}))";
            var result = browser.RunJs(toRun);
            var robj = JsonConvert.DeserializeObject<OnCallJsInfo>(result);
            if (!robj.Status)
                throw new Exception(robj.Data.ToString());
        }
    }
    class CreateInvokeHandle:IJsNavateInvokeHandle
    {
        public CreateInvokeHandle(JsNativeInvokeContext context)
        {
            this._context = context;
        }
        public MapResult Handle()
        {
            var mapInfo = this._context.MapInfo;
            var browser = this._context.WebBrowserInfo.WebBrowser;
            MapResult result = null;
            //Type t = Type.GetType(mapInfo.Path);
            Type t = this._context.GetTypeByString(mapInfo.Path);
            object[] args = new object[mapInfo.Args.Count];
            for (int i = 0; i < args.Length; i++)
            {
                var (type, value) = this._context.JsValueInfoToNative(mapInfo.Args[i]);
                if (value is Tuple<long, object>)
                {
                    var (nid, obj) = value as Tuple<long, object>;
                    args[i] = obj;
                }
                else
                {
                    args[i] = value;
                }
            }

            var o = Activator.CreateInstance(t, args);
            var genericInfo = this._context.GetGenericInfo(t);
            var id = this._context.WebBrowserInfo.AddNativeObject(o, genericInfo);
            result = new MapResult { Status = true, Data = new MapDataInfo { DataType = MapDataType.NativeObjectId, Value = new { Id = id, GenericInfo= genericInfo } } };
            return result;
        }
        JsNativeInvokeContext _context;
    }
    abstract class ObjectInvokeHandle : IJsNavateInvokeHandle
    {
        protected JsNativeInvokeContext Context { get; private set; }
        protected ObjectInvokeHandle(JsNativeInvokeContext context)
        {
            this.Context = context;
        }
        protected abstract object InvokeMethod(Type[] types, object[] args);
        public MapResult Handle()
        {
            var mapInfo = this.Context.MapInfo;
            var browser = this.Context.WebBrowserInfo.WebBrowser;
            MapResult result = null;
            object[] args = new object[mapInfo.Args.Count];
            Type[] types = new Type[mapInfo.Args.Count];
            for (int i = 0; i < mapInfo.Args.Count; i++)
            {
                var (type, value) = this.Context.JsValueInfoToNative(mapInfo.Args[i]);
                if (value is Tuple<long, object>)
                {
                    var (id, obj) = value as Tuple<long, object>;
                    args[i] = obj;
                }
                else
                {
                    args[i] = value;
                }
                types[i] = type;
            }
            object r = InvokeMethod(types, args);            
            if (r == null)
            {
                result = new MapResult { Status = true, Data = new MapDataInfo { DataType = MapDataType.Value, Value = null } };
            }
            else
            {
                if (r.GetType().IsValueType || this.Context.GetJsTypeName(r.GetType().FullName)==null)
                {
                    result = new MapResult { Status = true, Data = new MapDataInfo { DataType = MapDataType.Value, Value = r } };
                }
                else
                {
                    var id=this.Context.WebBrowserInfo.GetNativeObjectId(r);
                    var genericInfo = "";
                    if (id == -1)
                    {
                        genericInfo = this.Context.GetGenericInfo(r.GetType());
                        id = this.Context.WebBrowserInfo.AddNativeObject(r,genericInfo);
                    }
                    result = new MapResult { Status = true, Data = new MapDataInfo { DataType = MapDataType.NativeObjectId, Value = new NativeObjectInfo { Id = id, GenericInfo = genericInfo } } };
                }
            }
            return result;
        }
    }
    class InstanceInvokeHandle : ObjectInvokeHandle
    {
        public InstanceInvokeHandle(JsNativeInvokeContext context) : base(context) { }
        protected override object InvokeMethod(Type[] types,object[] args)
        {
            var mapInfo = this.Context.MapInfo;
            var obj = this.Context.WebBrowserInfo.GetNativeObject(mapInfo.Id,false);
            if (obj == null)
                throw new Exception($"本机对象{mapInfo.Id}不存在");
            var o = obj;
            var name = mapInfo.Name;
            var isGeneric = mapInfo.Name.IndexOf("<")!=-1;
            Type[] genericTypes = null;
            if (isGeneric)
            {
                var b = mapInfo.Name.IndexOf("<");
                var genericInfo = mapInfo.Name.Substring(b+1,mapInfo.Name.Length-b-2);
                var tmps = this.Context.GetTypeNames(genericInfo);
                
                genericTypes = new Type[tmps.Count];
                for(var i=0;i<genericTypes.Length;i++)
                {
                    genericTypes[i] = this.Context.GetTypeByString(tmps[i]);
                }
                name = mapInfo.Name.Substring(0,b);
            }
            var method = this.Context.GetMethod(name, o.GetType(), types, genericTypes);
            if (method == null)
            {
                throw new Exception($"未知映射{JsonConvert.SerializeObject(mapInfo)}");
            }
            var r= method.Invoke(o, args);
            if (method.ReturnType.FullName == "System.Void")
                return null;
            return r;

            //List<ParameterExpression> paramExps = new List<ParameterExpression>();
            //List<Expression> vParamExps = new List<Expression>();
            //var paramTypes = types;
            //foreach (var pt in paramTypes)
            //{
            //    var pEx = Expression.Parameter(pt);
            //    paramExps.Add(pEx);
            //    vParamExps.Add(Expression.Convert(pEx, typeof(object)));
            //}
            //var lambdaExpr = Expression.Lambda(Expression.Call(Expression.Constant(o), method, paramExps), paramExps).Compile();
            //r = lambdaExpr.DynamicInvoke(args);
        }
    }
    class StaticInvokeHandle : ObjectInvokeHandle
    {
        public StaticInvokeHandle(JsNativeInvokeContext context) : base(context) { }
        protected override object InvokeMethod(Type[] types, object[] args)
        {
            var mapInfo = this.Context.MapInfo;
            var method = Type.GetType(mapInfo.Path).GetMethod(mapInfo.Name, types);
            if (method == null)
            {
                throw new Exception($"未知映射{JsonConvert.SerializeObject(mapInfo)}");
            }
            var r = method.Invoke(null, args);
            if (method.ReturnType.FullName == "System.Void")
                return null;
            return r;
        }
    }
    class SetAccessHandle : ObjectInvokeHandle
    {
        internal SetAccessHandle(JsNativeInvokeContext context) : base(context) { }
        protected override object InvokeMethod(Type[] types, object[] args)
        {
            var mapInfo = this.Context.MapInfo;
            var obj = this.Context.WebBrowserInfo.GetNativeObject(mapInfo.Id,false);
            if (obj == null)
                throw new Exception($"本机对象{mapInfo.Id}不存在");
            var o = obj;
            var method = o.GetType().GetProperty(mapInfo.Name).SetMethod;
            var r = method.Invoke(o, args);
            if (method.ReturnType.FullName == "System.Void")
                return null;
            return r;
        }
    }
    class GetAccessHandle : ObjectInvokeHandle    {
        internal GetAccessHandle(JsNativeInvokeContext context) : base(context) { }
        protected override object InvokeMethod(Type[] types, object[] args)
        {
            var mapInfo = this.Context.MapInfo;
            var obj = this.Context.WebBrowserInfo.GetNativeObject(mapInfo.Id,false);
            if (obj == null)
                throw new Exception($"本机对象{mapInfo.Id}不存在");
            var o = obj;
            var method = o.GetType().GetProperty(mapInfo.Name).GetMethod;
            var r = method.Invoke(o, args);
            if (method.ReturnType.FullName == "System.Void")
                return null;
            return r;
        }
    }
}
