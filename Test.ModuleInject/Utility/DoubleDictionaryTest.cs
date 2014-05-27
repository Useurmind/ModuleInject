using ModuleInject.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Utility
{
    [TestFixture]
    public class DoubleDictionaryTest
    {
        private DoubleKeyDictionary<int, string, int> mDictionary;

        [SetUp]
        public void Init()
        {
            mDictionary = new DoubleKeyDictionary<int, string, int>();
        }

        [TestCase]
        public void Add_SomeCombination_TryGetValueReturnsCombination()
        {
            int key1 = 2;
            string key2= "sdf";
            int value = 6;

            int resultValue = 0;
            bool found = false;

            mDictionary.Add(key1, key2, value);
            found = mDictionary.TryGetValue(key1, key2, out resultValue);

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

            mDictionary.Add(key11, key21, value1);
            mDictionary.Add(key12, key22, value2);

            foreach (var dictItem in mDictionary)
            {
                Assert.IsTrue(dictItem.Key1 == key11 && dictItem.Key2 == key21 && dictItem.Value == value1
                           || dictItem.Key1 == key12 && dictItem.Key2 == key22 && dictItem.Value == value2);
            }
        }
    }
}
