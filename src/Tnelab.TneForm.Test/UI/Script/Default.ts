///<reference path="../../../Tnelab.TneForm.Test.BLL/SimpleTestService.cs.ts"/>
async function InvokeTest() {
    try {
        var win = Tnelab.ThisForm.GetHtmlWindow();
        alert(win);
        //var simple = await new BLL.SimpleTestService().Ready();
        var newForm = await new Tnelab.TneForm().Ready();
        await (newForm.Url = "Tne://Tnelab.TneForm.Test/ui/default.html?cmd=测试");
        await newForm.Show();
        await (newForm.ShowInTaskBar = false);
        //var t = await newForm.RunFunc(function () {
        //    var win = window as any;
        //    win.MiniWindow();
        //    return "";
        //});
    }
    catch (error) {
        console.log(error);
    }
}
async function showId() {
    alert(Tnelab.ThisForm.TneMapNativeObjectId);
}
