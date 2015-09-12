using System.Linq;

namespace Test.ModuleInject.Modules.TestModules
{
    public interface ISubComponent2
    {
        int IntProperty { get; set; }
    }

    public class SubComponent2 : ISubComponent2
    {
        public int IntProperty { get; set; }
    }
}
