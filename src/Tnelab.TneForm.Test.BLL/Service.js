var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
//此代码由机器生成，请不要手动修改
///<reference path="../Tnelab.TneForm/TneApp.d.ts"/>
var BLL;
(function (BLL) {
    let SimpleTestService = class SimpleTestService extends Tnelab.NativeObject {
        set Name(value) { }
        get Name() { return undefined; }
        set Age(value) { }
        get Age() { return undefined; }
        Add(x, y) { return undefined; }
        GetMessage(msg) { return undefined; }
        Equals(tneMapId) { }
        GetHashCode() { return undefined; }
        GetType() { return undefined; }
        ReferenceEquals(objA, objB) { return undefined; }
        ToString() { return undefined; }
    };
    __decorate([
        Tnelab.InvokeInfo(undefined, "System.String")
    ], SimpleTestService.prototype, "Name", null);
    __decorate([
        Tnelab.InvokeInfo(undefined, "System.Int32")
    ], SimpleTestService.prototype, "Age", null);
    __decorate([
        Tnelab.InvokeInfo("Add", "System.Int32", "System.Int32")
    ], SimpleTestService.prototype, "Add", null);
    __decorate([
        Tnelab.InvokeInfo("GetMessage", "System.String")
    ], SimpleTestService.prototype, "GetMessage", null);
    __decorate([
        Tnelab.InvokeInfo("Equals", "System.Object", "System.Object"),
        Tnelab.InvokeInfo("Equals", "System.Object")
    ], SimpleTestService.prototype, "Equals", null);
    __decorate([
        Tnelab.InvokeInfo("GetHashCode")
    ], SimpleTestService.prototype, "GetHashCode", null);
    __decorate([
        Tnelab.InvokeInfo("GetType")
    ], SimpleTestService.prototype, "GetType", null);
    __decorate([
        Tnelab.InvokeInfo("ReferenceEquals", "System.Object", "System.Object")
    ], SimpleTestService.prototype, "ReferenceEquals", null);
    __decorate([
        Tnelab.InvokeInfo("ToString")
    ], SimpleTestService.prototype, "ToString", null);
    SimpleTestService = __decorate([
        Tnelab.ToMap("BLL.SimpleTestService", "Tnelab.TneForm.Test.BLL.SimpleTestService")
    ], SimpleTestService);
    BLL.SimpleTestService = SimpleTestService;
})(BLL || (BLL = {}));
//# sourceMappingURL=Service.js.map