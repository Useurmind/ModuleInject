using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Unity = Microsoft.Practices.Unity.InterceptionExtension;

namespace ModuleInject.Interception
{
    public class ParameterCollection : IParameterCollection
    {
        internal Unity.IParameterCollection UnityParameters { get; private set; }

        public ParameterCollection(Unity.IParameterCollection parameterCollection)
        {
            UnityParameters = parameterCollection;
        }

        public object this[string parameterName]
        {
            get
            {
                return UnityParameters[parameterName];
            }
            set
            {
                UnityParameters[parameterName] = value;
            }
        }

        public bool ContainsParameter(string parameterName)
        {
            return UnityParameters.ContainsParameter(parameterName);
        }

        public ParameterInfo GetParameterInfo(int index)
        {
            return UnityParameters.GetParameterInfo(index);
        }

        public ParameterInfo GetParameterInfo(string parameterName)
        {
            return UnityParameters.GetParameterInfo(parameterName);
        }

        public string ParameterName(int index)
        {
            return UnityParameters.ParameterName(index);
        }

        public int Add(object value)
        {
            return UnityParameters.Add(value);
        }

        public void Clear()
        {
            UnityParameters.Clear();
        }

        public bool Contains(object value)
        {
            return UnityParameters.Contains(value);
        }

        public int IndexOf(object value)
        {
            return UnityParameters.IndexOf(value);
        }

        public void Insert(int index, object value)
        {
            UnityParameters.Insert(index, value);
        }

        public bool IsFixedSize
        {
            get { return UnityParameters.IsFixedSize; }
        }

        public bool IsReadOnly
        {
            get { return UnityParameters.IsReadOnly; }
        }

        public void Remove(object value)
        {
            UnityParameters.Remove(value);
        }

        public void RemoveAt(int index)
        {
            UnityParameters.RemoveAt(index);
        }

        public object this[int index]
        {
            get
            {
                return UnityParameters[index];
            }
            set
            {
                UnityParameters[index] = value;
            }
        }

        public void CopyTo(Array array, int index)
        {
            UnityParameters.CopyTo(array, index);
        }

        public int Count
        {
            get { return UnityParameters.Count; }
        }

        public bool IsSynchronized
        {
            get { return UnityParameters.IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return UnityParameters.SyncRoot; }
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return UnityParameters.GetEnumerator();
        }
    }
}
