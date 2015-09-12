namespace ModuleInject.Common.Utility
{
    public class DoubleKeyDictionaryItem<TKey1, TKey2, TValue>
    {
        public TKey1 Key1 { get; private set; }
        public TKey2 Key2 { get; private set; }
        public TValue Value { get; private set; }

        public DoubleKeyDictionaryItem(TKey1 key1, TKey2 key2, TValue value)
        {
            this.Key1 = key1;
            this.Key2 = key2;
            this.Value = value;
        }
    }
}