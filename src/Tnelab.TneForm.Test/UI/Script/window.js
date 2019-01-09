var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
///<reference path="../../../Tnelab.TneForm/TneApp.d.ts"/>
function MaxWindow(elm) {
    return __awaiter(this, void 0, void 0, function* () {
        let old = yield ThisForm.WindowState;
        let state = old === 1 ? 2 : 1;
        yield (ThisForm.WindowState = state);
        if (state === 1) {
            elm.innerText = "恢复";
        }
        else if (state === 2) {
            elm.innerText = "最大化";
        }
    });
}
function MiniWindow() {
    return __awaiter(this, void 0, void 0, function* () {
        yield (ThisForm.WindowState = 3);
    });
}
function MoveWindow() {
    return __awaiter(this, void 0, void 0, function* () {
        var maxButton = document.getElementById("MaxButton");
        maxButton.innerText = "最大化";
        ThisForm.Move();
    });
}
