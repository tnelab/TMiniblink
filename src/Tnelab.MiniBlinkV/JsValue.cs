using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tnelab.MiniBlink
{
    public class JsValue
    {
        private long _v;
        internal long Value { get { return _v; } }
        internal JsValue(long v)
        {
            this._v = v;
        }
        public JsType JsTypeOf()
        {
            return NativeMethods.jsTypeOf(this._v);
        }

        public bool IsNumber()
        {
            return NativeMethods.jsIsNumber(this._v);
        }

        public bool IsString()
        {
            return NativeMethods.jsIsString(this._v);
        }

        public bool IsBoolean()
        {
            return NativeMethods.jsIsBoolean(this._v);
        }

        public bool IsObject()
        {
            return NativeMethods.jsIsObject(this._v);
        }

        public bool IsFunction()
        {
            return NativeMethods.jsIsFunction(this._v);
        }

        public bool IsUndefined()
        {
            return NativeMethods.jsIsUndefined(this._v);
        }

        public bool IsNull()
        {
            return NativeMethods.jsIsNull(this._v);
        }

        public bool IsArray()
        {
            return NativeMethods.jsIsArray(this._v);
        }

        public bool IsTrue()
        {
            return NativeMethods.jsIsTrue(this._v);
        }

        public bool IsFalse()
        {
            return NativeMethods.jsIsFalse(this._v);
        }

    }
}
