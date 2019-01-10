///<reference path="./TneMap.ts"/>
namespace Tnelab {
    export class TneFormBase extends NativeObject{
        public async RunFunc(func: () => Promise<string>);
        public async RunFunc(func: (json: string) => Promise<string>, json: string);
        public async RunFunc(func: (json: string) => Promise<string>, json?: string): Promise<string> {
            return await Tnelab.RunFunctionForTneForm(this, json, func);
        }
        public async ShowContextMenu(elm: Element, evt: MouseEvent, width: number, height: number, url: string) {
            return await Tnelab.ShowContextMenuForTneForm(this, elm, evt, width, height, url);
        }
    }
}