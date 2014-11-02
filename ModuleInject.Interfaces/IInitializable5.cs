using System.Linq;

namespace ModuleInject.Interfaces
{
    public interface IInitializable<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>
    {
        void Initialize(TArgument1 dependency1, TArgument2 dependency2, TArgument3 dependency3, 
            TArgument4 dependency4, TArgument5 dependency5);
    }
}
