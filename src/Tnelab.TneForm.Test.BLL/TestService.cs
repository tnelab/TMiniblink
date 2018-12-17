using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tnelab.TneForm.Test.BLL
{
    public class TestService<T,T2>
    {
        public TestService(Action<T,Action<T,Action<T,Action<T>,T>,int>,T,T> action)
        {

        }
        public TestService()
        {
        }
        public TestService(T t,T t2,T t3,int t4,string t5)
        {

        }
        public Action<string> ActionProp {
            get;
            set;
        }
        public void CallbackActionTest(Action<string> action)
        {
            action("你好这是CallbackActionTest");
        }        
        public string CallbackFuncTest(Func<string, string> func)
        {
            return func("你好这是CallbackFuncTest");
        }
        public static void CallStaticMethod()
        {

        }
        /// <summary>
        /// 泛型方法测试
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public T GMethodTest<T1>(T t)
        {
            return t;
        }
        //public TestService TestProp { get;private set; }
        /*TS生成测试/////////////////////////////////////////////////////////////////////////////////////////////*/        
        public int Method1(string name,string pwd)
        {
            return 123;
        }
        public bool Method2(Action<string, bool, int, DateTime> action)
        {
            return false;
        }
        public void Method3(Func<string, bool, int, DateTime> func)
        {
            var t = new List<object>();
            foreach(var group in t.GroupBy(it => it.GetHashCode())){
                group.ToList();
            }
        }
        public void Method4(Func<string, bool, int, DateTime,int> func)
        {

        }
        public T2 Method51<T1, T2, T3>(Func<T1, bool> func, T t, T2 t2, T3 t3)
        {
            return t2;
        }
        public T2 Method5<T1,T2,T3>(Func<T1, bool> func,T t,T2 t2,T3 t3)
        {
            return t2;
        }
        public T2 Method5<T1, T2>(Func<T1, bool> func, T t, T2 t2)
        {
            return t2;
        }
        public T Method5(Func<T, bool> func, T t)
        {
            return t;
        }
        public T Method5<T1>(Func<T1, bool> func, T t)
        {
            return t;
        }
    }
}
