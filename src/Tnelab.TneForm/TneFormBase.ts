namespace Tnelab {
    export class TneFormBase extends NativeObject{
        public GetHtmlWindow(): Window {
            alert("HI");
            return window;
        }
        //public constructor(args?: IArguments) { super(arguments); }
    }
}