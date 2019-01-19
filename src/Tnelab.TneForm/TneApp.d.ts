declare let ThisForm: TMiniblink.TneForm;
declare namespace Tnelab {
    function RegisterNativeMapAsync(nativeTypeName: any, jsTypeName: any): Promise<void>;
    function RunFunctionForTneForm(theTneForm: TneFormBase, json: string, func: (json: string) => Promise<string>): Promise<string>;
    function ShowContextMenuForTneForm(theTneForm: TneFormBase, elm: Element, evt: MouseEvent, width: number, height: number, url: string): Promise<void>;
    function InvokeInfo(nativeName: string, ...args: string[]): (target: any, propertyKey: string, descriptor: PropertyDescriptor) => void;
    function IsEvent(type: string): (target: any, propertyKey: string, descriptor: PropertyDescriptor) => void;
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
    class TneEvent {
        private nativeObjectId_;
        private name_;
        private type_;
        constructor(id: number, name: string, type: string);
        AddListener(handler: (sender: any, args: any) => void): Promise<void>;
        RemoveListener(handler: (sender: any, args: any) => void): Promise<void>;
    }
    abstract class NativeObject {
        TneMapNativeObjectId: number;
        private TneMapGcObject_;
        private args_;
        private eventMap_;
        constructor(args?: IArguments);
        TneMapGenericInfo: string;
        Ready(): Promise<this>;
        FreeTneMapNativeObject(): void;
        static Build(mapInfo: JsMapInfo): Promise<void>;
        static TypeList: Array<string>;
    }
}
declare namespace TMiniblink {
    class NotifyIcon extends Tnelab.NativeObject {
        readonly Click: Tnelab.TneEvent;
        readonly ContextMenu: Tnelab.TneEvent;
        Tip: string;
        Info: string;
        Icon: string;
        Show(): void;
        Hide(): void;
        Equals(_obj: any): boolean;
        static Equals_(_objA: any, _objB: any): boolean;
        GetHashCode(): number;
        GetType(): any;
        static ReferenceEquals(_objA: any, _objB: any): boolean;
        ToString(): string;
        constructor(_hwnd: any, _icon: string);
    }
}
declare namespace TMiniblink {
    class OpenFileDialog extends Tnelab.NativeObject {
        OwnerHandle: any;
        Filter: string;
        Title: string;
        AllowMultiSelect: boolean;
        ShowDialog(): any;
        Equals(_obj: any): boolean;
        static Equals_(_objA: any, _objB: any): boolean;
        GetHashCode(): number;
        GetType(): any;
        static ReferenceEquals(_objA: any, _objB: any): boolean;
        ToString(): string;
        constructor();
    }
}
declare namespace TMiniblink {
    enum StartPosition {
        Manual = 1,
        CenterScreen = 2,
        CenterParent = 3
    }
    enum WindowState {
        Maximized = 1,
        Normal = 2,
        Minimized = 3
    }
}
declare namespace Tnelab {
    class TneFormBase extends NativeObject {
        RunFunc(func: () => Promise<string>): any;
        RunFunc(func: (json: string) => Promise<string>, json: string): any;
        ShowContextMenu(elm: Element, evt: MouseEvent, width: number, height: number, url: string): Promise<void>;
    }
}
declare namespace TMiniblink {
    class TneForm extends Tnelab.TneFormBase {
        readonly DragFilesEvent: Tnelab.TneEvent;
        readonly Handle: any;
        readonly Title: string;
        SizeAble: boolean;
        X: number;
        Y: number;
        Width: number;
        Height: number;
        ShowInTaskBar: boolean;
        TopMost: boolean;
        MinWidth: number;
        MinHeight: number;
        readonly Url: string;
        StartPosition: any;
        WindowState: any;
        Parent: any;
        AllowDrop: boolean;
        Icon: string;
        Close(): void;
        ShowDialog(): void;
        Show(): void;
        Hide(): void;
        Move(): void;
        Active(): void;
        Equals(_obj: any): boolean;
        static Equals_(_objA: any, _objB: any): boolean;
        GetHashCode(): number;
        GetType(): any;
        static ReferenceEquals(_objA: any, _objB: any): boolean;
        ToString(): string;
        constructor(_url: string);
    }
}
declare namespace TMiniblink {
    class SaveFileDialog extends Tnelab.NativeObject {
        OwnerHandle: any;
        Filter: string;
        Title: string;
        File: string;
        ShowDialog(): string;
        Equals(_obj: any): boolean;
        static Equals_(_objA: any, _objB: any): boolean;
        GetHashCode(): number;
        GetType(): any;
        static ReferenceEquals(_objA: any, _objB: any): boolean;
        ToString(): string;
        constructor();
    }
}
