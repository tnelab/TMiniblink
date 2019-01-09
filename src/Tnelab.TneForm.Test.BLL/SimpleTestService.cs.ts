//此代码由机器生成，请不要手动修改
///<reference path="../Tnelab.TneForm/TneApp.d.ts"/>
namespace BLL{

	@Tnelab.ToMap("BLL.SimpleTestService","Tnelab.TneForm.Test.BLL.SimpleTestService")
	export class SimpleTestService extends Tnelab.NativeObject {
		@Tnelab.InvokeInfo(undefined,"System.String")
		public  set Name(value:string) { }
		public  get Name():string { return undefined; }
		@Tnelab.InvokeInfo(undefined,"System.Int32")
		public  set Age(value:number) { }
		public  get Age():number { return undefined; }
		@Tnelab.InvokeInfo("Add", "System.Int32","System.Int32")
		public  Add(_x:number,_y:number):number {return undefined;}
		@Tnelab.InvokeInfo("GetMessage", "System.String")
		public static GetMessage(_msg:string):string {return undefined;}
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
		public constructor() {super(arguments);}
	}
	Tnelab.RegisterNativeMapAsync("Tnelab.TneForm.Test.BLL.SimpleTestService","BLL.SimpleTestService");
}
