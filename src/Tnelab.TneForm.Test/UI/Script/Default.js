var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
///<reference path="../../../Tnelab.TneForm.Test.BLL/SimpleTestService.cs.ts"/>
let newForm;
function InvokeTest() {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            //var simple = await new BLL.SimpleTestService().Ready();
            newForm = yield new Tnelab.TneForm().Ready();
            yield (newForm.Url = "Tne://Tnelab.TneForm.Test/ui/default.html?cmd=测试");
            yield (newForm.Parent = Tnelab.ThisForm);
            yield newForm.ShowDialog();
            //var t = await newForm.RunFunc(function () {
            //    var win = window as any;
            //    win.MiniWindow();
            //    return "";
            //});
        }
        catch (error) {
            console.log(error);
        }
    });
}
function callOtherForm() {
    return __awaiter(this, void 0, void 0, function* () {
        let result = yield newForm.RunFunc(function () {
            return __awaiter(this, void 0, void 0, function* () {
                try {
                    yield MiniWindow();
                }
                catch (error) {
                    alert(error);
                }
                return undefined;
            });
        });
    });
}
function showId() {
    return __awaiter(this, void 0, void 0, function* () {
        alert(Tnelab.ThisForm.TneMapNativeObjectId);
    });
}
