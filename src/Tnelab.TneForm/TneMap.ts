namespace Tnelab {
    /*////////////////////////////////////////////////////////////////////////说明
    基本概念：
    本机对象：指的是c#对象
    本机类型：指的是c#类型
    映射：指的是把js和c#关联起来
    概述
    此文件集中处理了js<=>c#的中间js的相关通讯过程，把所有js对本机的操作通过json封装传递给c#
    ////////////////////////////////////////////////////////////////////////////*/
    /////////////////////////////////////////////////////////////////////////全局
    //js通讯通道分类
    enum TneQueryId { NativeMap = 1/*本机对象调用映射*/, RegisterNativeMap/*注册类型映射*/ = 2, DeleteNativeObject = 3/*删除本机对象*/, GetThisFormHashCode = 4 /*获取当前窗口ID*/ };
    //原始js通讯声明
    declare function mbQuery(msgId: number, request: string, onResponse: (id: number, response: string) => any): void;
    let GcMap = new Map<number, object>();
    export function OnSetGC(id: number, gc: object) {
        GcMap.set(id, gc);
    }
    //本机调用,用于序列化RunJs的调用结果为JSON
    class OnCallJsInfo {
        public Status: boolean;
        public Data: any;
    }
    export function OnCallJs(args: any) {
        let result;
        try {
            let info = new OnCallJsInfo();
            info.Status = true;
            info.Data = args;
            result = JSON.stringify(info);
        }
        catch (error) {
            let info = new OnCallJsInfo();
            info.Status = false;
            info.Data = error;
            result = JSON.stringify(info);
        }
        return result;
    }
    //js通讯回调
    export function OnResponse(msgId: number, response: string) {
        response = unescape(response);
        return response;
    }
    //js通道异步封装
    async function TneQueryAsync(msgId: number, request: string): Promise<any> {
        return new Promise<any>((resolve, reject) => {
            //调用原始callback的js通讯接口
            request = escape(request);
            mbQuery(msgId, request, (id, response) => {
                resolve(OnResponse(id, response));
            });
        });
    }
    //本机调用通道
    async function NativeMapAsync(json: string): Promise<any> {
        return await TneQueryAsync(TneQueryId.NativeMap, json);
    }
    //类型映射注册通道
    async function RegisterNativeMapAsync(nativeTypeName, jsTypeName): Promise<void> {
        let args = { NativeTypeName: nativeTypeName, JsTypeName: jsTypeName };
        await TneQueryAsync(TneQueryId.RegisterNativeMap, JSON.stringify(args));
    }
    //本机对象释放通道
    function DeleteNativeObject(id: number): void {
        let args = id.toString();
        TneQueryAsync(TneQueryId.DeleteNativeObject, args);
    }
    //var JsFunctionMap: Map<number, Function> = new Map<number, Function>();
    /////////////////////////////////////////////////////////////////////////JsNativeMap
    //装饰器，用于修饰本机映射
    //用法为在属性或者方法上标注：@InvokeInfo("本机方法或属性名称，可为undefined=和方法或属性同名","参数1类型名称","参数2....."....)
    //对于一些通用类型，比如字符串，此声明可以省略，但不能中断参数类型声明
    export function InvokeInfo(nativeName: string, ...args: string[]) {
        return function (target: any, propertyKey: string, descriptor: PropertyDescriptor) {
            if (descriptor.set != undefined) {
                descriptor.set["NativeName"] = nativeName;
                descriptor.set["ArgsTypes"] = args;
            }
            if (descriptor.value != undefined) {
                if (descriptor.value["ArgsTypes"] == undefined) {
                    descriptor.value["NativeName"] = new Array<string>();
                    descriptor.value["ArgsTypes"] = new Array<string>();
                }
                descriptor.value["NativeName"].push(nativeName);
                descriptor.value["ArgsTypes"].push(args);
            }
        }
    }
    //类型映射JS存根
    class JsMapInfo {
        private static MapDicByNativeTypePath: Map<string, JsMapInfo> = new Map<string, JsMapInfo>();
        private static MapDicByJsTypePath: Map<string, string> = new Map<string, string>();
        private constructor() { }
        public JsType: Function;
        public JsTypePath: string;
        public NativeTypePath: string;
        public static GetMapInfoByNativeTypePath(nativeTypePath: string): JsMapInfo {
            if (JsMapInfo.MapDicByNativeTypePath.has(nativeTypePath)) {
                return JsMapInfo.MapDicByNativeTypePath.get(nativeTypePath);
            }
            return null;
        }
        public static GetMapInfoByJsTypePath(jsTypePath: string): JsMapInfo {
            if (JsMapInfo.MapDicByJsTypePath.has(jsTypePath)) {
                let nativeTypePath = JsMapInfo.MapDicByJsTypePath.get(jsTypePath);
                return JsMapInfo.GetMapInfoByNativeTypePath(nativeTypePath);
            }
            return null;
        }
        public static GetMapInfoByJsObject(jsObject: object): JsMapInfo {
            //let iterator = JsMapInfo.MapDicByNativeTypePath.values();
            //let r: IteratorResult<JsMapInfo>;
            //while (r = iterator.next(), !r.done) {
            //    if (jsObject instanceof r.value.JsType) {
            //        return r.value;
            //    }
            //}
            //return null;
            return JsMapInfo.GetMapInfoByJsTypePath((Reflect.getPrototypeOf(jsObject).constructor as any).TneMapJsTypePath);
        }
        public static AddMapInfo(jsTypePath: string, nativeTypePath: string, jsType: Function): void {
            jsTypePath = jsTypePath.trim();
            nativeTypePath = nativeTypePath.trim();
            (jsType as any).TneMapJsTypePath = jsTypePath;
            let mapInfo = new JsMapInfo();
            mapInfo.JsType = jsType;
            mapInfo.NativeTypePath = nativeTypePath;
            mapInfo.JsTypePath = jsTypePath;
            JsMapInfo.MapDicByNativeTypePath.set(nativeTypePath, mapInfo);
            JsMapInfo.MapDicByJsTypePath.set(jsTypePath, nativeTypePath);
        }
    }
    //装饰器，用于修饰本机映射
    //用法为在类上：@ToMap("JS类型路径"，"本机类型路径")
    export function ToMap(jsTypePath: string, nativeTypePath: string) {
        return function (target: Function) {
            !target.prototype.TneMap && (target.prototype.TneMap = {})
            target.prototype.TneMap.IsBuilded = false;
            target.prototype.TneMap.ConstructorInfos = new Array();
            JsMapInfo.AddMapInfo(jsTypePath, nativeTypePath, target);
        };
    }
    //装饰器，用于类，标注构造函数签名    
    export function ConstructorInfo(...args: string[]) {
        return function (target: Function) {
            target.prototype.TneMap.ConstructorInfos.push(args);
        };
    }
    //调用类型
    enum MapAction { Create = 1, SetAccess = 2, GetAccess = 3, StaticInvoke = 4, InstanceInvoke = 5 }
    //调用传递的参数类型描述，非本机或js类型，表示的是数据的元信息
    enum MapDataType { NativeObjectId = 1/*表示本机对象ID*/, Value = 2/*标识一个原生值*/, FunctionId = 3 /*表示数据为一个js函数*/ };
    //调用传递的数据描述
    class MapDataInfo {
        public DataType: MapDataType;//元数据类型描述
        public Value;//数据值
        public NativeTypePath: string;//本机类型路径，常为空
    }
    //一个本机对象描述
    class NativeObjectInfo {
        public Id: number;//本机对象唯一ID
        public Path: string;//本机类型路径，常为空
        public GenericInfo: string;//泛型信息
        public GcInfo: string;
        public GCObject: object;//垃圾回收对象
    }
    //映射结果描述
    class MapResult {
        public Status: boolean;//结果状态false为发生异常,data为则为异常信息描述,true表示调用良好,data表述结果描述
        public Data: MapDataInfo;//结果数据
    }
    //调用构造函数描述
    class MapCreateActionInfo {
        public Action: MapAction = MapAction.Create;
        public Path: string;
        public Args: Array<MapDataInfo> = new Array<MapDataInfo>();
    }
    //本机实例调用描述
    class MapInstanceInvokeActionInfo {
        public Action: MapAction = MapAction.InstanceInvoke;
        public Id: Number;//本机对象唯一ID
        public Name: string;
        public Args: Array<MapDataInfo> = new Array<MapDataInfo>();
    }
    //本机静态调用描述
    class MapStaticInvokeActionInfo {
        public Action: MapAction = MapAction.StaticInvoke;
        public Path: string;
        public Name: string;
        public Args: Array<MapDataInfo> = new Array<MapDataInfo>();
    }
    //设置属性描述
    class MapSetAccessActionInfo {
        public Action: MapAction = MapAction.SetAccess
        public Id: Number;
        public Name: string;
        public Args: Array<MapDataInfo> = new Array<MapDataInfo>();
    }
    //获取属性描述
    class MapGetAccessActionInfo {
        public Action: MapAction = MapAction.GetAccess;
        public Id: Number;
        public Name: string;
        public Args: Array<MapDataInfo> = new Array<MapDataInfo>();
    }
    //核心映射器，此类处理所有映射操作js部分
    class NativeMapper {
        //调用本机构造函数
        public static async NativeObjectConstructorAsync(nativeTypePath: string, argTypes: Array<string>, args: Array<any>): Promise<number> {
            let mapInfo = new MapCreateActionInfo();
            mapInfo.Path = nativeTypePath;
            for (let i = 0; i < args.length; i++) {
                let argInfo = new MapDataInfo();
                if (args[i] instanceof NativeObject) {
                    argInfo.DataType = MapDataType.NativeObjectId;
                    argInfo.Value = new NativeObjectInfo();
                    argInfo.Value.Id = args[i].TneMapNativeObjectId;
                    argInfo.Value.GenericInfo = args[i].TneMapGenericInfo;
                }
                else if (args[i] instanceof Function) {
                    argInfo.DataType = MapDataType.FunctionId;
                    argInfo.Value = args[i].toString();
                    argInfo.NativeTypePath = argTypes[i];
                }
                else {
                    argInfo.DataType = MapDataType.Value;
                    argInfo.Value = args[i];
                    argInfo.NativeTypePath = argTypes[i];
                }
                mapInfo.Args.push(argInfo);
            }
            let resultValue = await NativeMapper.MapNativeObjectAsync(mapInfo);
            return resultValue.Id;
        }
        //动态构造具备本机映射能力的JS类型,构造后该js类型可以和本机类型同步，根据规则动态生成js代码
        public static async BuildNativeType(mapInfo: JsMapInfo): Promise<void> {
            let keys = Reflect.ownKeys(mapInfo.JsType);
            for (let i = 0; i < keys.length; i++) {
                if (mapInfo.JsType[keys[i].toString()] instanceof Function) {
                    NativeMapper.BuildNativeStaticInvoke(mapInfo, keys[i].toString());
                }
            }
            keys = Reflect.ownKeys(mapInfo.JsType.prototype);
            for (let i = 0; i < keys.length; i++) {
                if (keys[i].toString() === "Ready" || keys[i].toString() === "constructor" || keys[i].toString() === "FreeTneMapNativeObject") {
                    continue;
                }
                NativeMapper.BuildNativeInvoke(mapInfo, keys[i].toString());
            }
            await RegisterNativeMapAsync(mapInfo.NativeTypePath, mapInfo.JsTypePath);
        }
        public static GetTypeNames(genericInfo: string): Array<string> {
            let tmps = new Array<string>();
            let gb = 0;
            let lc = 0;
            for (let i = 0; i < genericInfo.length; i++) {
                if (genericInfo[i] === "<") {
                    lc++;
                }
                else if (genericInfo[i] === ">") {
                    lc--;
                }
                else if (genericInfo[i] === ",") {
                    if (lc === 0) {
                        tmps.push(genericInfo.substring(gb, i));
                        gb = i + 1;
                    }
                }
            }
            tmps.push(genericInfo.substr(gb));
            return tmps;
        }
        public static GetRealNativeType(genericInfo: string, typeInfo: string): string {
            typeInfo = typeInfo.trim();
            genericInfo = genericInfo.trim();
            let b = typeInfo.indexOf("<");
            if (b !== -1) {
                let subTypeInfo = typeInfo.substring(b + 1, typeInfo.length - 1).trim();
                let subTypeInfos = NativeMapper.GetTypeNames(subTypeInfo);
                //if (subTypeInfos.length === 0 && subTypeInfo.length !== 0) {
                //    subTypeInfos.push(subTypeInfo);
                //}
                let realTypeInfos = new Array<string>();
                for (let t = 0; t < subTypeInfos.length; t++) {
                    realTypeInfos.push(this.GetRealNativeType(genericInfo, subTypeInfos[t]));
                }
                return typeInfo.substring(0, b + 1) + realTypeInfos.join(",") + ">";
            }
            let genericMaps = genericInfo.split("|");
            for (let i = 0; i < genericMaps.length; i++) {
                let map = genericMaps[i].split(":");
                if (typeInfo === map[0]) {
                    typeInfo = map[1];
                    break;
                }
            }
            return typeInfo;
        }
        //调用本机对象
        static async MapNativeObjectAsync(mapInfo: any): Promise<any> {
            let resultObject: string;
            try {
                resultObject = await NativeMapAsync(JSON.stringify(mapInfo));
                if (resultObject === "OK")
                    return;
                let result = JSON.parse(resultObject) as MapResult;
                if (!result.Status)
                    throw result.Data.Value;
                if (mapInfo.Action != MapAction.Create && result.Data.DataType === MapDataType.NativeObjectId) {
                    let nativeTypeName = result.Data.Value.Path;
                    let jsMapInfo = JsMapInfo.GetMapInfoByNativeTypePath(nativeTypeName);
                    let r = await new jsMapInfo.JsType.prototype.constructor(result.Data.Value).Ready();
                    //let r = eval("await new " + jsTypeName + "(result.Data.Value).Ready()");
                    return r;
                }
                return result.Data.Value;
            }
            catch (error) {
                console.error(error);
                console.error(resultObject);
                throw error;
            }
        }
        //动态构造静态调用，根据规则动态生成js代码
        static BuildNativeStaticInvoke(jsMapInfo: JsMapInfo, funcName: string): void {
            jsMapInfo.JsType[funcName] = async function () {
                let mapInfo = new MapStaticInvokeActionInfo();
                for (let i = 0; i < arguments.length; i++) {
                    let argInfo = new MapDataInfo();
                    let t = arguments[i].TneMapNativeObjectId;
                    if (t != undefined) {
                        argInfo.DataType = MapDataType.NativeObjectId;
                        argInfo.Value = new NativeObjectInfo();
                        argInfo.Value.Id = arguments[i].TneMapNativeObjectId;
                        argInfo.Value.Path = "";
                    }
                    else {
                        argInfo.DataType = MapDataType.Value;
                        argInfo.Value = arguments[i];
                    }
                    mapInfo.Args.push(argInfo);
                }
                mapInfo.Path = jsMapInfo.NativeTypePath;
                mapInfo.Name = funcName;
                let result = await NativeMapper.MapNativeObjectAsync(mapInfo);
                return result;
            }
        }
        //动态构造实例调用，根据规则动态生成js代码
        static BuildNativeInvoke(jsMapInfo: JsMapInfo, protoName: string) {
            let prop = jsMapInfo.JsType.prototype;
            let protoDefine = Reflect.getOwnPropertyDescriptor(prop, protoName);
            let setFlag = protoDefine.set != undefined;
            let getFlag = protoDefine.get != undefined;
            //let len = 0;
            //let ds = protoName + ":";
            //for (let p in protoDefine) {
            //    if (p === "set" && p) {
            //        setFlag = true;
            //    }
            //    else if (p === "get") {
            //        getFlag = true;
            //    }
            //    ds += "," + p;
            //}            
            if (setFlag) {//构造设置属性调用函数
                let argsTypes = protoDefine.set["ArgsTypes"];
                protoDefine.set = async function (value) {
                    let mapInfo = new MapSetAccessActionInfo();
                    let argInfo = new MapDataInfo();
                    if (value instanceof NativeObject) {
                        argInfo.DataType = MapDataType.NativeObjectId;
                        argInfo.Value = new NativeObjectInfo();
                        argInfo.Value.Id = value.TneMapNativeObjectId;
                        argInfo.Value.GenericInfo = value.TneMapGenericInfo;
                    }
                    else if (value instanceof Function) {
                        argInfo.DataType = MapDataType.FunctionId;
                        //let funcRnm = Math.random().toString();
                        //funcRnm = funcRnm.substr(2, funcRnm.length - 2)
                        //let funcName = "mapF" + funcRnm;
                        //eval("window." + funcName + "=arguments[i]");
                        //argInfo.Value = funcName;
                        argInfo.Value = value.toString();
                        argInfo.NativeTypePath = NativeMapper.GetRealNativeType(this.TneMapGenericInfo, argsTypes[0]);
                    }
                    else {
                        argInfo.DataType = MapDataType.Value;
                        argInfo.Value = value;
                        if (argsTypes != undefined) {
                            argInfo.NativeTypePath = NativeMapper.GetRealNativeType(this.TneMapGenericInfo, argsTypes[0]);
                        }
                    }
                    mapInfo.Args.push(argInfo);
                    mapInfo.Id = this.TneMapNativeObjectId;
                    mapInfo.Name = protoName;
                    let result = await NativeMapper.MapNativeObjectAsync(mapInfo);
                }
            }
            if (getFlag) {//构造获取属性调用函数体                
                protoDefine.get = async function () {
                    let mapInfo = new MapGetAccessActionInfo();
                    mapInfo.Id = this.TneMapNativeObjectId;
                    mapInfo.Name = protoName;
                    let result = await NativeMapper.MapNativeObjectAsync(mapInfo);
                    return result;
                }
            }
            if (getFlag || setFlag) {
                Object.defineProperty(prop, protoName, protoDefine);
            }
            else {//构造方法调用函数体
                if (prop[protoName] instanceof Function) {
                    let argsTypes = protoDefine.value["ArgsTypes"];
                    let nativeName = protoDefine.value["NativeName"];
                    prop[protoName] = async function () {
                        let classGenericInfo = this.TneMapGenericInfo;
                        let mapInfo = new MapInstanceInvokeActionInfo();
                        let types = argsTypes == undefined ? undefined : argsTypes[0];
                        let name = nativeName == undefined ? protoName : nativeName[0];
                        let genericInfoIndex = 0;
                        let argsIndex = 0;
                        if (argsTypes != undefined && argsTypes.length > 1) {
                            types = argsTypes[arguments[0] - 1];
                            name = nativeName[arguments[0] - 1];
                            genericInfoIndex = 1;
                            argsIndex = 1;
                        }
                        let isGeneric = name.indexOf("<") != -1;
                        if (isGeneric && types != undefined) {
                            let genericInfo = arguments[genericInfoIndex];
                            argsIndex = genericInfoIndex + 1;
                            let tmps = NativeMapper.GetTypeNames(genericInfo);
                            let b = name.indexOf("<");
                            let genericDefines = name.substring(b + 1, name.length - 1).trim().split(",");
                            if (tmps.length != genericDefines.length)
                                throw "泛型参数不匹配,name:" + name + ",RealTypeInfo:" + genericInfo;
                            let funcGenericInfo = "";
                            for (let i = 0; i < genericDefines.length; i++) {
                                funcGenericInfo += "|" + genericDefines[i].trim() + ":" + tmps[i].trim();
                            }
                            if (funcGenericInfo.length > 0) {
                                funcGenericInfo = funcGenericInfo.substr(1);
                            }
                            for (let i = 0; i < types.length; i++) {
                                types[i] = NativeMapper.GetRealNativeType(funcGenericInfo, types[i]);
                                types[i] = NativeMapper.GetRealNativeType(classGenericInfo, types[i]);
                            }
                            name = NativeMapper.GetRealNativeType(funcGenericInfo, name);
                        }
                        for (let i = argsIndex; i < arguments.length; i++) {
                            let argInfo = new MapDataInfo();
                            if (arguments[i] instanceof NativeObject) {
                                argInfo.DataType = MapDataType.NativeObjectId;
                                argInfo.Value = new NativeObjectInfo();
                                argInfo.Value.Id = arguments[i].TneMapNativeObjectId;
                                argInfo.Value.GenericInfo = arguments[i].TneMapGenericInfo;
                            }
                            else if (arguments[i] instanceof Function) {
                                argInfo.DataType = MapDataType.FunctionId;
                                //let funcRnm = Math.random().toString();
                                //funcRnm = funcRnm.substr(2, funcRnm.length - 2)
                                //let funcName = "mapF" + funcRnm;
                                //eval("window." + funcName + "=arguments[i]");
                                //argInfo.Value = funcName;
                                argInfo.Value = arguments[i].toString();
                                argInfo.NativeTypePath = types == undefined ? null : types[i - argsIndex];
                            }
                            else {
                                argInfo.DataType = MapDataType.Value;
                                argInfo.Value = arguments[i];
                                argInfo.NativeTypePath = types == undefined ? null : types[i - argsIndex];
                            }
                            mapInfo.Args.push(argInfo);
                        }
                        mapInfo.Id = this.TneMapNativeObjectId;
                        mapInfo.Name = name;
                        let result = await NativeMapper.MapNativeObjectAsync(mapInfo);
                        return result;
                    }
                }
            }
        }
    }
    //所有js本机同步类型的基类
    export abstract class NativeObject {
        public TneMapNativeObjectId: number;
        private TneMapGcObject_: object;
        private args_: any;
        public constructor(args?: IArguments) {
            if (args === undefined) {
                this.args_ = new Array();
            }
            else {
                this.args_ = args;
            }
        }
        public TneMapGenericInfo: string = "";
        public async Ready() {//请务必使用let t=await new obj().ready()来构造本机类型对象              
            let mapInfo = JsMapInfo.GetMapInfoByJsObject(this);
            if (!mapInfo.JsType.prototype.TneMap.IsBuilded) {
                await NativeObject.Build(mapInfo);
                mapInfo.JsType.prototype.TneMap.IsBuilded = true;
            }
            if (this.args_.length == 1 && (this.args_[0] instanceof NativeObjectInfo)) {
                let noi = this.args_[0] as NativeObjectInfo;
                this.TneMapNativeObjectId = noi.Id
                this.TneMapGenericInfo = noi.GenericInfo;
                return this;
            }
            let realArgsIndex = 0;
            let genericInfo: string = null;
            let isGeneric = mapInfo.JsTypePath.indexOf("<") != -1;
            let constructorInfos = mapInfo.JsType.prototype.TneMap.ConstructorInfos;
            if (constructorInfos.length > 1) {
                constructorInfos = constructorInfos[this.args_[0] - 1];
                if (isGeneric) {
                    genericInfo = this.args_[1].trim();
                    realArgsIndex = 2;
                }
                else {
                    realArgsIndex = 1;
                }
            }
            else {
                constructorInfos = constructorInfos[0];
                if (isGeneric) {
                    genericInfo = this.args_[0].trim();
                    realArgsIndex = 1;
                }
                else {
                    realArgsIndex = 0;
                }
            }
            if (genericInfo != null) {
                let tmps = NativeMapper.GetTypeNames(genericInfo);
                let b = mapInfo.JsTypePath.indexOf("<");
                let genericDefines = mapInfo.JsTypePath.substring(b + 1, mapInfo.JsTypePath.length - 1).trim().split(",");
                if (tmps.length != genericDefines.length)
                    throw "泛型参数不匹配,JsTypePath:" + mapInfo.JsTypePath + ",NativeTypePath:" + mapInfo.NativeTypePath + ",RealTypeInfo:" + tmps;
                this.TneMapGenericInfo = "";
                for (let i = 0; i < genericDefines.length; i++) {
                    this.TneMapGenericInfo += "|" + genericDefines[i].trim() + ":" + tmps[i].trim();
                }
                if (this.TneMapGenericInfo.length > 0) {
                    this.TneMapGenericInfo = this.TneMapGenericInfo.substr(1);
                }
                for (let i = 0; i < constructorInfos.length; i++) {
                    //let old = constructorInfos[i];
                    constructorInfos[i] = NativeMapper.GetRealNativeType(this.TneMapGenericInfo, constructorInfos[i]);
                    //alert(old + ":" + constructorInfos[i]);
                }
            }
            let args = new Array();
            for (let i = realArgsIndex; i < this.args_.length; i++) {
                args.push(this.args_[i]);
            }
            let nativeTypePath = NativeMapper.GetRealNativeType(this.TneMapGenericInfo, mapInfo.NativeTypePath);
            //alert(nativeTypePath);
            //alert(nativeTypePath);
            this.TneMapNativeObjectId = await NativeMapper.NativeObjectConstructorAsync(nativeTypePath, constructorInfos, args);
            this.TneMapGcObject_ = GcMap.get(this.TneMapNativeObjectId);
            GcMap.delete(this.TneMapNativeObjectId);
            return this;
        }
        public FreeTneMapNativeObject(): void {//不在使用本机对象后，请务必调用此方法，销毁本机对象
            DeleteNativeObject(this.TneMapNativeObjectId);
        }
        static async Build(mapInfo: JsMapInfo) {
            await NativeMapper.BuildNativeType(mapInfo);
        }
        public static TypeList: Array<string> = new Array<string>();
    }
    /////////////////////////////////////////////////////////////////////////////////ThisForm
    //自动定义当前窗口的本机id，可以用该ID随时构造出当前窗口本机对象的js同步对象    
    export let ThisForm: TneForm;
    let dom_ready_ = async function () {
        document.removeEventListener("DOMContentLoaded", dom_ready_, false);
        let hashCode = await TneQueryAsync(TneQueryId.GetThisFormHashCode, "GetThisFormHashCode");
        let no = new NativeObjectInfo();
        no.Id = hashCode;
        no.GenericInfo = "";
        let proxy = TneForm as any;
        ThisForm = await new proxy(no).Ready();
    };
    document.addEventListener("DOMContentLoaded", dom_ready_, true);
    /////////////////////////////////////////////////////////////////////////////////JSON
}
