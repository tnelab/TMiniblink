using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Tnelab.MiniBlinkV
{
    class Utf8Marshaler : ICustomMarshaler
    {
        IntPtr dataPtr_ = IntPtr.Zero;
        int size_ = 0;
        public void CleanUpManagedData(object ManagedObj)
        {
            
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            if (dataPtr_ != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(this.dataPtr_);
                this.dataPtr_ = IntPtr.Zero;
            }
        }

        public int GetNativeDataSize()
        {
            return this.size_;
        }

        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            var datas = Encoding.UTF8.GetBytes(ManagedObj.ToString());
            var ptr = Marshal.AllocHGlobal(datas.Length);
            Marshal.Copy(datas, 0, ptr, datas.Length);
            if (this.dataPtr_ != IntPtr.Zero)
            {
                throw new Exception("封送异常");
            }
            this.size_ = datas.Length;
            this.dataPtr_ = ptr;
            return ptr;
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
            var str = Encoding.UTF8.GetString(datas.ToArray());
            return str;
        }
        public static ICustomMarshaler GetInstance(string cookie)
        {
            return new Utf8Marshaler();
        }
    }
}
