using System;
using System.Text;
using System.Threading.Tasks;

namespace Tnelab.HtmlView
{
    public static class TneApplication
    {
        public static bool IsVip { get; private set; } = false;
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
        public static void SetToVip()
        {
            if (ReadOnlyVipFlag)
                throw new Exception("窗口消息循环已运行，不得更改VIP设置");
            IsVip = true;
        }
        static internal bool ReadOnlyVipFlag = false;
    }
}
