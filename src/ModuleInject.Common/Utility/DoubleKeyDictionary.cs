using System.Linq;

namespace ModuleInject.Common.Utility
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Two-level dictionary with two keys.
    /// </summary>
    /// <typeparam name="TKey1">Type of the first key.</typeparam>
    /// <typeparam name="TKey2">Type of the second key.</typeparam>
    /// <typeparam name="TValue">Type of the value stored.</typeparam>
    public class DoubleKeyDictionary<TKey1, TKey2, TValue> : IEnumerable<DoubleKeyDictionaryItem<TKey1, TKey2, TValue>>
    {
        private Dictionary<TKey1, Dictionary<TKey2, TValue>> _dictionary;
        private IList<DoubleKeyDictionaryItem<TKey1, TKey2, TValue>> _items;

        /// <summary>
        /// Returns the overall number of stored values.
        /// </summary>
        public int Count { get { return this._items.Count; } }

        public DoubleKeyDictionary()
        {
            this._dictionary = new Dictionary<TKey1, Dictionary<TKey2, TValue>>();
            this._items = new List<DoubleKeyDictionaryItem<TKey1, TKey2, TValue>>();
        }

        /// <summary>
        /// Adds a value under a key combination.
        /// </summary>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        /// <param name="value">The value.</param>
        public void Add(TKey1 key1, TKey2 key2, TValue value)
        {
            var secondLevel = this.GetSecondLevel(key1, true);

            secondLevel.Add(key2, value);

            this._items.Add(new DoubleKeyDictionaryItem<TKey1, TKey2, TValue>(key1, key2, value));
        }

        /// <summary>
        /// Check if a value is stored under a key combination.
        /// </summary>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        /// <returns>True if yes.</returns>
        public bool Contains(TKey1 key1, TKey2 key2)
        {
            TValue value = default(TValue);
            return this.TryGetValue(key1, key2, out value);
        }

        public bool TryGetValue(TKey1 key1, TKey2 key2, out TValue value)
        {
            bool result = false; 
            value = default(TValue);

            var secondLevel = this.GetSecondLevel(key1);
            if (secondLevel != null)
            {
                result = secondLevel.TryGetValue(key2, out value);
            }

            return result;
        }

        private Dictionary<TKey2, TValue> GetSecondLevel(TKey1 key1, bool create = false)
        {
            Dictionary<TKey2, TValue> secondLevel = null;

            if (!this._dictionary.TryGetValue(key1, out secondLevel))
            {
                if (create)
                {
                    secondLevel = new Dictionary<TKey2, TValue>();
                    this._dictionary.Add(key1, secondLevel);
                }
            }

            return secondLevel;
        }

        public IEnumerable<TValue> GetAll()
        {
            IList<TValue> values = new List<TValue>();

            foreach (var kv1 in this._dictionary)
            {
                foreach (var kv2 in kv1.Value)
                {
                    values.Add(kv2.Value);
                }
            }

            return values;
        }

        public IEnumerable<TValue> GetAll(TKey1 key1)
        {
            var dict2 = GetSecondLevel(key1);
            return dict2.Select(x => x.Value);
        }

        public IEnumerator<DoubleKeyDictionaryItem<TKey1, TKey2, TValue>> GetEnumerator()
        {
            return this._items.GetEnumerator();
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this._items.GetEnumerator();
        }
    }
}
