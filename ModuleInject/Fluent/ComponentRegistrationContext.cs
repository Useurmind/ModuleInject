using Microsoft.Practices.Unity;
using Unity = Microsoft.Practices.Unity.InterceptionExtension;
using ModuleInject.Interception;
using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Utility;
using System.Diagnostics.CodeAnalysis;

namespace ModuleInject.Fluent
{
    public class ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
        where TModule : IModule        
        where TComponent : IComponent
        where IModule : IInjectionModule
    {
        private bool _isInterceptorAlreadyAdded;
        private bool _isInterceptionActive;

        public string ComponentName { get; private set; }
        internal IUnityContainer Container { get; private set; }

        public ComponentRegistrationContext(string name, IUnityContainer container, bool interceptionActive)
        {
            _isInterceptionActive = interceptionActive;
            _isInterceptorAlreadyAdded = false;
            ComponentName = name;
            Container = container;
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification="This API is by design statically typed")]
        public ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> AddBehaviour<TBehaviour>()
            where TBehaviour : ISimpleBehaviour, new()
        {
            if (!_isInterceptionActive)
            {
                CommonFunctions.ThrowTypeException<TModule>(Errors.ComponentRegistrationContext_InterceptionNotActivated);
            }

            Unity.InterceptionBehavior unityBehaviour = new Unity.InterceptionBehavior<SimpleUnityBehaviour<TBehaviour>>();

            if(this._isInterceptorAlreadyAdded) {
                this.Container.RegisterType<IComponent, TComponent>(this.ComponentName,
                    unityBehaviour
                    );
            }else {
                this.Container.RegisterType<IComponent, TComponent>(this.ComponentName,
                    new Unity.Interceptor<Unity.InterfaceInterceptor>(),
                    unityBehaviour
                    );
                this._isInterceptorAlreadyAdded = true;
            }

            return this;
        }
    }
}
