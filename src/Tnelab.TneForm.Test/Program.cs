using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Tnelab.HtmlView.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //TneApplication.SetToVip();
            var f1 = new TneForm("Tne://Tnelab.TneForm.Test/ui/default.html?cmd=测试");
            //var f1 = new TneForm("http://m.cdsgnk.cn/"); 
            f1.Icon = "default.png";
            //f1.WindowState = WindowState.Maximized;
            //f1.StartPosition = StartPosition.CenterParent;
            //f1.Url = "file:///E:/workspace/Tnelab/src/Tnelab.TneForm.Test/bin/Debug/netcoreapp2.1/UI/Default.html";
            //f1.Url = "https://www.baidu.com";
            //f1.Url = "https://www.html5tricks.com/demo/html5-canvas-particle-effect/index.html";
            //f1.Url = "http://www.sina.com.cn";
            //f1.Url = "http://html5test.com/";
            f1.MinWidth = 800;
            f1.MinHeight = 600;
            //f1.SizeAble = false;
            f1.Show();
            //var f2 = new TneForm();
            //f2.Parent = f1;
            //f2.Width = 300;
            //f2.Height = 200;
            //f2.StartPosition = StartPosition.CenterParent;
            //f2.Show();
            TneApplication.Run(f1);
        }
    }
}
