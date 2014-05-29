using ModuleInject;
using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    public interface IRuleBreakingModule : IInjectionModule
    {

    }

    public class RuleBreakingModule : InjectionModule<IRuleBreakingModule, RuleBreakingModule>, IRuleBreakingModule
    {
    }
}
