using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tnelab.MiniBlink
{
    public class JsExecState
    {
        private IntPtr _es = IntPtr.Zero;
        internal IntPtr ESPtr { get { return this._es; } }
        internal JsExecState(IntPtr es)
        {
            this._es = es;
        }
        public static JsValue Int(int n)
        {
            return new JsValue(NativeMethods.jsInt(n));
        }
        public static JsValue Float(float f)
        {
            return new JsValue(NativeMethods.jsFloat(f));
        }
        public static JsValue Double(double d)
        {
            return new JsValue(NativeMethods.jsDouble(d));
        }
        public static JsValue Boolean(bool b)
        {
            return new JsValue(NativeMethods.jsBoolean(b));
        }

        public static JsValue Undefined()
        {
            return new JsValue(NativeMethods.jsUndefined());
        }
        public static JsValue Null()
        {
            return new JsValue(NativeMethods.jsNull());
        }
        public static JsValue True()
        {
            return new JsValue(NativeMethods.jsTrue());
        }
        public static JsValue False()
        {
            return new JsValue(NativeMethods.jsFalse());
        }
        public int ArgCount()
        {
            return NativeMethods.jsArgCount(_es);
        }
        public JsType ArgType(int argIdx)
        {
            return NativeMethods.jsArgType(this._es, argIdx);
        }
        public JsValue Arg(int argIdx)
        {
            return new JsValue(NativeMethods.jsArg(this._es, argIdx));
        }
        public int ToInt(JsValue v)
        {
            return NativeMethods.jsToInt(this._es, v.Value);
        }
        public float ToFloat(JsValue v)
        {
            return NativeMethods.jsToFloat(this._es, v.Value);
        }
        public double ToDouble(JsValue v)
        {
            return NativeMethods.jsToDouble(this._es, v.Value);
        }
        public bool ToBoolean(JsValue v)
        {
            return NativeMethods.jsToBoolean(this._es, v.Value);
        }
        public string ToTempStringW(JsValue v)
        {
            return NativeMethods.jsToTempStringW(this._es, v.Value);
        }
        public string ToStringW(JsValue v)
        {
            return NativeMethods.JsToStringW(this._es, v.Value);
        }
        public JsValue StringW(string str)
        {
            return new JsValue(NativeMethods.jsStringW(this._es, str));
        }
        public JsValue EmptyObject()
        {
            return new JsValue(NativeMethods.jsEmptyObject(this._es));
        }
        public JsValue EmptyArray()
        {
            return new JsValue(NativeMethods.jsEmptyArray(this._es));
        }
        public JsValue Object(IntPtr obj)
        {
            return new JsValue(NativeMethods.jsObject(this._es, obj));
        }
        public Tuple<JsValue,IntPtr> Object(JsData jsData)
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(jsData));
            Marshal.StructureToPtr(jsData, ptr, true);
            var jsValue= new JsValue(NativeMethods.jsObject(this._es, ptr));
            return new Tuple<JsValue, IntPtr>(jsValue, ptr);
        }
        //public JsValue Object(JsData obj)
        //{
        //    return new JsValue(NativeMethods.jsObject(this._es, obj));
        //}
        public JsValue Function(JsData obj)
        {
            return new JsValue(NativeMethods.jsFunction(this._es, obj));
        }

        public IntPtr GetData(JsValue obj)
        {
            return NativeMethods.jsGetData(this._es, obj.Value);
        }


        public JsValue Get(JsValue obj, string prop)
        {
            return new JsValue(NativeMethods.jsGet(this._es, obj.Value, prop));
        }

        public void Set(JsValue obj, string prop, JsValue v)
        {
            NativeMethods.jsSet(this._es, obj.Value, prop, v.Value);
        }


        public JsValue GetAt(JsValue obj, int index)
        {
            return new JsValue(NativeMethods.jsGetAt(this._es, obj.Value, index));
        }

        public void SetAt(JsValue obj, int index, JsValue v)
        {
            NativeMethods.jsSetAt(this._es, obj.Value, index, v.Value);
        }


        public int GetLength(JsValue obj)
        {
            return NativeMethods.jsGetLength(this._es, obj.Value);
        }

        public void SetLength(JsValue obj, int length)
        {
            NativeMethods.jsSetLength(this._es, obj.Value, length);
        }
        //window object
        public JsValue GlobalObject()
        {
            return new JsValue(NativeMethods.jsGlobalObject(this._es));
        }
        public WebView GetWebView()
        {
            return WebView.GetWebView(NativeMethods.jsGetWebView(this._es));
        }

        public JsValue EvalW(string str)
        {
            return new JsValue(NativeMethods.jsEvalW(this._es, str));
        }
        public JsValue EvalExW(string str, bool isInClosure)
        {
            return new JsValue(NativeMethods.jsEvalExW(this._es, str, isInClosure));
        }

        public JsValue Call(JsValue func, JsValue thisObject, JsValue[] args)
        {
            long[] lArgs = new long[args.Length];
            for (int i = 0; i < lArgs.Length; i++)
            {
                lArgs[i] = args[i].Value;
            }
            return new JsValue(NativeMethods.jsCall(this._es, func.Value, thisObject.Value, lArgs, lArgs.Length));
        }
        public JsValue CallGlobal(JsValue func, JsValue[] args)
        {
            long[] lArgs = new long[args.Length];
            for (int i = 0; i < lArgs.Length; i++)
            {
                lArgs[i] = args[i].Value;
            }
            return new JsValue(NativeMethods.jsCallGlobal(this._es, func.Value, lArgs, lArgs.Length));
        }

        public JsValue GetGlobal(string prop)
        {
            return new JsValue(NativeMethods.jsGetGlobal(this._es, prop));
        }
        public void SetGlobal(string prop, JsValue v)
        {
            NativeMethods.jsSetGlobal(this._es, prop, v.Value);
        }
    }
}
