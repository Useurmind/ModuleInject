using ModuleInject;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    using global::ModuleInject.Interfaces;

    public interface IRuleBreakingModule : IInjectionModule
    {

    }

    public class RuleBreakingModule : InjectionModule<IRuleBreakingModule, RuleBreakingModule>, IRuleBreakingModule
    {
    }
}
