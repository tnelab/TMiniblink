using System;
using System.Text;
using System.Threading.Tasks;

namespace Tnelab.HtmlView
{
    public class TneApplication
    {
        public static TneForm MainForm { get; private set; }
        public static void Run(TneForm form)
        {
            MainForm = form;
            NativeMethods.MSG msg = new NativeMethods.MSG();
            while (NativeMethods.GetMessage(ref msg, IntPtr.Zero, 0, 0))
            {
                NativeMethods.TranslateMessage(ref msg);

                NativeMethods.DispatchMessage(ref msg);
            }
        }
    }
}
