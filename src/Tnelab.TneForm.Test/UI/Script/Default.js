var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
///<reference path="../../../Tnelab.TneForm.Test.BLL/SimpleTestService.cs.ts"/>
function RunTestForFrom(func) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            func();
        }
        catch (error) {
            alert("测试失败:" + error);
        }
    });
}
/////////////////////////////////////////////////////////窗口测试///////////////////////////////////////////////
//居中于父窗口
let form;
function CreateSubFormTest1() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                form = yield new TMiniblink.TneForm("tne://Tnelab.TneForm.Test/UI/Test/FormTest/SubForm.html").Ready();
                yield (form.Width = 800);
                yield (form.Height = 600);
                yield (form.StartPosition = TMiniblink.StartPosition.CenterParent);
                yield (form.Parent = ThisForm);
                yield form.Show();
            });
        });
    });
}
function CreateSubFormTest2() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                form = yield new TMiniblink.TneForm("tne://Tnelab.TneForm.Test/UI/Test/FormTest/SubForm.html").Ready();
                yield (form.Width = 800);
                yield (form.Height = 600);
                yield (form.StartPosition = TMiniblink.StartPosition.CenterScreen);
                yield (form.Parent = ThisForm);
                yield form.Show();
            });
        });
    });
}
function CreateSubFormTest3() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                form = yield new TMiniblink.TneForm("tne://Tnelab.TneForm.Test/UI/Test/FormTest/SubForm.html").Ready();
                yield (form.Width = 800);
                yield (form.Height = 600);
                yield (form.StartPosition = TMiniblink.StartPosition.Manual);
                yield (form.X = 100);
                yield (form.Y = 100);
                yield (form.Parent = ThisForm);
                yield form.Show();
            });
        });
    });
}
function CreateSubFormTest4() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                form = yield new TMiniblink.TneForm("tne://Tnelab.TneForm.Test/UI/Test/FormTest/SubDialog.html").Ready();
                yield (form.Width = 800);
                yield (form.Height = 600);
                yield (form.StartPosition = TMiniblink.StartPosition.CenterParent);
                yield (form.Parent = ThisForm);
                yield form.ShowDialog();
            });
        });
    });
}
function CreateSubFormTest5() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                form = yield new TMiniblink.TneForm("file:///./UI/Test/FormTest/SubFormForFile.html").Ready();
                yield (form.Width = 800);
                yield (form.Height = 600);
                yield (form.StartPosition = TMiniblink.StartPosition.CenterParent);
                yield (form.Parent = ThisForm);
                yield form.Show();
            });
        });
    });
}
function RunInSubFormTest1() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                if (form == undefined) {
                    alert("请先创建子窗口");
                    return;
                }
                yield (form.WindowState = TMiniblink.WindowState.Minimized);
            });
        });
    });
}
function RunInSubFormTest2() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                if (form == undefined) {
                    alert("请先创建子窗口");
                    return;
                }
                yield form.Active();
                let result = yield form.RunFunc(function (msg) {
                    return __awaiter(this, void 0, void 0, function* () {
                        yield ThisForm.Active();
                        alert(msg);
                        return "这是从子窗口返回的消息";
                    });
                }, "这是从父窗口传送过来的消息");
                alert(result);
            });
        });
    });
}
