using System.Linq;

namespace ModuleInject.Interfaces
{
    public interface IInitializable<TArgument1, TArgument2, TArgument3>
    {
        void Initialize(TArgument1 dependency1, TArgument2 dependency2, TArgument3 dependency3);
    }
}
