using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Container.DependencyContainerTest
{
    public class TypeRegistration : IContainerRegistration
    {
        public Type ActualType { get; set; }
    }
}
