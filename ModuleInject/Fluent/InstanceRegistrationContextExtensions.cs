using Microsoft.Practices.Unity;
using ModuleInject.Interfaces;
using ModuleInject.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ModuleInject.Fluent
{
    public static class InstanceRegistrationContextExtensions
    {
        public static PostInjectionContext<IComponent, TComponent, IModule, TDependency> PostInject<IComponent, TComponent, IModule, TDependency>(
            this InstanceRegistrationContext<IComponent, TComponent, IModule> instance,
            Expression<Func<IModule, TDependency>> dependencySourceExpression
            )
            where TComponent : IComponent
            where IModule : IInjectionModule
        {
            string dependencyName = LinqHelper.GetMemberPath(dependencySourceExpression);

            return new PostInjectionContext<IComponent, TComponent, IModule, TDependency>(instance, dependencyName);
        }

        public static InstanceRegistrationContext<IComponent, TComponent, IModule> PostInitializeWith<IComponent, TComponent, IModule, TDep1>(
           this InstanceRegistrationContext<IComponent, TComponent, IModule> instance,
           Expression<Func<IModule, TDep1>> dependency1SourceExpression)
            where TComponent : IComponent, IInitializable<TDep1>
            where IModule : IInjectionModule
        {
            string propertyPath = LinqHelper.GetMemberPath(dependency1SourceExpression);

            instance.AddAssembler(new PostResolveAssembler<TComponent, IModule>((comp, module) =>
            {
                TDep1 dependency = module.GetComponent<TDep1>(propertyPath);
                comp.Initialize(dependency);
            }));

            return instance;
        }

        public static InstanceRegistrationContext<IComponent, TComponent, IModule> PostInitializeWith<IComponent, TComponent, IModule, TDep1, TDep2>(
           this InstanceRegistrationContext<IComponent, TComponent, IModule> instance,
           Expression<Func<IModule, TDep1>> dependency1SourceExpression,
           Expression<Func<IModule, TDep2>> dependency2SourceExpression)
            where TComponent : IComponent, IInitializable<TDep1, TDep2>
            where IModule : IInjectionModule
        {
            string property1Path = LinqHelper.GetMemberPath(dependency1SourceExpression);
            string property2Path = LinqHelper.GetMemberPath(dependency2SourceExpression);

            instance.AddAssembler(new PostResolveAssembler<TComponent, IModule>((comp, module) =>
            {
                TDep1 dependency1 = module.GetComponent<TDep1>(property1Path);
                TDep2 dependency2 = module.GetComponent<TDep2>(property2Path);
                comp.Initialize(dependency1, dependency2);
            }));

            return instance;
        }

        public static InstanceRegistrationContext<IComponent, TComponent, IModule> PostInitializeWith<IComponent, TComponent, IModule, TDep1, TDep2, TDep3>(
           this InstanceRegistrationContext<IComponent, TComponent, IModule> instance,
           Expression<Func<IModule, TDep1>> dependency1SourceExpression,
           Expression<Func<IModule, TDep2>> dependency2SourceExpression,
            Expression<Func<IModule, TDep3>> dependency3SourceExpression)
            where TComponent : IComponent, IInitializable<TDep1, TDep2, TDep3>
            where IModule : IInjectionModule
        {
            string property1Path = LinqHelper.GetMemberPath(dependency1SourceExpression);
            string property2Path = LinqHelper.GetMemberPath(dependency2SourceExpression);
            string property3Path = LinqHelper.GetMemberPath(dependency3SourceExpression);

            instance.AddAssembler(new PostResolveAssembler<TComponent, IModule>((comp, module) =>
            {
                TDep1 dependency1 = module.GetComponent<TDep1>(property1Path);
                TDep2 dependency2 = module.GetComponent<TDep2>(property2Path);
                TDep3 dependency3 = module.GetComponent<TDep3>(property3Path);
                comp.Initialize(dependency1, dependency2, dependency3);
            }));

            return instance;
        }

        public static InstanceRegistrationContext<IComponent, TComponent, IModule> AddAssembler<IComponent, TComponent, IModule>(
            this InstanceRegistrationContext<IComponent, TComponent, IModule> instance,
            IPostResolveAssembler<TComponent, IModule> assembler
            )
            where TComponent : IComponent
            where IModule : IInjectionModule
        {
            instance.AddAssembler(assembler);

            return instance;
        }
    }
}
