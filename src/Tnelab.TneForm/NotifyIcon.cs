//tne://to_ts
//namespace:TMiniblink
//import:./TneMap.ts
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Tnelab.HtmlView
{
    public class NotifyIcon
    {
        internal const uint WM_NOTIFYICON=NativeMethods.WM_USER + 0x123;
        static int seed = 1;
        static int NewId() {
            var r=seed;
            seed++;
            return r;
        }
        static readonly Dictionary<int, NotifyIcon> notifyIconDic_ = new Dictionary<int, NotifyIcon>();
        internal static NotifyIcon GetById(int id)
        {
            if (notifyIconDic_.ContainsKey(id))
            {
                return notifyIconDic_[id];
            }
            return null;
        }
        public event EventHandler<EventArgs> Click;
        public event EventHandler<EventArgs> ContextMenu;
        internal void OnClick(object sender, EventArgs args)
        {
            this.Click?.Invoke(sender, args);
        }
        internal void OnContextMenu(object sender,EventArgs args)
        {
            this.ContextMenu?.Invoke(sender, args);
        }
        string tip_ = null;
        public string Tip
        {
            get=>tip_;
            set
            {
                tip_ = value;
                if (isShow_)
                {
                    NativeMethods.NOTIFYICONDATA data = new NativeMethods.NOTIFYICONDATA();
                    data.uFlags = NativeMethods.NIF_MESSAGE | NativeMethods.NIF_TIP;
                    data.hWnd = this.hwnd_;
                    data.uID = id_;
                    data.uCallbackMessage = (int)WM_NOTIFYICON;
                    data.szTip = tip_;
                    data.cbSize = Marshal.SizeOf<NativeMethods.NOTIFYICONDATA>();
                    var dataPtr = Marshal.AllocHGlobal(Marshal.SizeOf<NativeMethods.NOTIFYICONDATA>());
                    Marshal.StructureToPtr(data, dataPtr, true);
                    NativeMethods.Shell_NotifyIcon(NativeMethods.NIM_MODIFY, dataPtr);
                    Marshal.FreeHGlobal(dataPtr);
                }
            }
        }
        string info_ = null;
        public string Info
        {
            get => info_;
            set
            {
                info_ = value;
                if (isShow_)
                {
                    NativeMethods.NOTIFYICONDATA data = new NativeMethods.NOTIFYICONDATA();
                    data.uFlags = NativeMethods.NIF_MESSAGE | NativeMethods.NIF_INFO;
                    data.hWnd = this.hwnd_;
                    data.uID = id_;
                    data.uCallbackMessage = (int)WM_NOTIFYICON;
                    data.szInfo = info_;
                    data.cbSize = Marshal.SizeOf<NativeMethods.NOTIFYICONDATA>();
                    var dataPtr = Marshal.AllocHGlobal(Marshal.SizeOf<NativeMethods.NOTIFYICONDATA>());
                    Marshal.StructureToPtr(data, dataPtr, true);
                    NativeMethods.Shell_NotifyIcon(NativeMethods.NIM_MODIFY, dataPtr);
                    Marshal.FreeHGlobal(dataPtr);
                }
            }
        }
        string icon_ = null;
        public string Icon { get {
                return icon_;
            }
            set {
                icon_ = value;
                if (isShow_)
                {
                    NativeMethods.NOTIFYICONDATA data = new NativeMethods.NOTIFYICONDATA();
                    data.uFlags = NativeMethods.NIF_MESSAGE | NativeMethods.NIF_ICON;
                    data.hIcon = LoadIcon();
                    data.hWnd = this.hwnd_;
                    data.uID = id_;
                    data.uCallbackMessage = (int)WM_NOTIFYICON;
                    data.cbSize = Marshal.SizeOf<NativeMethods.NOTIFYICONDATA>();
                    var dataPtr = Marshal.AllocHGlobal(Marshal.SizeOf<NativeMethods.NOTIFYICONDATA>());
                    Marshal.StructureToPtr(data, dataPtr, true);
                    NativeMethods.Shell_NotifyIcon(NativeMethods.NIM_MODIFY, dataPtr);
                    Marshal.FreeHGlobal(dataPtr);
                }
                else
                {
                    Show();
                }
            }
        }
        IntPtr hwnd_ = IntPtr.Zero;
        public NotifyIcon(IntPtr hwnd,string icon)
        {
            if (hwnd == IntPtr.Zero)
                throw new ArgumentNullException("hwnd");
            if (string.IsNullOrEmpty(icon))
                throw new ArgumentNullException("icon");
            hwnd_ = hwnd;
            icon_ = icon;
            notifyIconDic_.Add(id_, this);
        }
        bool isShow_ = false;
        int id_ = NewId();
        public void Show()
        {
            NativeMethods.NOTIFYICONDATA data = new NativeMethods.NOTIFYICONDATA();
            data.uFlags = NativeMethods.NIF_MESSAGE|NativeMethods.NIF_ICON|NativeMethods.NIF_TIP|NativeMethods.NIF_INFO;
            data.hIcon = LoadIcon();
            data.hWnd = this.hwnd_;
            data.uID = id_;
            data.uCallbackMessage = (int)WM_NOTIFYICON;
            data.szTip = tip_;
            data.szInfo = info_;
            data.cbSize = Marshal.SizeOf<NativeMethods.NOTIFYICONDATA>();
            var dataPtr = Marshal.AllocHGlobal(Marshal.SizeOf<NativeMethods.NOTIFYICONDATA>());
            Marshal.StructureToPtr(data,dataPtr,true);
            NativeMethods.Shell_NotifyIcon(NativeMethods.NIM_ADD,dataPtr);
            Marshal.FreeHGlobal(dataPtr);
            isShow_ = true;
        }
        public void Hide()
        {
            NativeMethods.NOTIFYICONDATA data = new NativeMethods.NOTIFYICONDATA();
            data.uFlags = NativeMethods.NIF_MESSAGE;
            data.hWnd = this.hwnd_;
            data.uID = id_;
            data.uCallbackMessage = (int)WM_NOTIFYICON;
            data.cbSize = Marshal.SizeOf<NativeMethods.NOTIFYICONDATA>();
            var dataPtr = Marshal.AllocHGlobal(Marshal.SizeOf<NativeMethods.NOTIFYICONDATA>());
            Marshal.StructureToPtr(data, dataPtr, true);
            NativeMethods.Shell_NotifyIcon(NativeMethods.NIM_DELETE, dataPtr);
            Marshal.FreeHGlobal(dataPtr);
            isShow_ = false;
        }
        IntPtr LoadIcon()
        {
            var assembly = Assembly.GetEntryAssembly();
            var names = assembly.GetManifestResourceNames();
            var path = names.Single(it => it.ToLower().EndsWith(icon_.ToLower()));
            using (var stream = assembly.GetManifestResourceStream(path))
            {
                var datas = new byte[stream.Length];
                stream.Read(datas, 0, datas.Length);
                var ptr = Marshal.AllocHGlobal((int)stream.Length);
                Marshal.Copy(datas, 0, ptr, datas.Length);
                var hIcon = NativeMethods.CreateIconFromResourceEx(ptr, datas.Length, 1, 0x30000, 32, 32, NativeMethods.LR_DEFAULTCOLOR);
                Marshal.FreeHGlobal(ptr);
                return hIcon;
            }
        }
        ~NotifyIcon()
        {
            notifyIconDic_.Remove(id_);
        }
    }
}
