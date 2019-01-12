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
        let state = old === TMiniblink.WindowState.Maximized ? TMiniblink.WindowState.Normal : TMiniblink.WindowState.Maximized;
        yield (ThisForm.WindowState = state);
        if (state === TMiniblink.WindowState.Maximized) {
            elm.innerText = "恢复";
        }
        else if (state === TMiniblink.WindowState.Normal) {
            elm.innerText = "最大化";
        }
    });
}
function MiniWindow() {
    return __awaiter(this, void 0, void 0, function* () {
        yield (ThisForm.WindowState = TMiniblink.WindowState.Minimized);
    });
}
function MoveWindow() {
    return __awaiter(this, void 0, void 0, function* () {
        var maxButton = document.getElementById("MaxButton");
        maxButton.innerText = "最大化";
        ThisForm.Move();
    });
}
function OnContextMenu(elm, evt) {
    return __awaiter(this, void 0, void 0, function* () {
        //let menuForm = await new TMiniblink.TneForm("Tne://Tnelab.TneForm.Test/ui/TestContextMenu.html").Ready();
        //await (menuForm.ShowInTaskBar = false);
        //await (menuForm.StartPosition = 1);
        //await (menuForm.SizeAble = false);
        //await (menuForm.Width = 180);
        //await (menuForm.Height = 100);
        //let x = await ThisForm.X;
        //x += e.x;
        //let y = await ThisForm.Y;
        //y += e.y;
        //await (menuForm.X = x);
        //await (menuForm.Y = y);
        //await menuForm.ShowDialog();
        yield ThisForm.ShowContextMenu(elm, evt, 180, 100, "Tne://Tnelab.TneForm.Test/ui/TestContextMenu.html");
    });
}
