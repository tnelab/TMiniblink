using System;
using System.Collections.Generic;

namespace cmd
{
    class Program
    {
        public static List<string> GetTypeNames(string genericInfo)
        {
            var typeNams = new List<string>();
            var lc = 0;
            var b = 0;
            for (var i = 0; i < genericInfo.Length; i++)
            {
                if (genericInfo[i] == '<')
                {
                    lc++;
                }
                else if (genericInfo[i] == '>')
                {
                    lc--;
                }
                else if (genericInfo[i] == ',')
                {
                    if (lc == 0)
                    {
                        typeNams.Add(genericInfo.Substring(b, i - b));
                        b = i + 1;
                    }
                }
            }
            if (b < genericInfo.Length)
            {
                typeNams.Add(genericInfo.Substring(b, genericInfo.Length - b));
            }
            return typeNams;
        }
        public static string GetFriendName(Type type)
        {
            if (!type.IsGenericType)
            {
                return $"{type.Namespace}.{type.Name}";
            }
            var gtypeNameList = new List<string>();
            foreach (var gtype in type.GenericTypeArguments)
            {
                gtypeNameList.Add(GetFriendName(gtype));
            }
            var b = type.Name.IndexOf("`");
            var name = type.Name.Substring(0, b);
            var result=$"{type.Namespace}.{name}<{string.Join(",",gtypeNameList.ToArray())}>";
            return result;
        }
        public static string GetGenericInfo(Type type)
        {
            string GetFriendName(Type t)
            {
                if (!t.IsGenericType)
                {
                    return $"{t.Namespace}.{t.Name}";
                }
                var gtypeNameList = new List<string>();
                foreach (var gtype in t.GenericTypeArguments)
                {
                    gtypeNameList.Add(GetFriendName(gtype));
                }
                var b = t.Name.IndexOf("`");
                var name = t.Name.Substring(0, b);
                var result = $"{t.Namespace}.{name}<{string.Join(",", gtypeNameList.ToArray())}>";
                return result;
            }
            var friendName = GetFriendName(type);
            var sb = friendName.IndexOf("<");
            var tmps = GetTypeNames(friendName.Substring(sb+1 , friendName.Length - sb-2));
            return String.Join("|",tmps);
        }
        static void Main(string[] args)
        {
            var action = new Action<string, Action<int, string>, int>((a1,a2,a3)=> { });
            var genericInfo = GetGenericInfo(action.GetType());
            
        }
    }
}
