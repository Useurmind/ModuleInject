using System.Linq;

namespace Test.ModuleInject.Common.Utility
{
    using global::ModuleInject.Common.Utility;

    using NUnit.Framework;

    [TestFixture]
    public class DoubleDictionaryTest
    {
        private DoubleKeyDictionary<int, string, int> mDictionary;

        [SetUp]
        public void Init()
        {
            this.mDictionary = new DoubleKeyDictionary<int, string, int>();
        }

        [TestCase]
        public void Add_SomeCombination_TryGetValueReturnsCombination()
        {
            int key1 = 2;
            string key2= "sdf";
            int value = 6;

            int resultValue = 0;
            bool found = false;

            this.mDictionary.Add(key1, key2, value);
            found = this.mDictionary.TryGetValue(key1, key2, out resultValue);

            Assert.IsTrue(found);
            Assert.AreEqual(value, resultValue);
        }

        [TestCase]
        public void Foreach_SomeElements_AllElementsFound()
        {
            int key11 = 1;
            string key21 = "dsg";
            int value1 = 34;

            int key12 = 45;
            string key22 = "sdf";
            int value2 = 98;

            this.mDictionary.Add(key11, key21, value1);
            this.mDictionary.Add(key12, key22, value2);

            foreach (var dictItem in this.mDictionary)
            {
                Assert.IsTrue(dictItem.Key1 == key11 && dictItem.Key2 == key21 && dictItem.Value == value1
                           || dictItem.Key1 == key12 && dictItem.Key2 == key22 && dictItem.Value == value2);
            }
        }

        [TestCase]
        public void GetAll_SomeElements_ReturnsAllElements()
        {
            int value1 = 45;
            int value2 = 56;
            int value3 = 87;

            this.mDictionary.Add(34, "skdlfj", value1);
            this.mDictionary.Add(34, "sdfg", value2);
            this.mDictionary.Add(56, "hfgh", value3);

            var values = mDictionary.GetAll();

            Assert.AreEqual(3, values.Count());
            Assert.IsTrue(values.Contains(value1));
            Assert.IsTrue(values.Contains(value2));
            Assert.IsTrue(values.Contains(value3));
        }
    }
}