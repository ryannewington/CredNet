using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CredNet
{
    internal class BiDictionary<TKey, TValue>
    {
        protected Dictionary<TKey, TValue> ForwardCollection { get; set; } = new Dictionary<TKey, TValue>();
        protected Dictionary<TValue, TKey> BackwardCollection { get; set; } = new Dictionary<TValue, TKey>();

        public Indexer<TKey, TValue> Forward { get; }
        public Indexer<TValue, TKey> Backward { get; }

        public BiDictionary()
        {
            Forward = new Indexer<TKey, TValue>(ForwardCollection);
            Backward = new Indexer<TValue, TKey>(BackwardCollection);
        }

        public void Add(TKey key, TValue value)
        {
            ForwardCollection.Add(key, value);
            BackwardCollection.Add(value, key);
        }

        public void Remove(TKey key)
        {
            BackwardCollection.Remove(ForwardCollection[key]);
            ForwardCollection.Remove(key);
        }

        public TValue this[TKey key]
        {
            get => Forward[key];
        }

        public int Count { get => ForwardCollection.Count; }

        public class Indexer<TKey2, TValue2>
        {
            private readonly IDictionary<TKey2, TValue2> mTarget;
            
            public Indexer(IDictionary<TKey2, TValue2> dic)
            {
                mTarget = dic;
            }

            public bool TryGetValue(TKey2 key, out TValue2 value)
            {
                return mTarget.TryGetValue(key, out value);
            }

            public TValue2 this[TKey2 index]
            {
                get => mTarget[index];
            }
        }
    }
}
