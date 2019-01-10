///<reference path="../../../Tnelab.TneForm.Test.BLL/SimpleTestService.cs.ts"/>
let newForm: TMiniblink.TneForm;
let eventTest = function (sender, args) {
    alert(args.Files.join(",") + ":X:" + args.X + ":Y:" + args.Y);
}
async function InvokeTest() {
    try {
        newForm = await new TMiniblink.TneForm("Tne://Tnelab.TneForm.Test/ui/default.html?cmd=测试").Ready();
        await newForm.Show();
    }
    catch (error) {
        console.log(error);
    }
}
async function callOtherForm() {
    let event = await ThisForm.DragFilesEvent;
    await event.RemoveListener(eventTest);
    alert("HI2");
}
async function showId() {
    alert(ThisForm.TneMapNativeObjectId);
}
