using System.Linq;

namespace ModuleInject.Interfaces
{
    public interface IInitializable<TArgument>
    {
        void Initialize(TArgument dependency1);
    }
}
