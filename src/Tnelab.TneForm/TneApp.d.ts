declare namespace Tnelab {
    function OnSetGC(id: number, gc: object): void;
    function OnCallJs(args: any): any;
    function OnResponse(msgId: number, response: string): string;
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
        constructor(args?: IArguments);
        TneMapGenericInfo: string;
        Ready(): Promise<this>;
        FreeTneMapNativeObject(): void;
        static Build(mapInfo: JsMapInfo): Promise<void>;
        static TypeList: Array<string>;
    }
    let ThisForm: TneForm;
}
declare namespace Tnelab {
    class TneForm extends Tnelab.NativeObject {
        readonly Handle: any;
        readonly Title: string;
        SizeAble: boolean;
        X: number;
        Y: number;
        Width: number;
        Height: number;
        MinWidth: number;
        MinHeight: number;
        Url: string;
        StartPosition: any;
        WindowState: any;
        Parent: any;
        Icon: string;
        Close(): void;
        ShowDialog(): void;
        Show(): void;
        Hide(): void;
        Move(): void;
        Equals(tneMapId: number, obj: any): boolean;
        Equals(tneMapId: number, objA: any, objB: any): boolean;
        GetHashCode(): number;
        GetType(): any;
        ReferenceEquals(objA: any, objB: any): boolean;
        ToString(): string;
        constructor();
    }
}
