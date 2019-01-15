//tne://to_ts
//namespace:TMiniblink
//import:./tnemap.ts
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Tnelab.HtmlView
{
    public class OpenFileDialog
    {
        public IntPtr OwnerHandle { get; set; } = IntPtr.Zero;
        public string Filter { get; set; }
        public string Title { get; set; }
        public bool AllowMultiSelect { get; set; } = false;
        public string[] ShowDialog()
        {
            var flag = AllowMultiSelect ? 0x00000200| 0x00800000| 0x00080000 : 0;
            var filePtr = Marshal.AllocHGlobal(1024);
            var fdatas = new byte[1024];
            Marshal.Copy(fdatas, 0, filePtr, fdatas.Length);
            NativeMethods.OPENFILENAMEW ofn = new NativeMethods.OPENFILENAMEW();
            ofn.lStructSize = Marshal.SizeOf<NativeMethods.OPENFILENAMEW>();
            ofn.lpstrTitle = this.Title;
            ofn.lpstrFilter = Filter.Replace('(','\0').Replace(')', '\0').Replace("|", "");
            ofn.hwndOwner = this.OwnerHandle;
            ofn.lpstrFile = filePtr;
            ofn.Flags = flag;
            ofn.nMaxFile = 1024;
            var r=NativeMethods.GetOpenFileNameW(ref ofn);
            if (r)
            {
                var datas = new byte[1024];
                Marshal.Copy(ofn.lpstrFile, datas, 0, datas.Length);
                var files = Encoding.Unicode.GetString(datas);
                var tmps = files.Split('\0');
                if (this.AllowMultiSelect)
                {
                    List<string> resultList = new List<string>();
                    var dir = tmps[0];
                    for (var i = 1; i < tmps.Length; i++)
                    {
                        if (tmps[i].Length == 0)
                        {
                            if (i == 1)
                                resultList.Add(dir);
                            break;
                        }
                        else
                        {
                            resultList.Add($"{dir}\\{tmps[i]}");
                        }
                    }
                    Marshal.FreeHGlobal(filePtr);
                    return resultList.ToArray();
                }
                else
                {
                    var result = new string[1] { tmps[0] };
                    Marshal.FreeHGlobal(filePtr);
                    return result;
                }
            }
            Marshal.FreeHGlobal(filePtr);
            return new string[0];
        }
    }
}
