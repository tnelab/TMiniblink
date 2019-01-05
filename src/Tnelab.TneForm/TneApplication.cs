using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tnelab.HtmlView
{
    public static class TneApplication
    {
        public static bool IsVip { get; private set; } = false;
        public static TneForm MainForm { get; private set; }
        static List<Action> uiInvokeList_ = new List<Action>();
        static object uiInvokeListLock_ = new object();
        static object uiTcsLock_ = new object();
        public static bool IsMainTask()
        {
            return mainTaskId_ == Task.CurrentId;
        }
        public static void UIInvoke(Action action){
            if (IsMainTask())
            {
                action();
                return;
            }
            lock (uiTcsLock_)
            {
                TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
                lock (uiInvokeListLock_)
                {
                    uiInvokeList_.Add(() =>
                    {
                        action();
                        tcs.SetResult(true);
                    });
                }
                tcs.Task.Wait();
            }
        }
        static int? mainTaskId_ = null;
        public static void DoEvent()
        {
            if (IsMainTask())
            {
                lock (uiInvokeListLock_)
                {
                    if (uiInvokeList_.Count > 0)
                    {
                        foreach (var action in uiInvokeList_)
                        {
                            action();
                        }
                        uiInvokeList_.Clear();
                    }
                }
            }
            UIInvoke(() => {
                NativeMethods.MSG msg = new NativeMethods.MSG();
                NativeMethods.GetMessage(ref msg, IntPtr.Zero, 0, 0);
                NativeMethods.TranslateMessage(ref msg);
                NativeMethods.DispatchMessage(ref msg);
            });
        }
        static void WatchMsg()
        {
            NativeMethods.MSG msg = new NativeMethods.MSG();
            while (NativeMethods.GetMessage(ref msg, IntPtr.Zero, 0, 0))
            {
                lock (uiInvokeListLock_)
                {
                    if (uiInvokeList_.Count > 0)
                    {
                        foreach (var action in uiInvokeList_)
                        {
                            action();
                        }
                        uiInvokeList_.Clear();
                    }
                }
                NativeMethods.TranslateMessage(ref msg);

                NativeMethods.DispatchMessage(ref msg);
            }
        }
        public static void Run(TneForm form)
        {            
            MainForm = form;
            WatchMsg();
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
