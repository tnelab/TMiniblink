using System;
using System.Collections.Generic;
using System.Text;

namespace Tnelab.HtmlView
{
    class TneHashDic
    {
        public void AddToHashDic(string keyToHash, object obj)
        {
            hashDic_.Add(GetHash(Encoding.UTF8.GetBytes(keyToHash)), obj);
        }
        public void RemoveFromHashDic(string keyToHash)
        {
            var hash = GetHash(Encoding.UTF8.GetBytes(keyToHash));
            if (hashDic_.ContainsKey(hash))
            {
                hashDic_.Remove(hash);
            }
        }
        public object GetFromHashDic(string keyToHash)
        {
            var hash = GetHash(Encoding.UTF8.GetBytes(keyToHash));
            if (hashDic_.ContainsKey(hash))
            {
                return hashDic_[hash];
            }
            return null;
        }
        private readonly Dictionary<int, object> hashDic_ = new Dictionary<int, object>();
        private int GetHash(byte[] datas)
        {
            int h = 0;
            foreach (var item in datas)
            {
                h = 31 * h + item;
            }
            return h;
        }
    }
}
