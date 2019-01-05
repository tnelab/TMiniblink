///<reference path="../../../Tnelab.TneForm.Test.BLL/SimpleTestService.cs.ts"/>
let newForm: Tnelab.TneForm;
async function InvokeTest() {
    try {
        //var simple = await new BLL.SimpleTestService().Ready();
        newForm = await new Tnelab.TneForm().Ready();
        await (newForm.Url = "Tne://Tnelab.TneForm.Test/ui/default.html?cmd=测试");
        await newForm.Show();       
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
async function callOtherForm() {
    let result = await newForm.RunFunc(async function () {
        try {
            await MiniWindow();
        }
        catch (error) {
            alert(error);
        }
        return undefined;
    });
}
async function showId() {
    alert(Tnelab.ThisForm.TneMapNativeObjectId);
}
