var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
let ThisForm;
var Tnelab;
(function (Tnelab) {
    let TM = Tnelab;
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
    let TneQueryId;
    (function (TneQueryId) {
        TneQueryId[TneQueryId["NativeMap"] = 1] = "NativeMap"; /*本机对象调用映射*/
        TneQueryId[TneQueryId["RegisterNativeMap"] = 2] = "RegisterNativeMap";
        TneQueryId[TneQueryId["DeleteNativeObject"] = 3] = "DeleteNativeObject"; /*删除本机对象*/
        TneQueryId[TneQueryId["GetThisFormHashCode"] = 4] = "GetThisFormHashCode"; /*获取当前窗口ID*/
        TneQueryId[TneQueryId["RunFunctionForTneForm"] = 5] = "RunFunctionForTneForm"; /*在指定TneForm中执行function*/
        TneQueryId[TneQueryId["ShowContextMenuForTneForm"] = 6] = "ShowContextMenuForTneForm";
    })(TneQueryId || (TneQueryId = {}));
    ;
    let GcMap = new Map();
    //垃圾回收支持结构
    function OnSetGC(id, gc) {
        GcMap.set(id, gc);
    }
    TM.OnSetGC = OnSetGC;
    function OnGetGC(id) {
        let gc = undefined;
        if (GcMap.has(id)) {
            gc = GcMap.get(id);
            GcMap.delete(id);
        }
        return gc;
    }
    TM.OnGetGC = OnGetGC;
    //本机调用,用于序列化RunJs的调用结果为JSON
    class OnCallJsInfo {
    }
    function OnCallJs(args) {
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
    TM.OnCallJs = OnCallJs;
    //js通讯回调
    function OnResponse(msgId, response) {
        response = unescape(response);
        return response;
    }
    TM.OnResponse = OnResponse;
    //js通道异步封装
    function TneQueryAsync(msgId, request) {
        return __awaiter(this, void 0, void 0, function* () {
            return new Promise((resolve, reject) => {
                //调用原始callback的js通讯接口
                request = escape(request);
                mbQuery(msgId, request, (id, response) => {
                    resolve(OnResponse(id, response));
                });
            });
        });
    }
    //本机调用通道
    function NativeMapAsync(json) {
        return __awaiter(this, void 0, void 0, function* () {
            return yield TneQueryAsync(TneQueryId.NativeMap, json);
        });
    }
    //类型映射注册通道
    function RegisterNativeMapAsync(nativeTypeName, jsTypeName) {
        return __awaiter(this, void 0, void 0, function* () {
            let args = { NativeTypeName: nativeTypeName, JsTypeName: jsTypeName };
            yield TneQueryAsync(TneQueryId.RegisterNativeMap, JSON.stringify(args));
        });
    }
    Tnelab.RegisterNativeMapAsync = RegisterNativeMapAsync;
    //本机对象释放通道
    function DeleteNativeObject(id) {
        let args = id.toString();
        TneQueryAsync(TneQueryId.DeleteNativeObject, args);
    }
    //在TneForm中执行Function
    function RunFunctionForTneForm(theTneForm, json, func) {
        return __awaiter(this, void 0, void 0, function* () {
            let request = JSON.stringify({ TneFormId: theTneForm.TneMapNativeObjectId, Arg: json, Function: func.toString() });
            let result = yield TneQueryAsync(TneQueryId.RunFunctionForTneForm, request);
            return result;
        });
    }
    Tnelab.RunFunctionForTneForm = RunFunctionForTneForm;
    //在TneForm中显示上下文菜单
    function ShowContextMenuForTneForm(theTneForm, elm, evt, width, height, url) {
        return __awaiter(this, void 0, void 0, function* () {
            let request = JSON.stringify({ TneFormId: theTneForm.TneMapNativeObjectId, ElementY: Math.round(elm.getBoundingClientRect().top), X: evt.x, Y: evt.y, width, height, Url: url });
            let result = yield TneQueryAsync(TneQueryId.ShowContextMenuForTneForm, request);
            return result;
        });
    }
    Tnelab.ShowContextMenuForTneForm = ShowContextMenuForTneForm;
    //var JsFunctionMap: Map<number, Function> = new Map<number, Function>();
    /////////////////////////////////////////////////////////////////////////JsNativeMap
    //装饰器，用于修饰本机映射
    //用法为在属性或者方法上标注：@InvokeInfo("本机方法或属性名称，可为undefined=和方法或属性同名","参数1类型名称","参数2....."....)
    //对于一些通用类型，比如字符串，此声明可以省略，但不能中断参数类型声明
    function InvokeInfo(nativeName, ...args) {
        return function (target, propertyKey, descriptor) {
            if (descriptor.set != undefined) {
                descriptor.set["NativeName"] = nativeName;
                descriptor.set["ArgsTypes"] = args;
            }
            if (descriptor.value != undefined) {
                if (descriptor.value["ArgsTypes"] == undefined) {
                    descriptor.value["NativeName"] = new Array();
                    descriptor.value["ArgsTypes"] = new Array();
                }
                descriptor.value["NativeName"].push(nativeName);
                descriptor.value["ArgsTypes"].push(args);
            }
        };
    }
    Tnelab.InvokeInfo = InvokeInfo;
    //装饰器，事件
    function IsEvent(type) {
        return function (target, propertyKey, descriptor) {
            if (descriptor.get != undefined) {
                descriptor.get["IsEvent"] = type;
            }
        };
    }
    Tnelab.IsEvent = IsEvent;
    //类型映射JS存根
    class JsMapInfo {
        constructor() { }
        static GetMapInfoByNativeTypePath(nativeTypePath) {
            if (JsMapInfo.MapDicByNativeTypePath.has(nativeTypePath)) {
                return JsMapInfo.MapDicByNativeTypePath.get(nativeTypePath);
            }
            return null;
        }
        static GetMapInfoByJsTypePath(jsTypePath) {
            if (JsMapInfo.MapDicByJsTypePath.has(jsTypePath)) {
                let nativeTypePath = JsMapInfo.MapDicByJsTypePath.get(jsTypePath);
                return JsMapInfo.GetMapInfoByNativeTypePath(nativeTypePath);
            }
            return null;
        }
        static GetMapInfoByJsObject(jsObject) {
            //let iterator = JsMapInfo.MapDicByNativeTypePath.values();
            //let r: IteratorResult<JsMapInfo>;
            //while (r = iterator.next(), !r.done) {
            //    if (jsObject instanceof r.value.JsType) {
            //        return r.value;
            //    }
            //}
            //return null;
            return JsMapInfo.GetMapInfoByJsTypePath(Reflect.getPrototypeOf(jsObject).constructor.TneMapJsTypePath);
        }
        static AddMapInfo(jsTypePath, nativeTypePath, jsType) {
            jsTypePath = jsTypePath.trim();
            nativeTypePath = nativeTypePath.trim();
            jsType.TneMapJsTypePath = jsTypePath;
            let mapInfo = new JsMapInfo();
            mapInfo.JsType = jsType;
            mapInfo.NativeTypePath = nativeTypePath;
            mapInfo.JsTypePath = jsTypePath;
            JsMapInfo.MapDicByNativeTypePath.set(nativeTypePath, mapInfo);
            JsMapInfo.MapDicByJsTypePath.set(jsTypePath, nativeTypePath);
        }
    }
    JsMapInfo.MapDicByNativeTypePath = new Map();
    JsMapInfo.MapDicByJsTypePath = new Map();
    //装饰器，用于修饰本机映射
    //用法为在类上：@ToMap("JS类型路径"，"本机类型路径")
    function ToMap(jsTypePath, nativeTypePath) {
        return function (target) {
            !target.prototype.TneMap && (target.prototype.TneMap = {});
            target.prototype.TneMap.IsBuilded = false;
            target.prototype.TneMap.ConstructorInfos = new Array();
            JsMapInfo.AddMapInfo(jsTypePath, nativeTypePath, target);
        };
    }
    Tnelab.ToMap = ToMap;
    //装饰器，用于类，标注构造函数签名    
    function ConstructorInfo(...args) {
        return function (target) {
            target.prototype.TneMap.ConstructorInfos.push(args);
        };
    }
    Tnelab.ConstructorInfo = ConstructorInfo;
    //调用类型
    let MapAction;
    (function (MapAction) {
        MapAction[MapAction["Create"] = 1] = "Create";
        MapAction[MapAction["SetAccess"] = 2] = "SetAccess";
        MapAction[MapAction["GetAccess"] = 3] = "GetAccess";
        MapAction[MapAction["StaticInvoke"] = 4] = "StaticInvoke";
        MapAction[MapAction["InstanceInvoke"] = 5] = "InstanceInvoke";
        MapAction[MapAction["TneEventAddListener"] = 6] = "TneEventAddListener";
        MapAction[MapAction["TneEventRemoveListener"] = 7] = "TneEventRemoveListener";
    })(MapAction || (MapAction = {}));
    //调用传递的参数类型描述，非本机或js类型，表示的是数据的元信息
    let MapDataType;
    (function (MapDataType) {
        MapDataType[MapDataType["NativeObjectId"] = 1] = "NativeObjectId"; /*表示本机对象ID*/
        MapDataType[MapDataType["Value"] = 2] = "Value"; /*标识一个原生值*/
        MapDataType[MapDataType["FunctionId"] = 3] = "FunctionId"; /*表示数据为一个js函数*/
    })(MapDataType || (MapDataType = {}));
    ;
    //调用传递的数据描述
    class MapDataInfo {
    }
    //一个本机对象描述
    class NativeObjectInfo {
    }
    TM.NativeObjectInfo = NativeObjectInfo;
    //映射结果描述
    class MapResult {
    }
    //调用构造函数描述
    class MapCreateActionInfo {
        constructor() {
            this.Action = MapAction.Create;
            this.Args = new Array();
        }
    }
    //本机实例调用描述
    class MapInstanceInvokeActionInfo {
        constructor() {
            this.Action = MapAction.InstanceInvoke;
            this.Args = new Array();
        }
    }
    //本机静态调用描述
    class MapStaticInvokeActionInfo {
        constructor() {
            this.Action = MapAction.StaticInvoke;
            this.Args = new Array();
        }
    }
    //设置属性描述
    class MapSetAccessActionInfo {
        constructor() {
            this.Action = MapAction.SetAccess;
            this.Args = new Array();
        }
    }
    //获取属性描述
    class MapGetAccessActionInfo {
        constructor() {
            this.Action = MapAction.GetAccess;
            this.Args = new Array();
        }
    }
    //添加事件侦听器
    class MapTneEventAddListenerActionInfo {
        constructor() {
            this.Action = MapAction.TneEventAddListener;
            this.Args = new Array();
        }
    }
    //移除事件侦听器
    class MapTneEventRemoveListenerActionInfo {
        constructor() {
            this.Action = MapAction.TneEventRemoveListener;
            this.Args = new Array();
        }
    }
    //核心映射器，此类处理所有映射操作js部分
    class NativeMapper {
        //调用本机构造函数
        static NativeObjectConstructorAsync(nativeTypePath, argTypes, args) {
            return __awaiter(this, void 0, void 0, function* () {
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
                let resultValue = yield NativeMapper.MapNativeObjectAsync(mapInfo);
                return resultValue.Id;
            });
        }
        //动态构造具备本机映射能力的JS类型,构造后该js类型可以和本机类型同步，根据规则动态生成js代码
        static BuildNativeType(mapInfo) {
            return __awaiter(this, void 0, void 0, function* () {
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
                yield RegisterNativeMapAsync(mapInfo.NativeTypePath, mapInfo.JsTypePath);
            });
        }
        static GetTypeNames(genericInfo) {
            let tmps = new Array();
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
        static GetRealNativeType(genericInfo, typeInfo) {
            typeInfo = typeInfo.trim();
            genericInfo = genericInfo.trim();
            let b = typeInfo.indexOf("<");
            if (b !== -1) {
                let subTypeInfo = typeInfo.substring(b + 1, typeInfo.length - 1).trim();
                let subTypeInfos = NativeMapper.GetTypeNames(subTypeInfo);
                //if (subTypeInfos.length === 0 && subTypeInfo.length !== 0) {
                //    subTypeInfos.push(subTypeInfo);
                //}
                let realTypeInfos = new Array();
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
        static MapNativeObjectAsync(mapInfo) {
            return __awaiter(this, void 0, void 0, function* () {
                let resultObject;
                try {
                    resultObject = yield NativeMapAsync(JSON.stringify(mapInfo));
                    if (resultObject === "OK")
                        return;
                    let result = JSON.parse(resultObject);
                    if (!result.Status)
                        throw result.Data.Value;
                    if (mapInfo.Action != MapAction.Create && result.Data.DataType === MapDataType.NativeObjectId) {
                        let nativeTypeName = result.Data.Value.Path;
                        let jsMapInfo = JsMapInfo.GetMapInfoByNativeTypePath(nativeTypeName);
                        let no = new NativeObjectInfo();
                        no.Id = result.Data.Value.Id;
                        no.GenericInfo = result.Data.Value.GenericInfo;
                        no.Path = result.Data.Value.Path;
                        let rx = new jsMapInfo.JsType.prototype.constructor(no);
                        let r = yield rx.Ready();
                        r.TneMapGcObject_ = GcMap.get(r.TneMapNativeObjectId);
                        GcMap.delete(r.TneMapNativeObjectId);
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
            });
        }
        //动态构造静态调用，根据规则动态生成js代码
        static BuildNativeStaticInvoke(jsMapInfo, funcName) {
            jsMapInfo.JsType[funcName] = function () {
                return __awaiter(this, arguments, void 0, function* () {
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
                    let result = yield NativeMapper.MapNativeObjectAsync(mapInfo);
                    return result;
                });
            };
        }
        //动态构造实例调用，根据规则动态生成js代码
        static BuildNativeInvoke(jsMapInfo, protoName) {
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
            if (setFlag) { //构造设置属性调用函数
                let argsTypes = protoDefine.set["ArgsTypes"];
                protoDefine.set = function (value) {
                    return __awaiter(this, void 0, void 0, function* () {
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
                        let result = yield NativeMapper.MapNativeObjectAsync(mapInfo);
                    });
                };
            }
            if (getFlag) { //构造获取属性调用函数体    
                let isEvent = protoDefine.get["IsEvent"];
                if (isEvent != undefined) {
                    protoDefine.get = function () {
                        return __awaiter(this, void 0, void 0, function* () {
                            if (!this.eventMap_.has(protoName)) {
                                this.eventMap_.set(protoName, new TneEvent(this.TneMapNativeObjectId, protoName, isEvent));
                            }
                            return this.eventMap_.get(protoName);
                        });
                    };
                }
                else {
                    protoDefine.get = function () {
                        return __awaiter(this, void 0, void 0, function* () {
                            let mapInfo = new MapGetAccessActionInfo();
                            mapInfo.Id = this.TneMapNativeObjectId;
                            mapInfo.Name = protoName;
                            let result = yield NativeMapper.MapNativeObjectAsync(mapInfo);
                            return result;
                        });
                    };
                }
            }
            if (getFlag || setFlag) {
                Object.defineProperty(prop, protoName, protoDefine);
            }
            else { //构造方法调用函数体
                if (prop[protoName] instanceof Function) {
                    let argsTypes = protoDefine.value["ArgsTypes"];
                    let nativeName = protoDefine.value["NativeName"];
                    prop[protoName] = function () {
                        return __awaiter(this, arguments, void 0, function* () {
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
                            let result = yield NativeMapper.MapNativeObjectAsync(mapInfo);
                            return result;
                        });
                    };
                }
            }
        }
    }
    //映射本机事件
    class TneEvent {
        constructor(id, name, type) {
            this.nativeObjectId_ = id;
            this.name_ = name;
            this.type_ = type;
        }
        AddListener(handler) {
            return __awaiter(this, void 0, void 0, function* () {
                let mapInfo = new MapTneEventAddListenerActionInfo();
                mapInfo.Id = this.nativeObjectId_;
                mapInfo.Name = this.name_;
                let data = new MapDataInfo();
                data.DataType = MapDataType.FunctionId;
                data.Value = handler.toString(); //EventHandler<DragFilesEventArgs>
                data.NativeTypePath = this.type_;
                mapInfo.Args.push(data);
                let result = yield NativeMapper.MapNativeObjectAsync(mapInfo);
                return result;
            });
        }
        RemoveListener(handler) {
            return __awaiter(this, void 0, void 0, function* () {
                let mapInfo = new MapTneEventRemoveListenerActionInfo();
                mapInfo.Id = this.nativeObjectId_;
                mapInfo.Name = this.name_;
                let data = new MapDataInfo();
                data.DataType = MapDataType.FunctionId;
                data.Value = handler.toString(); //EventHandler<DragFilesEventArgs>
                data.NativeTypePath = this.type_;
                mapInfo.Args.push(data);
                let result = yield NativeMapper.MapNativeObjectAsync(mapInfo);
                return result;
            });
        }
    }
    Tnelab.TneEvent = TneEvent;
    //所有js本机同步类型的基类
    class NativeObject {
        constructor(args) {
            this.eventMap_ = new Map();
            this.TneMapGenericInfo = "";
            if (args === undefined) {
                this.args_ = new Array();
            }
            else {
                this.args_ = args;
            }
        }
        Ready() {
            return __awaiter(this, void 0, void 0, function* () {
                let mapInfo = JsMapInfo.GetMapInfoByJsObject(this);
                if (!mapInfo.JsType.prototype.TneMap.IsBuilded) {
                    yield NativeObject.Build(mapInfo);
                    mapInfo.JsType.prototype.TneMap.IsBuilded = true;
                }
                if (this.args_.length == 1 && (this.args_[0] instanceof NativeObjectInfo)) {
                    let noi = this.args_[0];
                    this.TneMapNativeObjectId = noi.Id;
                    this.TneMapGenericInfo = noi.GenericInfo;
                    return this;
                }
                let realArgsIndex = 0;
                let genericInfo = null;
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
                this.TneMapNativeObjectId = yield NativeMapper.NativeObjectConstructorAsync(nativeTypePath, constructorInfos, args);
                this.TneMapGcObject_ = GcMap.get(this.TneMapNativeObjectId);
                GcMap.delete(this.TneMapNativeObjectId);
                return this;
            });
        }
        FreeTneMapNativeObject() {
            DeleteNativeObject(this.TneMapNativeObjectId);
        }
        static Build(mapInfo) {
            return __awaiter(this, void 0, void 0, function* () {
                yield NativeMapper.BuildNativeType(mapInfo);
            });
        }
    }
    NativeObject.TypeList = new Array();
    Tnelab.NativeObject = NativeObject;
    /////////////////////////////////////////////////////////////////////////////////ThisForm
    //自动定义当前窗口的本机id，可以用该ID随时构造出当前窗口本机对象的js同步对象    
    let dom_ready_ = function () {
        return __awaiter(this, void 0, void 0, function* () {
            document.removeEventListener("DOMContentLoaded", dom_ready_, false);
            let hashCode = yield TneQueryAsync(TneQueryId.GetThisFormHashCode, "GetThisFormHashCode");
            let no = new NativeObjectInfo();
            no.Id = parseInt(hashCode);
            no.GenericInfo = "";
            let proxy = TMiniblink.TneForm;
            ThisForm = yield new proxy(no).Ready();
        });
    };
    document.addEventListener("DOMContentLoaded", dom_ready_, true);
    /////////////////////////////////////////////////////////////////////////////////JSON
})(Tnelab || (Tnelab = {}));
///<reference path="./TneMap.ts"/>
var Tnelab;
(function (Tnelab) {
    class TneFormBase extends Tnelab.NativeObject {
        RunFunc(func, json) {
            return __awaiter(this, void 0, void 0, function* () {
                return yield Tnelab.RunFunctionForTneForm(this, json, func);
            });
        }
        ShowContextMenu(elm, evt, width, height, url) {
            return __awaiter(this, void 0, void 0, function* () {
                return yield Tnelab.ShowContextMenuForTneForm(this, elm, evt, width, height, url);
            });
        }
    }
    Tnelab.TneFormBase = TneFormBase;
})(Tnelab || (Tnelab = {}));
//此代码由机器生成，请不要手动修改
///<reference path="./TneMap.ts"/>
///<reference path="./TneFormBase.ts"/>
var TMiniblink;
(function (TMiniblink) {
    let TneForm = class TneForm extends Tnelab.TneFormBase {
        constructor(_url) { super(arguments); }
        get DragFilesEvent() { return undefined; }
        get Handle() { return undefined; }
        get Title() { return undefined; }
        set SizeAble(value) { }
        get SizeAble() { return undefined; }
        set X(value) { }
        get X() { return undefined; }
        set Y(value) { }
        get Y() { return undefined; }
        set Width(value) { }
        get Width() { return undefined; }
        set Height(value) { }
        get Height() { return undefined; }
        set ShowInTaskBar(value) { }
        get ShowInTaskBar() { return undefined; }
        set TopMost(value) { }
        get TopMost() { return undefined; }
        set MinWidth(value) { }
        get MinWidth() { return undefined; }
        set MinHeight(value) { }
        get MinHeight() { return undefined; }
        get Url() { return undefined; }
        set StartPosition(value) { }
        get StartPosition() { return undefined; }
        set WindowState(value) { }
        get WindowState() { return undefined; }
        set Parent(value) { }
        get Parent() { return undefined; }
        set AllowDrop(value) { }
        get AllowDrop() { return undefined; }
        set Icon(value) { }
        get Icon() { return undefined; }
        Close() { }
        ShowDialog() { }
        Show() { }
        Hide() { }
        Move() { }
        Active() { }
        Equals(_obj) { return undefined; }
        static Equals_(_objA, _objB) { return undefined; }
        GetHashCode() { return undefined; }
        GetType() { return undefined; }
        static ReferenceEquals(_objA, _objB) { return undefined; }
        ToString() { return undefined; }
    };
    __decorate([
        Tnelab.IsEvent("System.EventHandler<Tnelab.HtmlView.DragFilesEventArgs>")
    ], TneForm.prototype, "DragFilesEvent", null);
    __decorate([
        Tnelab.InvokeInfo(undefined, "System.Boolean")
    ], TneForm.prototype, "SizeAble", null);
    __decorate([
        Tnelab.InvokeInfo(undefined, "System.Int32")
    ], TneForm.prototype, "X", null);
    __decorate([
        Tnelab.InvokeInfo(undefined, "System.Int32")
    ], TneForm.prototype, "Y", null);
    __decorate([
        Tnelab.InvokeInfo(undefined, "System.Int32")
    ], TneForm.prototype, "Width", null);
    __decorate([
        Tnelab.InvokeInfo(undefined, "System.Int32")
    ], TneForm.prototype, "Height", null);
    __decorate([
        Tnelab.InvokeInfo(undefined, "System.Boolean")
    ], TneForm.prototype, "ShowInTaskBar", null);
    __decorate([
        Tnelab.InvokeInfo(undefined, "System.Boolean")
    ], TneForm.prototype, "TopMost", null);
    __decorate([
        Tnelab.InvokeInfo(undefined, "System.Int32")
    ], TneForm.prototype, "MinWidth", null);
    __decorate([
        Tnelab.InvokeInfo(undefined, "System.Int32")
    ], TneForm.prototype, "MinHeight", null);
    __decorate([
        Tnelab.InvokeInfo(undefined, "Tnelab.HtmlView.StartPosition")
    ], TneForm.prototype, "StartPosition", null);
    __decorate([
        Tnelab.InvokeInfo(undefined, "Tnelab.HtmlView.WindowState")
    ], TneForm.prototype, "WindowState", null);
    __decorate([
        Tnelab.InvokeInfo(undefined, "Tnelab.HtmlView.TneForm")
    ], TneForm.prototype, "Parent", null);
    __decorate([
        Tnelab.InvokeInfo(undefined, "System.Boolean")
    ], TneForm.prototype, "AllowDrop", null);
    __decorate([
        Tnelab.InvokeInfo(undefined, "System.String")
    ], TneForm.prototype, "Icon", null);
    __decorate([
        Tnelab.InvokeInfo("Close")
    ], TneForm.prototype, "Close", null);
    __decorate([
        Tnelab.InvokeInfo("ShowDialog")
    ], TneForm.prototype, "ShowDialog", null);
    __decorate([
        Tnelab.InvokeInfo("Show")
    ], TneForm.prototype, "Show", null);
    __decorate([
        Tnelab.InvokeInfo("Hide")
    ], TneForm.prototype, "Hide", null);
    __decorate([
        Tnelab.InvokeInfo("Move")
    ], TneForm.prototype, "Move", null);
    __decorate([
        Tnelab.InvokeInfo("Active")
    ], TneForm.prototype, "Active", null);
    __decorate([
        Tnelab.InvokeInfo("Equals", "System.Object")
    ], TneForm.prototype, "Equals", null);
    __decorate([
        Tnelab.InvokeInfo("GetHashCode")
    ], TneForm.prototype, "GetHashCode", null);
    __decorate([
        Tnelab.InvokeInfo("GetType")
    ], TneForm.prototype, "GetType", null);
    __decorate([
        Tnelab.InvokeInfo("ToString")
    ], TneForm.prototype, "ToString", null);
    __decorate([
        Tnelab.InvokeInfo("Equals_", "System.Object", "System.Object")
    ], TneForm, "Equals_", null);
    __decorate([
        Tnelab.InvokeInfo("ReferenceEquals", "System.Object", "System.Object")
    ], TneForm, "ReferenceEquals", null);
    TneForm = __decorate([
        Tnelab.ConstructorInfo("System.String"),
        Tnelab.ToMap("TMiniblink.TneForm", "Tnelab.HtmlView.TneForm")
    ], TneForm);
    TMiniblink.TneForm = TneForm;
    Tnelab.RegisterNativeMapAsync("Tnelab.HtmlView.TneForm", "TMiniblink.TneForm");
})(TMiniblink || (TMiniblink = {}));
//# sourceMappingURL=TneApp.js.map