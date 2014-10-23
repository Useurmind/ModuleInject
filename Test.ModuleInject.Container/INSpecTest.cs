using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Container
{
    public interface INSpecTest<TSpec>
        where TSpec : INSpec
    {
        void Check(TSpec spec);
    }
}
