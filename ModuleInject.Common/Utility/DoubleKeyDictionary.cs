using System.Linq;

namespace ModuleInject.Common.Utility
{
    using System.Collections;
    using System.Collections.Generic;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldHaveCorrectSuffix"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "This is really a dictionary.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "This is really a dictionary.")]
    public class DoubleKeyDictionary<TKey1, TKey2, TValue> : IEnumerable<DoubleKeyDictionaryItem<TKey1, TKey2, TValue>>
    {
        private Dictionary<TKey1, Dictionary<TKey2, TValue>> _dictionary;
        private IList<DoubleKeyDictionaryItem<TKey1, TKey2, TValue>> _items;

        public int Count { get { return this._items.Count; } }

        public DoubleKeyDictionary()
        {
            this._dictionary = new Dictionary<TKey1, Dictionary<TKey2, TValue>>();
            this._items = new List<DoubleKeyDictionaryItem<TKey1, TKey2, TValue>>();
        }

        public void Add(TKey1 key1, TKey2 key2, TValue value)
        {
            var secondLevel = this.GetSecondLevel(key1, true);

            secondLevel.Add(key2, value);

            this._items.Add(new DoubleKeyDictionaryItem<TKey1, TKey2, TValue>(key1, key2, value));
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
