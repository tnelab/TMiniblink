///<reference path="../../../Tnelab.TneForm.Test.BLL/SimpleTestService.cs.ts"/>
let newForm: TMiniblink.TneForm;
let eventTest = function (sender, args) {
    alert(args.Files.join(",") + ":X:" + args.X + ":Y:" + args.Y);
}
async function InvokeTest() {
    try {
        //newForm = await new TMiniblink.TneForm("Tne://Tnelab.TneForm.Test/ui/default.html?cmd=测试").Ready();
        //await newForm.Show();
        await showNotifyIcon();
        alert("HI");
    }
    catch (error) {
        console.log(error);
    }
}
let notifyicon: TMiniblink.NotifyIcon;
async function showNotifyIcon(){
    let hwnd = await ThisForm.Handle;
    notifyicon = await new TMiniblink.NotifyIcon(hwnd, "default.png").Ready();
    await(notifyicon.Tip = "你好世界");
    await(notifyicon.Info = "程序已经隐藏到托盘。");
    let clickEvent = await notifyicon.Click;
    await clickEvent.AddListener(async function (sender, args) {
        let form = await sender as TMiniblink.TneForm;
        await form.Active();
    });
    let contextMenuEvent = await notifyicon.ContextMenu;
    await contextMenuEvent.AddListener(async function (sender, args) {
        let form = await sender as TMiniblink.TneForm;
        await (form.ShowContextMenu(undefined, undefined, 180, 100, "Tne://Tnelab.TneForm.Test/ui/TestContextMenu.html"));
    });
    await notifyicon.Show();
}
async function callOtherForm() {
    await (notifyicon.Info="你好啊");
    alert("HI");
}
async function showId() {
    alert(ThisForm.TneMapNativeObjectId);
}
