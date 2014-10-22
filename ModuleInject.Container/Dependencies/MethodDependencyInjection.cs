using System.Linq;

namespace ModuleInject.Container.Dependencies
{
    using System.Collections.Generic;

    using ModuleInject.Container.Resolving;

    public class MethodDependencyInjection : IDependencyInjection
    {
        private IList<IResolvedValue> methodParameters; 

        public string MethodName { get; set; }

        public MethodDependencyInjection()
        {
            this.methodParameters = new List<IResolvedValue>();
        }

        public void AddParameterValue(IResolvedValue resolvedValue)
        {
            this.methodParameters.Add(resolvedValue);
        }

        public void Resolve(object instance)
        {
            var methodInfo = instance.GetType().GetMethod(this.MethodName);

            object[] parameters = new object[this.methodParameters.Count];
            for (int index = 0; index < this.methodParameters.Count; index++)
            {
                var resolvedParameter = this.methodParameters[index];
                parameters[index] = resolvedParameter.Resolve();
            }

            methodInfo.Invoke(instance, parameters);
        }
    }
}
