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
            await ThisForm.Active();
            alert(msg);
            return "这是从子窗口返回的消息";
        }, "这是从父窗口传送过来的消息");
        alert(result);
    });
}