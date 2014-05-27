using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Utility
{
    public class DoubleKeyDictionaryItem<TKey1, TKey2, TValue>
    {
        public TKey1 Key1 { get; private set; }
        public TKey2 Key2 { get; private set; }
        public TValue Value { get; private set; }

        public DoubleKeyDictionaryItem(TKey1 key1, TKey2 key2, TValue value)
        {
            Key1 = key1;
            Key2 = key2;
            Value = value;
        }
    }

    public class DoubleKeyDictionary<TKey1, TKey2, TValue> : IEnumerable<DoubleKeyDictionaryItem<TKey1, TKey2, TValue>>
    {
        private Dictionary<TKey1, Dictionary<TKey2, TValue>> _dictionary;
        private IList<DoubleKeyDictionaryItem<TKey1, TKey2, TValue>> _items;

        public DoubleKeyDictionary()
        {
            _dictionary = new Dictionary<TKey1, Dictionary<TKey2, TValue>>();
            _items = new List<DoubleKeyDictionaryItem<TKey1, TKey2, TValue>>();
        }

        public void Add(TKey1 key1, TKey2 key2, TValue value)
        {
            var secondLevel = GetSecondLevel(key1, true);

            secondLevel.Add(key2, value);

            _items.Add(new DoubleKeyDictionaryItem<TKey1, TKey2, TValue>(key1, key2, value));
        }

        public bool TryGetValue(TKey1 key1, TKey2 key2, out TValue value)
        {
            bool result = false; 
            value = default(TValue);

            var secondLevel = GetSecondLevel(key1);
            if (secondLevel != null)
            {
                result = secondLevel.TryGetValue(key2, out value);
            }

            return result;
        }

        private Dictionary<TKey2, TValue> GetSecondLevel(TKey1 key1, bool create = false)
        {
            Dictionary<TKey2, TValue> secondLevel = null;

            if (!_dictionary.TryGetValue(key1, out secondLevel))
            {
                if (create)
                {
                    secondLevel = new Dictionary<TKey2, TValue>();
                    _dictionary.Add(key1, secondLevel);
                }
            }

            return secondLevel;
        }

        public IEnumerator<DoubleKeyDictionaryItem<TKey1, TKey2, TValue>> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
