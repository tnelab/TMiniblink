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
                        alert(msg);
                        return "这是从子窗口返回的消息";
                    });
                }, "这是从父窗口传送过来的消息");
                alert(result);
            });
        });
    });
}
function RunInSubFormTest3() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                if (form == undefined) {
                    alert("请先创建子窗口");
                    return;
                }
                yield form.RunFunc(function () {
                    return __awaiter(this, void 0, void 0, function* () {
                        alert("这是一个来自父窗口的简单呼叫");
                    });
                });
            });
        });
    });
}
function RunInSubFormTest4() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                if (form == undefined) {
                    alert("请先创建子窗口");
                    return;
                }
                yield form.RunFunc(function () {
                    return __awaiter(this, void 0, void 0, function* () {
                        yield ThisForm.Hide();
                    });
                });
            });
        });
    });
}
function RunInSubFormTest5() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                if (form == undefined) {
                    alert("请先创建子窗口");
                    return;
                }
                yield form.RunFunc(function () {
                    return __awaiter(this, void 0, void 0, function* () {
                        yield ThisForm.Show();
                    });
                });
            });
        });
    });
}
function RunInParentFormTest1() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                let parent = yield ThisForm.Parent;
                yield (parent.WindowState = TMiniblink.WindowState.Minimized);
            });
        });
    });
}
function RunInParentFormTest2() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                let parent = yield ThisForm.Parent;
                yield parent.Active();
            });
        });
    });
}
function RunInParentFormTest3() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                let parent = yield ThisForm.Parent;
                let result = yield parent.RunFunc(function (msg) {
                    return __awaiter(this, void 0, void 0, function* () {
                        alert(msg);
                        return "这是从父窗口返回的消息";
                    });
                }, "这是从子窗口传送过来的消息");
                alert(result);
            });
        });
    });
}
function SetWindow1() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                yield (ThisForm.TopMost = true);
                alert("窗口已置顶");
            });
        });
    });
}
function SetWindow2() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                yield (ThisForm.MinWidth = 400);
                yield (ThisForm.MinHeight = 300);
                alert("最小宽高调整为：400*300");
            });
        });
    });
}
function SetWindow3() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                yield (ThisForm.SizeAble = false);
                alert("窗口已不能调整大小");
            });
        });
    });
}
function SetWindow4() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                yield (ThisForm.ShowInTaskBar = false);
                alert("窗口已不显示在任务栏");
            });
        });
    });
}
function SetWindow5() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                yield (ThisForm.ShowInTaskBar = true);
                alert("窗口已显示在任务栏");
            });
        });
    });
}
function SetWindow6() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                yield (ThisForm.Icon = "tb.png");
                let iconElm = document.getElementsByClassName("app_icon")[0];
                iconElm.src = "tne://Tnelab.TneForm.Test/tb.png";
                alert("图标已修改");
            });
        });
    });
}
function SetWindow7() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                yield (ThisForm.AllowDrop = true);
                let event = yield ThisForm.DragFilesEvent;
                event.AddListener((sender, args) => {
                    alert(args.Files.join("|"));
                });
                alert("已开启文件拖放");
            });
        });
    });
}
function SetWindow8() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                let clientElm = document.getElementsByClassName("window_client")[0];
                clientElm.oncontextmenu = function (evt) {
                    return __awaiter(this, void 0, void 0, function* () {
                        yield ThisForm.ShowContextMenu(clientElm, evt, 200, 100, "tne://tnelab.tneform.test/ui/test/contextmenutest/TestContextMenu.html");
                    });
                };
                alert("已开启右键菜单");
            });
        });
    });
}
function SetWindow9() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                let bfd = yield new TMiniblink.BrowseFolderDialog().Ready();
                yield (bfd.OwnerHandle = yield ThisForm.Handle);
                yield (bfd.Title = "测试-目录选择");
                let result = yield bfd.ShowDialog();
                alert(result);
            });
        });
    });
}
function SetWindow10() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                let bfd = yield new TMiniblink.OpenFileDialog().Ready();
                yield (bfd.OwnerHandle = yield ThisForm.Handle);
                yield (bfd.Filter = "文本文件(*.txt)|程序文件(*.exe)");
                yield (bfd.AllowMultiSelect = true);
                yield (bfd.Title = "测试-请选择文件-可多选");
                let result = yield bfd.ShowDialog();
                alert(result.join("|"));
            });
        });
    });
}
function SetWindow11() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                let bfd = yield new TMiniblink.SaveFileDialog().Ready();
                yield (bfd.OwnerHandle = yield ThisForm.Handle);
                yield (bfd.Filter = "文本文件(*.txt)|程序文件(*.exe)");
                yield (bfd.File = "test.txt");
                yield (bfd.Title = "测试-请选择文件-可多选");
                let result = yield bfd.ShowDialog();
                alert(result);
            });
        });
    });
}
let ntf;
function SetWindow12() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                ntf = yield new TMiniblink.NotifyIcon(yield ThisForm.Handle, "default.png").Ready();
                (yield (ntf.Click)).AddListener((sender, args) => {
                    alert("这是托盘图标");
                });
                (yield (ntf.ContextMenu)).AddListener((sender, args) => __awaiter(this, void 0, void 0, function* () {
                    yield ThisForm.ShowContextMenu(undefined, undefined, 200, 100, "tne://tnelab.tneform.test/ui/test/contextmenutest/TestContextMenu.html");
                }));
                yield (ntf.Tip = "托盘图标已显示");
                yield (ntf.Info = "这是托盘消息");
                yield ntf.Show();
            });
        });
    });
}
function SetWindow13() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                if (ntf === undefined) {
                    alert("请先显示托盘图标");
                    return;
                }
                yield (ntf.Info = "托盘图标已隐藏");
                yield ntf.Hide();
            });
        });
    });
}
function SetWindow14() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                if (ntf === undefined) {
                    alert("请先显示托盘图标");
                    return;
                }
                yield (ntf.Info = "这是托盘测试消息");
            });
        });
    });
}
function SetWindow15() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                if (ntf === undefined) {
                    alert("请先显示托盘图标");
                    return;
                }
                yield (ntf.Icon = "tb.png");
                yield (ntf.Info = "这是托盘图标已修改");
            });
        });
    });
}
function SetWindow16() {
    return __awaiter(this, void 0, void 0, function* () {
        yield RunTestForFrom(function () {
            return __awaiter(this, void 0, void 0, function* () {
                if (ntf === undefined) {
                    alert("请先显示托盘图标");
                    return;
                }
                yield (ntf.Info = "托盘图标已显示");
                yield ntf.Show();
            });
        });
    });
}
