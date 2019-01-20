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
let 文件选择对话框;
let 文件保存对话框;
let 目录选择对话框;
function InvokeTest() {
    return __awaiter(this, void 0, void 0, function* () {
        yield (ThisForm.AllowDrop = true);
        let event = yield ThisForm.DragFilesEvent;
        yield event.AddListener((sender, args) => {
            alert(args.Files[0]);
        });
    });
}
let notifyicon;
function SelectDir() {
    return __awaiter(this, void 0, void 0, function* () {
        目录选择对话框 = yield new TMiniblink.BrowseFolderDialog().Ready();
        yield (目录选择对话框.Title = "请选择游戏目录");
        let handle = yield ThisForm.Handle;
        yield (目录选择对话框.OwnerHandle = handle);
        let file = yield 目录选择对话框.ShowDialog();
        alert(file);
    });
}
function SaveFile() {
    return __awaiter(this, void 0, void 0, function* () {
        文件保存对话框 = yield new TMiniblink.SaveFileDialog().Ready();
        yield (文件保存对话框.Title = "保存测试文件");
        yield (文件保存对话框.Filter = "所有文件(*.*)|文本文件(*.txt)");
        let handle = yield ThisForm.Handle;
        yield (文件保存对话框.OwnerHandle = handle);
        yield (文件保存对话框.File = "t.txt");
        let file = yield 文件保存对话框.ShowDialog();
        alert(file);
    });
}
function OpenFile() {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            //newForm = await new TMiniblink.TneForm("Tne://Tnelab.TneForm.Test/ui/default.html?cmd=测试").Ready();
            //await newForm.Show();
            文件选择对话框 = new TMiniblink.OpenFileDialog();
            文件选择对话框.Ready().then(function () {
                return __awaiter(this, void 0, void 0, function* () {
                    yield (文件选择对话框.Title = "请选择测试文件");
                    yield (文件选择对话框.Filter = "所有文件(*.*)|文本文件(*.txt)");
                    let handle = yield ThisForm.Handle;
                    yield (文件选择对话框.OwnerHandle = handle);
                    yield (文件选择对话框.AllowMultiSelect = true);
                    文件选择对话框.ShowDialog().then(function (files) {
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
