///<reference path="./TneMap.ts"/>
namespace Tnelab {
    /////////////////////////////////////////////////////////////////////////////////TneForm
    //本机窗口类，用于控制窗口的姿态
    @Tnelab.ToMap("Tnelab.TneForm", "Tnelab.HtmlView.TneForm")
    export class TneForm extends Tnelab.NativeObject {
        public constructor(...args: any) {
            super(args);
        }
        @Tnelab.InvokeInfo("Width", "System.Int32")
        public set Width(value) { }
        public get Width() { return undefined; }
        @Tnelab.InvokeInfo("Height", "System.Int32")
        public set Height(value) { }
        public get Height() { return undefined; }
        public Show(): void { }
        public ShowDialog() { }
        public Close() { }
        public set Url(url: string) { }
        public get Url() { return undefined; }
    }
}