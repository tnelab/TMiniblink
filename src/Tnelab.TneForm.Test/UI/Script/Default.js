var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
///<reference path="../../../Tnelab.TneForm.Test.BLL/Service.ts"/>
function InvokeTest() {
    return __awaiter(this, void 0, void 0, function* () {
        let testService = yield new BLL.TestService(1, "System.Action<string,int>,int", function (args0, args1, args2, arg3, arg4, args5, args6, args7) { }).Ready();
        alert("OK");
        //await testService.FreeTneMapNativeObject();
        //testService = await new BLL.TestService<Function, boolean>(2, "System.Action<string,int>,bool").Ready();
        //await testService.Method5<string, Number, Number>(1, "string,int,int", function (msg) { return true; }, function () { }, 1, 2);
        //await testService.CallbackActionTest(function (msg) { });
        //await (testService.ActionProp = function (msg: string): void { });
        //let r = await testService.ActionProp;
        //await testService.FreeTneMapNativeObject();
        try {
        }
        catch (error) {
            alert(error);
        }
    });
}
class baseClass {
    constructor(args) {
    }
}
class testClass extends baseClass {
    m1(id) {
        return "";
    }
    constructor() { super(arguments); }
}
var t = new testClass();
