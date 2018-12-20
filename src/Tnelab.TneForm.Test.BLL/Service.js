var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
//导入T4工具库
//此代码由机器自动生成，请不要手动修改
///<reference path="../Tnelab.TneForm/TneApp.d.ts"/>
var BLL;
(function (BLL) {
    let TestService = class TestService extends Tnelab.NativeObject {
        constructor(...arg) { super(...arg); }
        set ActionProp(value) { }
        get ActionProp() { return undefined; }
        CallbackActionTest(action) { }
        CallbackFuncTest(func) { return undefined; }
        CallStaticMethod() { }
        GMethodTest(tneMapGenericTypeInfo, t) { return undefined; }
        Method1(name, pwd) { return undefined; }
        Method2(action) { return undefined; }
        Method3(func) { }
        Method4(func) { }
        Method51(tneMapGenericTypeInfo, func, t, t2, t3) { return undefined; }
        Method5(tneMapId) { }
    };
    __decorate([
        Tnelab.InvokeInfo(undefined, "System.Action<System.String>")
    ], TestService.prototype, "ActionProp", null);
    __decorate([
        Tnelab.InvokeInfo("CallbackActionTest", "System.Action<System.String>")
    ], TestService.prototype, "CallbackActionTest", null);
    __decorate([
        Tnelab.InvokeInfo("CallbackFuncTest", "System.Func<System.String, System.String>")
    ], TestService.prototype, "CallbackFuncTest", null);
    __decorate([
        Tnelab.InvokeInfo("CallStaticMethod")
    ], TestService.prototype, "CallStaticMethod", null);
    __decorate([
        Tnelab.InvokeInfo("GMethodTest<T1>", "T")
    ], TestService.prototype, "GMethodTest", null);
    __decorate([
        Tnelab.InvokeInfo("Method1", "System.String", "System.String")
    ], TestService.prototype, "Method1", null);
    __decorate([
        Tnelab.InvokeInfo("Method2", "System.Action<System.String, System.Boolean, System.Int32, System.DateTime>")
    ], TestService.prototype, "Method2", null);
    __decorate([
        Tnelab.InvokeInfo("Method3", "System.Func<System.String, System.Boolean, System.Int32, System.DateTime>")
    ], TestService.prototype, "Method3", null);
    __decorate([
        Tnelab.InvokeInfo("Method4", "System.Func<System.String, System.Boolean, System.Int32, System.DateTime, System.Int32>")
    ], TestService.prototype, "Method4", null);
    __decorate([
        Tnelab.InvokeInfo("Method51<T1, T2, T3>", "System.Func<T1, System.Boolean>", "T", "T2", "T3")
    ], TestService.prototype, "Method51", null);
    __decorate([
        Tnelab.InvokeInfo("Method5<T1>", "System.Func<T1, System.Boolean>", "T"),
        Tnelab.InvokeInfo("Method5", "System.Func<T, System.Boolean>", "T"),
        Tnelab.InvokeInfo("Method5<T1, T2>", "System.Func<T1, System.Boolean>", "T", "T2"),
        Tnelab.InvokeInfo("Method5<T1, T2, T3>", "System.Func<T1, System.Boolean>", "T", "T2", "T3")
    ], TestService.prototype, "Method5", null);
    TestService = __decorate([
        Tnelab.ConstructorInfo("T", "T", "T", "System.Int32", "System.String"),
        Tnelab.ConstructorInfo(),
        Tnelab.ConstructorInfo("System.Action<T, System.Action<T, System.Action<T, System.Action<T>, T>, System.Int32>, T, T>"),
        Tnelab.ToMap("BLL.TestService<T, T2>", "Tnelab.TneForm.Test.BLL.TestService<T, T2>")
    ], TestService);
    BLL.TestService = TestService;
})(BLL || (BLL = {}));
