using ModuleInject.Fluent;
using ModuleInject.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Fluent
{
    [TestFixture]
    public class PostResolveAssemblerTest
    {
        private PostResolveAssembler<TComponent, IModule, Module> _assembler;
        private TComponent _component;
        private Module _module;

        private interface IComponent { }
        private class TComponent
        {
            public bool Assembled { get; set; }
            public IModule Module { get; set; }
        }
        private interface IModule : IInjectionModule
        {
            IComponent Component { get; set; }
        }
        private class Module : IModule
        {
            public IComponent Component { get; set; }

            public bool IsResolved
            {
                get { throw new NotImplementedException(); }
            }

            public void Resolve()
            {
                throw new NotImplementedException();
            }

            public object GetComponent(Type componentType, string componentName)
            {
                throw new NotImplementedException();
            }
            
            public IComponent GetComponent<IComponent>(string componentName)
            {
                throw new NotImplementedException();
            }
        }

        [SetUp]
        public void Init()
        {
            _component = new TComponent() { Assembled = false };
            _module = new Module();
            _assembler = new PostResolveAssembler<TComponent, IModule, Module>((c, m) => {
                c.Assembled = true;
                c.Module = m;
            });
        }

        [TestCase]
        public void AssembleTyped_ValidComponent_ComponentAssembled()
        {
            _assembler.Assemble(_component, _module);

            Assert.IsTrue(_component.Assembled);
            Assert.AreSame(_module, _component.Module);
        }

        [TestCase]
        public void AssembleUntyped_ValidComponent_ComponentAssembled()
        {
            _assembler.Assemble((object)_component, (IInjectionModule)_module);

            Assert.IsTrue(_component.Assembled);
            Assert.AreSame(_module, _component.Module);
        }
    }
}
