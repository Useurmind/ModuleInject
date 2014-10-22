using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Container
{
    using ModuleInject.Container.Dependencies;
    using ModuleInject.Container.Resolving;

    public class ConstructorDependencyInjection
    {
        private IList<IResolvedValue> parameters;

        public ConstructorDependencyInjection()
        {
            parameters = new List<IResolvedValue>();
        }

        public void AddParameter(IResolvedValue resolvedParameter)
        {
            parameters.Add(resolvedParameter);
        }

        public object Resolve(Type actualType)
        {
            object[] parameterValues = new object[parameters.Count];

            for (int index = 0; index < parameters.Count; index++)
            {
                var resolvedParameter = parameters[index];
                parameterValues[index] = resolvedParameter.Resolve();
            }

            return Activator.CreateInstance(actualType, parameterValues);
        }
    }
}
