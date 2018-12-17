///<reference path="../../../Tnelab.TneForm/TneForm.d.ts"/>
///<reference path="../../../Tnelab.TneForm.Test.BLL/Service.ts"/>
async function test() {
    let t = Tnelab.ThisForm;
    await (t.Width = 200);
}
async function InvokeTest() {
    let testService = await new BLL.TestService(1, "System.Action<string,int>,int", function (args0, args1, args2, arg3, arg4, args5, args6, args7) { }).Ready();
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
