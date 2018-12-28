//tne://to_ts?namespace=BLL&TneAppPath=../Tnelab.TneForm/TneApp.d.ts
using System;
using System.Collections.Generic;
using System.Text;

namespace Tnelab.TneForm.Test.BLL
{
    class SimpleTestService
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public int Add(int x, int y) => x + y;
        public static string GetMessage(string msg) => msg;
    }
}
