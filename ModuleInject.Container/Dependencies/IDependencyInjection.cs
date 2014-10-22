using System.Linq;

namespace ModuleInject.Container.Dependencies
{
    public interface IDependencyInjection
    {
        void Resolve(object instance);
    }
}
