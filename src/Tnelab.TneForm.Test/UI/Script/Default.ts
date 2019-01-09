///<reference path="../../../Tnelab.TneForm.Test.BLL/SimpleTestService.cs.ts"/>
let newForm: Tnelab.TneForm;
let eventTest = function (sender, args) {
    alert(args.Files.join(",") + ":X:" + args.X + ":Y:" + args.Y);
}
async function InvokeTest() {
    try {
        newForm = await new Tnelab.TneForm().Ready();
        await (newForm.Url = "Tne://Tnelab.TneForm.Test/ui/default.html?cmd=测试");
        await (newForm.TopMost = true);
        await newForm.Show();
    }
    catch (error) {
        console.log(error);
    }
}
async function callOtherForm() {
    let event = await Tnelab.ThisForm.DragFilesEvent;
    await event.RemoveListener(eventTest);
    alert("HI2");
}
async function showId() {
    alert(Tnelab.ThisForm.TneMapNativeObjectId);
}
