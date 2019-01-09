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
let eventTest = function (sender, args) {
    alert(args.Files.join(",") + ":X:" + args.X + ":Y:" + args.Y);
};
function InvokeTest() {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            newForm = yield new Tnelab.TneForm().Ready();
            yield (newForm.Url = "Tne://Tnelab.TneForm.Test/ui/default.html?cmd=测试");
            yield (newForm.TopMost = true);
            yield newForm.Show();
        }
        catch (error) {
            console.log(error);
        }
    });
}
function callOtherForm() {
    return __awaiter(this, void 0, void 0, function* () {
        let event = yield Tnelab.ThisForm.DragFilesEvent;
        yield event.RemoveListener(eventTest);
        alert("HI2");
    });
}
function showId() {
    return __awaiter(this, void 0, void 0, function* () {
        alert(Tnelab.ThisForm.TneMapNativeObjectId);
    });
}
