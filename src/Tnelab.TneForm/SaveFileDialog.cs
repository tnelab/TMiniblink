//tne://to_ts
//namespace:TMiniblink
//import:./tnemap.ts
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Tnelab.HtmlView
{
    public class SaveFileDialog
    {
        public IntPtr OwnerHandle { get; set; } = IntPtr.Zero;
        public string Filter { get; set; }
        public string Title { get; set; }
        public string File { get; set; }
        public string ShowDialog()
        {
            var filePtr = Marshal.AllocHGlobal(1024);
            var fdatas = new byte[1024];
            if (!string.IsNullOrEmpty(this.File))
                Encoding.Unicode.GetBytes(this.File).CopyTo(fdatas, 0);
            Marshal.Copy(fdatas, 0, filePtr, fdatas.Length);
            NativeMethods.OPENFILENAMEW ofn = new NativeMethods.OPENFILENAMEW();
            ofn.lStructSize = Marshal.SizeOf<NativeMethods.OPENFILENAMEW>();
            ofn.lpstrTitle = this.Title;
            ofn.lpstrFilter = Filter.Replace('(', '\0').Replace(')', '\0').Replace("|", "");
            ofn.hwndOwner = this.OwnerHandle;
            ofn.lpstrFile = filePtr;
            ofn.nMaxFile = 1024;
            var r = NativeMethods.GetSaveFileNameW(ref ofn);
            if (r)
            {
                var datas = new byte[1024];
                Marshal.Copy(ofn.lpstrFile, datas, 0, datas.Length);
                var files = Encoding.Unicode.GetString(datas);
                var tmps = files.Split('\0');
                var result = tmps[0];
                Marshal.FreeHGlobal(filePtr);
                return result;
            }
            Marshal.FreeHGlobal(filePtr);
            return null;
        }
    }
}
