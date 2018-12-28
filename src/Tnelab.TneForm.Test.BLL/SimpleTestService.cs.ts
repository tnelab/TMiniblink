//此代码由机器生成，请不要手动修改
///<reference path="../Tnelab.TneForm/TneApp.d.ts"/>
namespace BLL{

	@Tnelab.ToMap("BLL.SimpleTestService","Tnelab.TneForm.Test.BLL.SimpleTestService")
	export class SimpleTestService extends Tnelab.NativeObject {
		@Tnelab.InvokeInfo(undefined,"System.String")
		public set Name(value:string) { }
		public get Name():string { return undefined; }
		@Tnelab.InvokeInfo(undefined,"System.Int32")
		public set Age(value:number) { }
		public get Age():number { return undefined; }
		@Tnelab.InvokeInfo("Add", "System.Int32","System.Int32")
		public Add(x:number,y:number):number {return undefined;}
		@Tnelab.InvokeInfo("GetMessage", "System.String")
		public GetMessage(msg:string):string {return undefined;}
		public Equals(tneMapId:number,obj:any):boolean;
		public Equals(tneMapId:number,objA:any,objB:any):boolean;
		@Tnelab.InvokeInfo("Equals", "System.Object","System.Object")
		@Tnelab.InvokeInfo("Equals", "System.Object")
		public Equals(tneMapId:number):any{}
		@Tnelab.InvokeInfo("GetHashCode")
		public GetHashCode():number {return undefined;}
		@Tnelab.InvokeInfo("GetType")
		public GetType():any {return undefined;}
		@Tnelab.InvokeInfo("ReferenceEquals", "System.Object","System.Object")
		public ReferenceEquals(objA:any,objB:any):boolean {return undefined;}
		@Tnelab.InvokeInfo("ToString")
		public ToString():string {return undefined;}
	}
}
