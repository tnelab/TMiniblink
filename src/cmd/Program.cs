using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;

namespace cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test t = new Test();
            //Console.ReadKey();
            //var now = DateTime.Now;
            //for (var i = 0; i < 10000000; i++)
            //{
            //    t.ToRun();
            //}
            //Console.WriteLine(DateTime.Now - now);
            //Console.ReadKey();

            //Console.ReadKey();
            //var now = DateTime.Now;
            //for (var i = 0; i < 10000000; i++)
            //{
            //    t.GetType().GetMethod("ToRun").Invoke(t, null);
            //}
            //Console.WriteLine(DateTime.Now - now);
            //Console.ReadKey();

            //dynamic t = new Test();
            //Console.ReadKey();
            //var now = DateTime.Now;
            //for (var i = 0; i < 10000000; i++)
            //{
            //    t.ToRun();
            //}
            //Console.WriteLine(DateTime.Now - now);
            //Console.ReadKey();

            var t = new Test();
            var r = Expression.Lambda(Expression.Call(Expression.Constant(t), t.GetType().GetMethod("ToRun"))).Compile();
            var method = t.GetType().GetMethod("ToRun");
            var action = r as Action;
            Console.ReadKey();
            var now = DateTime.Now;
            for (var i = 0; i < 10000000; i++)
            {
                //(Expression.Lambda(Expression.Call(Expression.Constant(t), method)).Compile() as Action)();
                //Expression.Lambda(Expression.Call(Expression.Constant(t), method)).Compile();
                method.Invoke(t, null);
            }
            Console.WriteLine(DateTime.Now - now);
            Console.ReadKey();

            //var t = new Test();
            //var r = Expression.Lambda(Expression.Call(Expression.Constant(t), t.GetType().GetMethod("ToRun"))).Compile();
            //var action = r as Action;
            //var Site = CallSite<Action<CallSite, object>>.Create(
            //                        Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ToRun", null, typeof(Test), new CSharpArgumentInfo[1]
            //                        {
            //                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
            //                        }));

            //Console.ReadKey();
            //var now = DateTime.Now;
            //for (var i = 0; i < 10000000; i++)
            //{
            //    CallSite<Action<CallSite, object>>.Create(
            //      Binder.InvokeMember(
            //            CSharpBinderFlags.ResultDiscarded, 
            //            "ToRun", 
            //            null, 
            //            typeof(Test), 
            //            new CSharpArgumentInfo[1]
            //            {
            //                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
            //            }
            //      )
            //    ).Target(Site, t);
            //}
            //Console.WriteLine(DateTime.Now - now);
            //Console.ReadKey();
        }
    }
    class Test
    {
        public void ToRun()
        {
            var n = 2;
            for(var i = 0; i < 1000; i++)
            {
                n = n * n;
            }
        }
    }

}
