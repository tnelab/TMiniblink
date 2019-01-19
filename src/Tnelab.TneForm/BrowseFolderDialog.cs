//tne://to_ts
//namespace:TMiniblink
//import:./tnemap.ts
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Tnelab.HtmlView
{
    public class BrowseFolderDialog
    {
        public IntPtr OwnerHandle { get; set; } = IntPtr.Zero;
        public string Title { get; set; } = null;
        public string ShowDialog()
        {
            IntPtr displayNamePtr = Marshal.AllocHGlobal(255 * 2);
            NativeMethods.BROWSEINFO bi = new NativeMethods.BROWSEINFO();
            bi.hwndOwner = this.OwnerHandle;
            bi.pszDisplayName = displayNamePtr;
            bi.lpszTitle = this.Title;
            IntPtr idl = NativeMethods.SHBrowseForFolderW(ref bi);
            var r = NativeMethods.SHGetPathFromIDListW(idl, displayNamePtr);
            string result = null;
            if (r)
            {
                result =Marshal.PtrToStringUni(displayNamePtr);
            }
            Marshal.FreeHGlobal(displayNamePtr);
            return result;

        }
    }
}
