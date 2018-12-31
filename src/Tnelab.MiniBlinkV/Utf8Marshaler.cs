using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Tnelab.MiniBlinkV
{
    class Utf8Marshaler : ICustomMarshaler
    {
        int size_ = 0;
        public void CleanUpManagedData(object ManagedObj)
        {
            
        }
        object ptrLock_ = new object();
        public void CleanUpNativeData(IntPtr pNativeData)
        {
            lock (ptrLock_)
            {
                if(ptr_.ToInt64()!=pNativeData.ToInt64()&&ptr_!=IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(ptr_);
                }
                this.size_ = 0;
            }
        }

        public int GetNativeDataSize()
        {
            return this.size_;
        }
        IntPtr ptr_ = IntPtr.Zero;
        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            lock (ptrLock_)
            {
                var datas = Encoding.UTF8.GetBytes(ManagedObj.ToString() + "\0");
                var ptr = Marshal.AllocHGlobal(datas.Length);
                Marshal.Copy(datas, 0, ptr, datas.Length);
                ptr_ = ptr;
                this.size_ = datas.Length;
                return ptr;
            }
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            List<byte> datas = new List<byte>();
            byte b = Marshal.ReadByte(pNativeData);
            while (b != 0)
            {
                datas.Add(b);
                pNativeData = pNativeData + 1;
                b = Marshal.ReadByte(pNativeData);
            }
            size_ = datas.Count;
            var str = Encoding.UTF8.GetString(datas.ToArray());
            return str;
        }
        public static ICustomMarshaler GetInstance(string cookie)
        {
            return new Utf8Marshaler();
        }
    }
}
