using System.Linq;

namespace ModuleInject.Container.InstanceCreation
{
    using System;
    using System.Collections.Generic;

    using ModuleInject.Container.Interface;
    using ModuleInject.Container.Resolving;

    public class ConstructorDependencyInjection : IInstanceCreation
    {
        private IList<IResolvedValue> parameters;

        public ConstructorDependencyInjection()
        {
            this.parameters = new List<IResolvedValue>();
        }

        public void AddParameter(IResolvedValue resolvedParameter)
        {
            this.parameters.Add(resolvedParameter);
        }

        public object Resolve(Type actualType)
        {
            object[] parameterValues = new object[this.parameters.Count];

            for (int index = 0; index < this.parameters.Count; index++)
            {
                var resolvedParameter = this.parameters[index];
                parameterValues[index] = resolvedParameter.Resolve();
            }

            return Activator.CreateInstance(actualType, parameterValues);
        }
    }
}
