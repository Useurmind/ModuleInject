using ModuleInject.Interfaces.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ModuleInject.Common.Exceptions;

namespace ModuleInject.Injection
{
    public class StartContext<TModule, TIComponent> :
        IInstantiationStrategyContext<TModule, TIComponent>,
        IDisposeStrategyContext<TModule, TIComponent>
           where TModule : class, IInjectionModule
    {
        private TModule module;

        public IInjectionRegister Register { get; private set; }

        public StartContext(TModule module, IInjectionRegister injectionRegister)
        {
            this.module = module;
            this.Register = injectionRegister;
            Register.CheckTypes<TModule, TIComponent>();
        }

        public IDisposeStrategyContext<TModule, TIComponent> InstantiateWith(IInstantiationStrategy<TIComponent> instantiationStrategy)
        {
            this.Register.InstantiationStrategy(instantiationStrategy);
            return this;
        }

        public IConstructionContext<TModule, TIComponent> DisposeWith(IDisposeStrategy disposeStrategy)
        {
            this.Register.DisposeStrategy(disposeStrategy);
            return this;
        }

        public ISourceOf<TModule, TIComponent, TComponent> Construct<TComponent>()
            where TComponent : TIComponent, new()
        {
            return this.Construct<TComponent>(m => new TComponent());
        }

        public ISourceOf<TModule, TIComponent, TComponent> Construct<TComponent>(Func<TModule, TComponent> constructInstance)
            where TComponent : TIComponent
        {
            var typedInjectionRegister = new InjectionRegister<TModule, TIComponent, TComponent>(this.Register);
            var source = new SourceOf<TModule, TIComponent, TComponent>(typedInjectionRegister);

            typedInjectionRegister.Construct(constructInstance);

            CheckDisposeStrategyForDisposableType<TComponent>();

            module.RegisterInjectionRegister(Register);

            return source;
        }

        private void CheckDisposeStrategyForDisposableType<TComponent>() where TComponent : TIComponent
        {
            bool hasDisposeStrategy = Register.GetDisposeStrategy() != null;
            if (hasDisposeStrategy)
            {
                return;
            }

            bool isDisposable = typeof(TComponent).GetInterface("IDisposable") != null;
            if(!isDisposable)
            {
                return;
            } 

            if (!TryApplyDefaultDisposeStrategy())
            {
                ExceptionHelper.ThrowFormatException(Errors.Construction_NoDisposeStrategySetForDisposableType,
                    Register.ComponentName,
                    Register.ComponentType.Name,
                    Register.ContextType.Name);
            }
        }

        private bool TryApplyDefaultDisposeStrategy()
        {            
            var instantiationStrategy = Register.GetInstantiationStrategy();
            if (instantiationStrategy is SingleInstanceInstantiationStrategy<TIComponent>)
            {
                Register.DisposeStrategy(new RememberAndDisposeStrategy());
                return true;
            }
            else if (instantiationStrategy is FactoryInstantiationStrategy<TIComponent>)
            {
                Register.DisposeStrategy(new FireAndForgetStrategy());
                return true;
            }

            return false;
        }
    }
}
