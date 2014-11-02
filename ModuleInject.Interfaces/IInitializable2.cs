using System.Linq;

namespace ModuleInject.Interfaces
{
    public interface IInitializable<TArgument1, TArgument2>
    {
        void Initialize(TArgument1 dependency1, TArgument2 dependency2);
    }
}
