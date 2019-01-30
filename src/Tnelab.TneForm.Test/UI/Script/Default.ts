///<reference path="../../../Tnelab.TneForm.Test.BLL/SimpleTestService.cs.ts"/>
async function RunTestForFrom(func: () => void) {
    try {
        func();
    }
    catch (error) {
        alert("测试失败:" + error);
    }
}
/////////////////////////////////////////////////////////窗口测试///////////////////////////////////////////////
//居中于父窗口
let form: TMiniblink.TneForm;
async function CreateSubFormTest1() {
    await RunTestForFrom(async function () {        
        form = await new TMiniblink.TneForm("tne://Tnelab.TneForm.Test/UI/Test/FormTest/SubForm.html").Ready();
        await (form.Width = 800);
        await (form.Height = 600);
        await (form.StartPosition = TMiniblink.StartPosition.CenterParent);
        await (form.Parent = ThisForm);
        await form.Show();
    });
}
async function CreateSubFormTest2() {
    await RunTestForFrom(async function () {
        form = await new TMiniblink.TneForm("tne://Tnelab.TneForm.Test/UI/Test/FormTest/SubForm.html").Ready();
        await (form.Width = 800);
        await (form.Height = 600);
        await (form.StartPosition = TMiniblink.StartPosition.CenterScreen);
        await (form.Parent = ThisForm);
        await form.Show();
    });
}
async function CreateSubFormTest3() {
    await RunTestForFrom(async function () {
        form = await new TMiniblink.TneForm("tne://Tnelab.TneForm.Test/UI/Test/FormTest/SubForm.html").Ready();
        await (form.Width = 800);
        await (form.Height = 600);
        await (form.StartPosition = TMiniblink.StartPosition.Manual);
        await (form.X = 100);
        await (form.Y = 100);
        await (form.Parent = ThisForm);
        await form.Show();
    });
}
async function CreateSubFormTest4() {
    await RunTestForFrom(async function () {
        form = await new TMiniblink.TneForm("tne://Tnelab.TneForm.Test/UI/Test/FormTest/SubDialog.html").Ready();
        await (form.Width = 800);
        await (form.Height = 600);
        await (form.StartPosition = TMiniblink.StartPosition.CenterParent);
        await (form.Parent = ThisForm);
        await form.ShowDialog();
    });
}
async function CreateSubFormTest5() {
    await RunTestForFrom(async function () {
        form = await new TMiniblink.TneForm("file:///./UI/Test/FormTest/SubFormForFile.html").Ready();
        await (form.Width = 800);
        await (form.Height = 600);
        await (form.StartPosition = TMiniblink.StartPosition.CenterParent);
        await (form.Parent = ThisForm);
        await form.Show();
    });
}
async function RunInSubFormTest1() {
    await RunTestForFrom(async function () {
        if (form == undefined) {
            alert("请先创建子窗口");
            return;
        }
        await (form.WindowState=TMiniblink.WindowState.Minimized);
    });
}
async function RunInSubFormTest2() {
    await RunTestForFrom(async function () {
        if (form == undefined) {
            alert("请先创建子窗口");
            return;
        }
        await form.Active();
        let result=await form.RunFunc(async function (msg) {
            alert(msg);
            return "这是从子窗口返回的消息";
        }, "这是从父窗口传送过来的消息");
        alert(result);
    });
}
async function RunInSubFormTest3() {
    await RunTestForFrom(async function () {
        if (form == undefined) {
            alert("请先创建子窗口");
            return;
        }
        await form.RunFunc(async function () {
            alert("这是一个来自父窗口的简单呼叫");
        });
    });
}
async function RunInSubFormTest4() {
    await RunTestForFrom(async function () {
        if (form == undefined) {
            alert("请先创建子窗口");
            return;
        }
        await form.RunFunc(async function () {
            await ThisForm.Hide();
        });
    });
}
async function RunInSubFormTest5() {
    await RunTestForFrom(async function () {
        if (form == undefined) {
            alert("请先创建子窗口");
            return;
        }
        await form.RunFunc(async function () {
            await ThisForm.Show();
        });
    });
}
async function RunInParentFormTest1() {
    await RunTestForFrom(async function () {
        let parent = await ThisForm.Parent;
        await (parent.WindowState = TMiniblink.WindowState.Minimized);
    });
}
async function RunInParentFormTest2() {
    await RunTestForFrom(async function () {
        let parent: TMiniblink.TneForm = await ThisForm.Parent;
        await parent.Active();
    });
}
async function RunInParentFormTest3() {
    await RunTestForFrom(async function () {
        let parent: TMiniblink.TneForm = await ThisForm.Parent;
        let result = await parent.RunFunc(async function (msg) {
            alert(msg);
            return "这是从父窗口返回的消息";
        }, "这是从子窗口传送过来的消息");
        alert(result);
    });
}
async function SetWindow1() {
    await RunTestForFrom(async function () {
        await (ThisForm.TopMost = true);
        alert("窗口已置顶");
    });
}
async function SetWindow2() {
    await RunTestForFrom(async function () {
        await (ThisForm.MinWidth = 400);
        await (ThisForm.MinHeight = 300);
        alert("最小宽高调整为：400*300");
    });
}
async function SetWindow3() {
    await RunTestForFrom(async function () {
        await (ThisForm.SizeAble = false);
        alert("窗口已不能调整大小");
    });
}
async function SetWindow4() {
    await RunTestForFrom(async function () {
        await (ThisForm.ShowInTaskBar = false);
        alert("窗口已不显示在任务栏");
    });
}
async function SetWindow5() {
    await RunTestForFrom(async function () {
        await (ThisForm.ShowInTaskBar = true);
        alert("窗口已显示在任务栏");
    });
}
async function SetWindow6() {
    await RunTestForFrom(async function () {
        await (ThisForm.Icon = "tb.png");
        let iconElm = document.getElementsByClassName("app_icon")[0] as any;
        iconElm.src = "tne://Tnelab.TneForm.Test/tb.png";
        alert("图标已修改");
    });
}
async function SetWindow7() {
    await RunTestForFrom(async function () {
        await (ThisForm.AllowDrop = true);
        let event = await ThisForm.DragFilesEvent;
        event.AddListener((sender, args) => {
            alert(args.Files.join("|"));
        });
        alert("已开启文件拖放");
    });
}
async function SetWindow8() {
    await RunTestForFrom(async function () {
        let clientElm = document.getElementsByClassName("window_client")[0] as any;
        clientElm.oncontextmenu =async function (evt) {
            await ThisForm.ShowContextMenu(clientElm, evt, 200, 100, "tne://tnelab.tneform.test/ui/test/contextmenutest/TestContextMenu.html");
        };
        alert("已开启右键菜单");
    });
}
async function SetWindow9() {
    await RunTestForFrom(async function () {
        let bfd = await new TMiniblink.BrowseFolderDialog().Ready();
        await (bfd.OwnerHandle = await ThisForm.Handle);
        await (bfd.Title = "测试-目录选择");
        let result = await bfd.ShowDialog();
        alert(result);
    });
}
async function SetWindow10() {
    await RunTestForFrom(async function () {
        let bfd = await new TMiniblink.OpenFileDialog().Ready();
        await (bfd.OwnerHandle = await ThisForm.Handle);
        await (bfd.Filter = "文本文件(*.txt)|程序文件(*.exe)");
        await (bfd.AllowMultiSelect = true);
        await (bfd.Title = "测试-请选择文件-可多选");
        let result = await bfd.ShowDialog();
        alert(result.join("|"));
    });
}
async function SetWindow11() {
    await RunTestForFrom(async function () {
        let bfd = await new TMiniblink.SaveFileDialog().Ready();
        await (bfd.OwnerHandle = await ThisForm.Handle);
        await (bfd.Filter = "文本文件(*.txt)|程序文件(*.exe)");
        await(bfd.File = "test.txt");
        await (bfd.Title = "测试-请选择文件-可多选");
        let result = await bfd.ShowDialog();
        alert(result);
    });
}
let ntf: TMiniblink.NotifyIcon;
async function SetWindow12() {
    await RunTestForFrom(async function () {
        ntf = await new TMiniblink.NotifyIcon(await ThisForm.Handle,"default.png").Ready();
        (await (ntf.Click)).AddListener((sender, args) => {
            alert("这是托盘图标");
        });
        (await (ntf.ContextMenu)).AddListener(async (sender, args) => {
            await ThisForm.ShowContextMenu(undefined, undefined, 200, 100, "tne://tnelab.tneform.test/ui/test/contextmenutest/TestContextMenu.html");
        });
        await (ntf.Tip = "托盘图标已显示");
        await (ntf.Info = "这是托盘消息");
        await ntf.Show();
    });
}
async function SetWindow13() {
    await RunTestForFrom(async function () {
        if (ntf === undefined) {
            alert("请先显示托盘图标");
            return;
        }
        await (ntf.Info = "托盘图标已隐藏");
        await ntf.Hide();        
    });
}
async function SetWindow14() {
    await RunTestForFrom(async function () {
        if (ntf === undefined) {
            alert("请先显示托盘图标");
            return;
        }
        await (ntf.Info = "这是托盘测试消息");
    });
}
async function SetWindow15() {
    await RunTestForFrom(async function () {
        if (ntf === undefined) {
            alert("请先显示托盘图标");
            return;
        }
        await (ntf.Icon = "tb.png");
        await (ntf.Info = "这是托盘图标已修改");
    });
}
async function SetWindow16() {
    await RunTestForFrom(async function () {
        if (ntf === undefined) {
            alert("请先显示托盘图标");
            return;
        }
        await (ntf.Info = "托盘图标已显示");
        await ntf.Show();
    });
}