declare namespace Tnelab {
    function OnSetGC(id: number, gc: object): void;
    function OnCallJs(args: any): any;
    function InvokeInfo(nativeName: string, ...args: string[]): (target: any, propertyKey: string, descriptor: PropertyDescriptor) => void;
    class JsMapInfo {
        private static MapDicByNativeTypePath;
        private static MapDicByJsTypePath;
        private constructor();
        JsType: Function;
        JsTypePath: string;
        NativeTypePath: string;
        static GetMapInfoByNativeTypePath(nativeTypePath: string): JsMapInfo;
        static GetMapInfoByJsTypePath(jsTypePath: string): JsMapInfo;
        static GetMapInfoByJsObject(jsObject: object): JsMapInfo;
        static AddMapInfo(jsTypePath: string, nativeTypePath: string, jsType: Function): void;
    }
    function ToMap(jsTypePath: string, nativeTypePath: string): (target: Function) => void;
    function ConstructorInfo(...args: string[]): (target: Function) => void;
    abstract class NativeObject {
        TneMapNativeObjectId: number;
        private TneMapGcObject_;
        private args_;
        constructor(...arg: any[]);
        TneMapGenericInfo: string;
        Ready(): Promise<this>;
        FreeTneMapNativeObject(): void;
        static Build(mapInfo: JsMapInfo): Promise<void>;
        static TypeList: Array<string>;
    }
    var ThisForm: TneForm;
}
declare namespace Tnelab {
    class TneForm extends Tnelab.NativeObject {
        constructor(...args: any);
        Width: any;
        Height: any;
        Show(): void;
        ShowDialog(): void;
        Close(): void;
        Url: string;
    }
}
