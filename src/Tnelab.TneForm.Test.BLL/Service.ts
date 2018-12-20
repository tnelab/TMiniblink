

//导入T4工具库
//此代码由机器自动生成，请不要手动修改
///<reference path="../Tnelab.TneForm/TneApp.d.ts"/>
namespace BLL{
	@Tnelab.ConstructorInfo("T","T","T","System.Int32","System.String")
	@Tnelab.ConstructorInfo()
	@Tnelab.ConstructorInfo("System.Action<T, System.Action<T, System.Action<T, System.Action<T>, T>, System.Int32>, T, T>")
	@Tnelab.ToMap("BLL.TestService<T, T2>","Tnelab.TneForm.Test.BLL.TestService<T, T2>")
	export class TestService<T, T2> extends Tnelab.NativeObject {
		@Tnelab.InvokeInfo(undefined,"System.Action<System.String>")
		public set ActionProp(value:(arg0:string)=>void) { }
		public get ActionProp():(arg0:string)=>void { return undefined; }
		@Tnelab.InvokeInfo("CallbackActionTest", "System.Action<System.String>")
		public CallbackActionTest(action:(arg0:string)=>void):void {}
		@Tnelab.InvokeInfo("CallbackFuncTest", "System.Func<System.String, System.String>")
		public CallbackFuncTest(func:(arg0:string)=>string):string {return undefined;}
		@Tnelab.InvokeInfo("CallStaticMethod")
		public CallStaticMethod():void {}
		@Tnelab.InvokeInfo("GMethodTest<T1>", "T")
		public GMethodTest<T1>(tneMapGenericTypeInfo:string,t:T):T {return undefined;}
		@Tnelab.InvokeInfo("Method1", "System.String","System.String")
		public Method1(name:string,pwd:string):number {return undefined;}
		@Tnelab.InvokeInfo("Method2", "System.Action<System.String, System.Boolean, System.Int32, System.DateTime>")
		public Method2(action:(arg0:string,arg1:boolean,arg2:number,arg3:any)=>void):boolean {return undefined;}
		@Tnelab.InvokeInfo("Method3", "System.Func<System.String, System.Boolean, System.Int32, System.DateTime>")
		public Method3(func:(arg0:string,arg1:boolean,arg2:number)=>any):void {}
		@Tnelab.InvokeInfo("Method4", "System.Func<System.String, System.Boolean, System.Int32, System.DateTime, System.Int32>")
		public Method4(func:(arg0:string,arg1:boolean,arg2:number,arg3:any)=>number):void {}
		@Tnelab.InvokeInfo("Method51<T1, T2, T3>", "System.Func<T1, System.Boolean>","T","T2","T3")
		public Method51<T1, T2, T3>(tneMapGenericTypeInfo:string,func:(arg0:T1)=>boolean,t:T,t2:T2,t3:T3):T2 {return undefined;}
		public Method5<T1, T2, T3>(tneMapId:number,tneMapGenericTypeInfo:string,func:(arg0:T1)=>boolean,t:T,t2:T2,t3:T3):T2;
		public Method5<T1, T2>(tneMapId:number,tneMapGenericTypeInfo:string,func:(arg0:T1)=>boolean,t:T,t2:T2):T2;
		public Method5(tneMapId:number,func:(arg0:T)=>boolean,t:T):T;
		public Method5<T1>(tneMapId:number,tneMapGenericTypeInfo:string,func:(arg0:T1)=>boolean,t:T):T;
		@Tnelab.InvokeInfo("Method5<T1>", "System.Func<T1, System.Boolean>","T")
		@Tnelab.InvokeInfo("Method5", "System.Func<T, System.Boolean>","T")
		@Tnelab.InvokeInfo("Method5<T1, T2>", "System.Func<T1, System.Boolean>","T","T2")
		@Tnelab.InvokeInfo("Method5<T1, T2, T3>", "System.Func<T1, System.Boolean>","T","T2","T3")
		public Method5(tneMapId:number):any{}
		public constructor(tneMapId:number,tneMapGenericTypeInfo:string,action:(arg0:T,arg1:T,arg2:T,arg3:T,arg4:T,arg5:number,arg6:T,arg7:T)=>void);
		public constructor(tneMapId:number,tneMapGenericTypeInfo:string);
		public constructor(tneMapId:number,tneMapGenericTypeInfo:string,t:T,t2:T,t3:T,t4:number,t5:string);
		public constructor(...arg: any[]){super(...arg);}
	}
}
