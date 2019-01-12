///<reference path="../../../Tnelab.TneForm/TneApp.d.ts"/>
async function MaxWindow(elm) {
    let old = await ThisForm.WindowState;
    let state = old === TMiniblink.WindowState.Maximized ? TMiniblink.WindowState.Normal : TMiniblink.WindowState.Maximized;
    await (ThisForm.WindowState = state);
    if (state === TMiniblink.WindowState.Maximized) {
        elm.innerText = "恢复";
    }
    else if (state === TMiniblink.WindowState.Normal) {
        elm.innerText = "最大化";
    }
}
async function MiniWindow() {
    await (ThisForm.WindowState = TMiniblink.WindowState.Minimized);
}
async function MoveWindow() {
    var maxButton = document.getElementById("MaxButton");
    maxButton.innerText = "最大化";
    ThisForm.Move()
}
async function OnContextMenu(elm,evt) {
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
    await ThisForm.ShowContextMenu(elm, evt,180,100, "Tne://Tnelab.TneForm.Test/ui/TestContextMenu.html");
}