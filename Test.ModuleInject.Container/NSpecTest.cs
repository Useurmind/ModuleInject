using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Container
{
    public abstract class NSpecTest<TSpec>
        where TSpec : INSpec
    {
        public abstract void Check(TSpec spec);
    }
}
