///<reference path="../../../Tnelab.TneForm.Test.BLL/SimpleTestService.cs.ts"/>
async function InvokeTest() {
    var simple = await new BLL.SimpleTestService().Ready();
    alert("hi");
    
}
