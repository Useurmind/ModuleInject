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
        public static PostInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>
            PostInject<IComponent, TComponent, IModule, TModule, TDependency>(
            this InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance,
            Expression<Func<TModule, TDependency>> dependencySourceExpression
            )
            where TComponent : IComponent
            where TModule : IModule
            where IModule : IInjectionModule
        {
            string dependencyName = LinqHelper.GetMemberPath(dependencySourceExpression);

            return new PostInjectionContext<IComponent, TComponent, IModule, TModule, TDependency>(instance, dependencyName);
        }

        public static InstanceRegistrationContext<IComponent, TComponent, IModule, TModule>
            PostInitializeWith<IComponent, TComponent, IModule, TModule, TDep1>(
           this InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance,
           Expression<Func<TModule, TDep1>> dependency1SourceExpression)
            where TComponent : IComponent, IInitializable<TDep1>
            where TModule : IModule
            where IModule : IInjectionModule
        {
            string memberPath = LinqHelper.GetMemberPath(dependency1SourceExpression);

            instance.AddAssembler(new PostResolveAssembler<TComponent, IModule, TModule>((comp, module) =>
            {
                TDep1 dependency = module.GetComponent<TDep1>(memberPath);
                comp.Initialize(dependency);
            }));

            return instance;
        }

        public static InstanceRegistrationContext<IComponent, TComponent, IModule, TModule>
            PostInitializeWith<IComponent, TComponent, IModule, TModule, TDep1, TDep2>(
           this InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance,
           Expression<Func<TModule, TDep1>> dependency1SourceExpression,
           Expression<Func<TModule, TDep2>> dependency2SourceExpression)
            where TComponent : IComponent, IInitializable<TDep1, TDep2>
            where TModule : IModule
            where IModule : IInjectionModule
        {
            string memberPath1 = LinqHelper.GetMemberPath(dependency1SourceExpression);
            string memberPath2 = LinqHelper.GetMemberPath(dependency2SourceExpression);

            instance.AddAssembler(new PostResolveAssembler<TComponent, IModule, TModule>((comp, module) =>
            {
                TDep1 dependency1 = module.GetComponent<TDep1>(memberPath1);
                TDep2 dependency2 = module.GetComponent<TDep2>(memberPath2);
                comp.Initialize(dependency1, dependency2);
            }));

            return instance;
        }

        public static InstanceRegistrationContext<IComponent, TComponent, IModule, TModule>
            PostInitializeWith<IComponent, TComponent, IModule, TModule, TDep1, TDep2, TDep3>(
           this InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance,
           Expression<Func<TModule, TDep1>> dependency1SourceExpression,
           Expression<Func<TModule, TDep2>> dependency2SourceExpression,
            Expression<Func<TModule, TDep3>> dependency3SourceExpression)
            where TComponent : IComponent, IInitializable<TDep1, TDep2, TDep3>
            where TModule : IModule
            where IModule : IInjectionModule
        {
            string memberPath1 = LinqHelper.GetMemberPath(dependency1SourceExpression);
            string memberPath2 = LinqHelper.GetMemberPath(dependency2SourceExpression);
            string memberPath3 = LinqHelper.GetMemberPath(dependency3SourceExpression);

            instance.AddAssembler(new PostResolveAssembler<TComponent, IModule, TModule>((comp, module) =>
            {
                TDep1 dependency1 = module.GetComponent<TDep1>(memberPath1);
                TDep2 dependency2 = module.GetComponent<TDep2>(memberPath2);
                TDep3 dependency3 = module.GetComponent<TDep3>(memberPath3);
                comp.Initialize(dependency1, dependency2, dependency3);
            }));

            return instance;
        }

        public static InstanceRegistrationContext<IComponent, TComponent, IModule, TModule>
            AddAssembler<IComponent, TComponent, IModule, TModule>(
            this InstanceRegistrationContext<IComponent, TComponent, IModule, TModule> instance,
            IPostResolveAssembler<TComponent, IModule, TModule> assembler
            )
            where TComponent : IComponent
            where TModule: IModule
            where IModule : IInjectionModule
        {
            instance.AddAssembler(assembler);

            return instance;
        }
    }
}
