using System.Linq;

namespace ModuleInject.Container.Interface
{
    public interface IDependencyInjection
    {
        void Resolve(object instance);
    }
}
