using ModuleInject.Container;
using ModuleInject.Container.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec;

namespace Test.ModuleInject.Container.DependencyContainerTest
{
    class and_defining_prerequisites : when_using_the_DependencyContainer
    {
        private string t1Name;
        private Type t1Type;

        private string t2Name;
        private Type t2Type;

        private IList<ComponentResolvedEventArgs> resolutionsOrdered;

        void before_each()
        {
            t1Name = "t1";
            t1Type = typeof(ITestClass);

            t2Name = "t2";
            t2Type = typeof(ITestClass);

            resolutionsOrdered = new List<ComponentResolvedEventArgs>();

            this.Container.Register(t2Name, t2Type, typeof(TestClass));
            this.Container.Register(t1Name, t1Type, typeof(TestClass));

            this.Container.DefinePrerequisite(t1Name, t1Type, new ContainerReference(this.Container, t2Name, t2Type));

            this.Container.ComponentResolved += (s, a) => resolutionsOrdered.Add(a);

            this.Container.Resolve(t1Name, t1Type);
        }

        void and_resolving_the_registration_with_prerequisite()
        {
            It["the prerequisite is resolved first"] = () => {
                this.resolutionsOrdered[0].Type.should_be(t2Type);
                this.resolutionsOrdered[0].Name.should_be(t2Name);
            };
            It["the resolved registration is resolved after the prerequisite"] = () =>
            {
                this.resolutionsOrdered[1].Type.should_be(t1Type);
                this.resolutionsOrdered[1].Name.should_be(t1Name);
            };
        }
    }
}
