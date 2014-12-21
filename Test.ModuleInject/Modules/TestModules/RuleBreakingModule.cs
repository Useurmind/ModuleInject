using System.Linq;

using ModuleInject.Interfaces;
using ModuleInject.Modules;

namespace Test.ModuleInject.Modules.TestModules
{
    public interface IRuleBreakingModule : IModule
    {

    }

    public class RuleBreakingModule : InjectionModule<IRuleBreakingModule, RuleBreakingModule>, IRuleBreakingModule
    {
    }
}
