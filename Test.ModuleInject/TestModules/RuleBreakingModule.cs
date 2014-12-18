using ModuleInject;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ModuleInject.Modules;

namespace Test.ModuleInject.TestModules
{
    using global::ModuleInject.Interfaces;

    public interface IRuleBreakingModule : IModule
    {

    }

    public class RuleBreakingModule : InjectionModule<IRuleBreakingModule, RuleBreakingModule>, IRuleBreakingModule
    {
    }
}
