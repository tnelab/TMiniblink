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
let ofn;
function InvokeTest() {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            //newForm = await new TMiniblink.TneForm("Tne://Tnelab.TneForm.Test/ui/default.html?cmd=测试").Ready();
            //await newForm.Show();
            ofn = new TMiniblink.OpenFileDialog();
            ofn.Ready().then(function () {
                return __awaiter(this, void 0, void 0, function* () {
                    yield (ofn.Title = "请选择测试文件");
                    yield (ofn.Filter = "所有文件(*.*)|文本文件(*.txt)");
                    let handle = yield ThisForm.Handle;
                    yield (ofn.OwnerHandle = handle);
                    yield (ofn.AllowMultiSelect = true);
                    ofn.ShowDialog().then(function (files) {
                        alert(files.join(";"));
                    });
                });
            });
        }
        catch (error) {
            console.log(error);
        }
    });
}
let notifyicon;
function showNotifyIcon() {
    return __awaiter(this, void 0, void 0, function* () {
        let hwnd = yield ThisForm.Handle;
        notifyicon = yield new TMiniblink.NotifyIcon(hwnd, "default.png").Ready();
        yield (notifyicon.Tip = "你好世界");
        yield (notifyicon.Info = "程序已经隐藏到托盘。");
        let clickEvent = yield notifyicon.Click;
        yield clickEvent.AddListener(function (sender, args) {
            return __awaiter(this, void 0, void 0, function* () {
                let form = yield sender;
                yield form.Active();
            });
        });
        let contextMenuEvent = yield notifyicon.ContextMenu;
        yield contextMenuEvent.AddListener(function (sender, args) {
            return __awaiter(this, void 0, void 0, function* () {
                let form = yield sender;
                yield (form.ShowContextMenu(undefined, undefined, 180, 100, "Tne://Tnelab.TneForm.Test/ui/TestContextMenu.html"));
            });
        });
        yield notifyicon.Show();
    });
}
function callOtherForm() {
    return __awaiter(this, void 0, void 0, function* () {
        yield (notifyicon.Info = "你好啊");
        alert("HI");
    });
}
function showId() {
    return __awaiter(this, void 0, void 0, function* () {
        alert(ThisForm.TneMapNativeObjectId);
    });
}
