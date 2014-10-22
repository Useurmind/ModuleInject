using System.Linq;

namespace ModuleInject.Container.Resolving
{
    public class ConstantValue : IResolvedValue
    {
        private object value;

        public ConstantValue(object value)
        {
            this.value = value;
        }

        public object Resolve()
        {
            return this.value;
        }
    }
}
