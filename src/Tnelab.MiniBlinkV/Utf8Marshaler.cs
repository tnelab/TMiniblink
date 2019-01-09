using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;

namespace Tnelab.MiniBlinkV
{
    public class Utf8Marshaler : ICustomMarshaler
    {
        static Utf8Marshaler static_instance;

        public IntPtr MarshalManagedToNative(object managedObj)
        {
            if (managedObj == null)
                return IntPtr.Zero;
            if (!(managedObj is string))
                throw new MarshalDirectiveException(
                       "UTF8Marshaler must be used on a string.");

            // not null terminated
            byte[] strbuf = Encoding.UTF8.GetBytes((string)managedObj);
            IntPtr buffer = Marshal.AllocHGlobal(strbuf.Length + 1);
            Marshal.Copy(strbuf, 0, buffer, strbuf.Length);

            // write the terminating null
            Marshal.WriteByte(buffer + strbuf.Length, 0);
            lock (ptrListLock_)
            {
                ptrList_.Add(buffer);
            }
            return buffer;
        }

        public unsafe object MarshalNativeToManaged(IntPtr pNativeData)
        {
            byte* walk = (byte*)pNativeData;

            // find the end of the string
            while (*walk != 0)
            {
                walk++;
            }
            int length = (int)(walk - (byte*)pNativeData);

            // should not be null terminated
            byte[] strbuf = new byte[length];
            // skip the trailing null
            Marshal.Copy((IntPtr)pNativeData, strbuf, 0, length);
            string data = Encoding.UTF8.GetString(strbuf);
            return data;
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            //IntPtr ptr = IntPtr.Zero;
            //lock (ptrListLock_)
            //{
            //    ptr = ptrList_.SingleOrDefault(it => it.ToInt64() == pNativeData.ToInt64());
            //    if (ptr != IntPtr.Zero)
            //    {
            //        Marshal.FreeHGlobal(pNativeData);
            //        ptrList_.Remove(ptr);
            //    }
            //}            
        }

        public void CleanUpManagedData(object managedObj)
        {
        }

        public int GetNativeDataSize()
        {
            return -1;
        }
        static readonly List<IntPtr> ptrList_ = new List<IntPtr>();
        static readonly object ptrListLock_=new Object();
        public static ICustomMarshaler GetInstance(string cookie)
        {
            if (static_instance == null)
            {
                return static_instance = new Utf8Marshaler();
            }
            return static_instance;
        }
    }
}
