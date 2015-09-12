using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Interfaces;
using Test.Performance.ModuleInject.Components;

namespace Test.Performance.ModuleInject
{
    public interface ITestModule : IModule
    {
        ITestComponent1 Component1 { get; }
        ITestComponent1 Component2 { get; }
        ITestComponent1 Component3 { get; }
        ITestComponent1 Component4 { get; }
        ITestComponent1 Component5 { get; }
        ITestComponent1 Component6 { get; }
        ITestComponent1 Component7 { get; }
        ITestComponent1 Component8 { get; }
        ITestComponent1 Component9 { get; }
        ITestComponent1 Component10 { get; }
    }
}
