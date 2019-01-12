//此代码由机器生成，请不要手动修改
///<reference path="./TneMap.ts"/>
namespace TMiniblink{
	@Tnelab.ConstructorInfo("System.IntPtr","System.String")
	@Tnelab.ToMap("TMiniblink.NotifyIcon","Tnelab.HtmlView.NotifyIcon")
	export class NotifyIcon extends Tnelab.NativeObject {
		@Tnelab.IsEvent("System.EventHandler<System.EventArgs>")
		public  get Click():Tnelab.TneEvent{ return undefined; }
		@Tnelab.IsEvent("System.EventHandler<System.EventArgs>")
		public  get ContextMenu():Tnelab.TneEvent{ return undefined; }
		@Tnelab.InvokeInfo(undefined,"System.String")
		public  set Tip(value:string) { }
		public  get Tip():string { return undefined; }
		@Tnelab.InvokeInfo(undefined,"System.String")
		public  set Info(value:string) { }
		public  get Info():string { return undefined; }
		@Tnelab.InvokeInfo(undefined,"System.String")
		public  set Icon(value:string) { }
		public  get Icon():string { return undefined; }
		@Tnelab.InvokeInfo("Show")
		public  Show():void {}
		@Tnelab.InvokeInfo("Hide")
		public  Hide():void {}
		@Tnelab.InvokeInfo("Equals", "System.Object")
		public  Equals(_obj:any):boolean {return undefined;}
		@Tnelab.InvokeInfo("Equals_", "System.Object","System.Object")
		public static Equals_(_objA:any,_objB:any):boolean {return undefined;}
		@Tnelab.InvokeInfo("GetHashCode")
		public  GetHashCode():number {return undefined;}
		@Tnelab.InvokeInfo("GetType")
		public  GetType():any {return undefined;}
		@Tnelab.InvokeInfo("ReferenceEquals", "System.Object","System.Object")
		public static ReferenceEquals(_objA:any,_objB:any):boolean {return undefined;}
		@Tnelab.InvokeInfo("ToString")
		public  ToString():string {return undefined;}
		public constructor(_hwnd:any,_icon:string) {super(arguments);}
	}
	Tnelab.RegisterNativeMapAsync("Tnelab.HtmlView.NotifyIcon","TMiniblink.NotifyIcon");
}
