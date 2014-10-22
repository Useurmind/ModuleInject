using System.Linq;

namespace ModuleInject.Container.Resolving
{
    public interface IResolvedValue
    {
        object Resolve();
    }
}
