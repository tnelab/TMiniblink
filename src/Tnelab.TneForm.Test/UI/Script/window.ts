///<reference path="../../../Tnelab.TneForm/TneApp.d.ts"/>
async function MaxWindow(elm) {
    let old = await ThisForm.WindowState;
    let state = old === 1 ? 2 : 1;
    await (ThisForm.WindowState = state);
    if (state === 1) {
        elm.innerText = "恢复";
    }
    else if(state===2) {
        elm.innerText = "最大化";
    }
}
async function MiniWindow() {
    await (ThisForm.WindowState = 3);
}
async function MoveWindow() {
    var maxButton = document.getElementById("MaxButton");
    maxButton.innerText = "最大化";
    ThisForm.Move()
}