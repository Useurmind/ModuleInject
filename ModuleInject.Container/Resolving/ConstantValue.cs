using System.Linq;

namespace ModuleInject.Container.Resolving
{
    using System;

    using ModuleInject.Container.Interface;

    public class ConstantValue : IResolvedValue
    {
        private object value;

        public Type Type { get; set; }

        public ConstantValue(object value, Type type)
        {
            this.value = value;
            this.Type = type;
        }

        public object Resolve()
        {
            return this.value;
        }
    }

    public class ConstantValue<T> : ConstantValue
    {
        public ConstantValue(T value):base(value, typeof(T))
        {
            
        }   
    }
}
