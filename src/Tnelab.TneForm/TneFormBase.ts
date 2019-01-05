///<reference path="./TneMap.ts"/>
namespace Tnelab {
    export class TneFormBase extends NativeObject{
        public async RunFunc(func: () => Promise<string>);
        public async RunFunc(func: (json: string) => Promise<string>, json: string);
        public async RunFunc(func: (json: string) => Promise<string>, json?: string): Promise<string> {
            return await RunFunctionForTneForm(this, json, func);
        }
        //public constructor(args?: IArguments) { super(arguments); }
    }
}